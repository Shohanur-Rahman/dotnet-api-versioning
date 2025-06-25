using Microsoft.IdentityModel.Tokens;
using My.ApiVersioningExample.Core.Security.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace My.ApiVersioningExample.WebApi.Utilities
{
	/// <summary>
	/// This class is responsible for JWT assistance
	/// </summary>
	public class JWTHelper
	{
		#region Private methods

		/// <summary>
		/// Generates a JSON Web Token (JWT) containing user-specific claims based on the provided <see cref="AuthResponse"/>.
		/// </summary>
		/// <param name="result">An <see cref="AuthResponse"/> object containing the user's identity and profile information.</param>
		/// <returns>A JWT string that includes claims such as user ID, name, email, mobile number, and profile photo URL.</returns>
		/// <remarks>
		/// The token is signed using HMAC SHA-256 and is valid for 30 days. Custom claims like "mobile" and "photoUrl" are added if available.
		/// </remarks>
		public static string GenerateJwtToken(AuthResponse result, IConfiguration configuration)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JWT") ?? "");

			var claims = new List<Claim>
				{
					new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()),
					new Claim(ClaimTypes.Name, result.Name),
					new Claim(ClaimTypes.Email, result.Email)
				};

			if (!string.IsNullOrWhiteSpace(result.Mobile))
			{
				claims.Add(new Claim("mobile", result.Mobile));
			}

			if (!string.IsNullOrWhiteSpace(result.PhotoUrl))
			{
				claims.Add(new Claim("photoUrl", result.PhotoUrl));
			}

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddDays(30),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(tokenKey),
					SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		#endregion
	}
}
