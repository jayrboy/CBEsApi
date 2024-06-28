using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CBEsApi.Data;
using CBEsApi.Dtos.CBEsRole;

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

        public static CbesRoleDto GetById(CbesManagementContext db, int id)
        {
            var role = db.CbesRoles.Where(q => q.Id == id)
                                  .Include(q => q.CbesUserWithRoles)
                                  .ThenInclude(q => q.User)
                                  .FirstOrDefault();

            if (role == null)
            {
                return null; // หรือส่งคืน CbesRoleDto ว่าง
            }

            var roleDto = new CbesRoleDto
            {
                Id = role.Id,
                Name = role.Name,  // สมมติว่ามีคุณสมบัติ RoleName ใน CbesRole
                Users = role.CbesUserWithRoles.Select(r => new UserDto
                {
                    Id = r.User.Id,
                    Fullname = r.User.Fullname,
                    Username = r.User.Username
                }).ToList()
            };

            return roleDto;
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

        //  Delete ID
        // public static CbesRole Delete(CbesManagementContext db, int id)
        // {
        //     CbesRole cbe = GetById(db, id);
        //     cbe.UpdateDate = DateTime.Now;
        //     cbe.IsDeleted = true;
        //     db.Entry(cbe).State = EntityState.Modified;
        //     db.SaveChanges();

        //     return cbe;
        // }

        //  Cancel Delete ID
        // public static CbesRole cancelDelete(CbesManagementContext db, int id)
        // {
        //     CbesRole cbe = GetById(db, id);
        //     cbe.UpdateDate = DateTime.Now;
        //     cbe.IsDeleted = false;
        //     db.Entry(cbe).State = EntityState.Modified;
        //     db.SaveChanges();

        //     return cbe;
        // }
        //  Last Delete ID
        // public static CbesRole lastDelete(CbesManagementContext db, int id)
        // {
        //     CbesRole cbe = GetById(db, id);
        //     cbe.UpdateDate = DateTime.Now;
        //     cbe.IsLastDelete = true;
        //     db.Entry(cbe).State = EntityState.Modified;
        //     db.SaveChanges();

        //     return cbe;
        // }

        public static List<CbesRole> GetAllBin(CbesManagementContext db)
        {
            List<CbesRole> roles = db.CbesRoles.Where(q => q.IsDeleted == true && q.IsLastDelete == false).ToList();
            return roles;
        }
    }
}
