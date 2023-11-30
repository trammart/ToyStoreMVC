using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToyStoreMVC.DataAccess.Data;
using ToyStoreMVC.DataAccess.Repository;
using ToyStoreMVC.DataAccess.Repository.IRepository;
using ToyStoreMVC.Utility;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToyStoreMVC.Models;

namespace ToyStoreMVC
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

        // Phương thức này được gọi bởi bộ thực thi. Sử dụng phương pháp này để thêm dịch vụ vào vùng chứa.
        public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDatabaseDeveloperPageExceptionFilter();

			
			services.AddIdentity<ApplicationUser, IdentityRole>().AddDefaultTokenProviders()
		.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddOptions(); // Kích hoạt Options
			var mailsettings = Configuration.GetSection("EmailOptions"); // ??c config
			services.Configure<EmailOptions>(mailsettings); // ??ng ký ?? Inject

			services.AddTransient<IEmailSender, SendMailService>(); // ??ng ký d?ch v? Mail

			//pay stripe
			services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

			//(me) add UnitOfWork
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddControllersWithViews().AddRazorRuntimeCompilation();
			services.AddRazorPages();

			//add
			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = $"/Identity/Account/Login";
				options.LogoutPath = $"/Identity/Account/Logout";
				options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
			});

			//add login facebook
			services.AddAuthentication().AddFacebook(options =>
			{
				options.AppId = "1046339409714580";
				options.AppSecret = "450825a1d2c31719fdd488399de73ca0";
				options.CallbackPath = "/facebook-login";

			});

			//add login google
			services.AddAuthentication().AddGoogle(options =>
			{
				options.ClientId = "604567945882-4q3c7sjp07fjt81io9q20r49lrs0ei9q.apps.googleusercontent.com";
				options.ClientSecret = "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";
				options.CallbackPath = "/google-login";
			});

			services.Configure<IdentityOptions>(options =>
			{

				// Cấu hình Lockout - khóa user
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
				options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lần thì khóa
				options.Lockout.AllowedForNewUsers = true;

				// Cấu hình về User.
				options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
					"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
				options.User.RequireUniqueEmail = true; // Email là duy nhất

			});

			// session
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(30);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

		}

        // Phương thức này được gọi bởi bộ thực thi. Sử dụng phương pháp này để định cấu hình đường dẫn yêu cầu HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			//session
			app.UseSession();

			//stripe
			StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					//pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
					pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});
		}
	}
}
