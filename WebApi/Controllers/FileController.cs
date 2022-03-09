using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.ConfigurationAccessor;

namespace WebApi.Controllers
{
    [Route("[controller]/[action]")]
    public class FileController : Controller
    {

        private readonly IConfigurationAccessor _configurationAccessor;
        public FileController(IConfigurationAccessor configurationAccessor)
        {
            _configurationAccessor = configurationAccessor;
        }

        [HttpGet]
        public IActionResult DownloadPdf([FromQuery] string filePath)
        {
            var sourcePath = Path.Combine(_configurationAccessor.GetIoPaths().Root, filePath.Remove(0, 7));
            //byte[] stream = await System.IO.File.ReadAllBytesAsync(sourcePath);
            //string mimeType = "application/pdf";
            //_httpContextAccessor.HttpContext.Response.Headers.Add("content-disposition", "inline;filename=" + filePath);
            //return File(stream, mimeType);
            var stream = new FileStream(sourcePath, FileMode.Open);
            this.HttpContext.Response.Headers.Add("content-disposition", "inline;filename=" + filePath);
          
            return new FileStreamResult(stream, "application/pdf");
        }
    }
}
