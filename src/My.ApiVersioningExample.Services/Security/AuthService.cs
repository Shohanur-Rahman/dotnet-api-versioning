using AutoMapper;
using My.ApiVersioningExample.Common.Helper;
using My.ApiVersioningExample.Core.Security.Request;
using My.ApiVersioningExample.Core.Security.Response;
using My.ApiVersioningExample.Core.Users.Entities;
using My.ApiVersioningExample.Data.Repositories.Users.Interfaces;
using My.ApiVersioningExample.Services.Security.Interfaces;

namespace My.ApiVersioningExample.Services.Security
{
	/// <summary>
	/// Provides authentication-related services such as user sign-up, login, and token generation.
	/// Implements the <see cref="IAuthService"/> interface.
	/// </summary>
	public class AuthService: IAuthService
	{
		#region Properties and Variables
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthService"/> class with the specified user repository and mapper.
		/// </summary>
		/// <param name="userRepository">The user repository for data access.</param>
		/// <param name="mapper">The AutoMapper instance for object mapping.</param>
		public AuthService(IUserRepository userRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Asynchronously handles the user sign-up process by validating the request,
		/// hashing the password, and storing the new user in the database.
		/// </summary>
		/// <param name="request">The sign-up request containing the user's email and password.</param>
		/// <returns>An <see cref="AuthResponse"/> containing the authentication result for the newly created user.</returns>
		/// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
		/// <exception cref="Exception">
		/// Thrown when the email or password is null or empty.
		/// </exception>
		public async Task<AuthResponse> SignUpUserAsync(SignUpRequest request)
		{
			if (request is null)
				throw new ArgumentNullException($"Signup request cannot be null {nameof(request)}");

			if (string.IsNullOrEmpty(request.Email))
				throw new Exception($"Email cannot be empty to create a new user.");

			if (string.IsNullOrEmpty(request.Password))
				throw new Exception($"Password cannot be empty to create a new user.");

			DbUser user = _mapper.Map<DbUser>(request);
			//Create has and salt password
			PasswordHelper.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);

			user.PasswordSalt = passwordSalt;
			user.PasswordHash = passwordHash;
			user.Name = string.Empty;
			user.Mobile = string.Empty;

			var result = await _userRepository.AddUserAsync(user);

			return _mapper.Map<AuthResponse>(result);

		}


		/// <summary>
		/// Asynchronously authenticates a user by verifying their email and password.
		/// </summary>
		/// <param name="request">The <see cref="SignInRequest"/> containing the user's email and password.</param>
		/// <returns>An <see cref="AuthResponse"/> representing the authenticated user's details.</returns>
		/// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is null.</exception>
		/// <exception cref="Exception">
		/// Thrown when the email or password is empty,
		/// or if the user is not found,
		/// or if the password verification fails.
		/// </exception>
		public async Task<AuthResponse> SignInUserAsync(SignInRequest request)
		{
			if (request is null)
				throw new ArgumentNullException($"Signup request cannot be null {nameof(request)}");

			if (string.IsNullOrEmpty(request.Email))
				throw new Exception($"Email cannot be empty to identify user.");

			if (string.IsNullOrEmpty(request.Password))
				throw new Exception($"Password cannot be empty to identify user.");

			DbUser? user = await _userRepository.GetUserByEmailAsync(request.Email);

			if(user is null)
				throw new Exception($"User not found to your current request.");
			

			//Verify user
			bool isVerify = PasswordHelper.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt);

			if(!isVerify)
				throw new Exception($"User not found to your current request.");

			return _mapper.Map<AuthResponse>(user);

		}
		#endregion
	}
}
