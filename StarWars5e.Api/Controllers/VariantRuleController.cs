using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariantRuleController : ControllerBase
    {
        private readonly IChapterRuleManager _chapterRuleManager;
        public VariantRuleController(IChapterRuleManager chapterRuleManager)
        {
            _chapterRuleManager = chapterRuleManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChapterRules>>> Get()
        {
            var chapterRules = await _chapterRuleManager.GetChapterRulesFromBlobContainer("variant-rules");

            return Ok(chapterRules);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<ChapterRules>> Get(string name)
        {
            var chapterRule = await _chapterRuleManager.GetChapterRuleFromBlobContainer("variant-rules", name);

            return Ok(chapterRule);
        }
    }
}
