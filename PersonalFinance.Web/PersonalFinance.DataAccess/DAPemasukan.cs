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
    public class DAPemasukan
    {
        private readonly db_personal_financeContext db;
        public DAPemasukan(db_personal_financeContext _db)
        {
            db = _db;
        }
        public VMResponse<VMMoneySource?> Create(VMMoneySource data)
        {
            var response = new VMResponse<VMMoneySource?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {

                    // Buat user baru
                    MoneySource newData = new MoneySource
                    {
                        UserId = data.UserId,
                        Kategori = data.Kategori,
                        SourceName = data.SourceName,
                        Amount = data.Amount,
                        CreatedBy = data.CreatedBy,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false
                    };

                    db.Add(newData);
                    db.SaveChanges();
                    dbTrans.Commit();

                    // Mapping hasil user baru ke VMUser
                    response.Data = new VMMoneySource
                    {
                        UserId = newData.UserId,
                        Kategori = newData.Kategori,
                        SourceName = newData.SourceName,
                        Amount = newData.Amount,
                        CreatedBy = newData.CreatedBy,
                        CreatedOn = newData.CreatedOn
                    };

                    response.StatusCode = HttpStatusCode.Created;
                    response.Message = $"{HttpStatusCode.Created} - New Amount successfully created.";
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

        public VMResponse<List<VMMoneySource>> GetByFilter(string filter)
        {
            VMResponse<List<VMMoneySource>> response = new VMResponse<List<VMMoneySource>>();
            try
            {
                response.Data = (
                    from a in db.MoneySources
                    where a.IsDeleted == false
                    && (a.SourceName!.Contains(filter))
                    select new VMMoneySource
                    {
                        SourceId = a.SourceId,
                        SourceName = a.SourceName,
                        UserId = a.UserId,
                        Amount = a.Amount,
                        Kategori = a.Kategori,
                        CreatedOn =  a.CreatedOn,
                        CreatedBy = a.CreatedBy,
                        ModifiedBy = a.ModifiedBy,
                        IsDeleted = a.IsDeleted
                    }
                    ).ToList();
                response.Message = (response.Data.Count > 0)
                    ? $"{response.Data.Count} of Data(s) found successfully."
                    : $"{HttpStatusCode.NoContent} - No data found";

                response.StatusCode = (response.Data.Count > 0)
                    ? HttpStatusCode.OK
                    : HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                response.Message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }

        public VMResponse<VMMoneySource?> GetById(int id)
        {
            VMResponse<VMMoneySource?> response = new VMResponse<VMMoneySource?>();
            try
            {
                if (id > 0)
                {
                    response.Data = (

                        from a in db.MoneySources
                        where a.IsDeleted == false
                        && a.SourceId == id
                        select new VMMoneySource
                        {
                            SourceId = a.SourceId,
                            SourceName = a.SourceName,
                            UserId = a.UserId,
                            Amount = a.Amount,
                            Kategori = a.Kategori,
                            CreatedOn = a.CreatedOn,
                            CreatedBy = a.CreatedBy,
                            ModifiedBy = a.ModifiedBy,
                            IsDeleted = a.IsDeleted
                        }
                    ).FirstOrDefault();

                    if (response.Data != null)
                    {
                        response.StatusCode = HttpStatusCode.OK;
                        response.Message = $"{HttpStatusCode.OK} - Source Sukses Full";
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.Message = $"{HttpStatusCode.NoContent} - Source does not exis";
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = $"{HttpStatusCode.BadRequest} - please input Source";
                }
            }
            catch (Exception e)
            {

                response.Message = $"{HttpStatusCode.InternalServerError} - {e.Message}";
            }
            return response;
        }
        public VMResponse<VMMoneySource?> Update(VMMoneySource data)
        {
            var response = new VMResponse<VMMoneySource?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {

                    var existingData = db.MoneySources
                                         .FirstOrDefault(c => c.SourceId == data.SourceId && c.IsDeleted == false);

                    if (existingData == null)
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Message = $"{HttpStatusCode.NotFound} - Data Not Found";
                        return response;
                    }


                    existingData.SourceName = data.SourceName;
                    existingData.Amount = data.Amount;
                    existingData.Kategori = data.Kategori;
                    existingData.ModifiedBy = data.ModifiedBy;
                    existingData.CreatedOn = DateTime.Now;
                    existingData.ModifiedOn = DateTime.Now;

                    db.Update(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();


                    response.Data = GetById(data.SourceId).Data;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = $"{HttpStatusCode.OK} - Data Has Been Updated";
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
                }
            }
            return response;
        }
        public VMResponse<VMMoneySource> Delete(int id, int userId)
        {
            VMResponse<VMMoneySource?> response = new VMResponse<VMMoneySource?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    // Cari data yang akan dihapus
                    MoneySource? existingData = db.MoneySources.FirstOrDefault(c => c.SourceId == id);
                    if (existingData == null)
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Message = $"{HttpStatusCode.NotFound} - Major Not Found";
                        return response;
                    }

                    // Hapus data secara fisik
                    db.MoneySources.Remove(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();

                    response.Data = null; // Data sudah terhapus, tidak perlu dikembalikan
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = $"{HttpStatusCode.OK} - Data Has Been Deleted";
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
                }
            }
            return response;
        }

    }
}
