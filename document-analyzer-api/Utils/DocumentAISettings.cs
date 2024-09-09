namespace document_analyzer_api.Utils
{
    public static class DocumentAISettings
    {
        public static string ProjectId { get; set; } = "";
        public static string LocationId { get; set; } = "";
        public static string CredentialsPath { get; set; } = "";
        public static string MimeType { get; set; } = "";
        public static string OCR { get; set; } = "";
        public static string INVOICE { get; set; } = "";
        public static string EXPENSE { get; set; } = "";
        public static string CUSTOM { get; set; } = "";
    }
}