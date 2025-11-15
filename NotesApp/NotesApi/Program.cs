using Microsoft.EntityFrameworkCore;
using NotesApi.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.AddServiceDefaults();

// builder.AddNpgsqlDbContext<NotesApi.Data.NotesDbContext>();
builder.Services.AddDbContext<NotesApi.Data.NotesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("notes-database")));


    builder.EnrichNpgsqlDbContext<NotesDbContext>();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
