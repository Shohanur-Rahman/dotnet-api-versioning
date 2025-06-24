using Scalar.AspNetCore;
using System.Net;

namespace My.ApiVersioningExample.WebApi.Configuration
{
	public static class AppBuilder
	{
		public static WebApplication ConfigApplication(this WebApplication app)
		{
			app.UseStaticFiles();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.MapScalarApiReference();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();

			return app;
		}
	}
}
