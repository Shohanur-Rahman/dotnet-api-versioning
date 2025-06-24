using Microsoft.Extensions.DependencyInjection;
using My.ApiVersioningExample.Data.Repositories.Users;
using My.ApiVersioningExample.Data.Repositories.Users.Interfaces;

namespace My.ApiVersioningExample.Data.Configurations
{
	/// <summary>
	/// Provides extension methods for registering application-specific services and dependencies.
	/// </summary>
	public static class DependenciInjections
	{
		/// <summary>
		/// Registers application repository services and third-party dependencies into the service collection.
		/// </summary>
		/// <param name="services">The service collection to which services are added.</param>
		/// <returns>The updated <see cref="IServiceCollection"/> instance with registered services.</returns>
		public static IServiceCollection AddRepositoryDependency(this IServiceCollection services)
		{
			#region User Defined Services

			services.AddScoped<IUserRepository, UserRepository>();

			#endregion

			return services;
		}
	}

}
