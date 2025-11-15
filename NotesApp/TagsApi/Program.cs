using TagsApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddServiceDefaults();

builder.Services.AddDbContext<TagsApi.Data.TagsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("tags-database")));

builder.EnrichNpgsqlDbContext<TagsApi.Data.TagsDbContext>();

var app = builder.Build();

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


app.Run();

