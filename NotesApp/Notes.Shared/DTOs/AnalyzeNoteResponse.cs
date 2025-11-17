namespace Notes.Shared.DTOs;
public record AnalyzeNoteResponse(Guid Id, string Title, string Content, DateTime CreatedAt, List<TagResponse> Tags);