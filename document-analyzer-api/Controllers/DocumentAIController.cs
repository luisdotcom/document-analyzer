using document_analyzer_api.DTOs;
using document_analyzer_api.Utils;
using Google.Cloud.DocumentAI.V1;
using Microsoft.AspNetCore.Mvc;

namespace document_analyzer_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentAIController : ControllerBase
    {
        /// <summary>
        /// Method to read a document with Document AI.
        /// </summary>
        /// <param name="documentAIRequest">Type: DTO. Description: DTO containing the model name and pdf file.</param>
        /// <returns></returns>
        [HttpPost("ReadDocument")]
        public IActionResult ReadDocument([FromBody] DocumentAIRequest documentAIRequest)
        {
            Document document = DocumentAI.ReadDocument(documentAIRequest);
            return Ok(document.Text);
        }

        /// <summary>
        /// Method to analyze a document with Vertex AI.
        /// </summary>
        /// <param name="vertexAIRequest">Type: DTO. Description: DTO containing the prompt and pdf file.</param>
        /// <returns></returns>
        [HttpPost("AnalyzeDocument")]
        public async Task<IActionResult> AnalyzeDocument([FromBody] VertexAIRequest vertexAIRequest)
        {
            string result = await VertexAI.AnalyzeDocument(vertexAIRequest);
            return Ok(result);
        }
    }
}
