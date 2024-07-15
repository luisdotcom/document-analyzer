using document_analyzer_api.Utils;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;

string[] documentAISettings = ["ProjectId", "LocationId", "MimeType", "OCR", "INVOICE", "EXPENSE", "CUSTOM"];
foreach (string setting in documentAISettings)
{
    typeof(DocumentAISettings).GetProperty(setting).SetValue(null, configuration.GetValue<string>($"DocumentAISettings:{setting}"));
}

string[] vertexAISettings = ["ProjectId", "LocationId", "Publisher", "Model", "Bucket", "CredentialsPath"];
foreach (string setting in vertexAISettings)
{
    typeof(VertexAISettings).GetProperty(setting).SetValue(null, configuration.GetValue<string>($"VertexAISettings:{setting}"));
}

WebApplication app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();