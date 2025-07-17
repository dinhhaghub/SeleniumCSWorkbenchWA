using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkbenchApp.UITest.Core.BaseClass;

namespace WorkbenchApp.UITest.Pages
{
    internal class TotalEndowmentPage : BasePageElementMap
    {
        // Initiate variables
        internal static WebDriverWait? wait;
        internal static string totalEndowmentHeadertable = "TOTAL ENDOWMENT";
        internal static string returnPublic = "Return (Public)"; // sub tab
        internal static string returnPrivate = "Return (Private)"; // sub tab
        internal static string risk = "Risk"; // sub tab
        internal static string liquidity = "Liquidity"; // sub tab
        internal static string scenarioTest = "Scenario Test"; // sub tab

        // Initiate the By objects for elements

        // Initiate the elements
    }

    internal sealed class TotalEndowmentAction : BasePage<TotalEndowmentAction, TotalEndowmentPage>
    {
        #region Constructor
        private TotalEndowmentAction() { }
        #endregion

        #region Items Action
        #endregion

        #region Built-in Actions
        #endregion
    }
}
