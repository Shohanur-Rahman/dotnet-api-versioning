using My.ApiVersioningExample.Core.Users.Entities;

namespace My.ApiVersioningExample.Data.Repositories.Users.Interfaces
{
	/// <summary>
	/// Defines the contract for user-related data operations in the application.
	/// </summary>
	public interface IUserRepository
	{
		/// <summary>
		/// Retrieves all users from the database.
		/// </summary>
		/// <returns>A collection of all user entities.</returns>
		Task<IEnumerable<DbUser>> GetUsersAsync();

		/// <summary>
		/// Retrieves a user by their unique identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the user.</param>
		/// <returns>The user entity if found; otherwise, null.</returns>
		Task<DbUser?> GetUserByIdAsync(Guid id);

		/// <summary>
		/// Adds a new user to the database.
		/// </summary>
		/// <param name="request">The user creation request containing user data.</param>
		/// <returns>The newly created user entity.</returns>
		Task<DbUser?> AddUserAsync(DbUser request);

		/// <summary>
		/// Updates an existing user's data in the database.
		/// </summary>
		/// <param name="request">The user update request containing updated user data.</param>
		/// <returns>The updated user entity if successful.</returns>
		Task<DbUser?> UpdateUserAsync(DbUser request);

		/// <summary>
		/// Deletes a user from the database by their unique identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the user to delete.</param>
		/// <returns>True if the user was successfully deleted; otherwise, false.</returns>
		Task<bool> DeleteUserAsync(Guid id);
	}

}
