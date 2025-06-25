using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using My.ApiVersioningExample.Common.Responses;
using My.ApiVersioningExample.Core.Security.V2;
using My.ApiVersioningExample.Services.Security.V2.Interfaces;
using My.ApiVersioningExample.WebApi.Utilities;

namespace My.ApiVersioningExample.WebApi.Controllers.V2.Security
{
	/// <summary>
	/// API controller responsible for handling authentication-related HTTP requests,
	/// such as user registration and login.
	/// </summary>
	/// <remarks>
	/// Routes are prefixed with 'api/[controller]', which resolves to 'api/auth' by convention.
	/// </remarks>
	[ApiVersion("2.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
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

				return Ok(ApiResponse<string>.Ok(JWTHelper.GenerateJwtToken(result, _configuration), "User created and logged in"));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<string>.Fail(ex.Message));
			}
		}
		#endregion

	}
}
