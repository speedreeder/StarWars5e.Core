using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerHandbookRuleController : ControllerBase
    {
        private readonly IChapterRuleManager _chapterRuleManager;
        public PlayerHandbookRuleController(IChapterRuleManager chapterRuleManager)
        {
            _chapterRuleManager = chapterRuleManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChapterRules>>> Get()
        {
            var chapterRules = await _chapterRuleManager.GetChapterRulesFromBlobContainer("player-handbook-rules");

            return Ok(chapterRules);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<ChapterRules>> Get(string name)
        {
            var chapterRule = await _chapterRuleManager.GetChapterRuleFromBlobContainer("player-handbook-rules", name);

            return Ok(chapterRule);
        }
    }
}
