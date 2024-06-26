using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CBEsApi.Data;
using CBEsApi.Models;

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
            CbesRole role = CbesRole.GetById(_db, id);

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
        ///         "updateBy": 1
        ///     }
        ///     
        /// </remarks>
        [HttpPost(Name = "PostRole")]
        public ActionResult<Response> PostRoleUsers(CbesRole roleCreate)
        {
            CbesRole role = new CbesRole
            {
                Name = roleCreate.Name,
                CreateBy = roleCreate.CreateBy,
                UpdateBy = roleCreate.UpdateBy,
            };

            role = CbesRole.Create(_db, role);

            return Ok(new Response
            {
                Status = 201,
                Message = "Role Saved",
                Data = role
            });
        }

        [HttpDelete("{id}", Name = "DeleteRole")]
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

        [HttpPost("users", Name = "PostUserWithRole")]
        public ActionResult<Response> PostUserWithRole([FromBody] CbesUserWithRole userWithRole)
        {
            return Ok(new Response
            {
                Status = 201,
                Message = "Users with Role Saved",
                Data = userWithRole
            });
        }

        [HttpGet("users", Name = "GetUserWithRole")]
        public ActionResult<Response> GetUserWithRole()
        {
            List<CbesUserWithRole> usersWithRole = CbesUserWithRole.GetAll(_db);

            return Ok(new Response
            {
                Status = 200,
                Message = "RoleUsers Saved",
                Data = usersWithRole
            });
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

        [HttpPut("bin/cancelDelete/{id}", Name = "UpdateDeleteRole")]
        public ActionResult<Response> UpdateDeleteRole(int id)
        {
            try
            {
                CbesRole cbe = CbesRole.cancelDelete(_db, id);

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
        [HttpDelete("bin/lastDelete/{id}", Name = "UpdateLastDeleteRole")]

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