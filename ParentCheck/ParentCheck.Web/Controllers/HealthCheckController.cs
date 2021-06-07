using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Controllers
{
    [Route("api/[controller]")]
    public class HealthCheckController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "val1","val2"};
        }
    }
}
