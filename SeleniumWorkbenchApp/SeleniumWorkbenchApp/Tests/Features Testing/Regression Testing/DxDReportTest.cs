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
    [TestFixture, Order(8)]
    internal class DxDReportTest : BaseTestCase
    {
        #region Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;
        #endregion

        [Test, Category("Regression Testing")]
        public void TC001_DxDReport()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string cambridgeFund = "VGO Capital Partners";
            const string sourceIcon = "C";
            const string asOfDate = "Effective Date*";
            string userProfileDownloadPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
            string fileName = @"download.pdf";
            string? fileNameBaseline = null;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - DxD Report Test - TC001");
            try
            {
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

                /// User Input
                // Select Report Type = Single Manager Dashboard
                System.Threading.Thread.Sleep(1000);
                SearchFundAction.Instance.ClickToCheckTheCheckbox("Deal by Deal")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.dataSource, "Manual"); // --> Select Data Source = Manual

                // Verify label of fields in Single Manager Dashboard
                verifyPoint = asOfDate == SearchFundAction.Instance.IsDatePickerLabelShown(10);
                verifyPoints.Add(summaryTC = "Verify label date-picker of '" + SearchFundPage.dateSelection + "'is shown correctly: '" + asOfDate + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Input data for Report - Deal by Deal
                SearchFundAction.Instance.WaitForElementVisible(10, SearchFundPage.labelDropdown(SearchFundPage.weightingMethodology))
                                         .InputTxtDatePickerTitle(10, SearchFundPage.dateSelection, "2021", "Jun", "30") //.InputTxtDatePickerLabel(10, dateSelection, "2022", "Jun", "30")
                                         .CheckIfExistingCRBMThenDeleteAll(10, sourceIcon)
                                         .InputTxtNameCRBMRow(10, 1, "s&p") // input 1st CRBM
                                         .WaitForElementVisible(20, SearchFundPage.nameCRBMReturnOfResults("S&P 500 Index"))
                                         .ClickNameCRBMReturnOfResults(10, "S&P 500 Index").WaitForElementInvisible(10, SearchFundPage.overlayDropdown)
                                         .InputNumberBetaCRBMRow(10, 1, "-0.5")
                                         .InputNumberGrossExposureCRBMRow(10, 1, "80")
                                         .ClickCRBMAddButton(10) // Add 2nd CRBM
                                         .InputTxtNameCRBMRow(10, 2, "msci")
                                         .WaitForElementVisible(20, SearchFundPage.nameCRBMReturnOfResults("MSCI Emerging Markets Latin America ex Brazil Net TR Index (USD)"))
                                         .ClickNameCRBMReturnOfResults(10, "MSCI Emerging Markets Latin America ex Brazil Net TR Index (USD)").WaitForElementInvisible(10, SearchFundPage.overlayDropdown)
                                         .InputNumberBetaCRBMRow(10, 2, "1.5")
                                         .InputNumberGrossExposureCRBMRow(10, 2, "-4.5")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.weightingMethodology, "Percent Fund Final")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.attributionCategory1, "% of Fund")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.attributionCategory2, "Equal Weight")
                                         .ClickAndSelectItemInDropdown(10, SearchFundPage.attributionCategory3, "Invested Capital");

                // Click on 'Run' button
                System.Threading.Thread.Sleep(2000);
                SearchFundAction.Instance.ClickRunButton(10)
                                         .WaitForLoadingIconToDisappear(30, General.loadingSpinner);

                #region Verify headers in tables of Report
                    // Manager Info table
                    verifyPoint = "Manager Name" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.managerInfoHeader, 1)
                    && "VGO Capital Partners" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.managerInfoHeader, 2);
                verifyPoints.Add(summaryTC = "Verify headers in the 1st table: '" + SearchFundPage.managerInfoHeader + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Custom Risk Benchmark table
                verifyPoint = "Index Name" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.crbmHeader, 1)
                    && "Beta" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.crbmHeader, 2)
                    && "Exposure" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.crbmHeader, 3);
                verifyPoints.Add(summaryTC = "Verify headers in the 2nd table: '" + SearchFundPage.crbmHeader + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Fund table
                verifyPoint = "Fund Name" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.fundTableHeader, 1)
                    && "Vintage Year" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.fundTableHeader, 2)
                    && "Fund Size" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.fundTableHeader, 3)
                    && "$ Invested" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.fundTableHeader, 4)
                    && "EST GROSS IRR" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.fundTableHeader, 5)
                    && "Gross TVPI" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.fundTableHeader, 6)
                    && "Loss Ratio" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.fundTableHeader, 7);
                verifyPoints.Add(summaryTC = "Verify headers in the 3rd table: '" + SearchFundPage.fundTableHeader + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Summary Results table --> No header column

                // Results table
                verifyPoint = "" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.resultTableHeader, 1)
                    && "Filtered Deals" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.resultTableHeader, 2)
                    && "All Deals" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.resultTableHeader, 3);
                verifyPoints.Add(summaryTC = "Verify headers in the 4th table: '" + SearchFundPage.resultTableHeader + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // GTA table
                verifyPoint = "Summary of deals by GTA" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.summaryOfGTATableHeader, 1)
                    && "As % of Deals" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.summaryOfGTATableHeader, 2)
                    && "As % of Capital" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.summaryOfGTATableHeader, 3);
                verifyPoints.Add(summaryTC = "Verify headers in the 5th table: '" + SearchFundPage.summaryOfGTATableHeader + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Attribution 1 Result table
                verifyPoint = "" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.attributionTableHeader(1), 1)
                    && "GTA (Actual Dates)" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.attributionTableHeader(1), 2)
                    && "GTA (Time Zero)" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.attributionTableHeader(1), 3)
                    && "% of Invest Capital" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.attributionTableHeader(1), 4);
                verifyPoints.Add(summaryTC = "Verify headers in the 6th table: '" + SearchFundPage.attributionTableHeader(1) + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Attribution 2 & 3 Result table --> are the same as the Attribution 1

                // Base table
                /// Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                { 
                    verifyPoint = "Company Name" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 1)
                    && "Fund" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 2)
                    && "Entry Date" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 3)
                    && "Exit Date" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 4)
                    && "Status" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 5)
                    && "Gross IRR" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 6)
                    && "Invested Capital" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 7)
                    && "Realized Capital" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 8)
                    && "Unrealized FMV" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 9)
                    && "Attribution Category 1" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 10)
                    && "Attribution Category 2" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 11)
                    && "Attribution Category 3" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 12)
                    && "Custom Weight" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 13)
                    && "Actual Weight" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 14)
                    && "Gross TVPI" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 15)
                    && "Final Gross IRR" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 16)
                    && "PME" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 17)
                    && "GTA" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 18);
                    verifyPoints.Add(summaryTC = "Verify headers in the 7th table: '" + SearchFundPage.baseTableHeader + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                }
                if (urlInstance.Contains("conceptia"))
                {
                    verifyPoint = "Company Name" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 1)
                    && "Fund" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 2)
                    && "Entry Date" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 3)
                    && "Exit Date" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 4)
                    && "Status" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 5)
                    && "Gross IRR" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 6)
                    && "Invested Capital" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 7)
                    && "Realized Capital" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 8)
                    && "Unrealized FMV" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 9)
                    && "Attribution Category 1" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 10)
                    && "Attribution Category 2" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 11)
                    && "Custom Weight" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 12)
                    && "Actual Weight" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 13)
                    && "Gross TVPI" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 14)
                    && "Final Gross IRR" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 15)
                    && "PME" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 16)
                    && "GTA" == SearchFundAction.Instance.HeadertableDxDGetText(10, SearchFundPage.baseTableHeader, 17);
                    verifyPoints.Add(summaryTC = "Verify headers in the 7th table: '" + SearchFundPage.baseTableHeader + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                }
                #endregion

                #region Verify data in tables of Report
                    string data;
                // Manager Info table
                verifyPoint = "Currency" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.managerInfoHeader, "1", "1")
                    && "Effective Date" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.managerInfoHeader, "2", "1")
                    && "Weight" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.managerInfoHeader, "3", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table: '" + SearchFundPage.managerInfoHeader + "' - Manager Name", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = "USD" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.managerInfoHeader, "1", "2")
                    && "06/30/2021" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.managerInfoHeader, "2", "2")
                    && "Percent Fund Final" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.managerInfoHeader, "3", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 1st table: '" + SearchFundPage.managerInfoHeader + "' - '" + cambridgeFund + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Custom Risk Benchmark table
                verifyPoint = "S&P 500 Index" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.crbmHeader, "1", "1")
                    && "MSCI Emerging Markets Latin America ex Brazil Net TR Index (USD)" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.crbmHeader, "2", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table: '" + SearchFundPage.crbmHeader + "' - Index Name", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = "-0.5" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.crbmHeader, "1", "2")
                    && "1.5" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.crbmHeader, "2", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table: '" + SearchFundPage.crbmHeader + "' - Beta", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = "80.0%" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.crbmHeader, "1", "3")
                    && "(4.5%)" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.crbmHeader, "2", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 2nd table: '" + SearchFundPage.crbmHeader + "' - Exposure", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Fund table
                verifyPoint = "Fund I" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.fundTableHeader, "1", "1")
                    && "Fund II" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.fundTableHeader, "2", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table: '" + SearchFundPage.fundTableHeader + "' - Fund Name", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = "2014" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.fundTableHeader, "1", "2")
                    && "2016" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.fundTableHeader, "2", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table: '" + SearchFundPage.fundTableHeader + "' - Vintage Year", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.fundTableHeader, "1", "3")) < 400
                    && Convert.ToDouble(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.fundTableHeader, "2", "3")) < 400;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table: '" + SearchFundPage.fundTableHeader + "' - Fund Size", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.fundTableHeader, "1", "4")) < 400
                    && Convert.ToDouble(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.fundTableHeader, "2", "4")) < 400;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table: '" + SearchFundPage.fundTableHeader + "' - $ Invested", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.fundTableHeader, "1", "5").Replace("%", "")) < 100
                    && double.Parse(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.fundTableHeader, "2", "5").Replace("%", "")) < 100;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table: '" + SearchFundPage.fundTableHeader + "' - EST GROSS IRR", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = Convert.ToDouble(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.fundTableHeader, "1", "6")) < 100
                    && Convert.ToDouble(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.fundTableHeader, "2", "6")) < 100;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table: '" + SearchFundPage.fundTableHeader + "' - Gross TVPI", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.fundTableHeader, "1", "7").Replace("%", "")) < 100
                    && double.Parse(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.fundTableHeader, "2", "7").Replace("%", "")) < 100;
                verifyPoints.Add(summaryTC = "Verify data in the 3rd table: '" + SearchFundPage.fundTableHeader + "' - Loss Ratio", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Summary Results table
                verifyPoint = "Gross IRR" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryTableHeader, "1", "1")
                    && "PME" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryTableHeader, "2", "1")
                    && "Gross Total Alpha" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryTableHeader, "3", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 4th table: '" + SearchFundPage.summaryTableHeader + "' - 1st column", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                /// Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab")) 
                {
                    verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryTableHeader, "1", "last()").Replace("%", "")) < 100;
                    //&& double.Parse(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryTableHeader, "2", "last()").Replace("%", "").Replace("(", "").Replace(")", "")) < 100
                    //&& double.Parse(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryTableHeader, "3", "last()").Replace("%", "")) < 100;
                    verifyPoints.Add(summaryTC = "Verify data in the 4th table: '" + SearchFundPage.summaryTableHeader + "' - last column", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                }
                if (urlInstance.Contains("conceptia"))
                {
                    verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryTableHeader, "1", "last()").Replace("%", "")) < 100;
                    //&& double.Parse(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryTableHeader, "2", "last()").Replace("%", "").Replace("(", "").Replace(")", "")) < 100
                    //&& double.Parse(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryTableHeader, "3", "last()").Replace("%", "")) < 100;
                    verifyPoints.Add(summaryTC = "Verify data in the 4th table: '" + SearchFundPage.summaryTableHeader + "' - last column", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                }

                // Results (Excl. Deals <3 yrs <50% Realized) table
                verifyPoint = "Deals Positive GTA" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.resultTableHeader, "1", "1")
                    && "Deals Negative GTA" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.resultTableHeader, "2", "1")
                    && "Deals Lost Money" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.resultTableHeader, "3", "1")
                    && "Capital Loss" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.resultTableHeader, "4", "1")
                    && "Capital Written Down" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.resultTableHeader, "5", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 5th table: '" + SearchFundPage.resultTableHeader + "' - 1st column", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.resultTableHeader, "1", "2").Replace("%", "")) < 400;
                    verifyPoints.Add(summaryTC = "Verify data in the 5th table: '" + SearchFundPage.resultTableHeader + "' - Filtered Deals (Deals Positive GTA): '" + (data="<400") + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.resultTableHeader, "1", "3").Replace("%", "")) < 400;
                verifyPoints.Add(summaryTC = "Verify data in the 5th table: '" + SearchFundPage.resultTableHeader + "' - All Deals (Deals Positive GTA): '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // GTA table
                verifyPoint = "Less than -30%" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryOfGTATableHeader, "1", "1")
                    && "-30% to -15%" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryOfGTATableHeader, "2", "1")
                    && "-15% to -0%" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryOfGTATableHeader, "3", "1")
                    && "-0% to 15%" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryOfGTATableHeader, "4", "1")
                    && "15% to 30%" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryOfGTATableHeader, "5", "1")
                    && "30% to 45%" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryOfGTATableHeader, "6", "1")
                    && "45% to 60%" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryOfGTATableHeader, "7", "1")
                    && "60% to above" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryOfGTATableHeader, "8", "1")
                    && "Total" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryOfGTATableHeader, "9", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 6th table: '" + SearchFundPage.summaryOfGTATableHeader + "' - Summary of deals by GTA", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryOfGTATableHeader, "last()", "2").Replace("%", "")) < 400;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table: '" + SearchFundPage.summaryOfGTATableHeader + "' - As % of Deals (Total): '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.summaryOfGTATableHeader, "last()", "3").Replace("%", "")) < 400;
                verifyPoints.Add(summaryTC = "Verify data in the 6th table: '" + SearchFundPage.summaryOfGTATableHeader + "' - As % of Capital (Total): '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Attribution 1 Result table
                verifyPoint = "Restructurings" == SearchFundAction.Instance.HeadertableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(1), " Restructurings ")
                    && "Special Situations" == SearchFundAction.Instance.HeadertableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(1), " Special Situations ")
                    && "Structured Finance" == SearchFundAction.Instance.HeadertableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(1), " Structured Finance ");
                verifyPoints.Add(summaryTC = "Verify data in the 7th table: '" + SearchFundPage.attributionTableHeader(1) + "' - 1st column", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                /// Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab")) 
                {
                    //verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(1), " Restructurings ", "1").Replace("%", "")) < 200
                    //    && double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(1), " Special Situations ", "1").Replace("%", "")) < 200
                    //    && double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(1), " Structured Finance ", "1").Replace("%", "")) < 200;
                    //verifyPoints.Add(summaryTC = "Verify data in the 7th table: '" + SearchFundPage.attributionTableHeader(1) + "' - GTA (Actual Dates)", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);
                    //verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(1), " Restructurings ", "2").Replace("%", "")) < 200
                    //    && double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(1), " Special Situations ", "2").Replace("%", "")) < 200
                    //    && double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(1), " Structured Finance ", "2").Replace("%", "")) < 200;
                    //verifyPoints.Add(summaryTC = "Verify data in the 7th table: '" + SearchFundPage.attributionTableHeader(1) + "' - GTA (Time Zero)", verifyPoint);
                    //ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(1), " Restructurings ", "3").Replace("%", "")) < 200
                        && double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(1), " Special Situations ", "3").Replace("%", "")) < 200
                        && double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(1), " Structured Finance ", "3").Replace("%", "")) < 200;
                    verifyPoints.Add(summaryTC = "Verify data in the 7th table: '" + SearchFundPage.attributionTableHeader(1) + "' - % of Invest Capital", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                }
                if (urlInstance.Contains("conceptia")) 
                {
                    verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(1), " Restructurings ", "3").Replace("%", "")) < 200
                        && double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(1), " Special Situations ", "3").Replace("%", "")) < 200
                        && double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(1), " Structured Finance ", "3").Replace("%", "")) < 200;
                    verifyPoints.Add(summaryTC = "Verify data in the 7th table: '" + SearchFundPage.attributionTableHeader(1) + "' - % of Invest Capital", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                }

                // Attribution 2 Result table
                verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(2), " Restructurings ", "3").Replace("%", "")) < 200
                    && double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(2), " Special Situations ", "3").Replace("%", "")) < 200
                    && double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.attributionTableHeader(2), " Structured Finance ", "3").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 8th table: '" + SearchFundPage.attributionTableHeader(2) + "' - % of Invest Capital", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Attribution 3 Result table
                /// Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {
                    // Attribution 3 Result table
                    verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.attributionTableHeader(3), "1", "4").Replace("%", "")) < 200;
                    verifyPoints.Add(summaryTC = "Verify data in the 8th table: '" + SearchFundPage.attributionTableHeader(3) + "' - % of Invest Capital: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                }

                // Base table
                string companyName = "Olive";
                verifyPoint = companyName == SearchFundAction.Instance.HeadertableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " Olive ")
                    && "Total" == SearchFundAction.Instance.DatatableDxDGetText(10, SearchFundPage.baseTableHeader, "last()", "1"); // row 24
                verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Company Name", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data= "Fund I") == SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "1");
                verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Fund Name ("+ companyName + "): '"+ data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data= "12/31/2016") == SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "2");
                verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Start Date (Total): '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "3");
                verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - End Date ("+ companyName + "): '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Partial") == SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "4");
                verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Status ("+ companyName + "): '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "5").Replace("%", "")) < 200;
                verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Gross Irr ("+ companyName + "): '" + (data="<100") + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = Convert.ToInt32(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "6")) < 100;
                verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Invested Capital ("+ companyName + "): '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = Convert.ToInt32(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "7")) < 100;
                verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Realized Capital ("+ companyName + "): '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = Convert.ToInt32(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "8")) < 100;
                verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Unrealized Fmv ("+ companyName + "): '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Restructurings") == SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "9");
                verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Attribution Category 1 ("+ companyName +"): '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Restructurings") == SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "10");
                verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Attribution Category 2 ("+ companyName + "): '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                /// Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {

                    verifyPoint = (data = "") == SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "11");
                    verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Attribution Category 3 (" + companyName + "): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = Convert.ToInt32(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "12")) < 100;
                    verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Custom Weight (" + companyName + "): '" + (data="<100") + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "13").Replace("%", "")) < 100;
                    verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Actual Weight (" + companyName + "): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "14").Replace("x", "")) < 100;
                    verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Gross Tvpi (" + companyName + "): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "15").Replace("%", "")) < 100;
                    verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Gross Irr Final (" + companyName + "): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "16").Replace("%", "").Replace("(", "").Replace(")", "")) < 100;
                    verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Final Pme (" + companyName + "): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "17").Replace("%", "")) < 100;
                    verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Deal Gta (" + companyName + "): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                }
                if (urlInstance.Contains("conceptia"))
                {
                    verifyPoint = Convert.ToInt32(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "11")) < 100;
                    verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Custom Weight (" + companyName + "): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "12").Replace("%", "")) < 100;
                    verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Actual Weight (" + companyName + "): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "13").Replace("x", "")) < 100;
                    verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Gross Tvpi (" + companyName + "): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "14").Replace("%", "")) < 100;
                    verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Gross Irr Final (" + companyName + "): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "15").Replace("%", "").Replace("(", "").Replace(")", "")) < 100;
                    verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Final Pme (" + companyName + "): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = double.Parse(SearchFundAction.Instance.DatatableDxDNameXGetText(10, SearchFundPage.baseTableHeader, " " + companyName + " ", "16").Replace("%", "")) < 100;
                    verifyPoints.Add(summaryTC = "Verify data in the 9th table: '" + SearchFundPage.baseTableHeader + "' - Deal Gta (" + companyName + "): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                }
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
                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab")) { fileNameBaseline = "Vgo_DxD_Sandbox_download.pdf"; }
                if (urlInstance.Contains("conceptia")) { fileNameBaseline = "Vgo_DxD_Staging_download.pdf"; }

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
            }
            catch (Exception exception)
            {
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
