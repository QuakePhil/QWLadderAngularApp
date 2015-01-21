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
    //[RoutePrefix("api/serverlist")]
    public class ServerlistController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            //return new string[] { "value1", "value2" };
            List<string> servers = new List<string>();

            // hints: http://stackoverflow.com/questions/566167/query-an-xdocument-for-elements-by-name-at-any-depth

            // thx mli
            // (apparently, this was for shambler bot, a creation by zalon...)
            XDocument doc = XDocument.Load("http://www.quakeservers.net/shambler_servers.php");
            foreach (XElement element in doc.Element("rss").Element("channel").Elements("item"))
            {
                string ip = "";
                try
                {
                    ip = element.Element("ip").Value.ToString();
                }
                catch (Exception)
                {
                }
                if (ip != "")
                {
                    Console.WriteLine("good ip found");
                    servers.Add(ip);
                }
            }
            // get rid of any duplicates as well (e.g. multiple ports per ip)
            IEnumerable<string> enumerableServers = servers.Distinct().ToList();
            return enumerableServers;
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
