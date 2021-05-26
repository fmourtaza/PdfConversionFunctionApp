using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace PdfConversionFunctionApp
{
    public class ConvertToPdf
    {
        private readonly FileService _fileService;
        private readonly ApiConfig _apiConfig;

        public ConvertToPdf(FileService fileService, ApiConfig apiConfig)
        {
            _fileService = fileService;
            _apiConfig = apiConfig;
        }

        [FunctionName("ConvertToPdf")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("ConvertToPdf - called");

            if (req.Headers.ContentLength == 0)
            {
                log.LogInformation("Please provide a file.");
                return new BadRequestObjectResult("Please provide a file.");
            }

            var path = $"{_apiConfig.GraphEndpoint}sites/{_apiConfig.SiteId}/drive/items/";
            log.LogInformation("ConvertToPdf - path:" + path);

            var fileId = await _fileService.UploadStreamAsync(path, req.Body, req.ContentType);
            log.LogInformation("ConvertToPdf - fileId:" + fileId);

            var pdf = await _fileService.DownloadConvertedFileAsync(path, fileId, "pdf");
            log.LogInformation("ConvertToPdf - pdf Length:" + pdf.Length);

            await _fileService.DeleteFileAsync(path, fileId);

            log.LogInformation("ConvertToPdf - Ending");

            return new FileContentResult(pdf, "application/pdf");
        }
    }
}
