using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using WebApplication1.IO;

namespace WebApplication1.ApiControllers
{
    public class ValuesController : ApiController
    {
        // GET api/values.
        [HttpGet]
        [HttpHead]
        public HttpResponseMessage Get(
            int id = 1)
        {
            var encoding = new UTF8Encoding(false, true);
            var ms = new MemoryStream();
            using (var writer = new StreamWriter(ms, encoding, 8192, true))
            {
                var numLines = id;
                foreach (var line in Enumerable.Range(0, numLines))
                {
                    writer.WriteLine("Hey there!");
                }
            }
            ms.Position = 0;

            var response = Request.CreateResponse();
            response.Content = new StreamContent(
                new WrappingLoggingStream(
                    ms,
                    true));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue($"text/plain")
            {
                CharSet = encoding.WebName,
            };
            return response;
        }
    }
}
