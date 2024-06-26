using System.ComponentModel.DataAnnotations;
using CBEsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace CBEsApi.Models
{
    public class CbesUserWithRoleMetadata
    {
        // Define metadata properties if needed
    }

    [MetadataType(typeof(CbesUserWithRoleMetadata))]
    public partial class CbesUserWithRole
    {
        public static List<CbesUserWithRole> GetAll(CbesManagementContext db)
        {
            List<CbesUserWithRole> roleUsers = db.CbesUserWithRoles.Where(q => q.IsDeleted != true).ToList();
            return roleUsers;
        }

    }
}
