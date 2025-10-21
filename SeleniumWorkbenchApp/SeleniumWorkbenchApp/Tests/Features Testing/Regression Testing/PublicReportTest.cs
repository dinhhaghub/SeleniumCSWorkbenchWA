using AventStack.ExtentReports;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using NUnit.Framework;
using SeleniumGendKS.Core.FilesComparision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using WorkbenchApp.UITest.Core.BaseTestCase;
using WorkbenchApp.UITest.Core.Selenium;
using WorkbenchApp.UITest.Generals;
using WorkbenchApp.UITest.Pages;
using static System.Windows.Forms.LinkLabel;

namespace WorkbenchApp.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(6)]
    internal class PublicReportTest : BaseTestCase
    {
        #region Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;

        /// Custom Risk Benchmark
        private string? crbm_ProjBeta;
        private string? crbm_ProjExposure;

        /// Auto Score
        private string? managerInception_Autoscore;
        private string? managerInception_IR;
        private string? managerInception_LengthInMonths;

        /// Manager Alpha
        private string? firstYear_NetReturn;
        private string? firstYear_GrossReturn;
        private string? firstYear_CRBM;
        private string? firstYear_GrossTotalAlpha;
        private string? firstYear_GrossStrategyAlpha;
        private string? firstYear_GrossManagerAlpha;
        private string? firstYear_Fees;
        private string? firstYear_NetTotalAlpha;
        private string? latestYear;

        /// Annualized Return
        private string? annualReturnSection_AnnManagerSinceInception_NetManagerReturn;
        private string? annualReturnSection_AnnManagerSinceInception_crbm1;
        private string? annualReturnSection_AnnManagerSinceInception_crbm2;
        private string? annualReturnSection_AnnManagerSinceInception_CustomRiskBenchmark;
        private string? annualRiskSection_10YearSR_NetManagerReturn;
        private string? annualRiskSection_10YearSR_crbm1;
        private string? annualRiskSection_10YearSR_crbm2;
        private string? annualRiskSection_10YearSR_CustomRiskBenchmark;
        private string? drawdown_NumberDownMonths_NetManagerReturn;
        private string? drawdown_NumberDownMonths_crbm1;
        private string? drawdown_NumberDownMonths_crbm2;
        private string? drawdown_NumberDownMonths_CustomRiskBenchmark;
        private string? annualizedCorrelation_CorrelationSinceInception_NetManagerReturn;
        private string? annualizedCorrelation_CorrelationSinceInception_crbm1;
        private string? annualizedCorrelation_CorrelationSinceInception_crbm2;
        private string? annualizedCorrelation_CorrelationSinceInception_CustomRiskBenchmark;

        /// Metrics Over Selected Time Frames
        private string? last3Years_StartDate;
        private string? last5Years_StartDate;
        private string? kSInception_StartDate;
        private string? iTD;
        private string? iTD_Risk_NetManagerReturn;
        private string? iTD_Risk_GrossManagerReturn;
        private string? iTD_Risk_CustomRiskBenchmark;
        private string? iTD_Risk_RFRate;
        private string? iTD_Risk_GrossTotalAlpha;
        private string? iTD_Risk_GrossStrategyAlpha;
        private string? iTD_Risk_GrossManagerAlpha;
        private string? iTD_Risk_Fees;
        private string? iTD_Risk_NetAlpha;
        

        /// Net Monthly Return
        private string? firstYear_Jan;
        private string? firstYear_Feb;
        private string? firstYear_Mar;
        private string? firstYear_Apr;
        private string? firstYear_May;
        private string? firstYear_Jun;
        private string? firstYear_Jul;
        private string? firstYear_Aug;
        private string? firstYear_Sep;
        private string? firstYearOct;
        private string? firstYearNov;
        private string? firstYearDec;
        #endregion

        [Test, Category("Regression Testing")]
        public void TC001_SinglePublicReport_Albourne_NoInput()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            const string managerName = "Citadel Advisors LLC";
            const string fundName = "Citadel Multi Strategy Funds";
            const string sourceIcon = "A";
            string userProfileDownloadPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
            string fileName = @"download.pdf";
            string? fileNameBaseline = null;
            string videoFileName = "PublicReportTestTC001";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Public Report Test - TC001");
            try
            {
                // Start recording video
                Driver.StartVideoRecord(videoFileName);

                // Delete downloaded PDF file
                GeneralAction.Instance.DeleteFilePath(userProfileDownloadPath, fileName);

                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Search a Fund - Source = Albourne
                SearchFundAction.Instance.InputNameToSearchFund(10, "cita", managerName, fundName, sourceIcon);

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable); Thread.Sleep(1000);

                // Check if the spinner loading icon is shown then wait for it load done
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(30, General.loadingSpinner);
                }

                // Click 'Model' menu
                NavigationAction.Instance.ClickPageNames(10, SearchFundPage.model);
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.labelButton(SearchFundPage.userInput));

                // Click 'User Input' button
                GeneralAction.Instance.ClickButtonLabel(10, SearchFundPage.userInput)
                                      .WaitForElementVisible(10, SearchFundPage.userInputPanel);

                // Check if A red notification is shown then waiting for that is disappeared
                System.Threading.Thread.Sleep(500);
                if (SearchFundAction.Instance.IsElementPresent(SearchFundPage.MessageFundDoesNotHaveReturnData))
                {
                    SearchFundAction.Instance.WaitForElementInvisible(10, SearchFundPage.MessageFundDoesNotHaveReturnData);
                }

                // Click to expand User Input (section)
                SearchFundAction.Instance.ClickUserInputSubSection(10, SearchFundPage.dateSection)
                                         .ClickUserInputSubSection(10, SearchFundPage.customRiskBenchmarkModelling)
                                         .ClickUserInputSubSection(10, SearchFundPage.feeModelSection)
                                         .ClickUserInputSubSection(10, SearchFundPage.liquiditySection)
                                         .PageDownToScrollDownPage();

                // Check if existing CRBM then delete All
                SearchFundAction.Instance.CheckIfExistingCRBMThenDeleteAll(10, sourceIcon);

                // Liquidity Section
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.lockup, "None")
                                         .InputTxtLabelField(SearchFundPage.lockupLengthMonths, "12")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.liquidityFrequency, "Quarterly")
                                         .InputTxtLabelField(SearchFundPage.investorGate, "")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.sidepocketProbability, "50%")
                                         .InputTxtLabelField(SearchFundPage.MaxPercOfSidepocketPermitted, "50"); Thread.Sleep(250);
                SearchFundAction.Instance.WaitForElementInvisible(10, SearchFundPage.errorInvalidMessageContent("All the liquidity parameters need to be populated except for Investor Gate"));

                // Click on 'Run' button
                SearchFundAction.Instance.ClickRunButton(10)
                                         .WaitForLoadingIconToDisappear(30, General.loadingSpinner);

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab")) 
                {
                    fileNameBaseline = "Citadel_Sandbox_download.pdf";

                    // Custom Risk Benchmark - data

                    // Auto Score - data
                    managerInception_IR = "1.86";
                    managerInception_LengthInMonths = "338"; // old: 336

                    // Annualized Return - data
                    annualRiskSection_10YearSR_NetManagerReturn = "2.49"; // old: 2.50

                    // Metrics Over Selected Time Frames - data
                    iTD = "07/01/1995"; // base on KS-603 (old: remove --> 11/01/1990)
                    last3Years_StartDate = "Last 3 Years"; // old: 09/01/2020
                    last5Years_StartDate = "Last 5 Years"; // old: 09/01/2018
                    iTD_Risk_GrossTotalAlpha = "10.3%";
                }
                if (urlInstance.Contains("conceptia")) 
                {
                    fileNameBaseline = "Citadel_Conceptia_download.pdf";

                    // Custom Risk Benchmark - data

                    // Auto Score - data
                    managerInception_IR = "1.86";
                    managerInception_LengthInMonths = "338";

                    // Annualized Return - data
                    annualRiskSection_10YearSR_NetManagerReturn = "2.49";

                    // Metrics Over Selected Time Frames - data
                    iTD = "07/01/1995";
                    last3Years_StartDate = "Last 3 Years"; // Last 3 Years : 09/01/2020 -
                    last5Years_StartDate = "Last 5 Years"; // Last 5 Years : 09/01/2018 -
                    iTD_Risk_GrossTotalAlpha = "10.3%";
                }

                #region Verify headers in tables of Report
                string table = "Custom Risk Benchmark";
                verifyPoint = SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 1, "Index Name")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 2, "Projected Beta")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 3, "Projected Exposure");
                verifyPoints.Add(summaryTC = "Verify headers in the 1st table ("+ fundName + "): '" + table + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                table = "Auto Score";
                verifyPoint = SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 1, "")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 2, "Autoscore")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 3, "IR")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 4, "Length In Months");
                verifyPoints.Add(summaryTC = "Verify headers in the 2nd table: '" + table + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                table = "Manager Alpha";
                verifyPoint = SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 1, "Year")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 2, "Net Return")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 3, "Gross Return")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 4, "CRBM")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 5, "Gross Total Alpha")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 6, "Gross Strategy Alpha")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 7, "Gross Manager Alpha")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 8, "Fees")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 9, "Liquidity Cost")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 10, "Net Total Alpha");
                verifyPoints.Add(summaryTC = "Verify headers in the 3rd table: '" + table + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                table = "Annualized Return";
                verifyPoint = SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 1, "")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 2, "")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 3, "Net Manager Return")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 4, "Custom Risk Benchmark");
                verifyPoints.Add(summaryTC = "Verify headers in the 4th table: '" + table + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                table = "Metrics Over Selected Time Frames";
                verifyPoint = SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 1, "")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 2, "Net Manager Return")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 3, "Gross Manager Return")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 4, "Custom Risk Benchmark")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 5, "RF Rate")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 6, "Gross Total Alpha")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 7, "Gross Strategy Alpha")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 8, "Gross Manager Alpha")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 9, "Fees")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 10, "Liquidity Cost")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 11, "Net Alpha");
                verifyPoints.Add(summaryTC = "Verify headers in the 5th table: '" + table + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                table = "Net Monthly Return";
                verifyPoint = SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 1, "CY");
                verifyPoints.Add(summaryTC = "Verify headers in the 6th table: '" + table + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify data in tables of Report
                table = "Custom Risk Benchmark";
                string data = "CASH";
                verifyPoint = data == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Index Name): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_IndexName_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "1") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (CASH, Projected Beta): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_CASHProjectedBeta_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "100.0%") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (CASH, Projected Exposure): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_CASHProjectedExposure_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Auto Score";
                verifyPoint = "Manager Inception" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "10 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && "5 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2")) > 0.00
                    && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2")) < 20.00;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Manager Inception, Autoscore): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_ManagerInceptionAutoscore_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3")) < 20.00;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Manager Inception, IR): " + managerInception_IR + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_ManagerInceptionIR_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToInt32(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4")) > 300
                    && Convert.ToInt32(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4")) < 400;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Manager Inception, Length in months): " + managerInception_LengthInMonths + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_ManagerInceptionLenInMth_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Manager Alpha";
                verifyPoint = "1995" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "2025" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (Year)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_Year_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2").Replace("%", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (1995, Net Return): " + (data= "> 0.00 and <100.00") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995NetRetr_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3").Replace("%", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (1995, Gross Return): " + (data = "> 0.00 and <100.00") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995GrossRetr_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4").Replace("%", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (1995, CRBM): " + (data = "> 0.00 and <100.00") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995CRBM_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "5").Replace("%", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "5").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (1995, Gross Total Alpha): " + (data = "> 0.00 and <100.00") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995GrossTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "6").Replace("%", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "6").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (1995, Gross Strategy Alpha): " + (data = "> 0.00 and <100.00") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995GrossStratAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "7").Replace("%", "")) >0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "7").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (1995, Gross Manager Alpha): " + (data = "> 0.00 and <100.00") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995GrossManagerAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "8").Replace("%", "").Replace("(", "").Replace(")", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "8").Replace("%", "").Replace("(", "").Replace(")", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (1995, Fees): " + (data = "> 0.00 and <100.00") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995Fees_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "9").Replace("%", "").Replace("(", "").Replace(")", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "9").Replace("%", "").Replace("(", "").Replace(")", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (1995, Liquidity Cost): " + (data = "> 0.00 and <100.00") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995Liquidity Cost_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "10").Replace("%", "").Replace("(", "").Replace(")", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "10").Replace("%", "").Replace("(", "").Replace(")", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (1995, Net Total Alpha): " + (data = "> 0.00 and <100.00") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995NetTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Annualized Return";
                verifyPoint = "Annual Return Section" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "Ann. Manager Since Inception" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2")
                    && "Ann Ret Custom Start Date 2" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && "Last 10 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1")
                    && "Last 5 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "4", "1")
                    && "Last 3 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "5", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_AnnualReturnSectionCol1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3").Replace("%", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, Ann. Manager Since Inception, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_ARetrnSec_AnnManagerSinceIncepNetMRetrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4").Replace("%", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, Ann. Manager Since Inception, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_ARetrnSec_AnnManagerSinceIncepCusRskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "Annual Risk Section" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "1")
                    && "10 Year SR" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "2")
                    && "5 Year SR" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "7", "1")
                    && "3 Year SR" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "8", "1")
                    && "SR of Custom Period" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "9", "1")
                    && "Standard Deviation Custom Period 1" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "10", "1")
                    && "IR over Custom Period 1" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "11", "1")
                    && "Tracking Error over Custom Period 1" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "12", "1")
                    && "Beta" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "13", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_ARisSec_dataColumn1)_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "3")) < 200.01
                    && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "3").Replace("(", "").Replace(")", "")) < 200.00;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, 10 Year SR, Net Manager Return): " + annualRiskSection_10YearSR_NetManagerReturn + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_ARisSec_10YSR_NetMRetrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "4")
                    || Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "4").Replace("(", "").Replace(")", "")) < 200.00;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, 10 Year SR, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_ARisSec_10YSRC_CusRskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "Annualized Correlation" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "1")
                    && "Correlation Since Inception" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "2")
                    && "One Year Rolling Correlation (Mean)" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "32", "1")
                    && "Two Year Rolling Correlation (Mean)" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "33", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_ACorrelation_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, Correlation Since Inception, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_ACorrelation_CorrelSinceIncep_NetMRetrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "4").Replace("%", "").Replace("(", "").Replace(")", "")) > 0.00
                    && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "4").Replace("%", "").Replace("(", "").Replace(")", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, Correlation Since Inception, Custom Risk Benchmark): " + (data=">0.00 and < 100.00") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_ACorrelation_CorrelSinceIncep_CusRskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "1")
                    && "Number Down Months" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "2")
                    && "Percentage Down Months" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "15", "1")
                    && "Average Down Months" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "16", "1")
                    && "Max Gain" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "17", "1")
                    && "Second Largest" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "18", "1")
                    && "Third Largest" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "19", "1")
                    && "Largest Loss" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "20", "1")
                    && "Second Largest Loss" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "21", "1")
                    && "Third Largest Loss" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "22", "1")
                    && "Max Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "23", "1")
                    && "Months in Max Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "24", "1")
                    && "Months to Recover" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "25", "1")
                    && "Peak before Max Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "26", "1")
                    && "Valley" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "27", "1")
                    && "2nd Worst" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "28", "1")
                    && "3rd Worst" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "29", "1")
                    && "4th Worst" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "30", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_Drawdown_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "3").Replace("%", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "3").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, Number Down Months, Net Manager Return): " + (data = ">0.00 and < 100.00") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_Drawdown_NumberDowMths_NetManRturn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "4")
                    || double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "4").Replace("%", "").Replace("(", "").Replace(")", "")) < 200.00;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, Number Down Months, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_Drawdown_NumberDowMths_CusRiskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Metrics Over Selected Time Frames";
                verifyPoint = SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1").Contains(data = "ITD : " + iTD)
                    && "Risk" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && "Sharpe Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "4", "1")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "5", "1")
                    && SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "1").Contains(last3Years_StartDate)
                    && "Risk" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "7", "1")
                    && "Sharpe Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "8", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "9", "1")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "10", "1")
                    && SearchFundAction.Instance.DataTableOfReportGetText(10, table, "11", "1").Contains(last5Years_StartDate)
                    && "Risk" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "12", "1")
                    && "Sharpe Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "13", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "1")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "15", "1")
                    && SearchFundAction.Instance.DataTableOfReportGetText(10, table, "16", "1").Contains("Projected Returns")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "17", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "18", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (data column1): " + last3Years_StartDate + ", " + last5Years_StartDate + ",", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "2").Replace("%", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "2").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/1995), Net Manager Return): " + (data = @">0.00 and <100.00") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_Risk_ITD07011995_NetManRetrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "3").Replace("%", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "3").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/1995), Gross Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_Risk_ITD07011995_GrosManRetrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "4").Replace("%", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "4").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/1995), Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_Risk_ITD07011995_CusRisBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "5").Replace("%", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "5").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/1995), RF Rate): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_Risk_ITD07011995_RFRate_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "6").Replace("%", "")) > 5.09
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "6").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/1995), Gross Total Alpha): " + iTD_Risk_GrossTotalAlpha + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_Risk_ITD07011995_GrossTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "7");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/1995), Gross Strategy Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_Risk_ITD07011995_GrossStratAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "8");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/1995), Gross Manager Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_Risk_ITD07011995_GrossManAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "9");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/1995), Fees): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_Risk_ITD07011995_Fees_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "10");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/1995), Liquidity Cost): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_Risk_ITD07011995_LiquidCost_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "11").Replace("%", "")) > 0.00
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "11").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/1995), Net Alpha): " + (data = @">0.00 and <100.00") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_Risk_ITD07011995_NetAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Net Monthly Return";
                verifyPoint = "1995" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "2025" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (CY): " + "Years" + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_CY_Years_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }
                
                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (1995, Jan): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995_Jan_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "8").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (1995, Jul): " + (data = @"<100.00") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995_Jul_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "9").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (1995, Aug): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995_Aug_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "10").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (1995, Sep): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995_Sep_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "11").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (1995, Oct): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995_Oct_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "12").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (1995, Nov): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995_Nov_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "13").Replace("%", "")) < 100.00;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (1995, Dec): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC001_" + table + "_1995_Dec_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }
                #endregion

                #region Verify Charts
                // Drawdown Graph (Chart)
                verifyPoint = SearchFundAction.Instance.IsDrawdownGraphPublicReportShown(10);
                verifyPoints.Add(summaryTC = "Verify the chart - Drawdown Graph is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Rolling Correlation Graph (Chart)
                verifyPoint = SearchFundAction.Instance.IsRollingCorrelationGraphPublicReportShown(10);
                verifyPoints.Add(summaryTC = "Verify the chart - Rolling Correlation Graph is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // fund Aum Graph (Chart)
                verifyPoint = SearchFundAction.Instance.IsFundAumGraphPublicReportShown(10);
                verifyPoints.Add(summaryTC = "Verify the chart - Fund Aum Graph is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion
                
                #region Save PDF file
                // Click on 'Print' button
                GeneralAction.Instance.ClickButtonLabel(10, SearchFundPage.print);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());
                System.Threading.Thread.Sleep(3500);

                // press Tab keyboard to click on 'Destination' dropdown
                System.Windows.Forms.SendKeys.SendWait(@"{TAB}"); System.Threading.Thread.Sleep(500);
                System.Windows.Forms.SendKeys.SendWait(@"{TAB}"); System.Threading.Thread.Sleep(500);
                System.Windows.Forms.SendKeys.SendWait(@"{TAB}"); System.Threading.Thread.Sleep(500);
                System.Windows.Forms.SendKeys.SendWait(@"{TAB}"); System.Threading.Thread.Sleep(500);
                System.Windows.Forms.SendKeys.SendWait(@"{TAB}"); System.Threading.Thread.Sleep(500);

                // Select 'Save as PDF'
                System.Windows.Forms.SendKeys.SendWait(@"{ENTER}"); System.Threading.Thread.Sleep(500);
                System.Windows.Forms.SendKeys.SendWait(@"{DOWN}"); System.Threading.Thread.Sleep(500);
                System.Windows.Forms.SendKeys.SendWait(@"{ENTER}"); System.Threading.Thread.Sleep(500);

                // Click 'Save' button
                System.Windows.Forms.SendKeys.SendWait(@"{TAB}"); System.Threading.Thread.Sleep(500);
                System.Windows.Forms.SendKeys.SendWait(@"{TAB}"); System.Threading.Thread.Sleep(500);
                System.Windows.Forms.SendKeys.SendWait(@"{TAB}"); System.Threading.Thread.Sleep(500);
                System.Windows.Forms.SendKeys.SendWait(@"{TAB}"); System.Threading.Thread.Sleep(500);
                System.Windows.Forms.SendKeys.SendWait(@"{TAB}"); System.Threading.Thread.Sleep(500);
                System.Windows.Forms.SendKeys.SendWait(@"{TAB}"); System.Threading.Thread.Sleep(500);
                System.Windows.Forms.SendKeys.SendWait(@"{ENTER}"); System.Threading.Thread.Sleep(1000);
                System.Windows.Forms.SendKeys.SendWait(@"{ENTER}"); System.Threading.Thread.Sleep(500);
                #endregion

                #region Verify PDF file
                // Check PDF file downloaded (timeout = 9s)
                verifyPoint = GeneralAction.Instance.CheckFileDownloadIsComplete(9, userProfileDownloadPath, fileName);
                verifyPoints.Add(summaryTC = "Verify the '" + fileName + "' file is downloaded successful", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                //// Verify PDF file by comparing files              
                //verifyPoint = FilesComparision.PDFIsFilesEqual(fileNameBaseline, fileName);
                //verifyPoints.Add(summaryTC = "Verify data of the '" + fileName + "' file is shown correctly", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                // Delete downloaded PDF file
                GeneralAction.Instance.DeleteFilePath(userProfileDownloadPath, fileName);
                #endregion

                // Stop recording video
                Driver.StopVideoRecord();

                // Delete video file
                Driver.DeleteFilesContainsName(System.IO.Path.GetFullPath(@"../../../../../TestResults/"), videoFileName);
            }
            catch (Exception exception)
            {
                // Stop recording video
                Driver.StopVideoRecord();

                // Print exception
                System.Console.WriteLine(exception);

                // Delete downloaded PDF file
                GeneralAction.Instance.DeleteFilePath(Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads", @"download.pdf");

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong! Please check console log.");
            }
            #endregion
        }

        [Test, Category("Regression Testing")]
        public void TC002_SinglePublicReport_AlbourneManual_Input()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            const string managerName = "Prelude Capital";
            const string fundName = "Prelude Structured Alternatives Fund";
            const string sourceIcon = "M";
            const string messageCRBM = "This Benchmark does not cover the time range of the report. Select another one.";
            string videoFileName = "PublicReportTestTC002";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Public Report Test - TC002");
            try
            {
                // Start recording video
                Driver.StartVideoRecord(videoFileName);

                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Search a Fund - Source = Albourne
                SearchFundAction.Instance.InputNameToSearchFund(10, "Prelude", managerName, fundName, sourceIcon);

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable); Thread.Sleep(1000);

                // Check if the spinner loading icon is shown then wait for it load done
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(30, General.loadingSpinner);
                }

                // Click 'Model' menu
                NavigationAction.Instance.ClickPageNames(10, SearchFundPage.model); // test update
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.labelButton(SearchFundPage.userInput));

                // Click 'User Input' button
                GeneralAction.Instance.ClickButtonLabel(10, SearchFundPage.userInput);

                // check if dialog popup 'Model Parameters' is still not shown then click until it appears
                int time = 0;
                while (GeneralAction.Instance.IsElementPresent(SearchFundPage.userInputPanel) == false && time < 10)
                {
                    // Click 'User Input' button
                    GeneralAction.Instance.ClickButtonLabel(10, SearchFundPage.userInput);

                    // if dialog popup 'Model Parameters' is shown then exit loop
                    if (GeneralAction.Instance.IsElementPresent(SearchFundPage.userInputPanel) == true) { break; }

                    time++;
                    Thread.Sleep(1000);
                }

                // Check if A red notification is shown then waiting for that is disappeared
                System.Threading.Thread.Sleep(500);
                string msg = "All the liquidity parameters need to be populated except for Investor Gate";
                if (SearchFundAction.Instance.IsElementPresent(SearchFundPage.errorInvalidMessageContent(msg)))
                {
                    SearchFundAction.Instance.WaitForElementInvisible(10, SearchFundPage.errorInvalidMessageContent(msg));
                }

                // Click to expand User Input (section)
                SearchFundAction.Instance.ClickUserInputSubSection(10, SearchFundPage.dateSection)
                                         .ClickUserInputSubSection(10, SearchFundPage.customRiskBenchmarkModelling)
                                         .ClickUserInputSubSection(10, SearchFundPage.feeModelSection)
                                         .ClickUserInputSubSection(10, SearchFundPage.liquiditySection)
                                         .PageDownToScrollDownPage();

                #region Input Data for (User Input)
                SearchFundAction.Instance.InputTxtLabelField(SearchFundPage.trackingError, "11")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.reportEndDate, "Latest Available")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.customStartDate1, "Last 3 Years")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.customStartDate2, "Last 5 Years")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.customEndDate1, "Latest Available")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.customEndDate2, "Latest Available");

                // Custom Risk Benchmark Modelling
                /// Check if existing CRBM then delete All
                SearchFundAction.Instance.CheckIfExistingCRBMThenDeleteAll(10, sourceIcon); 
                
                // Add the 1st CRBM
                SearchFundAction.Instance.ClickCRBMAddButton(10)
                                         .InputTxtNameCRBMRow(10, 1, "s&p")
                                         .WaitForElementVisible(20, SearchFundPage.nameCRBMReturnOfResults("S&P 500 Index"))
                                         .ClickNameCRBMReturnOfResults(10, "S&P 500 Index")
                                         .InputNumberBetaCRBMRow(10, 1, "-1.5")
                                         .InputNumberGrossExposureCRBMRow(10, 1, "2.5");

                /*// Add the 2nd CRBM
                SearchFundAction.Instance.ClickCRBMAddButton(10)
                                         .InputTxtNameCRBMRow(10, 2, "S&P US")
                                         .WaitForElementVisible(20, SearchFundPage.nameCRBMReturnOfResults("S&P US High Yield Corporate Bond TR Index"))
                                         .ClickNameCRBMReturnOfResults(10, "S&P US High Yield Corporate Bond TR Index");

                // Verify the error message in red toast is shown
                /// KS-238 The Start Date of the CRBM must be earlier or equal to the start date of the fund (fund return)
                verifyPoint = messageCRBM == SearchFundAction.Instance.ErrorMessageNameCRBMGetTextRow(10, 2);
                verifyPoints.Add(summaryTC = "Verify the error message in red toast is shown: " + messageCRBM + " ", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click delete icon of the 2nd CRBM
                SearchFundAction.Instance.ClickCRBMDeleteButton(10, 2)
                                       .WaitForElementInvisible(10, SearchFundPage.nameCRBMInputTxtRow(2)); */

                // Add the 2nd CRBM (another valid)
                SearchFundAction.Instance.ClickCRBMAddButton(10)
                                         .InputTxtNameCRBMRow(10, 2, "msci")
                                         .WaitForElementVisible(20, SearchFundPage.nameCRBMReturnOfResults("MSCI Europe Growth Net TR Index (LCL)"))
                                         .ClickNameCRBMReturnOfResults(10, "MSCI Europe Growth Net TR Index (LCL)")
                                         .InputNumberBetaCRBMRow(10, 2, "3.5")
                                         .InputNumberGrossExposureCRBMRow(10, 2, "-4.5");

                // Fee Model Section
                SearchFundAction.Instance.InputTxtLabelField(SearchFundPage.managementFee, "5")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.managementFeePaid, "Quarterly")
                                         .InputTxtLabelField(SearchFundPage.performanceFee, "21")
                                         .ClickToCheckTheCheckbox(SearchFundPage.highWaterMark)
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.catchUp, "Yes")
                                         .InputTxtLabelField(SearchFundPage.catchUpPercAgeIfSoft, "11") // Only enable when "Catch Up" = Yes
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.crystallizationEveryXYears, "10")

                                         /// Hurdle Status
                                         .ClickToCheckTheCheckbox(SearchFundPage.hurdleStatus)
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.hurdleFixedOrRelative, "Relative") // Only enable when Hurdle Status is checked (checkbox)
                                                                                                                             //InputTxtLabelField("Hurdle Rate (%)", "12"); // Only enable when Hurdle Fixed or Relative = Fixed
                                         .InputTxtLabelField("Fixed Percentage on Top of Index Return (%)", "40")
                                         //.InputTxtSearchLabelField(SearchFundPage.hurdleBenchmark, "msci") // (new: KS-834/838) (old: Only enable when Hurdle Fixed or Relative = Relative)
                                         .ClickAddButtonInXTable(10, "hurdle-section")
                                         .InputTxtNameBenchmarkXTableRow(10, "hurdle-section", "1", "msci")
                                         .WaitForElementVisible(20, SearchFundPage.nameCRBMReturnOfResults("MSCI Emerging Markets Latin America ex Brazil Net TR Index (USD)"))
                                         .ClickNameCRBMReturnOfResults(10, "MSCI Emerging Markets Latin America ex Brazil Net TR Index (USD)")
                                         .InputTxtExposureBenchmarkXTableRow(10, "hurdle-section", "1", "55.7")
                                         .ClickAndSelectItemInDropdown(10, "Hurdle Type", "Soft Hurdle")
                                         .ClickAndSelectItemInDropdown(10, "Ramp Type", "Performance Dependent");

                // Liquidity Section
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.lockup, "Hard")
                                         .InputTxtLabelField(SearchFundPage.lockupLengthMonths, "6")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.liquidityFrequency, "Monthly")
                                         .InputTxtLabelField(SearchFundPage.investorGate, "20.2")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.sidepocketProbability, "100%")
                                         .InputTxtLabelField(SearchFundPage.MaxPercOfSidepocketPermitted, "30.3"); Thread.Sleep(250);
                SearchFundAction.Instance.WaitForElementInvisible(10, SearchFundPage.errorInvalidMessageContent("All the liquidity parameters need to be populated except for Investor Gate"));
                #endregion

                // Click on 'Run' button
                SearchFundAction.Instance.ClickRunButton(10)
                                         .WaitForLoadingIconToDisappear(30, General.loadingSpinner);

                #region Verify data in tables of Report
                string table = "Custom Risk Benchmark";
                string data = "S&P 500 Index";
                verifyPoint = data == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Index Name, 1st crbm): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_IndexName_1stCrbm_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }
                
                verifyPoint = (data = "-1.5") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (S&P 500 Index, Projected Beta): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_SP500Index_ProjeBeta_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "2.5%") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (S&P 500 Index, Projected Exposure): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_SP500Index_ProjeEpos_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "MSCI Europe Growth Net TR Index (LCL)") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Index Name-2nd): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_IndexName2nd_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "3.5") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (MSCI Europe...(LCL)), Projected Beta): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_MSCIEurope_LCL_ProjeBeta_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "(4.5%)") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (MSCI Europe...(LCL)), Projected Exposure): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_MSCIEurope_LCL_ProjeEpos_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data="CASH") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Index Name-3rd): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_IndexName3rd_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "1") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (CASH, Projected Beta): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_CASH_ProjBeta" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "102.0%") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (CASH, Projected Exposure): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_CASH_ProjExpos" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Auto Score";
                verifyPoint = "Manager Inception" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "10 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && "5 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_datacolumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }
                
                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2")) < 200.00;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Manager Inception, Autoscore): " + (data=@"<200.00") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_ManInceptAutoscore_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3")) < 200.00;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Manager Inception, IR): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_ManInceptIR_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToInt32(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4")) < 200.00;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Manager Inception, Length in months): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_ManIncept_LenInMth_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Manager Alpha";
                verifyPoint = "2010" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "2021" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (Year)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_Year_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2010, Net Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_NetReturn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2010, Gross Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_GrosReturn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2010, CRBM): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_CRBM_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "5").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2010, Gross Total Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_GrossTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "6").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2010, Gross Strategy Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_GrossStratAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "7").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2010, Gross Manager Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_GrossManAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "8").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2010, Fees): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_Fees_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "9").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2010, Liquidity Cost): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_LiqCost_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "10").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2010, Net Total Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_NetTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Annualized Return";
                verifyPoint = "Annual Return Section" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "Ann. Manager Since Inception" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2")
                    && "Ann Ret Custom Start Date 2" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && "Last 10 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1")
                    && "Last 5 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "4", "1")
                    && "Last 3 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "5", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_ARtrSec_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, Ann. Manager Since Inception, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_ARtrSec_AnnManSinceIncep_NetManRetrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, Ann. Manager Since Inception, S&P 500 Index): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_ARtrSec_AnnManSinceIncep_SP500Indx_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "5").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, Ann. Manager Since Inception, MSCI Europe...(LCL)): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_ARtrSec_AnnManSinceIncep_MSCIEurope_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "6").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, Ann. Manager Since Inception, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_ARtrSec_AnnManSinceIncep_CusRisBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "Annual Risk Section" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "1")
                    && "10 Year SR" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "2")
                    && "5 Year SR" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "7", "1")
                    && "3 Year SR" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "8", "1")
                    && "SR of Custom Period" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "9", "1")
                    && "Standard Deviation Custom Period 1" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "10", "1")
                    && "IR over Custom Period 1" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "11", "1")
                    && "Tracking Error over Custom Period 1" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "12", "1")
                    && "Beta" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "13", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_ARiskSec_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "3")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, 10 Year SR, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_ARiskSec_10YearSR_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "4").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, 10 Year SR, S&P 500 Index): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_ARiskSec_10YearSR_SP500Ind_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "5").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, 10 Year SR, MSCI Europe...(LCL)): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_ARiskSec_10YearSR_MSCIEurope_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "6").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, 10 Year SR, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_ARiskSec_10YearSR_CusRiskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "1")
                    && "Number Down Months" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "2")
                    && "Percentage Down Months" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "15", "1")
                    && "Average Down Months" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "16", "1")
                    && "Max Gain" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "17", "1")
                    && "Second Largest" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "18", "1")
                    && "Third Largest" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "19", "1")
                    && "Largest Loss" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "20", "1")
                    && "Second Largest Loss" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "21", "1")
                    && "Third Largest Loss" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "22", "1")
                    && "Max Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "23", "1")
                    && "Months in Max Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "24", "1")
                    && "Months to Recover" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "25", "1")
                    && "Peak before Max Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "26", "1")
                    && "Valley" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "27", "1")
                    && "2nd Worst" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "28", "1")
                    && "3rd Worst" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "29", "1")
                    && "4th Worst" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "30", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "Drawdown_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "3")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, Number Down Months, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_Drawdown_NumDwnMnth_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "4")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, Number Down Months, S&P 500 Index): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_Drawdown_NumDwnMnth_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "5")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, Number Down Months, MSCI Europe...(LCL)): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_Drawdown_NumDwnMnth_MSCIEurope_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "6")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, Number Down Months, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_Drawdown_NumDwnMnth_CusRiskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "Annualized Correlation" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "1")
                    && "Correlation Since Inception" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "2")
                    && "One Year Rolling Correlation (Mean)" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "32", "1")
                    && "Two Year Rolling Correlation (Mean)" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "33", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_AnnualCorrel_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, Correlation Since Inception, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_AnnualCorrel_CorSincIncep_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "4")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, Correlation Since Inception, S&P 500 Index): " + (data="<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_AnnualCorrel_CorSincIncep_SP500Ind_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "5")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, Correlation Since Inception, MSCI Europe...(LCL)): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_AnnualCorrel_CorSincIncep_MSCIEurope_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "6").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, Correlation Since Inception, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_AnnualCorrel_CorSincIncep_CusRiskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Metrics Over Selected Time Frames";
                verifyPoint = SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1").Contains(data = "ITD : 05/01/2010 - ") // KS-603
                    && "Risk" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && "Sharpe Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "4", "1")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "5", "1")
                    && SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "1").Contains(data = "Last 3 Years : 05/01/2018 -")
                    && "Risk" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "7", "1")
                    && "Sharpe Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "8", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "9", "1")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "10", "1")
                    && SearchFundAction.Instance.DataTableOfReportGetText(10, table, "11", "1").Contains(data = "Last 5 Years : 05/01/2016 -")
                    && "Risk" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "12", "1")
                    && "Sharpe Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "13", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "1")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "15", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (data column1): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "2").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_Risk_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "3").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk, Gross Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_Risk_GrosManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "4").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_Risk_CusRiskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "5").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk, RF Rate): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_Risk_RFRate_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "6").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk, Gross Total Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_Risk_GrossTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "7");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk, Gross Strategy Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_Risk_GrossStratAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "8");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk, Gross Manager Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_Risk_GrossManAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "9");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk, Fees): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_Risk_Fees_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "10");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk, Liquidity Cost): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_Risk_LiqCost_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "11").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk, Net Alpha): " + (data="<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                table = "Net Monthly Return";
                verifyPoint = "2010" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "2021" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (CY): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_CY_Year_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2010, Jan): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_Jan_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2010, Feb): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_Feb_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2010, Mar): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_Mar_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "5");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2010, Apr): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_Apr_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "6").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2010, May): " + (data="<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_May_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "7").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2010, Jun): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_Jun_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "8").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2010, Jul): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_Jul_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "9").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2010, Aug): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_Aug_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "10").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2010, Sep): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_Sep_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "11").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2010, Oct): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_Oct_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "12").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2010, Nov): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_Nov_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "13").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2010, Dec): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC002_" + table + "_2010_Dec_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }
                #endregion

                #region Verify Charts
                // Drawdown Graph (Chart)
                verifyPoint = SearchFundAction.Instance.IsDrawdownGraphPublicReportShown(10);
                verifyPoints.Add(summaryTC = "Verify the chart - Drawdown Graph is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Rolling Correlation Graph (Chart)
                verifyPoint = SearchFundAction.Instance.IsRollingCorrelationGraphPublicReportShown(10);
                verifyPoints.Add(summaryTC = "Verify the chart - Rolling Correlation Graph is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // fund Aum Graph (Chart) --> No Aum for this Fund
                #endregion

                // Stop recording video
                Driver.StopVideoRecord();

                // Delete video file
                Driver.DeleteFilesContainsName(System.IO.Path.GetFullPath(@"../../../../../TestResults/"), videoFileName);
            }
            catch (Exception exception)
            {
                // Stop recording video
                Driver.StopVideoRecord();

                // Print exception
                System.Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong! Please check console log.");
            }
            #endregion
        }

        [Test, Category("Regression Testing")]
        public void TC003_SinglePublicReport_Solovis_NoInput()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            const string managerName = "Laurion Capital Management LP";
            const string fundName = "Laurion Capital Ltd.";
            const string sourceIcon = "S";
            string videoFileName = "PublicReportTestTC003";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Public Report Test - TC003");
            try
            {
                // Start recording video
                Driver.StartVideoRecord(videoFileName);

                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Search a Fund - Source = Albourne
                SearchFundAction.Instance.InputNameToSearchFund(10, "laurion", managerName, fundName, sourceIcon);

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable); Thread.Sleep(1000);

                // Check if the spinner loading icon is shown then wait for it load done
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(30, General.loadingSpinner);
                }

                // Click 'Model' menu
                NavigationAction.Instance.ClickPageNames(10, SearchFundPage.model);
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.labelButton(SearchFundPage.userInput));

                // Click 'User Input' button
                GeneralAction.Instance.ClickButtonLabel(10, SearchFundPage.userInput);

                // check if dialog popup 'Model Parameters' is still not shown then click until it appears
                int time = 0;
                while (GeneralAction.Instance.IsElementPresent(SearchFundPage.userInputPanel) == false && time < 10)
                {
                    // Click 'User Input' button
                    GeneralAction.Instance.ClickButtonLabel(10, SearchFundPage.userInput);

                    // if dialog popup 'Model Parameters' is shown then exit loop
                    if (GeneralAction.Instance.IsElementPresent(SearchFundPage.userInputPanel) == true) { break; }

                    time++;
                    Thread.Sleep(1000);
                }

                // Check if A red notification is shown then waiting for that is disappeared
                System.Threading.Thread.Sleep(500);
                string msg = "This fund does not have Return data. Select another data source or upload the fund data.";
                if (SearchFundAction.Instance.IsElementPresent(SearchFundPage.errorInvalidMessageContent(msg)))
                {
                    SearchFundAction.Instance.WaitForElementInvisible(10, SearchFundPage.errorInvalidMessageContent(msg)); // WaitForLoadingIconToDisappear
                }

                // Click to expand User Input (section)
                SearchFundAction.Instance.ClickUserInputSubSection(10, SearchFundPage.dateSection)
                                         .ClickUserInputSubSection(10, SearchFundPage.customRiskBenchmarkModelling)
                                         .ClickUserInputSubSection(10, SearchFundPage.feeModelSection)
                                         .ClickUserInputSubSection(10, SearchFundPage.liquiditySection)
                                         .PageDownToScrollDownPage();

                //// Check if existing CRBM then delete All
                //SearchFundAction.Instance.CheckIfExistingCRBMThenDeleteAll(10, sourceIcon);

                // Liquidity Section
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.lockup, "None")
                                         .InputTxtLabelField(SearchFundPage.lockupLengthMonths, "7")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.liquidityFrequency, "Semi-Annually")
                                         .InputTxtLabelField(SearchFundPage.investorGate, "10.2")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.sidepocketProbability, "50%")
                                         .InputTxtLabelField(SearchFundPage.MaxPercOfSidepocketPermitted, "40.5"); Thread.Sleep(250);
                SearchFundAction.Instance.WaitForElementInvisible(10, SearchFundPage.errorInvalidMessageContent("All the liquidity parameters need to be populated except for Investor Gate"));

                // Click on 'Run' button
                System.Threading.Thread.Sleep(2000);
                SearchFundAction.Instance.ClickRunButton(10); Thread.Sleep(3000);

                // Check if the spinner loading icon is shown then wait for it load done
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(30, General.loadingSpinner);
                }

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {
                    // Custom Risk Benchmark - data

                    // Auto Score - data
                    managerInception_Autoscore = "9.55";
                    managerInception_IR = "0.96";
                    managerInception_LengthInMonths = "215";

                    // Annualized Return - data
                    annualReturnSection_AnnManagerSinceInception_NetManagerReturn = "11.4%";
                    annualReturnSection_AnnManagerSinceInception_crbm1 = "1.3%";
                    annualReturnSection_AnnManagerSinceInception_crbm2 = "7.5%";
                    annualReturnSection_AnnManagerSinceInception_CustomRiskBenchmark = "2.0%";
                    annualRiskSection_10YearSR_NetManagerReturn = "0.78";
                    annualRiskSection_10YearSR_crbm2 = "0.53";
                    annualRiskSection_10YearSR_CustomRiskBenchmark = "0.56";
                    drawdown_NumberDownMonths_NetManagerReturn = "64.00";
                    drawdown_NumberDownMonths_crbm2 = "79.00";
                    annualizedCorrelation_CorrelationSinceInception_crbm1 = "0.05";
                    annualizedCorrelation_CorrelationSinceInception_crbm2 = "0.07";
                    annualizedCorrelation_CorrelationSinceInception_CustomRiskBenchmark = "0.08";

                    // Metrics Over Selected Time Frames - data
                    last3Years_StartDate = "Last 3 Years :";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    // Custom Risk Benchmark - data

                    // Auto Score - data
                    managerInception_Autoscore = "9.55";
                    managerInception_IR = "0.96";
                    managerInception_LengthInMonths = "215";

                    // Annualized Return - data
                    annualReturnSection_AnnManagerSinceInception_NetManagerReturn = "11.4%";
                    annualReturnSection_AnnManagerSinceInception_crbm1 = "1.3%";
                    annualReturnSection_AnnManagerSinceInception_crbm2 = "7.5%";
                    annualReturnSection_AnnManagerSinceInception_CustomRiskBenchmark = "2.0%";
                    annualRiskSection_10YearSR_NetManagerReturn = "0.78";
                    annualRiskSection_10YearSR_crbm2 = "0.53";
                    annualRiskSection_10YearSR_CustomRiskBenchmark = "0.56";
                    drawdown_NumberDownMonths_NetManagerReturn = "64.00";
                    drawdown_NumberDownMonths_crbm2 = "79.00";
                    annualizedCorrelation_CorrelationSinceInception_crbm1 = "0.05";
                    annualizedCorrelation_CorrelationSinceInception_crbm2 = "0.07";
                    annualizedCorrelation_CorrelationSinceInception_CustomRiskBenchmark = "0.08";

                    // Metrics Over Selected Time Frames - data
                    last3Years_StartDate = "Last 3 Years :";
                }

                #region Verify data in tables of Report
                string table = "Custom Risk Benchmark";
                string data = "FTSE 3 Month T-Bill Index";
                verifyPoint = data == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    || (data = "MSCI ACWI IMI with USA Net TR Index (USD)") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Index Name, 1st crbm): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_IndexName_1stCrbm_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "1") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (1st crbm, Projected Beta): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_1stCrbm_ProjBeta_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "90.0%") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3")
                    || (data = "10.0%") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (1st crbm, Projected Exposure): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_1stCrbm_ProjExpos_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "MSCI ACWI IMI with USA Net TR Index (USD)") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    || (data = "FTSE 3 Month T-Bill Index") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Index Name, 2nd crbm): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_IndexName_2ndCrbm_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "1") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (2nd crbm), Projected Beta): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2ndCrbm_ProjBeta_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "10.0%") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "3")
                    || (data = "90.0%") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (2nd crbm), Projected Exposure): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2ndCrbm_ProjExpos_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Auto Score";
                verifyPoint = "Manager Inception" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "10 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && "5 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2")) < 300;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Manager Inception, Autoscore): " + (data="<300") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ManIncep_Autoscore_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3")) < 300;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Manager Inception, IR): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ManIncep_IR_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToInt32(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4")) < 300;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Manager Inception, Length in months): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ManIncep_LenInMths_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Manager Alpha";
                verifyPoint = "2005" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "2025" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (Year)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_Year_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2005, Net Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_NetRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2005, Gross Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_GrosRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2005, CRBM): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_CRBM_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "5").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2005, Gross Total Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_GrossTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "6").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2005, Gross Strategy Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_GrossStratAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "7").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2005, Gross Manager Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_GrossManAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "8").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2005, Fees): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_Fees_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "9").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2005, Liquidity Cost): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_LiqCost_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "10").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2005, Net Total Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_NetTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Annualized Return";
                verifyPoint = "Annual Return Section" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "Ann. Manager Since Inception" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2")
                    && "Ann Ret Custom Start Date 2" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && "Last 10 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1")
                    && "Last 5 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "4", "1")
                    && "Last 3 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "5", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ARtrnSec_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, Ann. Manager Since Inception, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ARtrnSec_AnnManSinceIncept_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, Ann. Manager Since Inception, crbm1): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ARtrnSec_AnnManSinceIncept_crbm1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "5").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, Ann. Manager Since Inception, crbm2): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ARtrnSec_AnnManSinceIncept_crbm2_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "6").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, Ann. Manager Since Inception, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ARtrnSec_AnnManSinceIncept_CusRiskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "Annual Risk Section" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "1")
                        && "10 Year SR" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "2")
                        && "5 Year SR" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "7", "1")
                        && "3 Year SR" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "8", "1")
                        && "SR of Custom Period" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "9", "1")
                        && "Standard Deviation Custom Period 1" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "10", "1")
                        && "IR over Custom Period 1" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "11", "1")
                        && "Tracking Error over Custom Period 1" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "12", "1")
                        && "Beta" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "13", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ARiskSec_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "3").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, 10 Year SR, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ARiskSec_10YSR_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "4")
                    || Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "4").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, 10 Year SR, crbm1): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ARiskSec_10YSR_crbm1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "5")
                    || Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "5").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, 10 Year SR, crbm2): " + (data="<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ARiskSec_10YSR_crbm2_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "6").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, 10 Year SR, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ARiskSec_10YSR_crbm2_CusRiskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "1")
                        && "Number Down Months" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "2")
                        && "Percentage Down Months" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "15", "1")
                        && "Average Down Months" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "16", "1")
                        && "Max Gain" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "17", "1")
                        && "Second Largest" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "18", "1")
                        && "Third Largest" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "19", "1")
                        && "Largest Loss" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "20", "1")
                        && "Second Largest Loss" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "21", "1")
                        && "Third Largest Loss" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "22", "1")
                        && "Max Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "23", "1")
                        && "Months in Max Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "24", "1")
                        && "Months to Recover" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "25", "1")
                        && "Peak before Max Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "26", "1")
                        && "Valley" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "27", "1")
                        && "2nd Worst" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "28", "1")
                        && "3rd Worst" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "29", "1")
                        && "4th Worst" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "30", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_Drawdown_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "3")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, Number Down Months, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_Drawdown_NumDownMth_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "4")
                    || Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "4")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, Number Down Months, crbm1): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_Drawdown_NumDownMth_crbm1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "5")
                    || Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "5")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, Number Down Months, crbm2): " + (data="<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_Drawdown_NumDownMth_crbm2_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "6")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, Number Down Months, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_Drawdown_NumDownMth_CusRiskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "Annualized Correlation" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "1")
                        && "Correlation Since Inception" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "2")
                        && "One Year Rolling Correlation (Mean)" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "32", "1")
                        && "Two Year Rolling Correlation (Mean)" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "33", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_AnnualCorr_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, Correlation Since Inception, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_AnnualCorr_CorrSinceIncep_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "4").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, Correlation Since Inception, FTSE 3 Month T-Bill Index): " + (data = "<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_AnnualCorr_CorrSinceIncep_FTSE3TBillInd_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "5").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, Correlation Since Inception, MSCI ACWI...(USD)): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_AnnualCorr_CorrSinceIncep_MSCIACWIusd_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "6").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, Correlation Since Inception, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_AnnualCorr_CorrSinceIncep_CusRiskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Metrics Over Selected Time Frames";
                verifyPoint = SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1").Contains(data = "ITD : 09/01/2005 - ")
                    && "Risk" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && "Sharpe Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "4", "1")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "5", "1")
                    && SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "1").Contains(last3Years_StartDate)
                    && "Risk" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "7", "1")
                    && "Sharpe Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "8", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "9", "1")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "10", "1")
                    && SearchFundAction.Instance.DataTableOfReportGetText(10, table, "11", "1").Contains(data = "KS Inception : 12/31/2020 -")
                    && "Risk" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "12", "1")
                    && "Sharpe Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "13", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "1")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "15", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (data column1): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "2").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (ITD : 09/01/2005 -..., Risk, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ITD_09012005_RiskNetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "3").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (ITD : 09/01/2005 -..., Risk, Gross Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ITD_09012005_RiskGrosManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "4").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (ITD : 09/01/2005 -..., Risk, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ITD_09012005_RiskCusRiskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "5").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (ITD : 09/01/2005 -..., Risk, RF Rate): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ITD_09012005_RiskRFRate_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "6").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (ITD : 09/01/2005 -..., Risk, Gross Total Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ITD_09012005_RisGrosTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "7");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (ITD : 09/01/2005 -..., Risk, Gross Strategy Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ITD_09012005_RisGrosStratAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "8");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (ITD : 09/01/2005 -..., Risk, Gross Manager Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ITD_09012005_RisGrosManAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "9");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (ITD : 09/01/2005 -..., Risk, Fees): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ITD_09012005_RiskFees_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "10");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (ITD : 09/01/2005 -..., Risk, Liquidity Cost): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ITD_09012005_RiskLiqCost_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "11").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (ITD : 09/01/2005 -..., Risk, Net Alpha): " + (data="<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_ITD_09012005_RiskNetAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Net Monthly Return";
                verifyPoint = "2005" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "2025" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (CY): " + "Year" + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_CY_Year_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2005, Jan): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_Jan_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2005, Feb): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_Feb_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2005, Mar): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_Mar_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "5");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2005, Apr): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_Apr_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "6");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2005, May): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_May_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "7");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2005, Jun): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_Jun_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "8");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2005, Jul): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_Jul_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "9");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2005, Aug): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_Aug_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "10").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2005, Sep): " + (data="<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_Sep_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "11").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2005, Oct): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_Oct_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "12").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2005, Nov): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_Nov_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "13").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2005, Dec): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC003_" + table + "_2005_Dec_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }
                #endregion

                #region Verify Charts
                // Drawdown Graph (Chart)
                verifyPoint = SearchFundAction.Instance.IsDrawdownGraphPublicReportShown(10);
                verifyPoints.Add(summaryTC = "Verify the chart - Drawdown Graph is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Rolling Correlation Graph (Chart)
                verifyPoint = SearchFundAction.Instance.IsRollingCorrelationGraphPublicReportShown(10);
                verifyPoints.Add(summaryTC = "Verify the chart - Rolling Correlation Graph is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // fund NAV Graph (Chart)
                verifyPoint = SearchFundAction.Instance.IsFundAumGraphPublicReportShown(10);
                verifyPoints.Add(summaryTC = "Verify the chart - Fund Aum Graph is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                // Stop recording video
                Driver.StopVideoRecord();

                // Delete video file
                Driver.DeleteFilesContainsName(System.IO.Path.GetFullPath(@"../../../../../TestResults/"), videoFileName);
            }
            catch (Exception exception)
            {
                // Stop recording video
                Driver.StopVideoRecord();

                // Print exception
                System.Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong! Please check console log.");
            }
            #endregion
        }

        [Test, Category("Regression Testing")]
        public void TC004_SinglePublicReport_AEvestment_NoInput()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            const string managerName = "Amber Capital";
            const string fundName = "Amber European Long Opportunities Fund";
            const string sourceIcon = "E";
            string videoFileName = "PublicReportTestTC004";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Public Report Test - TC004");
            try
            {
                // Start recording video
                Driver.StartVideoRecord(videoFileName);

                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Search a Fund - Source = Albourne
                SearchFundAction.Instance.InputNameToSearchFund(10, "amber", managerName, fundName, sourceIcon);

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable); Thread.Sleep(3000);

                // Check if the spinner loading icon is shown then wait for it load done
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(30, General.loadingSpinner);
                }

                // Click 'Model' menu
                NavigationAction.Instance.ClickPageNames(10, SearchFundPage.model);
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.labelButton(SearchFundPage.userInput));

                // Click 'User Input' button
                GeneralAction.Instance.ClickButtonLabel(10, SearchFundPage.userInput)
                                      .WaitForElementVisible(10, SearchFundPage.userInputPanel);

                // Click to expand User Input (section)
                SearchFundAction.Instance.ClickUserInputSubSection(10, SearchFundPage.dateSection)
                                         .ClickUserInputSubSection(10, SearchFundPage.customRiskBenchmarkModelling)
                                         .ClickUserInputSubSection(10, SearchFundPage.feeModelSection)
                                         .ClickUserInputSubSection(10, SearchFundPage.liquiditySection)
                                         .PageDownToScrollDownPage();

                // Check if A red notification is shown then waiting for that is disappeared
                string msg = "This fund does not have Return data. Select another data source or upload the fund data.";
                if (SearchFundAction.Instance.IsElementPresent(SearchFundPage.errorInvalidMessageContent(msg)))
                {
                    SearchFundAction.Instance.WaitForElementInvisible(10, SearchFundPage.errorInvalidMessageContent(msg)); // WaitForLoadingIconToDisappear
                }

                // Check if existing CRBM then delete All
                SearchFundAction.Instance.CheckIfExistingCRBMThenDeleteAll(10, sourceIcon);

                // Liquidity Section
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.lockup, "Soft")
                                         .InputTxtLabelField(SearchFundPage.lockupLengthMonths, "8")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.liquidityFrequency, "Daily")
                                         .InputTxtLabelField(SearchFundPage.investorGate, "100")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.sidepocketProbability, "100%")
                                         .InputTxtLabelField(SearchFundPage.MaxPercOfSidepocketPermitted, "50.5"); Thread.Sleep(250);
                SearchFundAction.Instance.WaitForElementInvisible(10, SearchFundPage.errorInvalidMessageContent("All the liquidity parameters need to be populated except for Investor Gate"));

                // Click on 'Run' button
                System.Threading.Thread.Sleep(2000);
                SearchFundAction.Instance.ClickRunButton(10);
                SearchFundAction.Instance.WaitForLoadingIconToDisappear(30, General.loadingSpinner);

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {
                    // Custom Risk Benchmark - data

                    // Auto Score - data

                    // Manager Alpha - data
                    firstYear_GrossManagerAlpha = "6.4%";

                    // Annualized Return - data
                    annualReturnSection_AnnManagerSinceInception_CustomRiskBenchmark = "1.4%";
                    annualizedCorrelation_CorrelationSinceInception_CustomRiskBenchmark = "(0.20)";

                    // Metrics Over Selected Time Frames - data
                    iTD_Risk_GrossTotalAlpha = "17.8%";
                    iTD_Risk_NetAlpha = "17.8%";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    // Custom Risk Benchmark - data

                    // Auto Score - data

                    // Manager Alpha - data
                    firstYear_GrossManagerAlpha = "6.4%";

                    // Annualized Return - data
                    annualReturnSection_AnnManagerSinceInception_CustomRiskBenchmark = "1.4%";
                    annualizedCorrelation_CorrelationSinceInception_CustomRiskBenchmark = "(0.20)";

                    // Metrics Over Selected Time Frames - data
                    iTD_Risk_GrossTotalAlpha = "17.8%";
                    iTD_Risk_NetAlpha = "17.8%";
                }

                #region Verify data in tables of Report
                string table = "Custom Risk Benchmark";
                string data = "CASH";
                verifyPoint = data == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Index Name): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_IndexName_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "1") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (CASH, Projected Beta): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Cash_ProjBeta_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "100.0%") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (CASH, Projected Exposure): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Cash_ProjExpos_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Auto Score";
                verifyPoint = "Manager Inception" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "10 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && "5 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2")) < 300;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Manager Inception, Autoscore): " + (data="<300") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_ManIncep_Autoscore_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3").Replace("%", "").Replace("(", "").Replace(")", "")) < 300;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Manager Inception, IR): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_ManIncep_IR_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToInt32(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4").Replace("%", "").Replace("(", "").Replace(")", "")) < 300;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Manager Inception, Length in months): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_ManIncep_LenInMonth_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Manager Alpha";
                verifyPoint = "2016" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "2020" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (Year)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Year_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2016, Net Return): " + (data = "<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_NetReturn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2016, Gross Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_GrosReturn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2016, CRBM): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_CRBM_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "5").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2016, Gross Total Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_GrossTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "6").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2016, Gross Strategy Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_GrossStratAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "7").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2016, Gross Manager Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_GrossManAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "8").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2016, Fees): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_Fees_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "9").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2016, Liquidity Cost): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_LiqCost_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "10").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2016, Net Total Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_NetTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Annualized Return";
                verifyPoint = "Annual Return Section" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "Ann. Manager Since Inception" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2")
                    && "Ann Ret Custom Start Date 2" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && "Last 10 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1")
                    && "Last 5 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "4", "1")
                    && "Last 3 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "5", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_AnRtrnSec_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, Ann. Manager Since Inception, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_AnRtrnSec_ManSinceIncep_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, Ann. Manager Since Inception, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_AnRtrnSec_AnnManSinceIncep_CusRisBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "Annual Risk Section" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "1")
                    && "10 Year SR" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "2")
                    && "5 Year SR" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "7", "1")
                    && "3 Year SR" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "8", "1")
                    && "SR of Custom Period" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "9", "1")
                    && "Standard Deviation Custom Period 1" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "10", "1")
                    && "IR over Custom Period 1" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "11", "1")
                    && "Tracking Error over Custom Period 1" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "12", "1")
                    && "Beta" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "13", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_AnRiskSec_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, 10 Year SR, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_AnRiskSec_10YearSR_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "4");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, 10 Year SR, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_AnRiskSec_10YearSR_CusRisBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "1")
                    && "Number Down Months" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "2")
                    && "Percentage Down Months" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "15", "1")
                    && "Average Down Months" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "16", "1")
                    && "Max Gain" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "17", "1")
                    && "Second Largest" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "18", "1")
                    && "Third Largest" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "19", "1")
                    && "Largest Loss" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "20", "1")
                    && "Second Largest Loss" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "21", "1")
                    && "Third Largest Loss" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "22", "1")
                    && "Max Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "23", "1")
                    && "Months in Max Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "24", "1")
                    && "Months to Recover" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "25", "1")
                    && "Peak before Max Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "26", "1")
                    && "Valley" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "27", "1")
                    && "2nd Worst" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "28", "1")
                    && "3rd Worst" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "29", "1")
                    && "4th Worst" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "30", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Drawdown_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "3")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, Number Down Months, Net Manager Return): " + (data="<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Drawdown_NumDwnMth_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "4")
                    || Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "4").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, Number Down Months, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Drawdown_NumDwnMth_CusRisBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "Annualized Correlation" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "1")
                    && "Correlation Since Inception" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "2")
                    && "One Year Rolling Correlation (Mean)" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "32", "1")
                    && "Two Year Rolling Correlation (Mean)" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "33", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_AnnualizeCorrl_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, Correlation Since Inception, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_AnnualizeCorrl_CorrSinIncep_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "4").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, Correlation Since Inception, Custom Risk Benchmark): " + (data = "<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_AnnualizeCorrl_CorrSinIncep_CusRisBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Metrics Over Selected Time Frames";
                verifyPoint = SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1").Contains(data = "ITD") // ITD : 07/01/2016 - 
                    && "Risk" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && "Sharpe Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "4", "1")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "5", "1")
                    && SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "1").Contains(data = "Last 3 Years") // Last 3 Years : 09/01/2017 -
                    && "Risk" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "7", "1")
                    && "Sharpe Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "8", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "9", "1")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "10", "1")
                    && SearchFundAction.Instance.DataTableOfReportGetText(10, table, "11", "1").Contains(data = "Last 5 Years") // Last 5 Years : 09/01/2015 -
                    && "Risk" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "12", "1")
                    && "Sharpe Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "13", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "1")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "15", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (data column1): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "2").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/2016), Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Risk_ITD_07012016_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "3").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/2016), Gross Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Risk_ITD_07012016_GrosManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "4").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/2016), Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Risk_ITD_07012016_CusRisBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "5").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/2016), RF Rate): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Risk_ITD_07012016_RFRate_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "6").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/2016), Gross Total Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Risk_ITD_07012016_GrossTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "7");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/2016), Gross Strategy Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Risk_ITD_07012016_GrossStratAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "8");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/2016), Gross Manager Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Risk_ITD_07012016_GrossManAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "9");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/2016), Fees): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Risk_ITD_07012016_Fees_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "10");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/2016), Liquidity Cost): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Risk_ITD_07012016_LiqCost_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "11").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 07/01/2016), Net Alpha): " + (data="<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_Risk_ITD_07012016_NetAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Net Monthly Return";
                verifyPoint = "2016" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "2020" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (CY): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_CY_Year_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2016, Jan): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_Jan_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "8").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2016, Jul): " + (data="<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_Jul_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "9").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2016, Aug): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_Aug_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "10").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2016, Sep): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_Sep_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "11").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2016, Oct): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_Oct_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "12").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2016, Nov): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_Nov_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "13").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2016, Dec): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC004_" + table + "_2016_Dec_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }
                #endregion

                #region Verify Charts
                // Drawdown Graph (Chart)
                verifyPoint = SearchFundAction.Instance.IsDrawdownGraphPublicReportShown(10);
                verifyPoints.Add(summaryTC = "Verify the chart - Drawdown Graph is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Rolling Correlation Graph (Chart)
                verifyPoint = SearchFundAction.Instance.IsRollingCorrelationGraphPublicReportShown(10);
                verifyPoints.Add(summaryTC = "Verify the chart - Rolling Correlation Graph is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // fund Aum Graph (Chart) --> No Aum for this Fund
                #endregion

                // Stop recording video
                Driver.StopVideoRecord();

                // Delete video file
                Driver.DeleteFilesContainsName(System.IO.Path.GetFullPath(@"../../../../../TestResults/"), videoFileName);
            }
            catch (Exception exception)
            {
                // Stop recording video
                Driver.StopVideoRecord();

                // Print exception
                System.Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong! Please check console log.");
            }
            #endregion
        }

        [Test, Category("Regression Testing")]
        public void TC005_SinglePublicReport_TEvestment_NoInput()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            const string managerName = "John Hancock Investments";
            const string fundName = "John Hancock Multimanager Lifetime Portfolios 2035";
            const string sourceIcon = "E";
            string videoFileName = "PublicReportTestTC005";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Public Report Test - TC005");
            try
            {
                // Start recording video
                Driver.StartVideoRecord(videoFileName);

                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Search a Fund - Source = Albourne
                SearchFundAction.Instance.InputNameToSearchFund(10, managerName, managerName, fundName, sourceIcon);

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable); Thread.Sleep(1000);

                // Check if the spinner loading icon is shown then wait for it load done
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(30, General.loadingSpinner);
                }

                // Click 'Model' menu
                NavigationAction.Instance.ClickPageNames(10, SearchFundPage.model);
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.labelButton(SearchFundPage.userInput));

                // Click 'User Input' button
                GeneralAction.Instance.ClickButtonLabel(10, SearchFundPage.userInput)
                                      .WaitForElementVisible(10, SearchFundPage.userInputPanel);

                // Check if A red notification is shown then waiting for that is disappeared
                System.Threading.Thread.Sleep(500);
                string msg = "This fund does not have Return data. Select another data source or upload the fund data.";
                if (SearchFundAction.Instance.IsElementPresent(SearchFundPage.errorInvalidMessageContent(msg)))
                {
                    SearchFundAction.Instance.WaitForLoadingIconToDisappear(10, SearchFundPage.errorInvalidMessageContent(msg));
                }

                // Click to expand User Input (section)
                SearchFundAction.Instance.ClickUserInputSubSection(10, SearchFundPage.dateSection)
                                         .ClickUserInputSubSection(10, SearchFundPage.customRiskBenchmarkModelling)
                                         .ClickUserInputSubSection(10, SearchFundPage.feeModelSection)
                                         .ClickUserInputSubSection(10, SearchFundPage.liquiditySection)
                                         .PageDownToScrollDownPage();

                // Check if existing CRBM then delete All
                SearchFundAction.Instance.CheckIfExistingCRBMThenDeleteAll(10, sourceIcon);

                // Liquidity Section
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.lockup, "Hard")
                                         .InputTxtLabelField(SearchFundPage.lockupLengthMonths, "10")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.liquidityFrequency, "Weekly")
                                         .InputTxtLabelField(SearchFundPage.investorGate, "70")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.sidepocketProbability, "50%")
                                         .InputTxtLabelField(SearchFundPage.MaxPercOfSidepocketPermitted, "100"); Thread.Sleep(250);
                SearchFundAction.Instance.WaitForElementInvisible(10, SearchFundPage.errorInvalidMessageContent("All the liquidity parameters need to be populated except for Investor Gate"));

                // Click on 'Run' button
                System.Threading.Thread.Sleep(2000);
                SearchFundAction.Instance.ClickRunButton(10);
                //SearchFundAction.Instance.WaitForLoadingIconToDisappear(30, General.loadingSpinner);
                Thread.Sleep(3000);

                // Check if the spinner loading icon is shown then wait for it load done
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(30, General.loadingSpinner);
                }

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {
                    // Custom Risk Benchmark - data

                    // Auto Score - data
                    managerInception_Autoscore = "6.60";
                    managerInception_IR = "0.39";
                    managerInception_LengthInMonths = "195";

                    // Manager Alpha
                    latestYear = "2025";

                    // Annualized Return - data
                    annualReturnSection_AnnManagerSinceInception_NetManagerReturn = "5.8%";
                    annualReturnSection_AnnManagerSinceInception_crbm1 = "1.0%";
                    annualRiskSection_10YearSR_NetManagerReturn = "0.53";
                    drawdown_NumberDownMonths_NetManagerReturn = "73.00";
                    annualizedCorrelation_CorrelationSinceInception_CustomRiskBenchmark = "(0.05)";

                    // Metrics Over Selected Time Frames - data
                    last3Years_StartDate = "Last 3 Years :";
                    last5Years_StartDate = "Last 5 Years :";
                    iTD_Risk_NetManagerReturn = "15.5%";
                    iTD_Risk_GrossManagerReturn = "15.5%";
                    iTD_Risk_GrossTotalAlpha = "15.5%";
                    iTD_Risk_NetAlpha = "15.5%";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    // Custom Risk Benchmark - data

                    // Auto Score - data
                    managerInception_Autoscore = "6.59";
                    managerInception_IR = "0.38";
                    managerInception_LengthInMonths = "201";

                    // Manager Alpha
                    latestYear = "2025";

                    // Annualized Return - data
                    annualReturnSection_AnnManagerSinceInception_NetManagerReturn = "5.9%";
                    annualReturnSection_AnnManagerSinceInception_crbm1 = "1.1%";
                    annualRiskSection_10YearSR_NetManagerReturn = "0.48";
                    drawdown_NumberDownMonths_NetManagerReturn = "75.00";
                    annualizedCorrelation_CorrelationSinceInception_CustomRiskBenchmark = "(0.03)";

                    // Metrics Over Selected Time Frames - data
                    last3Years_StartDate = "Last 3 Years";
                    last5Years_StartDate = "Last 5 Years";
                    iTD_Risk_NetManagerReturn = "15.3%";
                    iTD_Risk_GrossManagerReturn = "15.3%";
                    iTD_Risk_GrossTotalAlpha = "15.3%";
                    iTD_Risk_NetAlpha = "15.3%";
                }

                #region Verify data in tables of Report
                string table = "Custom Risk Benchmark";
                string data = "CASH";
                verifyPoint = data == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Index Name): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_IndexName_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "1") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (CASH, Projected Beta): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_Cash_ProjBeta_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "100.0%") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (CASH, Projected Exposure): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_Cash_ProjExpos_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Auto Score";
                verifyPoint = "Manager Inception" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "10 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && "5 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2")) < 300;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Manager Inception, Autoscore): " + (data="<300") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_ManIncep_Autoscore_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3")) < 300;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Manager Inception, IR): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_ManIncep_IR_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToInt32(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4")) < 300;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Manager Inception, Length in months): " + managerInception_LengthInMonths + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_ManIncep_LenInMonth_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Manager Alpha";
                verifyPoint = (data="2006") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && (data=latestYear) == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (Year): '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_Year_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2006, Net Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_NetReturn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2006, Gross Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_GrossReturn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4").Replace("%", "").Replace("(", "").Replace(")", "")) < 200
                    || "" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2006, CRBM): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_CRBM_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "5").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2006, Gross Total Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_GrossTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "6").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2006, Gross Strategy Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_GrossStratAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "7").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2006, Gross Manager Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_GrossManAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "8").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2006, Fees): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_Fees_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "9").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2006, Liquidity Cost): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_LiqCost_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "10").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (2006, Net Total Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_NetTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Annualized Return";
                verifyPoint = "Annual Return Section" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && "Ann. Manager Since Inception" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2")
                    && "Ann Ret Custom Start Date 2" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && "Last 10 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1")
                    && "Last 5 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "4", "1")
                    && "Last 3 Years" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "5", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_AnRtrnSec_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, Ann. Manager Since Inception, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_AnRtrnSec_AnnManSinceIncep_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Return Section, Ann. Manager Since Inception, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_AnRtrnSec_AnnManSinceIncep_CusRiskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "Annual Risk Section" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "1")
                    && "10 Year SR" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "2")
                    && "5 Year SR" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "7", "1")
                    && "3 Year SR" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "8", "1")
                    && "SR of Custom Period" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "9", "1")
                    && "Standard Deviation Custom Period 1" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "10", "1")
                    && "IR over Custom Period 1" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "11", "1")
                    && "Tracking Error over Custom Period 1" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "12", "1")
                    && "Beta" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "13", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_AnRiskSec_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "3").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, 10 Year SR, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_AnRiskSec_10YSR_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "4").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annual Risk Section, 10 Year SR, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_AnRiskSec_10YSR_CusRiskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "1")
                    && "Number Down Months" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "2")
                    && "Percentage Down Months" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "15", "1")
                    && "Average Down Months" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "16", "1")
                    && "Max Gain" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "17", "1")
                    && "Second Largest" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "18", "1")
                    && "Third Largest" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "19", "1")
                    && "Largest Loss" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "20", "1")
                    && "Second Largest Loss" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "21", "1")
                    && "Third Largest Loss" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "22", "1")
                    && "Max Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "23", "1")
                    && "Months in Max Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "24", "1")
                    && "Months to Recover" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "25", "1")
                    && "Peak before Max Drawdown" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "26", "1")
                    && "Valley" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "27", "1")
                    && "2nd Worst" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "28", "1")
                    && "3rd Worst" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "29", "1")
                    && "4th Worst" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "30", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_Drawdown_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "3")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, Number Down Months, Net Manager Return): " + (data="<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_Drawdown_NumDwnMonth_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "4")
                    || double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "4").Replace("%", "").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Drawdown, Number Down Months, Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_Drawdown_NumDwnMonth_CusRiskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = "Annualized Correlation" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "1")
                    && "Correlation Since Inception" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "2")
                    && "One Year Rolling Correlation (Mean)" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "32", "1")
                    && "Two Year Rolling Correlation (Mean)" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "33", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_AnnuCorrl_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, Correlation Since Inception, Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_AnnuCorrl_CorrlSinceIncep_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "31", "4").Replace("(", "").Replace(")", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 4th table '" + table + "' (Annualized Correlation, Correlation Since Inception, Custom Risk Benchmark): " + (data="<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_AnnuCorrl_CorrlSinceIncep_CusRiskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Metrics Over Selected Time Frames";
                verifyPoint = SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1").Contains("ITD") // ITD : 11/01/2006 - 
                    && "Risk" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && "Sharpe Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "4", "1")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "5", "1")
                    && SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "1").Contains(last3Years_StartDate)
                    && "Risk" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "7", "1")
                    && "Sharpe Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "8", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "9", "1")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "10", "1")
                    && SearchFundAction.Instance.DataTableOfReportGetText(10, table, "11", "1").Contains(last5Years_StartDate)
                    && "Risk" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "12", "1")
                    && "Sharpe Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "13", "1")
                    && "Information Ratio" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "14", "1")
                    && "Tracking Error" == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "15", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (data column1)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_dataColumn1_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "2").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 10/01/2006), Net Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_RiskITD_10012006_NetManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "3").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 10/01/2006), Gross Manager Return): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_RiskITD_10012006_GrosManRtrn_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "4").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 10/01/2006), Custom Risk Benchmark): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_RiskITD_10012006_CusRiskBen_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "5").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 10/01/2006), RF Rate): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_RiskITD_10012006_RFRate_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "6").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD :10/01/2006), Gross Total Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_RiskITD_10012006_GrossTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "7");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 10/01/2006), Gross Strategy Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_RiskITD_10012006_GrossStratAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "8");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 10/01/2006), Gross Manager Alpha): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_RiskITD_10012006_GrossManAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "9");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 10/01/2006), Fees): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_RiskITD_10012006_Fees_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "10");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 10/01/2006), Liquidity Cost): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_RiskITD_10012006_LiqCost_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "11").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table '" + table + "' (Risk (ITD : 10/01/2006), Net Alpha): " + (data="<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_RiskITD_10012006_NetAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "Net Monthly Return";
                verifyPoint = (data="2006") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && (data=latestYear) == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (CY): '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_CY_Year_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2006, Jan): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_Jan_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "8");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2006, Jul): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_Jul_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "9");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2006, Aug): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_Aug_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "10");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2006, Sep): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_Sep_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "11");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2006, Oct): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_Oct_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "12").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2006, Nov): " + (data = "<200") + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_Nov_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "13").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table '" + table + "' (2006, Dec): " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PublicReportTC005_" + table + "_2006_Dec_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }
                #endregion

                #region Verify Charts
                // Drawdown Graph (Chart)
                verifyPoint = SearchFundAction.Instance.IsDrawdownGraphPublicReportShown(10);
                verifyPoints.Add(summaryTC = "Verify the chart - Drawdown Graph is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Rolling Correlation Graph (Chart)
                verifyPoint = SearchFundAction.Instance.IsRollingCorrelationGraphPublicReportShown(10);
                verifyPoints.Add(summaryTC = "Verify the chart - Rolling Correlation Graph is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // fund Aum Graph (Chart) --> No Aum for this Fund
                #endregion

                // Stop recording video
                Driver.StopVideoRecord();

                // Delete video file
                Driver.DeleteFilesContainsName(System.IO.Path.GetFullPath(@"../../../../../TestResults/"), videoFileName);
            }
            catch (Exception exception)
            {
                // Stop recording video
                Driver.StopVideoRecord();

                // Print exception
                System.Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong! Please check console log.");
            }
            #endregion
        }
    }
}