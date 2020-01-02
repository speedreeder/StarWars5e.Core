using Newtonsoft.Json;
using NUnit.Framework;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Monster;
using StarWars5e.Parser.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var result = await _monsterProcessor.Process(_filesToParse);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task ParsedSampleFile_AssertValues()
        {
            var monsterResult = (await _monsterProcessor.Process(_filesToParse)).First();

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
            var monsterResult = (await _monsterProcessor.Process(_filesToParse)).First();

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
            var monsterResult = (await _monsterProcessor.Process(_filesToParse)).First();

            Assert.IsNotEmpty(monsterResult.FlavorText);
            Assert.AreEqual("Acklays are amphibious reptillian crustaceans with six deadly claws and razor-sharp teeth native to the planet Vendaxa. They are often used as execution beasts or fodder for gladiatorial arenas.", monsterResult.FlavorText);

            new JsonSerializer().Serialize(TestContext.Out, monsterResult);
        }

        [TestCase("TestData.mm_sample.txt")]
        [TestCase("TestData.SNV_Content_sample.txt")]
        public async Task ParsedNewFormat_AssertFlavorTextParsed(string fileName)
        {
            _filesToParse = new List<string> { fileName };

            var monsterResult = (await _monsterProcessor.Process(_filesToParse));

            foreach (var monster in monsterResult)
            {
                Assert.NotNull(monster.FlavorText, $"{monster.Name} failed to have flavor text.");
                //Assert.IsNotEmpty(monster.FlavorText, $"{monster.Name} failed to have flavor text.");

                new JsonSerializer().Serialize(TestContext.Out, monster);
            }
        }

        [TestCase("TestData.mm_sample.txt")]
        [TestCase("TestData.new_mm_content.txt")]
        [TestCase("TestData.SNV_Content_sample.txt")]
        public async Task GenericParse_AssertValues(string fileName)
        {
            _filesToParse = new List<string> { fileName };

            var monsterResult = (await _monsterProcessor.Process(_filesToParse));

            foreach(var monster in monsterResult)
            {
                try
                {
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
                    Assert.IsNotEmpty(monster.Senses);
                    Assert.IsNotNull(monster.Behaviors);
                    var hasFlavorOrSectionText = monster.FlavorText != string.Empty || monster.SectionText != string.Empty;
                    if(!hasFlavorOrSectionText)
                        Assert.Warn($"{monster.Name} missing flavor text and section flavor text");
                }
                catch(AssertionException e)
                {
                    new JsonSerializer().Serialize(TestContext.Out, monster);
                    throw e;
                }
            }
        }

        [TestCase("TestData.mm_sample.txt")]
        [TestCase("TestData.new_mm_content.txt")]
        public async Task ParsedGeneric_AssertBehaviors(string fileName)
        {
            _filesToParse = new List<string> { fileName };
            var monsterResult = (await _monsterProcessor.Process(_filesToParse));

            foreach(var monster in monsterResult)
            {

                Assert.NotNull(monster.Behaviors);
                Assert.IsNotEmpty(monster.Behaviors);

                foreach(var action in monster.Behaviors)
                {
                    new JsonSerializer().Serialize(TestContext.Out, action);

                    Assert.NotNull(action.AttackBonus);
                    Assert.NotNull(action.AttackType);
                    Assert.NotNull(action.AttackTypeEnum);
                    Assert.NotNull(action.DamageTypeEnum);
                    Assert.IsNotNull(action.DamageType);

                    Assert.IsNotEmpty(action.Damage);
                    Assert.IsNotEmpty(action.DamageRoll);
                    Assert.IsNotEmpty(action.Description);
                }
            }
        }
    }
}
