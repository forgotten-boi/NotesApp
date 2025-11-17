
    
    namespace NotesApi.Helpers;
    using Notes.Shared.DTOs;

    // Helper class to analyze tags for notes
    // TODO: Update producer and consumer methods
    internal static class TagsCommunicators
    {
        internal static async Task<List<TagResponse>> AnalyzeNoteForTags(AnalyzeNoteRequest request, 
            IHttpClientFactory httpClientFactory, ILogger<Program> logger, CancellationToken ct)
        {
            try
            {
                var client = httpClientFactory.CreateClient("TagsApiClient");

                var response = await client.PostAsJsonAsync("/tags/analyze", request, ct);
                response.EnsureSuccessStatusCode();

                var tags = await response.Content.ReadFromJsonAsync<AnalyzeNoteResponse>(cancellationToken: ct);
                return tags?.Tags ?? new List<TagResponse>();
            }
            catch (System.Exception)
            {
                logger.LogError("Error analyzing note for tags");
                return new List<TagResponse>();
            }
        }
    }