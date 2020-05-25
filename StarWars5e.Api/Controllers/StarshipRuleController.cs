using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models;
using StarWars5e.Models.Enums;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StarshipRuleController : ControllerBase
    {
        private readonly IChapterRuleManager _chapterRuleManager;
        public StarshipRuleController(IChapterRuleManager chapterRuleManager)
        {
            _chapterRuleManager = chapterRuleManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChapterRules>>> Get(Language language = Language.en)
        {
            var chapterRules = await _chapterRuleManager.GetChapterRulesFromBlobContainer("starships-rules", language);

            return Ok(chapterRules);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<ChapterRules>> Get(string name, Language language = Language.en)
        {
            var chapterRule = await _chapterRuleManager.GetChapterRuleFromBlobContainer("starships-rules", name, language);

            return Ok(chapterRule);
        }
    }
}
