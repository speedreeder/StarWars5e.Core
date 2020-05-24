using Newtonsoft.Json;
using NUnit.Framework;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Monster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;

namespace StarWars5e.ParserTests.MonsterManual
{
    [TestFixture]
    //most of the tests are hardcoded just to test parsing algorithms with the 
    //apparent standard markdown patterns in the monster text.
    //modify mm_sample.txt with caution.
    public class MonsterManualProcessorTests : BaseTestSetup
    {
        private IBaseProcessor<Monster> _monsterProcessor;
        private List<string> _filesToParse;

        [SetUp]
        public void Setup()
        {
            _monsterProcessor = new MonsterProcessor();
            _filesToParse = new List<string> { "TestData.mm_sample.txt" };
        }

        [Test]
        public async Task ParsedSampleFile()
        {
            var result = await _monsterProcessor.Process(_filesToParse, new LocalizationEn());
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task ParsedSampleFile_AssertValues()
        {
            var monsterResult = (await _monsterProcessor.Process(_filesToParse, new LocalizationEn())).First();

            Assert.AreEqual("Adolescent Acklay", monsterResult.Name);
            Assert.AreEqual(MonsterSize.Huge, Enum.Parse<MonsterSize>(monsterResult.Size));
            Assert.AreEqual("unaligned", monsterResult.Alignment);
            Assert.AreEqual(95, monsterResult.HitPoints);
            Assert.AreEqual(40, monsterResult.Speed);
            Assert.AreEqual(13, monsterResult.ArmorClass);
            Assert.AreEqual("natural armor", monsterResult.ArmorType);
            Assert.AreEqual(22, monsterResult.Strength);
            Assert.AreEqual(6, monsterResult.StrengthModifier);
            Assert.AreEqual(9, monsterResult.Dexterity);
            Assert.AreEqual(-1, monsterResult.DexterityModifier);
            Assert.AreEqual(17, monsterResult.Constitution);
            Assert.AreEqual(3, monsterResult.ConstitutionModifier);
            Assert.AreEqual(3, monsterResult.Intelligence);
            Assert.AreEqual(-4, monsterResult.IntelligenceModifier);
            Assert.AreEqual(11, monsterResult.Wisdom);
            Assert.AreEqual(0, monsterResult.WisdomModifier);
            Assert.AreEqual(5, monsterResult.Charisma);
            Assert.AreEqual(-3, monsterResult.CharismaModifier);
            Assert.Contains("—", monsterResult.Languages);
            Assert.AreEqual("5", monsterResult.ChallengeRating);
            Assert.Contains("passive Perception 10", monsterResult.Senses);
            Assert.AreEqual(3, monsterResult.Behaviors.Count);
            Assert.IsNotEmpty(monsterResult.ConditionImmunities);
            Assert.AreEqual("[\"Charmed\",\"Frightened\",\"Paralyzed\",\"Petrified\",\"Prone\",\"Restrained\",\"Stunned\"]"
                , monsterResult.ConditionImmunitiesParsedJson);
            Assert.IsEmpty(monsterResult.DamageResistances);
            Assert.AreEqual("", monsterResult.DamageResistancesParsedJson);
        }

        [Test]
        public async Task ParsedSampleFile_AssertBehaviors()
        {
            var monsterResult = (await _monsterProcessor.Process(_filesToParse, new LocalizationEn())).First();

            var traitBehavior = monsterResult.Behaviors.FirstOrDefault(x => x.MonsterBehaviorTypeEnum == MonsterBehaviorType.Trait);

            Assert.NotNull(traitBehavior);
            Assert.IsNotEmpty(traitBehavior.Description);

            var actionBehavior = monsterResult.Behaviors.FirstOrDefault(x => x.MonsterBehaviorTypeEnum == MonsterBehaviorType.Action);

            Assert.AreEqual(AttackType.MeleeWeapon, actionBehavior.AttackTypeEnum);
            Assert.AreEqual("Bite", actionBehavior.Name);
            Assert.AreEqual(9, actionBehavior.AttackBonus);
            Assert.AreEqual("reach 5 ft.", actionBehavior.Range);
            Assert.AreEqual("one target.", actionBehavior.NumberOfTargets);
            Assert.AreEqual(DamageType.Kinetic, actionBehavior.DamageTypeEnum);
            Assert.AreEqual("4d8+6", actionBehavior.DamageRoll);
        }

        [Test]
        public async Task ParsedSampleFile_AssertFlavorTextParsed()
        {
            var monsterResult = (await _monsterProcessor.Process(_filesToParse, new LocalizationEn())).First();

            Assert.IsNotEmpty(monsterResult.SectionText);
            Assert.AreEqual("Acklays are amphibious reptillian crustaceans with six deadly claws and razor-sharp teeth native to the planet Vendaxa. They are often used as execution beasts or fodder for gladiatorial arenas.", monsterResult.SectionText);

            new JsonSerializer().Serialize(TestContext.Out, monsterResult);
        }

        [TestCase("TestData.mm_sample.txt")]
        [TestCase("SNV_Content.txt")]
        public async Task GenericParse_AssertValues(string fileName)
        {
            _filesToParse = new List<string> { fileName };

            var monsterResult = (await _monsterProcessor.Process(_filesToParse, new LocalizationEn()));

            Assert.Multiple(() =>
            {
                foreach(var monster in monsterResult)
                {
                    try
                    {
                        var doesNotHaveFlavorText = monster.FlavorText == null || monster.FlavorText == string.Empty;
                        var doesNotHaveSectionText = monster.SectionText == null || monster.SectionText == string.Empty;

                        if (doesNotHaveFlavorText)
                            Assert.Warn($"{monster.Name} missing flavor text");

                        if (doesNotHaveSectionText)
                            Assert.Warn($"{monster.Name} missing section text");

                        Assert.IsNotEmpty(monster.Name);
                        Assert.IsNotEmpty(monster.Size);
                        Assert.IsNotEmpty(monster.Alignment);
                        Assert.Greater(monster.HitPoints, -1);
                        Assert.Greater(monster.Speed, -1);
                        Assert.Greater(monster.ArmorClass, 0);
                        Assert.IsNotEmpty(monster.ArmorType);
                        Assert.Greater(monster.Strength, -1);
                        Assert.IsNotNull(monster.StrengthModifier);
                        Assert.Greater(monster.Dexterity, -1);
                        Assert.IsNotNull(monster.DexterityModifier);
                        Assert.Greater(monster.Constitution, -1);
                        Assert.IsNotNull(monster.ConstitutionModifier);
                        Assert.Greater(monster.Intelligence, -1);
                        Assert.IsNotNull(monster.IntelligenceModifier);
                        Assert.Greater(monster.Wisdom, -1);
                        Assert.IsNotNull(monster.WisdomModifier);
                        Assert.Greater(monster.Charisma, -1);
                        Assert.IsNotNull(monster.CharismaModifier);
                        Assert.IsNotNull(monster.Languages);
                        Assert.IsNotEmpty(monster.Languages);
                        Assert.IsNotEmpty(monster.ChallengeRating);
                        Assert.IsNotNull(monster.Senses, $"Expected senses to not be null on {monster.Name}");
                        Assert.IsNotNull(monster.Behaviors);

                    }
                    catch(AssertionException e)
                    {
                        new JsonSerializer().Serialize(TestContext.Out, monster);
                        throw e;
                    }
                }
            });

            Assert.Pass(); //this is dumb that nunit requires this be put here.
        }

        [TestCase("TestData.mm_sample.txt")]
        [TestCase("SNV_Content.txt")]
        public async Task ParsedGeneric_AssertBehaviors(string fileName)
        {
            _filesToParse = new List<string> { fileName };
            var monsterResult = (await _monsterProcessor.Process(_filesToParse, new LocalizationEn()));

            foreach(var monster in monsterResult)
            {
                try
                {
                    Assert.NotNull(monster.Behaviors);
                    Assert.IsNotEmpty(monster.Behaviors);

                    foreach (var action in monster.Behaviors)
                    {
                        Assert.NotNull(action.AttackBonus);
                        Assert.NotNull(action.AttackType);
                        Assert.NotNull(action.AttackTypeEnum);
                        Assert.NotNull(action.DamageTypeEnum);
                        Assert.IsNotNull(action.DamageType);

                        Assert.IsNotEmpty(action.Damage);
                        Assert.IsNotEmpty(action.DamageRoll);
                        Assert.IsNotEmpty(action.Description);
                    }
                } catch(AssertionException e)
                {
                    new JsonSerializer().Serialize(TestContext.Out, monster);
                    throw e;
                }

                Assert.Pass();
            }
        }

        [TestCase("SNV_Content.txt")]
        public async Task ParsedGeneric_AssertLegendaryActions(string fileName)
        {
            _filesToParse = new List<string> { fileName };
            var monsterResult = (await _monsterProcessor.Process(_filesToParse, new LocalizationEn()));

            var monsterBehaviors = monsterResult
                .Where(x => x.Behaviors.Any())
                .SelectMany(x => x.Behaviors);

            var legendaryActions = monsterBehaviors.Where(x => x.MonsterBehaviorTypeEnum == MonsterBehaviorType.Legendary);

            try
            {
                foreach (var lAction in legendaryActions)
                {
                    Assert.IsNotNull(lAction.Name);
                    Assert.IsNotEmpty(lAction.Name);
                    Assert.IsFalse(lAction.Name.Contains('*'));

                    Assert.IsNotNull(lAction.Description);
                    Assert.IsNotEmpty(lAction.Description);
                }
            }
            catch(AssertionException e)
            {
                new JsonSerializer().Serialize(TestContext.Out, legendaryActions);
                throw e;
            }

            Assert.Pass();
        }
    }
}
