using Microsoft.AspNetCore.Mvc;
using PersonalFinance.DataAccess;
using PersonalFinance.DataModel;
using PersonalFinance.ViewModel;

namespace PersonalFinance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public DAUsers users;
        public UsersController(db_personal_financeContext _db)
        {
            users = new DAUsers(_db);
        }
        [HttpPost]
        public async Task<ActionResult> Create(VMUser data)
        {
            try
            {
                return Created("api/Users", await Task.Run(() => users.Create(data)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("UsersController.Create " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{email?}")]
        public async Task<ActionResult> GetByEmail(string email)
        {
            try
            {
                VMResponse<VMUser> response = await Task.Run(() => users!.GetByEmail(email));
                return (response.Data != null) ?
                    Ok(response) : throw new Exception(response.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("User Controller.GetByEmail : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{email}/{password}")]
        public async Task<ActionResult> Login(string email, string password)
        {
            try
            {
                VMResponse<VMUser> response = await Task.Run(() => users.Login(email,password));
                if (response.Data != null)
                {
                    return Ok(response);
                }
                else
                {
                    Console.WriteLine("LoginController.Update: " + response.Message);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("UserController.Update: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
