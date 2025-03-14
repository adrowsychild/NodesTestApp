using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Dto;
using Services.Interfaces;

namespace NodesTestApp.Controllers
{
    [Route("user/[controller]")]
    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly IJournalService service;

        public JournalController(IJournalService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JournalItem>>> GetJournalItems()
        {
            var result = await service.GetJournalItemsAsync();
            return Ok(result);
        }

        [HttpGet("getRange{skip}/{take}")]
        public async Task<ActionResult<IEnumerable<JournalItem>>> GetJournalItems(int? skip, int? take)
        {
            var result = await service.GetJournalItemsAsync(new JournalFilter() { Skip = skip, Take = take });
            return Ok(result);
        }

        [HttpPost("getRange{skip}/{take}")]
        public async Task<ActionResult<IEnumerable<JournalItem>>> GetJournalItems([FromBody] JournalBodyFilterDto body, int? skip, int take)
        {
            var result = await service.GetJournalItemsAsync(new JournalFilter() { Skip = skip, Take = take, BodyFilter = body });
            return Ok(result);
        }

        [HttpGet("getSingle{id}")]
        public async Task<ActionResult<IEnumerable<JournalItem>>> GetJournalItem(int id)
        {
            var result = await service.GetJournalItemAsync(id);
            return Ok(result);
        }
    }
}
