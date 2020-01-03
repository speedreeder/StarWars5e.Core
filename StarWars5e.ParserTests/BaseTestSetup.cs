using NUnit.Framework;

namespace StarWars5e.ParserTests
{
    public abstract class BaseTestSetup
    {
        [OneTimeSetUp]
        public virtual void BaseSetup()
        {
        }

        [OneTimeTearDown]
        public virtual void BaseTearDown()
        {
        }
    }
}