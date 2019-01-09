using FaviconApi.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FaviconApi.Tests
{
    [TestClass]
    public class UrlHelperTests
    {
        [DataRow("https://favicon.sboulema.nl/swagger", "./favicon-32x32.png", "https://favicon.sboulema.nl/swagger/favicon-32x32.png")]
        [DataRow("https://github.com", "https://github.com/fluidicon.png", "https://github.com/fluidicon.png")]
        [DataTestMethod]
        public void EnsureAbsoluteUrlTests(string url, string faviconUrl, string expectedUrl)
        {
            var absoluteUrl = UrlHelper.EnsureAbsoluteUrl(url, faviconUrl);

            Assert.AreEqual(expectedUrl, absoluteUrl);
        }
    }
}
