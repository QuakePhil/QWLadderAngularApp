using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
            //http://azure.microsoft.com/en-us/documentation/articles/sql-database-dotnet-how-to-use/#connect-db
            SqlConnectionStringBuilder csBuilder;
            csBuilder = new SqlConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString
            );
            //After you have built your connection string, you can use the SQLConnection class to connect the SQL Database server:
            List<string> serversToExclude = new List<string>();

            using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("GetServersAllreadyPinged", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(PingPost.ParameterFactory(RequestContext.Principal.Identity.Name, "@Player", 256));
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                serversToExclude.Add(reader.GetString(0));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("Ladder Post error on SP: " + e.Message);
                    }
                }
            }
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
                System.Diagnostics.Debug.Write(ip);
                if (ip != "" && !serversToExclude.Contains(ip))
                {
                    servers.Add(ip);
                }
            }


            // get rid of any duplicates as well (e.g. multiple ports per ip, also the fact that we add)
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
