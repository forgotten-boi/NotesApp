namespace Notes.Shared.DTOs;
public record TagResponse(Guid Id, string Name, string Color, Guid NoteId, DateTime CreatedAt, DateTime? UpdatedAt);