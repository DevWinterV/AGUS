using AGUS.Hubs;
using AGUS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace AGUS.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        /*
        public IActionResult Index()
        {
            return Redirect($"/{Guid.NewGuid()}");
        }*/
        public IActionResult CreateRoom()
        {
            return Redirect($"/meet/{Guid.NewGuid()}");
        }
        [HttpGet("/{roomId}")]
        public IActionResult Room(string roomId)
        {
            ViewData["RoomId"] = roomId;
            return View();
        }

        [HttpGet("/meet/{roomId}")]
        public IActionResult Meet(string roomId)
        {
            ViewData["RoomId"] = roomId;
            return View();
        }
        public IActionResult matchfound()
        {
            return View();
        }


    }
}