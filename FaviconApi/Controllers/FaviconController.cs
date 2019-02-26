using System;
using System.Net;
using FaviconApi.Helpers;
using FaviconApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;

namespace FaviconApi.Controllers
{
    [Route("favicon")]
    [ApiController]
    public class FaviconController : ControllerBase
    {
        private IMemoryCache _cache;

        public FaviconController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get the favicon",
            Description = "Get the favicon for the website specified",
            OperationId = "GetFavicon"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "The favicon was returned")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Website has no favicon")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Failed getting the favicon")]
        public IActionResult Get([SwaggerParameter("Url of the website", Required = true)]string url, 
            [SwaggerParameter("Should the favicon be returned as image or base64")]bool base64 = false)
        {
            try
            {
                var faviconModel = _cache.GetOrCreate(url, entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromDays(1);

                    var model = new FaviconModel
                    {
                        WebsiteUrl = url,
                        FaviconUrl = UrlHelper.EnsureAbsoluteUrl(url, FaviconHelper.RetrieveFavicon(url))
                    };

                    if (string.IsNullOrEmpty(model.FaviconUrl) || !UrlHelper.UrlExists(model.FaviconUrl))
                    {
                        model.Data = null;
                        return model;
                    }

                    using (var webClient = new WebClient())
                    {
                        model.Data = webClient.DownloadData(model.FaviconUrl);
                        model.Mimetype = webClient.ResponseHeaders["Content-Type"];
                    }

                    return model;
                });

                if (faviconModel.Data == null) return NoContent();

                if (base64)
                {
                    return Ok($"data:{faviconModel.Mimetype};base64,{Convert.ToBase64String(faviconModel.Data)}");
                }

                return File(faviconModel.Data, faviconModel.Mimetype);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{url} | {e.Message} | {e.StackTrace}");
            }
        }
    }
}
