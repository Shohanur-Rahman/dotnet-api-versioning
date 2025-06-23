using Microsoft.EntityFrameworkCore;
using My.ApiVersioningExample.Data.DB;

namespace My.ApiVersioningExample.WebApi.Configuration
{
	/// <summary>
	/// Provides extension methods for registering application-specific services and dependencies.
	/// </summary>
	public static class WebApiServices
	{
		/// <summary>
		/// Registers application services and third-party dependencies into the service collection.
		/// </summary>
		/// <param name="services">The service collection to which services are added.</param>
		/// <returns>The updated <see cref="IServiceCollection"/> instance with registered services.</returns>
		public static IServiceCollection AddWebApiDependency(this IServiceCollection services, WebApplicationBuilder builder)
		{
			#region User Defined Services

			services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
				options.EnableSensitiveDataLogging();
				options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
			}, ServiceLifetime.Transient);


			services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			services.AddOpenApi();

			#endregion

			return services;
		}
	}
}
