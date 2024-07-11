using System.ComponentModel.DataAnnotations;
using CBEsApi.Data;
using CBEsApi.Dtos.CBEsDto;

namespace CBEsApi.Models
{
    public class CbesLogMetadata
    {

    }

    [MetadataType(typeof(CbesLogMetadata))]
    public partial class CbeLog
    {

        public static List<CBEsLogDto> GetAll(CbesManagementContext db)
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
                UpdateBy = s.UpdateBy,
                CbesLogHeader = s.CbesLogHeader.Id
            }).ToList();

            return cbes;
        }
    }

}