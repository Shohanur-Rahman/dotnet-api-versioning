namespace My.ApiVersioningExample.Core.Users.DTOs.Request
{
	/// <summary>
	/// Represents the data required to update an existing user's information.
	/// Inherits common user properties from <see cref="UserRequestBase"/>.
	/// </summary>
	public class UserUpdateRequest : UserRequestBase
	{
		#region Properties

		/// <summary>
		/// Gets or sets the unique identifier of the user to be updated.
		/// </summary>
		public Guid Id { get; set; }
		#endregion
	}
}
