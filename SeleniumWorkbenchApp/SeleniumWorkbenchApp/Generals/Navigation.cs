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

namespace WorkbenchApp.UITest.Generals
{
    internal class Navigation : BasePageElementMap
    {
        // Initiate variables
        internal static WebDriverWait? wait;
        internal static string totalEndowment = "Total Endowment";
        internal static string publicFund = "Public Fund";
        internal static string privateFund = "Private Fund";
        internal static string pipeline = "Pipeline";
        internal static string equityBeta = "Equity Beta";
        internal static string risk = "Risk";
        internal static string percLiquidAsset = "% Illiquid Asset";
        internal static string nav = "NAV";
        internal static string totalUnfundedCommitments = "Total Unfunded Commitments";
        internal static string on2_OnePager = "2 - One Pager";
        internal static string on3_Memo = "3 - Memo";
        internal static string on1_PreOnePager = "1 - Pre-One Pager";

        // Initiate the By objects for elements
        internal static By pageNames(string name) => By.XPath(@"//span[.='" + name + "']");

        // Initiate the elements
        public IWebElement menuNames(int timeoutInSeconds, string name)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(pageNames(name)));
        }
    }

    internal sealed class NavigationAction : BasePage<NavigationAction, Navigation>
    {
        #region Constructor
        private NavigationAction() { }
        #endregion

        #region Items Action
        public NavigationAction ClickPageNames(int timeoutInSeconds, string name)
        {
            Map.menuNames(timeoutInSeconds, name).Click();
            return this;
        }
        #endregion

        #region Built-in Actions
        #endregion
    }
}
