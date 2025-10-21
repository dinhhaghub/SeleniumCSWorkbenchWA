using AventStack.ExtentReports.Gherkin.Model;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WorkbenchApp.UITest.Core.BaseClass;
using WorkbenchApp.UITest.Core.Selenium;
using WorkbenchApp.UITest.Generals;

namespace WorkbenchApp.UITest.Pages
{
    internal class SearchFundPage : BasePageElementMap
    {
        // Initiate variables
        internal static WebDriverWait? wait;
        internal static string overview = "Overview";
        internal static string fundSetup = "Fund Setup";
        internal static string rating = "Rating";
        internal static string checklist = "Checklist";
        internal static string model = "Model";
        internal static string dynamo = "Dynamo";
        internal static string userInput = "Model Parameters"; // old name: User Input
        /// Data Status
        internal static string rowCounts = "Row Counts";
        internal static string startDate = "Start Date";
        internal static string endDate = "End Date";
        internal static string rowCount = "Row Count";
        internal static string asOfDate = "As of Date";
        internal static string uploadDate = "Upload Date";
        internal static string fileType = "File Type";
        /// Public Report
        internal static string print = "Print";
        internal static string dataStatus = "Data Status";
        internal static string dateSection = "Date Section";
        internal static string customRiskBenchmarkModelling = "Custom Risk Benchmark Modeling";
        internal static string feeModelSection = "Fee Model Section";
        internal static string liquiditySection = "Liquidity Section";
        internal static string trackingError = "Tracking Error";
        internal static string reportEndDate = "Report End Date";
        internal static string customStartDate1 = "Custom Start Date 1";
        internal static string customStartDate2 = "Custom Start Date 2";
        internal static string customEndDate1 = "Custom End Date 1";
        internal static string customEndDate2 = "Custom End Date 2";
        internal static string managementFee = "Management Fee";
        internal static string managementFeePaid = "Management Fee Paid";
        internal static string performanceFee = "Performance Fee";
        internal static string highWaterMark = "High Water Mark";
        internal static string catchUp = "Catch Up";
        internal static string catchUpPercAgeIfSoft = "Catch Up %-age (if Soft)";
        internal static string crystallizationEveryXYears = "Crystallization Every X Years";
        internal static string hurdleStatus = "Hurdle Status";
        internal static string hurdleFixedOrRelative = "Hurdle Fixed or Relative";
        internal static string hurdleBenchmark = "Hurdle Benchmark";
        internal static string lockup = "Lockup";
        internal static string lockupLengthMonths = "Lockup Length (months)";
        internal static string liquidityFrequency = "Liquidity Frequency";
        internal static string investorGate = "Investor Gate";
        internal static string sidepocketProbability = "Sidepocket Probability";
        internal static string MaxPercOfSidepocketPermitted = "Max % of Sidepocket Permitted";
        internal static string currency = "currency"; // Upload file
        internal static string date = "date"; // Upload file
        internal static string share_class_id = "share_class_id"; // Upload file
        internal static string exposure = "exposure"; // Upload file
        internal static string aum = "aum"; // Upload file
        internal static string gross = "gross"; // Upload file
        internal static string net = "net"; // Upload file
        internal static string other_amt_1 = "other_amt_1"; // Upload file
        internal static string other_amt_2 = "other_amt_2"; // Upload file
        internal static string other_amt_3 = "other_amt_3"; // Upload file
        /// Cambridge Report
        internal static string dataSource = "Data source";
        internal static string dateSelection = "Date Selection";
        internal static string weightingMethodology = "Weighting Methodology";
        internal static string attributionCategory1 = "Attribution Category 1";
        internal static string attributionCategory2 = "Attribution Category 2";
        internal static string attributionCategory3 = "Attribution Category 3";
        internal static string assetClass = "Asset Class";
        internal static string geography = "Geography";
        internal static string company_name = "company_name";
        internal static string contribution = "contribution";
        internal static string custom_weight = "custom_weight";
        internal static string data_as_of_date = "data_as_of_date";
        internal static string distribution = "distribution";
        internal static string entry_date = "entry_date";
        internal static string exit_date = "exit_date";
        internal static string fund = "fund";
        internal static string fund_name = "fund_name";
        internal static string fund_size = "fund_size";
        internal static string gross_irr = "gross_irr";
        internal static string gross_tvpi = "gross_tvpi";
        internal static string net_irr = "net_irr";
        internal static string net_tvpi = "net_tvpi";
        internal static string invested_capital = "invested_capital";
        internal static string realized = "realized";
        internal static string realized_capital = "realized_capital";
        internal static string status = "status";
        internal static string unrealized_fmv = "unrealized_fmv";
        internal static string unrealized_current_nav = "unrealized_current_nav";
        internal static string vintage_year = "vintage_year";
        internal static string attribution_category_1 = "attribution_category_1";
        internal static string attribution_category_2 = "attribution_category_2";
        internal static string attribution_category_3 = "attribution_category_3";
        /// Deal by Deal table
        internal static string managerInfoHeader = "app-manager-info";
        internal static string crbmHeader = "app-private-custom-risk-benchmark";
        internal static string fundTableHeader = "app-private-fund-table";
        internal static string summaryTableHeader = "app-private-summary-table";
        internal static string resultTableHeader = "app-private-deal-result-table";
        internal static string summaryOfGTATableHeader = "app-private-deal-summary-by-gta";
        internal static string attributionTableHeader(int number) => "app-private-deal-attribution-result[" + number + "]";
        internal static string baseTableHeader = "app-private-deal-base-table";

        // Initiate the By objects for elements
        internal static By valueDataStatus(int row, string label) => By.XPath(@"//div[.=' Data Status ']/following-sibling::div/app-fund-data-status/div[" + row + "]//div[.='" + label + "' or .=' " + label + " ']/following-sibling::div");
        internal static By valuePrivateDataStatus(int row, string label) => By.XPath(@"//app-manager-data-status/div[" + row + "]//div[.=' " + label + " ' or .='" + label + "']/following-sibling::div");
        internal static By fundNavbarTable = By.XPath(@"//table[@class='navbar-table']");
        internal static By valueFundNavbarTable(int row, int column, int valNumber) => By.XPath(@"//table[@class='navbar-table']/tr[" + row + "]/td[" + column + "]//span[" + valNumber + "]");
        internal static By searchFundInputTxt = By.XPath(@"//input[@placeholder='Search']");
        internal static By fundNameReturnOfResultsWithItemSource(string fundName, string sourceIcon) => By.XPath(@"//div[.='" + fundName + "']/preceding-sibling::span[.='" + sourceIcon + "']");
        internal static By fundNameReturnOfResultsWithItemSourceIndex(string fundName, string sourceIcon, string index) => By.XPath(fundNameReturnOfResultsWithItemSource(fundName, sourceIcon).ToString().Remove(0, 10) + "/ancestor::div[@class='ng-star-inserted selected']/li["+index+"]");
        internal static By fundNameReturnOfResults(string fundName) => By.XPath(@"//div[.='" + fundName + "']");
        internal static By userInputPanel = By.XPath(@"//div[contains(@class, 'animation')]");
        internal static By userInputSubSection(string sectionName) => By.XPath(@"//span[@class='action']/ancestor::div[@class='section-title' and contains(.,'" + sectionName + "')]");
        internal static By labelInputTxt(string label) => By.XPath(@"//label[.='" + label + "']/preceding-sibling::input");
        internal static By labelSearchInputTxt(string label) => By.XPath(@"//label[.='" + label + "']/preceding-sibling::p-autocomplete//input");
        internal static By datePickerLabelButton(string label) => By.XPath(@"//label[contains(.,'" + label + "')]/preceding-sibling::p-calendar//button");
        internal static By datePickerTitleButton(string title) => By.XPath(@"//div[.='" + title + "' or .=' " + title + " ']/following-sibling::div//button");
        internal static By fieldCheckbox(string label) => By.XPath(@"//label[.='" + label + "']");
        internal static By labelDropdown(string label) => By.XPath(@"//label[.='" + label + "']/preceding-sibling::p-dropdown");
        internal static By dropdownSelect(string item) => By.XPath(@"//li[@aria-label='" + item + "']");
        internal static By overlayDropdown = By.XPath(@"//div[contains(@class, 'overlayContentAnimation') or contains(@class, 'overlayAnimation')]");
        internal static By datePickerPrevButton = By.XPath(@"//button[contains(@class,'datepicker-prev')]");
        internal static By datePickerNextButton = By.XPath(@"//button[contains(@class,'datepicker-next')]");
        internal static By datePickerMonthOnTopButton = By.XPath(@"//button[contains(@class,'p-datepicker-month')]");
        internal static By datePickerYearOnTopButton = By.XPath(@"//button[contains(@class,'p-datepicker-year')]");
        internal static By datePickerMonthOrYearButton(string monthOrYear) => By.XPath(@"//span[.=' " + monthOrYear + " ']");
        internal static By datePickerDateButton(string date) => By.XPath(@"//span[.='" + date + "' and not(contains(@class,'p-disabled'))]");
        internal static By addCRBMbutton = By.XPath(@"//th[@class='gend-action']/button");
        internal static By deleteCRBMbutton(int number) => By.XPath(@"//tbody[contains(@class,'datatable')]/tr[" + number.ToString() + "]//button");
        internal static By nameCRBMInputTxtRow(int number) => By.XPath(@"//tbody[contains(@class,'datatable')]/tr[" + number.ToString() + "]/td[@class='b-name']//input");
        internal static By nameCRBMReturnOfResults(string benchmark) => By.XPath(@"//span[.='" + benchmark + "']");
        internal static By betaCRBMInputNumberRow(int number) => By.XPath(@"//tbody[contains(@class,'datatable')]/tr[" + number.ToString() + "]/td[@class='beta']//input");
        internal static By grossExposureCRBMInputNumberRow(int number) => By.XPath(@"//tbody[contains(@class,'datatable')]/tr[" + number.ToString() + "]/td[@class='exposure']//input");
        internal static By nameCRBMRedErrorMessageRow(int number) => By.XPath(@"//tbody[contains(@class,'datatable')]/tr[" + number.ToString() + "]/following-sibling::tr//div");
        internal static By nameCRBMErrorMessageRow(int number) => By.XPath(@"//tbody[contains(@class,'datatable')]/tr[" + number.ToString() + "]/td[@class='b-name']//div");
        /// Hurdle Benchmark
        internal static By addButtonInXTable(string attrValue) => By.XPath(@"//div[contains(@class,'" + attrValue + "')]//button[@icon='pi pi-plus']");
        internal static By deleteButtonInXTable(string attrValue, string number) => By.XPath(@"//div[contains(@class,'" + attrValue + "')]//tbody/tr[" + number + "]//button");
        internal static By nameBenchmarkXTableInputTxtRow(string attrValue, string number) => By.XPath(@"//div[contains(@class,'" + attrValue + "')]//tbody/tr[" + number + "]/td[@class='b-name']//input");
        internal static By exposureBenchmarkXTableInputTxtRow(string attrValue, string number) => By.XPath(@"//div[contains(@class,'" + attrValue + "')]//tbody/tr[" + number + "]/td[@class='exposure']//input");
        internal static By MessageFundDoesNotHaveReturnData = By.XPath(@"//div[contains(@class,'error invalid-message')]");
        internal static By columnNameInTableOfReport(string table, int columnNumber) => By.XPath(@"//div[.=' " + table + " ']/following-sibling::div//table/thead/tr/th[" + columnNumber + "]");
        internal static By columnNameTVPIAndDPIInTableOfPrivReport(int columnNumber) => By.XPath(@"//th[.='TVPI']/ancestor::div/following-sibling::div//table/thead/tr/th[" + columnNumber + "]");
        internal static By dataInTableOfReport(string table, string row, string column) => By.XPath(@"//div[.=' " + table + " ']/following-sibling::div//table/tbody/tr[" + row + "]/td[" + column + "]");
        internal static By dataInTVPIAndDPITableOfReport(string row, string column) => By.XPath(@"//div[contains(.,'TVPI')]/following-sibling::div//table/tbody/tr[" + row + "]/td[" + column + "]");
        internal static By drawdownGraphPublicReport = By.XPath(@"//app-drawdown-graph//p-chart");
        internal static By rollingCorrelationGraphPublicReport = By.XPath(@"//app-rolling-correlation-graph//p-chart");
        internal static By fundAumGraphPublicReport = By.XPath(@"//app-fund-aum-graph//p-chart");
        internal static By runButton = By.XPath(@"//p-button[@type='submit']");
        internal static By labelButton(string label) => By.XPath(@"//button[contains(.,'" + label + "')]");
        internal static By dialogPopup = By.XPath(@"//div[@role='dialog']");
        internal static By dialogWarning = By.XPath(@"//div[contains(@class,'confirm-dialog')]");
        internal static By spinnerLoading = By.XPath(@"//p-progressspinner[@class='p-element']"); //span[contains(.,'Uploading')]
        internal static By uploadFile = By.XPath(@"//input[@type='file']");
        internal static By toastMessageInvalidFile = By.XPath(@"//div[contains(@class,'p-message-wrapper')]/span[contains(@class,'message-summary')]");
        internal static By toastMessageInvalidFileDetail = By.XPath(@"//div[contains(@class,'p-message-wrapper')]/span[contains(@class,'message-detail')]");
        internal static By closeToastMessageButton = By.XPath(@"//button[contains(@class,'message-close')]");
        internal static By errorInvalidMessageContent(string content) => By.XPath(@"//div[contains(@class,'error invalid-message') and contains(.,'" + content + "')]"); // in red text
        internal static By toastMessage(int number) => By.XPath(@"//p-toastitem[contains(@class,'toastAnimation')]//div[@role='alert']/div/div[" + number + "]");
        internal static By destinationDropdown(int row) => By.XPath(@"//tbody[@class='ng-star-inserted']/tr[" + row + "]/td/p-dropdown");
        internal static By destinationDropdownItemSelect(int row, string item) => By.XPath(@"//tbody[@class='ng-star-inserted']/tr[" + row + "]/td/p-dropdown//li[@aria-label='" + item + "']");
        internal static By asOfDateInputTxt = By.XPath(@"//div[@class='input-section']//span[contains(@class,'p-calendar')]/input");
        internal static By asOfDateButton = By.XPath(@"//div[@class='input-section']//button[contains(@class,'datepicker')]");
        internal static By buttonInDialog(string label) => By.XPath(@"//div[contains(@class,'p-dialog-footer')]/button[.='" + label + "']");
        internal static By backButton = By.XPath(@"//button[@icon='pi pi-arrow-left']");
        /// Deal by Deal (By objects)
        internal static By dxDReportHeader(string table, int columnNumber) => By.XPath(@"//" + table + "//thead/tr/th[" + columnNumber + "]");
        internal static By dxDReportHeaderNameX(string table, string nameX) => By.XPath(@"//" + table + "//tbody//td[.='" + nameX + "']");
        internal static By dxDReportData(string table, string row, string column) => By.XPath(@"//" + table + "//tbody/tr[" + row + "]/td[" + column + "]");
        internal static By dxDReportDataNameX(string table, string nameX, string column) => By.XPath(@"//" + table + "//tbody//td[.='" + nameX + "']/following-sibling::td[" + column + "]");

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

        public IWebElement dataStatusValue(int timeoutInSeconds, int row, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(valueDataStatus(row, label)));
        }
        public IWebElement dataStatusPrivateValue(int timeoutInSeconds, int row, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(valuePrivateDataStatus(row, label)));
        }
        public IWebElement fundNavbarTableValue(int timeoutInSeconds, int row, int column, int valNumber)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(valueFundNavbarTable(row, column, valNumber)));
        }
        public IWebElement inputTxtSearchFund(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(searchFundInputTxt));
        }
        public IWebElement returnOfResultsFundName(int timeoutInSeconds, string fundName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(fundNameReturnOfResults(fundName)));
        }
        public IWebElement returnOfResultsFundNameWithItemSource(int timeoutInSeconds, string fundName, string sourceIcon)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(fundNameReturnOfResultsWithItemSource(fundName, sourceIcon)));
        }
        public IWebElement returnOfResultsFundNameWithItemSource(int timeoutInSeconds, string fundName, string sourceIcon, string index)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(fundNameReturnOfResultsWithItemSourceIndex(fundName, sourceIcon, index)));
        }
        public IWebElement subSectionUserInput(int timeoutInSeconds, string sectionName)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(userInputSubSection(sectionName)));
        }
        public IWebElement inputTxtLabel(string label) => Driver.Browser.FindElement(labelInputTxt(label));
        public IWebElement inputTxtSearchLabel(string label) => Driver.Browser.FindElement(labelSearchInputTxt(label));
        public IWebElement buttondatePickerLabel(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(datePickerLabelButton(label)));
        }
        public IWebElement buttondatePickerTitle(int timeoutInSeconds, string title)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(datePickerTitleButton(title)));
        }
        public IWebElement buttonOnTopDatePickerYear(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(datePickerYearOnTopButton));
        }
        public IWebElement buttonDatePickerMonthOrYear(int timeoutInSeconds, string monthOrYear)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(datePickerMonthOrYearButton(monthOrYear)));
        }
        public IWebElement buttonDatePickerDate(int timeoutInSeconds, string date)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(datePickerDateButton(date)));
        }
        public IWebElement checkboxField(string label) => Driver.Browser.FindElement(fieldCheckbox(label));
        public IWebElement dropdownLabel(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(labelDropdown(label)));
        }
        public IWebElement selectItemDropdown(int timeoutInSeconds, string item)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(dropdownSelect(item)));
        }
        public IWebElement buttonAddCRBM(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(addCRBMbutton));
        }
        public IWebElement buttonDeleteCRBM(int timeoutInSeconds, int number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(deleteCRBMbutton(number)));
        }
        public IWebElement inputTxtNameCRBMRow(int timeoutInSeconds, int number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(nameCRBMInputTxtRow(number)));
        }
        public IWebElement returnOfResultsNameCRBM(int timeoutInSeconds, string benchmark)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(nameCRBMReturnOfResults(benchmark)));
        }
        public IWebElement inputNumberBetaCRBMRow(int timeoutInSeconds, int number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(betaCRBMInputNumberRow(number)));
        }
        public IWebElement inputNumberGrossExposureCRBMRow(int timeoutInSeconds, int number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(grossExposureCRBMInputNumberRow(number)));
        }
        public IWebElement redErrorMessageNameCRBMRow(int timeoutInSeconds, int number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(nameCRBMRedErrorMessageRow(number)));
        }
        public IWebElement errorMessageNameCRBMRow(int timeoutInSeconds, int number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(nameCRBMErrorMessageRow(number)));
        }
        public IWebElement buttonAddInXTable(int timeoutInSeconds, string attrValue)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(addButtonInXTable(attrValue)));
        }
        public IWebElement buttonDeleteInXTable(int timeoutInSeconds, string attrValue, string number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(deleteButtonInXTable(attrValue, number)));
        }
        public IWebElement inputTxtNameBenchmarkXTableRow(int timeoutInSeconds, string attrValue, string number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(nameBenchmarkXTableInputTxtRow(attrValue, number)));
        }
        public IWebElement inputTxtExposureBenchmarkXTableRow(int timeoutInSeconds, string attrValue, string number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(exposureBenchmarkXTableInputTxtRow(attrValue, number)));
        }
        public IWebElement redMsgFundDoesNotHaveReturnData(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(MessageFundDoesNotHaveReturnData));
        }
        public IWebElement headerTableOfReport(int timeoutInSeconds, string table, int column)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(columnNameInTableOfReport(table, column)));
        }
        public IWebElement headerTVPIAndDPITableOfReport(int timeoutInSeconds, int column)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(columnNameTVPIAndDPIInTableOfPrivReport(column)));
        }
        public IWebElement dataTableOfReport(int timeoutInSeconds, string table, string row, string column)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(dataInTableOfReport(table, row, column)));
        }
        public IWebElement dataTVPIAndDPITableOfReport(int timeoutInSeconds, string row, string column)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(dataInTVPIAndDPITableOfReport(row, column)));
        }
        public IWebElement publicReportDrawdownGraph(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(drawdownGraphPublicReport));
        }
        public IWebElement publicReportRollingCorrelationGraph(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(rollingCorrelationGraphPublicReport));
        }
        public IWebElement publicReportFundAumGraph(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(fundAumGraphPublicReport));
        }
        public IWebElement buttonRun(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(runButton));
        }
        public IWebElement buttonLabel(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(labelButton(label)));
        }
        public IWebElement fileUpload => Driver.Browser.FindElement(uploadFile);
        public IWebElement invalidFileToastMessage(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(toastMessageInvalidFile));
        }
        public IWebElement invalidFileDetailToastMessage(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(toastMessageInvalidFileDetail));
        }
        public IWebElement buttonCloseToastMessage(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(closeToastMessageButton));
        }
        public IWebElement toastMessageAlert(int timeoutInSeconds, int number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(toastMessage(number)));
        }
        public IWebElement dropdownDestination(int timeoutInSeconds, int row)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(destinationDropdown(row)));
        }
        public IWebElement selectItemDestinationDropdown(int timeoutInSeconds, int row, string item)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(destinationDropdownItemSelect(row, item)));
        }
        public IWebElement inputTxtAsOfDate => Driver.Browser.FindElement(asOfDateInputTxt);
        public IWebElement buttonAsOfDate(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(asOfDateButton));
        }
        public IWebElement buttonBack(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(backButton));
        }
        public IWebElement buttonInDialogPopup(int timeoutInSeconds, string label)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(buttonInDialog(label)));
        }
        /// Deal by Deal (elements)
        public IWebElement headertableDxD(int timeoutInSeconds, string table, int columnNumber)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(dxDReportHeader(table, columnNumber)));
        }
        public IWebElement headertableDxDNameX(int timeoutInSeconds, string table, string nameX)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(dxDReportHeaderNameX(table, nameX)));
        }
        public IWebElement datatableDxD(int timeoutInSeconds, string table, string row, string column)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(dxDReportData(table, row, column)));
        }
        public IWebElement datatableDxDNameX(int timeoutInSeconds, string table, string nameX, string column)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(dxDReportDataNameX(table, nameX, column)));
        }
        
    }

    internal sealed class SearchFundAction : BasePage<SearchFundAction, SearchFundPage>
    {
        #region Constructor
        private SearchFundAction() { }
        #endregion

        #region Items Action
        // Wait for the new tab
        public SearchFundAction WaitForNewTab(int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(d => d.WindowHandles.Count == 2);
            }
            return this;
        }

        // Wait for more new tabs
        public SearchFundAction WaitForMoreNewTab(int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(d => d.WindowHandles.Count >= 2);
            }
            return this;
        }

        // Wait for loading Spinner icon to disappear
        public SearchFundAction WaitForLoadingIconToDisappear(int timeoutInSeconds, By element)
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
        public SearchFundAction WaitForElementVisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(element));
            }
            return this;
        }

        // Wait for all of elements visible (use for dropdown on-overlay visible)
        public SearchFundAction WaitForAllElementsVisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(element));
            }
            return this;
        }

        // Wait for element Invisible (can use for dropdown on-overlay Invisible)
        public SearchFundAction WaitForElementInvisible(int timeoutInSeconds, By element)
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

        // Scroll into Element
        public SearchFundAction ScrollIntoView(IWebElement iwebE)
        {
            //IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            //je.ExecuteScript("arguments[0].scrollIntoView(false);", iwebE);

            Actions actions = new Actions(Driver.Browser);
            actions.ScrollToElement(iwebE).Build().Perform();
            return this;
        }

        // PageDown To scroll down page
        public SearchFundAction PageDownToScrollDownPage()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.PageDown).Build().Perform();
            return this;
        }

        // PageUp To scroll up page
        public SearchFundAction PageUpToScrollDownPage()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.PageUp).Build().Perform();
            return this;
        }

        // Press Up arrow keyboard
        public SearchFundAction PressUpArrowKeyboard()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.ArrowUp).Build().Perform();
            return this;
        }

        // Press Enter keyboard
        public SearchFundAction PressEnterKeyboard()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.Enter).Build().Perform();
            return this;
        }

        // verify elements
        public string ValueDataStatusGetText(int timeoutInSeconds, int row, string label)
        {
            return Map.dataStatusValue(timeoutInSeconds, row, label).Text;
        }
        public bool ValueDataStatusGetText(int timeoutInSeconds, int row, string label, string textParam)
        {
            ScrollIntoView(Map.dataStatusValue(timeoutInSeconds, row, label));
            var iweb = Map.dataStatusValue(timeoutInSeconds, row, label);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DataStatusGetText_" + label + "_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public string ValueDataStatusPrivateGetText(int timeoutInSeconds, int row, string label)
        {
            return this.Map.dataStatusPrivateValue(timeoutInSeconds, row, label).Text;
        }
        public bool ValueDataStatusPrivateGetText(int timeoutInSeconds, int row, string label, string textParam)
        {
            ScrollIntoView(Map.dataStatusPrivateValue(timeoutInSeconds, row, label));
            var iweb = Map.dataStatusPrivateValue(timeoutInSeconds, row, label);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DataStatusPrivateGetText_" + label + "_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public string IsDatePickerLabelShown(int timeoutInSeconds)
        {
            var wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(@"//div[.=' Date Selection ' or .='Date Selection']/following-sibling::div//label"))).Text;
        }
        public string ValueInFundNavbarTable(int timeoutInSeconds, int row, int column, int valNumber)
        {
            return this.Map.fundNavbarTableValue(timeoutInSeconds, row, column, valNumber).Text;
        }
        public string RedErrorMessageNameCRBMGetTextRow(int timeoutInSeconds, int number)
        {
            return this.Map.redErrorMessageNameCRBMRow(timeoutInSeconds, number).Text;
        }
        public string ErrorMessageNameCRBMGetTextRow(int timeoutInSeconds, int number)
        {
            return this.Map.errorMessageNameCRBMRow(timeoutInSeconds, number).Text;
        }
        public string RedMessageFundDoesNotHaveReturnDataGetText(int timeoutInSeconds)
        {
            return this.Map.redMsgFundDoesNotHaveReturnData(timeoutInSeconds).Text;
        }
        public bool HeaderTableOfReportGetText(int timeoutInSeconds, string table, int column, string textParam)
        {
            ScrollIntoView(Map.headerTableOfReport(timeoutInSeconds, table, column));
            var iweb = Map.headerTableOfReport(timeoutInSeconds, table, column);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_HeaderTableOfReport_" + table + "_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public bool HeaderTVPIAndDPITableOfReportGetText(int timeoutInSeconds, int column, string textParam)
        {
            ScrollIntoView(Map.headerTVPIAndDPITableOfReport(timeoutInSeconds, column));
            var iweb = Map.headerTVPIAndDPITableOfReport(timeoutInSeconds, column);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_HeaderTVPIAndDPIReport_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public string DataTableOfReportGetText(int timeoutInSeconds, string table, string row, string column)
        {
            ScrollIntoView(Map.dataTableOfReport(timeoutInSeconds, table, row, column));
            Map.HighlightElement(Map.dataTableOfReport(timeoutInSeconds, table, row, column), "blue");
            return Map.dataTableOfReport(timeoutInSeconds, table, row, column).Text;
        }
        public string DataTVPIAndDPITableOfReportGetText(int timeoutInSeconds, string row, string column)
        {
            return Map.dataTVPIAndDPITableOfReport(timeoutInSeconds, row, column).Text;
        }
        public bool IsDrawdownGraphPublicReportShown(int timeoutInSeconds)
        {
            var iweb = Map.publicReportDrawdownGraph(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Enabled;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute"); // keep highlight in red border if fail
                Driver.TakeScreenShot("ss_IsDrawdownGraphPublicReportShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsRollingCorrelationGraphPublicReportShown(int timeoutInSeconds)
        {
            var iweb = this.Map.publicReportRollingCorrelationGraph(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Enabled;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute"); // keep highlight in red border if fail
                Driver.TakeScreenShot("ss_IsRollingCorrelationGraphPublicReportShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsFundAumGraphPublicReportShown(int timeoutInSeconds)
        {
            var iweb = this.Map.publicReportFundAumGraph(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Enabled;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute"); // keep highlight in red border if fail
                Driver.TakeScreenShot("ss_IsFundAumGraphPublicReportShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public string ToastMessageInvalidFileGetText(int timeoutInSeconds)
        {
            return this.Map.invalidFileToastMessage(timeoutInSeconds).Text;
        }
        public string ToastMessageInvalidFileDetailGetText(int timeoutInSeconds)
        {
            return this.Map.invalidFileDetailToastMessage(timeoutInSeconds).Text;
        }
        public string toastMessageAlertGetText(int timeoutInSeconds, int number)
        {
            return this.Map.toastMessageAlert(timeoutInSeconds, number).Text;
        }
        /// Deal by Deal
        public bool HeadertableDxDGetText(int timeoutInSeconds, string table, int column, string textParam)
        {
            ScrollIntoView(Map.headertableDxD(timeoutInSeconds, table, column));
            var iweb = Map.headertableDxD(timeoutInSeconds, table, column);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_HeadertableDxDGetText_" + table + "_"+ textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public bool HeadertableDxDNameXGetText(int timeoutInSeconds, string table, string nameX, string textParam)
        {
            ScrollIntoView(Map.headertableDxDNameX(timeoutInSeconds, table, nameX));
            var iweb = Map.headertableDxDNameX(timeoutInSeconds, table, nameX);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_HeaderDxDNameX_" + table + "_" + nameX + "_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public string DatatableDxDGetText(int timeoutInSeconds, string table, string row, string column)
        {
            ScrollIntoView(Map.datatableDxD(timeoutInSeconds, table, row, column));
            return Map.datatableDxD(timeoutInSeconds, table, row, column).Text;
        }
        public bool DatatableDxDGetText(int timeoutInSeconds, string table, string row, string column, string textParam)
        {
            ScrollIntoView(Map.datatableDxD(timeoutInSeconds, table, row, column));
            var iweb = Map.datatableDxD(timeoutInSeconds, table, row, column);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DatatableDxD_" + table + "_" + textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public string DatatableDxDNameXGetText(int timeoutInSeconds, string table, string nameX, string column)
        {
            ScrollIntoView(Map.datatableDxDNameX(timeoutInSeconds, table, nameX, column));
            return Map.datatableDxDNameX(timeoutInSeconds, table, nameX, column).Text;
        }
        public bool DatatableDxDNameXGetText(int timeoutInSeconds, string table, string nameX, string column, string textParam)
        {
            ScrollIntoView(Map.datatableDxDNameX(timeoutInSeconds, table, nameX, column));
            var iweb = Map.datatableDxDNameX(timeoutInSeconds, table, nameX, column);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_DataDxDNameX_" + table + "_" + nameX + "_"+ textParam + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }

        // Actions
        public SearchFundAction InputNameToSearchFund(int timeoutInSeconds, string fundName)
        {
            Map.HighlightElement(Map.inputTxtSearchFund(timeoutInSeconds)).Clear();
            Map.HighlightElement(Map.inputTxtSearchFund(timeoutInSeconds)).SendKeys(fundName);
            return this;
        }
        public SearchFundAction ClickFundNameReturnOfResults(int timeout, string fundName, string sourceIcon)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.MoveToElement(this.Map.returnOfResultsFundNameWithItemSource(timeout, fundName, sourceIcon));
            actions.Perform();
            Map.HighlightElement(Map.returnOfResultsFundNameWithItemSource(timeout, fundName, sourceIcon)).Click();
            return this;
        }
        public SearchFundAction ClickFundNameReturnOfResults(int timeout, string fundName, string sourceIcon, string index)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.MoveToElement(this.Map.returnOfResultsFundNameWithItemSource(timeout, fundName, sourceIcon, index));
            actions.Perform();
            Map.HighlightElement(Map.returnOfResultsFundNameWithItemSource(timeout, fundName, sourceIcon, index)).Click();
            return this;
        }
        public SearchFundAction ClickFundNameReturnOfResults(int timeout, string fundName)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.MoveToElement(this.Map.returnOfResultsFundName(timeout, fundName));
            actions.Perform();
            Map.HighlightElement(Map.returnOfResultsFundName(timeout, fundName)).Click();
            return this;
        }
        public SearchFundAction ClickUserInputSubSection(int timeoutInSeconds, string sectionName)
        {
            Map.HighlightElement(Map.subSectionUserInput(timeoutInSeconds, sectionName)).Click();
            return this;
        }
        public SearchFundAction InputTxtLabelField(string label, string text)
        {
            Map.HighlightElement(Map.inputTxtLabel(label)).Clear();
            Map.HighlightElement(Map.inputTxtLabel(label)).SendKeys(text);
            return this;
        }
        public SearchFundAction InputTxtSearchLabelField(string label, string text)
        {
            Map.HighlightElement(Map.inputTxtSearchLabel(label)).Clear();
            Map.HighlightElement(Map.inputTxtSearchLabel(label)).SendKeys(text);
            return this;
        }
        public SearchFundAction ClickDatePickerTitleButton(int timeoutInSeconds, string title)
        {
            //this.Map.buttondatePickerTitle(timeoutInSeconds, title).Click(); --> Issue Element Click Intercepted Exception
            // Try with javascript if Element Click Intercepted Exception
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].click();", this.Map.buttondatePickerTitle(timeoutInSeconds, title));
            WaitForElementVisible(10, SearchFundPage.overlayDropdown);
            return this;
        }
        public SearchFundAction ClickDatePickerYearOnTopButton(int timeoutInSeconds)
        {
            Map.HighlightElement(Map.buttonOnTopDatePickerYear(timeoutInSeconds)).Click();
            return this;
        }
        public SearchFundAction ClickMonthOrYearInDatePicker(int timeoutInSeconds, string monthOrYear)
        {
            Map.HighlightElement(Map.buttonDatePickerMonthOrYear(timeoutInSeconds, monthOrYear)).Click();
            return this;
        }
        public SearchFundAction ClickDateInDatePicker(int timeoutInSeconds, string date)
        {
            Map.HighlightElement(Map.buttonDatePickerDate(timeoutInSeconds, date)).Click();
            return this;
        }
        public SearchFundAction ClickFieldCheckbox(string label)
        {
            Map.HighlightElement(Map.checkboxField(label)).Click();
            return this;
        }
        public SearchFundAction ClickLabelDropdown(int timeoutInSeconds, string label)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.MoveToElement(this.Map.dropdownLabel(timeoutInSeconds, label));
            actions.Perform();
            Map.HighlightElement(Map.dropdownLabel(timeoutInSeconds, label)).Click();
            return this;
        }
        public SearchFundAction SelectItemInDropdown(int timeoutInSeconds, string item)
        {
            ScrollIntoView(Map.selectItemDropdown(timeoutInSeconds, item));
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].click();", Map.selectItemDropdown(timeoutInSeconds, item));
            return this;
        }
        public SearchFundAction ClickCRBMAddButton(int timeoutInSeconds)
        {
            ScrollIntoView(Map.buttonAddCRBM(timeoutInSeconds));
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].click();", Map.buttonAddCRBM(timeoutInSeconds));
            return this;
        }
        public SearchFundAction ClickCRBMDeleteButton(int timeoutInSeconds, int rowNumber)
        {
            PageDownToScrollDownPage();
            ScrollIntoView(Map.buttonAddCRBM(timeoutInSeconds));
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].click();", Map.buttonDeleteCRBM(timeoutInSeconds, rowNumber));
            return this;
        }
        public SearchFundAction InputTxtNameCRBMRow(int timeoutInSeconds, int number, string text)
        {
            ScrollIntoView(this.Map.inputTxtNameCRBMRow(timeoutInSeconds, number));
            Map.HighlightElement(Map.inputTxtNameCRBMRow(timeoutInSeconds, number)).Clear();
            Map.HighlightElement(Map.inputTxtNameCRBMRow(timeoutInSeconds, number)).SendKeys(text);
            return this;
        }
        public SearchFundAction ClickNameCRBMReturnOfResults(int timeoutInSeconds, string benchmark)
        {
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].click();", Map.returnOfResultsNameCRBM(timeoutInSeconds, benchmark));
            return this;
        }
        public SearchFundAction InputNumberBetaCRBMRow(int timeoutInSeconds, int rowNumber, string text)
        {
            Map.HighlightElement(Map.inputNumberBetaCRBMRow(timeoutInSeconds, rowNumber)).Clear();
            Map.HighlightElement(Map.inputNumberBetaCRBMRow(timeoutInSeconds, rowNumber)).SendKeys(text);
            return this;
        }
        public SearchFundAction InputNumberGrossExposureCRBMRow(int timeoutInSeconds, int number, string text)
        {
            Map.HighlightElement(Map.inputNumberGrossExposureCRBMRow(timeoutInSeconds, number)).Clear();
            Map.HighlightElement(Map.inputNumberGrossExposureCRBMRow(timeoutInSeconds, number)).SendKeys(text);
            return this;
        }
        public SearchFundAction ClickAddButtonInXTable(int timeoutInSeconds, string attrValue)
        {
            ScrollIntoView(Map.buttonAddInXTable(timeoutInSeconds, attrValue));
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].click();", Map.buttonAddInXTable(timeoutInSeconds, attrValue));
            return this;
        }
        public SearchFundAction ClickDeleteButtonInXTable(int timeoutInSeconds, string attrValue, string rowNumber)
        {
            PageDownToScrollDownPage();
            ScrollIntoView(Map.buttonDeleteInXTable(timeoutInSeconds, attrValue, rowNumber));
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].click();", Map.buttonDeleteInXTable(timeoutInSeconds, attrValue, rowNumber));
            return this;
        }
        public SearchFundAction InputTxtNameBenchmarkXTableRow(int timeoutInSeconds, string attrValue, string number, string text)
        {
            ScrollIntoView(Map.inputTxtNameBenchmarkXTableRow(timeoutInSeconds, attrValue, number));
            Map.HighlightElement(Map.inputTxtNameBenchmarkXTableRow(timeoutInSeconds, attrValue, number)).Clear();
            Map.HighlightElement(Map.inputTxtNameBenchmarkXTableRow(timeoutInSeconds, attrValue, number)).SendKeys(text);
            return this;
        }
        public SearchFundAction InputTxtExposureBenchmarkXTableRow(int timeoutInSeconds, string attrValue, string number, string text)
        {
            ScrollIntoView(Map.inputTxtExposureBenchmarkXTableRow(timeoutInSeconds, attrValue, number));
            Map.HighlightElement(Map.inputTxtExposureBenchmarkXTableRow(timeoutInSeconds, attrValue, number)).Clear();
            Map.HighlightElement(Map.inputTxtExposureBenchmarkXTableRow(timeoutInSeconds, attrValue, number)).SendKeys(text);
            return this;
        }
        public SearchFundAction ClickRunButton(int timeoutInSeconds)
        {
            // Scroll to element
            //ScrollIntoView(this.Map.buttonRun(timeoutInSeconds));

            // MouseHover and Click
            Actions action = new Actions(Driver.Browser);
            action.MoveToElement(Map.buttonRun(timeoutInSeconds)).Click(Map.HighlightElement(Map.buttonRun(timeoutInSeconds))).Perform();
            return this;
        }
        public SearchFundAction ClickLabelButton(int timeoutInSeconds, string label)
        {
            // Scroll to element
            ScrollIntoView(this.Map.buttonLabel(timeoutInSeconds, label));

            // MouseHover and Click
            Actions action = new Actions(Driver.Browser);
            action.MoveToElement(Map.buttonRun(timeoutInSeconds)).Click(Map.HighlightElement(Map.buttonLabel(timeoutInSeconds, label))).Perform();
            return this;
        }
        public SearchFundAction ClickButtonInDialog(int timeoutInSeconds, string label)
        {
            Map.HighlightElement(Map.buttonInDialogPopup(timeoutInSeconds, label)).Click();
            return this;
        }
        public SearchFundAction UploadFileInput(string filepath)
        {
            this.Map.fileUpload.SendKeys(filepath);
            return this;
        }
        public SearchFundAction ClickCloseToastMessageButton(int timeoutInSeconds)
        {
            Map.HighlightElement(Map.buttonCloseToastMessage(timeoutInSeconds)).Click();
            return this;
        }
        public SearchFundAction ClickDestinationDropdown(int timeoutInSeconds, int row)
        {
            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(Driver.Browser);
            actions.MoveToElement(Map.dropdownDestination(timeoutInSeconds, row));
            actions.Click(Map.HighlightElement(Map.dropdownDestination(timeoutInSeconds, row))).Build().Perform();
            return this;
        }
        public SearchFundAction SelectItemInDestinationDropdown(int timeoutInSeconds, int row, string item)
        {
            ScrollIntoView(Map.selectItemDestinationDropdown(timeoutInSeconds, row, item));
            //this.Map.selectItemDestinationDropdown(timeoutInSeconds, row, item).Click(); // --> element click intercepted

            // Try with javascript
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].click();", Map.selectItemDestinationDropdown(timeoutInSeconds, row, item));
            return this;
        }
        public SearchFundAction InputAsOfDate(int timeoutInSeconds, string date)
        {
            Map.HighlightElement(Map.inputTxtAsOfDate).SendKeys(OpenQA.Selenium.Keys.Control + "a");
            Map.HighlightElement(Map.inputTxtAsOfDate).SendKeys(date);
            // this.Map.buttonAsOfDate(timeoutInSeconds).Click(); --> Element Click Intercepted Exception
            // Try with javascript if Element Click Intercepted Exception
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].click();", Map.buttonAsOfDate(timeoutInSeconds));
            WaitForElementInvisible(10, AddEditFundPage.overlayDropdown);
            return this;
        }
        public SearchFundAction PressDownArrowKeyUntilElementIsVisible(int timeoutInSeconds)
        {
            System.Windows.Forms.SendKeys.SendWait(@"{DOWN}");
            string? item = Driver.Browser.FindElement(By.XPath(@"//tbody[@class='ng-star-inserted']/tr[1]/td/p-dropdown//ul//li")).Text;
            while (item.Contains("No results found") && timeoutInSeconds > 0)
            {
                System.Windows.Forms.SendKeys.SendWait(@"{DOWN}"); System.Threading.Thread.Sleep(1000);
                var refreshItem = Driver.Browser.FindElement(By.XPath(@"//tbody[@class='ng-star-inserted']/tr[1]/td/p-dropdown//ul//li")).Text;
                if (!refreshItem.Contains("No results found")) break;
                timeoutInSeconds--;
            }
            return this;
        }
        public SearchFundAction ClickBackButton(int timeoutInSeconds)
        {
            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(Driver.Browser);
            actions.MoveToElement(Map.buttonBack(timeoutInSeconds));
            actions.Click(Map.HighlightElement(Map.buttonBack(timeoutInSeconds))).Build().Perform();
            return this;
        }
        #endregion

        #region Built-in Actions
        public SearchFundAction InputNameToSearchFund(int timeoutInSeconds, string searchInput, string managerName, string fundName, string sourceIcon)
        {
            InputNameToSearchFund(timeoutInSeconds, searchInput);
            WaitForElementVisible(timeoutInSeconds, SearchFundPage.fundNameReturnOfResults(managerName));
            ClickFundNameReturnOfResults(timeoutInSeconds, managerName, sourceIcon);
            WaitForElementVisible(timeoutInSeconds, SearchFundPage.fundNameReturnOfResultsWithItemSource(fundName, sourceIcon));
            ClickFundNameReturnOfResults(timeoutInSeconds, fundName, sourceIcon);
            //.WaitForLoadingIconToDisappear(timeoutInSeconds, SearchFundPage.loadingIcon);
            return this;
        }
        public SearchFundAction InputNameToSearchFund(int timeoutInSeconds, string searchInput, string managerName, string fundName, string sourceIcon, string index)
        {
            InputNameToSearchFund(timeoutInSeconds, searchInput);
            WaitForElementVisible(timeoutInSeconds, SearchFundPage.fundNameReturnOfResults(managerName));
            ClickFundNameReturnOfResults(timeoutInSeconds, managerName, sourceIcon);
            WaitForElementVisible(timeoutInSeconds, SearchFundPage.fundNameReturnOfResultsWithItemSourceIndex(fundName, sourceIcon, index));
            ClickFundNameReturnOfResults(timeoutInSeconds, fundName, sourceIcon, index);
            //.WaitForLoadingIconToDisappear(timeoutInSeconds, SearchFundPage.loadingIcon);
            return this;
        }

        public SearchFundAction ClickAndSelectItemInDropdown(int timeoutInSeconds, string label, string item)
        {
            if (item == "Custom")
            {
                ClickLabelDropdown(timeoutInSeconds, label);
                WaitForAllElementsVisible(10, SearchFundPage.overlayDropdown);
                WaitForAllElementsVisible(10, SearchFundPage.dropdownSelect(item));
                SelectItemInDropdown(timeoutInSeconds, item);
            }
            else
            {
                ScrollIntoView(this.Map.dropdownLabel(timeoutInSeconds, label));
                ClickLabelDropdown(timeoutInSeconds, label);
                WaitForAllElementsVisible(10, SearchFundPage.overlayDropdown);
                WaitForAllElementsVisible(10, SearchFundPage.dropdownSelect(item)); Thread.Sleep(500);
                SelectItemInDropdown(timeoutInSeconds, item);
                WaitForElementInvisible(10, SearchFundPage.overlayDropdown); Thread.Sleep(500);
            }
            return this;
        }

        public SearchFundAction ClickAndSelectItemInDestinationDropdown(int timeoutInSeconds, int row, string item)
        {
            ScrollIntoView(this.Map.dropdownDestination(timeoutInSeconds, row));
            ClickDestinationDropdown(timeoutInSeconds, row); Thread.Sleep(250);
            WaitForAllElementsVisible(timeoutInSeconds, SearchFundPage.overlayDropdown); Thread.Sleep(250);   
            SelectItemInDestinationDropdown(timeoutInSeconds, row, item);
            //PageDownToScrollDownPage(); Thread.Sleep(250);
            WaitForElementInvisible(timeoutInSeconds, SearchFundPage.overlayDropdown);
            PageDownToScrollDownPage(); Thread.Sleep(250);
            return this;
        }

        public SearchFundAction CheckIfExistingCRBMThenDeleteAll(int timeoutInSeconds, string dataSource)
        {
            System.Threading.Thread.Sleep(1000);
            ReadOnlyCollection<IWebElement>? crbm = Driver.Browser.FindElements(By.XPath("//tbody[contains(@class,'datatable')]/tr"));
            if (crbm.Count == 1 && dataSource == "C")
            {
                ClickCRBMAddButton(timeoutInSeconds);
                System.Threading.Thread.Sleep(500);
                ClickCRBMDeleteButton(timeoutInSeconds, 1);
                System.Threading.Thread.Sleep(500);
            }
            if (crbm.Count > 1 && dataSource == "C")
            {
                ClickCRBMAddButton(timeoutInSeconds);
                System.Threading.Thread.Sleep(500);
                int getCRBM = Driver.Browser.FindElements(By.XPath("//tbody[contains(@class,'datatable')]/tr")).Count;
                while (getCRBM > 1)
                {
                    ClickCRBMDeleteButton(timeoutInSeconds, 1);
                    System.Threading.Thread.Sleep(500);
                    getCRBM--;
                }
            }
            if (crbm.Count >= 1 && (dataSource == "A" || dataSource == "S" || dataSource == "M"))
            {
                int getCRBM = Driver.Browser.FindElements(By.XPath("//tbody[contains(@class,'datatable')]/tr")).Count;
                while (getCRBM >= 1)
                {
                    ClickCRBMDeleteButton(timeoutInSeconds, 1);
                    System.Threading.Thread.Sleep(500);
                    getCRBM--;

                    if (getCRBM == 0) { break; }
                }
            }
            return this;
        }

        public SearchFundAction InputTxtDatePickerTitle(int timeoutInSeconds, string title, string yyyy, string mmm, string dd)
        {
            // Click on Date Picker button
            ClickDatePickerTitleButton(timeoutInSeconds, title);
            WaitForElementVisible(10, SearchFundPage.overlayDropdown);

            // Click Year button in Date-Picker (to select Year, month, date)
            WaitForElementVisible(timeoutInSeconds, SearchFundPage.datePickerYearOnTopButton);
            ClickDatePickerYearOnTopButton(timeoutInSeconds);
            ClickMonthOrYearInDatePicker(timeoutInSeconds, yyyy);
            ClickMonthOrYearInDatePicker(timeoutInSeconds, mmm);
            ClickDateInDatePicker(timeoutInSeconds, dd);
            WaitForElementInvisible(10, SearchFundPage.overlayDropdown);
            return this;
        }

        public SearchFundAction ClickToCheckTheCheckbox(string label)
        {
            bool isChecked = false;
            isChecked = Driver.Browser.FindElement(SearchFundPage.fieldCheckbox(label)).GetAttribute("class").Contains("active");
            if (isChecked)
            {
                return this;
            }
            else ClickFieldCheckbox(label);
            return this;
        }
        public SearchFundAction ClickToUnCheckTheCheckbox(string label)
        {
            bool isChecked = false;
            isChecked = Driver.Browser.FindElement(SearchFundPage.fieldCheckbox(label)).GetAttribute("class").Contains("active");
            if (isChecked)
            {
                ClickFieldCheckbox(label);
            }
            return this;
        }

        public bool CompareObjects(IOrderedEnumerable<JObject> list1, IOrderedEnumerable<JObject> list2)
        {
            bool result = false;
            foreach (var item1 in list1)
            {
                foreach (var item2 in list2)
                {
                    if (JToken.DeepEquals(item1, item2) != true)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        public bool CompareJObjectsToString(IOrderedEnumerable<JObject> list1, IOrderedEnumerable<JObject> list2)
        {
            bool result;
            var jobject1 = string.Join(",", list1);
            var jobject2 = string.Join(",", list2);

            if (jobject1.Equals(jobject2))
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
        #endregion
    }
}
