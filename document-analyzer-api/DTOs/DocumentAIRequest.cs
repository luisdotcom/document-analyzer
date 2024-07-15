namespace document_analyzer_api.DTOs
{
    public class DocumentAIRequest
    {
        public string ModelName { get; set; }
        public IFormFile File { get; set; }
    }
}
