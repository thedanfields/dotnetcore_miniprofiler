using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace profiler.service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FastController : ControllerBase
    {
        [HttpGet]
        public Task<IActionResult> Get()
        {
            using (MiniProfiler.Current.Step("Index"))
            {
                return Task.FromResult(Ok(1) as IActionResult);
            }

           
        }
    }
}
