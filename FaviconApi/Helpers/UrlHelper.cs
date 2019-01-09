using System;
using System.Net;

namespace FaviconApi.Helpers
{
    public static class UrlHelper
    {
        public static string EnsureAbsoluteUrl(string url, string faviconUrl)
        {
            if (string.IsNullOrEmpty(faviconUrl)) return string.Empty;

            if (!IsAbsoluteUrl(faviconUrl))
            {
                if (faviconUrl.StartsWith("./"))
                {
                    faviconUrl = faviconUrl.Replace("./", string.Empty);
                }

                if (!url.EndsWith("/"))
                {
                    url = url + "/";
                }

                return new Uri(faviconUrl, UriKind.Relative).ToAbsolute(url);
            }

            return faviconUrl;
        }

        public static string ToAbsolute(this Uri uri, string baseUrl)
        {
            if (uri == null) return string.Empty;

            var baseUri = new Uri(baseUrl);

            return uri.ToAbsolute(baseUri);
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
        {
            return new UriBuilder(url).Uri;
        }

        private static bool IsAbsoluteUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }
    }
}
