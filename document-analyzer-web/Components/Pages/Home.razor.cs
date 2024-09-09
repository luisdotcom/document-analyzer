using document_analyzer_api.DTOs;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace document_analyzer_web.Components.Pages
{
    public partial class Home
    {
        private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
        private string dragClass = DefaultDragClass;
        private bool readDocument = true;
        private bool analyzeDocument = false;
        private bool ReadDocument
        {
            get => readDocument;
            set
            {
                readDocument = value;
                analyzeDocument = !value;
                Prompt = "";
                Result = "";
            }
        }
        private bool AnalyzeDocument
        {
            get => analyzeDocument;
            set
            {
                analyzeDocument = value;
                readDocument = !value;
                Prompt = "";
                Result = "";
            }
        }
        private string Prompt { get; set; } = "";
        private string Model { get; set; } = "OCR";

        private readonly List<string> fileNames = [];
        private MudFileUpload<IReadOnlyList<IBrowserFile>>? fileUpload;
        private MudTextField<string> multilineReference;
        private string src = "";
        private string Result { get; set; } = "";
        private bool Waiting { get; set; } = false;

        protected override void OnInitialized()
        {
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
        }

        private void SetDragClass() => dragClass = $"{DefaultDragClass} mud-border-primary";
        private void ClearDragClass() => dragClass = DefaultDragClass;
        private async Task ClearAsync()
        {
            await (fileUpload?.ClearAsync() ?? Task.CompletedTask);
            fileNames.Clear();
            src = "";
            Result = "";
            StateHasChanged();
            ClearDragClass();
        }

        private Task OpenFilePickerAsync() => fileUpload?.OpenFilePickerAsync() ?? Task.CompletedTask;
        private async void OnInputFileChanged(InputFileChangeEventArgs e)
        {
            try
            {
                await ClearAsync();
                var files = e.GetMultipleFiles();
                if (files[0].Name.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) == false)
                {
                    Snackbar.Add("Please upload a PDF file", Severity.Warning);
                    return;
                }

                fileNames.Add(files[0].Name);
                long maxFileSize = 10 * 1024 * 1024;
                using var memoryStream = new MemoryStream();
                await files[0].OpenReadStream(maxFileSize).CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(fileBytes);
                src = $"data:application/pdf;base64,{base64String}";
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{ex.Message}", Severity.Warning);
                await ClearAsync();
            }
        }

        private async Task Analyze()
        {
            Result = "";
            Waiting = true;
            try
            {
                using HttpClient client = new();
                string url = Configuration["ApiSettings:BaseUrl"]!;

                if (ReadDocument)
                {
                    DocumentAIRequest documentAIRequest = new()
                    {
                        Base64String = src.Split(',')[1],
                        ModelName = Model
                    };

                    HttpResponseMessage response = await client.PostAsJsonAsync($"{url}ReadDocument", documentAIRequest);
                    string responseText = await response.Content.ReadAsStringAsync();
                    Result = responseText;
                }
                else
                {
                    VertexAIRequest vertexAIRequest = new()
                    {
                        Base64String = src.Split(',')[1],
                        Prompt = Prompt
                    };

                    HttpResponseMessage response = await client.PostAsJsonAsync($"{url}AnalyzeDocument", vertexAIRequest);
                    string responseText = await response.Content.ReadAsStringAsync();
                    Result = responseText;
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{ex.Message}", Severity.Error);
                await ClearAsync();
            }
            Waiting = false;
        }
    }
}