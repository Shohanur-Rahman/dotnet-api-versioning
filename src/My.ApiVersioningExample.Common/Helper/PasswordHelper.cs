using System.Security.Cryptography;
using System.Text;

namespace My.ApiVersioningExample.Common.Helper
{
	/// <summary>
	/// Provides functionality for securely hashing and verifying passwords using HMACSHA512.
	/// </summary>
	public class PasswordHelper
	{
		/// <summary>
		/// Creates a secure hash and salt for a given plaintext password.
		/// </summary>
		/// <param name="password">The plaintext password to hash.</param>
		/// <param name="passwordHash">The resulting password hash as a byte array (output).</param>
		/// <param name="passwordSalt">The cryptographic salt used to generate the hash (output).</param>
		public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using var hmac = new HMACSHA512();
			passwordSalt = hmac.Key;
			passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
		}

		/// <summary>
		/// Verifies whether a given plaintext password matches a stored hash using the original salt.
		/// </summary>
		/// <param name="password">The plaintext password to verify.</param>
		/// <param name="storedHash">The previously stored password hash.</param>
		/// <param name="storedSalt">The salt that was used to create the stored hash.</param>
		/// <returns>True if the password matches the stored hash; otherwise, false.</returns>
		public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
		{
			using var hmac = new HMACSHA512(storedSalt);
			var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			return computedHash.SequenceEqual(storedHash);
		}
	}

}
