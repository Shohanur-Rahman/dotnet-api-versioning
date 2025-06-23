using Microsoft.EntityFrameworkCore;
using My.ApiVersioningExample.Common.Helper;
using My.ApiVersioningExample.Core.Users.Entities;

namespace My.ApiVersioningExample.Data.DB
{
	public class DataBuilder
	{
		#region "Constructors #
		/// <summary>
		/// Inject Model Builder
		/// </summary>
		private ModelBuilder modelBuilder;
		public DataBuilder(ModelBuilder modelBuilder)
		{
			this.modelBuilder = modelBuilder;
		}

		#endregion

		#region Public Methods
		/// <summary>
		/// Build demo data
		/// </summary>
		public void BuildData()
		{
			this.SetUserDemoData();
		}

		#endregion

		#region Private Methods
		/// <summary>
		/// Setup few demo user insert schema
		/// </summary>
		private void SetUserDemoData()
		{
			string password = "Password123";
			PasswordHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

			this.modelBuilder.Entity<DbUser>().HasData(
				new DbUser
				{
					Name = "John Doe",
					Email = "john@gmail.com",
					Mobile = "+9108765412",
					PasswordHash = passwordHash,
					PasswordSalt = passwordSalt,
					PhotoUrl = "locastorage/users/john.jpg"
				},
				new DbUser
				{
					Name = "Alex Hels",
					Email = "alex@gmail.com",
					Mobile = "+9108765411",
					PasswordHash = passwordHash,
					PasswordSalt = passwordSalt,
					PhotoUrl = "locastorage/users/alex.jpg"
				}
			);
		}

		#endregion
	}
}
