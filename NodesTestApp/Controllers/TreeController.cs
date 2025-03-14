using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Models;

namespace NodesTestApp.Controllers
{
    [Route("user/[controller]")]
    [ApiController]
    public class TreeController : ControllerBase
    {
        private readonly ITreeService service;

        public TreeController(ITreeService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tree>>> GetTrees()
        {
            var result = await service.GetTreesAsync();
            return Ok(result);
        }

        [HttpGet("{treeName}")]
        public async Task<ActionResult<Tree>> GetTree(string treeName)
        {
            var result = await service.GetTreeAsync(treeName);
            return Ok(result);
        }

        [HttpPost("{treeName}")]
        public async Task<ActionResult<Tree>> CreateTree(string treeName)
        {
            var result = await service.AddTreeAsync(treeName);
            return Ok(result);
        }
    }
}
