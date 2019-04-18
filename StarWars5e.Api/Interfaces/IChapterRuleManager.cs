using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models;

namespace StarWars5e.Api.Interfaces
{
    public interface IChapterRuleManager
    {
        Task<List<ChapterRules>> GetChapterRulesFromBlobContainer(string containerName);
        Task<ChapterRules> GetChapterRuleFromBlobContainer(string containerName, string chapterName);
    }
}
