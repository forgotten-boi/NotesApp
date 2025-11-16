var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5449)
    .WithEnvironment("POSTGRES_USER", "postgres")
    .WithEnvironment("POSTGRES_PASSWORD", "postgres")
    .WithDataVolume("notesapp-postgres-data")
    .WithLifetime(ContainerLifetime.Persistent)
    ;

var pgAdmin = builder.AddContainer("pgadmin", "dpage/pgadmin4:7.8")
    .WithEndpoint(50500, name: "pgadmin-endpoint")
    .WithEnvironment("PGADMIN_DEFAULT_EMAIL", "admin@example.com")
    .WithEnvironment("PGADMIN_DEFAULT_PASSWORD", "Admin123!")
    .WithLifetime(ContainerLifetime.Persistent)
    .WaitFor(postgres);
// var postgres = builder.AddPostgres("postgres")
//     .WithHostPort(5449)
//     .WithUserName("postgres")
//     .WithUsername("postgres")
//     .WithPassword("SuperSecret123!")
//     .WithDataVolume("notesapp-postgres-data")
//     .WithLifetime(ContainerLifetime.Persistent);
    // .WithDataVolume("notesapp-postgres-data")
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
