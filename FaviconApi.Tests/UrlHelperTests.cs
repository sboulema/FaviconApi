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
        [DataRow("nu.nl", "/static/favicon.ico", "http://nu.nl/static/favicon.ico")]
        [DataTestMethod]
        public void EnsureAbsoluteUrlTests(string url, string faviconUrl, string expectedUrl)
        {
            var absoluteUrl = UrlHelper.EnsureAbsoluteUrl(url, faviconUrl);

            Assert.AreEqual(expectedUrl, absoluteUrl);
        }
    }
}
