using FaviconApi.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FaviconApi.Tests
{
    [TestClass]
    public class UrlHelperTests
    {
        [DataRow("https://favicon.sboulema.nl/swagger", "./favicon-32x32.png", "https://favicon.sboulema.nl/swagger/favicon-32x32.png")]
        [DataRow("https://github.com", "https://github.com/fluidicon.png", "https://github.com/fluidicon.png")]
        [DataRow("https://gijgo.com", "", "")]
        [DataRow("https://httpbin.org", "/static/favicon.ico", "https://httpbin.org/static/favicon.ico")]
        [DataRow("nu.nl", "/static/img/atoms/images/favicon/apple-touch-icon-57x57.png", "http://nu.nl/static/img/atoms/images/favicon/apple-touch-icon-57x57.png")]
        [DataRow("http://http-prompt.com/", "/favicon.ico", "http://http-prompt.com/favicon.ico")]
        [DataRow("https://gitter.im/eliangcs/http-prompt", "//cdn03.gitter.im/_s/66fc778/images/favicon-normal.ico", "https://cdn03.gitter.im/_s/66fc778/images/favicon-normal.ico")]
        [DataRow("https://socket.io/get-started/chat/", "/images/favicon.png", "https://socket.io/images/favicon.png")]
        [DataRow("https://www.mztools.com/articles/2013/MZ2013019.aspx", "../../images/apple-touch-icon.png", "https://www.mztools.com/images/apple-touch-icon.png")]
        [DataTestMethod]
        public void EnsureAbsoluteUrlTests(string url, string faviconUrl, string expectedUrl)
        {
            var absoluteUrl = UrlHelper.EnsureAbsoluteUrl(url, faviconUrl);

            Assert.AreEqual(expectedUrl, absoluteUrl);
        }
    }
}
