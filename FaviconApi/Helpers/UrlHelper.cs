using System;
using System.Net;

namespace FaviconApi.Helpers
{
    public static class UrlHelper
    {
        public static string EnsureAbsoluteUrl(string url, string faviconUrl)
        {
            if (faviconUrl.StartsWith("/"))
            {
                return new Uri(faviconUrl, UriKind.Relative).ToAbsolute(url);
            }

            return faviconUrl;
        }

        public static string ToAbsolute(this Uri uri, string baseUrl)
        {
            // Null-checks

            var baseUri = new Uri(baseUrl);

            return uri.ToAbsolute(baseUri);
        }

        public static string ToAbsolute(this Uri uri, Uri baseUri)
        {
            // Null-checks

            var relative = uri.ToRelative();

            if (Uri.TryCreate(baseUri, relative, out var absolute))
            {
                return absolute.ToString();
            }

            return uri.IsAbsoluteUri ? uri.ToString() : null;
        }

        public static string ToRelative(this Uri uri)
        {
            // Null-check

            return uri.IsAbsoluteUri ? uri.PathAndQuery : uri.OriginalString;
        }

        public static bool UrlExists(string url)
        {
            try
            {
                var webRequest = WebRequest.Create(url);
                webRequest.Method = "HEAD";
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                return webResponse.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
