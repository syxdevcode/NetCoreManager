using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreManager.WebApi.Filter;

namespace NetCoreManager.WebApi.Controllers
{
    [Route("values")]
    public class ValuesController : Controller
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

        // GET api/values/1
        [HttpGet]
        [Route("Get1")]
        [Authorize("Bearer")]
        public string Get1()
        {
            return "test";
        }
        
    }
}
