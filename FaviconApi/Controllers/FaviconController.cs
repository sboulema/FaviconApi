using System;
using System.Net;
using FaviconApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FaviconApi.Controllers
{
    [Route("favicon")]
    [ApiController]
    public class FaviconController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string url, bool base64 = false)
        {
            var faviconUrl = FaviconHelper.RetrieveFavicon(url);
            faviconUrl = UrlHelper.EnsureAbsoluteUrl(url, faviconUrl);

            if (string.IsNullOrEmpty(faviconUrl)) return Ok();

            try
            {
                using (var webClient = new WebClient())
                {
                    var data = webClient.DownloadData(faviconUrl);
                    var mimeType = webClient.ResponseHeaders["Content-Type"];

                    if (base64)
                    {
                        return Ok($"data:{mimeType};base64,{Convert.ToBase64String(data)}");
                    }

                    return File(data, mimeType);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{url} | {faviconUrl} | {e.Message} | {e.StackTrace}");
            }
        }
    }
}
