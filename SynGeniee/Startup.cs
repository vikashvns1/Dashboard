using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radzen;
using Syncfusion.Blazor;
using SynGeniee.Components;

namespace SynGeniee
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSyncfusionBlazor();
            services.AddScoped<DialogService>();
            services.AddScoped<BrowserService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            //services.AddSingleton<IDatabaseNotification, DatabaseNotification>();
            //services.AddSingleton<IToastService, ToastService>();
            services.AddScoped<ToastNotification>();
            services.AddHttpClient<IDynamicAPIService, DynamicAPIService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44320/"); 
                //client.BaseAddress = new Uri("https://genieedashboardapi.azurewebsites.net/"); //https://genieedashboardapi.azurewebsites.net/
            });
            services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
            services.AddSignalRCore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {//verson 18.3.0.48
          Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzQwMDI2QDMxMzgyZTMzMmUzMFRLOUlMMU9XT1hZSHI3RXZPS3ZQTUxYUlFJaVBOdTgyWmd3QUNjdnp4OFU9");
         //version 18.4.0.31
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzc0MTc4QDMxMzgyZTM0MmUzME9zL0RBMVRYanozc2ovNjM4MjJqUU0xaHVRR01wUDdxNEt1Z3c3NUdLb1U9");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.Use((ctx, next) =>
                {
                    ctx.Request.Scheme = "https";
                    return next();
                });
                //app.UseExceptionHandler("/Error");
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseSession();

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                //endpoints.MapHub<NotificationHub>("/NotificationHub");
            });
        }
    }
}
