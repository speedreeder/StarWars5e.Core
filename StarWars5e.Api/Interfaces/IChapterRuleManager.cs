using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;

namespace StarWars5e.Api.Interfaces
{
    public interface IChapterRuleManager
    {
        Task<List<ChapterRules>> GetChapterRulesFromBlobContainer(string containerName, Language language);
        Task<ChapterRules> GetChapterRuleFromBlobContainer(string containerName, string chapterName, Language language);
    }
}
