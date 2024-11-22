using Microsoft.AspNetCore.Mvc;
using PersonalFinance.DataAccess;
using PersonalFinance.DataModel;
using PersonalFinance.ViewModel;
using System.Net;

namespace PersonalFinance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PemasukanController : ControllerBase
    {
        public DAPemasukan pemasukan;
        public PemasukanController(db_personal_financeContext _db)
        {
            pemasukan = new DAPemasukan(_db);
        }
        [HttpPost]
        public async Task<ActionResult> Create(VMMoneySource data)
        {
            try
            {
                return Created("api/Pemasukan", await Task.Run(() => pemasukan.Create(data)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("IncomeController.Create " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                VMResponse<List<VMMoneySource>> response = await Task.Run(() => pemasukan.GetByFilter(""));
                if (response.Data.Count > 0)
                {
                    return Ok(response);
                }
                else
                {
                    Console.WriteLine(response.Message);
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("DanaController.GetAll: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{filter?}")]
        public async Task<ActionResult> GetBy(string? filter)
        {
            try
            {
                return (filter != null)
                    ? Ok(await Task.Run(() => pemasukan.GetByFilter(filter)))
                    : BadRequest("Dana name or description must be.... ");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                VMResponse<VMMoneySource?> response = await Task.Run(() => pemasukan.GetById(id));
                if (response.Data != null)
                {
                    return Ok(response);
                }
                else
                {
                    Console.WriteLine(response.Message);
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("DanaController.GetBy " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Update(VMMoneySource data)
        {
            try
            {
                VMResponse<VMMoneySource?> response = await Task.Run(() => pemasukan.Update(data));
                if (response.Data != null)
                {
                    return Ok(response);
                }
                else
                {
                    Console.WriteLine(response.Message);
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("MajorController.Update " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}/{userId:int}")]
        public async Task<IActionResult> Delete(int id, int userId)
        {
            try
            {
                // Memanggil service atau repository untuk menghapus data
                VMResponse<VMMoneySource?> response = await Task.Run(() => pemasukan.Delete(id, userId));

                // Jika data berhasil dihapus, kembalikan response OK
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Ok(new
                    {
                        status = "success",
                        message = response.Message
                    });
                }

                // Jika data tidak ditemukan
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound(new
                    {
                        status = "error",
                        message = response.Message
                    });
                }

                // Jika ada error lainnya
                return StatusCode((int)response.StatusCode, new
                {
                    status = "error",
                    message = response.Message
                });
            }
            catch (Exception ex)
            {
                // Log error untuk debugging
                Console.WriteLine($"DanaController.Delete Error: {ex.Message}");

                // Kembalikan respons Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "error",
                    message = "An unexpected error occurred.",
                    detail = ex.Message
                });
            }
        }

    }
}
