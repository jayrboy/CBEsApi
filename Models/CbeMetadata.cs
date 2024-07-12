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

        public static CBEsDto GetById(CbesManagementContext db, int id)
        {
            CBEsDto? cbe = db.Cbes.Where(q => q.Id == id).Select(s => new CBEsDto
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
                CbesProcesses = s.CbesProcesses.Select(p => new CBEsProcessDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Weight = p.Weight,
                    CreateDate = p.CreateDate,
                    UpdateDate = p.UpdateDate,
                    IsDeleted = p.IsDeleted,
                    ProcessHeaderId = p.ProcessHeaderId,
                    CbesId = p.CbesId,
                    CbesMaturities = p.CbesMaturities.Select(m => new CbesMaturity
                    {
                        Id = m.Id,
                        Detail = m.Detail,
                        Lv = m.Lv,
                        CreateDate = m.CreateDate,
                        UpdateDate = m.UpdateDate,
                        IsDeleted = m.IsDeleted,
                        CbesProcessId = m.CbesProcessId
                    }).ToList(),
                    ProcessHeader = p.ProcessHeader != null ? new CBEsProcessDto
                    {
                        Id = p.ProcessHeader.Id,
                        Name = p.ProcessHeader.Name,
                        Weight = p.ProcessHeader.Weight,
                        CreateDate = p.ProcessHeader.CreateDate,
                        UpdateDate = p.ProcessHeader.UpdateDate,
                        IsDeleted = p.ProcessHeader.IsDeleted,
                        ProcessHeaderId = p.ProcessHeader.ProcessHeaderId,
                        CbesId = p.ProcessHeader.CbesId,
                        CbesMaturities = p.ProcessHeader.CbesMaturities.Select(m => new CbesMaturity
                        {
                            Id = m.Id,
                            Detail = m.Detail,
                            Lv = m.Lv,
                            CreateDate = m.CreateDate,
                            UpdateDate = m.UpdateDate,
                            IsDeleted = m.IsDeleted,
                            CbesProcessId = m.CbesProcessId
                        }).ToList()
                    } : null
                }).ToList()
            }).FirstOrDefault();

            return cbe ?? new CBEsDto();
        }

        public static CBEsDto Delete(CbesManagementContext db, int id)
        {
            CBEsDto cbe = GetById(db, id);
            cbe.IsDeleted = true;

            db.SaveChanges();
            return cbe;
        }

        public static CBEsDto CancelDelete(CbesManagementContext db, int id)
        {
            CBEsDto cbe = GetById(db, id);
            cbe.IsDeleted = false;

            // db.Entry(cbe).State = EntityState.Modified;
            db.SaveChanges();

            return cbe;
        }

        public static CBEsDto LastDelete(CbesManagementContext db, int id)
        {
            CBEsDto cbe = GetById(db, id);
            cbe.IsLastDelete = true;

            // db.Entry(cbe).State = EntityState.Modified;
            db.SaveChanges();

            return cbe;
        }
    }
}
