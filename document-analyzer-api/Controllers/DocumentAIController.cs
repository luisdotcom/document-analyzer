using document_analyzer_api.DTOs;
using document_analyzer_api.Utils;
using Google.Cloud.DocumentAI.V1;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public IActionResult ReadDocument(DocumentAIRequest documentAIRequest)
        {
            if (documentAIRequest.File.FileName.ToLower().EndsWith(".pdf"))
            {
                Document document = DocumentAI.ReadDocument(documentAIRequest);
                string json = JsonConvert.SerializeObject(document);
                return Ok(json);
            }
            else
            {
                return BadRequest("Tipo de documento no válido.");
            }
        }

        /// <summary>
        /// Method to analyze a document with Vertex AI.
        /// </summary>
        /// <param name="vertexAIRequest">Type: DTO. Description: DTO containing the prompt and pdf file.</param>
        /// <returns></returns>
        [HttpPost("AnalyzeDocument")]
        public async Task<IActionResult> AnalyzeDocument(VertexAIRequest vertexAIRequest)
        {
            if (vertexAIRequest.File.FileName.ToLower().EndsWith(".pdf"))
            {
                string result = await VertexAI.AnalyzeDocument(vertexAIRequest);
                return Ok(result);
            }
            else
            {
                return BadRequest("Tipo de documento no válido.");
            }

        }
    }
}
