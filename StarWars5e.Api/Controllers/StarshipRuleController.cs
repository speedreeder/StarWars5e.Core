using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models;

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
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ChapterRules>>> Get()
        {
            var chapterRules = await _chapterRuleManager.GetChapterRulesFromBlobContainer("starships-rules");

            return Ok(chapterRules);
        }

        [HttpGet("{name}")]
        [AllowAnonymous]
        public async Task<ActionResult<ChapterRules>> Get(string name)
        {
            var chapterRule = await _chapterRuleManager.GetChapterRuleFromBlobContainer("starships-rules", name);

            return Ok(chapterRule);
        }
    }
}
