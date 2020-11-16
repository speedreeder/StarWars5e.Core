using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Monster;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors
{
    public class MonsterProcessor : BaseProcessor<Monster>
    {
        private string _lastSectionText = null;
        private string _lastCreatureText = null;

        public override Task<List<Monster>> FindBlocks(List<string> lines)
        {
            var monsters = new List<Monster>();

            for (var i = 0; i < lines.Count; i++)
            {
                var canBeSectionText = lines[i].StartsWith("## ");
                var canBeFlavorText = lines[i].StartsWith("### ");

                if (canBeSectionText || canBeFlavorText)
                {
                    var flavorTextEndIndex = canBeSectionText 
                        ? lines.FindIndex(i, f => f == "___" || f.StartsWith("### ")) 
                        : lines.FindIndex(i, f => f == "___");

                    var flavorTextLines = lines.Skip(i)
                        .Take(flavorTextEndIndex - i)
                        .CleanListOfStrings()
                        .ToList();

                    if(flavorTextLines.Any())
                        ParseFlavorText(flavorTextLines);
                }

                if (!lines[i].StartsWith("> ## ")) continue;

                var monsterEndIndex = lines.FindIndex(i, f => f == string.Empty || f == "___");
                var monsterLines = lines.Skip(i).Take(monsterEndIndex - i).CleanListOfStrings().ToList();


                monsters.Add(ParseMonster(monsterLines));
            }

            return Task.FromResult(monsters);
        }

        private void ParseFlavorText(List<string> flavorTextLines)
        {
            _lastCreatureText = null;
            var isSectionText = flavorTextLines.First().StartsWith("## ");

            var key = flavorTextLines.Find(f => f.StartsWith("### ") || f.StartsWith("## "))
                                ?.RemoveHashtagCharacters()
                                ?.Trim();

            var textLines = flavorTextLines?.Select(x => x.Contains("<") || x.Contains(">") ? "" : x)
                        ?.Skip(1);

            var text = textLines.Any() ? textLines.Aggregate((x, y) => x + y) : null;

            if(text != null)
            {
                if(isSectionText)
                {
                    _lastSectionText = text;
                }
                else
                {
                    _lastCreatureText = text;
                }
            }
        }

        private Monster ParseMonster(List<string> monsterLines)
        {
            var name = monsterLines.Find(f => f.StartsWith("> ## ")).Split("## ")[1].Trim().RemoveMarkdownCharacters();

            try
            {
                var monster = new Monster
                {
                    ContentTypeEnum = ContentType.Core,
                    PartitionKey = ContentType.Core.ToString(),
                    RowKey = name,
                    Name = name,
                    FlavorText = _lastCreatureText ?? "",
                    SectionText = _lastSectionText ?? "",
                };

                var typeLine = monsterLines.Find(f => f.StartsWith(">*") || f.StartsWith("> *")).RemoveMarkdownCharacters().Trim().Split(',', '.');

                monster.SizeEnum =
                    Enum.Parse<MonsterSize>(typeLine[0].RemoveMarkdownCharacters().Trim().Split(' ')[0], true);

                //removes parenthesis, if needed
                typeLine[0] = Regex.Replace(typeLine[0], @"(\(+(\w* *)*\)+)", "");

                monster.Types.Add(typeLine.SafeAccess(0)
                    .Substring(typeLine
                        .SafeAccess(0)
                        .IndexOf('*') + 1)
                    .Split(' ', 2)
                    .SafeAccess(1)
                    .Replace("*", string.Empty)
                    .Trim());

                monster.Alignment = typeLine[1].Trim().RemoveMarkdownCharacters();
                monster.ArmorClass = int.Parse(Regex
                    .Match(monsterLines.Find(f => f.Contains(Localization.MonsterArmorClass)), @"\d+").Value);
                var armorTypeSplit = monsterLines.Find(f => f.Contains(Localization.MonsterArmorClass)).Split('(', ')');
                if (armorTypeSplit.ElementAtOrDefault(1) != null)
                {
                    monster.ArmorType = monsterLines.Find(f => f.Contains(Localization.MonsterArmorClass)).Split('(', ')')[1];
                }

                monster.HitPoints = int.Parse(Regex
                    .Match(monsterLines.Find(f => f.Contains(Localization.MonsterHitPoints)), @"\d+").Value);
                monster.HitPointRoll = monsterLines.Find(f => f.Contains(Localization.MonsterHitPoints)).Split('(', ')')
                    .SafeAccess(1) ?? "";
                monster.Speed = int.Parse(Regex
                    .Match(monsterLines.Find(f => f.Contains(Localization.MonsterSpeed)), @"\d+").Value);
                monster.Speeds = monsterLines.Find(f => f.Contains(Localization.MonsterSpeed)).Split("**")[2].Trim();

                var attributeLine =
                    monsterLines[monsterLines.FindIndex(f => f.Contains(Localization.MonsterAttributes)) + 2];
                var attributeNumbers = Regex.Matches(attributeLine, @"-?\d+");
                monster.Strength = int.Parse(attributeNumbers[0].ToString());
                monster.StrengthModifier = int.Parse(attributeNumbers[1].ToString());
                monster.Dexterity = int.Parse(attributeNumbers[2].ToString());
                monster.DexterityModifier = int.Parse(attributeNumbers[3].ToString());
                monster.Constitution = int.Parse(attributeNumbers[4].ToString());
                monster.ConstitutionModifier = int.Parse(attributeNumbers[5].ToString());
                monster.Intelligence = int.Parse(attributeNumbers[6].ToString());
                monster.IntelligenceModifier = int.Parse(attributeNumbers[7].ToString());
                monster.Wisdom = int.Parse(attributeNumbers[8].ToString());
                monster.WisdomModifier = int.Parse(attributeNumbers[9].ToString());
                monster.Charisma = int.Parse(attributeNumbers[10].ToString());
                monster.CharismaModifier = int.Parse(attributeNumbers[11].ToString());

                monster.SavingThrows =
                    monsterLines.Find(f => f.Contains(Localization.MonsterSavingThrows))?.Split(Localization.MonsterSavingThrows)[1]
                        .Split(',').Select(s => s.Trim()).ToList();
                monster.Skills = monsterLines.Find(f => f.Contains(Localization.MonsterSkills))?.Split(Localization.MonsterSkills)[1]
                    .Split(',')
                    .Select(s => s.Trim()).ToList();

                var damageVulnerabilitiesLine = monsterLines.Find(f => f.Contains(Localization.MonsterDamageVulnerabilities,
                    StringComparison.InvariantCultureIgnoreCase));

                if (damageVulnerabilitiesLine != null)
                {
                    var damageVulnerabilitiesSplit =
                        Regex.Split(damageVulnerabilitiesLine
                                , Localization.MonsterDamageVulnerabilitiesPattern,
                                RegexOptions.IgnoreCase)
                            [1].Split(',').Select(s => s.Trim()).ToList();
                    monster.DamageVulnerabilitiesParsed = damageVulnerabilitiesSplit
                        .Where(d => Enum.TryParse(d, true, out DamageType _))
                        .Select(s => Enum.Parse<DamageType>(s, true)).ToList();
                    monster.DamageVulnerabilitiesOther = damageVulnerabilitiesSplit.Any(d => !Enum.TryParse(d, true, out DamageType _))
                        ? damageVulnerabilitiesSplit
                            .Where(d => !Enum.TryParse(d, true, out DamageType _)).ToList()
                        : null;
                }

                var damageImmunitiesSplit = monsterLines.Find(f => f.Contains(Localization.MonsterDamageImmunities))?
                    .Split(Localization.MonsterDamageImmunities)[1].Split(',').Select(s => s.Trim()).ToList();
                if (damageImmunitiesSplit != null)
                {
                    monster.DamageImmunitiesParsed = damageImmunitiesSplit
                        .Where(d => Enum.TryParse(d, true, out DamageType _))
                        .Select(s => Enum.Parse<DamageType>(s, true)).ToList();
                    monster.DamageImmunitiesOther = damageImmunitiesSplit
                        .Any(d => !Enum.TryParse(d, true, out DamageType _))
                        ? damageImmunitiesSplit
                            .Where(d => !Enum.TryParse(d, true, out DamageType _)).ToList()
                        : null;
                }

                var damageResistancesSplit = monsterLines.Find(f => f.Contains(Localization.MonsterDamageResistances))?
                    .Split(Localization.MonsterDamageResistances)[1].Split(',').Select(s => s.Trim()).ToList();
                if (damageResistancesSplit != null)
                {
                    monster.DamageResistancesParsed = damageResistancesSplit
                        .Where(d => Enum.TryParse(d, true, out DamageType _))
                        .Select(s => Enum.Parse<DamageType>(s, true)).ToList();
                    monster.DamageResistancesOther = damageResistancesSplit
                        .Any(d => !Enum.TryParse(d, true, out DamageType _))
                        ? damageResistancesSplit
                            .Where(d => !Enum.TryParse(d, true, out DamageType _)).ToList()
                        : null;
                }

                var conditionImmunitiesSplit = monsterLines.Find(f => f.Contains(Localization.MonsterConditionImmunities))?
                    .Split(Localization.MonsterConditionImmunities)[1].Split(',').Select(s => s.Trim()).ToList();
                if (conditionImmunitiesSplit != null)
                {
                    monster.ConditionImmunitiesParsed = conditionImmunitiesSplit
                        .Where(d => Enum.TryParse(d, true, out Condition _)).Select(s => Enum.Parse<Condition>(s, true))
                        .ToList();
                    monster.ConditionImmunitiesOther = conditionImmunitiesSplit
                        .Any(d => !Enum.TryParse(d, true, out Condition _))
                        ? conditionImmunitiesSplit
                            .Where(d => !Enum.TryParse(d, true, out Condition _)).ToList()
                        : null;
                }

                monster.Senses = monsterLines.Find(f => f.Contains(Localization.MonsterSenses))?
                    .Split(Localization.MonsterSenses)[1].Split(',').Select(s => s.Trim()).ToList();
                monster.Languages = monsterLines.Find(f => f.Contains(Localization.MonsterLanguages))?
                    .Split(Localization.MonsterLanguages)[1].Split(new[] {",", Localization.and}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim()).ToList() ?? new List<string> { "—" };

                var challengeLine = monsterLines.Find(f => f.Contains(Localization.MonsterChallenge));
                var challengeRatingSplit = challengeLine
                    .Substring(challengeLine.LastIndexOf("**", StringComparison.InvariantCultureIgnoreCase)).Split(' ');
                //var challengeRatingNumbers = Regex.Matches(monsterLines.Find(f => f.Contains("**Challenge**")), @"[0-9]+(,[0-9]+)*");
                monster.ChallengeRating = challengeRatingSplit[1];
                var didParseXP = int.TryParse(Regex.Match(challengeRatingSplit.SafeAccess(2) ?? "", @"[0-9]+(,[0-9]+)*").Value,
                    NumberStyles.AllowThousands, null,  out var parsedXP);
                monster.ExperiencePoints = didParseXP ? parsedXP : 0;

                ParseBehaviorLines(monster, monsterLines);

                return monster;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing {name}", e);
            }
        }

        private void ParseBehaviorLines(Monster monster, List<string> monsterLines)
        {
            var firstTripleHashIndex = monsterLines.FindIndex(f => f.StartsWith("> ###"));
            var lineAfterLastUnderscoreIndex = monsterLines.Select((line, index) => new { line, index })
                .Last(s => s.line.Contains("___") && s.index < firstTripleHashIndex).index + 1;

            if (firstTripleHashIndex != -1)
            {
                var traitLines = monsterLines.Skip(lineAfterLastUnderscoreIndex).Take(firstTripleHashIndex - lineAfterLastUnderscoreIndex).CleanListOfStrings().ToList();
                if (traitLines.Any())
                {
                    monster.Behaviors.AddRange(GetMonsterBehaviorsFromLines(traitLines, MonsterBehaviorType.Trait));
                }

                var secondTripleHash = monsterLines.FindIndex(firstTripleHashIndex + 1, f => f.StartsWith("> ###"));
                if (secondTripleHash != -1)
                {
                    traitLines = monsterLines.Skip(firstTripleHashIndex).Take(secondTripleHash - firstTripleHashIndex)
                        .CleanListOfStrings().ToList();
                    var behaviorType = DetermineBehaviorType(monsterLines[firstTripleHashIndex]);
                    monster.Behaviors.AddRange(GetMonsterBehaviorsFromLines(traitLines, behaviorType));

                    var thirdTripleHash = monsterLines.FindIndex(secondTripleHash + 1, f => f.StartsWith("> ###"));
                    if (thirdTripleHash != -1)
                    {
                        traitLines = monsterLines.Skip(secondTripleHash).Take(thirdTripleHash - secondTripleHash)
                            .CleanListOfStrings().ToList();
                        behaviorType = DetermineBehaviorType(monsterLines[secondTripleHash]);
                        monster.Behaviors.AddRange(GetMonsterBehaviorsFromLines(traitLines, behaviorType));

                        traitLines = monsterLines.Skip(thirdTripleHash).CleanListOfStrings().ToList();
                        behaviorType = DetermineBehaviorType(monsterLines[thirdTripleHash]);
                        monster.Behaviors.AddRange(GetMonsterBehaviorsFromLines(traitLines, behaviorType));
                    }
                    else
                    {
                        traitLines = monsterLines.Skip(secondTripleHash).CleanListOfStrings().ToList();
                        behaviorType = DetermineBehaviorType(monsterLines[secondTripleHash]);
                        monster.Behaviors.AddRange(GetMonsterBehaviorsFromLines(traitLines, behaviorType));
                    }
                }
                else
                {
                    traitLines = monsterLines.Skip(firstTripleHashIndex).CleanListOfStrings().ToList();
                    var result = Enumerable.Range(0, traitLines.Count)
                        .Where(i => traitLines[i].StartsWith("> ***") || traitLines[i].StartsWith(">***"))
                        .ToList();
                    var behaviorType = DetermineBehaviorType(monsterLines[firstTripleHashIndex]);
                    monster.Behaviors.AddRange(GetMonsterBehaviorsFromLines(traitLines, behaviorType));
                }
            }
            else
            {
                var traitLines = monsterLines.Skip(lineAfterLastUnderscoreIndex).ToList();

                if (traitLines.Any())
                {
                    monster.Behaviors.AddRange(GetMonsterBehaviorsFromLines(traitLines, MonsterBehaviorType.Trait));
                }
            }
        }

        private MonsterBehaviorType DetermineBehaviorType(string behaviorTitleLine)
        {
            if (behaviorTitleLine.Contains(Localization.MonsterBehaviorLegendary))
            {
                return MonsterBehaviorType.Legendary;
            }

            if (behaviorTitleLine.Contains(Localization.MonsterBehaviorActions))
            {
                return MonsterBehaviorType.Action;
            }

            if (behaviorTitleLine.Contains(Localization.MonsterBehaviorReactions))
            {
                return MonsterBehaviorType.Reaction;
            }

            return MonsterBehaviorType.None;
        }

        private IEnumerable<MonsterBehavior> GetMonsterBehaviorsFromLines(IReadOnlyList<string> behaviorLines, MonsterBehaviorType behaviorType)
        {
            var monsterBehaviors = new List<MonsterBehavior>();

            List<int> behaviorLinesIndexes;
            if (behaviorType == MonsterBehaviorType.Legendary)
            {
                behaviorLinesIndexes = Enumerable.Range(0, behaviorLines.Count)
                    .Where(i => behaviorLines[i].StartsWith("> **") || behaviorLines[i].StartsWith(">**"))
                    .ToList();
            }
            else
            {
                behaviorLinesIndexes = Enumerable.Range(0, behaviorLines.Count)
                    .Where(i => behaviorLines[i].StartsWith("> ***") || behaviorLines[i].StartsWith(">***"))
                    .ToList();
            }
 
            for (var i = 0; i < behaviorLinesIndexes.Count; i++)
            {
                List<string> singleBehaviorLines;
                if (i + 1 < behaviorLinesIndexes.Count)
                {
                    singleBehaviorLines = behaviorLines.Skip(behaviorLinesIndexes[i])
                        .Take(behaviorLinesIndexes[i + 1] - behaviorLinesIndexes[i]).ToList();
                }
                else
                {
                    singleBehaviorLines = behaviorLines.Skip(behaviorLinesIndexes[i]).ToList();
                }

                if (behaviorType == MonsterBehaviorType.Legendary)
                {
                    var baseLine = singleBehaviorLines.First(b => b.StartsWith("> **") || b.StartsWith(">**"));
                    var baseLineSplit = baseLine.Split(new[] {"**", "**"}, StringSplitOptions.None);
                    var parenIndex = baseLineSplit[1].IndexOf('(');

                    var name = baseLineSplit[1].Trim().Replace(".", string.Empty);
                    string restrictions = null;
                    if (parenIndex != -1)
                    {
                        name = baseLineSplit[1].Remove(parenIndex).Trim();

                        var restrictionsSplit = baseLine.Split('(', ')');
                        if (restrictionsSplit.ElementAtOrDefault(1) != null)
                        {
                            restrictions = restrictionsSplit[1].Trim();
                        }
                    }

                    var descriptionFinal = string
                        .Join(" ", new List<string>(singleBehaviorLines.Skip(1)) {baseLine.Split("**")[2].Trim()})
                        .RemoveMarkdownCharacters();

                    var monsterBehavior = new MonsterBehavior
                    {
                        MonsterBehaviorTypeEnum = behaviorType,
                        Name = name.RemoveMarkdownCharacters(),
                        Description = descriptionFinal,
                        DescriptionWithLinks = descriptionFinal,
                        Restrictions = restrictions
                    };

                    monsterBehaviors.Add(monsterBehavior);
                }
                else
                {
                    var baseLine = singleBehaviorLines.First(b => b.StartsWith("> **") || b.StartsWith(">**"));
                    var description = singleBehaviorLines.Skip(1).ToList();
                    description.Insert(0, baseLine.Split("***")[2].Trim());

                    var baseLineSplit = baseLine.Split(new[] { "***", "***" }, StringSplitOptions.None);
                    var parenIndex = baseLineSplit[1].IndexOf('(');

                    var name = baseLineSplit[1].Trim().Replace(".", string.Empty);
                    string restrictions = null;
                    if (parenIndex != -1)
                    {
                        name = baseLineSplit[1].Remove(parenIndex).Trim();

                        var restrictionsSplit = baseLine.Split('(', ')');
                        if (restrictionsSplit.ElementAtOrDefault(1) != null)
                        {
                            restrictions = restrictionsSplit[1].Trim();
                        }
                    }

                    var descriptionFinal = string
                        .Join(" ", new List<string>(singleBehaviorLines.Skip(1)) { baseLine.Split("**")[2].Trim() })
                        .RemoveMarkdownCharacters();

                    var descriptionWithLinks = descriptionFinal;

                    var castingLevels = new List<string>
                    {
                        Localization.AtWill, Localization.FirstLevel, Localization.SecondLevel, Localization.ThirdLevel,
                        Localization.FourthLevel, Localization.FifthLevel, Localization.SixthLevel,
                        Localization.SeventhLevel, Localization.EighthLevel, Localization.NinthLevel
                    };
                    castingLevels.AddRange(castingLevels.Select(c => c.Replace(' ', '-')).ToList());

                    if (name.Contains("casting", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var lines = new List<string>(singleBehaviorLines);

                        for (var j = 0; j < lines.Count; j++)
                        {
                            var c = castingLevels.FirstOrDefault(c => Regex.IsMatch(lines[j], @"^>\s*.*:", RegexOptions.IgnoreCase));
                            if (c != null)
                            {
                                var powerLineSplit = lines[j].Split(':');
                                var powers = powerLineSplit[1].RemoveMarkdownCharacters().Split(',')
                                    .Select(s => s.Trim());
                                var powersUpdated = new List<string>();
                                foreach (var power in powers)
                                {
                                    powersUpdated.Add($"[{power}](#{Uri.EscapeUriString(power)})");
                                }

                                lines[j] = $"{powerLineSplit[0]}: *{string.Join(", ", powersUpdated)}*";
                            }
                        }

                        descriptionWithLinks = string
                            .Join("\r\n", lines)
                            .RemoveMarkdownCharacters();
                    }

                    var monsterBehavior = new MonsterBehavior
                    {
                        MonsterBehaviorTypeEnum = behaviorType,
                        Name = name.RemoveMarkdownCharacters(),
                        Description = descriptionFinal,
                        DescriptionWithLinks = descriptionWithLinks,
                        Restrictions = restrictions
                    };

                    if (Regex.IsMatch(baseLine.Split("***")[2].Trim(), @"\*.+\*"))
                    {
                        if (behaviorType == MonsterBehaviorType.Action)
                        {
                            if (baseLine.Split("***")[2].Trim().Contains($"*{Localization.melee}", StringComparison.InvariantCultureIgnoreCase))
                            {
                                monsterBehavior.AttackTypeEnum = AttackType.MeleeWeapon;
                            }
                            else if (baseLine.Split("***")[2].Trim().Contains($"*{Localization.ranged}",
                                StringComparison.InvariantCultureIgnoreCase))
                            {
                                monsterBehavior.AttackTypeEnum = AttackType.RangedWeapon;
                            }
                            else
                            {
                                monsterBehavior.AttackTypeEnum = AttackType.None;
                            }

                            var attackSplit = baseLine.Split("***")[2].Trim().Split(',').Select(s => s.Trim()).ToList();
                            var hitSplit = Regex.Split(baseLine.Split("***")[2].Trim(), Localization.MonsterHitSplitPattern);
                            var hitCommaSplit = hitSplit.SafeAccess(0)?.Split(',')?.ToList() ?? new List<string>();
                            var hitSpaceSplit = hitSplit.SafeAccess(1)?.Split(' ')?.ToList() ?? new List<string>();
                            var didParseAttBonus = int.TryParse(Regex.Match(attackSplit[0], @"-?\d+").Value, out var parsedAttBonus);
                            monsterBehavior.AttackBonus = didParseAttBonus ? parsedAttBonus : 0;
                            monsterBehavior.Range = attackSplit.ToArray().SafeAccess(1)?.Trim() ?? "";
                            monsterBehavior.NumberOfTargets = hitCommaSplit.ToArray().SafeAccess(2)?.Trim();

                            if (Regex.IsMatch(hitSplit.SafeAccess(1)?.Trim() ?? string.Empty, @"^\d+.*\(.*\)"))
                            {
                                monsterBehavior.Damage = Regex.Match(hitSplit.SafeAccess(1) ?? string.Empty, @"-?\d+").Value.Trim();
                                monsterBehavior.DamageRoll = hitSplit.SafeAccess(1)?.Split('(', ')').ElementAtOrDefault(1)?.Trim();
                                if (hitSpaceSplit.FindIndex(f =>
                                        f.Contains(Localization.damage, StringComparison.InvariantCultureIgnoreCase)) != -1)
                                {
                                    if (Enum.TryParse(hitSpaceSplit[hitSpaceSplit.FindIndex(f => f.Contains(Localization.damage, StringComparison.InvariantCultureIgnoreCase)) - 1],
                                            true, out DamageType damageType) &&
                                        Enum.IsDefined(typeof(DamageType), damageType))
                                    {
                                        monsterBehavior.DamageTypeEnum = damageType;
                                    }
                                    else
                                    {
                                        monsterBehavior.DamageTypeEnum = DamageType.Unknown;
                                    }
                                }
                            }
                        }
                    }

                    monsterBehaviors.Add(monsterBehavior);
                }
            }
            
            return monsterBehaviors;
        }
    }
}
