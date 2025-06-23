using My.ApiVersioningExample.WebApi.Configuration;
using My.ApiVersioningExample.Services.Configurations;
using My.ApiVersioningExample.Data.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddWebApiDependency(builder)
	.AddServiceDependency()
	.AddRepositoryDependency();

var app = builder.Build();
app.ConfigApplication();
