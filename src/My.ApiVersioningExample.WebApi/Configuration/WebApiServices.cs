using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using My.ApiVersioningExample.Common.Responses;
using My.ApiVersioningExample.Data.DB;
using My.ApiVersioningExample.WebApi.Urilities;
using System.Text;
using System.Text.Json;

namespace My.ApiVersioningExample.WebApi.Configuration
{
	/// <summary>
	/// Provides extension methods for registering application-specific services and dependencies.
	/// </summary>
	public static class WebApiServices
	{
		/// <summary>
		/// Registers application services and third-party dependencies into the service collection.
		/// </summary>
		/// <param name="services">The service collection to which services are added.</param>
		/// <returns>The updated <see cref="IServiceCollection"/> instance with registered services.</returns>
		public static IServiceCollection AddWebApiDependency(this IServiceCollection services, WebApplicationBuilder builder)
		{
			#region User Defined Services


			services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).ConfigureWarnings(warnings =>
						   warnings.Ignore(RelationalEventId.PendingModelChangesWarning)); ;

				#if DEBUG
				options.EnableSensitiveDataLogging(); 
				#endif
				options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
			}, ServiceLifetime.Transient);


			// Configure lowercase URLs
			builder.Services.AddRouting(options =>
			{
				options.LowercaseUrls = true;
				options.LowercaseQueryStrings = true; // Optional
			});


			services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			services.AddOpenApi(options =>
			{
				options.AddDocumentTransformer((document, context, cancellationToken) =>
				{
					document.Info = new()
					{
						Title = "REST API Example",
						Version = "v1",
						Description = "This is an example with api project structure, security and API documentation."
					};
					return Task.CompletedTask;
				});
				options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
			});

			services.AddAuthentication(options =>
			 {
				 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				 options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			 })
			.AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT"] ?? ""))
				};
				options.Events = new JwtBearerEvents
				{
					OnChallenge = context =>
					{
						context.HandleResponse();

						var response = new ApiResponse<string>("You are not authorized");
						var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
						{
							PropertyNamingPolicy = JsonNamingPolicy.CamelCase
						});

						context.Response.ContentType = "application/json";
						context.Response.StatusCode = StatusCodes.Status401Unauthorized;

						return context.Response.WriteAsync(json);
					}
				};
			});

			// File Upload Service
			services.AddScoped<FileUploadService>();
			
			#endregion

			return services;
		}
	}
}
