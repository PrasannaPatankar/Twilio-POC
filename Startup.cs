using IEvangelist.VideoChat.Abstractions;
using IEvangelist.VideoChat.Hubs;
using IEvangelist.VideoChat.Options;
using IEvangelist.VideoChat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace IEvangelist.VideoChat
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.Configure<TwilioSettings>(settings =>
                    {
                        settings.AccountSid = "ACf7af67aadfe207eb4cf6cc40a59fc5ed";
                        settings.ApiSecret = "AC339ca3ab1e2c65f400232bbcf3e5f633";
                        settings.ApiKey = "fbdc47741b382ce487043ae7cbe9c8af";
                    })
                    .AddTransient<IVideoService, VideoService>()
                    .AddSpaStaticFiles(config => config.RootPath = "ClientApp/dist/ClientApp");

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection()
               .UseStaticFiles()
               .UseSpaStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller}/{action=Index}/{id?}");

                    endpoints.MapHub<NotificationHub>("/notificationHub");
                })
               .UseSpa(spa =>
                {
                    // To learn more about options for serving an Angular SPA from ASP.NET Core,
                    // see https://go.microsoft.com/fwlink/?linkid=864501
                    spa.Options.SourcePath = "ClientApp";

                    if (env.IsDevelopment())
                    {
                        spa.UseAngularCliServer(npmScript: "start");
                    }
                });
        }
    }
}