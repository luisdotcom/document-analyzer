namespace document_analyzer_api.DTOs
{
    public class VertexAIRequest
    {
        public string Prompt { get; set; }
        public string Base64String { get; set; }
    }
}