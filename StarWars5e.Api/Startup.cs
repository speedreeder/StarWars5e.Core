using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Search;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Api.Storage;
using Microsoft.Identity.Web;

namespace StarWars5e.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(options =>
                    {
                        Configuration.Bind("AzureAdB2C", options);

                        options.TokenValidationParameters.NameClaimType = "name";
                    },
                    options => { Configuration.Bind("AzureAdB2C", options); });

            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }).AddNewtonsoftJson();

            services.AddAuthorization();


            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.FirstOrDefault());
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StarWars5e.Api", Version = "v1"});

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement();
                securityRequirement.Add(securitySchema, new[] { "Bearer" });
                c.AddSecurityRequirement(securityRequirement);
            });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            var tableStorage = new AzureTableStorage(Configuration["StorageAccountConnectionString"]);
            var cloudStorageAccount = CloudStorageAccount.Parse(Configuration["StorageAccountConnectionString"]);
            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var searchServiceClient = new SearchServiceClient("sw5esearch", new SearchCredentials(Configuration["SearchKey"]));
            var searchIndexClient = searchServiceClient.Indexes.GetClient("searchterms-index");

            services.AddSingleton<IAzureTableStorage>(tableStorage);

            services.Scan(scan => scan
                .FromAssemblies(typeof(Program).GetTypeInfo().Assembly)
                .AddClasses(true)
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
            );

            services.AddSingleton(cloudBlobClient);
            services.AddSingleton(cloudTableClient);
            services.AddSingleton(searchIndexClient);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StarWars5e.Api v1");
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // must come after UseRouting
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
