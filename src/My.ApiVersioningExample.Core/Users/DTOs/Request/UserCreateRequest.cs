using System.ComponentModel.DataAnnotations;

namespace My.ApiVersioningExample.Core.Users.DTOs.Request
{
	/// <summary>
	/// Represents the data required to create a new user, including credentials and personal information.
	/// Inherits common user properties from <see cref="UserRequestBase"/>.
	/// </summary>
	public class UserCreateRequest : UserRequestBase
	{
		#region Properties
		/// <summary>
		/// Gets or sets the password for the new user as a byte array.
		/// </summary>
		[Required]
		public byte[] Password { get; set; } = default!;
		#endregion
	}

}
