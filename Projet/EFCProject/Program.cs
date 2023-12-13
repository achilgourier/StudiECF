using EFCProject;
using EFCProject.Data;
using EFCProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)//valide adress mail
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User", "CommManager","Producer" };

    foreach (var role in roles)
    {

        if(!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    string name = "admin";
    string email  = "admin@admin";
    string password = "Admin123!";
    if (await userManager.FindByEmailAsync(email) == null)
    {   
        var user = new ApplicationUser();
        
        user.UserName = name;
        user.Email = email;
        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");
		var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
		await userManager.ConfirmEmailAsync(user, code);
	}
	string nameProducteur = "producteur";
	string emailProducteur = "producteur@producteur.com";
	string passwordProducteur = "Producteur123!";
	if (await userManager.FindByEmailAsync(emailProducteur) == null)
	{
		var user = new ApplicationUser();

		user.UserName = nameProducteur;
		user.Email = emailProducteur;
		await userManager.CreateAsync(user, passwordProducteur);
		await userManager.AddToRoleAsync(user, "Producer");
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await userManager.ConfirmEmailAsync(user, code);

	}

	string nameManager = "manager";
	string emailManager = "manager@manager.com";
	string passwordManager = "Manager123!";

	if (await userManager.FindByEmailAsync(emailManager) == null)
	{
		var user = new ApplicationUser();

		user.UserName = nameManager;
		user.Email = emailManager;
		await userManager.CreateAsync(user, passwordManager);
		await userManager.AddToRoleAsync(user, "CommManager");
		var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
		await userManager.ConfirmEmailAsync(user, code);
	}

	string nameUser = "user";
	string emailUser = "user@user.com";
	string passwordUser = "User123!";

	if (await userManager.FindByEmailAsync(emailUser) == null)
	{
		var user = new ApplicationUser();

		user.UserName = nameUser;
		user.Email = emailUser;
		await userManager.CreateAsync(user, passwordUser);
		await userManager.AddToRoleAsync(user, "User");
		var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
		await userManager.ConfirmEmailAsync(user, code);
	}




}




app.Run();
