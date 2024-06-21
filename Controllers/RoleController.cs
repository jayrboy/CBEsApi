using CBEsApi.Data;
using CBEsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CBEsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;

        public RoleController(ILogger<RoleController> logger)
        {
            _logger = logger;
        }

        private CbesManagementContext _db = new CbesManagementContext();

        [HttpGet(Name = "GetRoles")]
        public ActionResult<Response> GetRoles()
        {
            List<CbesRole> role = new List<CbesRole>();

            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = role
            });
        }

        [HttpGet("{id}", Name = "GetRole")]
        public ActionResult<Response> GetRole(int id)
        {
            CbesRole role = new CbesRole();

            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = role
            });
        }

        [HttpPost(Name = "PostRole")]
        public ActionResult<Response> PostRoleUsers([FromBody] CbesRole role)
        {
            return Ok(new Response
            {
                Status = 201,
                Message = "Role Saved",
                Data = role
            });
        }

        [HttpDelete("{id}", Name = "DeleteRole")]
        public ActionResult<Response> DeleteRole(int id)
        {
            return Ok(new Response
            {
                Status = 201,
                Message = "Role Saved",
                Data = id
            });
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
        public ActionResult<Response> GetUserWithRole([FromBody] List<CbesUserWithRole> usersWithRole)
        {
            return Ok(new Response
            {
                Status = 200,
                Message = "RoleUsers Saved",
                Data = usersWithRole
            });
        }

        [HttpGet("bin", Name = "GetRoleBin")]
        public ActionResult<Response> GetRoleBin([FromBody] List<CbesRole> role)
        {
            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = role
            });
        }

        [HttpPut("bin/delete/{id}", Name = "UpdateDeleteRole")]
        public ActionResult<Response> UpdateDeleteRole(int id)
        {
            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = id
            });
        }
        [HttpDelete("bin/last-delete/{id}", Name = "UpdateLastDeleteRole")]
        public ActionResult<Response> UpdateLastDeleteRole(int id)
        {
            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = id
            });
        }
    }
}