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
    public class ExpandedContentController : ControllerBase
    {
        private readonly IChapterRuleManager _chapterRuleManager;
        public ExpandedContentController(IChapterRuleManager chapterRuleManager)
        {
            _chapterRuleManager = chapterRuleManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChapterRules>>> Get(Language language = Language.en)
        {
            var chapterRules = await _chapterRuleManager.GetChapterRulesFromBlobContainer("expanded-content", language);

            return Ok(chapterRules);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<ChapterRules>> Get(string name, Language language = Language.en)
        {
            var chapterRule = await _chapterRuleManager.GetChapterRuleFromBlobContainer("expanded-content", name, language);

            return Ok(chapterRule);
        }
    }
}
