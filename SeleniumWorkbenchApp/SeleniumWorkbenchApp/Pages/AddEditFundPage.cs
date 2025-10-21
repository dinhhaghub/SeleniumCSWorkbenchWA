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

namespace WorkbenchApp.UITest.Pages
{
    internal class AddEditFundPage : BasePageElementMap
    {
        // Initiate variables
        internal static WebDriverWait? wait;
        internal static string fundNameManager = "Fund Manager Name";
        internal static string firm = "Firm";
        internal static string fundType = "Fund Type";
        internal static string lowestLevelSubAssetClass = "Lowest Level Sub-Asset Class *";
        internal static string subAssetClass = "Sub-Asset Class ";
        internal static string latestActualValue = "Latest Actual Value";
        internal static string businessCity = "Business City";
        internal static string businessState = "Business State";
        internal static string businessCountry = "Business Country";
        internal static string businessStreet = "Business Street";
        internal static string businessZIP = "Business ZIP";
        internal static string businessContact = "Business Contact";
        internal static string businessEmail = "Business Email";
        internal static string businessPhone = "Business Phone";
        internal static string fundManager = " Fund Manager *"; //"Fund Manager *";
        internal static string fundName = "Fund Name *";
        internal static string fundNameIndex = "Fund Name";
        internal static string strategy = "Strategy";
        internal static string vintageYear = "Vintage Year";
        internal static string strategyHeadquarter = "Strategy Headquarter";
        internal static string assetClass = "Asset Class";
        internal static string investmentStage = "Investment Stage";
        internal static string industryFocus = "Industry Focus";
        internal static string geographicFocus = "Geographic Focus";
        internal static string fundSizeM = "Fund Size (M)";
        internal static string currency = "Currency";

        // Initiate the By objects for elements
        internal static By fundTypeDropdown = By.XPath(@"//div[.='Fund Type' or .=' Fund Type ']/following-sibling::div//p-dropdown");
        internal static By lowestAssetClassDropdownIcon = By.XPath(@"//label[.=' Lowest Level Sub-Asset Class *']/ancestor::div[contains(@class,'flex-column')]//div[@class='p-treeselect-trigger']");
        internal static By labelDropdown(string label) => By.XPath(@"//label[contains(.,'" + label + "')]/ancestor::div[position() = 1]//p-treeselect"); // /preceding-sibling::p-dropdown
        internal static By itemInDropdown(string item) => By.XPath(@"//li[@aria-label='" + item + "' or .='" + item + "']");
        internal static By itemInDropdown(string item, string posTagIndex) => By.XPath(@"//li[@aria-label='" + item + "' or .='" + item + "' and position()=" + posTagIndex + "]"); // pos = index (ex: 2)
        internal static By overlayDropdown = By.XPath(@"//div[contains(@class, 'overlayAnimation')]");
        internal static By labelDropdownInputField(string label) => By.XPath(@"//label[.='" + label + "']/ancestor::div[contains(@class,'flex-column')]//div");
        internal static By fundDropdownInputField(string label) => By.XPath(@"//label[.='" + label + "']/ancestor::div[contains(@class,'flex-column')]//div//input");
        internal static By buttonLinkInDropdown(string linkName) => By.XPath(@"//button[.='" + linkName + "']");
        internal static By fundManagerInputTxt = By.XPath(@"//input[@role='searchbox' and not(@placeholder)]");
        internal static By searchLoadingSpinnerIcon = By.XPath(@"//i[contains(@class,'spinner')]");
        internal static By managerNameReturnOfResults(string fundName) => By.XPath(@"//li[.='" + fundName + "']");
        internal static By fundNameReturnOfResults(string fundName) => By.XPath(@"//div[.=' " + fundName + " ']");
        internal static By fundManagerAddNewLink = By.XPath(@"//span[.='Add New']");
        internal static By managerNameInputTxt = By.XPath(@"//input[@formcontrolname='manager_name']");
        internal static By fundNameInputTxt = By.XPath(@"//input[@formcontrolname='fund_name']");
        internal static By inceptionDateInputTxt = By.XPath(@"//span[contains(@class,'p-calendar')]/input");
        internal static By inceptionDateButton = By.XPath(@"//button[contains(@class,'datepicker')]");
        internal static By inceptionDateDayButton(string day) => By.XPath(@"//span[.='" + day + "']");
        internal static By labelBtn(string label) => By.XPath(@"//button[@label='" + label + "']");
        internal static By menuLabelBtn(string label) => By.XPath(@"//div[.='" + label + "']");
        //internal static By labelInputTxt(string label) => By.XPath(@"//label[contains(.,'" + label + "')]/preceding-sibling::input");
        internal static By labelInputField(string label) => By.XPath(@"//*[local-name()='label' or local-name()='lable-display'][.='" + label + "' or .=' " + label + " ']/ancestor::div[position() = 1]//*[local-name()='input' or local-name()='textarea']");
        internal static By labelFundInputTxt(int number, string label) => By.XPath(@"//div[.=' Fund " + number + " ']/following-sibling::div[1]//label[.='" + label + "']/preceding-sibling::input");
        internal static By loadingIcon = By.XPath(@"//div[contains(@class,'progress-spinner')]");
        internal static By managerNameExistsErrorMessage = By.XPath(@"//div[contains(@class,'sm:col-6 ng-star-inserted')]/span");
        internal static By fundNameExistsErrorMessage = By.XPath(@"//div[contains(@class,'md:col-3 lg:col-2')]/span");
        internal static By toastMessage(string text) => By.XPath(@"//p-toastitem[contains(@class,'toastAnimation')]//div[.='" + text + "']");

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

        public IWebElement dropdownFundType(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(fundTypeDropdown));
        }
        public IWebElement dropdownIconLowestAssetClass(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(lowestAssetClassDropdownIcon));
        }
        public IWebElement dropdownLabel(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(labelDropdown(label)));
        }
        public IWebElement dropdownItem(int timeoutInSeconds, string item)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(itemInDropdown(item)));
        }
        public IWebElement dropdownItem(int timeoutInSeconds, string item, string posTagIndex)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(itemInDropdown(item, posTagIndex)));
        }
        public IWebElement dropdownLabelInputField(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(labelDropdownInputField(label)));
        }
        public IWebElement dropdownFundInputField(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(fundDropdownInputField(label)));
        }
        public IWebElement btnLinkInDropdown(int timeoutInSeconds, string linkName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(buttonLinkInDropdown(linkName)));
        }
        public IWebElement inputTxtFundManager => Driver.Browser.FindElement(fundManagerInputTxt);
        public IWebElement returnOfResultsManagerName(int timeoutInSeconds, string managerName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(managerNameReturnOfResults(managerName)));
        }
        public IWebElement returnOfResultsFundName(int timeoutInSeconds, string fundName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(fundNameReturnOfResults(fundName)));
        }
        public IWebElement addNewLinkFundManager(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(fundManagerAddNewLink));
        }
        public IWebElement inputTxtManagerName => Driver.Browser.FindElement(managerNameInputTxt);
        public IWebElement inputTxtFundName => Driver.Browser.FindElement(fundNameInputTxt);
        public IWebElement inputTxtInceptionDate => Driver.Browser.FindElement(inceptionDateInputTxt);
        public IWebElement buttonInceptionDate(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(inceptionDateButton));
        }
        public IWebElement buttonLabel(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(labelBtn(label)));
        }
        public IWebElement btnMenuLabel(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(menuLabelBtn(label)));
        }
        //public IWebElement inputTxtLabel(string label) => Driver.Browser.FindElement(labelInputTxt(label));
        public IWebElement inputFieldLabel(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(labelInputField(label)));
        }
        public IWebElement inputFundLabelTxt(int number, string label) => Driver.Browser.FindElement(labelFundInputTxt(number, label));
        public IWebElement errorMessageManagerNameExists(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(managerNameExistsErrorMessage));
        }
        public IWebElement errorMessageFundNameExists(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(fundNameExistsErrorMessage));
        }
        public IWebElement toastMessageAlert(int timeoutInSeconds, string text)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(toastMessage(text)));
        }
    }

    internal sealed class AddEditFundAction : BasePage<AddEditFundAction, AddEditFundPage>
    {
        #region Constructor
        private AddEditFundAction(){}
        #endregion

        #region Items Action
        // Wait for loading Spinner icon to disappear
        public AddEditFundAction WaitForLoadingIconToDisappear(int timeoutInSeconds, By element)
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
        public AddEditFundAction WaitForElementVisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(element));
            }
            return this;
        }

        // Wait for element Invisible (can use for dropdown on-overlay Invisible)
        public AddEditFundAction WaitForElementInvisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(element));
            }
            return this;
        }

        // verify elements
        public string ErrorMessageManagerNameExistsGetText(int timeoutInSeconds)
        {
            return this.Map.errorMessageManagerNameExists(timeoutInSeconds).Text;
        }
        public string ErrorMessageFundNameExistsGetText(int timeoutInSeconds)
        {
            return this.Map.errorMessageFundNameExists(timeoutInSeconds).Text;
        }
        //public string toastMessageAlertGetText(int timeout, string text)
        //{
        //    return this.Map.toastMessageAlert(timeout, text).Text;
        //}
        public bool toastMessageAlertGetText(int timeout, string text, string textParam)
        {
            var iweb = Map.toastMessageAlert(timeout, text);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_toastMessageAlertGetText_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }

        // Actions
        /// scroll to element with JavaScript
        public AddEditFundAction ScrollIntoView(IWebElement iwebE)
        {
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].scrollIntoView(false);", iwebE);
            return this;
        }
        public AddEditFundAction ClickFundTypeDropdown(int timeoutInSeconds) 
        {
            Map.HighlightElement(Map.dropdownFundType(timeoutInSeconds)).Click();
            return this;
        }
        public AddEditFundAction ClickLowestAssetClassDropdownIcon(int timeoutInSeconds)
        {
            Map.HighlightElement(Map.dropdownIconLowestAssetClass(timeoutInSeconds)).Click();
            return this;
        }
        public AddEditFundAction ClickLabelDropdown(int timeoutInSeconds, string label)
        {
            Map.HighlightElement(Map.dropdownLabel(timeoutInSeconds, label)).Click();
            return this;
        }
        public AddEditFundAction ClickToSelectItemInDropdown(int timeoutInSeconds, string item)
        {
            ScrollIntoView(Map.dropdownItem(timeoutInSeconds, item));
            //IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            //je.ExecuteScript("arguments[0].click();", this.Map.dropdownItem(timeoutInSeconds, item));
            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.dropdownItem(timeoutInSeconds, item))).Build().Perform();
            return this;
        }
        public AddEditFundAction ClickToSelectItemInDropdown(int timeoutInSeconds, string item, string posTagIndex)
        {
            ScrollIntoView(Map.dropdownItem(timeoutInSeconds, item));
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].click();", Map.dropdownItem(timeoutInSeconds, item, posTagIndex));
            return this;
        }
        public AddEditFundAction ClickLabelDropdownInputField(int timeoutInSeconds, string label)
        {
            Map.HighlightElement(Map.dropdownLabelInputField(timeoutInSeconds, label)).Click(); Thread.Sleep(500);
            int time = 0;
            while (GeneralAction.Instance.IsElementPresent(General.overlayDropdown) == false && time < timeoutInSeconds)
            {
                if (GeneralAction.Instance.IsElementPresent(General.overlayDropdown) == true) { break; }
                if (time == timeoutInSeconds) { Console.WriteLine("Timeout - Click dropdown failed!"); }
                Map.HighlightElement(Map.dropdownLabelInputField(timeoutInSeconds, label)).Click(); Thread.Sleep(1000);
                time++;
            }
            return this;
        }
        public AddEditFundAction ClickFundDropdownInputField(int timeoutInSeconds, string label)
        {
            Map.HighlightElement(Map.dropdownFundInputField(timeoutInSeconds, label)).Click(); Thread.Sleep(500);
            //Actions actions = new Actions(Driver.Browser);
            //actions.DoubleClick(this.Map.dropdownFundInputField(timeoutInSeconds, label)).Build().Perform(); Thread.Sleep(500);

            int time = 0;
            while (GeneralAction.Instance.IsElementPresent(General.overlayDropdown) == false && time < timeoutInSeconds)
            {
                if (GeneralAction.Instance.IsElementPresent(General.overlayDropdown) == true) { break; }
                if (time == timeoutInSeconds) { Console.WriteLine("Timeout - Click dropdown failed!"); }
                Map.HighlightElement(Map.dropdownFundInputField(timeoutInSeconds, label)).Click(); Thread.Sleep(1000);
                //actions.DoubleClick(this.Map.dropdownFundInputField(timeoutInSeconds, label)).Build().Perform(); Thread.Sleep(1000);
                time++;
            }
            return this;
        }
        public AddEditFundAction ClickButtonLinkInDropdown(int timeoutInSeconds, string linkName)
        {
            ScrollIntoView(Map.btnLinkInDropdown(timeoutInSeconds, linkName));
            Map.HighlightElement(Map.btnLinkInDropdown(timeoutInSeconds, linkName)).Click();
            return this;
        }

        /// Fund & Manager Information Section (Items action)
        public AddEditFundAction ClickFundManagerToSearchFund()
        {
            Map.HighlightElement(Map.inputTxtFundManager).Click();
            return this;
        }
        public AddEditFundAction InputFundManagerToSearchFund(string fundManager)
        {
            Map.HighlightElement(Map.inputTxtFundManager).Clear(); System.Threading.Thread.Sleep(500);
            Map.HighlightElement(Map.inputTxtFundManager).SendKeys(fundManager);
            return this;
        }
        public AddEditFundAction ClickManagerNameReturnOfResults(int timeout, string fundName)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.MoveToElement(Map.returnOfResultsManagerName(timeout, fundName));
            actions.Perform();
            Map.HighlightElement(Map.returnOfResultsManagerName(timeout, fundName)).Click();
            return this;
        }
        public AddEditFundAction ClickFundNameReturnOfResults(int timeout, string fundName)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.MoveToElement(Map.returnOfResultsFundName(timeout, fundName));
            actions.Perform();
            Map.HighlightElement(Map.returnOfResultsFundName(timeout, fundName)).Click();
            return this;
        }
        public AddEditFundAction ClickFundManagerAddNewLink(int timeoutInSeconds)
        {
            Map.HighlightElement(Map.addNewLinkFundManager(timeoutInSeconds)).Click();
            return this;
        }
        public AddEditFundAction InputManagerName(string managerName)
        {
            Map.HighlightElement(Map.inputTxtManagerName).Clear();
            Map.HighlightElement(Map.inputTxtManagerName).SendKeys(managerName);
            return this;
        }
        public AddEditFundAction InputFundName(string fundName)
        {
            Map.HighlightElement(Map.inputTxtFundName).Clear();
            Map.HighlightElement(Map.inputTxtFundName).SendKeys(fundName);
            return this;
        }
        public AddEditFundAction InputInceptionDate(int timeoutInSeconds, string date)
        {
            Map.HighlightElement(Map.inputTxtInceptionDate).SendKeys(OpenQA.Selenium.Keys.Control + "a");
            Map.HighlightElement(Map.inputTxtInceptionDate).SendKeys(date);

            // Try with javascript if Element Click Intercepted Exception
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].click();", Map.buttonInceptionDate(timeoutInSeconds));
            WaitForElementInvisible(10, AddEditFundPage.overlayDropdown);
            return this;
        }
        public AddEditFundAction ClickButtonLabel(int timeoutInSeconds, string label)
        {
            Map.HighlightElement(Map.buttonLabel(timeoutInSeconds, label)).Click();
            return this;
        }
        public AddEditFundAction ClickMenuButtonLabel(int timeoutInSeconds, string label)
        {
            //Map.btnMenuLabel(timeoutInSeconds, label).Click(); // issue: Element Click Intercepted Exception

            // Try with javascript if Element Click Intercepted Exception
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].click();", Map.btnMenuLabel(timeoutInSeconds, label));
            return this;
        }
        //public AddEditFundAction InputTxtLabel(string label, string text)
        //{
        //    this.Map.inputTxtLabel(label).Clear();
        //    this.Map.inputTxtLabel(label).SendKeys(text);
        //    return this;
        //}
        public AddEditFundAction ClearInputFieldLabel(int timeoutInSeconds, string label)
        {
            Map.HighlightElement(Map.inputFieldLabel(timeoutInSeconds, label)).SendKeys(OpenQA.Selenium.Keys.Control + "a");
            Map.HighlightElement(Map.inputFieldLabel(timeoutInSeconds, label)).SendKeys(OpenQA.Selenium.Keys.Backspace); Thread.Sleep(250);
            return this;
        }
        public AddEditFundAction InputFieldLabel(int timeoutInSeconds, string label, string txt)
        {
            Map.HighlightElement(Map.inputFieldLabel(timeoutInSeconds, label)).Clear();
            Map.HighlightElement(Map.inputFieldLabel(timeoutInSeconds, label)).SendKeys(txt); Thread.Sleep(250);
            return this;
        }
        public AddEditFundAction InputTxtFundLabel(int fundNumber, string label, string txt)
        {
            Map.HighlightElement(Map.inputFundLabelTxt(fundNumber, label)).Clear();
            Map.HighlightElement(Map.inputFundLabelTxt(fundNumber, label)).SendKeys(txt);
            return this;
        }
        #endregion

        #region Built-in Actions
        public AddEditFundAction ClickAndSelectItemInDropdown(int timeoutInSeconds, string label, string item)
        {
            ScrollIntoView(this.Map.dropdownLabel(timeoutInSeconds, label));
            ClickLabelDropdown(timeoutInSeconds, label);
            /// Click dropdown label until the overlayDropdown is displayed
            int time = 0; 
            while (GeneralAction.Instance.IsElementPresent(General.overlayDropdown) == false && time < timeoutInSeconds)
            {
                if (GeneralAction.Instance.IsElementPresent(General.overlayDropdown) == true) { break; }
                if (time == timeoutInSeconds) { Console.WriteLine("Timeout - Click dropdown failed!"); }
                this.Map.dropdownLabel(timeoutInSeconds, label).Click(); Thread.Sleep(1000);
                time++;
            }
            /// Wait for item in dropdown is displayed, and then click it
            WaitForElementVisible(10, AddEditFundPage.itemInDropdown(item)); Thread.Sleep(500);
            ClickToSelectItemInDropdown(timeoutInSeconds, item); Thread.Sleep(500);
            return this;
        }
        public AddEditFundAction ClickAndSelectItemInDropdown(int timeoutInSeconds, string label, string item, string posTagIndex)
        {
            ScrollIntoView(this.Map.dropdownLabel(timeoutInSeconds, label));
            ClickLabelDropdown(timeoutInSeconds, label);
            WaitForElementVisible(10, General.overlayDropdown);
            WaitForElementVisible(10, General.dropdownSelectItem(item));
            ClickToSelectItemInDropdown(timeoutInSeconds, item, posTagIndex);
            WaitForElementInvisible(10, General.overlayDropdown);
            return this;
        }

        public AddEditFundAction ClickAndSelectItemInDropdownInputField(int timeoutInSeconds, string label, string item)
        {
            ScrollIntoView(this.Map.dropdownLabelInputField(timeoutInSeconds, label));
            ClickLabelDropdownInputField(timeoutInSeconds, label);
            WaitForElementVisible(10, General.overlayDropdown);
            WaitForElementVisible(10, PipelinePage.itemInDropdown(item));
            ClickToSelectItemInDropdown(timeoutInSeconds, item);
            WaitForElementInvisible(10, General.overlayDropdown);
            return this;
        }
        public AddEditFundAction ClickAndSelectItemInDropdownLowestAssetClass(int timeoutInSeconds, string label, string item)
        {
            ScrollIntoView(this.Map.dropdownLabelInputField(timeoutInSeconds, label));
            ClickLowestAssetClassDropdownIcon(timeoutInSeconds);
            WaitForElementVisible(10, General.overlayDropdown);
            WaitForElementVisible(10, PipelinePage.itemInDropdown(item));
            ClickToSelectItemInDropdown(timeoutInSeconds, item);
            WaitForElementInvisible(10, General.overlayDropdown);
            return this;
        }

        public AddEditFundAction ClickAndSelectItemInDropdownInputField(int timeoutInSeconds, string label, string item, string posTagIndex)
        {
            ScrollIntoView(this.Map.dropdownLabelInputField(timeoutInSeconds, label));
            ClickLabelDropdownInputField(timeoutInSeconds, label);
            WaitForElementVisible(10, General.overlayDropdown);
            WaitForElementVisible(10, PipelinePage.itemInDropdown(item));
            ClickToSelectItemInDropdown(timeoutInSeconds, item, posTagIndex);
            WaitForElementInvisible(10, General.overlayDropdown);
            return this;
        }
        #endregion
    }
}
