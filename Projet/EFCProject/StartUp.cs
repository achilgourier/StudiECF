using EFCProject.Data;
using EFCProject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EFCProject
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;

		}

		public IConfiguration Configuration { get; }

		// Cette méthode est appelée lors de l'exécution. Utilisez cette méthode pour ajouter des services à votre conteneur de dépendances.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			services.AddAuthentication(options =>
			{
				options.DefaultScheme = "Cookies";
				options.DefaultSignInScheme = "Cookies";
			})
			.AddCookie("Cookies", options =>
			{
				options.LoginPath = "/Account/Login"; // Spécifiez votre chemin de connexion
				options.AccessDeniedPath = "/Account/AccessDenied"; // Spécifiez votre chemin d'accès refusé
			});
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();
			// Autres services et configurations peuvent être ajoutés ici
		}

		// Cette méthode est appelée lors de l'exécution. Utilisez cette méthode pour configurer le pipeline HTTP.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication(); // Ajoutez cette ligne pour activer l'authentification
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}