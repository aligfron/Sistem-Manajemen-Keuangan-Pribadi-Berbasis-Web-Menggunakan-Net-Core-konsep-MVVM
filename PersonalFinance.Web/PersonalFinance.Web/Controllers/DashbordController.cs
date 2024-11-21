using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Web.Models;

namespace PersonalFinance.Web.Controllers
{
    public class DashbordController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UsersModel users;
        private string? _custId;

        public DashbordController(IConfiguration _config)
        {
            users = new UsersModel(_config);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
