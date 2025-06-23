using System.ComponentModel.DataAnnotations;

namespace My.ApiVersioningExample.Core.Users.DTOs.Request
{
	/// <summary>
	/// Represents the base class for user-related request models, containing common user properties.
	/// </summary>
	public class UserRequestBase
	{
		#region Properties

		/// <summary>
		/// Gets or sets the full name of the user.
		/// </summary>
		[Required, MaxLength(100)]
		public string Name { get; set; } = default!;

		/// <summary>
		/// Gets or sets the email address of the user.
		/// </summary>
		[Required, MaxLength(100), EmailAddress]
		public string Email { get; set; } = default!;

		/// <summary>
		/// Gets or sets the mobile phone number of the user (optional).
		/// </summary>
		[MaxLength(20)]
		public string? Mobile { get; set; }

		#endregion
	}

}

