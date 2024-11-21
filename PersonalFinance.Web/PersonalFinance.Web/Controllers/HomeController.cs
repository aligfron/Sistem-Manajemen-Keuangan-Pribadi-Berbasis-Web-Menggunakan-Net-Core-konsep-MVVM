using Microsoft.AspNetCore.Mvc;
using PersonalFinance.DataModel;
using PersonalFinance.ViewModel;
using PersonalFinance.Web.Models;
using System.Diagnostics;
using System.Net;

namespace PersonalFinance.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UsersModel users;
        private string? _custId;

        public HomeController(IConfiguration _config)
        {
            users = new UsersModel(_config);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registrasi()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(VMUser data)
        {
            try
            {

                VMUser? dataApi = await users.GetByEmail(data.Email!);
                if (dataApi != null)
                {
                    if (data.Password == dataApi.Password)
                    {
                        int value = dataApi.UserId;
                        HttpContext.Session.SetString("userId", value.ToString());
                        HttpContext.Session.SetString("userEmail", dataApi.Email!);
                        HttpContext.Session.SetString("userName", dataApi.Username!);
                        TempData["AlertMessage"] = "Login successful!";
                        TempData["AlertType"] = "success";
                        return RedirectToAction("Index", "Dashbord");
                    }
                    else
                    {
                        TempData["AlertMessage"] = "Invalid Password";
                        TempData["AlertType"] = "error";
                        throw new Exception("Invalid Password");
                    }
                }
                else
                {
                    TempData["AlertMessage"] = "Email not registered!";
                    TempData["AlertType"] = "error";
                    throw new Exception("Email not registered!");
                }
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = ex.Message;
                TempData["AlertType"] = "error";
            }
            return RedirectToAction("Index", "Home");

        }
        public IActionResult Logout()
        {
            TempData["AlertMessage"] = "Logout successful!";
            TempData["AlertType"] = "success";
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(VMUser data)
        {
            VMResponse<VMUser>? response = null;

            try
            {
                // Ambil user ID dari session
                data.CreatedBy = 1;

                // Panggil service untuk membuat user
                response = await users.CreateAsync(data);

                // Tangani respons berdasarkan status
                if (response?.StatusCode == HttpStatusCode.Created)
                {
                    TempData["AlertMessage"] = "Berhasil Daftar, Silahkan Melakukan Login di Menu Login";
                    TempData["AlertType"] = "success";

                }
                else if (response?.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["AlertMessage"] = response.Message; // "Email already exists."
                    TempData["AlertType"] = "error";
                }
                else
                {
                    TempData["AlertMessage"] = response?.Message ?? "Unknown error occurred.";
                    TempData["AlertType"] = "error";
                }
            }
            catch (Exception ex)
            {
                // Tangani error umum
                TempData["AlertMessage"] = ex.Message;
                TempData["AlertType"] = "error";
            }

            // Arahkan ke halaman Register
            return RedirectToAction("Registrasi", "Home");
        }


    }
}
