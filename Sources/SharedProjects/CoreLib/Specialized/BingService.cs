using System;
using System.Globalization;
using System.Net.Http;
using System.Xml.Linq;
using System.Xml.XPath;
using Mohammad.Specialized.Bing;

namespace Mohammad.Specialized
{
    /// <summary>
    ///     Provides an attached property determining the current Bing image and assigning it to an image or imagebrush.
    /// </summary>
    public static class BingService
    {
        public static Uri GetCurrentBingImageUrl()
        {
            HttpResponseMessage resp;
            LibrarySupervisor.Logger.Debug("Sending request for Bing image");
            using (var client = new HttpClient())
                resp = client.GetAsync("http://www.bing.com/hpimagearchive.aspx?format=xml&idx=0&n=1&mbl=1&mkt=en-ww").Result;
            if (!resp.IsSuccessStatusCode)
            {
                LibrarySupervisor.Logger.Debug($"A failure status code received: {resp.StatusCode}");
                return null;
            }
            LibrarySupervisor.Logger.Debug("Getting image");
            using (var stream = resp.Content.ReadAsStreamAsync().Result)
            {
                var doc = XDocument.Load(stream);
                var url = (string) doc.XPathSelectElement("/images/image/url");
                return new Uri(string.Format(CultureInfo.InvariantCulture, "http://bing.com{0}", url), UriKind.Absolute);
            }
        }

        public static string Translate(string text, string fromLanguage, string toLanguage, string clientId = "hamed324210",
            string clientSecret = "TCI30W5vO6E9C8sR9c/2sZlqfXVOzqhch0fag6+VjVU=")
        {
            var service = new TranslationService(clientId, clientSecret);
            return service.Translate(text, fromLanguage, toLanguage);
        }
    }
}