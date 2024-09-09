using document_analyzer_api.DTOs;
using Google.Cloud.AIPlatform.V1;
using Google.Cloud.Storage.V1;

namespace document_analyzer_api.Utils
{
    public static class VertexAI
    {
        public static async Task<string> AnalyzeDocument(VertexAIRequest vertexAIRequest)
        {
            bool uploaded = await UploadFile(vertexAIRequest.Base64String);
            if (uploaded)
            {
                PredictionServiceClient predictionServiceClient = new PredictionServiceClientBuilder
                {
                    Endpoint = $"{VertexAISettings.LocationId}-aiplatform.googleapis.com"
                }.Build();

                GenerateContentRequest generateContentRequest = new()
                {
                    Model = $"projects/{VertexAISettings.ProjectId}/locations/{VertexAISettings.LocationId}/publishers/{VertexAISettings.Publisher}/models/{VertexAISettings.Model}",
                    Contents =
                    {
                        new Content
                        {
                            Role = "USER",
                            Parts =
                            {
                                new Part { Text = vertexAIRequest.Prompt },
                                new Part { FileData = new() { MimeType =  VertexAISettings.MimeType, FileUri = "gs://document-analyzer/Document.pdf"} }
                            }
                        }
                    }
                };

                GenerateContentResponse response = await predictionServiceClient.GenerateContentAsync(generateContentRequest);
                string responseText = response.Candidates[0].Content.Parts[0].Text;
                return responseText.ToString();
            }
            else
            {
                return "No se pudo subir el documento.";
            }
        }
        private static async Task<bool> UploadFile(string Base64String)
        {
            var storage = StorageClient.Create();
            byte[] bytes = Convert.FromBase64String(Base64String);
            using var fileStream = new MemoryStream(bytes);
            await storage.UploadObjectAsync(VertexAISettings.Bucket, "Document.pdf", VertexAISettings.MimeType, fileStream);

            var obj = await storage.GetObjectAsync(VertexAISettings.Bucket, "Document.pdf");
            return obj != null;
        }
    }
}
