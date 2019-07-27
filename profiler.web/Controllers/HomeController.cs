using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using profiler.web.Models;
using StackExchange.Profiling;

namespace profiler.web.Controllers
{
    public class HomeController : Controller
    {
        readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) => _logger = logger;

        public IActionResult Index()
        {
            _logger.LogWarning("Called Home/Index {Id}", 1);

            using (MiniProfiler.Current.Step("Index"))
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        ///  Resharper sucks
        /// </summary>
        public static void HelloDan()
        {
            Console.WriteLine("Does it show my cursor as a blue cross");
        }
    }

    
}
