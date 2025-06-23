using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My.ApiVersioningExample.Core.Users.Entities
{
	/// <summary>
	/// Represents an address entry in the address book associated with a specific user.
	/// </summary>
	public class DbAddressBook
	{
		/// <summary>
		/// Gets or sets the unique identifier for the address entry.
		/// </summary>
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		/// <summary>
		/// Gets or sets the title or label for the address (e.g., "Home", "Office").
		/// </summary>
		[Required, MaxLength(100)]
		public string AddressTitle { get; set; } = default!;

		/// <summary>
		/// Gets or sets the city of the address.
		/// </summary>
		[Required, MaxLength(100)]
		public string City { get; set; } = default!;

		/// <summary>
		/// Gets or sets the state or province of the address (optional).
		/// </summary>
		[MaxLength(100)]
		public string? State { get; set; }

		/// <summary>
		/// Gets or sets the country of the address (optional).
		/// </summary>
		[MaxLength(100)]
		public string? Country { get; set; }

		/// <summary>
		/// Gets or sets the ZIP or postal code of the address (optional).
		/// </summary>
		[MaxLength(10)]
		public string? ZipCode { get; set; }

		/// <summary>
		/// Gets or sets the foreign key referencing the associated user.
		/// </summary>
		[ForeignKey(nameof(User))]
		public Guid UserId { get; set; }

		/// <summary>
		/// Gets or sets the navigation property for the associated user.
		/// </summary>
		public DbUser User { get; set; } = default!;
	}
}
