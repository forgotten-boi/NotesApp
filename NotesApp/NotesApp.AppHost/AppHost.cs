var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.NotesApi>("notes-api");
builder.AddProject<Projects.TagsApi>("tags-api");

builder.Build().Run();
