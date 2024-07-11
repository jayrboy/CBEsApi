using System.ComponentModel.DataAnnotations;
using CBEsApi.Data;
using CBEsApi.Dtos.CBEsDto;
using Microsoft.EntityFrameworkCore;

namespace CBEsApi.Models
{
    public class CbeMetadata
    {
        // Define metadata properties if needed
    }

    [MetadataType(typeof(CbeMetadata))]
    public partial class Cbe
    {
        public static List<CBEsDto> GetAll(CbesManagementContext db)
        {
            List<CBEsDto> cbes = db.Cbes.Where(q => q.IsDeleted != true).Select(s => new CBEsDto
            {
                Id = s.Id,
                ThaiName = s.ThaiName,
                EngName = s.EngName,
                ShortName = s.ShortName,
                Detail = s.Detail,
                IsActive = s.IsActive,
                CreateDate = s.CreateDate,
                UpdateDate = s.UpdateDate,
                IsDeleted = s.IsDeleted,
                IsLastDelete = s.IsLastDelete,
                CreateBy = s.CreateBy,
            }).ToList();

            return cbes;
        }

        public static List<CBEsDto> GetAllBin(CbesManagementContext db)
        {
            List<CBEsDto> cbes = db.Cbes.Where(q => q.IsDeleted == true).Select(s => new CBEsDto
            {
                Id = s.Id,
                ThaiName = s.ThaiName,
                EngName = s.EngName,
                ShortName = s.ShortName,
                Detail = s.Detail,
                IsActive = s.IsActive,
                CreateDate = s.CreateDate,
                UpdateDate = s.UpdateDate,
                IsDeleted = s.IsDeleted,
                IsLastDelete = s.IsLastDelete,
                CreateBy = s.CreateBy,
            }).ToList();

            return cbes;
        }

        public static Cbe GetById(CbesManagementContext db, int id)
        {
            Cbe? cbe = db.Cbes.Where(q => q.Id == id).Include(q => q.CbesProcesses).FirstOrDefault();
            return cbe ?? new Cbe();
        }

        public static Cbe Delete(CbesManagementContext db, int id)
        {
            Cbe cbe = GetById(db, id);
            cbe.IsDeleted = true;

            db.SaveChanges();

            return cbe;
        }

        public static Cbe CancelDelete(CbesManagementContext db, int id)
        {
            Cbe cbe = GetById(db, id);
            cbe.IsDeleted = false;

            db.Entry(cbe).State = EntityState.Modified;
            db.SaveChanges();

            return cbe;
        }

        public static Cbe LastDelete(CbesManagementContext db, int id)
        {
            Cbe cbe = GetById(db, id);
            cbe.IsLastDelete = true;

            db.Entry(cbe).State = EntityState.Modified;
            db.SaveChanges();

            return cbe;
        }

        public static List<CBEsLogDto> GetHistories(CbesManagementContext db)
        {
            List<CBEsLogDto> cbes = db.CbesLogs.Where(q => q.IsDeleted != true).Select(s => new CBEsLogDto
            {
                Id = s.Id,
                ThaiName = s.ThaiName,
                EngName = s.EngName,
                ShortName = s.ShortName,
                Detail = s.Detail,
                Year = s.Year,
                CreateDate = s.CreateDate,
                UpdateDate = s.UpdateDate,
                IsDeleted = s.IsDeleted,
                IsLastDelete = s.IsLastDelete,
            }).ToList();

            return cbes;
        }

        public static Cbe GetLogById(CbesManagementContext db, int id)
        {
            Cbe? cbe = db.Cbes.Where(q => q.Id == id).Include(q => q.CbesLogs).FirstOrDefault();
            return cbe ?? new Cbe();
        }
    }
}
