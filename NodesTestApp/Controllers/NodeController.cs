using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Dto;
using Services.Interfaces;

namespace NodesTestApp.Controllers
{
    [Route("user/[controller]")]
    [ApiController]
    public class NodeController : ControllerBase
    {
        private readonly INodeService service;

        public NodeController(INodeService service)
        {
            this.service = service;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Node>> CreateNode([FromBody] AddNodeDto dto)
        {
            var result = await service.AddNodeAsync(dto);
            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<Node>> DeleteNode([FromBody] DeleteNodeDto dto)
        {
            await service.DeleteNodeAsync(dto);
            return Ok();
        }

        [HttpPut("rename")]
        public async Task<ActionResult<Node>> RenameNode([FromBody] RenameNodeDto dto)
        {
            await service.RenameNodeAsync(dto);
            return Ok();
        }
    }
}
