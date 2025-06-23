using AutoMapper;
using My.ApiVersioningExample.Core.Users.DTOs.Request;
using My.ApiVersioningExample.Core.Users.DTOs.Response;
using My.ApiVersioningExample.Core.Users.Entities;
using My.ApiVersioningExample.Data.Repositories.Users.Interfaces;
using My.ApiVersioningExample.Services.Users.Interfaces;

namespace My.ApiVersioningExample.Services.Users
{
	/// <summary>
	/// Provides user-related business logic and operations by interacting with the user repository and mapping data transfer objects.
	/// </summary>
	public class UserService : IUserService
	{
		#region Properties and Variables
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		#endregion


		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="UserService"/> class with the specified user repository and mapper.
		/// </summary>
		/// <param name="userRepository">The user repository for data access.</param>
		/// <param name="mapper">The AutoMapper instance for object mapping.</param>
		public UserService(IUserRepository userRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Retrieves all users and maps them to detailed response models.
		/// </summary>
		/// <returns>A collection of user details.</returns>
		public async Task<IEnumerable<UserDetailResponse>> GetUsersAsync()
		{
			var result = await _userRepository.GetUsersAsync();
			return _mapper.Map<IEnumerable<UserDetailResponse>>(result);
		}

		/// <summary>
		/// Retrieves a specific user by ID and maps it to a detailed response model.
		/// </summary>
		/// <param name="id">The unique identifier of the user.</param>
		/// <returns>The detailed user response if found; otherwise, null.</returns>
		public async Task<UserDetailResponse> GetUserByIdAsync(Guid id)
		{
			var result = await _userRepository.GetUserByIdAsync(id);
			return _mapper.Map<UserDetailResponse>(result);
		}

		/// <summary>
		/// Creates a new user from the provided creation request.
		/// </summary>
		/// <param name="request">The user creation request data.</param>
		/// <returns>The created user's detailed response.</returns>
		/// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
		public async Task<UserDetailResponse> AddUserAsync(UserCreateRequest request)
		{
			if (request is null)
				throw new ArgumentNullException($"User information cannot be null {nameof(request)}");

			DbUser user = _mapper.Map<DbUser>(request);
			var result = await _userRepository.AddUserAsync(user);
			return _mapper.Map<UserDetailResponse>(result);
		}

		/// <summary>
		/// Updates an existing user's information based on the provided update request.
		/// </summary>
		/// <param name="request">The user update request data.</param>
		/// <returns>The updated user's detailed response.</returns>
		/// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
		public async Task<UserDetailResponse> UpdateUserAsync(UserUpdateRequest request)
		{
			if (request is null)
				throw new ArgumentNullException($"User information cannot be null {nameof(request)}");

			DbUser user = _mapper.Map<DbUser>(request);
			var result = await _userRepository.UpdateUserAsync(user);
			return _mapper.Map<UserDetailResponse>(result);
		}

		/// <summary>
		/// Deletes a user identified by the specified ID.
		/// </summary>
		/// <param name="id">The unique identifier of the user to delete.</param>
		/// <returns>True if the user was successfully deleted; otherwise, false.</returns>
		/// <exception cref="ArgumentException">Thrown when the provided ID is empty.</exception>
		public async Task<bool> DeleteUserAsync(Guid id)
		{
			if (id == Guid.Empty)
				throw new ArgumentException("User ID cannot be empty.", nameof(id));

			return await _userRepository.DeleteUserAsync(id);
		}
		#endregion
	}


}
