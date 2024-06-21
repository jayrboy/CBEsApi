using CBEsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CBEsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CBEsTargetResultController : ControllerBase
    {

        // Endpoint GET /api/CBEsTargetResult
        [HttpGet(Name = "GetAllTargetResult")]
        public ActionResult GetAllTargetResult(CbesProcess cbeProcess)
        {
            return Ok(cbeProcess);
        }

        // Endpoint GET /api/CBEsTargetResult/{id}
        [HttpGet("{id}", Name = "GetTargetResult")]
        public ActionResult GetTargetResult(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return Ok(id);
        }

        // Endpoint POST /api/CBEsTargetResult
        [HttpPost("target", Name = "PostTarget")]
        public ActionResult PostTarget(int? id, CbesProcessTarget cbesProcessTarget)
        {
            if (id == null)
            {
                return NotFound();
            }
            return Ok(cbesProcessTarget);
        }

        // Endpoint POST /api/CBEsTargetResult
        [HttpPost("result", Name = "PostResult")]
        public ActionResult PostResult(int? id, CbesProcessTarget cbesProcessTarget)
        {
            if (id == null)
            {
                return NotFound();
            }
            return Ok(cbesProcessTarget);
        }

        // Endpoint GET /api/CBEsTargetResult/history/{id}
        [HttpGet("history")]
        public ActionResult<string> GetAllHistory(Cbe cbe)
        {
            if (cbe == null)
            {
                return NotFound();
            }
            return Ok($"History for result {cbe}");
        }

        // Endpoint GET /api/CBEsTargetResult/history/{id}
        [HttpGet("history/{id}")]
        public ActionResult<string> GetHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return Ok($"History for result {id}");
        }
    }

}