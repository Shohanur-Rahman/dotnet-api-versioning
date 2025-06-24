using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My.ApiVersioningExample.Common.Enums.File;
using My.ApiVersioningExample.Common.Responses;
using My.ApiVersioningExample.Core.Users.DTOs.Request;
using My.ApiVersioningExample.Core.Users.DTOs.Response;
using My.ApiVersioningExample.Services.Users.Interfaces;
using My.ApiVersioningExample.WebApi.Urilities;

namespace My.ApiVersioningExample.WebApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		#region Properties and Variables

		private readonly IUserService _userService;
		private readonly ILogger<UsersController> _logger;
		private readonly FileUploadService _fileUploadService;

		#endregion

		#region Constructors

		public UsersController(IUserService userService, ILogger<UsersController> logger, FileUploadService fileUploadService)
		{
			_logger = logger;
			_userService = userService;
			_fileUploadService = fileUploadService;
		}
		#endregion

		#region Public Endpoints


		/// <summary>
		/// Retrieves a list of all users.
		/// </summary>
		/// <returns>
		/// An <see cref="ApiResponse{IEnumerable{UserDetailResponse}}"/> containing a collection of user details,
		/// or an error response if the operation fails.
		/// </returns>
		/// <response code="200">Users fetched successfully.</response>
		/// <response code="401">Unauthorized to perform this operation.</response>
		/// <response code="500">An internal server error occurred while fetching users.</response>
		/// 
		[EndpointSummary("Get all users")]
		[EndpointDescription("Containing a collection of user details")]
		[HttpGet]
		[ProducesResponseType(typeof(ApiResponse<List<UserDetailResponse>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<List<UserDetailResponse>>), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(ApiResponse<List<UserDetailResponse>>), StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<ApiResponse<IEnumerable<UserDetailResponse>>>> GetUsersAsync()
		{
			try
			{
				var result = await _userService.GetUsersAsync();
				var response = ApiResponse<IEnumerable<UserDetailResponse>>.Ok(result, "Users fetched successfully");
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<IEnumerable<UserDetailResponse>>.Fail(ex.Message));
			}
		}

		/// <summary>
		/// Retrieves user details by the specified user ID.
		/// </summary>
		/// <param name="id">The unique identifier of the user to retrieve.</param>
		/// <returns>
		/// An <see cref="ApiResponse{UserDetailResponse}"/> containing the user details if found,
		/// or an error response if the user is not found or the request is invalid.
		/// </returns>
		/// <response code="200">User details retrieved successfully.</response>
		/// <response code="400">The provided user ID is invalid.</response>
		/// <response code="401">Unauthorized to perform this operation.</response>
		/// <response code="404">No user found with the specified ID.</response>
		/// <response code="500">An internal server error occurred while processing the request.</response>
		/// 
		[EndpointSummary("Get specefif user")]
		[EndpointDescription("Retrieves user details by the specified user ID.")]
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<UserDetailResponse>>> GetUserByIdAsync(Guid id)
		{
			try
			{
				if (id == Guid.Empty)
					return BadRequest(ApiResponse<UserDetailResponse>.Fail($"User ID {id} not valid."));

				var result = await _userService.GetUserByIdAsync(id);

				if(result is null)
					return NotFound(ApiResponse<UserDetailResponse>.Fail($"User not found with ID: {id}"));

				return Ok(ApiResponse<UserDetailResponse>.Ok(result, "User fetched successfully"));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<UserDetailResponse>.Fail(ex.Message));
			}
		}

		/// <summary>
		/// Creates a new user based on the provided <see cref="UserCreateRequest"/>.
		/// </summary>
		/// <param name="request">The request containing information to create a new user.</param>
		/// <returns>
		/// An <see cref="ApiResponse{UserDetailResponse}"/> containing the details of the newly created user if successful,
		/// or an error response if the creation fails or the request is invalid.
		/// </returns>
		/// <response code="200">User created successfully and the user details are returned.</response>
		/// <response code="400">The request is invalid, such as a null input.</response>
		/// <response code="401">Unauthorized to perform the create operation.</response>
		/// <response code="500">An internal server error occurred during the user creation process.</response>

		[EndpointSummary("Create user")]
		[EndpointDescription("Creates a new user based on the provided information.")]
		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<UserDetailResponse>>> AddUserAsync(UserCreateRequest request)
		{
			try
			{
				if (request is null)
					return BadRequest(ApiResponse<UserDetailResponse>.Fail($"Request cannot be null {nameof(request)}"));

				var result = await _userService.AddUserAsync(request);

				if (result is null)
					return BadRequest(ApiResponse<UserDetailResponse>.Fail($"User create request fail with provided values"));

				return Ok(ApiResponse<UserDetailResponse>.Ok(result, "User created successfully"));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<UserDetailResponse>.Fail(ex.Message));
			}
		}

		/// <summary>
		/// Updates the user details based on the provided <see cref="UserUpdateRequest"/>.
		/// </summary>
		/// <param name="request">The request containing user information to update.</param>
		/// <returns>
		/// An <see cref="ApiResponse{UserDetailResponse}"/> containing the updated user details if successful,
		/// or an error response if the update fails or the request is invalid.
		/// </returns>
		/// <response code="200">User updated successfully and updated details are returned.</response>
		/// <response code="400">The request is invalid, such as null input or invalid user ID.</response>
		/// <response code="401">Unauthorized to perform the update operation.</response>
		/// <response code="500">An internal server error occurred during the update process.</response>
		/// 
		[EndpointSummary("Update user")]
		[EndpointDescription("Updates the user details based on the provided information.")]
		[HttpPut]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<UserDetailResponse>>> UpdateUserAsync(UserUpdateRequest request)
		{
			try
			{
				if (request is null)
					return BadRequest(ApiResponse<UserDetailResponse>.Fail($"Request cannot be null {nameof(request)}"));

				if (request.Id == Guid.Empty)
					return BadRequest(ApiResponse<UserDetailResponse>.Fail($"User ID {request.Id} not valid."));

				var result = await _userService.UpdateUserAsync(request);

				if (result is null)
					return BadRequest(ApiResponse<UserDetailResponse>.Fail($"User update request fail with provided values"));

				return Ok(ApiResponse<UserDetailResponse>.Ok(result, "User updated successfully"));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<UserDetailResponse>.Fail(ex.Message));
			}
		}

		/// <summary>
		/// Updates the user's profile photo by uploading a new image and updating the user's photo URL.
		/// </summary>
		/// <param name="request">The request containing the user's ID and the new photo file.</param>
		/// <returns>
		/// An <see cref="ApiResponse{UserDetailResponse}"/> containing the updated user details if successful,
		/// or an error response if the request is invalid or the update fails.
		/// </returns>
		/// <response code="200">Photo updated successfully and user details returned.</response>
		/// <response code="400">The request is invalid (e.g., null request, empty ID, or missing file).</response>
		/// <response code="401">The user is not authorized to perform this operation.</response>
		/// <response code="500">An internal server error occurred during photo upload or user update.</response>
		/// 
		[EndpointSummary("Picture update")]
		[EndpointDescription("Updates the user's profile photo by uploading a new image and updating the user's photo URL.")]
		[HttpPatch("photo-update")]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<UserDetailResponse>>> PhotoUrlUpdateAsync([FromForm] PhotoChangeRequest request)
		{
			try
			{
				if (request is null)
					return BadRequest(ApiResponse<UserDetailResponse>.Fail($"Request cannot be null {nameof(request)}"));

				if (request.Id == Guid.Empty)
					return BadRequest(ApiResponse<UserDetailResponse>.Fail($"User ID {request.Id} not valid."));

				if (request.Attachment is null)
					return BadRequest(ApiResponse<UserDetailResponse>.Fail($"Attached file cannot be null"));


				FileResponse? fileResponse = await _fileUploadService.UploadFileLocalyAndGetUrl(request.Attachment, UploadFolder.Users.ToString());

				if (fileResponse == null || string.IsNullOrEmpty(fileResponse.FilePath))
					throw new Exception("Photo upload failed");

				PhotoUrlRequest photoUrlRequest = new PhotoUrlRequest()
				{
					Id = request.Id,
					PhotoUrl = fileResponse.FilePath
				};

				var result = await _userService.PhotoUrlUpdateAsync(photoUrlRequest);

				if (result is null)
					return BadRequest(ApiResponse<UserDetailResponse>.Fail($"User update request fail with provided values"));

				return Ok(ApiResponse<UserDetailResponse>.Ok(result, "Photo updated successfully"));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<UserDetailResponse>.Fail(ex.Message));
			}
		}



		/// <summary>
		/// Deletes the user with the specified ID.
		/// </summary>
		/// <param name="id">The unique identifier of the user to delete.</param>
		/// <returns>
		/// An <see cref="ApiResponse{UserDetailResponse}"/> indicating the success or failure of the delete operation.
		/// </returns>
		/// <response code="200">User deleted successfully.</response>
		/// <response code="400">The provided user ID is invalid or the delete operation failed.</response>
		/// <response code="401">Unauthorized to perform the delete operation.</response>
		/// <response code="500">An internal server error occurred during the delete process.</response>
		/// 
		[EndpointSummary("Delete user")]
		[EndpointDescription("Deletes the user with the specified ID.")]
		[HttpDelete]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(ApiResponse<UserDetailResponse>), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<UserDetailResponse>>> DeleteUserAsync(Guid id)
		{
			try
			{
				
				if (id == Guid.Empty)
					return BadRequest(ApiResponse<UserDetailResponse>.Fail($"User ID {id} not valid."));


				var result = await _userService.DeleteUserAsync(id);

				if (!result)
					return BadRequest(ApiResponse<UserDetailResponse>.Fail($"User delete request fail with id {id}"));

				return Ok(ApiResponse<bool>.Ok(result, "User deleted successfully"));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<bool>.Fail(ex.Message));
			}
		}

		#endregion
	}
}
