using Microsoft.Extensions.DependencyInjection;
using My.ApiVersioningExample.Services.Users;
using My.ApiVersioningExample.Services.Users.Interfaces;

namespace My.ApiVersioningExample.Services.Configurations
{
	/// <summary>
	/// Provides extension methods for registering application-specific services and dependencies.
	/// </summary>
	public static class DependenciInjections
	{
		/// <summary>
		/// Registers application services and third-party dependencies into the service collection.
		/// </summary>
		/// <param name="services">The service collection to which services are added.</param>
		/// <returns>The updated <see cref="IServiceCollection"/> instance with registered services.</returns>
		public static IServiceCollection AddServiceDependency(this IServiceCollection services)
		{
			#region Package Dependencies

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			#endregion

			#region User Defined Services

			services.AddScoped<IUserService, UserService>();

			#endregion

			return services;
		}
	}
}
