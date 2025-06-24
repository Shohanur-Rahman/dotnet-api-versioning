using Microsoft.EntityFrameworkCore;
using My.ApiVersioningExample.Core.Users.Entities;
using My.ApiVersioningExample.Data.DB;
using My.ApiVersioningExample.Data.Repositories.Users.Interfaces;

namespace My.ApiVersioningExample.Data.Repositories.Users
{
	/// <summary>
	/// Provides data access operations for user entities in the application.
	/// Implements the <see cref="IUserRepository"/> interface and interacts with the database using Entity Framework Core.
	/// </summary>
	public class UserRepository : IUserRepository
	{
		#region Properties and Variables

		private readonly AppDbContext _dbContext;
		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="UserRepository"/> class with the specified database context and object mapper.
		/// </summary>
		/// <param name="dbContext">The application's database context.</param>
		public UserRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		#endregion

		#region Public Methods

		/// <summary>
		/// Retrieves all users from the database.
		/// </summary>
		/// <returns>A list of user entities.</returns>
		public async Task<IEnumerable<DbUser>> GetUsersAsync()
		{
			return await _dbContext.Users.ToListAsync();
		}

		/// <summary>
		/// Retrieves a user by their unique identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the user.</param>
		/// <returns>The matching user entity, or null if not found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when the ID is empty.</exception>
		public async Task<DbUser?> GetUserByIdAsync(Guid id)
		{
			if (id == Guid.Empty)
				throw new ArgumentException("User ID cannot be empty.", nameof(id));

			return await _dbContext.Users.FindAsync(id);
		}

		/// <summary>
		/// Asynchronously retrieves a user from the database by their email address.
		/// </summary>
		/// <param name="email">The email address of the user to retrieve.</param>
		/// <returns>
		/// A <see cref="DbUser"/> instance if a user with the specified email exists; otherwise, <c>null</c>.
		/// </returns>
		/// <exception cref="ArgumentException">Thrown when the provided email is null or empty.</exception>
		public async Task<DbUser?> GetUserByEmailAsync(string email)
		{
			if (string.IsNullOrEmpty(email))
				throw new ArgumentException("User email cannot be empty.", nameof(email));

			return await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
		}


		/// <summary>
		/// Adds a new user to the database.
		/// </summary>
		/// <param name="user">The request containing user creation data.</param>
		/// <returns>The created user entity.</returns>
		/// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
		public async Task<DbUser?> AddUserAsync(DbUser user)
		{
			if (user is null)
				throw new ArgumentNullException($"User create request cannot be null {nameof(user)}");

			await _dbContext.Users.AddAsync(user);
			await _dbContext.SaveChangesAsync();
			return await GetUserByIdAsync(user.Id);
		}

		/// <summary>
		/// Updates an existing user's information in the database.
		/// </summary>
		/// <param name="user">The request containing updated user data.</param>
		/// <returns>The updated user entity.</returns>
		/// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
		/// <exception cref="KeyNotFoundException">Thrown when the user does not exist.</exception>
		public async Task<DbUser?> UpdateUserAsync(DbUser user)
		{
			if (user is null)
				throw new ArgumentNullException($"User information cannot be null {nameof(user)}");

			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync();
			return await GetUserByIdAsync(user.Id);
		}

		/// <summary>
		/// Deletes a user from the database by their unique identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the user to delete.</param>
		/// <returns>True if the user was successfully deleted; otherwise, false.</returns>
		/// <exception cref="ArgumentNullException">Thrown when the ID is empty.</exception>
		/// <exception cref="KeyNotFoundException">Thrown when the user does not exist.</exception>
		public async Task<bool> DeleteUserAsync(Guid id)
		{
			bool result = false;
			if (id == Guid.Empty)
				throw new ArgumentException("User ID cannot be empty.", nameof(id));

			var userInfo = await _dbContext.Users.FindAsync(id);

			if (userInfo is null)
				throw new KeyNotFoundException($"Data with ID {id} was not found.");

			_dbContext.Users.Remove(userInfo);
			await _dbContext.SaveChangesAsync();
			result = true;
			return result;
		}
		#endregion
	}

}
