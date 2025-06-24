using My.ApiVersioningExample.Core.Users.DTOs.Request;
using My.ApiVersioningExample.Core.Users.DTOs.Response;

namespace My.ApiVersioningExample.Services.Users.Interfaces
{
	/// <summary>
	/// Defines user-related operations for managing user data and business logic.
	/// </summary>
	public interface IUserService
	{
		#region Public Methods

		/// <summary>
		/// Retrieves all users.
		/// </summary>
		/// <returns>A collection of user detail responses.</returns>
		Task<IEnumerable<UserDetailResponse>> GetUsersAsync();

		/// <summary>
		/// Retrieves a user by their unique identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the user.</param>
		/// <returns>The user detail response if found; otherwise, null.</returns>
		Task<UserDetailResponse> GetUserByIdAsync(Guid id);

		/// <summary>
		/// Creates a new user.
		/// </summary>
		/// <param name="request">The user creation request data.</param>
		/// <returns>The created user's detailed response.</returns>
		Task<UserDetailResponse> AddUserAsync(UserCreateRequest request);

		/// <summary>
		/// Updates an existing user.
		/// </summary>
		/// <param name="request">The user update request data.</param>
		/// <returns>The updated user's detailed response.</returns>
		Task<UserDetailResponse> UpdateUserAsync(UserUpdateRequest request);

		/// <summary>
		/// Updates the user's profile photo URL based on the provided request.
		/// </summary>
		/// <param name="request">The request containing the user's ID and the new photo URL.</param>
		/// <returns>A <see cref="UserDetailResponse"/> containing the updated user details.</returns>
		Task<UserDetailResponse> PhotoUrlUpdateAsync(PhotoUrlRequest request);

		/// <summary>
		/// Deletes a user by their unique identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the user to delete.</param>
		/// <returns>True if the user was successfully deleted; otherwise, false.</returns>
		Task<bool> DeleteUserAsync(Guid id);
		#endregion
	}

}
