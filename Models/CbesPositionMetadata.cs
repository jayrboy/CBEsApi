using System.ComponentModel.DataAnnotations;
using CBEsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace CBEsApi.Models
{
    public class CbesPositionMetadata
    {

    }

    [MetadataType(typeof(CbesPositionMetadata))]
    public partial class CbesPosition
    {
        public static List<CbesPosition> GetAll(CbesManagementContext db)
        {
            List<CbesPosition>? positions = db.CbesPositions.Where(q => q.IsDeleted != true).Include(u => u.CbesUsers).ToList();
            return positions ?? new List<CbesPosition>();
        }

        public static CbesPosition GetById(CbesManagementContext db, int id)
        {
            CbesPosition? position = db.CbesPositions.Where(q => q.Id == id && q.IsDeleted != true).Include(u => u.CbesUsers).FirstOrDefault();
            return position ?? new CbesPosition();
        }
    }
}