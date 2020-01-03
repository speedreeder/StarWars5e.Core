using NUnit.Framework;
using System.Reflection;

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