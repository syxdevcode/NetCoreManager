using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreManager.WebApi.Controllers
{
    [Route("values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [HttpGet]
        [Authorize("Bearer")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Authorize("Bearer")]
        public string Get(int id)
        {
            return "value";
        }
        
    }
}
