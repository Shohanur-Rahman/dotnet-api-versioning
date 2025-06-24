using My.ApiVersioningExample.Core.Security.Request;
using My.ApiVersioningExample.Core.Security.Response;

namespace My.ApiVersioningExample.Services.Security.Interfaces
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
		Task<AuthResponse> SignUpUserAsync(SignUpRequest request);


		/// <summary>
		/// Asynchronously authenticates a user by verifying their email and password.
		/// </summary>
		/// <param name="request">The <see cref="SignInRequest"/> containing the user's email and password.</param>
		/// <returns>An <see cref="AuthResponse"/> representing the authenticated user's details.</returns>
		Task<AuthResponse> SignInUserAsync(SignInRequest request);
		#endregion
	}

}
