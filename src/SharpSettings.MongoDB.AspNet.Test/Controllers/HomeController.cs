using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace SharpSettings.MongoDB.AspNet.Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly TestSettings2 _optionsAccessor;

        public HomeController(IOptions<TestSettings2> optionsAccessor)
        {
            _optionsAccessor = optionsAccessor.Value;
        }

        public IActionResult Index()
        {
            return Ok(_optionsAccessor.Bobs[1]);
        }
    }
}
