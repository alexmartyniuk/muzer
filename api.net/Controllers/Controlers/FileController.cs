using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Routing;
using MuzerAPI.DtoConvertors;

namespace MuzerAPI.Controlers
{
    public class FileController : ApiController
    {
        private FileService.FileService _fileService;

        public FileController(FileService.FileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        [Route("file/{fileId}")]
        public HttpResponseMessage Get(string fileId)
        {
            var stream = _fileService.Get(fileId);
            if (stream == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);                
            }

            using (MemoryStream ms = new MemoryStream())
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(ms);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };

                result.Content.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = $"{fileId}.mp3"
                    };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                return result;
            }
        }
    }
}
