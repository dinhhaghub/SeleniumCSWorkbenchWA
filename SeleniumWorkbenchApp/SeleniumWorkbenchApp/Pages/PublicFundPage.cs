using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkbenchApp.UITest.Core.BaseClass;
using WorkbenchApp.UITest.Core.Selenium;

namespace WorkbenchApp.UITest.Pages
{
    internal class PublicFundPage : BasePageElementMap
    {
        // Initiate variables
        internal static WebDriverWait? wait;
        internal static string publicFundHeadertable = "Public Fund";
        internal static string overview = "Overview";
        internal static string fee = "Fee";
        internal static string peer = "Peer";
        internal static string notes = "Notes";
        internal static string dataStatus = "Data Status";
        internal static string groupByAssetClass = "Group by Asset Class";
        internal static string showFundsOnly = "Show Funds Only";

        // Initiate the By objects for elements


        // Initiate the elements

    }

    internal sealed class PublicFundAction : BasePage<PublicFundAction, PublicFundPage>
    {
        #region Constructor
        private PublicFundAction() { }
        #endregion

        #region Items Action
        // verify elements
        

        // Actions
        #endregion

        #region Built-in Actions
        #endregion
    }
}
