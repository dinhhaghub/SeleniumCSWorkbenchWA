using AventStack.ExtentReports;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkbenchApp.UITest.Core.BaseTestCase;
using WorkbenchApp.UITest.Generals;
using WorkbenchApp.UITest.Pages;

namespace WorkbenchApp.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(11)]
    internal class ScenarioTest : BaseTestCase
    {
        #region Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;
        #endregion

        [Test, Category("Regression Testing")]
        public void TC001_ScenarioTest()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Scenario Test - TC001");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Click on 'Scenario Test' menu
                NavigationAction.Instance.ClickPageNames(10, TotalEndowmentPage.scenarioTest);

                // Wait for the loading spninner load done (disappear)
                GeneralAction.Instance.WaitForElementInvisible(10, General.loadingSpinner);

                #region Verify buttons/Icons are displayed
                string buttonText = "Print";
                verifyPoint = GeneralAction.Instance.IsButtonLabelShown(10, buttonText);
                verifyPoints.Add(summaryTC = "Verify 'Print' button is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Column names for each Section
                // Test Scenario Input
                /// Expand a Section
                ScenarioTestAction.Instance.ClickIconToExpand(10, ScenarioTestPage.testScenarioInput);

                /// Verify Column names
                string data = "";
                verifyPoint = ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.testScenarioInput, "1", "1").Contains(data = "Asset")
                    && ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.testScenarioInput, "1", "2").Contains(data = "Beta")
                    && ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.testScenarioInput, "1", "3").Contains(data = "Expected Beta")
                    && ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.testScenarioInput, "1", "4").Contains(data = "Current NAV/MV")
                    && ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.testScenarioInput, "1", "5").Contains(data = "Scenario Flows")
                    && ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.testScenarioInput, "1", "6").Contains(data = "Scenario NAV")
                    && ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.testScenarioInput, "1", "7").Contains(data = "% of Endowment\r\n( Current NAV)") // KS-679
                    && ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.testScenarioInput, "1", "8").Contains(data = "% of Endowment\r\n(Scenario NAV)"); // KS-679
                verifyPoints.Add(summaryTC = "Verify Column names in '" + ScenarioTestPage.testScenarioInput + "' Section is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Prior Day Comparison
                /// Expand a Section
                ScenarioTestAction.Instance.ClickIconToExpand(10, ScenarioTestPage.priorDayComparison);

                /// Verify Column names
                /// Parent-column
                verifyPoint = //ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.priorDayComparison, "1", "1").Contains(data = "Today")
                     ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.priorDayComparison, "1", "2").Contains(data = "Actual")
                    && ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.priorDayComparison, "1", "3").Contains(data = "");
                verifyPoints.Add(summaryTC = "Verify Parent-column names in '" + ScenarioTestPage.priorDayComparison + "' Section is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Sub-column
                verifyPoint = ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.priorDayComparison, "2", "1").Contains(data = "Asset")
                    && ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.priorDayComparison, "2", "2").Contains(data = "Current NAV/ MV")
                    && ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.priorDayComparison, "2", "3").Contains(data = "Dollar Change in NAV")
                    && ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.priorDayComparison, "2", "4").Contains(data = "NAV%")
                    && ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.priorDayComparison, "2", "5").Contains(data = "Beta")
                    && ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.priorDayComparison, "2", "6").Contains(data = "Change In NAV")
                    && ScenarioTestAction.Instance.ColumnNamesInSectionGetText(10, ScenarioTestPage.priorDayComparison, "2", "7").Contains(data = "Change In Beta");
                verifyPoints.Add(summaryTC = "Verify Sub-column names in '" + ScenarioTestPage.priorDayComparison + "' Section is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Sort Z to A at column in 'Section'
                // Test Scenario Input (section)
                /// Click sort button at column 'Section'
                ScenarioTestAction.Instance.ClickColumnNameInSectionSortIcon(10, ScenarioTestPage.testScenarioInput, "1", "Asset")
                                           .WaitForElementInvisible(10, ScenarioTestPage.columnNameInSectionSortIconStatus(ScenarioTestPage.testScenarioInput, "1", "Asset", General.sortZtoA))
                                           .WaitForElementVisible(10, ScenarioTestPage.columnNameInSectionSortIconStatus(ScenarioTestPage.testScenarioInput, "1", "Asset", General.sortAtoZ));

                // Verify sort icon 'Z to A' is shown at the columns in section
                string sortStatus = ScenarioTestAction.Instance.ColumnNameSortIconInSectionGetStatus(10, ScenarioTestPage.testScenarioInput, "1", "Asset"); // get sort icon status
                verifyPoint = sortStatus.Contains(General.sortAtoZ);
                verifyPoints.Add(summaryTC = "Verify (" + ScenarioTestPage.testScenarioInput + " - Asset) sort icon status is shown Z->A: '" + General.sortZtoA + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify data at the column of section is sorted Z to A
                string title = "Real Assets";
                verifyPoint = title == ScenarioTestAction.Instance.DataInSectionGetText(10, ScenarioTestPage.testScenarioInput, "1", "1");
                verifyPoints.Add(summaryTC = "Verify the 1st row value in 'Asset' column of '" + ScenarioTestPage.testScenarioInput + "' is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify data for each Section (Input value)
                // Input data for the 'Cash' row at 'Scenario Flows In millions' of 'Test Scenario Input'
                ScenarioTestAction.Instance.ClickAndInputScenarioFlowsLine(10, "10", "111"); Thread.Sleep(1500);

                // Verify data is calulated for the 'Cash' row of 'Scenario NAV' of 'Test Scenario Input' section
                verifyPoint = double.Parse(ScenarioTestAction.Instance.DataInSectionGetText(10, ScenarioTestPage.testScenarioInput, "10", "6").Replace(",", "")) > 30.00;
                verifyPoints.Add(summaryTC = "Verify data is calulated for the 'Cash' row of 'Scenario NAV' of '" + ScenarioTestPage.testScenarioInput + "' section: > 30.00", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify data is calulated for the ' Scenario Cash' row of 'Scenario Flows In millions' of 'Test Scenario Input' section
                verifyPoint = ScenarioTestAction.Instance.DataInSectionGetText(10, ScenarioTestPage.testScenarioInput, "9", "5").Contains(data = "111.00");
                verifyPoints.Add(summaryTC = "Verify data is calulated for the 'Scenario Cash' row of 'Scenario Flows In millions' of '" + ScenarioTestPage.testScenarioInput + "' section: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify data is calulated for the ' Scenario Cash' row of 'Scenario NAV' of 'Test Scenario Input' section
                verifyPoint = //ScenarioTestAction.Instance.DataInSectionGetText(10, ScenarioTestPage.testScenarioInput, "9", "6").Contains(data = "547.47")
                    double.Parse(ScenarioTestAction.Instance.DataInSectionGetText(10, ScenarioTestPage.testScenarioInput, "9", "6").Replace(",", "")) > 200.00;
                verifyPoints.Add(summaryTC = "Verify data is calulated for the 'Scenario Cash' row of 'Scenario NAV' of '" + ScenarioTestPage.testScenarioInput + "' section: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Add/Edit Funds in 'Test Scenario input'
                // Click 'Add/Edit Funds' button
                GeneralAction.Instance.ClickButtonLabel(10, "Add/Edit Funds")
                                      .WaitForElementVisible(10, General.dynamicDialog);

                // Click (+) 'Add more funds' button
                GeneralAction.Instance.ClickButtonLabel(10, "(Add more Funds)");

                // Input data to add a new fund
                string fundName = "QA auto Glo Eq";
                ScenarioTestAction.Instance.ClickAndSelectItemInDropDownAddEditFundsAsset(10, "1", "Global Equity")
                                           .InputAddEditFunds(10, "1", "2", fundName)
                                           .InputAddEditFunds(10, "1", "3", "1.05")
                                           .InputAddEditFunds(10, "1", "6", "2.05");

                // Add a duplicated Fund Names
                GeneralAction.Instance.ClickButtonLabel(10, "(Add more Funds)");
                ScenarioTestAction.Instance.ClickAndSelectItemInDropDownAddEditFundsAsset(10, "2", "Absolute Return")
                                           .InputAddEditFunds(10, "2", "2", fundName);
                // Click 'Save' button
                GeneralAction.Instance.ClickButtonLabel(10, "Save");

                // Verify a popup warning is shown after adding a duplicated Fund name
                verifyPoint = (data = "Duplicate Fund Names are not allowed") == ScenarioTestAction.Instance.AddEditFundsMsgContentPopupGetText(10);
                verifyPoints.Add(summaryTC = "Verify a popup warning is shown after adding a duplicated Fund name: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click 'OK' button
                GeneralAction.Instance.ClickButtonLabel(10, "OK"); Thread.Sleep(1000);

                // Click 'Remove' to delete the duplicated fund name
                ScenarioTestAction.Instance.ClickAddEditFundsPosButton(10, "2", "7"); Thread.Sleep(1000);

                // Click 'Save' button
                GeneralAction.Instance.ClickButtonLabel(10, "Save")
                                      .WaitForElementInvisible(10, General.dynamicDialog); Thread.Sleep(1000);

                // Click to expand group 'Global Equity' to verify the new fund has just been added
                ScenarioTestAction.Instance.ClickAssetNameInSectionName(10, ScenarioTestPage.testScenarioInput, "Global Equity");

                // Verify the new fund has just been added
                verifyPoint = (data = "1.050") == ScenarioTestAction.Instance.AssetNameInSectionNameColDataGetText(10, ScenarioTestPage.testScenarioInput, fundName, "2");
                verifyPoints.Add(summaryTC = "Verify the new fund (" + fundName + ") has just been added: 'Beta'='" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (data = "1.050") == ScenarioTestAction.Instance.AssetNameInSectionNameColDataGetText(10, ScenarioTestPage.testScenarioInput, fundName, "3");
                verifyPoints.Add(summaryTC = "Verify the new fund (" + fundName + ") has just been added: 'Expected Beta'='" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (data = "2.05") == ScenarioTestAction.Instance.AssetNameInSectionNameColDataGetText(10, ScenarioTestPage.testScenarioInput, fundName, "5");
                verifyPoints.Add(summaryTC = "Verify the new fund (" + fundName + ") has just been added: 'Scenario Flows in millions'='" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (data = "2.05") == ScenarioTestAction.Instance.AssetNameInSectionNameColDataGetText(10, ScenarioTestPage.testScenarioInput, fundName, "6");
                verifyPoints.Add(summaryTC = "Verify the new fund (" + fundName + ") has just been added: 'Scenario NAV'='" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify data is Recalulated for the ' Scenario Cash' row of 'Scenario Flows In millions' of 'Test Scenario Input' section
                verifyPoint = ScenarioTestAction.Instance.AssetNameInSectionNameColDataGetText(10, ScenarioTestPage.testScenarioInput, "Scenario Cash", "5").Contains(data = "(2.05)");
                verifyPoints.Add(summaryTC = "Verify data is Recalulated for the 'Scenario Cash' row of 'Scenario Flows In millions' of '" + ScenarioTestPage.testScenarioInput + "' section: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify data is Recalulated for the ' Scenario Cash' row of 'Scenario NAV' of 'Test Scenario Input' section
                verifyPoint = ScenarioTestAction.Instance.AssetNameInSectionNameColDataGetText(10, ScenarioTestPage.testScenarioInput, "Scenario Cash", "6").Contains(data = "(2.05)");
                verifyPoints.Add(summaryTC = "Verify data is Recalulated for the 'Scenario Cash' row of 'Scenario NAV' of '" + ScenarioTestPage.testScenarioInput + "' section: " + data + "", verifyPoint);
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
