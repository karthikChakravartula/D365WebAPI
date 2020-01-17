using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMAPI.CRMFunctions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<Object> Get()
        {
            CRMBaseMethods bm = new CRMBaseMethods();

            //var res = bm.Retrieve("opportunities", new Guid("6c9e3495-1432-ea11-a812-000d3a579ca4"),"name");

            var res = bm.RetrieveMultiple("accounts","name,accountid");

            return res;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public ActionResult<object> Post([FromBody] object value)
        {
            //CRMBaseMethods bm = new CRMBaseMethods();

            //var res = bm.Update("accounts",new Guid("6e8227e4-e837-ea11-a813-000d3a579805"), value);

            //return res;



            CRMBaseMethods bm = new CRMBaseMethods();

            //var res = bm.Update("accounts", new Guid("a3f6f127-e437-ea11-a813-000d3a579805"), value);

            var res = bm.Create("accounts", value);

            return res;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult<object> Delete([FromBody] string gid)
        {
            CRMBaseMethods bm = new CRMBaseMethods();

            var res = bm.Delete("accounts", new Guid(gid));

            return res;
        }
    }
}
