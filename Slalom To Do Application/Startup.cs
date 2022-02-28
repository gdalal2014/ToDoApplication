using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Autofac;
using Slalom_To_Do_Application.UoW;
using Slalom_To_Do_Application.Repository;
using Slalom_To_Do_Application.BusinessLayer;
using Autofac.Extensions.DependencyInjection;

namespace Slalom_To_Do_Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddMvc().AddControllersAsServices();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var dbConnectionString = Configuration.GetConnectionString("DatabaseConnectionString").ToString();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().WithParameter(new TypedParameter(typeof(string), dbConnectionString));
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<ToDoRepository>().As<IToDoRepository>();
            builder.RegisterType<UserActionBusinessLayer>().As<IUserActionsBusinessLayer>();
            builder.RegisterType<ToDoActionBusinessLayer>().As<IToDoActionBusinessLayer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                   name: "selectUser",
                   pattern: "{controller=Home}/{action=selectUser}/{id?}");
            });
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
        }
    }
}

