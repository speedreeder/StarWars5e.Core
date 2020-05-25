using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models;
using StarWars5e.Models.Enums;

namespace StarWars5e.Api.Controllers
{
    [Route("api/wretchedHivesRule")]
    [ApiController]
    public class WretchedHivesRuleController : ControllerBase
    {
        private readonly IChapterRuleManager _chapterRuleManager;
        public WretchedHivesRuleController(IChapterRuleManager chapterRuleManager)
        {
            _chapterRuleManager = chapterRuleManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChapterRules>>> Get(Language language = Language.en)
        {
            var chapterRules = await _chapterRuleManager.GetChapterRulesFromBlobContainer("wretched-hives-rules", language);

            return Ok(chapterRules);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<ChapterRules>> Get(string name, Language language = Language.en)
        {
            var chapterRule = await _chapterRuleManager.GetChapterRuleFromBlobContainer("wretched-hives-rules", name, language);

            return Ok(chapterRule);
        }
    }
}
