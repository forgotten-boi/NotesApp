// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.Extensions.Configuration;
// using System.IO;

// namespace TagsApi.Data;

// public class TagsDesignTimeDbContextFactory : IDesignTimeDbContextFactory<TagsDbContext>
// {
//     public TagsDbContext CreateDbContext(string[] args)
//     {
//         var config = new ConfigurationBuilder()
//             .SetBasePath(Directory.GetCurrentDirectory())
//             .AddJsonFile("appsettings.json", optional: true)
//             .AddJsonFile("appsettings.Development.json", optional: true)
//             .AddEnvironmentVariables()
//             .Build();

//         var connectionString = config.GetConnectionString("tags-database")
//                                ?? "Host=localhost;Database=tags;Username=postgres;Password=postgres";

//     var optionsBuilder = new DbContextOptionsBuilder<TagsDbContext>();
//         optionsBuilder.UseNpgsql(connectionString);

//         return new TagsDbContext(optionsBuilder.Options);
//     }
// }
