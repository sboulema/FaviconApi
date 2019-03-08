using System;
using System.Net;

namespace FaviconApi.Helpers
{
    public static class UrlHelper
    {
        public static string EnsureAbsoluteUrl(string url, string faviconUrl)
        {
            if (string.IsNullOrEmpty(faviconUrl)) return string.Empty;

            if (faviconUrl.StartsWith("//"))
            {
                return faviconUrl.Replace("//", "https://");
            }

            if (!IsAbsoluteUrl(faviconUrl))
            {
                if (faviconUrl.StartsWith("./"))
                {
                    if (!url.EndsWith("/"))
                    {
                        url = url + "/";
                    }

                    faviconUrl = faviconUrl.Replace("./", string.Empty);
                }

                if (faviconUrl.StartsWith("/"))
                {
                    return new Uri(GetUri(url), faviconUrl).AbsoluteUri;
                }

                return new Uri(faviconUrl, UriKind.Relative).ToAbsolute(url);
            }

            return faviconUrl;
        }

        public static string ToAbsolute(this Uri uri, string baseUrl)
        {
            if (uri == null) return string.Empty;

            return uri.ToAbsolute(GetUri(baseUrl));
        }

        public static string ToAbsolute(this Uri uri, Uri baseUri)
        {
            if (uri == null) return string.Empty;

            var relative = uri.ToRelative();

            if (Uri.TryCreate(baseUri, relative, out var absolute))
            {
                return absolute.ToString();
            }

            return uri.IsAbsoluteUri ? uri.ToString() : null;
        }

        public static string ToRelative(this Uri uri)
        {
            if (uri == null) return string.Empty;

            return uri.IsAbsoluteUri ? uri.PathAndQuery : uri.OriginalString;
        }

        // Check if the url exists by checking for a 200 OK response
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

        // Make sure string url has correct format and a http as scheme if missing
        public static Uri GetUri(this string url) 
            => new UriBuilder(url).Uri;

        // Check if the url is absolute eg. starts with http
        private static bool IsAbsoluteUrl(string url) 
            => url.StartsWith("http://") || url.StartsWith("https://");
    }
}
