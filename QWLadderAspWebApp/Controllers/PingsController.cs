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
    public class PingPost
    {
        public string[] serverip { get; set; }
        public int[] ping { get; set; }

        public static SqlParameter ParameterFactory(string value, string paramname, int paramsize)
        {
            if (value == null) value = "";
            SqlParameter parm;
            parm = new SqlParameter(paramname, System.Data.SqlDbType.NVarChar, paramsize);
            parm.Value = value;
            return parm;
        }
        public static SqlParameter ParameterFactory(int value, string paramname)
        {
            if (value == null) value = 0;
            SqlParameter parm;
            parm = new SqlParameter(paramname, System.Data.SqlDbType.Int);
            parm.Value = value;
            return parm;
        }
    }

    [Authorize]
    [RoutePrefix("api/pings")]
    public class PingsController : ApiController
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
        public void Post([FromBody] PingPost post)
        {
            if (post == null)
            {
                System.Diagnostics.Debug.WriteLine("No data to post");
                return;
            }

            //http://azure.microsoft.com/en-us/documentation/articles/sql-database-dotnet-how-to-use/#connect-db
            SqlConnectionStringBuilder csBuilder;
            csBuilder = new SqlConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString
            );
            //After you have built your connection string, you can use the SQLConnection class to connect the SQL Database server:
            using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
            {
                conn.Open();
                for (int i = 0; i < post.serverip.Length; i++) using (SqlCommand command = new SqlCommand("PostQWPings", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(PingPost.ParameterFactory(RequestContext.Principal.Identity.Name, "@Player", 256));

                    command.Parameters.Add(PingPost.ParameterFactory(post.serverip[i], "@Serverip", 17));
                    command.Parameters.Add(PingPost.ParameterFactory(post.ping[i], "@Ping"));
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("Pings Post error on SP: " + e.Message);
                    }
                }
            }

            return;
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
