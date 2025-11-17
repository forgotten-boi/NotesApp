using Notes.Shared.DTOs;
using TagsApi.Data;

namespace TagsApi.Features.AnalyzeNote;
internal static class AnalyzeNoteEndpoint
{
    public static async Task<IResult> AnalyzeNote(AnalyzeNoteRequest request,
        TagsDbContext dbContext, ILogger<Program> logger, CancellationToken ct)
    {
        try
        {
            // Simple keyword-based tag analysis (for demonstration purposes)
            // In a real-world scenario, this could involve NLP or ML models
            //TODO: implement real analysis logic
            
            var tags = AnalyzeContentForTags(request.Title, request.Content);

          
            var tagsEntities = tags.Select(t => new Tag
            {
                Id = t.Id,
                Name = t.Name,
                Color = t.Color,
                NoteId = t.NoteId,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList();

            dbContext.Tags.AddRange(tagsEntities);
            await dbContext.SaveChangesAsync(ct);
            var response = new AnalyzeNoteResponse  (
                request.NoteId,
                request.Title,
                request.Content,
                DateTime.UtcNow,
                tags
            );
            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error analyzing note for tags");
            return Results.Problem("An error occurred while analyzing the note for tags.");
        }
    }

    private static List<TagResponse> AnalyzeContentForTags(string title, string content)
    {
        var tags = new List<TagResponse>();
        var combinedContent = $"{title} {content}".ToLowerInvariant();

        var tagKeywords = new Dictionary<string, (string Name, string Color)>
        {
            { "urgent", ("Urgent", "Red") },
            { "important", ("Important", "Blue") },
            { "todo", ("To-Do", "Green") },
            { "idea", ("Idea", "Yellow") },
            { "reminder", ("Reminder", "Orange") },
            { "follow up", ("Follow Up", "Purple")},
            { "work", ("Work", "Cyan")},
            { "personal", ("Personal", "Magenta")},
            { "meeting", ("Meeting", "Brown") },
            { "project", ("Project", "Teal")},
            { "deadline", ("Deadline", "Crimson")},
            { "review", ("Review", "Navy")}


        };

        foreach (var keyword in tagKeywords)
        {
            if (combinedContent.Contains(keyword.Key, StringComparison.OrdinalIgnoreCase))
            {
                tags.Add(new TagResponse(Guid.NewGuid(), keyword.Value.Name, keyword.Value.Color, Guid.Empty, DateTime.UtcNow, null));
            }
        }

        if (tags.Count == 0)
        {
            tags.Add(new TagResponse(Guid.NewGuid(), "General", "Gray", Guid.Empty, DateTime.UtcNow, null));
        }


        return tags;
    }
}