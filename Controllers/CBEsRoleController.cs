using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CBEsApi.Data;
using CBEsApi.Models;
using CBEsApi.Dtos.CBEsRole;
using System.Security.Claims;

namespace CBEsApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CBEsRoleController : ControllerBase
    {
        private readonly ILogger<CBEsRoleController> _logger;

        public CBEsRoleController(ILogger<CBEsRoleController> logger)
        {
            _logger = logger;
        }

        private CbesManagementContext _db = new CbesManagementContext();

        public class RequestRoleID
        {
            [Required]
            public int ID { get; set; }
        }

        [HttpGet(Name = "GetRoles")]
        public ActionResult GetRoles()
        {
            List<CbesRole> roles = CbesRole.GetAll(_db);

            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = roles
            });
        }

        [HttpGet("{id}", Name = "GetRole")]
        public ActionResult<Response> GetRole(int id)
        {
            CbesRoleDto role = CbesRole.GetRole(_db, id);

            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = role
            });
        }

        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/CBEsRole
        ///     
        ///     {
        ///         "name": "บทบาททดสอบ",
        ///         "createBy": 1,
        ///         "updateBy": 2,
        ///         "cbesRoleWithPermission": [
        ///             {
        ///                 "isCheck": true,
        ///                 "roleId": = 1,
        ///                 "permissionId: 1,
        ///                 "permission": { "id": 1, "name": "กำหนดสิทธิ์การใช้งานระบบ และกลุ่มผู้ใช้งาน"}
        ///             },
        ///             {
        ///                 "isCheck": true,
        ///                 "roleId": = 1,
        ///                 "permissionId: 2,
        ///                 "permission": { "id": 2, "name": "ประวัติการใช้งาน"}
        ///             }
        ///         ]
        ///     }
        ///     
        /// </remarks>
        [HttpPost(Name = "PostRolePermission")]
        public ActionResult<Response> PostRolePermission(CbesRoleDto createRole)
        {
            var userClaimsString = User.FindFirst("ID")?.Value;
            int userClaims = Convert.ToInt32(userClaimsString);

            CbesRole role = new CbesRole
            {
                Name = createRole.Name,
                CreateBy = userClaims,
                UpdateBy = userClaims,
            };

            foreach (var p in createRole.CbesRoleWithPermissions)
            {
                CbesRoleWithPermission rolePermissions = new CbesRoleWithPermission
                {
                    IsChecked = p.IsChecked,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    RoleId = role.Id, // This will be updated after saving role
                    IsDeleted = false,
                    PermissionId = p.PermissionId,
                    CreateBy = userClaims,
                    UpdateBy = userClaims,

                };

                role.CbesRoleWithPermissions.Add(rolePermissions);
            }

            role = CbesRole.Create(_db, role);

            return Ok(new Response
            {
                Status = 201,
                Message = "Role and Permissions Saved",
                Data = role
            });
        }

        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/CBEsRole
        ///     
        ///     {
        ///         "id": 1,
        ///         "name": "บทบาททดสอบ",
        ///         "permissions": [
        ///             {
        ///                 "id": 1
        ///             },
        ///             {
        ///                 "id": 3
        ///             }
        ///         ],
        ///         "createBy": 1,
        ///         "updateBy": 1
        ///     }
        ///     
        /// </remarks>
        [HttpPut(Name = "PutRolePermission")]
        public ActionResult<Response> PutRolePermission(CbesRoleDto updateRole)
        {
            var userClaimsString = User.FindFirst("ID")?.Value;
            int userClaims = Convert.ToInt32(userClaimsString);

            // Fetch role as CbesRole
            CbesRole existingRole = CbesRole.GetById(_db, updateRole.Id);

            if (existingRole == null)
            {
                return NotFound(new Response
                {
                    Status = 404,
                    Message = "Role not found",
                });
            }

            existingRole.Name = updateRole.Name;
            existingRole.UpdateBy = userClaims;
            existingRole.UpdateDate = DateTime.Now;

            // Update existing permissions
            foreach (var permission in updateRole.CbesRoleWithPermissions)
            {
                var existingPermission = existingRole.CbesRoleWithPermissions
                    .FirstOrDefault(p => p.PermissionId == permission.PermissionId);

                if (existingPermission != null)
                {
                    // Update existing permission
                    existingPermission.IsChecked = permission.IsChecked;
                    existingPermission.UpdateDate = DateTime.Now;
                    existingPermission.UpdateBy = userClaims;

                    // Set RoleId for the permission (assuming existingRole.Id is correct)
                    existingPermission.RoleId = existingRole.Id;
                }
            }

            // Save updates to the database
            existingRole = CbesRole.Update(_db, existingRole);

            return Ok(new Response
            {
                Status = 200,
                Message = "Role and Permissions Updated",
                Data = existingRole
            });
        }

        [HttpDelete("delete/{id}", Name = "DeleteRole")]
        public ActionResult DeleteRole(int id)
        {
            try
            {
                CbesRole cbe = CbesRole.Delete(_db, id);

                return Ok(new Response
                {
                    Status = 200,
                    Message = "Success",
                    Data = cbe
                });
            }
            catch
            {
                // ถ้าไม่พบข้อมูล user ตาม id ที่ระบุ
                return NotFound(new Response
                {
                    Status = 404,
                    Message = "User not found",
                    Data = null
                });
            }
        }

        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/CBEsRole/RoleWithUsers
        ///     
        ///     {
        ///         "id": 3,
        ///         "name": "ผู้รายงานผล",
        ///         "createDate": "2024-07-01T07:26:55.084Z",
        ///         "updateDate": "2024-07-01T07:26:55.084Z",
        ///         "isDeleted": false,
        ///         "isLastDelete": false,
        ///         "createBy": 1,
        ///         "updateBy": 1,
        ///         "users": [
        ///             {
        ///                 "id": 1,
        ///                 "fullname": "จักรกฤษ อ่อนส้มกฤษ",
        ///                 "username": "admin",
        ///                 "isDeleted": false
        ///             }
        ///         ]
        ///     }
        /// </remarks>
        [HttpPut("RoleWithUsers", Name = "PutRoleWithPermissions")]
        public ActionResult<Response> PutRoleWithPermissions(CbesManagementContext _db, CbesRoleDto updateRoleUsers)
        {
            try
            {
                // ดึงค่า userClaims และแปลงเป็น int
                var userClaimsString = User.FindFirst("ID")?.Value;
                int userClaims = Convert.ToInt32(userClaimsString);

                CbesRole oldRole = CbesRole.GetRoleByIdAndUser(_db, updateRoleUsers.Id);

                if (oldRole == null)
                {
                    return NotFound(new Response
                    {
                        Status = 404,
                        Message = "Role not found"
                    });
                }

                if (updateRoleUsers.CbesRoleWithPermissions == null || updateRoleUsers.CbesRoleWithPermissions.Count == 0)
                {
                    // ถ้าไม่มีผู้ใช้ใน updateRoleUsers.Users ให้ตั้งค่า IsDeleted เป็น true สำหรับผู้ใช้ทั้งหมด
                    foreach (var userRole in oldRole.CbesUserWithRoles)
                    {
                        userRole.IsDeleted = true;
                        userRole.UpdateBy = userClaims;
                        userRole.UpdateDate = DateTime.UtcNow;
                    }
                }
                else
                {
                    foreach (var u in updateRoleUsers.CbesRoleWithPermissions)
                    {
                        // ค้นหาผู้ใช้ที่มีอยู่ใน oldRole.CbesUserWithRoles
                        CbesUserWithRole? existingUserRole = oldRole.CbesUserWithRoles.FirstOrDefault(ur => ur.UserId == u.Id);

                        if (existingUserRole != null)
                        {
                            // ถ้ามีอยู่แล้วอัปเดต isDeleted
                            existingUserRole.UpdateBy = userClaims;
                            existingUserRole.UpdateDate = DateTime.UtcNow;
                        }
                        else
                        {
                            // ถ้าไม่มี สร้างใหม่
                            CbesUserWithRole userRole = new CbesUserWithRole
                            {
                                UserId = u.Id,
                                RoleId = oldRole.Id,
                                IsDeleted = false,
                                CreateBy = userClaims,
                                UpdateBy = userClaims,
                                CreateDate = DateTime.UtcNow,
                                UpdateDate = DateTime.UtcNow,
                            };

                            oldRole.CbesUserWithRoles.Add(userRole);
                        }
                    }
                }

                _db.SaveChanges();

                return Ok(new Response
                {
                    Status = 201,
                    Message = "Users with Role Saved",
                    Data = oldRole
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response
                {
                    Status = 500,
                    Message = $"Error: {ex.Message}",
                    Data = null,
                });
            }
        }

        [HttpGet("bin", Name = "GetRoleBin")]
        public ActionResult<Response> GetRoleBin()
        {
            List<CbesRole> roles = CbesRole.GetAllBin(_db);

            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = roles
            });
        }

        [HttpPut("bin/CancelDelete", Name = "UpdateDeleteRole")]
        public ActionResult<Response> UpdateDeleteRole(RequestRoleID request)
        {
            try
            {
                CbesRole cbe = CbesRole.cancelDelete(_db, request.ID);

                return Ok(new Response
                {
                    Status = 200,
                    Message = "Success",
                    Data = cbe
                });
            }
            catch
            {
                // ถ้าไม่พบข้อมูล user ตาม id ที่ระบุ
                return NotFound(new Response
                {
                    Status = 404,
                    Message = "User not found",
                    Data = null
                });
            }
        }

        [HttpDelete("bin/LastDelete/{id}", Name = "UpdateLastDeleteRole")]
        public ActionResult<Response> UpdateLastDeleteRole(int id)
        {
            try
            {
                CbesRole cbe = CbesRole.lastDelete(_db, id);

                return Ok(new Response
                {
                    Status = 200,
                    Message = "Success",
                    Data = cbe
                });
            }
            catch
            {
                // ถ้าไม่พบข้อมูล user ตาม id ที่ระบุ
                return NotFound(new Response
                {
                    Status = 404,
                    Message = "User not found",
                    Data = null
                });
            }
        }
    }
}