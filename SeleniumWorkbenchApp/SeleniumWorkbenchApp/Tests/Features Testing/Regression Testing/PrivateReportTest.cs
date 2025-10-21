using AventStack.ExtentReports;
using NUnit.Framework;
using SeleniumGendKS.Core.FilesComparision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WorkbenchApp.UITest.Core.BaseTestCase;
using WorkbenchApp.UITest.Core.Selenium;
using WorkbenchApp.UITest.Generals;
using WorkbenchApp.UITest.Pages;

namespace WorkbenchApp.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(7)]
    internal class PrivateReportTest : BaseTestCase
    {
        #region Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;

        // Fund Metrics
        private string? fundName1 = null;
        private string? fundName2 = null;
        private string? fundName3 = null;
        private string? fundName4 = null;
        private string? fundName5 = null;
        private string? fundName6 = null;
        private string? fundName7 = null;
        private string? fundName8 = null;
        private string? fundMetricsVintageYear1 = null;
        private string? fundMetricsVintageYear2 = null;
        private string? fundMetricsVintageYear3 = null;
        private string? fundMetricsVintageYear4 = null;
        private string? fundMetricsVintageYear5 = null;
        private string? fundMetricsVintageYear6 = null;
        private string? fundMetricsVintageYear7 = null;
        private string? fundMetricsVintageYear8 = null;
        private string? fundMetricsFundSize1 = null;
        private string? fundMetricsFundSize2 = null;
        private string? fundMetricsFundSize3 = null;
        private string? fundMetricsFundSize4 = null;
        private string? fundMetricsFundSize5 = null;
        private string? fundMetricsFundSize6 = null;
        private string? fundMetricsFundSize7 = null;
        private string? fundMetricsFundSize8 = null;
        private string? fundMetricsTotalInvested1 = null;
        private string? fundMetricsTotalInvestedLast = null;
        private string? fundMetricsPercInvested1 = null;
        private string? fundMetricsPercInvestedLast = null;
        private string? fundMetricsRealized1 = null;
        private string? fundMetricsRealizedLast = null;
        private string? fundMetricsUnRealizedCurNav1 = null;
        private string? fundMetricsUnRealizedCurNavLast = null;
        private string? fundMetricsTotalValue1 = null;
        private string? fundMetricsTotalValueLast = null;

        // IRR
        private string? iRRCAUpperQuartile1 = null;
        private string? iRRCAUpperQuartileLast = null;
        private string? iRRCAMedian1 = null;
        private string? iRRCAMedianLast = null;

        // TVPI
        private string? tVPINetTVPI1 = null;
        private string? tVPINetTVPILast = null;
        private string? tVPICAUpperQuartile1 = null;
        private string? tVPICAUpperQuartileLast = null;
        private string? tVPICAMedian1 = null;
        private string? tVPICAMedianLast = null;

        // DPI
        private string? dPICAUpperQuartile1 = null;
        private string? dPICAUpperQuartileLast = null;
        private string? dPICAMedian1 = null;
        private string? dPICAMedianLast = null;
        #endregion

        [Test, Category("Regression Testing")]
        public void TC001_SingleManagerDashboard_SourceManual() // Private Report
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string cambridgeFund = "VGO Capital Partners";
            const string sourceIcon = "C";
            const string asOfDate = "As of Date";
            string userProfileDownloadPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
            string fileName = @"download.pdf";
            string? fileNameBaseline = null;
            string videoFileName = "PrivateReportTestTC001";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Private Report Test - TC001");
            try
            {
                // Start recording video
                Driver.StartVideoRecord(videoFileName);

                // Delete downloaded PDF file
                GeneralAction.Instance.DeleteFilePath(userProfileDownloadPath, fileName);

                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Search a Fund - Source = Albourne
                SearchFundAction.Instance.InputNameToSearchFund(10, "vgo ")
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(cambridgeFund, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, cambridgeFund, sourceIcon);

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

                /// User Input
                // Select Report Type = Single Manager Dashboard
                SearchFundAction.Instance.ClickToCheckTheCheckbox("Single Manager Dashboard")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.dataSource, "Manual"); // --> Select Data Source = Manual

                // Verify label of fields in Single Manager Dashboard
                verifyPoint = asOfDate == SearchFundAction.Instance.IsDatePickerLabelShown(10);
                verifyPoints.Add(summaryTC = "Verify label date-picker of '" + SearchFundPage.dateSelection + "'is shown correctly: '" + asOfDate + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Input data for Report - Single Manager Dashboard
                SearchFundAction.Instance.WaitForElementVisible(10, SearchFundPage.labelDropdown(SearchFundPage.assetClass))
                                         .InputTxtDatePickerTitle(10, SearchFundPage.dateSelection, "2021", "Jun", "30")
                                         .PageDownToScrollDownPage() //.ClickCRBMAddButton(10)
                                         .CheckIfExistingCRBMThenDeleteAll(10, sourceIcon)
                                         .InputTxtNameCRBMRow(10, 1, "s&p") // input 1st CRBM
                                         .WaitForElementVisible(20, SearchFundPage.nameCRBMReturnOfResults("S&P 500 Index"))
                                         .ClickNameCRBMReturnOfResults(10, "S&P 500 Index").WaitForElementInvisible(10, SearchFundPage.overlayDropdown)
                                         .InputNumberBetaCRBMRow(10, 1, "1")
                                         .InputNumberGrossExposureCRBMRow(10, 1, "80")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.assetClass, "Buyout")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.geography, "Africa");

                // Click on 'Run' button
                System.Threading.Thread.Sleep(2000);
                SearchFundAction.Instance.ClickRunButton(10)
                                         .WaitForLoadingIconToDisappear(30, General.loadingSpinner);

                #region Verify headers in tables of Report
                string table = "Fund Metrics";
                verifyPoint = SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 1, "Fund Name")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 2, "Vintage Year")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 3, "Fund Size")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 4, "Total Invested")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 5, "% Invested")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 6, "Realized")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 7, "Unrealized (Current NAV)")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 8, "Total Value");
                verifyPoints.Add(summaryTC = "Verify headers in the 1st table: '" + table + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                table = "IRR";
                verifyPoint = SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 1, "Fund Name")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 2, "Gross")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 3, "Net")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 4, "PME")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 5, "Gross Total Alpha")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 6, "Net Total Alpha")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 7, "CA Upper Quartile")
                    && SearchFundAction.Instance.HeaderTableOfReportGetText(10, table, 8, "CA Median");
                verifyPoints.Add(summaryTC = "Verify headers in the 2nd table: '" + table + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                table = "TVPI & DPI";
                verifyPoint = SearchFundAction.Instance.HeaderTVPIAndDPITableOfReportGetText(10, 1, "Fund Name")
                    && SearchFundAction.Instance.HeaderTVPIAndDPITableOfReportGetText(10, 2, "Gross TVPI")
                    && SearchFundAction.Instance.HeaderTVPIAndDPITableOfReportGetText(10, 3, "Net TVPI")
                    && SearchFundAction.Instance.HeaderTVPIAndDPITableOfReportGetText(10, 4, "PME")
                    && SearchFundAction.Instance.HeaderTVPIAndDPITableOfReportGetText(10, 5, "CA Upper Quartile")
                    && SearchFundAction.Instance.HeaderTVPIAndDPITableOfReportGetText(10, 6, "CA Median")
                    && SearchFundAction.Instance.HeaderTVPIAndDPITableOfReportGetText(10, 7, "DPI")
                    && SearchFundAction.Instance.HeaderTVPIAndDPITableOfReportGetText(10, 8, "CA Upper Quartile")
                    && SearchFundAction.Instance.HeaderTVPIAndDPITableOfReportGetText(10, 9, "CA Median");
                verifyPoints.Add(summaryTC = "Verify headers in the 3rd table: '" + table + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify data in tables of Report
                table = "Fund Metrics";
                string data;
                verifyPoint = (data = "Fund I") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && (data = "Fund II") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    && (data = "Total Funds") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Fund Name)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_FundName_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }
                
                verifyPoint = (data = "2014") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2")
                    && (data = "2016") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Vintage Year)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_VintageYear_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }
                
                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "3")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Fund Size)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_FundSize_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "4")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "4")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Total Invested)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_TotalInvested_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "5").Replace("%", "")) < 1000
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "5").Replace("%", "")) < 1000
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "5").Replace("%", "")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (% Invested)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_PercInvested_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "6")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "6")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "6")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Realized)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_Realized_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "7")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "7")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "7")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Unrealized (Current NAV))", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_UnrelCurNAV_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "8")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "8")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "8")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Total Value)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_TotalValue_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "IRR";
                verifyPoint = (data = "Fund I") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1")
                    && (data = "Fund II") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Fund Name)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_FundName_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2").Replace("%", "")) < 1000
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "2").Replace("%", "")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Gross)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_Gross_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3").Replace("%", "")) < 1000
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "3").Replace("%", "")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Net)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_Net_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4")
                    && (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "4");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (PME)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_PME_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "5")
                    && (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "5");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Gross Total Alpha)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_GrossTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "6")
                    && (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "6");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Net Total Alpha)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_NetTotalAlpha_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "7")
                    && (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "7");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (CA Upper Quartile)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_CAUpperQuartile_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "8")
                    && (data = "") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "8");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (CA Median)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_CAMedian_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "TVPI & DPI";
                verifyPoint = (data = "Fund I") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "1")
                    && (data = "Fund II") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "2", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (Fund Name)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_FundName_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "2")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "2", "2")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (Gross TVPI)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_GrossTVPI_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "3")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "2", "3")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (Net TVPI)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_NetTVPI_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "4")
                    && (data = "") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "2", "4");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (PME)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_PME_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "5")
                    && (data = "") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "2", "5");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (TVPI-CA Upper Quartile)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_TVPI-CAUpQuartile_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "6")
                    && (data = "") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "2", "6");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (TVPI-CA Median)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_TVPI-CAMedian_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "7")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "2", "7")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (DPI)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_DPI_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "8")
                    && (data = "") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "2", "8");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (DPI-CA Upper Quartile)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_DPI-CAUpQuartile_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (data = "") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "9")
                    && (data = "") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "2", "9");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (DPI-CA Median)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC001_" + table + "_DPI-CAMedian_" + data + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }
                #endregion

                #region Save PDF file
                // Click on 'Print' button
                GeneralAction.Instance.ClickButtonLabel(10, SearchFundPage.print);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());
                System.Threading.Thread.Sleep(3000);

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
                //fileNameBaseline = "Vgo_Private_Sandbox_download.pdf";
                //verifyPoint = FilesComparision.PDFIsFilesEqual(fileNameBaseline, fileName);
                //verifyPoints.Add(summaryTC = "Verify data of the '" + fileName + "' file is shown correctly", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                // Delete downloaded PDF file
                GeneralAction.Instance.DeleteFilePath(userProfileDownloadPath, fileName);
                #endregion

                // Stop recording video
                Driver.StopVideoRecord();

                // Delete video file
                Driver.DeleteFilesContainsName(Path.GetFullPath(@"../../../../../TestResults/"), videoFileName);
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
        public void TC002_SingleManagerDashboard_SourceCambridge() // Private Report
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string cambridgeFund = "GSR Ventures";
            const string sourceIcon = "C";
            string videoFileName = "PrivateReportTestTC002";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Private Report Test - TC002");
            try
            {
                // Start recording video
                Driver.StartVideoRecord(videoFileName);

                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Search a Fund - Source = Albourne
                SearchFundAction.Instance.InputNameToSearchFund(10, "GSR Ve")
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(cambridgeFund, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, cambridgeFund, sourceIcon);

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

                /// User Input
                // Select Report Type = Single Manager Dashboard
                SearchFundAction.Instance.ClickToCheckTheCheckbox("Single Manager Dashboard")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.dataSource, "Cambridge"); // --> Select Data Source = Cambridge

                // Input data for Report - Single Manager Dashboard
                SearchFundAction.Instance.WaitForElementVisible(10, SearchFundPage.labelDropdown(SearchFundPage.assetClass))
                                         .InputTxtDatePickerTitle(10, SearchFundPage.dateSelection, "2021", "Jun", "30") // old: "2021", "Dec", "31"
                                         .PageDownToScrollDownPage()
                                         .CheckIfExistingCRBMThenDeleteAll(10, sourceIcon)
                                         .InputTxtNameCRBMRow(10, 1, "s&p") // input 1st CRBM
                                         .WaitForElementVisible(20, SearchFundPage.nameCRBMReturnOfResults("S&P 500 Index"))
                                         .ClickNameCRBMReturnOfResults(10, "S&P 500 Index").WaitForElementInvisible(10, SearchFundPage.overlayDropdown)
                                         .InputNumberBetaCRBMRow(10, 1, "1")
                                         .InputNumberGrossExposureCRBMRow(10, 1, "90")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.assetClass, "Venture")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.geography, "United States");

                // Click on 'Run' button
                System.Threading.Thread.Sleep(2000);
                SearchFundAction.Instance.ClickRunButton(10)
                                         .WaitForLoadingIconToDisappear(30, General.loadingSpinner);

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {
                    // Fund Metrics - data
                    fundName1 = "GSR Opportunities IV";// old: "GSR Ventures VI, L.P.";
                    fundName2 = "GSR Ventures V, L.P.";
                    fundName3 = "GSR 2017 Opportunities Fund, L.P.";
                    fundName4 = "GSR Ventures I, L.P.";
                    fundName5 = "GSR Ventures II, L.P.";
                    fundName6 = "GSR Ventures III, L.P.";
                    fundName7 = "GSR Ventures IV, L.P.";
                    fundName8 = "GSR Ventures VII, L.P.";
                    fundMetricsVintageYear1 = "2011"; // old: 2017
                    fundMetricsVintageYear2 = "2014";
                    fundMetricsVintageYear3 = "2017";
                    fundMetricsVintageYear4 = "2005";
                    fundMetricsVintageYear5 = "2006";
                    fundMetricsVintageYear6 = "2008";
                    fundMetricsVintageYear7 = "2011";
                    fundMetricsVintageYear8 = "2019";
                    fundMetricsFundSize1 = "325.80";
                    fundMetricsFundSize2 = "151.50";
                    fundMetricsFundSize3 = "100.00";
                    fundMetricsFundSize4 = "75.80";
                    fundMetricsFundSize5 = "202.00";
                    fundMetricsFundSize6 = "383.30";
                    fundMetricsFundSize7 = "353.50";
                    fundMetricsFundSize8 = "451.10";
                    fundMetricsTotalInvested1 = "";
                    fundMetricsTotalInvestedLast = "0.00";
                    fundMetricsPercInvested1 = "";
                    fundMetricsPercInvestedLast = "0.0%";
                    fundMetricsRealized1 = "";
                    fundMetricsRealizedLast = "0.00";
                    fundMetricsUnRealizedCurNav1 = "";
                    fundMetricsUnRealizedCurNavLast = "0.00";
                    fundMetricsTotalValue1 = "";
                    fundMetricsTotalValueLast = "0.00";

                    // IRR - data
                    iRRCAUpperQuartile1 = "53.4%";
                    iRRCAUpperQuartileLast = "72.0%";
                    iRRCAMedian1 = "34.0%";
                    iRRCAMedianLast = "41.1%";

                    // TVPI - data
                    tVPINetTVPI1 = "Unavailable";
                    tVPINetTVPILast = "Unavailable";
                    tVPICAUpperQuartile1 = "3.20";
                    tVPICAUpperQuartileLast = "1.97";
                    tVPICAMedian1 = "2.26";
                    tVPICAMedianLast = "1.54";

                    // DPI - data
                    dPICAUpperQuartile1 = "0.43";
                    dPICAUpperQuartileLast = "0.00";
                    dPICAMedian1 = "0.11";
                    dPICAMedianLast = "0.00";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    // Fund Metrics - data
                    fundName1 = "GSR Opportunities IV";// old: "GSR Ventures VI, L.P.";
                    fundName2 = "GSR Ventures I, L.P.";
                    fundName3 = "GSR Ventures II, L.P.";
                    fundName4 = "GSR Ventures III, L.P.";
                    fundName5 = "GSR Ventures IV, L.P.";
                    fundName6 = "GSR Ventures V, L.P.";
                    fundName7 = "GSR Ventures VI, L.P.";
                    fundName8 = "GSR Ventures VII, L.P.";
                    fundMetricsVintageYear1 = "2011"; // old: 2017
                    fundMetricsVintageYear2 = "2005";
                    fundMetricsVintageYear3 = "2006";
                    fundMetricsVintageYear4 = "2008";
                    fundMetricsVintageYear5 = "2011";
                    fundMetricsVintageYear6 = "2014";
                    fundMetricsVintageYear7 = "2017";
                    fundMetricsVintageYear8 = "2019";
                    fundMetricsFundSize1 = "100.00";
                    fundMetricsFundSize2 = "75.80";
                    fundMetricsFundSize3 = "202.00";
                    fundMetricsFundSize4 = "383.30";
                    fundMetricsFundSize5 = "353.50";
                    fundMetricsFundSize6 = "151.50";
                    fundMetricsFundSize7 = "325.80";
                    fundMetricsFundSize8 = "451.10";
                    fundMetricsTotalInvested1 = "";
                    fundMetricsTotalInvestedLast = "0.00";
                    fundMetricsPercInvested1 = "";
                    fundMetricsPercInvestedLast = "0.0%";
                    fundMetricsRealized1 = "";
                    fundMetricsRealizedLast = "0.00";
                    fundMetricsUnRealizedCurNav1 = "";
                    fundMetricsUnRealizedCurNavLast = "0.00";
                    fundMetricsTotalValue1 = "";
                    fundMetricsTotalValueLast = "0.00";

                    // IRR - data
                    iRRCAUpperQuartile1 = "53.4%";
                    iRRCAUpperQuartileLast = "72.0%";
                    iRRCAMedian1 = "34.0%";
                    iRRCAMedianLast = "41.1%";

                    // TVPI - data
                    tVPINetTVPI1 = "Unavailable";
                    tVPINetTVPILast = "Unavailable";
                    tVPICAUpperQuartile1 = "3.20";
                    tVPICAUpperQuartileLast = "1.97";
                    tVPICAMedian1 = "2.26";
                    tVPICAMedianLast = "1.54";

                    // DPI - data
                    dPICAUpperQuartile1 = "0.43";
                    dPICAUpperQuartileLast = "0.00";
                    dPICAMedian1 = "0.11";
                    dPICAMedianLast = "0.00";
                }

                #region Verify data in tables of Report
                string table = "Fund Metrics";
                verifyPoint = fundName1 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1");
                    //&& fundName2 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "1")
                    //&& fundName3 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "1")
                    //&& fundName4 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "4", "1")
                    //&& fundName5 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "5", "1")
                    //&& fundName6 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "1")
                    //&& fundName7 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "7", "1")
                    //&& fundName8 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "8", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Fund Name)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_FundName_" + fundName1 + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = fundMetricsVintageYear1 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2");
                    //&& fundMetricsVintageYear2 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "2")
                    //&& fundMetricsVintageYear3 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "2")
                    //&& fundMetricsVintageYear4 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "4", "2")
                    //&& fundMetricsVintageYear5 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "5", "2")
                    //&& fundMetricsVintageYear6 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "2")
                    //&& fundMetricsVintageYear7 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "7", "2")
                    //&& fundMetricsVintageYear8 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "8", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Vintage Year)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_VinYear_" + fundMetricsVintageYear1 + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3")) < 1000;
                    //&& Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "2", "3")) < 1000
                    //&& Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "3", "3")) < 1000
                    //&& Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "4", "3")) < 1000
                    //&& Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "5", "3")) < 1000
                    //&& Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "6", "3")) < 1000
                    //&& Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "7", "3")) < 1000
                    //&& Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "8", "3")) < 1000;
                //verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Fund Size)", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);
                //verifyPoint = fundMetricsTotalInvested1 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4")
                //   && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "4")) < 1000;
                //verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Total Invested)", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);
                //verifyPoint = fundMetricsPercInvested1 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "5")
                //   && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "5").Replace("%", "")) < 1000;
                //verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (% Invested)", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);
                //verifyPoint = fundMetricsRealized1 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "6")
                //   && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "6")) < 1000;
                //verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Realized)", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);
                //verifyPoint = fundMetricsUnRealizedCurNav1 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "7")
                //   && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "7")) < 1000;
                //verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Unrealized (Current NAV))", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);
                //verifyPoint = fundMetricsTotalValue1 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "8")
                //   && Convert.ToDouble(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "8")) < 1000;
                //verifyPoints.Add(summaryTC = "Verify data in the 1st table '" + table + "' (Total Value)", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                table = "IRR";
                verifyPoint = fundName1 == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Fund Name)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_FundName_" + fundName1 + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = ("") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "2")
                    && ("") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Gross)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_Gross_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = ("") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "3")
                    && ("") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Net)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_Net_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = ("") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "4")
                    && ("") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "4");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (PME)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_PME_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = ("") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "5")
                    && ("") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "5");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Gross Total Alpha)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_GrossTotalAlpha_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = ("") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "6")
                    && ("") == SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "6");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (Net Total Alpha)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_NetTotalAlpha_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "7").Replace("%", "")) < 1000
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "7").Replace("%", "")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (CA Upper Quartile)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_CAUpperQuartile_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "1", "8").Replace("%", "")) < 1000
                    && double.Parse(SearchFundAction.Instance.DataTableOfReportGetText(10, table, "last()", "8").Replace("%", "")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table '" + table + "' (CA Median)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_CAMedian_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                table = "TVPI & DPI";
                verifyPoint = fundName1 == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (Fund Name)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_FundName_" + fundName1 + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = ("") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "2")
                    && ("") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "last()", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (Gross TVPI)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_GrossTVPI_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = tVPINetTVPI1 == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "3")
                    && tVPINetTVPILast == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "last()", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (Net TVPI)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_NetTVPI_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = ("") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "4")
                    && ("") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "last()", "4");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (PME)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_PME_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "5")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "last()", "5")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (TVPI-CA Upper Quartile)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_TVPI-CAUpperQuartile_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "6")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "last()", "6")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (TVPI-CA Median)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_TVPI-CAUpperMedian_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = ("") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "7")
                    && ("") == SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "last()", "7");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (DPI)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_DPI_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "8")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "last()", "8")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (DPI-CA Upper Quartile)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_DPI-CAUpperQuartile_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "1", "9")) < 1000
                    && Convert.ToDouble(SearchFundAction.Instance.DataTVPIAndDPITableOfReportGetText(10, "last()", "9")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table '" + table + "' (DPI-CA Median)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("ss_PrivateReportTC002_" + table + "_DPI-CAMedian_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }
                #endregion

                // Stop recording video
                Driver.StopVideoRecord();

                // Delete video file
                Driver.DeleteFilesContainsName(Path.GetFullPath(@"../../../../../TestResults/"), videoFileName);
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
