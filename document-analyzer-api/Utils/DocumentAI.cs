using document_analyzer_api.DTOs;
using Google.Cloud.DocumentAI.V1;
using Google.Protobuf;

namespace document_analyzer_api.Utils
{
    public static class DocumentAI
    {
        public static Document ReadDocument(DocumentAIRequest documentAIRequest)
        {
            DocumentProcessorServiceClient client = new DocumentProcessorServiceClientBuilder()
            {
                Endpoint = $"{DocumentAISettings.LocationId}-documentai.googleapis.com",
                CredentialsPath = DocumentAISettings.CredentialsPath
            }.Build();

            RawDocument rawDocument = new()
            {
                Content = ByteString.FromBase64(documentAIRequest.Base64String),
                MimeType = DocumentAISettings.MimeType
            };

            string processorId = typeof(DocumentAISettings).GetProperty(documentAIRequest.ModelName.ToUpper())?.GetValue(null, null).ToString();

            ProcessRequest request = new()
            {
                Name = ProcessorName.FromProjectLocationProcessor(DocumentAISettings.ProjectId, DocumentAISettings.LocationId, processorId).ToString(),
                RawDocument = rawDocument
            };

            ProcessResponse response = client.ProcessDocument(request);

            return response.Document;
        }
    }
}
