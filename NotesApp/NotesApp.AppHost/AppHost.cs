var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("notepostgres")
    .WithHostPort(5449)
    .WithDataVolume("notesapp-postgres-data-new")
    .WithLifetime(ContainerLifetime.Persistent);

var pgAdmin = builder.AddContainer("notepgadmin", "dpage/pgadmin4:7.8")
    .WithEndpoint(5450, targetPort: 80, name: "pgadmin-endpoint")
    .WithEnvironment("PGADMIN_DEFAULT_EMAIL", "admin@example.com")
    .WithEnvironment("PGADMIN_DEFAULT_PASSWORD", "Admin123!")
    .WithLifetime(ContainerLifetime.Persistent)
    .WaitFor(postgres);

var redis = builder.AddRedis("noteredis")
    .WithHostPort(5451)
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
