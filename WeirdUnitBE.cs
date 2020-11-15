using NUnit.Framework;

namespace WeirdUnitBE
{
    public class WeirdUnitBE
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(3, 3);
        }
    }
}