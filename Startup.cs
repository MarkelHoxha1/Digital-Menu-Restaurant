using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalMenuRestaurant.Helper;
using DigitalMenuRestaurant.Models;
using DigitalMenuRestaurant.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace DigitalMenuRestaurant
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // requires using Microsoft.Extensions.Options
            services.Configure<DigitalMenuDatabaseSettings>(
                Configuration.GetSection(nameof(DigitalMenuDatabaseSettings)));

            services.AddSingleton<IDigitalMenuDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DigitalMenuDatabaseSettings>>().Value);
            services.AddScoped<ValidateModelAttribute>();
            services.AddSingleton<DishService>();

            //Allowing connection with a simple VueJS Application
            services.AddCors(options =>
           {
               options.AddPolicy("CorsPolicy", builder =>
                  builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });


            services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = ".NET Core API, MongoDB Digital Menu Restaurant",
                    Description = "Swagger surface",
                    Contact = new OpenApiContact
                    {
                        Name = "Markel Hoxha",
                        Email = "markelhoxha5@gmail.com",
                        Url = new Uri("https://github.com/MarkelHoxha1/Digital-Menu-Restaurant")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://github.com/MarkelHoxha1/Digital-Menu-Restaurant/blob/master/LICENSE")
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Digital Menu Restaurant v1.0");
            });
        }
    }
}
