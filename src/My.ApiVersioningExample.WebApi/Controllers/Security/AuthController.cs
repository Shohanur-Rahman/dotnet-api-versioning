using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using My.ApiVersioningExample.Common.Responses;
using My.ApiVersioningExample.Core.Security.Request;
using My.ApiVersioningExample.Core.Security.Response;
using My.ApiVersioningExample.Core.Users.DTOs.Response;
using My.ApiVersioningExample.Core.Users.Entities;
using My.ApiVersioningExample.Services.Security.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace My.ApiVersioningExample.WebApi.Controllers.Security
{
	/// <summary>
	/// API controller responsible for handling authentication-related HTTP requests,
	/// such as user registration and login.
	/// </summary>
	/// <remarks>
	/// Routes are prefixed with 'api/[controller]', which resolves to 'api/auth' by convention.
	/// </remarks>
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		#region Properties and Variables

		private readonly IAuthService _authService;
		private readonly ILogger<AuthController> _logger;
		private readonly IConfiguration _configuration;
		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthController"/> class with the specified services.
		/// </summary>
		/// <param name="authService">Service for handling authentication operations.</param>
		/// <param name="logger">Logger for logging controller activity.</param>
		/// <param name="configuration">Application configuration settings, typically used for accessing values like JWT keys or environment-specific data.</param>
		public AuthController(IAuthService authService, ILogger<AuthController> logger, IConfiguration configuration)
		{
			_logger = logger;
			_authService = authService;
			_configuration = configuration;
		}

		#endregion

		#region Public Endpoints

		/// <summary>
		/// Handles the user sign-up request by validating input, creating a new user,
		/// and returning a JWT token upon successful registration.
		/// </summary>
		/// <param name="request">The <see cref="SignUpRequest"/> containing user registration details like email and password.</param>
		/// <returns>
		/// An <see cref="ActionResult{ApiResponse{string}}"/> containing a JWT token if sign-up is successful,
		/// or an appropriate error response if validation fails or an exception occurs.
		/// </returns>
		/// <response code="200">Returns a JWT token when the user is successfully created and logged in.</response>
		/// <response code="400">Returns validation error messages if the request is invalid.</response>
		/// <response code="500">Returns a server error if an unexpected exception occurs during sign-up.</response>

		[EndpointSummary("Signup user")]
		[EndpointDescription("Creating a new user, and returns a JWT token upon successful sign-in.")]
		[Route("SignUp")]
		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<string>>> SignUpUserAsync(SignUpRequest request)
		{
			try
			{
				if (request is null)
					return BadRequest($"Signup request cannot be null {nameof(request)}");

				if (string.IsNullOrEmpty(request.Email))
					return BadRequest($"Email cannot be empty to create a new user.");

				if (string.IsNullOrEmpty(request.Password))
					return BadRequest($"Password cannot be empty to create a new user.");

				var result = await _authService.SignUpUserAsync(request);

				if (result is null)
					return BadRequest(ApiResponse<string>.Fail($"User signup failed with provided values"));

				return Ok(ApiResponse<string>.Ok(GenerateJwtToken(result), "User created and logged in"));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<string>.Fail(ex.Message));
			}
		}


		/// <summary>
		/// Authenticates a user by verifying their email and password, and returns a JWT token upon successful sign-in.
		/// </summary>
		/// <param name="request">The <see cref="SignInRequest"/> containing the user's email and password.</param>
		/// <returns>
		/// An <see cref="ActionResult{ApiResponse{string}}"/> containing a JWT token if sign-in is successful,
		/// or an appropriate error response if validation fails or an exception occurs.
		/// </returns>
		/// <response code="200">Returns a JWT token when the user is successfully signed in.</response>
		/// <response code="400">Returns validation error messages if the request is invalid.</response>
		/// <response code="500">Returns a server error if an unexpected exception occurs during sign-in.</response>
		[EndpointSummary("Signin user")]
		[EndpointDescription("Authenticates a user by verifying their email and password, and returns a JWT token upon successful sign-in.")]
		[Route("SignIn")]
		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<string>>> SignInUserAsync(SignInRequest request)
		{
			try
			{
				if (request is null)
					return BadRequest($"Signup request cannot be null {nameof(request)}");

				if (string.IsNullOrEmpty(request.Email))
					return BadRequest($"Email cannot be empty to identify user.");

				if (string.IsNullOrEmpty(request.Password))
					return BadRequest($"Password cannot be empty to identify user.");

				var result = await _authService.SignInUserAsync(request);

				if (result is null)
					return BadRequest(ApiResponse<string>.Fail($"User signin failed with provided values"));

				return Ok(ApiResponse<string>.Ok(GenerateJwtToken(result), "User signedin"));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<string>.Fail(ex.Message));
			}
		}
		#endregion


		#region Private methods

		/// <summary>
		/// Generates a JSON Web Token (JWT) containing user-specific claims based on the provided <see cref="AuthResponse"/>.
		/// </summary>
		/// <param name="result">An <see cref="AuthResponse"/> object containing the user's identity and profile information.</param>
		/// <returns>A JWT string that includes claims such as user ID, name, email, mobile number, and profile photo URL.</returns>
		/// <remarks>
		/// The token is signed using HMAC SHA-256 and is valid for 30 days. Custom claims like "mobile" and "photoUrl" are added if available.
		/// </remarks>
		private string GenerateJwtToken(AuthResponse result)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWT") ?? "");

			var claims = new List<Claim>
				{
					new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()),
					new Claim(ClaimTypes.Name, result.Name),
					new Claim(ClaimTypes.Email, result.Email)
				};

			if (!string.IsNullOrWhiteSpace(result.Mobile))
			{
				claims.Add(new Claim("mobile", result.Mobile));
			}

			if (!string.IsNullOrWhiteSpace(result.PhotoUrl))
			{
				claims.Add(new Claim("photoUrl", result.PhotoUrl));
			}

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddDays(30),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(tokenKey),
					SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		#endregion
	}

}
