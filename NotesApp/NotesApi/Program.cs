using Microsoft.EntityFrameworkCore;
using Notes.Shared.DTOs;
using NotesApi.Data;
using NotesApi.Features.CreateeNote;
using NotesApi.Features.GetNote;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.AddServiceDefaults();

// builder.AddNpgsqlDbContext<NotesApi.Data.NotesDbContext>();
builder.Services.AddDbContext<NotesApi.Data.NotesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("notes-database")));

builder.Services.AddHttpClient("TagsApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["TagsApi:BaseUrl"] ?? "https+http://tags-api");
});

builder.EnrichNpgsqlDbContext<NotesDbContext>();

builder.Services.AddOpenApi();

var app = builder.Build();

// Log the effective connection string so we can confirm AppHost wiring for debugging.
// This helps diagnose mismatches between the DB connection used by EF and the one used
// by any automatically-added database health checks.
var loggerFactory = app.Services.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();
var startupLogger = loggerFactory.CreateLogger("Startup");
startupLogger.LogInformation("notes-database connection string: {connection}", builder.Configuration.GetConnectionString("notes-database"));

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<NotesDbContext>();
    dbContext.Database.Migrate();

}

app.UseHttpsRedirection();

app.MapPost("notes", CreateNoteEndpoint.CreateNote);
app.MapGet("notes/{id}", GetNoteEndpoint.GetNote);

app.Run();
