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
            CbesRole? role = db.CbesRoles.Where(q => q.Id == id)
                                         .Include(q => q.CbesUserWithRoles)
                                            .ThenInclude(ur => ur.User)
                                         .Include(q => q.CbesRoleWithPermissions)
                                         .FirstOrDefault();

            if (role == null)
            {
                return null; // หรือส่งคืน CbesRoleDto ว่าง
            }

            CbesRoleDto roleDto = new CbesRoleDto
            {
                Id = role.Id,
                Name = role.Name,  // สมมติว่ามีคุณสมบัติ RoleName ใน CbesRole
                Users = role.CbesUserWithRoles.Select(r => new UserDto
                {
                    Id = r.User.Id,
                    Fullname = r.User.Fullname,
                    Username = r.User.Username,
                    IsDeleted = r.User.IsDeleted
                }).ToList(),

                Permissions = role.CbesRoleWithPermissions.Where((q) => q.IsChecked == true).Select(p => new PermissionDto
                {
                    Id = p.Id,
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

        public static CbesRole GetRoleByIdAndUser(CbesManagementContext db, int id)
        {
            CbesRole? role = db.CbesRoles.Where(q => q.Id == id).Include((u) => u.CbesUserWithRoles).ThenInclude(q => q.User).FirstOrDefault();

            if (role == null)
            {
                return null; // หรือส่งคืน CbesRoleDto ว่าง
            }


            return role;
        }

        //  Delete ID
        public static CbesRole Delete(CbesManagementContext db, int id)
        {
            CbesRole cbe = GetRoleByIdAndUser(db, id);
            cbe.UpdateDate = DateTime.Now;
            cbe.IsDeleted = true;
            db.Entry(cbe).State = EntityState.Modified;
            db.SaveChanges();

            return cbe;
        }

        //  Cancel Delete ID
        public static CbesRole cancelDelete(CbesManagementContext db, int id)
        {
            CbesRole cbe = GetRoleByIdAndUser(db, id);
            cbe.UpdateDate = DateTime.Now;
            cbe.IsDeleted = false;
            db.Entry(cbe).State = EntityState.Modified;
            db.SaveChanges();

            return cbe;
        }
        //  Last Delete ID
        public static CbesRole lastDelete(CbesManagementContext db, int id)
        {
            CbesRole cbe = GetRoleByIdAndUser(db, id);
            cbe.UpdateDate = DateTime.Now;
            cbe.IsLastDelete = true;
            db.Entry(cbe).State = EntityState.Modified;
            db.SaveChanges();

            return cbe;
        }

        public static List<CbesRole> GetAllBin(CbesManagementContext db)
        {
            List<CbesRole> roles = db.CbesRoles.Where(q => q.IsDeleted == true && q.IsLastDelete == false).ToList();
            return roles;
        }
    }
}
