// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.Extensions.Configuration;
// using System.IO;

// namespace NotesApi.Data;

// public class NotesDesignTimeDbContextFactory : IDesignTimeDbContextFactory<NotesDbContext>
// {
//     public NotesDbContext CreateDbContext(string[] args)
//     {
//         var config = new ConfigurationBuilder()
//             .SetBasePath(Directory.GetCurrentDirectory())
//             .AddJsonFile("appsettings.json", optional: true)
//             .AddJsonFile("appsettings.Development.json", optional: true)
//             .AddEnvironmentVariables()
//             .Build();

//         var connectionString = config.GetConnectionString("notes-database")
//                                ?? "Host=localhost;Database=notes;Username=postgres;Password=postgres";

//     var optionsBuilder = new DbContextOptionsBuilder<NotesDbContext>();
//         optionsBuilder.UseNpgsql(connectionString);

//         return new NotesDbContext(optionsBuilder.Options);
//     }
// }
