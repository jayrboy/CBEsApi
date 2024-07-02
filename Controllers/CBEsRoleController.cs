using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CBEsApi.Data;
using CBEsApi.Models;
using CBEsApi.Dtos.CBEsRole;
using System.Security.Claims;

namespace CBEsApi.Controllers
{
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
            CbesRoleDto role = CbesRole.GetById(_db, id);

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
        [HttpPost(Name = "PostRolePermission")]
        public ActionResult<Response> PostRolePermission(CbesRolePermissionDto roleCreate)
        {
            CbesRole role = new CbesRole
            {
                Name = roleCreate.Name,
                CreateBy = roleCreate.CreateBy,
                UpdateBy = roleCreate.UpdateBy,
            };

            foreach (var p in roleCreate.Permissions)
            {
                CbesRoleWithPermission rolePermissions = new CbesRoleWithPermission
                {
                    IsChecked = true,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    IsDeleted = false,
                    PermissionId = p.Id,
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
        [Authorize]
        [HttpPut("RoleWithUsers", Name = "PutRoleWithUsers")]
        public ActionResult<Response> PutRoleWithUsers(CbesManagementContext _db, CbesRoleUserDto requestRole)
        {
            try
            {
                // ดึงค่า userClaims และแปลงเป็น int
                var userClaimsString = User.FindFirst("ID")?.Value;
                int userClaims = Convert.ToInt32(userClaimsString);

                CbesRole oldRole = CbesRole.GetRoleByIdAndUser(_db, requestRole.ID);

                if (oldRole == null)
                {
                    return NotFound(new Response
                    {
                        Status = 404,
                        Message = "Role not found"
                    });
                }

                if (requestRole.Users == null || requestRole.Users.Count == 0)
                {
                    // ถ้าไม่มีผู้ใช้ใน requestRole.Users ให้ตั้งค่า IsDeleted เป็น true สำหรับผู้ใช้ทั้งหมด
                    foreach (var userRole in oldRole.CbesUserWithRoles)
                    {
                        userRole.IsDeleted = true;
                        userRole.UpdateBy = userClaims;
                        userRole.UpdateDate = DateTime.UtcNow;
                    }
                }
                else
                {
                    foreach (var u in requestRole.Users)
                    {
                        // ค้นหาผู้ใช้ที่มีอยู่ใน oldRole.CbesUserWithRoles
                        CbesUserWithRole? existingUserRole = oldRole.CbesUserWithRoles.FirstOrDefault(ur => ur.UserId == u.Id);

                        if (existingUserRole != null)
                        {
                            // ถ้ามีอยู่แล้วอัปเดต isDeleted
                            existingUserRole.IsDeleted = requestRole.isDeleted;
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