using System.ComponentModel.DataAnnotations;
using CBEsApi.Data;
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
        public static List<Cbe> GetAll(CbesManagementContext db)
        {
            List<Cbe> cbes = db.Cbes.Where(q => q.IsDeleted != true).ToList();
            return cbes;
        }

        public static Cbe GetById(CbesManagementContext db, int id)
        {
            Cbe? cbe = db.Cbes.Where(q => q.Id == id && q.IsDeleted != true).FirstOrDefault();
            return cbe ?? new Cbe();
        }

        public static Cbe Delete(CbesManagementContext db, int id)
        {
            Cbe cbe = GetById(db, id);
            cbe.IsDeleted = true;
            db.Entry(cbe).State = EntityState.Modified;
            db.SaveChanges();

            return cbe;
        }

    }
}
