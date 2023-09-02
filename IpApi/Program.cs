using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseForwardedHeaders(new ForwardedHeadersOptions()
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();

app.MapGet("/", (HttpContext context) =>
{
    return context.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? context.Connection.RemoteIpAddress?.ToString();
});

app.Run();
