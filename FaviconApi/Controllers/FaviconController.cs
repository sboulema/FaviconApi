using System;
using System.Net;
using FaviconApi.Helpers;
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
    }
}
