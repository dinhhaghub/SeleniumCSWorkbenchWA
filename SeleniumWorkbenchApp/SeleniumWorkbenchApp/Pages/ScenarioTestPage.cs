using AventStack.ExtentReports.Gherkin.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkbenchApp.UITest.Core.BaseClass;
using WorkbenchApp.UITest.Core.Selenium;
using WorkbenchApp.UITest.Generals;
using static System.Net.Mime.MediaTypeNames;

namespace WorkbenchApp.UITest.Pages
{
    internal class ScenarioTestPage : BasePageElementMap
    {
        // Initiate variables
        internal static WebDriverWait? wait;
        internal static string testScenarioInput = "Test Scenario Input";
        internal static string priorDayComparison = "Prior Day Comparison";

        // Initiate the By objects for elements
        internal static By expandCollapseIcon(string section) => By.XPath(@"//a[.='" + section + "']");
        internal static By expandCollapseIconStatus(string section, string truefalse) => By.XPath(@"//a[.='" + section + "' and @aria-expanded='" + truefalse + "']");
        internal static By columnNamesInSection(string section, string numberRow, string numberCol) => By.XPath(@"//div//span[.='" + section + "']/ancestor::legend/following-sibling::div//p-treetable//thead/tr[" + numberRow + "]/th[" + numberCol + "]");
        internal static By dataInSection(string section, string numberRow, string numberCol) => By.XPath(@"//div//span[.='" + section + "']/ancestor::legend/following-sibling::div//p-treetable//table//tbody/tr[" + numberRow + "]/td[" + numberCol + "]");
        internal static By columnNameInSectionSortIcon(string section, string row, string columnName) => By.XPath(@"//div//span[.='" + section + "']/ancestor::legend/following-sibling::div//p-treetable//table//thead/tr[" + row + "]/th[contains(.,'" + columnName + "')]/span");
        internal static By columnNameInSectionSortIconStatus(string section, string row, string columnName, string status) => By.XPath(columnNameInSectionSortIcon(section, row, columnName).ToString().Remove(0, 10) + "//i[contains(@class, '" + status + "')]");
        internal static By columnNameInSectionSortIconGetStatus(string section, string row, string columnName) => By.XPath(columnNameInSectionSortIcon(section, row, columnName).ToString().Remove(0, 10) + "//i");
        internal static By searchKeyWordInSectionTxt(string section) => By.XPath(@"//div//span[.='" + section + "']/ancestor::legend/following-sibling::div//p-treetable//input");
        internal static By scenarioFlowsLine(string row) => By.XPath(@"//div//span[.='Test Scenario Input']/ancestor::legend/following-sibling::div//p-treetable//tbody/tr[" + row + "]/td[5]");
        internal static By assetNameInSectionName(string sectionName, string assetName) => By.XPath(@"//a[.='" + sectionName + "']/ancestor::p-fieldset//tbody[@class='p-element p-treetable-tbody']//td[.=' " + assetName + " ']//button");
        internal static By assetNameInSectionNameColData(string sectionName, string assetName, string column) => By.XPath(@"//a[.='" + sectionName + "']/ancestor::p-fieldset//tbody[@class='p-element p-treetable-tbody']//td[.=' " + assetName + " ']/parent::tr/td[" + column + "]");
        /// Add/Edit Funds
        internal static By addEditFundsAssetDropDown(string row) => By.XPath(@"//p-table//table/tbody/tr[" + row + "]/td[1]/p-dropdown");
        internal static By addEditFundsAssetSelectItemDropDown(string row, string item) => By.XPath(@"//p-table//table/tbody/tr[" + row + "]/td[1]/p-dropdown//span[.='"+ item + "']");
        internal static By addEditFundsInput(string row, string column) => By.XPath(@"//p-table//table/tbody/tr[" + row + "]/td[" + column + "]//input");
        internal static By addEditFundsButton(string row, string column) => By.XPath(@"//p-table//table/tbody/tr[" + row + "]/td[" + column + "]//button");
        internal static By addEditFundsMsgContentPopup = By.XPath(@"//app-add-edit-fund//div[@role='dialog']/div[contains(@class,'p-dialog-content')]");

        // Initiate the elements
        public IWebElement HighlightElement(IWebElement element, string? color = null, string? setOrRemoveAttr = null)
        {
            color ??= "blue";
            setOrRemoveAttr ??= "remove"; // chỉ định hành động "remove" hoặc "keep"

            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Browser;

            // Highlight
            js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", element, "border: 3px solid " + color + ";");
            Thread.Sleep(150);

            // Unhighlight nếu setOrRemoveAttr = "remove"
            if (setOrRemoveAttr.Equals("remove", StringComparison.OrdinalIgnoreCase))
            {
                js.ExecuteScript("arguments[0].removeAttribute('style');", element);
            }

            return element;
        }

        public IWebElement iconExpandCollapse(int timeoutInSeconds, string section)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(expandCollapseIcon(section)));
        }
        public IWebElement nameColumnsInSection(int timeoutInSeconds, string section, string numberRow, string numberCol)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(columnNamesInSection(section, numberRow, numberCol)));
        }
        public IWebElement dataOfSection(int timeoutInSeconds, string section, string numberRow, string numberCol)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(dataInSection(section, numberRow, numberCol)));
        }
        public IWebElement sortIconInSectionColumnName(int timeoutInSeconds, string section, string row, string columnName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(columnNameInSectionSortIcon(section, row, columnName)));
        }
        public IWebElement sortIconInSectionColumnNameGetStatus(int timeoutInSeconds, string section, string row, string columnName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(columnNameInSectionSortIconGetStatus(section, row, columnName)));
        }
        public IWebElement txtSearchKeyWordInSection(string section) => Driver.Browser.FindElement(searchKeyWordInSectionTxt(section));
        public IWebElement lineScenarioFlows(int timeoutInSeconds, string row)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(scenarioFlowsLine(row)));
        }
        public IWebElement sectionNameContainsAssetName(int timeoutInSeconds, string sectionName, string assetName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(assetNameInSectionName(sectionName, assetName)));
        }
        public IWebElement dataColAssetNameInSectionName(int timeoutInSeconds, string sectionName, string assetName, string column)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(assetNameInSectionNameColData(sectionName, assetName, column)));
        }
        /// Add/Edit Funds
        public IWebElement assetDropDownAddEditFunds(int timeoutInSeconds, string row)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(addEditFundsAssetDropDown(row)));
        }
        public IWebElement assetSelectItemDropDownAddEditFunds(int timeoutInSeconds, string row, string item)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(addEditFundsAssetSelectItemDropDown(row, item)));
        }
        public IWebElement inputAddEditFunds(int timeoutInSeconds, string row, string column)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d =>d.FindElement(addEditFundsInput(row, column)));
        }
        public IWebElement buttonAddEditFunds(int timeoutInSeconds, string row, string column)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(addEditFundsButton(row, column)));
        }
        public IWebElement msgContentPopupAddEditFunds(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(addEditFundsMsgContentPopup));
        }
    }

    internal sealed class ScenarioTestAction : BasePage<ScenarioTestAction, ScenarioTestPage>
    {
        #region Constructor
        private ScenarioTestAction() { }
        #endregion

        #region Items Action
        // Wait for element visible
        public ScenarioTestAction WaitForElementVisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(element));
            }
            return this;
        }

        // Wait for element Invisible (can use for dropdown on-overlay Invisible)
        public ScenarioTestAction WaitForElementInvisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(element));
            }
            return this;
        }

        // Checking element exists or not
        public bool IsElementPresent(By by)
        {
            try
            {
                Driver.Browser.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        // verify elements
        public bool ColumnNamesInSectionGetText(int timeoutInSeconds, string section, string numberRow, string numberColumn, string textParam)
        {
            var iweb = Map.nameColumnsInSection(timeoutInSeconds, section, numberRow, numberColumn);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_ColumnNamesInSection_" + section + "_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public string DataInSectionGetText(int timeoutInSeconds, string section, string numberRow, string numberColumn)
        {
            return Map.dataOfSection(timeoutInSeconds, section, numberRow, numberColumn).Text;
        }
        public bool DataInSectionGetText(int timeoutInSeconds, string section, string numberRow, string numberColumn, string textParam)
        {
            var iweb = Map.dataOfSection(timeoutInSeconds, section, numberRow, numberColumn);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DataInSection_" + section + "_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public string ColumnNameSortIconInSectionGetStatus(int timeoutInSeconds, string section, string row, string columnName)
        {
            return Map.sortIconInSectionColumnNameGetStatus(timeoutInSeconds, section, row, columnName).GetAttribute("class");
        }
        public bool AssetNameInSectionNameColDataGetText(int timeoutInSeconds, string sectionName, string assetName, string column, string textParam)
        {
            var iweb = Map.dataColAssetNameInSectionName(timeoutInSeconds, sectionName, assetName, column);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_AssetNameInSection_" + sectionName + "_" + assetName + "_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public bool AddEditFundsMsgContentPopupGetText(int timeoutInSeconds, string textParam)
        {
            var iweb = Map.msgContentPopupAddEditFunds(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_AddEditFundsMsgContentPopup_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }

        // Actions
        public ScenarioTestAction ClickExpandCollapseIcon(int timeoutInSeconds, string section) 
        {
            Map.HighlightElement(Map.iconExpandCollapse(timeoutInSeconds, section)).Click();
            return this;
        }
        public ScenarioTestAction ClickColumnNameInSectionSortIcon(int timeoutInSeconds, string section, string row, string columnName)
        {
            Map.HighlightElement(Map.sortIconInSectionColumnName(timeoutInSeconds, section, row, columnName)).Click();
            return this;
        }
        public ScenarioTestAction InputSearchKeyWordInSection(string section, string text)
        {
            Map.HighlightElement(Map.txtSearchKeyWordInSection(section)).Clear(); Thread.Sleep(250);
            Map.HighlightElement(Map.txtSearchKeyWordInSection(section)).SendKeys(text); Thread.Sleep(500);
            return this;
        }
        public ScenarioTestAction ClickAndInputScenarioFlowsLine(int timeoutInSeconds, string row, string text)
        {
            Map.HighlightElement(Map.lineScenarioFlows(timeoutInSeconds, row)).Click();
            //this.Map.lineScenarioFlows(timeoutInSeconds, row).Clear(); Thread.Sleep(250);
            //this.Map.lineScenarioFlows(timeoutInSeconds, row).SendKeys(text); Thread.Sleep(250);

            ////Way1: Input text from keyboard by using System.Windows.Forms
            //System.Windows.Forms.SendKeys.SendWait(text); Thread.Sleep(250);
            //System.Windows.Forms.SendKeys.SendWait(@"{ENTER}");

            // Way2: Input text from keyboard by using Actions of Browser
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(text).SendKeys(OpenQA.Selenium.Keys.Enter).Build().Perform();
            return this;
        }
        public ScenarioTestAction ClickAssetNameInSectionName(int timeoutInSeconds, string sectionName, string assetName)
        {
            Map.HighlightElement(Map.sectionNameContainsAssetName(timeoutInSeconds, sectionName, assetName)).Click();
            return this;
        }
        /// Add/Edit Funds <summary>
        public ScenarioTestAction ClickAddEditFundsAssetDropDown(int timeoutInSeconds, string row)
        {
            Map.HighlightElement(Map.assetDropDownAddEditFunds(timeoutInSeconds, row)).Click();
            return this;
        }
        public ScenarioTestAction ClickAddEditFundsAssetSelectItemDropDown(int timeoutInSeconds, string row, string item)
        {
            Map.HighlightElement(Map.assetSelectItemDropDownAddEditFunds(timeoutInSeconds, row, item)).Click();
            return this;
        }
        public ScenarioTestAction InputAddEditFunds(int timeoutInSeconds, string row, string column, string text)
        {
            Map.HighlightElement(Map.inputAddEditFunds(timeoutInSeconds, row, column)).Clear(); Thread.Sleep(250);
            Map.HighlightElement(Map.inputAddEditFunds(timeoutInSeconds, row, column)).SendKeys(text); Thread.Sleep(250);
            return this;
        }
        public ScenarioTestAction ClickAddEditFundsPosButton(int timeoutInSeconds, string row, string column)
        {
            Map.HighlightElement(Map.buttonAddEditFunds(timeoutInSeconds, row, column)).Click();
            return this;
        }
        #endregion

        #region Built-in Actions
        public ScenarioTestAction ClickIconToExpand(int timeoutInSeconds, string section)
        {
            var isExpanded = Driver.Browser.FindElement(ScenarioTestPage.expandCollapseIcon(section)).GetAttribute("aria-expanded");
            if (isExpanded.Contains("false")) 
            { 
                ClickExpandCollapseIcon(timeoutInSeconds, section);
                WaitForElementInvisible(10, ScenarioTestPage.expandCollapseIconStatus(section, "false"));
                WaitForElementVisible(10, ScenarioTestPage.expandCollapseIconStatus(section, "true"));
            }
            return this;
        }
        public ScenarioTestAction ClickIconToCollapse(int timeoutInSeconds, string section)
        {
            var isExpanded = Driver.Browser.FindElement(ScenarioTestPage.expandCollapseIcon(section)).GetAttribute("aria-expanded");
            if (isExpanded.Contains("true")) 
            { 
                ClickExpandCollapseIcon(timeoutInSeconds, section);
                WaitForElementInvisible(10, ScenarioTestPage.expandCollapseIconStatus(section, "true"));
                WaitForElementVisible(10, ScenarioTestPage.expandCollapseIconStatus(section, "false"));
            }
            return this;
        }
        public ScenarioTestAction ClickAndSelectItemInDropDownAddEditFundsAsset(int timeoutInSeconds, string row, string item)
        {
            ClickAddEditFundsAssetDropDown(timeoutInSeconds, row);
            WaitForElementVisible(timeoutInSeconds, General.overlayDropdown); Thread.Sleep(200);
            ClickAddEditFundsAssetSelectItemDropDown(timeoutInSeconds, row, item);
            WaitForElementInvisible(timeoutInSeconds, General.overlayDropdown); Thread.Sleep(200);
            return this;
        }
        #endregion
    }
}
