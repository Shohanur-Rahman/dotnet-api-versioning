using System.ComponentModel.DataAnnotations;

namespace My.ApiVersioningExample.Core.Users.DTOs.Request
{
	/// <summary>
	/// Represents a request to update or set the URL of a user's profile photo.
	/// Contains the user's unique identifier and an optional photo URL.
	/// </summary>
	public class PhotoUrlRequest
	{
		/// <summary>
		/// Gets or sets the unique identifier for the user.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the URL to the user's profile photo.
		/// </summary>
		[Required]
		public string PhotoUrl { get; set; } = default!;
	}

}
