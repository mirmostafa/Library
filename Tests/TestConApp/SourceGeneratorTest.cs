using System.Web.Http;

namespace TestConApp
{
    public class SourceGeneratorTest
    {
        [HttpGet]
        public DateTime GetDateTime() => DateTime.Now;
    }
}
