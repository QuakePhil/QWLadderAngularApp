using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Configuration;

namespace QWLadderAspWebApp.Controllers
{
    public class StatsPost
    {
        public string ip { get; set; }
        public string port { get; set; }
        public string host { get; set; }
        public string map { get; set; }
        public int teamplay { get; set; }
        public int timelimit { get; set; }
        public int maxclients { get; set; }
        public int players { get; set; }
        public string[] nick { get; set; }
        public string[] frags { get; set; }
        public string[] team { get; set; }

        public static SqlParameter ParameterFactory(string value, string paramname, int paramsize)
        {
            if (value == null) value = "";
            SqlParameter parm;
            parm = new SqlParameter(paramname, System.Data.SqlDbType.NVarChar, paramsize);
            parm.Value = value;
            return parm;
        }
        public static SqlParameter ParameterFactory(int value, string paramname,
            System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input)
        {
            if (value == null) value = 0;
            SqlParameter parm;
            parm = new SqlParameter(paramname, System.Data.SqlDbType.Int);
            parm.Value = value;
            return parm;
        }

        public static void AttributesPostFactory(string[] attributes, string attributename, Int32 ID, SqlConnection conn)
        {
            if (attributes != null) if (attributes.Length > 0) foreach (string s in attributes)
            {
                using (SqlCommand command = new SqlCommand("PostQWStatsPlayerAttributes", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(StatsPost.ParameterFactory(ID, "@QWStatsID"));
                    command.Parameters.Add(StatsPost.ParameterFactory(attributename, "@Attribute", 50));
                    command.Parameters.Add(StatsPost.ParameterFactory(s, "@Value", 200));

                    try { command.ExecuteNonQuery(); }
                    catch (Exception e) { System.Diagnostics.Debug.WriteLine("Attributes Post error on SP: " + e.Message); }
                }
            }
        }

    }

    //[Authorize]
    //[RoutePrefix("api/stats")]
    public class StatsController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return null;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }


        // POST api/values
        public IEnumerable<string> Post([FromBody] StatsPost post)
        {
            if (post == null)
            {
                System.Diagnostics.Debug.WriteLine("No data to post");
                return null;
            }
            List<string> ret = new List<string>();

            // the only reason I'm doing the insert logic here and not in a stored procedure
            // is because I don't want to have to worry about passing attribute value tables
            // as sp parameters tt

            //http://azure.microsoft.com/en-us/documentation/articles/sql-database-dotnet-how-to-use/#connect-db
            SqlConnectionStringBuilder csBuilder;
            csBuilder = new SqlConnectionStringBuilder(
                // LocalSqlServer is something else
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString
            );
            //After you have built your connection string, you can use the SQLConnection class to connect the SQL Database server:
            using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("PostQWStats", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(StatsPost.ParameterFactory(post.ip, "@ip", 50));
                    command.Parameters.Add(StatsPost.ParameterFactory(post.port, "@port", 50));
                    command.Parameters.Add(StatsPost.ParameterFactory(post.host, "@host", 200));
                    command.Parameters.Add(StatsPost.ParameterFactory(post.map, "@map", 50));
                    command.Parameters.Add(StatsPost.ParameterFactory(post.teamplay, "@teamplay"));
                    command.Parameters.Add(StatsPost.ParameterFactory(post.timelimit, "@timelimit"));
                    command.Parameters.Add(StatsPost.ParameterFactory(post.maxclients, "@maxclients"));
                    command.Parameters.Add(StatsPost.ParameterFactory(post.players, "@players"));
                    command.Parameters.Add(StatsPost.ParameterFactory(0, "@ID", System.Data.ParameterDirection.Output));

                    Int32 ID = 0;

                    try
                    {
                        ID = (Int32)command.ExecuteScalar();
                        System.Diagnostics.Debug.WriteLine("ID value: " + ID);

                        StatsPost.AttributesPostFactory(post.nick, "nick", ID, conn);
                        StatsPost.AttributesPostFactory(post.team, "team", ID, conn);
                        StatsPost.AttributesPostFactory(post.frags, "frags", ID, conn);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("Stats Post error on SP: " + e.Message);
                    }

                }

                using (SqlCommand command = new SqlCommand("select ip from QWStats", conn))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add(reader.GetString(0));
                    }
                }
            }

            /*
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
                    servers.Add(ip);
                    System.Diagnostics.Debug.WriteLine(ip);
                }
            }
            // get rid of any duplicates as well (e.g. multiple ports per ip)
            IEnumerable<string> enumerableServers = servers.Distinct().ToList();
             * */
            return ret;
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
