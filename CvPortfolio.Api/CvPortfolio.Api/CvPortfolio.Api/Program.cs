using GithubIntegrationService.Options;
using GithubIntegrationService.Services;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<GitHubOptions>(
    builder.Configuration.GetSection("GitHub"));
builder.Services.AddMemoryCache();

builder.Services.AddScoped<GitHubService>();
builder.Services.AddScoped<IGitHubService>(sp =>
{
    var inner = sp.GetRequiredService<GitHubService>();
    var cache = sp.GetRequiredService<IMemoryCache>();
    return new CachedGitHubService(inner, cache);
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
