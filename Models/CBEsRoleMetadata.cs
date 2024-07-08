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

        //TODO: Get Role Permissions and Users
        public static CbesRoleDto GetRole(CbesManagementContext db, int id)
        {
            CbesRole? role = db.CbesRoles
                                .Where(q => q.Id == id && q.IsDeleted != true)
                                .Include(q => q.CbesRoleWithPermissions)
                                    .ThenInclude((q) => q.Permission)
                                .Include(q => q.CbesUserWithRoles)
                                    .ThenInclude((q) => q.User)
                                .FirstOrDefault();

            if (role == null)
            {
                return new CbesRoleDto();
            }

            CbesRoleDto roleDto = new CbesRoleDto
            {
                Id = role.Id,
                Name = role.Name,
                UpdateDate = role.UpdateDate,
                IsDeleted = role.IsDeleted,
                IsLastDelete = role.IsLastDelete,
                CreateBy = role.CreateBy,
                UpdateBy = role.UpdateBy,
                CbesRoleWithPermissions = role.CbesRoleWithPermissions
                                            .Select(p => new CbesRoleWithPermissionDto
                                            {
                                                Id = p.Id,
                                                IsChecked = p.IsChecked,
                                                IsDeleted = p.IsDeleted,
                                                RoleId = p.RoleId,
                                                PermissionId = p.PermissionId,
                                                Permission = new PermissionDto
                                                {
                                                    Id = p.Permission.Id,
                                                    Name = p.Permission.Name,
                                                },
                                                CreateDate = p.CreateDate,
                                                UpdateDate = p.UpdateDate,
                                                CreateBy = p.CreateBy,
                                                UpdateBy = p.UpdateBy
                                            }).ToList(),
                CbesUserWithRole = role.CbesUserWithRoles.Select(user => new CbesRoleUserDto
                {
                    ID = user.Id,
                    IsDeleted = user.IsDeleted,
                    CreateDate = user.CreateDate,
                    UpdateDate = user.UpdateDate,
                    CreateBy = user.CreateBy,
                    UpdateBy = user.UpdateBy,
                    Users = new UserDto
                    {
                        Id = user.Id,
                        Fullname = user.User.Fullname,
                        Username = user.User.Username,
                        IsDeleted = user.IsDeleted,
                    }
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

        public static CbesRole GetById(CbesManagementContext db, int roleId)
        {
            var role = db.CbesRoles
                         .Include(q => q.CbesRoleWithPermissions)
                          .ThenInclude((q) => q.Permission)
                         .FirstOrDefault(q => q.Id == roleId);

            if (role == null)
            {
                throw new ArgumentException("Role not found");
            }

            return role;
        }

        public static CbesRole Update(CbesManagementContext db, CbesRole cbeRole)
        {
            var existingRole = db.CbesRoles.Find(cbeRole.Id);

            if (existingRole == null)
            {
                throw new ArgumentException("Role not found");
            }

            // Update existingRole properties from cbeRole
            existingRole.Name = cbeRole.Name;
            existingRole.UpdateBy = cbeRole.UpdateBy;
            existingRole.UpdateDate = DateTime.Now;
            existingRole.IsDeleted = cbeRole.IsDeleted;
            existingRole.IsLastDelete = cbeRole.IsLastDelete;

            db.CbesRoles.Update(existingRole);
            db.SaveChanges();

            return existingRole;
        }
    }
}
