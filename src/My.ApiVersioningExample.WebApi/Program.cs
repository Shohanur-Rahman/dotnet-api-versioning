using My.ApiVersioningExample.WebApi.Configuration;
using My.ApiVersioningExample.Services.Configurations;
using My.ApiVersioningExample.Data.Configurations;
using My.ApiVersioningExample.Services.Configurations.V2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddWebApiDependency(builder)
	.AddServiceDependency()
	.AddRepositoryDependency();

builder.Services.AddV2ServiceDependency();

var app = builder.Build();
app.ConfigApplication();
