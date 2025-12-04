using GithubIntegrationService.Options;
using GithubIntegrationService.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IGitHubService, GitHubService>();

builder.Services.Configure<GitHubOptions>(
    builder.Configuration.GetSection("GitHub"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
