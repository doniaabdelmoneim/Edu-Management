using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using projectX.Models;
using projectX.Repositories;

namespace projectX
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //! Dependency Injection Container  (DIC)
            //* Registering the services  in the Dependency Injection Container
            builder.Services.AddScoped<IStudentRepo, StudentRepo> ();
            builder.Services.AddScoped<IDepartmentRepo, DepartmentRepo> ();
            //* Registering the services  in the Dependency Injection Container
            builder.Services.AddDbContext<entitiesDbContext> (options => options.UseSqlServer (builder.Configuration.GetConnectionString ("DefaultConnection")));

            //* Registering the services  in the Dependency Injection Container 
            // to use session
            builder.Services.AddSession (s=>
            {
             s.Cookie.Name = ".projectX.Session";
             s.IdleTimeout = TimeSpan.FromMinutes (30);
             s.Cookie.HttpOnly = true;
             s.Cookie.IsEssential = true;
             });
            // to use authentication
            //builder.Services.AddAuthentication ("MyCookieAuth").AddCookie ("MyCookieAuth", config =>
            //{
            //    config.Cookie.Name = "projectX.Cookie";
            //    config.LoginPath = "/Account/Login";
            //    config.LogoutPath = "/Account/Logout";
            //    config.ExpireTimeSpan = TimeSpan.FromMinutes (30);
            //    config.SlidingExpiration = true;
            //});
            builder.Services.AddAuthentication (CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie (options =>
                {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.ExpireTimeSpan = TimeSpan.FromMinutes (30);
            options.SlidingExpiration = true;
            options.Cookie.Name = "projectX.Cookie";
       
                 });

            var app = builder.Build();

            //Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment ())
            {
                app.UseExceptionHandler ("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }

            app.UseHttpsRedirection ();
            app.UseStaticFiles ();  // wwwroot   

            app.UseRouting (); //  name of controller , name  of action
            app.UseAuthentication (); // get user data from reguest (cookies), user:userprincipal
            app.UseAuthorization ();   // to use authorization
            app.UseSession (); // to use session in the application

            app.MapControllerRoute (
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // CREATE MIDDLEWARE 
            //===> app.run
            //app.Run (
            //    //make short circuit for the request to the middleware 
            //    // Request Delegate
            //    async (context) =>
            //    {
            //        await context.Response.WriteAsync ("Hello World!");
            //    }
            //    );

            ////==> app.use
            //app.Use (async(context,next)=>{
            //    await context.Response.WriteAsync ("Hello World!");
            //   await next.Invoke ();
            //    await context.Response.WriteAsync ("GoodBye");
            //});


            app.Run();
        }
    }
}
