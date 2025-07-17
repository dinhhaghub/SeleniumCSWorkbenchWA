using AventStack.ExtentReports;
using NUnit.Framework;
using System;
using System.Linq;
using WorkbenchApp.UITest.Core.BaseTestCase;
using WorkbenchApp.UITest.Core.Selenium;
using WorkbenchApp.UITest.Generals;
using WorkbenchApp.UITest.Pages;
//using static MongoDB.Driver.WriteConcern;

namespace WorkbenchApp.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(5)]
    internal class SearchFundTest : BaseTestCase
    {
        // Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;

        [Test, Category("Regression Testing")]
        public void TC001_SearchFund_Albourne_FundInfo()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            const string managerName = "Citadel Advisors LLC";
            const string fundName = "Citadel Multi Strategy Funds";
            const string sourceIcon = "A";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Search Fund Test - TC001");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Search a Fund - Source = Albourne
                SearchFundAction.Instance.InputNameToSearchFund(10, "cita")
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(managerName, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, managerName, sourceIcon)
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(fundName, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, fundName, sourceIcon);

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable);

                // Wait for loading icon to disappear
                GeneralAction.Instance.WaitForLoadingIconToDisappear(30, General.loadingSpinner);

                #region Verify Fund Info
                verifyPoint = fundName == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 2, 1);
                verifyPoints.Add(summaryTC = "Verify Fund name (Albourne) is shown correctly after searching: " + fundName + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                string value = "Location: Miami, Florida"; //  Chicago, Illinois
                verifyPoint = value == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 4, 1);
                verifyPoints.Add(summaryTC = "Verify Location (Albourne) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "Multi-Strategy - Diversified") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Strategy (Albourne) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "Europe, United States, Asia, Global") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Geographic Focus (Albourne) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "No") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Hard Lockup (Albourne) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "11/01/1990") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 2, 2);
                verifyPoints.Add(summaryTC = "Verify Inception Date (Albourne) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = double.Parse(SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 3, 2).Replace("%", "")) < 100;
                verifyPoints.Add(summaryTC = "Verify Performance Fee (Albourne) is shown correctly after searching: 'value < 100%'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "1%") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 3, 2);
                verifyPoints.Add(summaryTC = "Verify Management Fee (Albourne) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "Standard") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 3, 2);
                verifyPoints.Add(summaryTC = "Verify High Watermark (Albourne) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Model Parameters (User Input)
                // Click 'Model' menu
                NavigationAction.Instance.ClickPageNames(10, SearchFundPage.model);
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.labelButton(SearchFundPage.userInput));

                // Verify 'User Input' button is shown
                value = "Model Parameters";
                verifyPoint = GeneralAction.Instance.IsButtonLabelShown(10, value);
                verifyPoints.Add(summaryTC = "Verify '" + value + "' button is shown after searching", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click 'User Input' button
                GeneralAction.Instance.ClickButtonLabel(10, value)
                                      .WaitForElementVisible(10, SearchFundPage.userInputPanel);

                // Click to expand User Input (section)
                SearchFundAction.Instance.ClickUserInputSubSection(10, SearchFundPage.dataStatus)
                                         .ClickUserInputSubSection(10, SearchFundPage.dateSection)
                                         .ClickUserInputSubSection(10, SearchFundPage.customRiskBenchmarkModelling)
                                         .ClickUserInputSubSection(10, SearchFundPage.feeModelSection)
                                         .PageDownToScrollDownPage();

                // Check if A red notification is shown then waiting for that is disappeared
                string msg = "This fund does not have Return data. Select another data source or upload the fund data.";
                if (SearchFundAction.Instance.IsElementPresent(SearchFundPage.errorInvalidMessageContent(msg)))
                {
                    SearchFundAction.Instance.WaitForLoadingIconToDisappear(10, SearchFundPage.errorInvalidMessageContent(msg));
                }
                #endregion

                #region Verify Data Status
                // PageUp To scroll up page
                SearchFundAction.Instance.PageUpToScrollDownPage();

                // Verify Data Status (KS-610)
                /// Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                string? rowCount = null, startDate = null, endDate = null;
                if (urlInstance.Contains("lab"))
                {
                    // Upload Source: Manual
                    /// Exposure Manual (index = 1)
                    verifyPoint = (rowCount = "1") == SearchFundAction.Instance.ValueDataStatusGetText(10, 1, SearchFundPage.rowCounts)
                        && (startDate = "04/30/2020") == SearchFundAction.Instance.ValueDataStatusGetText(10, 1, SearchFundPage.startDate)
                        && (endDate = "04/30/2020") == SearchFundAction.Instance.ValueDataStatusGetText(10, 1, SearchFundPage.endDate);
                    verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Manual' - Exposure: '" + rowCount + "'; '" + startDate + "'; '" + endDate + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    /// Verify Fund Returns Manual (index = 2)
                    /// Fund AUM Manual (index = 3)
                    verifyPoint = (rowCount = "285") == SearchFundAction.Instance.ValueDataStatusGetText(10, 3, SearchFundPage.rowCounts)
                        && (startDate = "01/31/1998") == SearchFundAction.Instance.ValueDataStatusGetText(10, 3, SearchFundPage.startDate)
                        && (endDate = "09/30/2021") == SearchFundAction.Instance.ValueDataStatusGetText(10, 3, SearchFundPage.endDate);
                    verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Manual' - Fund AUM: '" + rowCount + "'; '" + startDate + "'; '" + endDate + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Upload Source: Albourne
                    /// Exposure Albourne (index = 6)
                    /// Fund Returns Albourne (index = 7)
                    /// Fund AUM Albourne (index = 8)
                }
                if (urlInstance.Contains("conceptia"))
                {
                    // Upload Source: Manual
                    /// Exposure Manual (index = 1)
                    /// Fund Returns Manual (index = 2)
                    /// Fund AUM Manual (index = 3)

                    // Upload Source: Albourne
                    /// Exposure Albourne (index = 6)
                    ///Fund Returns Albourne(index = 7)
                    /// Fund AUM Albourne (index = 8)
                }

                // Upload Source: Manual
                /// Exposure Manual (index = 1) --> No verify
                /// Fund Returns Manual (index = 2)
                verifyPoint = (rowCount = "314") == SearchFundAction.Instance.ValueDataStatusGetText(10, 2, SearchFundPage.rowCounts)
                        && (startDate = "07/31/1995") == SearchFundAction.Instance.ValueDataStatusGetText(10, 2, SearchFundPage.startDate)
                        && (endDate = "08/31/2021") == SearchFundAction.Instance.ValueDataStatusGetText(10, 2, SearchFundPage.endDate);
                verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Manual' - Fund Returns: '" + rowCount + "'; '" + startDate + "'; '" + endDate + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Fund AUM Manual (index = 3) --> No verify

                // Upload Source: Albourne
                /// Exposure Albourne (index = 6) --> None
                /// Fund Returns Albourne (index = 7)
                verifyPoint = Convert.ToInt64(SearchFundAction.Instance.ValueDataStatusGetText(10, 7, SearchFundPage.rowCounts)) > 100
                    && (startDate = "07/31/1995") == SearchFundAction.Instance.ValueDataStatusGetText(10, 7, SearchFundPage.startDate)
                    && SearchFundAction.Instance.ValueDataStatusGetText(10, 7, SearchFundPage.endDate).Contains(endDate="2025");
                verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Albourne' - Fund Returns: 'RowCounts > 100'; '" + startDate + "'; 'EndDate contains (" + endDate + ")'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                /// Fund AUM Albourne (index = 8)
                verifyPoint = Convert.ToInt64(SearchFundAction.Instance.ValueDataStatusGetText(10, 8, SearchFundPage.rowCounts)) > 100
                    && (startDate = "01/31/1998") == SearchFundAction.Instance.ValueDataStatusGetText(10, 8, SearchFundPage.startDate)
                    && SearchFundAction.Instance.ValueDataStatusGetText(10, 8, SearchFundPage.endDate).Contains(endDate="2024");
                verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Albourne' - Fund AUM: 'RowCounts > 100'; '" + startDate + "'; 'EndDate contains (" + endDate + ")'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
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

        [Test, Category("Regression Testing")]
        public void TC002_SearchFund_AlbourneManual_FundInfo()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            const string managerName = "Prelude Capital";
            const string fundName = "Prelude Structured Alternatives Fund";
            const string sourceIcon = "M";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Search Fund Test - TC002");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Search a Fund - Source = Albourne (Manual)
                SearchFundAction.Instance.InputNameToSearchFund(10, "prelude")
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(managerName, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, managerName, sourceIcon)
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(fundName, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, fundName, sourceIcon);

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable);

                #region Verify Fund Info
                verifyPoint = fundName == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 2, 1);
                verifyPoints.Add(summaryTC = "Verify Fund name (Manual Albourne) is shown correctly after searching: " + fundName + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                string value = "10/31/2013";
                verifyPoint = value == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 2, 2);
                verifyPoints.Add(summaryTC = "Verify Inception Date (Manual Albourne) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
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

        [Test, Category("Regression Testing")]
        public void TC003_SearchFund_Solovis_FundInfo()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            const string managerName = "Laurion Capital Management LP";
            const string fundName = "Laurion Capital Ltd.";
            const string sourceIcon = "S";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Search Fund Test - TC003");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Search a Fund - Source = Solovis
                SearchFundAction.Instance.InputNameToSearchFund(10, "laurion")
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(managerName, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, managerName, sourceIcon)
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(fundName, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, fundName, sourceIcon);

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable);

                #region Verify Fund Info
                verifyPoint = fundName == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 2, 1);
                verifyPoints.Add(summaryTC = "Verify Fund name (Solovis) is shown correctly after searching: " + fundName + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                string value = "Location:";
                verifyPoint = value == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 4, 1);
                verifyPoints.Add(summaryTC = "Verify Location (Solovis) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                value = "Relative Value";
                verifyPoint = value == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Strategy (Solovis) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                value = "";
                verifyPoint = value == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Hard Lockup (Solovis) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                value = "09/01/2005";
                verifyPoint = value == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 2, 2);
                verifyPoints.Add(summaryTC = "Verify Inception Date (Solovis) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                value = "12/29/2020";
                verifyPoint = value == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 2, 2);
                verifyPoints.Add(summaryTC = "Verify KS Inception Date (Solovis) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                value = "20%";
                verifyPoint = value == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 3, 2);
                verifyPoints.Add(summaryTC = "Verify Performance Fee (Solovis) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                value = "2%";
                verifyPoint = value == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 3, 2);
                verifyPoints.Add(summaryTC = "Verify Management Fee (Solovis) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                value = "Y";
                verifyPoint = value == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 3, 2);
                verifyPoints.Add(summaryTC = "Verify High Watermark (Solovis) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify User Input
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

        [Test, Category("Regression Testing")]
        public void TC004_SearchFund_AEvestment_FundInfo()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            const string managerName = "Amber Capital";
            const string fundName = "Amber European Long Opportunities Fund";
            const string sourceIcon = "E";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Search Fund Test - TC004");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Search a Fund - Source = Evestment (Alternative Evestment)
                SearchFundAction.Instance.InputNameToSearchFund(10, "amber")
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(managerName, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, managerName, sourceIcon)
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(fundName, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, fundName, sourceIcon);

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable);

                #region Verify Fund Info
                verifyPoint = fundName == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 2, 1);
                verifyPoints.Add(summaryTC = "Verify Fund name (Alternative Evestment) is shown correctly after searching: " + fundName + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                string value = "07/01/2016";
                verifyPoint = value == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 2, 2);
                verifyPoints.Add(summaryTC = "Verify Inception Date (Alternative Evestment) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "12.5%") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 3, 2);
                verifyPoints.Add(summaryTC = "Verify Performance Fee (Alternative Evestment) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "1%") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 3, 2);
                verifyPoints.Add(summaryTC = "Verify Management Fee (Alternative Evestment) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
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

        [Test, Category("Regression Testing")]
        public void TC005_SearchFund_TEvestment_FundInfo()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            const string managerName = "John Hancock Investments";
            const string fundName = "John Hancock Multimanager Lifetime Portfolios 2035";
            const string sourceIcon = "E";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Search Fund Test - TC005");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Search a Fund - Source = Evestment (Traditional Evestment)
                SearchFundAction.Instance.InputNameToSearchFund(10, managerName)
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(managerName, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, managerName, sourceIcon)
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(fundName, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, fundName, sourceIcon);

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable);

                #region Verify Fund Info
                verifyPoint = fundName == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 2, 1);
                verifyPoints.Add(summaryTC = "Verify Fund name (Traditional Evestment) is shown correctly after searching: " + fundName + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                string value = "10/01/2006";
                verifyPoint = value == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 2, 2);
                verifyPoints.Add(summaryTC = "Verify Inception Date (Traditional Evestment) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "0%") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 3, 2);
                verifyPoints.Add(summaryTC = "Verify Performance Fee (Traditional Evestment) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "0.6%") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 3, 2);
                verifyPoints.Add(summaryTC = "Verify Management Fee (Traditional Evestment) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
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

        [Test, Category("Regression Testing")]
        public void TC006_SearchFund_Cambridge_FundInfo()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string cambridgeFund = "Spectrum Equity";
            const string sourceIcon = "C";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Search Fund Test - TC006");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Search a Fund - Source = Albourne
                SearchFundAction.Instance.InputNameToSearchFund(10, "spectrum eq")
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(cambridgeFund, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, cambridgeFund, sourceIcon);

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable); Thread.Sleep(1000);

                // Check if 'spinner' loading icon is shown then wait for it to disappear
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(10, General.loadingSpinner); Thread.Sleep(1000);
                }

                //// Wait for loading icon to disappear
                //GeneralAction.Instance.WaitForLoadingIconToDisappear(30, General.loadingSpinner);

                #region Verify Fund Info
                // Verify Fund Name
                verifyPoint = cambridgeFund == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 2, 1);
                verifyPoints.Add(summaryTC = "Verify Fund Name (Cambridge) is shown correctly after searching: " + cambridgeFund + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Firm Date (Fund as of date)
                /// Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                string? firmDate = null;
                if (urlInstance.Contains("lab"))
                {
                    firmDate = "June 30, 2023";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    firmDate = "June 30, 2023";
                }
                verifyPoint = firmDate == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 3, 1);
                verifyPoints.Add(summaryTC = "Verify Firm Date (Cambridge) is shown correctly after searching: " + firmDate + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Location
                string value = "Location: One International Place 35th Floor Boston, MA 02110 USA";
                verifyPoint = value == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 4, 1);
                verifyPoints.Add(summaryTC = "Verify Location (Cambridge) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify field names
                verifyPoint = (value = "Strategy:") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 1, 1);
                verifyPoints.Add(summaryTC = "Verify field name 'Strategy' (Cambridge) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "Inception Date:") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 1, 1);
                verifyPoints.Add(summaryTC = "Verify field name 'Inception Date' (Cambridge) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "Geographic Focus:") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 1, 1);
                verifyPoints.Add(summaryTC = "Verify field name 'Geographic Focus' (Cambridge) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "Total # of Funds / Most Recent Fund:") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 2, 1);
                verifyPoints.Add(summaryTC = "Verify field name 'Total # of Funds...Recent Fund' (Cambridge) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "Industry Focus:") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 2, 1);
                verifyPoints.Add(summaryTC = "Verify field name 'Industry Focus' (Cambridge) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify data
                verifyPoint = SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 1, 2).Contains((value = "U.S. Growth Equity"));
                verifyPoints.Add(summaryTC = "Verify data of 'Strategy' (Cambridge) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "1993") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 1, 2);
                verifyPoints.Add(summaryTC = "Verify data of 'Inception Date' (Cambridge) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "U.S. Cross-Region General") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 1, 2);
                verifyPoints.Add(summaryTC = "Verify data of 'Geographic Focus' (Cambridge) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = int.Parse(SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 2, 2).Replace(" ", "")) < 100;
                verifyPoints.Add(summaryTC = "Verify data of 'Total # of Funds...Recent Fund' (Cambridge) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (value = "Media/Communications") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 2, 2)
                    || (value = "Multi Industry") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 2, 2);
                verifyPoints.Add(summaryTC = "Verify data of 'Industry Focus' (Cambridge) is shown correctly after searching: " + value + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Model Parameters (User Input)
                // Click 'Model' menu
                NavigationAction.Instance.ClickPageNames(10, SearchFundPage.model);
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.labelButton(SearchFundPage.userInput));

                // Verify 'Model Parameters' button is shown (old label: User Input)
                value = "Model Parameters";
                verifyPoint = GeneralAction.Instance.IsButtonLabelShown(10, value);
                verifyPoints.Add(summaryTC = "Verify '" + value + "' button is shown after searching", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click 'User Input' button
                GeneralAction.Instance.ClickButtonLabel(10, value)
                                      .WaitForElementVisible(10, SearchFundPage.userInputPanel);
                #endregion

                #region Verify Data Status
                System.Threading.Thread.Sleep(1000); // KS-610
                string rowCount, asOfDate, uploadDate;
                // Upload Source: Manual
                /// Deal Information
                verifyPoint = (rowCount = "84") == SearchFundAction.Instance.ValueDataStatusPrivateGetText(10, 1, SearchFundPage.rowCount)
                    && (asOfDate = "12/31/2021") == SearchFundAction.Instance.ValueDataStatusPrivateGetText(10, 1, SearchFundPage.asOfDate);
                verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Manual' - Deal Information: '" + rowCount + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                /// Fund Information
                verifyPoint = (rowCount = "10") == SearchFundAction.Instance.ValueDataStatusPrivateGetText(10, 2, SearchFundPage.rowCount)
                    && (asOfDate = "12/31/2021") == SearchFundAction.Instance.ValueDataStatusPrivateGetText(10, 2, SearchFundPage.asOfDate);
                verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Manual' - Fund Information: '" + rowCount + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                /// Cash Flow Information

                // Upload Source: Cambridge
                /// Deal Information
                verifyPoint = int.Parse(SearchFundAction.Instance.ValueDataStatusPrivateGetText(10, 6, SearchFundPage.rowCount).Replace(" ", "")) < 1000;
                verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Cambridge' - Deal Information: '" + rowCount + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

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

        [Test, Category("Regression Testing")]
        public void TC007_SearchFund_Pipeline_PublicFundInfo()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            const string managerName = "QA Test Oasis Japan Public Pipeline Manager";
            const string fundName = "QA Test Oasis Japan Public Pipeline Fund 1";
            const string sourceIcon = "P";
            string? fundId = null;
            string? index = null;
            string? feeTermCatchUp = null;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp -Search Fund Test - TC007");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {
                    fundId = "625597952";
                    index = "2";
                    feeTermCatchUp = "Yes";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    fundId = "475124736";
                    index = "1";
                    feeTermCatchUp = "Yes";
                }

                // Search a Pipeline Fund
                SearchFundAction.Instance.InputNameToSearchFund(10, "qa test oasis", managerName, fundName, sourceIcon, index);

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable); Thread.Sleep(1000);

                // Check if 'spinner' loading icon is shown then wait for it to disappear
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(10, General.loadingSpinner); Thread.Sleep(1000);
                }

                #region Verify Fund Info
                string data = fundName;
                verifyPoint = data == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 2, 1);
                verifyPoints.Add(summaryTC = "Verify Fund name (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Location: 129 Cleeland St Dandenong, VIC 3175 Australia") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 4, 1)
                           || (data = "Location: 129 Cleeland Street, Dandenong VIC 3175, Australia") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 4, 1);
                verifyPoints.Add(summaryTC = "Verify Location (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Description " + fundName) == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Strategy (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Developed Markets") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Geographic Focus (Albourne) is shown correctly after searching: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Yes") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Hard Lockup (Albourne) is shown correctly after searching: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 2, 2);
                verifyPoints.Add(summaryTC = "Verify Inception Date (Albourne) is shown correctly after searching: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = double.Parse(SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 3, 2).Replace("%", "")) < 100;
                verifyPoints.Add(summaryTC = "Verify Performance Fee (Albourne) is shown correctly after searching: 'value < 100%'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "21%") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 3, 2);
                verifyPoints.Add(summaryTC = "Verify Management Fee (Albourne) is shown correctly after searching: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Y") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 3, 2);
                verifyPoints.Add(summaryTC = "Verify High Watermark (Albourne) is shown correctly after searching: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify data at 'Fund List' & 'Status' dropdown
                verifyPoint = (data = "3 - Memo") == PipelineAction.Instance.PipelineStatusDropdownGetText(10);
                verifyPoints.Add(summaryTC = "Verify data at 'Pipeline Status' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify data at 'General' section
                verifyPoint = (data = managerName) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundManagerNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Manager' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "129 Cleeland St\r\nDandenong, VIC 3175\r\nAustralia") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessAddress)
                           || (data = "129 Cleeland Street, Dandenong VIC 3175, Australia") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessAddress);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Address' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Hoi An") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactFirstLastNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact First Last' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "test01Cnext@yahoo.com") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactEmail);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact Email' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "112233445566") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessPhone);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Phone' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = fundName) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundNameNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Name' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "General") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundLiquidityTypeNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Liquidity Type' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Description " + fundName) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.descriptionRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Description' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Domestic Equity") == PipelineAction.Instance.LabelDropdownValueGetText(10, PipelinePage.lowestLevelSubAssetClassNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Lowest Level Sub-Asset Class' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Public Equities") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.assetClassNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Asset Class' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Domestic Equity") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel1NoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 1' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Domestic Equity") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel2NoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 2' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Financials") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.sectorRequired, "1");
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Sector' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Developed Markets") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.geographyRequired, "1");
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Geography' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify data at 'Process' section
                verifyPoint = (data = "Ly Nguyen") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryResponsibleNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Primary Responsible' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Thuyen Nguyen") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.secondaryResponsible);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Secondary Responsible' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "03/15/2024") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.targetCloseDate);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Target Close Date' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "03/15/2024") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.docsDueDate);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Docs Due Date' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "19") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundingAmount);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Funding Amount' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Observer") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.lpacSeat);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'LPAC Seat' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "AUD") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.reportingCurrency);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Reporting Currency' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "199") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.closedSize);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Closed Size' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "2024") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.vintageYear);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Vintage Year/ Inception Year' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify data at 'Custom Risk Benchmark and Risk' section
                verifyPoint = (data = "20") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.trackingErrorRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Custom Risk Benchmark and Risk' - 'Tracking Error' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify data at 'Fee Term' section
                verifyPoint = (data = "21") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.managementFeeRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Management Fee' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Monthly") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.managementFeePaidRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Management Fee Paid' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "22") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.performanceFeeRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Performance Fee' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "true") == PipelineAction.Instance.LabelCheckboxFieldGetText(10, PipelinePage.highWaterMark);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'highWaterMark' checkbox is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = feeTermCatchUp) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.catchUpRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Catch Up' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "23") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.catchUpPercAgeIfSoftRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Catch Up Perc Age If Soft' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "1") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.crystallizationEveryXYearsRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Crystallization Every X Years' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "true") == PipelineAction.Instance.LabelCheckboxFieldGetText(10, PipelinePage.hurdleStatus);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Status' checkbox is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Fixed") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.hurdleFixedOrRelativeRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Fixed Or Relative *' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "24") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.hurdleRatePerc);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Rate (%)' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Hard Hurdle") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.hurdleTypeRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Type' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "NAV Dependent") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.rampTypeRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Ramp Type' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify data at 'Liquidity' section
                verifyPoint = (data = "Hard") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.lockupRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Lockup' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "25") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.lockupLengthMonthsRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Lockup Length Months' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Monthly") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.liqudityFrequencyRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Liqudity Frequency' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "26") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.noticeDaysRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Notice Days' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "27") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.investorGateRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Investor Gate' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.softLockupRedemptionFeePerc); // KS-742
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Soft Lockup Redemption Fee %' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "29") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.receiptDaysRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Receipt Days' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "30") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.holdbackRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Holdback' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "31") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.liquidityNote);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Liquidity Note' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "50%") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.sidepocketProbability);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Sidepocket Probability' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "32") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.maxPercOfSidepocketPermitted);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Max % of Sidepocket Permitted' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
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

        [Test, Category("Regression Testing")]
        public void TC008_SearchFund_Pipeline_PrivateFundInfo()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            const string managerName = "QA Test 427 Ventures Private Pipeline Manager";
            string fundName = null;
            const string sourceIcon = "P";
            string? managerId = null;
            string? fundId = null;
            string? index = null;
            string? locationFundInfo = null;
            string? businessAddressGeneral = null;
            string? reportingCurrencyProcess = null;
            string? feeTermCatchUp = null;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp -Search Fund Test - TC008");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {
                    managerId = "c4d7dc07-9e00-42e9-8084-ed78aeb20df4";
                    fundName = "QA Test 427 Ventures Private Pipeline Fund 1 Sandbox";
                    fundId = "573303808";
                    index = "1";
                    locationFundInfo = "Location: 129 Cleeland Street, Dandenong VIC 3175, Australia";
                    businessAddressGeneral = "129 Cleeland Street, Dandenong VIC 3175, Australia";
                    reportingCurrencyProcess = "AUD";
                    feeTermCatchUp = "27";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    managerId = "c4d7dc07-9e00-42e9-8084-ed78aeb20df4";
                    fundName = "QA Test 427 Ventures Private Pipeline Fund 1";
                    fundId = "445422080";
                    index = "1";
                    locationFundInfo = "Location: 129 Cleeland St Dandenong, VIC 3175 Australia";
                    businessAddressGeneral = "129 Cleeland St\r\nDandenong, VIC 3175\r\nAustralia";
                    reportingCurrencyProcess = "";
                    feeTermCatchUp = "27";
                }

                // Search a Pipeline Fund
                SearchFundAction.Instance.InputNameToSearchFund(10, "qa test 427", managerName, fundName, sourceIcon); // index

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable); Thread.Sleep(1000);

                // Check if 'spinner' loading icon is shown then wait for it to disappear
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(10, General.loadingSpinner); Thread.Sleep(1000);
                }

                #region Verify Fund Info
                string data = managerName;
                verifyPoint = data == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 2, 1);
                verifyPoints.Add(summaryTC = "Verify Fund name (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data=locationFundInfo) == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 4, 1)
                    || (data = "Location: 129 Cleeland St Dandenong, VIC 3175 Australia") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 4, 1);
                verifyPoints.Add(summaryTC = "Verify Location (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "US Venture Capital") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Strategy (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                //verifyPoint = (data = "1") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 2, 2);
                //verifyPoints.Add(summaryTC = "Verify 'Total # of Funds” / Most Recent Fund' (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "2017") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Inception Date (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 2, 2);
                verifyPoints.Add(summaryTC = "Verify Industry Focus (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "United States") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Geographic Focus (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify data at 'Fund List' & 'Status' dropdown
                verifyPoint = (data = fundName) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundList);
                verifyPoints.Add(summaryTC = "Verify data at 'Fund List' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "4 - RFA") == PipelineAction.Instance.PipelineStatusDropdownGetText(10);
                verifyPoints.Add(summaryTC = "Verify data at 'Pipeline Status' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify data at 'General' section
                verifyPoint = (data = managerName) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundManagerNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Manager' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = businessAddressGeneral) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessAddress)
                           || (data = "129 Cleeland St Dandenong, VIC 3175 Australia") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessAddress)
                           || (data = "129 Cleeland St\r\nDandenong, VIC 3175\r\nAustralia") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessAddress);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Address' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Kathleen Bui") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactFirstLastNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact First Last' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "kabui@ksbe.edux") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactEmail);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact Email' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "112233445566") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessPhone);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Phone' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = fundName) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundNameNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Name' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Drawdown") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundLiquidityTypeNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Liquidity Type' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Description " + fundName) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.descriptionRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Description' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "US Venture Capital") == PipelineAction.Instance.LabelDropdownValueGetText(10, PipelinePage.lowestLevelSubAssetClassNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Lowest Level Sub-Asset Class' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Private Equity") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.assetClassNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Asset Class' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Venture Capital") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel1NoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 1' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "US Venture Capital") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel2NoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 2' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Health Care") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.sectorRequired, "1");
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Sector' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "United States") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.geographyRequired, "1");
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Geography' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify data at 'Process' section
                verifyPoint = (data = "Ly Nguyen") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryResponsibleNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Primary Responsible' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Thuyen Nguyen") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.secondaryResponsible);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Secondary Responsible' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "03/15/2024") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.targetCloseDate);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Target Close Date' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "03/15/2024") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.docsDueDate);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Docs Due Date' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Full") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.lpacSeatRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'LPAC Seat' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = reportingCurrencyProcess == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.reportingCurrency);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Reporting Currency' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "19") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundingAmount);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Funding Amount' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "18") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.closedSize);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Closed size' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "2017") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.vintageYear);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Vintage Year' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify data at 'Custom Risk Benchmark and Risk' section
                verifyPoint = (data = "20") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.trackingError); // KS-743
                verifyPoints.Add(summaryTC = "Verify data at 'Custom Risk Benchmark and Risk' - 'Tracking Error' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify data at 'Fee Term' section
                verifyPoint = (data = "21") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.mgtFeeDuringInvestmentPeriodRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Mgt Fee During Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Committed Capital") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.mgtFeeDuringInvestmentPeriodOnRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Mgt Fee During Investment Period On' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Yes") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.stepUpDownDuringInvestmentPeriodRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Step Up/Down During Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "22") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.stepUpDownDuringTheInvestmentPeriod);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Step Up/Down During The Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "23") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.mgtFeeFloorDuringInvestmentPeriodRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Mgt Fee Floor During Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "24") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.mgtFeeAfterInvestmentPeriodRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Mgt Fee After Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Invested Capital") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.mgtFeeAfterInvestmentPeriodOnRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Mgt Fee After Investment Period On' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "No") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.stepUpDownAfterInvestmentPeriodRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Step Up/Down After Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.stepUpDownRateAfterInvestmentPeriod);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Step Up/Down Rate After Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.mgtFeeFloorAfterInvestmentPeriod);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Mgt Fee Floor After Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "25") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.gPCarryRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'GP Carry' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Gross") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.carryGrossOrNetRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Carry Gross or Net' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "26") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.preferredReturnIfNoPreferredReturnEnter0PercRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Preferred Return (if no preferred return, enter 0%)' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = feeTermCatchUp) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.catchUpRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Catch Up' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Yes") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.stepupCarryRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Stepup Carry' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "28") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.stepupCarryPercRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Stepup Carry %' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "29") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundTermYears); // KS-738
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Fund Term (Years)' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "30") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.investmentPeriodYears); // KS-738
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Investment Period (Years)' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
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
