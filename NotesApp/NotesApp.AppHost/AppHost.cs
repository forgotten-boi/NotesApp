var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5432)
    .WithDataVolume("notesapp-postgres-data")
    .WithLifetime(ContainerLifetime.Persistent);
var redis = builder.AddRedis("redis")
    .WithHostPort(6379)
    .WithDataVolume("notesapp-redis-data")
    .WithLifetime(ContainerLifetime.Persistent);

var notesDatabase = postgres.AddDatabase("notes-database");
var tagsDatabase = postgres.AddDatabase("tags-database");

var tagsApi = builder.AddProject<Projects.TagsApi>("tags-api")
    .WithReference(tagsDatabase)
    .WithReference(redis)
    .WaitFor(tagsDatabase)
    .WaitFor(redis);

var notesApi = builder.AddProject<Projects.NotesApi>("notes-api")
    .WithHttpsEndpoint(5002, name: "notes-api-endpoint")
    .WithReference(notesDatabase)
    .WithReference(tagsApi)
    .WithReference(redis)
    .WaitFor(notesDatabase)
    .WaitFor(redis)
    .WaitFor(tagsApi);

builder.Build().Run();
