using System.ComponentModel.DataAnnotations;

namespace My.ApiVersioningExample.Core.Security.Request
{
	/// <summary>
	/// This class is responsible for user signin request
	/// </summary>
	public class SignInRequest
	{
		/// <summary>
		/// Gets or sets the email address of the user.
		/// This field is required, must be a valid email format, and can have a maximum length of 100 characters.
		/// </summary>
		[Required, MaxLength(100), EmailAddress]
		public string Email { get; set; } = default!;

		/// <summary>
		/// Gets or sets the password for the user account.
		/// This field is required.
		/// </summary>
		[Required]
		public string Password { get; set; } = default!;
	}
}
