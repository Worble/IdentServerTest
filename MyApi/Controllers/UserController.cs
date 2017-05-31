using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Controllers
{
    [Route("Identity")]
    [Authorize]
    public class UserController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return new JsonResult(new User() { Id = 1, Name = "Test" });
        }
    }
}
