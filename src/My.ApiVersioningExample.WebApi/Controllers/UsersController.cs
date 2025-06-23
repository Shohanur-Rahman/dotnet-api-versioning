using Microsoft.AspNetCore.Mvc;
using My.ApiVersioningExample.Common.Responses;
using My.ApiVersioningExample.Core.Users.DTOs.Response;
using My.ApiVersioningExample.Services.Users.Interfaces;

namespace My.ApiVersioningExample.WebApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		#region Properties and Variables

		private readonly IUserService _userService;
		private readonly ILogger<UsersController> _logger;

		#endregion

		#region Constructors

		public UsersController(IUserService userService, ILogger<UsersController> logger)
		{
			_logger = logger;
			_userService = userService;
		}
		#endregion

		#region Public Endpoints


		/// <summary>
		/// Retrieves all users.
		/// </summary>
		/// <returns>A collection of user detail responses.</returns>
		/// 
		[HttpGet]
		[ProducesResponseType(typeof(ApiResponse<List<UserDetailResponse>>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
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
				return NotFound(ApiResponse<IEnumerable<UserDetailResponse>>.Fail(ex.Message));
			}
		}

		#endregion
	}
}
