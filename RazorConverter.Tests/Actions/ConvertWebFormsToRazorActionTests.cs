using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace RazorConverter.Tests.Actions
{
    [TestFixture]
    public class ConvertWebFormsToRazorActionTests : ExecuteActionTestBase
    {
        protected override string ActionId
        {
            get { return "ConvertWebFormsToRazorAction"; }
        }


        [Test]
        public void TestCase01()
        {
            DoTest();
        }
    }
}