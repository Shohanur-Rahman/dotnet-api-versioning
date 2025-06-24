using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using My.ApiVersioningExample.Common.Helper;
using My.ApiVersioningExample.Core.Users.Entities;
using My.ApiVersioningExample.Data.DB;
using My.ApiVersioningExample.Data.Repositories.Users;

namespace My.ApiVersioningExample.Data.Tests.Repositories.Users
{
	/// <summary>
	/// Unit tests for the <see cref="UserRepository"/> class, which validates data access operations
	/// such as creating, retrieving, updating, and deleting users using an in-memory database.
	/// </summary>
	public class UserRepositoryTests
	{
		private AppDbContext GetInMemoryDbContext()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			return new AppDbContext(options);
		}

		#region Test Methods

		/// <summary>
		/// Creates a test <see cref="DbUser"/> with mock data and hashed password.
		/// </summary>
		private DbUser CreateTestUser(Guid? id = null, string? name = "John Doe")
		{
			string password = "Password123";
			PasswordHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

			return new DbUser
			{
				Id = id ?? Guid.NewGuid(),
				Name = name,
				Email = "john@gmail.com",
				Mobile = "+9108765412",
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt,
				PhotoUrl = "localstorage/users/john.jpg"
			};
		}

		/// <summary>
		/// Tests that a user is successfully added to the database.
		/// </summary>
		[Fact]
		public async Task AddUserAsync_Should_Add_User()
		{
			var dbContext = GetInMemoryDbContext();
			var repository = new UserRepository(dbContext);
			var user = CreateTestUser();

			var result = await repository.AddUserAsync(user);

			result.Should().NotBeNull();
			result!.Id.Should().Be(user.Id);
		}

		/// <summary>
		/// Tests that all users are retrieved from the database.
		/// </summary>
		[Fact]
		public async Task GetUsersAsync_Should_Return_All_Users()
		{
			var dbContext = GetInMemoryDbContext();
			var users = new[] {
			CreateTestUser(name: "John Doe"),
			CreateTestUser(name: "Alex Hels")
		};

			await dbContext.Users.AddRangeAsync(users);
			await dbContext.SaveChangesAsync();

			var repository = new UserRepository(dbContext);
			var result = await repository.GetUsersAsync();

			result.Should().HaveCount(2);
		}

		/// <summary>
		/// Tests that a specific user is retrieved by their unique ID.
		/// </summary>
		[Fact]
		public async Task GetUserByIdAsync_Should_Return_Correct_User()
		{
			var dbContext = GetInMemoryDbContext();
			var targetUser = CreateTestUser();
			var anotherUser = CreateTestUser();

			await dbContext.Users.AddRangeAsync(targetUser, anotherUser);
			await dbContext.SaveChangesAsync();

			var repository = new UserRepository(dbContext);
			var result = await repository.GetUserByIdAsync(targetUser.Id);

			result.Should().NotBeNull();
			result!.Id.Should().Be(targetUser.Id);
		}

		/// <summary>
		/// Tests that a user is updated successfully in the database.
		/// </summary>
		[Fact]
		public async Task UpdateUserAsync_Should_Modify_User()
		{
			var dbContext = GetInMemoryDbContext();
			var user = CreateTestUser();
			await dbContext.Users.AddAsync(user);
			await dbContext.SaveChangesAsync();

			user.Name = "Updated Name";

			var repository = new UserRepository(dbContext);
			var result = await repository.UpdateUserAsync(user);

			result!.Name.Should().Be("Updated Name");
		}

		/// <summary>
		/// Tests that a user is deleted successfully from the database.
		/// </summary>
		[Fact]
		public async Task DeleteUserAsync_Should_Remove_User()
		{
			var dbContext = GetInMemoryDbContext();
			var user = CreateTestUser();
			await dbContext.Users.AddAsync(user);
			await dbContext.SaveChangesAsync();

			var repository = new UserRepository(dbContext);
			var result = await repository.DeleteUserAsync(user.Id);

			result.Should().BeTrue();
			(await dbContext.Users.FindAsync(user.Id)).Should().BeNull();
		}

		/// <summary>
		/// Tests that requesting a user by an empty ID throws an exception.
		/// </summary>
		[Fact]
		public async Task GetUserByIdAsync_With_EmptyId_Should_Throw()
		{
			var dbContext = GetInMemoryDbContext();
			var repository = new UserRepository(dbContext);

			Func<Task> act = async () => await repository.GetUserByIdAsync(Guid.Empty);

			await act.Should().ThrowAsync<ArgumentException>();
		}


		#endregion
	}

}
