namespace EFCProject
{
	public class Startup
	{

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				//app.UseExceptionHandler("/error-development");
			}
			else
			{
				app.UseHsts();
				//app.UseExceptionHandler("/error-staging");
			}
			// log errors 
			if (string.IsNullOrWhiteSpace(env.WebRootPath))
			{
				env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
			}
			// Create log folder if not exists
			if (!Directory.Exists(Path.Combine(env.WebRootPath, "logs"))) Directory.CreateDirectory(Path.Combine(env.WebRootPath, "logs"));


		}

	}
}
