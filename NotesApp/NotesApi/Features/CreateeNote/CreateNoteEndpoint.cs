

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NotesApi.Data;
using Notes.Shared.DTOs;
using NotesApi.Helpers;

namespace NotesApi.Features.CreateeNote;
internal static class CreateNoteEndpoint
{
    // public record CreateNoteRequest(string Title, string Content);
    // public record CreateNoteResponse(Guid Id, string Title, string Content, DateTime CreatedAt, List<TagResponse> Tags);

    // public record TagResponse(Guid Id, string Name, string Color, Guid NoteId, DateTime CreatedAt, DateTime? UpdatedAt);

    public static async Task<IResult> CreateNote(AnalyzeNoteRequest request, 
            IHttpClientFactory httpClientFactory,
            NotesDbContext dbContext, ILogger<Program> logger, CancellationToken ct)
    {
        try
        {
            
       
        var note = new Note
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Notes.Add(note);
        await dbContext.SaveChangesAsync(ct);

        var analyzeNoteRequest = new AnalyzeNoteRequest(note.Id, note.Title, note.Content);
        var tags = new List<TagResponse>();
        tags = await TagsCommunicators.AnalyzeNoteForTags(analyzeNoteRequest, httpClientFactory,  logger, ct);

        var response = new AnalyzeNoteResponse(
            note.Id,
            note.Title,
            note.Content,
            note.CreatedAt,
            tags
        );

        return Results.Created($"/notes/{note.Id}", response);
         }
        catch (System.Exception)
        {
            logger.LogError("Error creating note");
            return Results.Problem("An error occurred while creating the note.");
        }
    }


    // public static IEndpointRouteBuilder MapCreateNoteEndpoint(this IEndpointRouteBuilder endpoints)
    // {
    //     endpoints.MapPost("/notes", async (CreateNoteRequest request, NotesDbContext dbContext, CancellationToken ct) =>
    //     {
    //         var note = new Note
    //         {
    //             Id = Guid.NewGuid(),
    //             Title = request.Title,
    //             Content = request.Content,
    //             CreatedAt = DateTime.UtcNow
    //         };

    //         dbContext.Notes.Add(note);
    //         await dbContext.SaveChangesAsync(ct);

    //         return Results.Created($"/notes/{note.Id}", note);
    //     })
    //     .WithTags("Notes")
    //     .WithSummary("Create a new note")
    //     .WithDescription("Creates a new note with the provided title and content.")
    //     .Produces<Note>(StatusCodes.Status201Created)
    //     .ProducesProblem(StatusCodes.Status400BadRequest);

    //     return endpoints;
    // }
}