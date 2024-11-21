using Microsoft.EntityFrameworkCore.Storage;
using PersonalFinance.DataModel;
using PersonalFinance.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.DataAccess
{
    public class DAUsers
    {
        private readonly db_personal_financeContext db;
        public DAUsers(db_personal_financeContext _db)
        {
            db = _db;
        }
        public VMResponse<VMUser?> Create(VMUser data)
        {
            var response = new VMResponse<VMUser?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    // Cek apakah email sudah ada di database
                    var existingUser = db.Users.FirstOrDefault(u => u.Email == data.Email && u.IsDeleted == false);
                    if (existingUser != null)
                    {
                        response.Data = null;
                        response.StatusCode = HttpStatusCode.Conflict; // 409 Conflict
                        response.Message = "Email already exists. Please use a different email.";
                        return response;
                    }

                    // Buat user baru
                    User newData = new User
                    {
                        Username = data.Username,
                        Email = data.Email,
                        Password = data.Password,
                        FullName = data.FullName,
                        Role = data.Role,
                        CreatedBy = data.CreatedBy,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false
                    };

                    db.Add(newData);
                    db.SaveChanges();
                    dbTrans.Commit();

                    // Mapping hasil user baru ke VMUser
                    response.Data = new VMUser
                    {
                        Username = newData.Username,
                        Email = newData.Email,
                        FullName = newData.FullName,
                        Role = newData.Role,
                        CreatedBy = newData.CreatedBy,
                        CreatedOn = newData.CreatedOn
                    };

                    response.StatusCode = HttpStatusCode.Created;
                    response.Message = $"{HttpStatusCode.Created} - New User successfully created.";
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    response.Data = null;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
                }
            }

            return response;
        }
        public VMResponse<VMUser> GetByEmail(string email)
        {
            VMResponse<VMUser?> response = new VMResponse<VMUser?>();
            try
            {
                response.Data = (
                    from u in db.Users
                    where u.IsDeleted == false && u.Email == email
                    select new VMUser
                    {
                        UserId = u.UserId,
                        Username = u.Username,
                        Role = u.Role,
                        Email = u.Email,
                        Password = u.Password,
                        IsDeleted = u.IsDeleted
                    }
                    ).FirstOrDefault();
                response.StatusCode = (response.Data != null) ?
                        HttpStatusCode.OK :
                        HttpStatusCode.NotFound;
                response.Message = (response.Data != null) ?
                    $"{HttpStatusCode.OK} - User succesfully fetched!"
                    : $"{HttpStatusCode.NotFound} - User does not exist!";
            }
            catch (Exception ex)
            {
                response.Message = $"{HttpStatusCode.NoContent} - {ex.Message}";
            }
            return response!;
        }
        public VMResponse<VMUser> Login(string email, string password)
        {
            VMResponse<VMUser> response = new VMResponse<VMUser>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    // Ambil data user berdasarkan email
                    VMUser? existingData = GetByEmail(email).Data;

                    // Jika user tidak ditemukan
                    if (existingData == null)
                    {
                        response.Data = null;
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Message = "User not found";
                        return response;
                    }

                    // Jika password salah
                    if (existingData.Password != password)
                    {
                        response.Data = null; // Pastikan data tidak diisi
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        response.Message = "Invalid password";
                        return response;
                    }

                    // Jika login berhasil
                    response.Data = existingData;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Successfully logged in";
                }
                catch (Exception ex)
                {
                    // Rollback transaksi jika terjadi kesalahan
                    dbTrans.Rollback();
                    response.Data = null;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = $"Internal server error: {ex.Message}";
                }
            }

            return response;
        }

    }
}
