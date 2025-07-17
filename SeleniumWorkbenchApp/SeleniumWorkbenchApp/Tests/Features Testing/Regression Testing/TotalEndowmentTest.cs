using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkbenchApp.UITest.Core.BaseTestCase;
using WorkbenchApp.UITest.Core.Selenium;
using WorkbenchApp.UITest.Generals;
using WorkbenchApp.UITest.Pages;

namespace WorkbenchApp.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(2)]
    internal class TotalEndowmentTest : BaseTestCase
    {
        // Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;

        [Test, Category("Regression Testing")]
        public void TC001_TotalEndowment_NavigateToAnotherPage()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Total Endowment Test - TC001");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                #region Return (Public) Link
                // Click on 'Return (Public)' menu
                NavigationAction.Instance.ClickPageNames(10, TotalEndowmentPage.returnPublic);

                // Wait For the new tab
                EvestmentLoginAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementInvisible(60, EvestmentLoginPage.loggingInText)
                                      .WaitForElementInvisible(60, EvestmentLoginPage.loadingIcon);

                // Verify icons/buttons are shown after navigating to a new tab successfully
                verifyPoint = EvestmentLoginAction.Instance.IsLoginButtonShown(60);
                verifyPoints.Add(summaryTC = "Verify the 'Log In' button (in the new tab) is shown after clicking on the '" + TotalEndowmentPage.returnPublic + "' link", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Close the new tab
                Driver.Browser.Close();

                // Switch to the main tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.First());
                #endregion

                #region Return (Private) Link
                // Click on 'Return (Private)' menu
                NavigationAction.Instance.ClickPageNames(10, TotalEndowmentPage.returnPrivate);

                // Wait For the new tab
                EvestmentLoginAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last()); Thread.Sleep(1000);

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementInvisible(60, EvestmentLoginPage.loggingInText)
                                      .WaitForElementInvisible(60, EvestmentLoginPage.loadingIcon);

                // Verify icons/buttons are shown after navigating to a new tab successfully
                verifyPoint = EvestmentLoginAction.Instance.IsLoginButtonShown(30);
                verifyPoints.Add(summaryTC = "Verify the 'Log In' button (in the new tab) is shown after clicking on the '" + TotalEndowmentPage.returnPrivate + "' link", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Close the new tab
                Driver.Browser.Close();

                // Switch to the main tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.First());
                #endregion

                #region Liquidity Link
                // Click on 'Liquidity' menu
                NavigationAction.Instance.ClickPageNames(10, TotalEndowmentPage.liquidity);

                // Wait For the new tab
                EvestmentLoginAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementInvisible(60, EvestmentLoginPage.loggingInText)
                                      .WaitForElementInvisible(60, EvestmentLoginPage.loadingIcon);

                // Verify icons/buttons are shown after navigating to a new tab successfully
                verifyPoint = EvestmentLoginAction.Instance.IsLoginButtonShown(30);
                verifyPoints.Add(summaryTC = "Verify the 'Log In' button (in the new tab) is shown after clicking on the '" + TotalEndowmentPage.liquidity + "' link", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Close the new tab
                Driver.Browser.Close();

                // Switch to the main tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.First());
                #endregion

                #region Total Unfunded Commitments Link
                // Click on 'Total Unfunded Commitments' menu
                NavigationAction.Instance.ClickPageNames(10, "Total Unfunded Commitments");

                // Wait For the new tab
                EvestmentLoginAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementInvisible(60, EvestmentLoginPage.loggingInText)
                                      .WaitForElementInvisible(60, EvestmentLoginPage.loadingIcon);

                // Verify icons/buttons are shown after navigating to a new tab successfully
                verifyPoint = EvestmentLoginAction.Instance.IsLoginButtonShown(30);
                verifyPoints.Add(summaryTC = "Verify the 'Log In' button (in the new tab) is shown after clicking on the 'Total Unfunded Commitments' link", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Close the new tab
                Driver.Browser.Close();

                // Switch to the main tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.First());
                #endregion

                #region Pipeline (Link to Dynamo Software)
                /*
                // Click on 'Pipeline' Page (KS-601 Web portal)
                NavigationAction.Instance.ClickPageNames(10, Navigation.pipeline);

                // Wait For the new tab
                EvestmentLoginAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(60, EvestmentLoginPage.dynamoSoftwarelearnMoreBtn);

                // Verify icons/buttons are shown after navigating to a new tab successfully
                verifyPoint = EvestmentLoginAction.Instance.IsDynamoSoftwarelearnMoreButtonShown(30);
                verifyPoints.Add(summaryTC = "Verify the 'learn More' button (in the new tab - Dynamo Software) is shown after clicking on the '" + Navigation.pipeline + "' link", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Close the new tab
                Driver.Browser.Close();

                // Switch to the main tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.First());*/
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
        public void TC002_TotalEndowment_sort_Asset()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Total Endowment Test - TC002");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Go to Public Fund page
                NavigationAction.Instance.ClickPageNames(10, Navigation.totalEndowment);

                // Wait for Asset & 'Total Endowment' table are displayed
                GeneralAction.Instance.WaitForElementVisible(10, LoginPage.assetTable)
                                      .WaitForElementVisible(10, General.tableHeaderName(TotalEndowmentPage.totalEndowmentHeadertable));

                #region Sort A to Z at column 'Asset'
                // Click sort button at column 'Asset'
                GeneralAction.Instance.ClickSortAssetTableButton(10, General.asset)
                                      .WaitForElementInvisible(10, General.assetTableSortIconStatus(General.asset, General.noSort))
                                      .WaitForElementVisible(10, General.assetTableSortIconStatus(General.asset, General.sortZtoA));

                // Verify sort icon 'Z to A' is shown at the column 'Asset'
                string sortStatus = GeneralAction.Instance.SortIconAssetTableGetStatus(10, General.asset); // get sort icon status
                verifyPoint = sortStatus.Contains(General.sortZtoA); // old: sortAtoZ
                verifyPoints.Add(summaryTC = "Verify sort icon status is shown Z->A: '" + General.sortZtoA + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify data at the column 'Asset' is sorted Z to A
                string title = "Financial Assets"; // KS-666: Renamne "Financial Assets Division" to ""Financial Assets"
                verifyPoint = GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 1).Contains(title);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Absolute Return";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 2, 1);
                verifyPoints.Add(summaryTC = "Verify the 2nd row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Fixed Income";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 3, 1);
                verifyPoints.Add(summaryTC = "Verify the 3rd row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Private Equity";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 4, 1);
                verifyPoints.Add(summaryTC = "Verify the 4th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Public Equities";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 5, 1);
                verifyPoints.Add(summaryTC = "Verify the 5th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Real Assets";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 6, 1);
                verifyPoints.Add(summaryTC = "Verify the 6th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Total Cash";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 7, 1);
                verifyPoints.Add(summaryTC = "Verify the 7th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Sort Z to A at column 'Asset'
                // Click sort button at column 'Asset'
                GeneralAction.Instance.ClickSortAssetTableButton(10, General.asset)
                                      .WaitForElementInvisible(10, General.assetTableSortIconStatus(General.asset, General.sortZtoA))
                                      .WaitForElementVisible(10, General.assetTableSortIconStatus(General.asset, General.sortAtoZ));

                // Verify sort icon 'A to Z' is shown at the column 'Asset'
                sortStatus = GeneralAction.Instance.SortIconAssetTableGetStatus(10, General.asset); // get sort icon status
                verifyPoint = sortStatus.Contains(General.sortAtoZ); // old: sortZtoA
                verifyPoints.Add(summaryTC = "Verify sort icon status is shown A->Z: '" + General.sortZtoA + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify data at the column 'Asset' is sorted A to Z
                title = "Financial Assets"; // KS-666: Renamne "Financial Assets Division" to ""Financial Assets"
                verifyPoint = GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 1).Contains(title);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Total Cash";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 2, 1);
                verifyPoints.Add(summaryTC = "Verify the 2nd row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Real Assets";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 3, 1);
                verifyPoints.Add(summaryTC = "Verify the 3rd row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Public Equities";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 4, 1);
                verifyPoints.Add(summaryTC = "Verify the 4th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Private Equity";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 5, 1);
                verifyPoints.Add(summaryTC = "Verify the 5th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Fixed Income";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 6, 1);
                verifyPoints.Add(summaryTC = "Verify the 6th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Absolute Return";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 7, 1);
                verifyPoints.Add(summaryTC = "Verify the 7th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
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
