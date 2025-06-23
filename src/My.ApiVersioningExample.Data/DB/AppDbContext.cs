using Microsoft.EntityFrameworkCore;
using My.ApiVersioningExample.Core.Users.Entities;

namespace My.ApiVersioningExample.Data.DB
{
	/// <summary>
	/// Represents the application's database context, providing access to entity sets and 
	/// enabling configuration for Entity Framework Core operations.
	/// </summary>
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options) { }

		#region Configuration
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			DataBuilder dataBuilder = new DataBuilder(modelBuilder);
			dataBuilder.BuildData();
		}

		#endregion

		public virtual DbSet<DbUser> Users { get; set; }
		public virtual DbSet<DbAddressBook> UserAddressBooks { get; set; }
	}
}
