using System.ComponentModel.DataAnnotations;
using CBEsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace CBEsApi.Models
{
    public class CbesLogHeaderMetadata
    {

    }

    [MetadataType(typeof(CbesLogHeaderMetadata))]
    public partial class CbesLogHeader
    {
        public static List<CbesLogHeader> GetHistory(CbesManagementContext db, int id)
        {
            List<CbesLogHeader>? cbe = db.CbesLogHeaders.Where(q => q.CbesId == id && q.IsDeleted != true)
                                                        .Include(q => q.CbesLogType)
                                                        .Select(s => new CbesLogHeader
                                                        {
                                                            Id = s.Id,
                                                            Round = s.Round,
                                                            Remark = s.Remark,
                                                            CreateDate = s.CreateDate,
                                                            UpdateDate = s.UpdateDate,
                                                            IsDeleted = s.IsDeleted,
                                                            CbesLogTypeId = s.CbesLogTypeId,
                                                            CbesId = s.CbesId,
                                                            UpdateBy = s.UpdateBy,
                                                            CbesLogType = s.CbesLogType

                                                        }).ToList();

            return cbe;
        }

    }
}