using HtmlAgilityPack;
using System;
using System.Linq;

namespace FaviconApi.Helpers
{
    public static class FaviconHelper
    {
        public static string RetrieveFavicon(string url)
        {
            string returnFavicon = null;

            var htmlDocument = new HtmlWeb().Load(url.GetUri());

            var elementsAppleTouchIcon = htmlDocument.DocumentNode.SelectNodes("//link[contains(@rel, 'apple-touch-icon')]");
            if (elementsAppleTouchIcon != null && elementsAppleTouchIcon.Any())
            {
                var favicon = elementsAppleTouchIcon.First();
                returnFavicon = favicon.GetAttributeValue("href", null);
                if (!string.IsNullOrWhiteSpace(returnFavicon))
                {
                    return returnFavicon;
                }
            }

            var elements = htmlDocument.DocumentNode.SelectNodes("//link[contains(@rel, 'icon')]");
            if (elements != null && elements.Any())
            {
                var favicon = elements.First();
                returnFavicon = favicon.GetAttributeValue("href", null);
                if (!string.IsNullOrWhiteSpace(returnFavicon))
                {
                    return returnFavicon;
                }
            }

            var uri = new Uri(url);
            if (uri.HostNameType == UriHostNameType.Dns)
            {
                returnFavicon = string.Format("{0}://{1}/favicon.ico", uri.Scheme == "https" ? "https" : "http", uri.Host);
                if (UrlHelper.UrlExists(returnFavicon))
                {
                    return returnFavicon;
                }
            }

            return returnFavicon;
        }
    }
}
