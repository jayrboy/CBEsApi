using System.ComponentModel.DataAnnotations;
using CBEsApi.Data;

namespace CBEsApi.Models
{
    public class CbesRoleMetadata
    {
        // Define metadata properties if needed
    }

    [MetadataType(typeof(CbesRoleMetadata))]
    public partial class CbesRole
    {
        public static List<CbesRole> GetAll(CbesManagementContext db)
        {
            List<CbesRole> roles = db.CbesRoles.Where(q => q.IsDeleted != true).ToList();
            return roles;
        }

        public static CbesRole GetById(CbesManagementContext db, int id)
        {
            CbesRole? role = db.CbesRoles.Where(q => q.Id == id && q.IsDeleted != true).FirstOrDefault();
            return role ?? new CbesRole();
        }

        public static CbesRole Create(CbesManagementContext db, CbesRole cbeRole)
        {
            cbeRole.CreateDate = DateTime.Now;
            cbeRole.UpdateDate = DateTime.Now;
            cbeRole.IsDeleted = false;
            cbeRole.IsLastDelete = false;
            db.CbesRoles.Add(cbeRole);
            db.SaveChanges();

            return cbeRole;
        }
    }
}
