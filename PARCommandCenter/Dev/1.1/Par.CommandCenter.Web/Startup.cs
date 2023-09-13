using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Par.CommandCenter.Application;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.AzureMapAPI;
using Par.CommandCenter.AzureServiceBus;
using Par.CommandCenter.CloudDeviceManagement;
using Par.CommandCenter.Identity;
using Par.CommandCenter.Notifications;
using Par.CommandCenter.Persistence;
using Par.CommandCenter.ProductsWebAPI;
using Par.CommandCenter.Web.Services;
using System;

namespace Par.CommandCenter.Web
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
            services.AddControllers();

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddApplication(Configuration);
            services.AddPersistence(Configuration);
            services.AddIdentityService(Configuration);
            services.AddCloudDeviceManagementService(Configuration);
            services.AddAzureMapAPIService(Configuration);
            services.AddAzureServiceBus(Configuration);
            services.AddNotifications(Configuration);

            services.AddProductsWebAPI(Configuration);


            services.AddHttpContextAccessor();

            services.AddAuthorization();

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
            //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IApplicationDbContext>());

            // Register the Swagger generator, defining 1 or more Swagger documents            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Command Center API", Version = "1.0.1" });
                ////c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                ////{
                ////    Name = "Authorization",
                ////    Type = SecuritySchemeType.ApiKey,
                ////    Scheme = "Bearer",
                ////    BearerFormat = "JWT",
                ////    In = ParameterLocation.Header,
                ////    Description = "JWT Authorization header using the Bearer scheme."
                ////});
                ////c.AddSecurityRequirement(new OpenApiSecurityRequirement
                ////{
                ////    {
                ////        new OpenApiSecurityScheme
                ////        {
                ////            Reference = new OpenApiReference
                ////            {
                ////                Type = ReferenceType.SecurityScheme,
                ////                Id = "Bearer"
                ////            }
                ////        },
                ////        new string[] {}

                ////    }
                ////});
            }).AddSwaggerGenNewtonsoftSupport();

            services.AddCors();

            services.AddMvc(options => options.EnableEndpointRouting = false)
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IApplicationDbContext>());


            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            // Add security headers
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
                options.ExcludedHosts.Add("example.com");
                options.ExcludedHosts.Add("www.example.com");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ////// Add security headers
            ////app.Use(async (context, next) =>
            ////{
            ////    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            ////    context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
            ////    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            ////    context.Response.Headers.Add("Referrer-Policy", "same-origin"); //// "no-referrer-when-downgrade");
            ////    context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
            ////    context.Response.Headers.Add("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");                

            ////    context.Response.Headers.Add("Content-Security-Policy", 
            ////        "default-src https:; " +
            ////        "img-src * 'self'  data: https:; " +
            ////        "style-src 'self' 'unsafe-inline'; " +
            ////        "script-src 'self' 'unsafe-inline' 'unsafe-eval'; ");

            ////    // The remove headers should be moved to web.config.
            ////    context.Response.Headers.Remove("X-Powered-By");
            ////    context.Response.Headers.Remove("X-AspNet-Version");
            ////    context.Response.Headers.Remove("Server");

            ////    await next();
            ////});

            if (env.IsDevelopment() || env.IsEnvironment("LocalDevelopment"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // Add security headers
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();


            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            if (!env.IsProduction())
            {
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "/swagger/{documentName}/swagger.json";
                });

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                });
            }

            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            ////app.UseEndpoints(endpoints =>
            ////{
            ////    endpoints.MapControllerRoute(
            ////        name: "default",
            ////        pattern: "{controller}/{action=Index}/{id?}");
            ////});           

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                ////spa.Options.SourcePath = env.ContentRootPath + "\\..\\" + "ClientApp";

                if (env.IsEnvironment("LocalDevelopment"))
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
