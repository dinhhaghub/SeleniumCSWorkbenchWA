using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
//using Microsoft.Net.Http.Headers;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WorkbenchApp.UITest.Core.BaseClass;
using WorkbenchApp.UITest.Core.Selenium;
using WorkbenchApp.UITest.Pages;

namespace WorkbenchApp.UITest.Generals
{
    internal class General : BasePageElementMap
    {
        // Initiate variables
        internal static WebDriverWait? wait;
        internal static string asset = "Asset"; // Column Name in Asset table
        internal static string benchmark = "Benchmark"; // Column Name in Asset table
        internal static string beta = "Beta"; // Column Name in Asset table
        internal static string estDailyNAV = "Est Daily NAV"; // Column Name in Asset table
        internal static string percOfFAD = "% of FAD"; // Column Name in Asset table
        internal static string mTD = "MTD"; // Column Name in Asset table
        internal static string qTD = "QTD"; // Column Name in Asset table
        internal static string fYTD = "FYTD"; // Column Name in Asset table
        internal static string threeYears = "3 Years"; // Column Name in Asset table
        internal static string fiveYears = "5 Years"; // Column Name in Asset table
        internal static string tenYears = "10 Years"; // Column Name in Asset table
        internal static string sortZtoA = "amount-up"; // old: sortAtoZ
        internal static string sortAtoZ = "amount-down"; // old: sortZtoA
        internal static string noSort = "sort-alt";
        internal static string ratingHistory = "Rating History";
        internal static string teamRatingHistory = "Team Rating History";

        // Initiate the By objects for elements
        internal static By tableHeaderName(string headerName) => By.XPath(@"//*[@class='header'][.='" + headerName + "']");
        internal static By dateOfTableHeaderName(string headerName) => By.XPath(tableHeaderName(headerName).ToString().Remove(0, 10) + "/following-sibling::span");
        internal static By dataOfTableHeader(string headerName, string pageName) => By.XPath(tableHeaderName(headerName).ToString().Remove(0, 10) + "/ancestor::*[@class='flex-auto flex']/following-sibling::div//*[.='" + pageName + "']/preceding-sibling::div");
        internal static By menuTitles(int number) => By.XPath(@"//div[@role='group']/div[" + number + "]/span[1]");
        internal static By labelBtn(string labelOrText) => By.XPath(@"//button[@label='" + labelOrText + "' or .='" + labelOrText + "']");
        internal static By addFilterBtn = By.XPath(@"//button[@label='Add Filter']");
        internal static By groupUngroupShowFundBtn(string buttonName) => By.XPath(@"//span[contains(.,'" + buttonName + "')]");
        internal static By assetTableSortBtn(string columnName) => By.XPath(@"//thead/tr[@class='ng-star-inserted']/th[contains(., '" + columnName + "')]");
        internal static By assetTableSortIcon(string columnName) => By.XPath(assetTableSortBtn(columnName).ToString().Remove(0, 10) + "//i");
        internal static By assetTableSortIconStatus(string columnName, string sortStatus) => By.XPath(assetTableSortBtn(columnName).ToString().Remove(0, 10) + "//i[contains(@class, '" + sortStatus + "')]");
        internal static By columnNamesInAssetTable(int number) => By.XPath(@"//thead[@class='p-treetable-thead' or @class='p-datatable-thead']/tr/th[" + number + "]");
        internal static By rowValuesInAssetTable(int row, int column) => By.XPath(@"//tbody[@class='p-element p-treetable-tbody' or @class='p-element p-datatable-tbody']/tr[" + row + "]/td[" + column + "]");
        internal static By rowNameInAssetTable(string nameStartsWith) => By.XPath(@"//tbody[@class='p-element p-treetable-tbody' or @class='p-element p-datatable-tbody']/tr[starts-with(.,'" + nameStartsWith + "')]");
        internal static By assetName(string name) => By.XPath("//tbody[@class='p-element p-treetable-tbody']//span[.=' "+ name + " ']");
        internal static By assetNameBtn(string name, string colNumber) => By.XPath(@"//span[.=' " + name + " ']/ancestor::tr/td[" + colNumber + "]/button");

        /// <summary>
        /// Dialog, Popup, Save, Cancel, Yes, No
        /// </summary>
        internal static By overlayDropdown = By.XPath(@"//div[contains(@class, 'overlayContentAnimation') or contains(@class, 'overlayAnimation')]");
        internal static By dynamicDialog = By.XPath(@"//div[@role='dialog']");
        internal static By errorInvalidMessageContent(string content) => By.XPath(@"//div[contains(@class,'error invalid-message') and contains(.,'" + content + "')]"); // in red text
        internal static By ratingAssetNameDialog(string name) => By.XPath(@"//div[@role='dialog']/div/p/span[.='" + name + "']");
        internal static By ratingDialogTabName(string tabName) => By.XPath(@"//div[@role='dialog']//li//span[.='" + tabName + "']");
        internal static By ratingSliderLabel(string label) => By.XPath(@"//div[@role='dialog']//p-tabpanel[1]//label[.='"+ label + ": ']/following-sibling::div/input");
        internal static By labelDropdown(string label) => By.XPath(@"//label[contains(.,'" + label + "')]/preceding-sibling::p-dropdown");
        internal static By ratingLabelDropdown(string label) => By.XPath(@"//label[.='" + label + ": ']/following-sibling::div/p-dropdown");
        internal static By dropdownSelectItem(string item) => By.XPath(@"//li[@aria-label='" + item + "']");
        internal static By saveRatingBtn = By.XPath(@"//div[@role='dialog']//button/span[.='Save']");
        internal static By loadingSpinner = By.XPath(@"//div[@role='alert']");
        internal static By ratingTabHeader(string tabName, int number) => By.XPath(@"//p-tabpanel[@header='" + tabName + "']//table[@role='table']/thead/tr/th[" + number + "]");
        internal static By rowUsersInRatingHistory(int row, int rowData) => By.XPath(@"//table[@role='table']/tbody/tr[" + row + "]/td[1]/div[" + rowData + "]");
        internal static By rowValuesInRatingTab(string tabName, int row, int column) => By.XPath(@"//p-tabpanel[@header='" + tabName + "']//table[@role='table']/tbody/tr[" + row + "]/td[" + column + "]");

        // Initiate the elements
        public IWebElement HighlightElement(IWebElement element, string? color = null, string? setOrRemoveAttr = null) // Highlight & Un-Highlight Element
        {
            // Check if give color/setOrRemoveAttr with a specific color/setOrRemoveAttr, if no then will get blue color (by default)
            color ??= "blue"; setOrRemoveAttr ??= "removeAttribute";

            IJavaScriptExecutor? js = Driver.Browser as IJavaScriptExecutor;
            js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", element, " border: 3px solid " + color + ";"); Thread.Sleep(150);
            js.ExecuteScript("arguments[0]." + setOrRemoveAttr + "('style', arguments[1]);", element, " border: 3px solid " + color + ";"); // un-highlight
            return element;
        }

        public IWebElement dateInTableHeaderName(int timeoutInSeconds, string headerName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(dateOfTableHeaderName(headerName)));
        }
        public IWebElement dataInTableHeader(int timeoutInSeconds, string headerName, string pageName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(dataOfTableHeader(headerName, pageName)));
        }
        public IWebElement titlesMenu(int timeoutInSeconds, int number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(menuTitles(number)));
        }
        public IWebElement buttonLabel(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(labelBtn(label)));
        }
        public IWebElement btnAddFilter(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(addFilterBtn));
        }
        public IWebElement btnGroupUngroupShowFund(int timeoutInSeconds, string buttonName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(groupUngroupShowFundBtn(buttonName)));
        }
        public IWebElement btnSortAssetTable(int timeoutInSeconds, string columnName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(assetTableSortBtn(columnName)));
        }
        public IWebElement iconSortAssetTable(int timeoutInSeconds, string columnName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(assetTableSortIcon(columnName)));
        }
        public IWebElement assetTableColumnNames(int timeoutInSeconds, int number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(columnNamesInAssetTable(number)));
        }
        public IWebElement assetTableRowValues(int timeoutInSeconds, int row, int column)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(rowValuesInAssetTable(row, column)));
        }
        public IWebElement assetTableRowName(int timeoutInSeconds, string nameStartsWith)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(rowNameInAssetTable(nameStartsWith)));
        }
        public IWebElement nameAsset(int timeoutInSeconds, string name)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(assetName(name)));
        }
        public IWebElement buttonAssetName(int timeoutInSeconds, string name, string colNumber)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(assetNameBtn(name, colNumber)));
        }
        public IWebElement tabNameRatingDialog(int timeoutInSeconds, string tabName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(ratingDialogTabName(tabName)));
        }
        public IWebElement sliderLabelRating(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(ratingSliderLabel(label)));
        }
        public IWebElement dropdownLabel(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(labelDropdown(label)));
        }
        public IWebElement dropdownLabelRating(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(ratingLabelDropdown(label)));
        }
        public IWebElement selectItemDropdown(int timeoutInSeconds, string item)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(dropdownSelectItem(item)));
        }
        public IWebElement buttonSaveRating(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(saveRatingBtn));
        }
        public IWebElement headerRatingTab(int timeoutInSeconds,string tabName, int number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(ratingTabHeader(tabName, number)));
        }
        public IWebElement ratingHistoryRowUsers(int timeoutInSeconds, int row, int rowData)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(rowUsersInRatingHistory(row, rowData)));
        }
        public IWebElement ratingTabRowValues(int timeoutInSeconds,string tabName, int row, int column)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(rowValuesInRatingTab(tabName, row, column)));
        }
    }

    internal sealed class GeneralAction : BasePage<GeneralAction, General>
    {
        #region Constructor
        private GeneralAction() { }
        #endregion

        #region Items Action
        // Wait for loading Spinner icon to disappear
        public GeneralAction WaitForLoadingIconToDisappear(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(element));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(element));
            }
            return this;
        }

        // Wait for element visible
        public GeneralAction WaitForElementVisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(element));
            }
            return this;
        }

        // Wait for element Invisible (can use for dropdown on-overlay Invisible)
        public GeneralAction WaitForElementInvisible(int timeoutInSeconds, By element)
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
        public bool CheckFileDownloadIsComplete(int timeout, string downloadPath, string fileName)
        {
            int times = 0;
            while (!File.Exists(downloadPath + @"\" + fileName) && times < timeout)
            {
                times++;
                System.Threading.Thread.Sleep(1000);
            }

            if (times == timeout)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool DateInTableHeaderNameGetText(int timeoutInSeconds, string headerName, string textParam)
        {
            ScrollIntoView(Map.dateInTableHeaderName(timeoutInSeconds, headerName));
            var iweb = Map.dateInTableHeaderName(timeoutInSeconds, headerName);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DateInTableHeaderName_" + headerName + "_" + textParam + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public string DataInTableHeaderGetText(int timeoutInSeconds, string headerName, string pageName)
        {
            // Should not use HighlightElement here, because it will not work with order column names
            ScrollIntoView(Map.dataInTableHeader(timeoutInSeconds, headerName, pageName));
            return Map.dataInTableHeader(timeoutInSeconds, headerName, pageName).Text;
        }
        public string MenuTitlesGetText(int timeoutInSeconds, int number)
        {
            return Map.titlesMenu(timeoutInSeconds, number).Text;
        }
        public bool IsButtonLabelShown(int timeoutInSeconds, string labelOrInnerText)
        {
            ScrollIntoView(Map.buttonLabel(timeoutInSeconds, labelOrInnerText));
            var iweb = Map.buttonLabel(timeoutInSeconds, labelOrInnerText);
            bool element = Map.HighlightElement(iweb, "green").Displayed;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_IsButtonLabelShown_" + labelOrInnerText + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsAddFilterButtonShown(int timeoutInSeconds)
        {
            ScrollIntoView(Map.btnAddFilter(timeoutInSeconds));
            var iweb = Map.btnAddFilter(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Displayed;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_IsAddFilterButtonShown_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsGroupUngroupShowFundButtonShown(int timeoutInSeconds, string label)
        {
            ScrollIntoView(Map.btnGroupUngroupShowFund(timeoutInSeconds, label));
            var iweb = Map.btnGroupUngroupShowFund(timeoutInSeconds, label);
            bool element = Map.HighlightElement(iweb, "green").Displayed;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_IsGroupUngroupShowFundBtnShown_" + label + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public string ColumnNamesInAssetTableGetText(int timeoutInSeconds, int number)
        {
            // Should not use HighlightElement here, because it will not work with order column names
            ScrollIntoView(Map.assetTableColumnNames(timeoutInSeconds, number));
            return Map.assetTableColumnNames(timeoutInSeconds, number).Text;
        }
        public string RowValuesInAssetTableGetText(int timeoutInSeconds, int row, int column)
        {
            ScrollIntoView(Map.assetTableRowValues(timeoutInSeconds, row, column));
            return Map.assetTableRowValues(timeoutInSeconds, row, column).Text;
        }
        public bool RowNameInAssetTableGetText(int timeoutInSeconds, string nameStartsWith, string textParam)
        {
            ScrollIntoView(Map.assetTableRowName(timeoutInSeconds, nameStartsWith));
            var iweb = Map.assetTableRowName(timeoutInSeconds, nameStartsWith);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_RowAssetTableGetText_" + nameStartsWith + "_"+ textParam + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public string SortIconAssetTableGetStatus(int timeoutInSeconds, string columName)
        {
            return Map.iconSortAssetTable(timeoutInSeconds, columName).GetAttribute("class");
        }
        public string RatingTabHeaderGetText(int timeoutInSeconds,string tabName, int number)
        {
            ScrollIntoView(Map.headerRatingTab(timeoutInSeconds, tabName, number));
            return Map.headerRatingTab(timeoutInSeconds, tabName, number).Text;
        }
        public bool RatingHistoryRowUsersGetText(int timeoutInSeconds, int mainRow, int rowData, string textParam)
        {
            ScrollIntoView(Map.ratingHistoryRowUsers(timeoutInSeconds, mainRow, rowData));
            var iweb = Map.ratingHistoryRowUsers(timeoutInSeconds, mainRow, rowData);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_RatingHistoryRowUsers_" + mainRow + "_" + rowData + "_" + textParam + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public string RatingTabRowValuesGetText(int timeoutInSeconds,string tabName, int row, int column)
        {
            return Map.ratingTabRowValues(timeoutInSeconds,tabName, row, column).Text;
        }

        // Actions
        public GeneralAction ScrollIntoView(IWebElement iweb)
        {
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].scrollIntoView(false);", iweb);
            return this;
        }
        
        /// PageDown To scroll down page
        public GeneralAction PageDownToScrollDownPage()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.PageDown).Build().Perform();
            return this;
        }

        /// PageUp To scroll up page
        public GeneralAction PageUpToScrollDownPage()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.PageUp).Build().Perform();
            return this;
        }

        /// Press Tab keyboard
        public GeneralAction PressTabKeyboard()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.Tab).Build().Perform();
            return this;
        }

        /// Press Enter keyboard
        public GeneralAction PressEnterKeyboard()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.Enter).Build().Perform();
            return this;
        }

        /// Press ArrowDown keyboard
        public GeneralAction PressDownKeyboard()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.ArrowDown).Build().Perform();
            return this;
        }

        public void DeleteFilePath(string path, string fileName)
        {
            string[] fileNames = Directory.GetFiles(path);
            foreach (string file in fileNames)
            {
                if (file.Contains(fileName))
                {
                    File.Delete(file);
                }
            }
        }
        public GeneralAction ClickSortAssetTableButton(int timeoutInSeconds, string columName) 
        {
            // this method should not be used HighlightElement, because it will not work with sort icon
            Map.btnSortAssetTable(timeoutInSeconds, columName).Click();
            return this;
        }
        public GeneralAction ClickGroupUngroupShowFundButton(int timeoutInSeconds, string buttonName)
        {
            Map.HighlightElement(Map.btnGroupUngroupShowFund(timeoutInSeconds, buttonName)).Click();
            return this;
        }
        public GeneralAction ClickRowValuesInAssetTable(int timeoutInSeconds, int row, int column)
        {
            Map.HighlightElement(Map.assetTableRowValues(timeoutInSeconds, row, column)).Click();
            return this;
        }
        public GeneralAction ClickToExpandAssetName(int timeoutInSeconds, string name)
        {
            Map.HighlightElement(Map.nameAsset(timeoutInSeconds, name)).Click();
            return this;
        }
        public GeneralAction ClickTabNameInRatingDialog(int timeoutInSeconds, string tabName)
        {
            Map.HighlightElement(Map.tabNameRatingDialog(timeoutInSeconds, tabName)).Click();
            return this;
        }
        public GeneralAction SlideSliderLabelRating(int timeoutInSeconds, string label, int ArrowLeftXTimes)
        {
            for (int i = 0; i <= ArrowLeftXTimes; i++) 
            {
                // Slide to LEFT
                Map.sliderLabelRating(timeoutInSeconds, label).SendKeys(OpenQA.Selenium.Keys.ArrowLeft);
            }
            return this;
        }
        public GeneralAction ClickButtonLabel(int timeoutInSeconds, string label)
        {
            Actions action = new Actions(Driver.Browser);
            action.MoveToElement(Map.buttonLabel(timeoutInSeconds, label)).Click(Map.HighlightElement(Map.buttonLabel(timeoutInSeconds, label))).Perform();
            return this;
        }
        public GeneralAction ClickLabelDropdown(int timeoutInSeconds, string label)
        {
            Map.HighlightElement(Map.dropdownLabel(timeoutInSeconds, label)).Click();
            return this;
        }
        public GeneralAction ClickLabelDropdownRating(int timeoutInSeconds, string label)
        {
            Map.HighlightElement(Map.dropdownLabelRating(timeoutInSeconds, label)).Click();
            return this;
        }
        public GeneralAction SelectItemInDropdown(int timeoutInSeconds, string item)
        {
            Map.HighlightElement(Map.selectItemDropdown(timeoutInSeconds, item)).Click();
            return this;
        }
        public GeneralAction ClickSaveRatingButton(int timeoutInSeconds)
        {
            Map.HighlightElement(Map.buttonSaveRating(timeoutInSeconds)).Click();
            return this;
        }
        public GeneralAction ClickAssetNameButton(int timeoutInSeconds, string name, string colNumber)
        {
            Map.HighlightElement(Map.buttonAssetName(timeoutInSeconds, name, colNumber)).Click();
            return this;
        }
        #endregion

        #region Built-in Actions
        public GeneralAction ClickAndSelectItemInDropdown(int timeoutInSeconds, string label, string item)
        {
            ScrollIntoView(this.Map.dropdownLabel(timeoutInSeconds, label));
            ClickLabelDropdown(timeoutInSeconds, label);
            WaitForElementVisible(10, General.overlayDropdown);
            WaitForElementVisible(10, General.dropdownSelectItem(item));
            SelectItemInDropdown(timeoutInSeconds, item);
            WaitForElementInvisible(10, General.overlayDropdown);
            return this;
        }
        public GeneralAction ClickAndSelectItemInRatingDropdown(int timeoutInSeconds, string label, string item)
        {
            ScrollIntoView(this.Map.dropdownLabelRating(timeoutInSeconds, label));
            ClickLabelDropdownRating(timeoutInSeconds, label);
            WaitForElementVisible(timeoutInSeconds, General.overlayDropdown);
            WaitForElementVisible(timeoutInSeconds, General.dropdownSelectItem(item));
            SelectItemInDropdown(timeoutInSeconds, item);
            WaitForElementInvisible(10, General.overlayDropdown);
            return this;
        }
        #endregion
    }
}
