using AutoMapper;
using FluentAssertions;
using Moq;
using My.ApiVersioningExample.Core.Users.DTOs.Request;
using My.ApiVersioningExample.Core.Users.DTOs.Response;
using My.ApiVersioningExample.Core.Users.Entities;
using My.ApiVersioningExample.Data.Repositories.Users.Interfaces;
using My.ApiVersioningExample.Services.Users;

namespace My.ApiVersioningExample.Services.Tests.Users
{
	/// <summary>
	/// Contains unit tests for <see cref="UserService"/> methods using mocked dependencies.
	/// </summary>
	public class UserServiceTests
	{
		private readonly Mock<IUserRepository> _userRepositoryMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly UserService _userService;

		public UserServiceTests()
		{
			_userRepositoryMock = new Mock<IUserRepository>();
			_mapperMock = new Mock<IMapper>();
			_userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object);
		}

		/// <summary>
		/// Tests that GetUsersAsync returns a list of users mapped correctly from the database.
		/// </summary>
		[Fact]
		public async Task GetUsersAsync_Should_Return_Mapped_Users()
		{
			// Arrange
			var dbUsers = new List<DbUser> { new DbUser { Id = Guid.NewGuid(), Name = "John" } };
			var mappedUsers = new List<UserDetailResponse> { new UserDetailResponse { Id = dbUsers[0].Id, Name = "John" } };

			_userRepositoryMock.Setup(r => r.GetUsersAsync()).ReturnsAsync(dbUsers);
			_mapperMock.Setup(m => m.Map<IEnumerable<UserDetailResponse>>(dbUsers)).Returns(mappedUsers);

			// Act
			var result = await _userService.GetUsersAsync();

			// Assert
			result.Should().BeEquivalentTo(mappedUsers);
		}

		/// <summary>
		/// Tests that GetUserByIdAsync throws an ArgumentException when given an empty GUID.
		/// </summary>
		[Fact]
		public async Task GetUserByIdAsync_Should_Throw_When_Id_Is_Empty()
		{
			// Act
			Func<Task> act = async () => await _userService.GetUserByIdAsync(Guid.Empty);

			// Assert
			await act.Should().ThrowAsync<ArgumentException>();
		}

		/// <summary>
		/// Tests that GetUserByIdAsync returns the correct mapped user when a valid ID is provided.
		/// </summary>
		[Fact]
		public async Task GetUserByIdAsync_Should_Return_Mapped_User()
		{
			// Arrange
			var userId = Guid.NewGuid();
			var dbUser = new DbUser { Id = userId, Name = "John" };
			var mappedUser = new UserDetailResponse { Id = userId, Name = "John" };

			_userRepositoryMock.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync(dbUser);
			_mapperMock.Setup(m => m.Map<UserDetailResponse>(dbUser)).Returns(mappedUser);

			// Act
			var result = await _userService.GetUserByIdAsync(userId);

			// Assert
			result.Should().BeEquivalentTo(mappedUser);
		}

		/// <summary>
		/// Tests that AddUserAsync correctly creates a user and returns the mapped response.
		/// </summary>
		[Fact]
		public async Task AddUserAsync_Should_Create_User_And_Return_Mapped_Response()
		{
			// Arrange
			var request = new UserCreateRequest
			{
				Name = "Jane",
				Email = "jane@example.com",
				Mobile = "1234567890",
				Password = "Secret123"
			};

			var dbUser = new DbUser { Id = Guid.NewGuid(), Name = request.Name };
			var createdUser = new DbUser { Id = dbUser.Id, Name = request.Name };
			var response = new UserDetailResponse { Id = dbUser.Id, Name = dbUser.Name };

			_mapperMock.Setup(m => m.Map<DbUser>(request)).Returns(dbUser);
			_userRepositoryMock.Setup(r => r.AddUserAsync(It.IsAny<DbUser>())).ReturnsAsync(createdUser);
			_mapperMock.Setup(m => m.Map<UserDetailResponse>(createdUser)).Returns(response);

			// Act
			var result = await _userService.AddUserAsync(request);

			// Assert
			result.Should().BeEquivalentTo(response);
		}

		/// <summary>
		/// Tests that UpdateUserAsync throws a KeyNotFoundException when the user is not found.
		/// </summary>
		[Fact]
		public async Task UpdateUserAsync_Should_Throw_If_User_Not_Found()
		{
			// Arrange
			var request = new UserUpdateRequest { Id = Guid.NewGuid(), Name = "Updated" };
			_userRepositoryMock.Setup(r => r.GetUserByIdAsync(request.Id)).ReturnsAsync((DbUser?)null);

			// Act
			Func<Task> act = async () => await _userService.UpdateUserAsync(request);

			// Assert
			await act.Should().ThrowAsync<KeyNotFoundException>();
		}

		/// <summary>
		/// Tests that UpdateUserAsync correctly maps and updates an existing user.
		/// </summary>
		[Fact]
		public async Task UpdateUserAsync_Should_Map_And_Update_User()
		{
			// Arrange
			var request = new UserUpdateRequest { Id = Guid.NewGuid(), Name = "Updated" };
			var dbUser = new DbUser { Id = request.Id, Name = "Old" };
			var updatedUser = new DbUser { Id = request.Id, Name = "Updated" };
			var response = new UserDetailResponse { Id = request.Id, Name = "Updated" };

			_userRepositoryMock.Setup(r => r.GetUserByIdAsync(request.Id)).ReturnsAsync(dbUser);
			_userRepositoryMock.Setup(r => r.UpdateUserAsync(dbUser)).ReturnsAsync(updatedUser);
			_mapperMock.Setup(m => m.Map(request, dbUser));
			_mapperMock.Setup(m => m.Map<UserDetailResponse>(updatedUser)).Returns(response);

			// Act
			var result = await _userService.UpdateUserAsync(request);

			// Assert
			result.Should().BeEquivalentTo(response);
		}

		/// <summary>
		/// Tests that PhotoUrlUpdateAsync throws an ArgumentNullException when the request is null.
		/// </summary>
		[Fact]
		public async Task PhotoUrlUpdateAsync_Should_Throw_If_Request_Is_Null()
		{
			// Act
			Func<Task> act = async () => await _userService.PhotoUrlUpdateAsync(null!);

			// Assert
			await act.Should().ThrowAsync<ArgumentNullException>();
		}

		/// <summary>
		/// Tests that DeleteUserAsync returns true when a user is successfully deleted.
		/// </summary>
		[Fact]
		public async Task DeleteUserAsync_Should_Return_True_When_Deleted()
		{
			// Arrange
			var userId = Guid.NewGuid();
			_userRepositoryMock.Setup(r => r.DeleteUserAsync(userId)).ReturnsAsync(true);

			// Act
			var result = await _userService.DeleteUserAsync(userId);

			// Assert
			result.Should().BeTrue();
		}
	}
}
