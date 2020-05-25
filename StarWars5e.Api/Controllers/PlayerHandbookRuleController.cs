using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models;
using StarWars5e.Models.Enums;

namespace StarWars5e.Api.Controllers
{
    [Route("api/playerHandbookRule")]
    [ApiController]
    public class PlayerHandbookRuleController : ControllerBase
    {
        private readonly IChapterRuleManager _chapterRuleManager;
        public PlayerHandbookRuleController(IChapterRuleManager chapterRuleManager)
        {
            _chapterRuleManager = chapterRuleManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChapterRules>>> Get(Language language = Language.en)
        {
            var chapterRules = await _chapterRuleManager.GetChapterRulesFromBlobContainer($"player-handbook-rules", language);

            return Ok(chapterRules);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<ChapterRules>> Get(string name, Language language = Language.en)
        {
            var chapterRule = await _chapterRuleManager.GetChapterRuleFromBlobContainer($"player-handbook-rules", name, language);

            return Ok(chapterRule);
        }
    }
}
