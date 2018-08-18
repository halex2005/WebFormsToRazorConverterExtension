using JetBrains.Application.DataContext;
using JetBrains.Application.Threading;
using JetBrains.Application.UI.Actions.ActionManager;
using JetBrains.Application.UI.ActionSystem.ActionBar;
using JetBrains.Application.UI.ActionSystem.ActionsRevised.Handlers;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Daemon.CSharp.Errors;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TestFramework.Projects;
using NUnit.Framework;

namespace RazorConverter.Tests
{
    [TestFixture]
    public class ConvertWebFormsToRazorActionTests : ExecuteActionTestBase
    {
        override string ActionId
        {
            get { return "ConvertWebFormsToRazorAction"; }
        }
        
        [Test]
        public void ConvertTest()
        {
            DoTest();
        }
    }
}