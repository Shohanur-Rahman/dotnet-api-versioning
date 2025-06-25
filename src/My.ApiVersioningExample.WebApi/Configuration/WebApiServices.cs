using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using My.ApiVersioningExample.Common.Responses;
using My.ApiVersioningExample.Data.DB;
using System.Text;
using System.Text.Json;
using My.ApiVersioningExample.WebApi.Utilities;


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

			string[] versions = ["v1", "v2"];
			foreach (var version in versions)
			{
				services.AddOpenApi(version, options =>
				{
					options.AddDocumentTransformer((document, context, cancellationToken) =>
					{
						document.Info = new()
						{
							Title = "REST API Example",
							Description = "This is an example with api project structure, security and API documentation."
						};
						return Task.CompletedTask;
					});
					options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
				});
			}


			//services.AddOpenApi("v1"); ;
			//services.AddOpenApi("v2"); ;

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


			// Add API versioning
			services.AddApiVersioning(options =>
			{
				options.AssumeDefaultVersionWhenUnspecified = true;
				options.DefaultApiVersion = new ApiVersion(1, 0);
				options.ReportApiVersions = true;

				// Use URL segment versioning, query string, or header-based as needed
				options.ApiVersionReader = ApiVersionReader.Combine(
					new QueryStringApiVersionReader("api-version"),
					new HeaderApiVersionReader("X-Version"),
					new UrlSegmentApiVersionReader()
				);
			}).AddApiExplorer(options =>
			{
				options.GroupNameFormat = "'v'VVV";
				options.SubstituteApiVersionInUrl = true;
			});


			// File Upload Service
			services.AddScoped<FileUploadService>();

			#endregion

			return services;
		}
	}
}
