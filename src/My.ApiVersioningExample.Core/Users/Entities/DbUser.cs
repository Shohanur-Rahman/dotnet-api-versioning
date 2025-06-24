using System.ComponentModel.DataAnnotations;

namespace My.ApiVersioningExample.Core.Users.Entities
{
	/// <summary>
	/// Represents a user entity in the database, including authentication and profile information.
	/// </summary>
	public class DbUser
	{
		/// <summary>
		/// Gets or sets the unique identifier for the user.
		/// </summary>
		[Key]
		public Guid Id { get; set; }

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
		/// Gets or sets the user's mobile phone number.
		/// </summary>
		[Required, MaxLength(20)]
		public string? Mobile { get; set; } = default!;

		/// <summary>
		/// Gets or sets the hashed password of the user.
		/// </summary>
		[Required]
		public byte[] PasswordHash { get; set; } = default!;

		/// <summary>
		/// Gets or sets the salt used to hash the user's password.
		/// </summary>
		[Required]
		public byte[] PasswordSalt { get; set; } = default!;

		/// <summary>
		/// Gets or sets the URL to the user's profile photo (optional).
		/// </summary>
		public string? PhotoUrl { get; set; }
	}

}
