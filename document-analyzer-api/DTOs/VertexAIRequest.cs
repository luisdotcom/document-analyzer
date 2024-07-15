namespace document_analyzer_api.DTOs
{
    public class VertexAIRequest
    {
        public string Prompt { get; set; }
        public IFormFile File { get; set; }
    }
}