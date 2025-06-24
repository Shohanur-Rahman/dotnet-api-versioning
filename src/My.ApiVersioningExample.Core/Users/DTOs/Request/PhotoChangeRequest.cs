using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace My.ApiVersioningExample.Core.Users.DTOs.Request
{
	/// <summary>
	/// Represents a request to change a photo, including the request ID and the new photo file.
	/// </summary>
	public class PhotoChangeRequest
	{
		/// <summary>
		/// Gets or sets the unique identifier for the photo change request.
		/// </summary>
		[Required]
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the uploaded photo file to be used as the new attachment.
		/// This file is required and typically represents the new profile picture or image to update.
		/// </summary>
		public IFormFile? Attachment { get; set; }
	}

}
