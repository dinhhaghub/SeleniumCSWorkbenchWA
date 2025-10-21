using iTextSharp.text;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WorkbenchApp.UITest.Core.BaseClass;
using WorkbenchApp.UITest.Core.Selenium;
using WorkbenchApp.UITest.Generals;
using static iTextSharp.text.pdf.events.IndexEvents;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace WorkbenchApp.UITest.Pages
{
    internal class PipelinePage : BasePageElementMap
    {
        // Initiate variables
        internal static WebDriverWait? wait;
        internal static string pipelineHeadertable = "Pipeline", fundList = " Fund List ",
        /// General
        fundManager = " Fund Manager *", fundManagerNoRequired = " Fund Manager ", businessAddressRequired = " Business Address *", businessAddress = " Business Address ", primaryContactFirstLast = " Primary Contact: First, Last *", primaryContactFirstLastNoRequired = " Primary Contact: First, Last ", primaryContactEmail = " Primary Contact Email ", primaryContactEmailRequired = " Primary Contact Email *", businessPhone = " Business Phone ",
        fundName = " Fund Name *", fundNameNoRequired = " Fund Name ", fundLiquidityType = " Fund Liquidity Type *", fundLiquidityTypeNoRequired = " Fund Liquidity Type ", description = " Description ", descriptionRequired = " Description *",
        lowestLevelSubAssetClass = " Lowest Level Sub-Asset Class *", lowestLevelSubAssetClassNoRequired = " Lowest Level Sub-Asset Class ", assetClass = " Asset Class *", assetClassNoRequired = " Asset Class ", fundSubAssetClassLevel2 = " Fund Sub Asset Class Level 2 *", fundSubAssetClassLevel2NoRequired = " Fund Sub Asset Class Level 2 ", fundSubAssetClassLevel1 = " Fund Sub Asset Class Level 1 *", fundSubAssetClassLevel1NoRequired = " Fund Sub Asset Class Level 1 ",
        sector = " Sector ", sectorRequired = " Sector *", geography = " Geography ", geographyRequired = " Geography *",
        /// Process
        primaryResponsible = " Primary Responsible *", primaryResponsibleNoRequired = " Primary Responsible ", secondaryResponsible = " Secondary Responsible ", targetCloseDate = " Target Close Date ", docsDueDate = " Docs Due Date ", lpacSeat = " LPAC Seat ", lpacSeatRequired = " LPAC Seat *", reportingCurrency = " Reporting Currency ", reportingCurrencyRequired = " Reporting Currency *", fundingAmount = " Funding Amount ", closedSize = " Closed Size ", vintageYear = " Vintage Year/ Inception Year ",
        /// Custom Risk Benchmark and Risk
        trackingError = " Tracking Error ", trackingErrorRequired = " Tracking Error *",
        /// Fee Term - Fund Liquidity Type = General
        managementFee = " Management Fee ", managementFeeRequired = " Management Fee *", managementFeePaid = " Management Fee Paid ", managementFeePaidRequired = " Management Fee Paid *", performanceFee = " Performance Fee ", performanceFeeRequired = " Performance Fee *", highWaterMark = " High Water Mark ",
        catchUp = " Catch Up ", catchUpRequired = " Catch Up *", catchUpPercAgeIfSoft = " Catch Up %-age (if Soft) ", catchUpPercAgeIfSoftRequired = " Catch Up %-age (if Soft) *", crystallizationEveryXYears = " Crystallization Every X Years ", crystallizationEveryXYearsRequired = " Crystallization Every X Years *",
        hurdleStatus = " Hurdle Status ", hurdleFixedOrRelative = " Hurdle Fixed or Relative ", hurdleFixedOrRelativeRequired = " Hurdle Fixed or Relative *", hurdleRatePerc = " Hurdle Rate (%) *", hurdleRatePercNoRequired = " Hurdle Rate (%) ", hurdleType = " Hurdle Type ", hurdleTypeRequired = " Hurdle Type *", rampType = " Ramp Type ", rampTypeRequired = " Ramp Type *",
        /// Fee Term - Fund Liquidity Type = Drawdown
        mgtFeeDuringInvestmentPeriod = " Mgt Fee During Investment Period ", mgtFeeDuringInvestmentPeriodRequired = " Mgt Fee During Investment Period *", mgtFeeDuringInvestmentPeriodOn = " Mgt Fee During Investment Period On ", mgtFeeDuringInvestmentPeriodOnRequired = " Mgt Fee During Investment Period On *", stepUpDownDuringInvestmentPeriod = " Step Up/Down During Investment Period ", stepUpDownDuringInvestmentPeriodRequired = " Step Up/Down During Investment Period *", stepUpDownDuringTheInvestmentPeriod = " Step Up/Down During The Investment Period ", mgtFeeFloorDuringInvestmentPeriod = " Mgt Fee Floor During Investment Period ", mgtFeeFloorDuringInvestmentPeriodRequired = " Mgt Fee Floor During Investment Period *",
        mgtFeeAfterInvestmentPeriod = " Mgt Fee After Investment Period ", mgtFeeAfterInvestmentPeriodRequired = " Mgt Fee After Investment Period *", mgtFeeAfterInvestmentPeriodOn = " Mgt Fee After Investment Period On ", mgtFeeAfterInvestmentPeriodOnRequired = " Mgt Fee After Investment Period On *", stepUpDownAfterInvestmentPeriod = " Step Up/Down After Investment Period ", stepUpDownAfterInvestmentPeriodRequired = " Step Up/Down After Investment Period *", stepUpDownRateAfterInvestmentPeriod = " Step Up/Down Rate After Investment Period ", mgtFeeFloorAfterInvestmentPeriod = " Mgt Fee Floor After Investment Period ",
        gPCarry = " GP Carry ", gPCarryRequired = " GP Carry *", carryGrossOrNet = " Carry Gross or Net ", carryGrossOrNetRequired = " Carry Gross or Net *", preferredReturnIfNoPreferredReturnEnter0Perc = " Preferred Return (if no preferred return, enter 0%) ", preferredReturnIfNoPreferredReturnEnter0PercRequired = " Preferred Return (if no preferred return, enter 0%) *", catchup = " Catchup ",
        stepupCarry = " Stepup Carry ", stepupCarryRequired = " Stepup Carry *", stepupCarryPerc = " Stepup Carry % ", stepupCarryPercRequired = " Stepup Carry % *", fundTermYears = " Fund Term (Years) ", investmentPeriodYears = " Investment Period (Years) ", //holdPeriod = " Hold Period ", harvestPeriod = " Harvest Period ",
        /// Liquidity
        lockup = " Lockup ", lockupRequired = " Lockup *", lockupLengthMonths = " Lockup length (months) ", lockupLengthMonthsRequired = " Lockup length (months) *", liqudityFrequency = " Liqudity Frequency ", liqudityFrequencyRequired = " Liqudity Frequency *", noticeDays = " Notice Days ", noticeDaysRequired = " Notice Days *", investorGate = " Investor Gate ", investorGateRequired = " Investor Gate *",
        softLockupRedemptionFeePerc = " Soft Lockup Redemption Fee % ", receiptDays = " Receipt Days ", receiptDaysRequired = " Receipt Days *", holdback = " Holdback ", holdbackRequired = " Holdback *", liquidityNote = " Liquidity Note ", sidepocketProbability = " Sidepocket Probability ", maxPercOfSidepocketPermitted = " Max % of Sidepocket Permitted ";

        // Initiate the By objects for elements
        internal static By pipelineTable = By.XPath(@"//tbody[@class='p-element p-datatable-tbody']");
        internal static By pipelineFundSetupPopupHeader(string header) => By.XPath(@"//div[.='" +header + "']");
        internal static By lowestAssetClassDropdownIcon = By.XPath(@"//label[.=' Lowest Level Sub-Asset Class *']/ancestor::div[contains(@class,'flex-column')]//div[@class='p-treeselect-trigger']");
        internal static By labelDropdown(string label) => By.XPath(@"//label[.='" + label + "']/ancestor::div[contains(@class,'flex-column')]//div[@class='input-container']");
        internal static By itemInDropdown(string item) => By.XPath(@"//li[.='" + item + "']");
        internal static By labelDropdownInputField(string label) => By.XPath(@"//label[.='" + label + "']/ancestor::div[contains(@class,'flex-column')]//div");
        internal static By labelDropdownRemoveInputTextIndexBtn(string label, string index) => By.XPath(labelDropdown(label).ToString().Remove(0, 10) + "//ul/li[" + index + "]/span[2]");
        internal static By labelDropdownTextValue(string label) => By.XPath(labelDropdown(label).ToString().Remove(0, 10).Replace("[@class='input-container']", "") + "[contains(@class,'treeselect-label-container')]");
        internal static By pipelineLinkBtn = By.XPath("//button[@label='Pipeline Link']");
        internal static By pipelineStatusDropdownBtn = By.XPath("//button[contains(@class,'splitbutton')][1]");
        internal static By pipelineStatusDropdownTxt = By.XPath(pipelineStatusDropdownBtn.ToString().Remove(0, 10) +  "/span");
        internal static By saveBtn = By.XPath(@"//div[contains(@class,'custom-grid')]//button[@label='Save']"); // old: ...[2]//button[@label='Save']
        internal static By buttonLinkInDropdown(string linkName) => By.XPath(@"//button[.='" + linkName + "']");
        internal static By labelInputField(string label) => By.XPath(@"//label[.='" + label + "']/ancestor::div[position() = 2]//*[local-name()='input' or local-name()='textarea']");
        internal static By labelInputFieldListTxt(string label, string index) => By.XPath(labelInputField(label).ToString().Remove(0, 10) + "/ancestor::ul[position() = 1]/li[" + index + "]/span[contains(@class,'label')]");
        internal static By labelCheckbox(string label) => By.XPath(labelInputField(label).ToString().Remove(0, 10) + "/ancestor::div[position() = 2]");
        internal static By labelButtonInputField(string label) => By.XPath("//label[.='" + label + "']/ancestor::div[position() = 2]//button");
        internal static By datepickerMonthYearTopButton(string label, string monthOrYear) => By.XPath(labelInputField(label).ToString().Remove(0, 10) + "/following-sibling::div//button[.=' " + monthOrYear + " ']");
        internal static By datepickerMonthYearPicker(string label, string monthOrYear) => By.XPath(labelInputField(label).ToString().Remove(0, 10) + "/following-sibling::div/div[2]/span[.=' " + monthOrYear + " ']");
        internal static By datepickerDayPicker(string label, string day) => By.XPath(labelInputField(label).ToString().Remove(0, 10) + "/following-sibling::div/div[1]//tbody//span[.='" + day + "' and not(contains(@class,'disabled'))]");
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
        public IWebElement dropdownLabelInputField(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(labelDropdownInputField(label)));
        }
        public IWebElement btnRemoveInputTextIndexDropdownLabel(int timeoutInSeconds, string label, string index)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(labelDropdownRemoveInputTextIndexBtn(label, index)));
        }
        public IWebElement dropdownLabelTextValue(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(labelDropdownTextValue(label)));
        }
        public IWebElement btnPipelineLink(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(pipelineLinkBtn));
        }
        public IWebElement btnPipelineStatusDropdown(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(pipelineStatusDropdownBtn));
        }
        public IWebElement txtDropdownPipelineStatus(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(pipelineStatusDropdownTxt));
        }
        public IWebElement btnSave(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(saveBtn));
        }
        public IWebElement btnLinkInDropdown(int timeoutInSeconds, string linkName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(buttonLinkInDropdown(linkName)));
        }
        public IWebElement inputFieldLabel(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(labelInputField(label)));
        }
        public IWebElement inputFieldLabel(int timeoutInSeconds, string label, string index)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(labelInputFieldListTxt(label, index)));
        }
        public IWebElement checkboxLabel(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(labelCheckbox(label)));
        }
        public IWebElement buttonInputFieldLabel(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(labelButtonInputField(label)));
        }
        public IWebElement btnDatepickerMonthYearTop(int timeoutInSeconds, string label, string monthOrYear)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(datepickerMonthYearTopButton(label, monthOrYear)));
        }
        public IWebElement pickerMonthYearInDatepicker(int timeoutInSeconds, string label, string monthOrYear)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(datepickerMonthYearPicker(label, monthOrYear)));
        }
        public IWebElement pickerDayInDatepicker(int timeoutInSeconds, string label, string day)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(datepickerDayPicker(label, day)));
        }
    }

    internal sealed class PipelineAction : BasePage<PipelineAction, PipelinePage>
    {
        #region Constructor
        private PipelineAction() { }
        #endregion

        #region Items Action
        // Wait for loading Spinner icon to disappear
        public PipelineAction WaitForLoadingIconToDisappear(int timeoutInSeconds, By element)
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
        public PipelineAction WaitForAnElementAttributeToChange(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(element));
            }
            return this;
        }

        // Wait for element visible
        public PipelineAction WaitForElementVisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(element));
            }
            return this;
        }

        // Wait for element Invisible (can use for dropdown on-overlay Invisible)
        public PipelineAction WaitForElementInvisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(element));
            }
            return this;
        }

        // verify elements
        public bool LabelInputFieldGetText(int timeoutInSeconds, string label, string textParam)
        {
            ScrollIntoView(Map.inputFieldLabel(timeoutInSeconds, label));
            var iweb = Map.inputFieldLabel(timeoutInSeconds, label);
            bool element = Map.HighlightElement(iweb, "green").GetAttribute("value").Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_GetText_" + label + "_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public bool LabelInputFieldGetText(int timeoutInSeconds, string label, string index, string textParam)
        {
            ScrollIntoView(Map.inputFieldLabel(timeoutInSeconds, label, index));
            var iweb = Map.inputFieldLabel(timeoutInSeconds, label, index);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_GetText_" + label + "_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public bool LabelCheckboxFieldGetText(int timeoutInSeconds, string label, string textParam)
        {
            ScrollIntoView(Map.inputFieldLabel(timeoutInSeconds, label));
            var iweb = Map.inputFieldLabel(timeoutInSeconds, label);
            bool element = Map.HighlightElement(iweb, "green").GetAttribute("aria-checked").Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_CheckboxGetText_" + label + "_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public bool PipelineStatusDropdownGetText(int timeoutInSeconds, string textParam)
        {
            ScrollIntoView(Map.txtDropdownPipelineStatus(timeoutInSeconds));
            var iweb = Map.txtDropdownPipelineStatus(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_PipelineStatusDropdownGetText_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public bool LabelDropdownValueGetText(int timeoutInSeconds, string label, string textParam)
        {
            ScrollIntoView(Map.dropdownLabelTextValue(timeoutInSeconds, label));
            var iweb = Map.dropdownLabelTextValue(timeoutInSeconds, label);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_Dropdown_" + label + "_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }

        // Actions
        /// scroll to element with JavaScript
        public PipelineAction ScrollIntoView(IWebElement iwebE)
        {
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].scrollIntoView(false);", iwebE);
            return this;
        }
        /// PageDown To scroll down page
        public PipelineAction PageDownToScrollDownPage()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.PageDown).Build().Perform();
            return this;
        }
        /// Press Tab keyboard
        public PipelineAction PressTabKeyboard()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.Tab).Build().Perform();
            return this;
        }
        public PipelineAction ClickLowestAssetClassDropdownIcon(int timeoutInSeconds)
        {
            Map.HighlightElement(Map.dropdownIconLowestAssetClass(timeoutInSeconds)).Click();
            return this;
        }
        public PipelineAction ClickLabelDropdown(int timeoutInSeconds, string label)
        {
            //this.Map.dropdownLabel(timeoutInSeconds, label).Click(); Thread.Sleep(500);
            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.dropdownLabel(timeoutInSeconds, label))).Build().Perform(); Thread.Sleep(500);
            int time = 0;
            while (GeneralAction.Instance.IsElementPresent(General.overlayDropdown) == false && time < timeoutInSeconds)
            {
                if (GeneralAction.Instance.IsElementPresent(General.overlayDropdown) == true) { break; }
                if (time == timeoutInSeconds) { Console.WriteLine("Timeout - Click dropdown failed!"); }
                //this.Map.dropdownLabel(timeoutInSeconds, label).Click(); Thread.Sleep(1000);
                actions.Click(Map.HighlightElement(Map.dropdownLabel(timeoutInSeconds, label))).Build().Perform(); Thread.Sleep(250);
                time++;
            }
            return this;
        }
        public PipelineAction ClickToSelectItemInDropdown(int timeoutInSeconds, string item)
        {
            ScrollIntoView(Map.dropdownItem(timeoutInSeconds, item));
            //IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            //je.ExecuteScript("arguments[0].click();", this.Map.dropdownItem(timeoutInSeconds, item));
            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.HighlightElement(Map.dropdownItem(timeoutInSeconds, item))).Build().Perform();
            return this;
        }
        public PipelineAction ClickRemoveInputTextIndexDropdownLabelButton(int timeoutInSeconds, string label, string index)
        {
            Map.HighlightElement(Map.btnRemoveInputTextIndexDropdownLabel(timeoutInSeconds, label, index)).Click();
            PressTabKeyboard();
            return this;
        }
        public PipelineAction ClickPipelineLink(int timeoutInSeconds)
        {
            Map.HighlightElement(Map.btnPipelineLink(timeoutInSeconds)).Click();
            return this;
        }
        public PipelineAction ClickPipelineStatusDropdownButton(int timeoutInSeconds)
        {
            Map.HighlightElement(Map.btnPipelineStatusDropdown(timeoutInSeconds)).Click(); Thread.Sleep(1000);
            int time = 0;
            while (GeneralAction.Instance.IsElementPresent(General.overlayDropdown) == false && time < timeoutInSeconds)
            {
                Map.HighlightElement(Map.btnPipelineStatusDropdown(timeoutInSeconds)).Click(); Thread.Sleep(1000);
                if (GeneralAction.Instance.IsElementPresent(General.overlayDropdown) == true) { break; }
                if (time == timeoutInSeconds) { Console.WriteLine("Timeout - Click 'Pipeline Status' dropdown failed!"); }
                time++;
            }
            return this;
        }
        public PipelineAction ClickSaveButton(int timeoutInSeconds)
        {
            ScrollIntoView(Map.btnSave(timeoutInSeconds));
            Map.HighlightElement(Map.btnSave(timeoutInSeconds)).Click();
            return this;
        }
        public PipelineAction ClickButtonLinkInDropdown(int timeoutInSeconds, string linkName)
        {
            ScrollIntoView(Map.btnLinkInDropdown(timeoutInSeconds, linkName));
            Map.HighlightElement(Map.btnLinkInDropdown(timeoutInSeconds, linkName)).Click();
            return this;
        }
        public PipelineAction InputFieldLabel(int timeoutInSeconds, string label, string txt)
        {
            Map.HighlightElement(Map.inputFieldLabel(timeoutInSeconds, label)).Clear();
            Map.HighlightElement(Map.inputFieldLabel(timeoutInSeconds, label)).SendKeys(txt); Thread.Sleep(250);
            return this;
        }
        public PipelineAction ClickInputFieldLabel(int timeoutInSeconds, string label)
        {
            Map.HighlightElement(Map.inputFieldLabel(timeoutInSeconds, label)).Click(); Thread.Sleep(500);
            int time = 0;
            while (GeneralAction.Instance.IsElementPresent(General.overlayDropdown) == false && time < timeoutInSeconds)
            {
                Map.HighlightElement(Map.inputFieldLabel(timeoutInSeconds, label)).Click(); Thread.Sleep(1000);
                if (GeneralAction.Instance.IsElementPresent(General.overlayDropdown) == true) { break; }
                if (time == timeoutInSeconds) { Console.WriteLine("Timeout - Click dropdown failed!"); }
                time++;
            }
            return this;
        }
        public PipelineAction ClearInputFieldLabel(int timeoutInSeconds, string label)
        {
            Map.HighlightElement(Map.inputFieldLabel(timeoutInSeconds, label)).SendKeys(OpenQA.Selenium.Keys.Control + "a");
            Map.HighlightElement(Map.inputFieldLabel(timeoutInSeconds, label)).SendKeys(OpenQA.Selenium.Keys.Backspace); Thread.Sleep(250);
            return this;
        }
        public PipelineAction ClickCheckboxLabel(int timeoutInSeconds, string label) 
        {
            Map.HighlightElement(Map.checkboxLabel(timeoutInSeconds, label)).Click(); Thread.Sleep(250);
            return this;
        }
        public PipelineAction ClickButtonInputFieldLabel(int timeoutInSeconds, string label)
        {
            Map.HighlightElement(Map.buttonInputFieldLabel(timeoutInSeconds, label)).Click(); Thread.Sleep(250);
            return this;
        }
        public PipelineAction ClickDatepickerMonthYearTopButton(int timeoutInSeconds, string label, string monthOrYear)
        {
            Map.HighlightElement(Map.btnDatepickerMonthYearTop(timeoutInSeconds, label, monthOrYear)).Click(); Thread.Sleep(500);
            return this;
        }
        public PipelineAction ClickToPickerMonthYearInDatepicker(int timeoutInSeconds, string label, string monthOrYear)
        {
            Map.HighlightElement(Map.pickerMonthYearInDatepicker(timeoutInSeconds, label, monthOrYear)).Click(); Thread.Sleep(500);
            return this;
        }
        public PipelineAction ClickToPickerDayInDatepicker(int timeoutInSeconds, string label, string day)
        {
            Map.HighlightElement(Map.pickerDayInDatepicker(timeoutInSeconds, label, day)).Click(); Thread.Sleep(500);
            return this;
        }
        #endregion

        #region Built-in Actions
        public PipelineAction ClickAndSelectItemInDropdown(int timeoutInSeconds, string label, string item)
        {
            ScrollIntoView(Map.dropdownLabel(timeoutInSeconds, label));
            ClickLabelDropdown(timeoutInSeconds, label);
            WaitForElementVisible(10, General.overlayDropdown); Thread.Sleep(200);
            WaitForElementVisible(10, PipelinePage.itemInDropdown(item));
            ClickToSelectItemInDropdown(timeoutInSeconds, item); Thread.Sleep(500);
            //WaitForElementInvisible(10, General.overlayDropdown);
            return this;
        }
        public PipelineAction ClickAndSelectItemInDropdownLowestAssetClass(int timeoutInSeconds, string label, string item)
        {
            ScrollIntoView(Map.dropdownLabelInputField(timeoutInSeconds, label));
            ClickLowestAssetClassDropdownIcon(timeoutInSeconds);
            WaitForElementVisible(10, General.overlayDropdown); Thread.Sleep(200);
            WaitForElementVisible(10, PipelinePage.itemInDropdown(item));
            ClickToSelectItemInDropdown(timeoutInSeconds, item); Thread.Sleep(500);
            WaitForElementInvisible(10, General.overlayDropdown);
            return this;
        }
        public PipelineAction ClickAndSelectItemInDropdownPipelineStatus(int timeoutInSeconds, string item)
        {
            ClickPipelineStatusDropdownButton(timeoutInSeconds);
            WaitForElementVisible(10, General.overlayDropdown);
            WaitForElementVisible(10, PipelinePage.itemInDropdown(item));
            Map.dropdownItem(timeoutInSeconds, item).Click();
            WaitForElementInvisible(10, General.overlayDropdown);
            return this;
        }
        public PipelineAction ClickCheckboxLabelToCheck(int timeoutInSeconds, string label) 
        {
            var isChecked = Map.inputFieldLabel(timeoutInSeconds, label).GetAttribute("aria-checked");
            if (isChecked.Contains("false"))
            {
                ClickCheckboxLabel(timeoutInSeconds, label);
                WaitForElementInvisible(timeoutInSeconds, By.XPath(PipelinePage.labelInputField(label).ToString().Remove(0, 10) + "[@aria-checked='false']"));
                WaitForAnElementAttributeToChange(timeoutInSeconds, By.XPath(PipelinePage.labelInputField(label).ToString().Remove(0, 10) + "[@aria-checked='true']"));
            }
            return this;
        }
        public PipelineAction ClickCheckboxLabelToUnCheck(int timeoutInSeconds, string label)
        {
            var isChecked = Map.inputFieldLabel(timeoutInSeconds, label).GetAttribute("aria-checked");
            if (isChecked.Contains("true"))
            {
                ClickCheckboxLabel(timeoutInSeconds, label);
                WaitForElementInvisible(timeoutInSeconds, By.XPath(PipelinePage.labelInputField(label).ToString().Remove(0, 10) + "[@aria-checked='true']"));
                WaitForAnElementAttributeToChange(timeoutInSeconds, By.XPath(PipelinePage.labelInputField(label).ToString().Remove(0, 10) + "[@aria-checked='false']"));
            }
            return this;
        }
        public PipelineAction ClickAndSelectDayMonthYearInDatePickerLabel(int timeoutInSeconds, string label, string year, string month, string day) 
        {
            // Click a date-picker label
            ClickInputFieldLabel(timeoutInSeconds, label);
            WaitForElementVisible(timeoutInSeconds, General.overlayDropdown);

            // CLick a Year at top button
            ClickDatepickerMonthYearTopButton(timeoutInSeconds, label, year);

            // CLick to pick a Year in date-picker
            ClickToPickerMonthYearInDatepicker(timeoutInSeconds, label, year);

            // CLick to pick a Month in date-picker
            ClickToPickerMonthYearInDatepicker(timeoutInSeconds, label, month);

            // CLick to pick a Day in date-picker
            ClickToPickerDayInDatepicker(timeoutInSeconds, label, day);
            WaitForElementInvisible(timeoutInSeconds, General.overlayDropdown);
            return this;
        }
        #endregion
    }
}
