using Reqnroll;
using WorkbenchApp.UITest.Core;
using WorkbenchApp.UITest.Core.BaseTestCase;

namespace WorkbenchApp.UITest.BDD.Hooks
{
    [Binding]
    internal class TestHooks : BaseTestCase
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
            SetupTest();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            TeardownTest();
        }
    }
}
