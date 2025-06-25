using Microsoft.Extensions.DependencyInjection;
using My.ApiVersioningExample.Services.Security.V2;
using My.ApiVersioningExample.Services.Security.V2.Interfaces;

namespace My.ApiVersioningExample.Services.Configurations.V2
{
	public static class Version2Dependencies
	{
		// <summary>
		/// Registers application services and third-party dependencies into the service collection for version 2
		/// </summary>
		/// <param name="services">The service collection to which services are added.</param>
		/// <returns>The updated <see cref="IServiceCollection"/> instance with registered services.</returns>
		public static IServiceCollection AddV2ServiceDependency(this IServiceCollection services)
		{
			#region User Defined Services

			services.AddScoped<IAuthService, AuthService>();

			#endregion

			return services;
		}
	}
}
