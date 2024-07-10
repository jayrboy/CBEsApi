using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CBEsApi.Models;
using CBEsApi.Data;

namespace CBEsApi.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class CBEsPositionController : ControllerBase
    {
        private CbesManagementContext _db = new CbesManagementContext();

        [HttpGet(Name = "GetPositions")]
        public ActionResult GetPositions()
        {
            List<CbesPosition> positions = CbesPosition.GetAll(_db);

            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = positions
            });
        }

        [HttpGet("{id}", Name = "GetPosition")]
        public ActionResult GetPosition(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CbesPosition position = CbesPosition.GetById(_db, id);

            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = position
            });
        }

    }
}