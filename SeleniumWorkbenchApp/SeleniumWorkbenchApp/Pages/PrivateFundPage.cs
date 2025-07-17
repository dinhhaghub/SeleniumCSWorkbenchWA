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
    internal class PrivateFundPage : BasePageElementMap
    {
        // Initiate variables
        internal static WebDriverWait? wait;
        internal static string privateFundHeadertable = "Private Fund";
        internal static string overview = "Overview";
        internal static string fee = "Fee";
        internal static string peer = "Peer";
        internal static string notes = "Notes";
        internal static string dataStatus = "Data Status";
        internal static string groupByAssetClass = "Group by Asset Class";
        internal static string groupByManager = "Group by Manager";

        // Initiate the By objects for elements
        internal static By columnNamesInAssetTable(int number) => By.XPath(@"//thead[@class='p-treetable-thead']/tr/th[" + number + "]");


        // Initiate the elements
        
    }

    internal sealed class PrivateFundAction : BasePage<PrivateFundAction, PrivateFundPage>
    {
        #region Constructor
        private PrivateFundAction() { }
        #endregion

        #region Items Action
        #endregion

        #region Built-in Actions
        #endregion
    }
}
