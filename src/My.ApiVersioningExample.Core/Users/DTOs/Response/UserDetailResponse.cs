namespace My.ApiVersioningExample.Core.Users.DTOs.Response
{
	public class UserDetailResponse
	{
		/// <summary>
		/// Gets or sets the unique identifier for the user.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the full name of the user.
		/// </summary>
		public string Name { get; set; } = default!;

		/// <summary>
		/// Gets or sets the email address of the user.
		/// </summary>
		public string Email { get; set; } = default!;

		/// <summary>
		/// Gets or sets the user's mobile phone number (optional).
		/// </summary>
		public string? Mobile { get; set; }

		/// <summary>
		/// Gets or sets the URL to the user's profile photo (optional).
		/// </summary>
		public string? PhotoUrl { get; set; }
	}
}
