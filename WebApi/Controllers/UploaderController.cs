using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.CommandQueries.Uploader;
using Infrastructure.ConfigurationAccessor;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace WebApi.Controllers
{
    public class UploaderController : BaseController
    {
        private readonly IConfigurationAccessor _configurationAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UploaderController(IConfigurationAccessor configurationAccessor ,IHttpContextAccessor httpContextAccessor)
        {
            _configurationAccessor = configurationAccessor;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] CreateUploaderCommand model) => Ok(ServiceResult<string>.Set(await Mediator.Send(model)));

        [AllowAnonymous]
        [HttpGet] 
        public async Task<IActionResult> DownloadPdf([FromQuery] string filePath)
        {
            var sourcePath = Path.Combine(_configurationAccessor.GetIoPaths().Root, filePath.Remove(0, 7));
            //byte[] stream = await System.IO.File.ReadAllBytesAsync(sourcePath);
            //string mimeType = "application/pdf";
            //_httpContextAccessor.HttpContext.Response.Headers.Add("content-disposition", "inline;filename=" + filePath);
            //return File(stream, mimeType);
            var stream = new FileStream(sourcePath, FileMode.Open);
            _httpContextAccessor.HttpContext.Response.Headers.Add("content-disposition", "inline;filename=" + filePath);
      
            return new FileStreamResult(stream, "application/pdf");
        }

    }
}
