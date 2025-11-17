using TagsApi.Data;
using Microsoft.EntityFrameworkCore;
using TagsApi.Features.AnalyzeNote;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddServiceDefaults();

builder.Services.AddDbContext<TagsApi.Data.TagsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("tags-database")));

builder.EnrichNpgsqlDbContext<TagsApi.Data.TagsDbContext>();

var app = builder.Build();

// Log the effective connection string so we can confirm AppHost wiring for debugging.
var loggerFactory = app.Services.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();
var startupLogger = loggerFactory.CreateLogger("Startup");
startupLogger.LogInformation("tags-database connection string: {connection}", builder.Configuration.GetConnectionString("tags-database"));

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TagsDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapPost("tags/analyze", AnalyzeNoteEndpoint.AnalyzeNote);


app.Run();

