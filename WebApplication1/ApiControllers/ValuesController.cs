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
        static readonly Encoding encodingNoBom = new UTF8Encoding(false, true);
        static bool log = true;
        
        // GET api/values.
        [HttpGet]
        [HttpHead]
        public HttpResponseMessage Get()
        {
            var response = Request.CreateResponse();
            var stream = GetStreamFromSomewhere();
            if (log)
            {
                stream = new WrappingLoggingStream(stream, true);
            }
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue($"text/plain")
            {
                CharSet = encodingNoBom.WebName,
            };
            return response;
        }

        private Stream GetStreamFromSomewhere()
        {
            var ms = new MemoryStream();
            using (var writer = new StreamWriter(ms, encodingNoBom, 8192, true))
            {
                var numLines = 1000;
                foreach (var line in Enumerable.Range(0, numLines))
                {
                    writer.WriteLine("Hey there!");
                }
            }
            ms.Position = 0;
            return ms;
        }
    }
}
