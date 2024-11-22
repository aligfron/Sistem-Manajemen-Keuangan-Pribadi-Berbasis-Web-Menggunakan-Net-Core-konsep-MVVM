using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalFinance.ViewModel;
using PersonalFinance.Web.Models;
using System.Net;

namespace PersonalFinance.Web.Controllers
{
    public class PemasukanController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string? _custId;
        private PemasukanModel pemasukan;
        public PemasukanController(IConfiguration _config)
        {
            pemasukan = new PemasukanModel(_config);
        }
        public async Task<IActionResult> Index(string? filter)
        {
            List<VMMoneySource>? data = new List<VMMoneySource>();
            try
            {
                data = (string.IsNullOrEmpty(filter)) ? await pemasukan.getByFilter("") : await pemasukan.getByFilter(filter);
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = ex.Message;
                TempData["AlertType"] = "error";
            }

            ViewBag.Title = "Major Index";
            ViewBag.filter = filter;
            return View(data);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "New Source";

            return View();
        }
        [HttpPost]
        public async Task<VMResponse<VMMoneySource>?> CreateAsync(VMMoneySource data)
        {
            VMResponse<VMMoneySource>? response = null;

            try
            {
                data.CreatedBy = HttpContext.Session.GetString("userId")!;
                data.UserId = int.Parse(HttpContext.Session.GetString("userId")!);
                response = await pemasukan.CreateAsync(data);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    HttpContext.Session.SetString("successMsg", response.Message);
                }
                else
                {
                    HttpContext.Session.SetString("errMsg", response.Message);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }
            return (response);
        }
    }
}
