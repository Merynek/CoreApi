using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace CoreApi.Controllers
{
    public class User
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string role { get; set; }
    }

    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET api/values
        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("noadmin")]
        [Authorize]
        public ActionResult<IEnumerable<string>> GetXX()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        [Route("free")]
        public ActionResult<string> GetId()
        {
            string ss = _configuration.GetConnectionString("LocalConnection");
            var connection = new SqlConnection(ss);
            var compiler = new SqlServerCompiler();

            var db = new QueryFactory(connection, compiler);

            // You can register the QueryFactory in the IoC container

            // var User = db.Query("Users").Where("Id", 1).First<User>();
            var user = new User();
            user.username = "Ambrda";
            user.role = "nic";

            db.Query("Users").Insert(new
            {
                username = user.username,
                role = user.role
            });
            var x = "";

            return Ok(
                new
                {
                    test = "test"
                });
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
