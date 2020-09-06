using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PickupAnnouncerLegacy.Helpers;
using PickupAnnouncerLegacy.Hubs;
using PickupAnnouncerLegacy.Interfaces;
using PickupAnnouncerLegacy.Mappings;
using PickupAnnouncerLegacy.Services;

namespace PickupAnnouncerLegacy
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
            services.AddTransient<IDbHelper, DbHelper>();
            services.AddTransient<IDbService, DbService>(builder => new DbService(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IStudentHelper, StudentHelper>();
            services.AddTransient<IMapper, Mapper>(builder =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new BaseProfile());
                });
                return new Mapper(config);
            });
            services.AddTransient<IRegistrationFileHelper, RegistrationFileHelper>();


            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizePage("/Admin");
                })
                .AddNToastNotifyToastr()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseNToastNotify();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
            app.UseSignalR(route =>
            {
                route.MapHub<PickupHub>("/pickup");
            });
        }
    }
}
