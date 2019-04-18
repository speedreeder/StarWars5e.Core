using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Api.Interfaces;
using StarWars5e.Api.Managers;
using Swashbuckle.AspNetCore.Swagger;
using Wolnik.Azure.TableStorage.Repository;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "StarWars5e.Api", Version = "v1" });
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
            services.AddSingleton<ITableStorage>(tableStorage);
            services.AddSingleton<IEquipmentManager, EquipmentManager>();
            services.AddSingleton<IChapterRuleManager, ChapterRuleManager>();

            var cloudBlobClient = CloudStorageAccount.Parse(Configuration["StorageAccountConnectionString"])
                .CreateCloudBlobClient();
            services.AddSingleton(cloudBlobClient);

            //services.AddAuthentication()
            //    .AddGoogle(googleOptions =>
            //    {
            //        googleOptions.ClientId = Configuration["GoogleOAuthClientId"];
            //        googleOptions.ClientSecret = Configuration["GoogleOAuthSecret"];
            //    });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseMvc();
            //app.UseAuthentication();
        }
    }
}
