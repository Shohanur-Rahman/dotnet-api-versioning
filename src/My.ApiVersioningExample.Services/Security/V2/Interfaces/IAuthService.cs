using My.ApiVersioningExample.Core.Security.Response;
using SecurityV2 = My.ApiVersioningExample.Core.Security.V2;

namespace My.ApiVersioningExample.Services.Security.V2.Interfaces
{
	/// <summary>
	/// Defines the contract for authentication-related operations such as user sign-up, login, and token management.
	/// </summary>
	public interface IAuthService
	{
		#region Public Methods

		/// <summary>
		/// Asynchronously handles the user sign-up process by validating the request,
		/// hashing the password, and storing the new user in the database.
		/// </summary>
		/// <param name="request">The sign-up request containing the user's email and password.</param>
		/// <returns>An <see cref="AuthResponse"/> containing the authentication result for the newly created user.</returns>
		Task<AuthResponse> SignUpUserAsync(SecurityV2.SignUpRequest request);
		#endregion
	}
}
