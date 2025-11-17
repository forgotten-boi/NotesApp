using Microsoft.Extensions.Diagnostics.Latency;
using Notes.Shared.DTOs;
using NotesApi.Data;
using NotesApi.Helpers;

namespace NotesApi.Features.GetNote;
internal static class GetNoteEndpoint
{
    public static async Task<IResult> GetNote(Guid id, NotesDbContext dbContext, IHttpClientFactory httpClientFactory, ILogger<Program> logger, CancellationToken ct)
    {
        try
        {
            var note = await dbContext.Notes.FindAsync(new object[] { id }, ct);

            if (note == null)
            {
                return Results.NotFound();
            }
            var tags = await TagsCommunicators.AnalyzeNoteForTags(new AnalyzeNoteRequest(note.Id, note.Title, note.Content), httpClientFactory, logger, ct);

            var response = new AnalyzeNoteResponse(
                note.Id,
                note.Title,
                note.Content,
                note.CreatedAt,
                tags
            );
        
            return Results.Ok(response);
        }
        catch (System.Exception)
        {
            logger.LogError("Error retrieving note with ID {NoteId}", id);
            return Results.Problem("An error occurred while retrieving the note.");
        }
    }
}