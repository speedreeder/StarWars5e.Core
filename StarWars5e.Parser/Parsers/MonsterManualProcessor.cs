using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Monster;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Parsers
{
    public class MonsterProcessor : BaseProcessor<Monster>
    {

        public override Task<List<Monster>> FindBlocks(List<string> lines)
        {
            var monsters = new List<Monster>();

            for (var i = 0; i < lines.Count; i++)
            {
                if (!lines[i].StartsWith("> ## ")) continue;

                var monsterEndIndex = lines.FindIndex(i, f => f == string.Empty || f == "___");
                var monsterLines = lines.Skip(i).Take(monsterEndIndex - i).CleanListOfStrings().ToList();


                monsters.Add(ParseMonster(monsterLines));
            }

            return Task.FromResult(monsters);
        }

        private static Monster ParseMonster(List<string> monsterLines)
        {
            var name = monsterLines.Find(f => f.StartsWith("> ## ")).Split("## ")[1].Trim().RemoveMarkdownCharacters();
            try
            {
                var monster = new Monster
                {
                    ContentTypeEnum = ContentType.Core,
                    PartitionKey = ContentType.Core.ToString(),
                    RowKey = name,
                    Name = name
                };

                var typeLine = monsterLines.Find(f => f.StartsWith(">*") || f.StartsWith("> *")).RemoveMarkdownCharacters().Trim().Split(',');

                monster.SizeEnum =
                    Enum.Parse<MonsterSize>(typeLine[0].RemoveMarkdownCharacters().Trim().Split(' ')[0], true);

                var parenIndex = typeLine[0].Split(' ', 2)[1].Trim().IndexOf('(');

                monster.Types.Add(parenIndex != -1
                    ? typeLine[0].Substring(typeLine[0].IndexOf('*') + 1).Split(' ', 2)[1].Replace("*", string.Empty).Trim().Remove(parenIndex-1).Trim()
                    : typeLine[0].Substring(typeLine[0].IndexOf('*') + 1).Split(' ', 2)[1].Replace("*", string.Empty).Trim());

                monster.Alignment = typeLine[1].Trim().RemoveMarkdownCharacters();
                monster.ArmorClass = int.Parse(Regex
                    .Match(monsterLines.Find(f => f.Contains("**Armor Class**")), @"\d+").Value);
                var armorTypeSplit = monsterLines.Find(f => f.Contains("**Armor Class**")).Split('(', ')');
                if (armorTypeSplit.ElementAtOrDefault(1) != null)
                {
                    monster.ArmorType = monsterLines.Find(f => f.Contains("**Armor Class**")).Split('(', ')')[1];
                }

                monster.HitPoints = int.Parse(Regex
                    .Match(monsterLines.Find(f => f.Contains("**Hit Points**")), @"\d+").Value);
                monster.HitPointRoll = monsterLines.Find(f => f.Contains("**Hit Points**")).Split('(', ')')[1];
                monster.Speed = int.Parse(Regex
                    .Match(monsterLines.Find(f => f.Contains("**Speed**")), @"\d+").Value);

                var attributeLine =
                    monsterLines[monsterLines.FindIndex(f => f.Contains("|STR|DEX|CON|INT|WIS|CHA|")) + 2];
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
                    monsterLines.Find(f => f.Contains("**Saving Throws**"))?.Split("**Saving Throws**")[1]
                        .Split(',').Select(s => s.Trim()).ToList();
                monster.Skills = monsterLines.Find(f => f.Contains("**Skills**"))?.Split("**Skills**")[1]
                    .Split(',')
                    .Select(s => s.Trim()).ToList();

                var damageVulnerabilitiesLine = monsterLines.Find(f => f.Contains("**Damage Vulnerabilities**",
                    StringComparison.InvariantCultureIgnoreCase));

                if (damageVulnerabilitiesLine != null)
                {
                    var damageVulnerabilitiesSplit =
                        Regex.Split(damageVulnerabilitiesLine
                                , @"\*\*Damage Vulnerabilities\*\*",
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

                var damageImmunitiesSplit = monsterLines.Find(f => f.Contains("**Damage Immunities**"))?
                    .Split("**Damage Immunities**")[1].Split(',').Select(s => s.Trim()).ToList();
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

                var damageResistancesSplit = monsterLines.Find(f => f.Contains("**Damage Resistances**"))?
                    .Split("**Damage Resistances**")[1].Split(',').Select(s => s.Trim()).ToList();
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

                var conditionImmunitiesSplit = monsterLines.Find(f => f.Contains("**Condition Immunities**"))?
                    .Split("**Condition Immunities**")[1].Split(',').Select(s => s.Trim()).ToList();
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

                monster.Senses = monsterLines.Find(f => f.Contains("**Senses**"))?
                    .Split("**Senses**")[1].Split(',').Select(s => s.Trim()).ToList();
                monster.Languages = monsterLines.Find(f => f.Contains("**Languages**"))?
                    .Split("**Languages**")[1].Split(new[] {",", "and"}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim()).ToList();

                var challengeLine = monsterLines.Find(f => f.Contains("**Challenge**"));
                var challengeRatingSplit = challengeLine
                    .Substring(challengeLine.LastIndexOf("**", StringComparison.InvariantCultureIgnoreCase)).Split(' ');
                //var challengeRatingNumbers = Regex.Matches(monsterLines.Find(f => f.Contains("**Challenge**")), @"[0-9]+(,[0-9]+)*");
                monster.ChallengeRating = challengeRatingSplit[1];
                monster.ExperiencePoints = int.Parse(Regex.Match(challengeRatingSplit[2], @"[0-9]+(,[0-9]+)*").Value,
                    NumberStyles.AllowThousands);

                monster.Behaviors = new List<MonsterBehavior>();
                var lastUnderScoreLine = monsterLines.FindLastIndex(f => f.Contains("___")) + 1;
                var firstTripleHash = monsterLines.FindIndex(f => f.StartsWith("> ###"));

                if (firstTripleHash != -1)
                {
                    var traitLines = monsterLines.Skip(lastUnderScoreLine).Take(firstTripleHash - lastUnderScoreLine).CleanListOfStrings().ToList();
                    if (traitLines.Any())
                    {
                        monster.Behaviors.AddRange(GetMonsterBehaviorsFromLines(traitLines, MonsterBehaviorType.Trait));
                    }

                    var secondTripleHash = monsterLines.FindIndex(firstTripleHash + 1, f => f.StartsWith("> ###"));
                    if (secondTripleHash != -1)
                    {
                        traitLines = monsterLines.Skip(firstTripleHash).Take(secondTripleHash - firstTripleHash)
                            .CleanListOfStrings().ToList();
                        var behaviorType = DetermineBehaviorType(monsterLines[firstTripleHash]);
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
                        traitLines = monsterLines.Skip(firstTripleHash).CleanListOfStrings().ToList();
                        var result = Enumerable.Range(0, traitLines.Count)
                            .Where(i => traitLines[i].StartsWith("> ***") || traitLines[i].StartsWith(">***"))
                            .ToList();
                        var behaviorType = DetermineBehaviorType(monsterLines[firstTripleHash]);
                        monster.Behaviors.AddRange(GetMonsterBehaviorsFromLines(traitLines, behaviorType));
                    }
                }
                else
                {
                    var traitLines = monsterLines.Skip(lastUnderScoreLine).ToList();

                    if (traitLines.Any())
                    {
                        monster.Behaviors.AddRange(GetMonsterBehaviorsFromLines(traitLines, MonsterBehaviorType.Trait));
                    }
                }

                return monster;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing {name}", e);
            }
        }

        private static MonsterBehaviorType DetermineBehaviorType(string behaviorTitleLine)
        {
            if (behaviorTitleLine.Contains("Legendary"))
            {
                return MonsterBehaviorType.Legendary;
            }

            if (behaviorTitleLine.Contains("Actions"))
            {
                return MonsterBehaviorType.Action;
            }

            if (behaviorTitleLine.Contains("Reactions"))
            {
                return MonsterBehaviorType.Reaction;
            }

            return MonsterBehaviorType.None;
        }

        private static IEnumerable<MonsterBehavior> GetMonsterBehaviorsFromLines(IReadOnlyList<string> behaviorLines, MonsterBehaviorType behaviorType)
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

                    var monsterBehavior = new MonsterBehavior
                    {
                        MonsterBehaviorTypeEnum = behaviorType,
                        Name = name,
                        Description = string.Join(" ", new List<string>(singleBehaviorLines.Skip(1)) { baseLine.Split("**")[2].Trim() }).RemoveMarkdownCharacters(),
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

                    var monsterBehavior = new MonsterBehavior
                    {
                        MonsterBehaviorTypeEnum = behaviorType,
                        Name = name,
                        Description = string.Join(" ", description).RemoveMarkdownCharacters(),
                        Restrictions = restrictions
                    };

                    if (Regex.IsMatch(baseLine.Split("***")[2].Trim(), @"\*.+\*"))
                    {
                        if (behaviorType == MonsterBehaviorType.Action)
                        {
                            if (baseLine.Split("***")[2].Trim().Contains("*melee", StringComparison.InvariantCultureIgnoreCase))
                            {
                                monsterBehavior.AttackTypeEnum = AttackType.MeleeWeapon;
                            }
                            else if (baseLine.Split("***")[2].Trim().Contains("*ranged",
                                StringComparison.InvariantCultureIgnoreCase))
                            {
                                monsterBehavior.AttackTypeEnum = AttackType.RangedWeapon;
                            }
                            else
                            {
                                monsterBehavior.AttackTypeEnum = AttackType.None;
                            }

                            var attackSplit = baseLine.Split("***")[2].Trim().Split(',').Select(s => s.Trim()).ToList();
                            var hitSplit = Regex.Split(baseLine.Split("***")[2].Trim(), @"\*Hit[:]*\*|Hit:");
                            var hitCommaSplit = hitSplit[0].Split(',').ToList();
                            var hitSpaceSplit = hitSplit[1].Split(' ').ToList();
                            monsterBehavior.AttackBonus = int.Parse(Regex.Match(attackSplit[0], @"-?\d+").Value);
                            monsterBehavior.Range = attackSplit[1].Trim();
                            monsterBehavior.NumberOfTargets = hitCommaSplit[2].Trim();

                            if (Regex.IsMatch(hitSplit[1].Trim(), @"^\d+.*\(.*\)"))
                            {
                                monsterBehavior.Damage = Regex.Match(hitSplit[1], @"-?\d+").Value.Trim();
                                monsterBehavior.DamageRoll = hitSplit[1].Split('(', ')').ElementAtOrDefault(1)?.Trim();
                                if (hitSpaceSplit.FindIndex(f =>
                                        f.Contains("damage", StringComparison.InvariantCultureIgnoreCase)) != -1)
                                {
                                    if (Enum.TryParse(hitSpaceSplit[hitSpaceSplit.FindIndex(f => f.Contains("damage", StringComparison.InvariantCultureIgnoreCase)) - 1],
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
