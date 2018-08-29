using AutoMapper;
using Cuda.Api.Configurations.Systems;
using Cuda.Api.Extensions;
using Cuda.Common.Constants;
using Cuda.Model;
using Cuda.Service.Implementations;
using Cuda.Service.Interfaces;
using Cuda.Service.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cuda.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env, IHostingEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        private readonly IHostingEnvironment _environment;
        private readonly string _assemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString(ConfigurationKeys.DefaultConnection), x => x.MigrationsAssembly(_assemblyName)));

            services.ConfigureIdentityService(Configuration, _environment);
            ConfigIoc(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });
        }

        public void ConfigIoc(IServiceCollection services)
        {
            services.AddScoped<AppInitializer>();
            services.AddScoped<ILoginHistoryService, LoginHistoryService>();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<DtoMappingProfile>();
                cfg.IgnoreUnmapped();
            });
            services.AddSingleton(Mapper.Instance.RegisterMap());
        }
    }
}