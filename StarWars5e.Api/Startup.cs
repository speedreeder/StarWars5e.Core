using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ElCamino.AspNetCore.Identity.AzureTable.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Search;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Api.Auth;
using StarWars5e.Api.Extensions;
using StarWars5e.Api.Helpers;
using Swashbuckle.AspNetCore.Swagger;
using Wolnik.Azure.TableStorage.Repository;
using IdentityRole = ElCamino.AspNetCore.Identity.AzureTable.Model.IdentityRole;

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
            services.Configure<FacebookAuthSettings>(facebookAuthSettings => {
                facebookAuthSettings.AppId = Configuration["FacebookAuthSettings:AppId"];
                facebookAuthSettings.AppSecret = Configuration["FacebookAuthSettings:AppSecret"];
                facebookAuthSettings.RedirectUri = Configuration["FacebookAuthSettings:RedirectUri"];
            });
            services.Configure<GoogleAuthSettings>(googleAuthSettings => {
                googleAuthSettings.ClientId = Configuration["GoogleAuthSettings:ClientId"];
                googleAuthSettings.ClientSecret = Configuration["GoogleAuthSettings:ClientSecret"];
                googleAuthSettings.RedirectUri = Configuration["GoogleAuthSettings:RedirectUri"];
            });
            services.Configure<RedditAuthSettings>(redditAuthSettings => {
                redditAuthSettings.ClientId = Configuration["RedditAuthSettings:ClientId"];
                redditAuthSettings.ClientSecret = Configuration["RedditAuthSettings:ClientSecret"];
                redditAuthSettings.RedirectUri = Configuration["RedditAuthSettings:RedirectUri"];
            });
            services.Configure<DiscordAuthSettings>(discordAuthSettings => {
                discordAuthSettings.ClientId = Configuration["DiscordAuthSettings:ClientId"];
                discordAuthSettings.ClientSecret = Configuration["DiscordAuthSettings:ClientSecret"];
                discordAuthSettings.RedirectUri = Configuration["DiscordAuthSettings:RedirectUri"];
            });

            services.AddSingleton<JwtFactory>();
            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["TokenSigningKey"]));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = "testIssuer";
                options.Audience = "testAudience";
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "testIssuer",

                ValidateAudience = true,
                ValidAudience = "testAudience",

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = "testIssuer";
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
                configureOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            });

            services.AddIdentityCore<AppUser>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<IdentityRole>()
                .AddAzureTableStoresV2<ApplicationDbContext>(() =>
                {
                    var idConfig = new IdentityConfiguration
                    {
                        TablePrefix = Configuration["IdentityAzureTable:IdentityConfiguration:TablePrefix"],
                        StorageConnectionString = Configuration["StorageAccountConnectionString"],
                        //LocationMode = Configuration.GetSection("IdentityAzureTable:IdentityConfiguration:LocationMode")
                        //    .Value
                    };
                    return idConfig;
                })
                .AddDefaultTokenProviders();
                //.CreateAzureTablesIfNotExists<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.FirstOrDefault());
                c.DescribeAllEnumsAsStrings();
                c.SwaggerDoc("v1", new Info {Title = "StarWars5e.Api", Version = "v1"});
            });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
                options.AddPolicy("development", builder =>
                {
                    builder.WithOrigins("http://localhost:8080")
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader();
                });
                options.AddPolicy("production", builder =>
                {
                    builder.WithOrigins("https://sw5e.com", "https://www.sw5e.com")
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader();
                });
            });

            var tableStorage = new AzureTableStorage(Configuration["StorageAccountConnectionString"]);
            var cloudStorageAccount = CloudStorageAccount.Parse(Configuration["StorageAccountConnectionString"]);
            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var searchServiceClient = new SearchServiceClient("sw5esearch", new SearchCredentials(Configuration["SearchKey"]));
            var searchIndexClient = searchServiceClient.Indexes.GetClient("searchterms-index");

            services.AddSingleton<ITableStorage>(tableStorage);

            services.Scan(scan => scan
                .FromAssemblies(typeof(Program).GetTypeInfo().Assembly)
                .AddClasses()
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
            );

            services.AddSingleton(cloudBlobClient);
            services.AddSingleton(cloudTableClient);
            services.AddSingleton(searchIndexClient);

            //services.AddAuthentication()
            //    .AddGoogle(googleOptions =>
            //    {
            //        googleOptions.ClientId = Configuration["GoogleOAuthClientId"];
            //        googleOptions.ClientSecret = Configuration["GoogleOAuthSecret"];
            //    });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            //app.UseMiddleware<OptionsMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseCors("development");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StarWars5e.Api v1");
                });
            }
            else
            {
                app.UseCors("production");
                app.UseHsts();
                
                app.UseHttpsRedirection();
            }

            app.UseExceptionHandler(
                builder =>
                {
                    builder.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                            var error = context.Features.Get<IExceptionHandlerFeature>();
                            if (error != null)
                            {
                                context.Response.AddApplicationError(error.Error.Message);
                                await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                            }
                        });
                });

            
            app.UseAuthentication();
            app.UseMiddleware<JwtInHeaderMiddleware>();
            app.UseMvc();
        }
    }
}
