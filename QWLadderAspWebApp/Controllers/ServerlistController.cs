using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;

namespace QWLadderAspWebApp.Controllers
{
    [Authorize]
    public class ServerlistController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            //return new string[] { "value1", "value2" };

            // thx mli
            // (apparently, this was for shambler bot, a creation by zalon...)
            XDocument doc = XDocument.Load("http://www.quakeservers.net/shambler_servers.php");
            var xmlContents = doc;
            return new string[] { "190.90.90.90", "9.9.9.9" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
