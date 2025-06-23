using Scalar.AspNetCore;
using System.Net;

namespace My.ApiVersioningExample.WebApi.Configuration
{
	public static class AppBuilder
	{
		public static WebApplication ConfigApplication(this WebApplication app)
		{
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.MapScalarApiReference();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();

			return app;
		}
	}
}
