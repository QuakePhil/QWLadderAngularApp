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
    public class LadderPost
    {
        public string[] ladder { get; set; }
    }

    [Authorize]
    [RoutePrefix("api/ladder")]
    public class LadderController : ApiController
    {
        // May be worth it to replace this Get api with a websockets implement
        public IEnumerable<object[]> Get()
        {
            List<object[]> ladderCandidates = new List<object[]>();

            SqlConnectionStringBuilder csBuilder;
            csBuilder = new SqlConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString
            );
            using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("GetLadderOpponents", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(PingPost.ParameterFactory(RequestContext.Principal.Identity.Name, "@Player", 50));
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                object[] buffer = new object[10]; 
                                reader.GetValues(buffer);
                                ladderCandidates.Add(buffer);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("Ladder Post error on SP: " + e.Message);
                    }
                }
            }

            return ladderCandidates;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }


        // POST api/values
        public void Post([FromBody] LadderPost post)
        {
            if (post == null)
            {
                System.Diagnostics.Debug.WriteLine("No ladder to post");
                return;
            }

            //http://azure.microsoft.com/en-us/documentation/articles/sql-database-dotnet-how-to-use/#connect-db
            SqlConnectionStringBuilder csBuilder;
            csBuilder = new SqlConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString
            );
            using (SqlConnection conn = new SqlConnection(csBuilder.ToString()))
            {
                conn.Open();
                for (int i = 0; i < post.ladder.Length; ++i) using (SqlCommand command = new SqlCommand("PostLadder", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(PingPost.ParameterFactory(RequestContext.Principal.Identity.Name, "@Player", 50));
                    command.Parameters.Add(PingPost.ParameterFactory(post.ladder[i], "@Ladder", 20));
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("Ladder Post error on SP: " + e.Message);
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
