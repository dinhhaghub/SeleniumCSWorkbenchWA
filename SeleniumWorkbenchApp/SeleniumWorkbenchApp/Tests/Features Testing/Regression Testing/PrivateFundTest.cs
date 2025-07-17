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
using static System.Windows.Forms.AxHost;

namespace WorkbenchApp.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(4)]
    internal class PrivateFundTest : BaseTestCase
    {
        // Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;

        [Test, Category("Regression Testing")]
        public void TC001_PrivateFund_default_data()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string? datePrivateFund = null;
            string? equityBeta = null;
            string? risk = null;
            string? percLiquidAsset = null;
            string? nav = null;
            string? totalUnfundedCommitments = null;
            string? firstRowDataNAV = null;
            string? sixthRowDataNAV = null;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Private Fund Test - TC001");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Go to Private Fund page
                NavigationAction.Instance.ClickPageNames(10, Navigation.privateFund);

                // Wait for Asset & Private Fund table are displayed
                GeneralAction.Instance.WaitForElementVisible(10, LoginPage.assetTable)
                                       .WaitForElementVisible(10, General.tableHeaderName(PrivateFundPage.privateFundHeadertable));

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {
                    // Data in 'Private Fund' table header
                    datePrivateFund = "2025";
                    equityBeta = "1.23";
                    risk = "14.0%";
                    percLiquidAsset = "61.0%";
                    nav = "3.89 Billion";
                    totalUnfundedCommitments = "1.1 Billion";

                    // Row Values in Asset Table (Overview tab)
                    firstRowDataNAV = "3,172,279,673";
                    sixthRowDataNAV = "721,191,854";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    // Data in 'Private Fund' table header
                    datePrivateFund = "2025";
                    equityBeta = "1.21";
                    risk = "14.0%";
                    percLiquidAsset = "61.5%";
                    nav = "3.88 Billion";
                    totalUnfundedCommitments = "1.1 Billion";

                    // Row Values in Asset Table (Overview tab)
                    firstRowDataNAV = "3,180,751,490";
                    sixthRowDataNAV = "703,849,762";
                }

                #region Verify Data in 'Private Fund' table header
                verifyPoint = GeneralAction.Instance.DateInTableHeaderNameGetText(10, PrivateFundPage.privateFundHeadertable).Contains(datePrivateFund);
                verifyPoints.Add(summaryTC = "Verify date of '" + PrivateFundPage.privateFundHeadertable + "' in the table header is shown: '" + datePrivateFund + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = Convert.ToDouble(GeneralAction.Instance.DataInTableHeaderGetText(10, PrivateFundPage.privateFundHeadertable, Navigation.equityBeta)) > 0.00 
                    && Convert.ToDouble(GeneralAction.Instance.DataInTableHeaderGetText(10, PrivateFundPage.privateFundHeadertable, Navigation.equityBeta)) < 3.00;
                verifyPoints.Add(summaryTC = "Verify data of '" + PrivateFundPage.privateFundHeadertable + "' - '" + Navigation.equityBeta + "' is shown: '" + equityBeta + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PrivateFundPage.privateFundHeadertable, Navigation.risk).Replace("%", "")) > 5.0 
                    && double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PrivateFundPage.privateFundHeadertable, Navigation.risk).Replace("%", "")) < 18.0;
                verifyPoints.Add(summaryTC = "Verify data of '" + PrivateFundPage.privateFundHeadertable + "' - '" + Navigation.risk + "' is shown: '" + risk + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PrivateFundPage.privateFundHeadertable, Navigation.percLiquidAsset).Replace("%", "")) > 1.0 
                    && double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PrivateFundPage.privateFundHeadertable, Navigation.percLiquidAsset).Replace("%", "")) < 300.0;
                verifyPoints.Add(summaryTC = "Verify data of '" + PrivateFundPage.privateFundHeadertable + "' - '" + Navigation.percLiquidAsset + "' is shown: '" + percLiquidAsset + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PrivateFundPage.privateFundHeadertable, Navigation.nav).Replace(" Billion", "")) > 0.01 
                    && double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PrivateFundPage.privateFundHeadertable, Navigation.nav).Replace(" Billion", "")) < 5.00;
                verifyPoints.Add(summaryTC = "Verify data of '" + PrivateFundPage.privateFundHeadertable + "' - '" + Navigation.nav + "' is shown: '" + nav + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PrivateFundPage.privateFundHeadertable, Navigation.totalUnfundedCommitments).Replace(" Million", "").Replace(" Billion", "")) > 0.01
                    && double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PrivateFundPage.privateFundHeadertable, Navigation.totalUnfundedCommitments).Replace(" Million", "").Replace(" Billion", "")) < 1000.00;
                verifyPoints.Add(summaryTC = "Verify data of '" + PrivateFundPage.privateFundHeadertable + "' - '" + Navigation.totalUnfundedCommitments + "' is shown: '" + totalUnfundedCommitments + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Menu titles are shown after login successfully
                //string title = PrivateFundPage.overview; --> KS-671 remove Overview tab
                //verifyPoint = title == GeneralAction.Instance.MenuTitlesGetText(10, 1);
                //verifyPoints.Add(summaryTC = "Verify the 1st menu/tab title is shown after login successfully: '" + title + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                /* Remove these sub tab/menu titles base on ticket KS-510
                title = PrivateFundPage.fee;
                verifyPoint = title == GeneralAction.Instance.MenuTitlesGetText(10, 2);
                verifyPoints.Add(summaryTC = "Verify the 2nd menu/tab title is shown after login successfully: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = PrivateFundPage.peer;
                verifyPoint = title == GeneralAction.Instance.MenuTitlesGetText(10, 3);
                verifyPoints.Add(summaryTC = "Verify the 3rd menu/tab title is shown after login successfully: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = PrivateFundPage.notes;
                verifyPoint = title == GeneralAction.Instance.MenuTitlesGetText(10, 4);
                verifyPoints.Add(summaryTC = "Verify the 4th menu/tab title is shown after login successfully: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = PrivateFundPage.dataStatus;
                verifyPoint = title == GeneralAction.Instance.MenuTitlesGetText(10, 5);
                verifyPoints.Add(summaryTC = "Verify the 5th menu/tab title is shown after login successfully: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); */
                #endregion

                #region Verify Column Names in Asset Table (Overview tab)
                // Click on Overview tab
                //NavigationAction.Instance.ClickPageNames(10, PrivateFundPage.overview);

                /* Verify Add Filter button is shown after login successfully // --> Remove this button (KS-528)
                verifyPoint = GeneralAction.Instance.IsAddFilterButtonShown(10);
                verifyPoints.Add(summaryTC = "Verify Add Filter button is shown after login successfully", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); */

                // Verify Column Names in Asset Table
                string title = "Asset";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "NAV"; // Benchmark --> removed this column
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 2);
                verifyPoints.Add(summaryTC = "Verify the 2nd column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                //title = "Vintage Year"; // KS-551 --> Remove 'Vintage Year'
                //verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 3);
                //verifyPoints.Add(summaryTC = "Verify the 3rd column name in Asset table is shown: '" + title + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                title = "Commitment"; // KS-520
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 3);
                verifyPoints.Add(summaryTC = "Verify the 3th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Unfunded"; // KS-520
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 4);
                verifyPoints.Add(summaryTC = "Verify the 4th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Edge";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 5);
                verifyPoints.Add(summaryTC = "Verify the 5th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Org";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 6);
                verifyPoints.Add(summaryTC = "Verify the 6th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Track Record";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 7);
                verifyPoints.Add(summaryTC = "Verify the 7th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Overall Rating";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 9);
                verifyPoints.Add(summaryTC = "Verify the 8th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Conviction";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 10);
                verifyPoints.Add(summaryTC = "Verify the 9th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Tracking Error";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 11);
                verifyPoints.Add(summaryTC = "Verify the 9th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Gross Manager IR";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 12);
                verifyPoints.Add(summaryTC = "Verify the 10th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Gross Manager Alpha";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 13);
                verifyPoints.Add(summaryTC = "Verify the 11th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Risk Adj Strategy Alpha";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 14);
                verifyPoints.Add(summaryTC = "Verify the 12th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Gross Total Alpha"; 
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 15);
                verifyPoints.Add(summaryTC = "Verify the 13th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Fees"; 
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 16);
                verifyPoints.Add(summaryTC = "Verify the 14th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Liquidity Cost";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 17);
                verifyPoints.Add(summaryTC = "Verify the 15th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Net Total Alpha";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 18);
                verifyPoints.Add(summaryTC = "Verify the 16th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Net IR";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 19);
                verifyPoints.Add(summaryTC = "Verify the 17th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Row Values in Asset Table (Overview tab)
                // Verify row values in Asset column
                verifyPoint = (title = "Private Equity") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Asset column is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify row value in NAV column
                verifyPoint = Int64.Parse(GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 2).Replace(",", "")) > 1000000000 
                    && Int64.Parse(GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 2).Replace(",", "")) < 5000000000;
                verifyPoints.Add(summaryTC = "Verify the 1st row value in 'NAV' column is shown: '" + firstRowDataNAV + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = Int64.Parse(GeneralAction.Instance.RowValuesInAssetTableGetText(10, 6, 2).Replace(",", "")) > 300000000
                    && Int64.Parse(GeneralAction.Instance.RowValuesInAssetTableGetText(10, 6, 2).Replace(",", "")) < 1000000000;
                verifyPoints.Add(summaryTC = "Verify the 6th row value in 'NAV' column is shown: '" + sixthRowDataNAV + "'", verifyPoint);
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
        public void TC002_PrivateFund_sort_Asset()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Private Fund Test - TC002");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Go to Private Fund page
                NavigationAction.Instance.ClickPageNames(10, Navigation.privateFund);

                // Wait for Asset & Private Fund table are displayed
                GeneralAction.Instance.WaitForElementVisible(10, LoginPage.assetTable)
                                       .WaitForElementVisible(10, General.tableHeaderName(PrivateFundPage.privateFundHeadertable));

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {

                }
                if (urlInstance.Contains("conceptia"))
                {

                }

                #region Sort A to Z at column 'Asset'
                // Click sort button at column 'Asset'
                GeneralAction.Instance.ClickSortAssetTableButton(10, General.asset)
                                      .WaitForElementInvisible(10, General.assetTableSortIconStatus(General.asset, General.noSort))
                                      .WaitForElementVisible(10, General.assetTableSortIconStatus(General.asset, General.sortZtoA)); // old: sortAtoZ

                // Verify row values in Asset column
                string title = "Private Equity";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = "Buyout and Growth Equity") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 2, 1);
                verifyPoints.Add(summaryTC = "Verify the 2nd row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = "Distressed and Credit") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 3, 1);
                verifyPoints.Add(summaryTC = "Verify the 3rd row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = "Hawaii Targeted Investment") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 4, 1);
                verifyPoints.Add(summaryTC = "Verify the 4th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = "Venture Capital") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 5, 1);
                verifyPoints.Add(summaryTC = "Verify the 5th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = "Real Assets") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 6, 1);
                verifyPoints.Add(summaryTC = "Verify the 6th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); 

                verifyPoint = (title = "FAD Real Estate") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 7, 1);
                verifyPoints.Add(summaryTC = "Verify the 7th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = "Natural Resources") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 8, 1);
                verifyPoints.Add(summaryTC = "Verify the 8th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Sort Z to A at column 'Asset'
                // Click sort button at column 'Asset'
                GeneralAction.Instance.ClickSortAssetTableButton(10, General.asset)
                                      .WaitForElementInvisible(10, General.assetTableSortIconStatus(General.asset, General.sortZtoA)) // old: sortAtoZ
                                      .WaitForElementVisible(10, General.assetTableSortIconStatus(General.asset, General.sortAtoZ)); // old: sortZtoA

                // Verify row values in Asset column
                verifyPoint = (title = "Real Assets") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = "Natural Resources") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 2, 1);
                verifyPoints.Add(summaryTC = "Verify the 2nd row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = "FAD Real Estate") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 3, 1);
                verifyPoints.Add(summaryTC = "Verify the 3rd row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = "Private Equity") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 4, 1);
                verifyPoints.Add(summaryTC = "Verify the 4th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = "Venture Capital") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 5, 1);
                verifyPoints.Add(summaryTC = "Verify the 5th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = "Hawaii Targeted Investment") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 6, 1);
                verifyPoints.Add(summaryTC = "Verify the 6th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = "Distressed and Credit") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 7, 1);
                verifyPoints.Add(summaryTC = "Verify the 7th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = "Buyout and Growth Equity") == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 8, 1);
                verifyPoints.Add(summaryTC = "Verify the 8th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
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
        public void TC003_PrivateFund_GroupByAssetClass_and_GroupByManager()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string dataGroupByManagerOrder2 = null;
            string dataGroupByManagerOrder3 = null;
            string dataGroupByManagerOrder4 = null;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Private Fund Test - TC003");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Go to Private Fund page
                NavigationAction.Instance.ClickPageNames(10, Navigation.privateFund);

                // Wait for Asset & Private Fund table are displayed
                GeneralAction.Instance.WaitForElementVisible(10, LoginPage.assetTable)
                                       .WaitForElementVisible(10, General.tableHeaderName(PrivateFundPage.privateFundHeadertable));

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {
                    dataGroupByManagerOrder2 = "Agriculture Capital";
                    dataGroupByManagerOrder3 = "AlphaX";
                    dataGroupByManagerOrder4 = "Alterna CCA";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    dataGroupByManagerOrder2 = "Agriculture Capital";
                    dataGroupByManagerOrder3 = "AlphaX";
                    dataGroupByManagerOrder4 = "Alterna CCA";
                }

                #region Verify 'group by Manager' button working correctly
                // Click on 'Group by Manager' button
                GeneralAction.Instance.ClickGroupUngroupShowFundButton(10, PrivateFundPage.groupByManager);

                // Click sort button at column 'Asset'
                GeneralAction.Instance.ClickSortAssetTableButton(10, General.asset)
                                      .WaitForElementInvisible(10, General.assetTableSortIconStatus(General.asset, General.noSort))
                                      .WaitForElementVisible(10, General.assetTableSortIconStatus(General.asset, General.sortZtoA)); // old: sortAtoZ

                // Verify row values in Asset column
                string title = "Accel Management Co. Inc."; //  Accel Capital LLC
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Asset column is shown (Group by Manager - button) (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = dataGroupByManagerOrder2) == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 2, 1);
                verifyPoints.Add(summaryTC = "Verify the 2nd row value in Asset column is shown (Group by Manager - button) (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = dataGroupByManagerOrder3) == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 3, 1);
                verifyPoints.Add(summaryTC = "Verify the 3rd row value in Asset column is shown (Group by Manager - button) (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = dataGroupByManagerOrder4) == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 4, 1);
                verifyPoints.Add(summaryTC = "Verify the 4th row value in Asset column is shown (Group by Manager - button) (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify 'Group By Asset Class' button working correctly
                // Click on 'Grouped By Asset Class' button
                GeneralAction.Instance.ClickGroupUngroupShowFundButton(10, PrivateFundPage.groupByAssetClass);

                // Verify row values in Asset column
                title = "Private Equity";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Asset column is shown (Group By Asset Class - button): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Venture Capital";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 2, 1);
                verifyPoints.Add(summaryTC = "Verify the 2nd row value in Asset column is shown (Group By Asset Class - button): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Buyout and Growth Equity";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 3, 1);
                verifyPoints.Add(summaryTC = "Verify the 3rd row value in Asset column is shown (Group By Asset Class - button): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Distressed and Credit";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 4, 1);
                verifyPoints.Add(summaryTC = "Verify the 4th row value in Asset column is shown (Group By Asset Class - button): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Hawaii Targeted Investment";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 5, 1);
                verifyPoints.Add(summaryTC = "Verify the 5th row value in Asset column is shown (Group By Asset Class - button): '" + title + "'", verifyPoint);
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
        public void TC004_PrivateFund_Rating()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string userName = LoginPage.username;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Private Fund Test - TC004");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Go to Private Fund page
                NavigationAction.Instance.ClickPageNames(10, Navigation.privateFund);

                // Wait for Asset & Private Fund table are displayed
                GeneralAction.Instance.WaitForElementVisible(10, LoginPage.assetTable)
                                       .WaitForElementVisible(10, General.tableHeaderName(PrivateFundPage.privateFundHeadertable));

                // Check if the data of Sandbox or Staging(Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {
                    // Click on 'Venture Capital' Asset to expand
                    GeneralAction.Instance.ClickToExpandAssetName(10, "Venture Capital")
                                          .WaitForElementVisible(10, General.assetName("US Venture Capital"));

                    // Click on 'US Venture Capital' Asset to expand
                    GeneralAction.Instance.ClickToExpandAssetName(10, "US Venture Capital")
                                          .WaitForElementVisible(10, General.assetName("Y Combinator (YC)"));

                    // Click on 'Y Combinator (YC)' Asset to expand
                    GeneralAction.Instance.ClickToExpandAssetName(10, "Y Combinator (YC)")
                                          .WaitForElementVisible(10, General.ratingAssetNameDialog("Y Combinator (YC)"));

                    // Click on a TabName in Rating Dialog
                    GeneralAction.Instance.ClickTabNameInRatingDialog(10, "Ratings");

                    // Slide slider on Rating tab
                    GeneralAction.Instance.SlideSliderLabelRating(10, "Edge", 20) // --> 0.0
                                          .SlideSliderLabelRating(10, "Organization", 18) // --> 0.5
                                          .SlideSliderLabelRating(10, "Track Record", 17); // --> 1

                    // Select an item in 'Conviction' dropdown
                    GeneralAction.Instance.ClickAndSelectItemInRatingDropdown(10, "Conviction", "Low");

                    // Click on 'Save Data' button 
                    GeneralAction.Instance.ClickSaveRatingButton(10)
                                          .WaitForLoadingIconToDisappear(10, General.loadingSpinner);

                    // Verify the Rating
                    // Click on 'Y Combinator (YC)' Asset again to expand
                    GeneralAction.Instance.ClickToExpandAssetName(10, "Y Combinator (YC)")
                                          .WaitForElementVisible(10, General.ratingAssetNameDialog("Y Combinator (YC)"));

                    // Click on a TabName in Rating Dialog
                    GeneralAction.Instance.ClickTabNameInRatingDialog(10, General.ratingHistory);

                    #region Verify headers in 'Rating History' tab
                    string header = "User";
                    verifyPoint = header == GeneralAction.Instance.RatingTabHeaderGetText(10, General.ratingHistory, 1);
                    verifyPoints.Add(summaryTC = "Verify the 1st header in 'Rating History' tab: " + header + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (header = "Edge") == GeneralAction.Instance.RatingTabHeaderGetText(10, General.ratingHistory, 2);
                    verifyPoints.Add(summaryTC = "Verify the 2nd header in 'Rating History' tab: " + header + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (header = "Organization") == GeneralAction.Instance.RatingTabHeaderGetText(10, General.ratingHistory, 3);
                    verifyPoints.Add(summaryTC = "Verify the 3rd header in 'Rating History' tab: " + header + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (header = "Track Record") == GeneralAction.Instance.RatingTabHeaderGetText(10, General.ratingHistory, 4);
                    verifyPoints.Add(summaryTC = "Verify the 4th header in 'Rating History' tab: " + header + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
 
                    verifyPoint = (header = "Total Rating") == GeneralAction.Instance.RatingTabHeaderGetText(10, General.ratingHistory, 5); // KS-568, 569
                    verifyPoints.Add(summaryTC = "Verify the 5th header in 'Rating History' tab: " + header + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (header = "Conviction") == GeneralAction.Instance.RatingTabHeaderGetText(10, General.ratingHistory, 6);
                    verifyPoints.Add(summaryTC = "Verify the 6th header in 'Rating History' tab: " + header + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Verify the latest rating in 'Rating History' tab
                    verifyPoint = userName == GeneralAction.Instance.RatingHistoryRowUsersGetText(10, 1, 1);
                    verifyPoints.Add(summaryTC = "Verify (User) the latest rating in 'Rating History' tab: " + userName + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    string data = "0.0";
                    verifyPoint = data == GeneralAction.Instance.RatingTabRowValuesGetText(10, General.ratingHistory, 1, 2);
                    verifyPoints.Add(summaryTC = "Verify (Edge) the latest rating in 'Rating History' tab: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (data = "0.5") == GeneralAction.Instance.RatingTabRowValuesGetText(10, General.ratingHistory, 1, 3);
                    verifyPoints.Add(summaryTC = "Verify (Organization) the latest rating in 'Rating History' tab: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (data = "1.0") == GeneralAction.Instance.RatingTabRowValuesGetText(10, General.ratingHistory, 1, 4);
                    verifyPoints.Add(summaryTC = "Verify (Track Record) the latest rating in 'Rating History' tab: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (data = "Low") == GeneralAction.Instance.RatingTabRowValuesGetText(10, General.ratingHistory, 1, 6);
                    verifyPoints.Add(summaryTC = "Verify (Conviction) the latest rating in 'Rating History' tab: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    // Click on a TabName in Rating Dialog
                    GeneralAction.Instance.ClickTabNameInRatingDialog(10, General.teamRatingHistory);

                    #region Verify headers in 'Team Rating History' tab
                    verifyPoint = (header = "Year") == GeneralAction.Instance.RatingTabHeaderGetText(10, General.teamRatingHistory, 1);
                    verifyPoints.Add(summaryTC = "Verify the 1st header in 'Team Rating History' tab: " + header + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (header = "Edge") == GeneralAction.Instance.RatingTabHeaderGetText(10, General.teamRatingHistory, 2);
                    verifyPoints.Add(summaryTC = "Verify the 2nd header in 'Team Rating History' tab: " + header + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (header = "Organization") == GeneralAction.Instance.RatingTabHeaderGetText(10, General.teamRatingHistory, 3);
                    verifyPoints.Add(summaryTC = "Verify the 3rd header in 'Team Rating History' tab: " + header + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (header = "Track Record") == GeneralAction.Instance.RatingTabHeaderGetText(10, General.teamRatingHistory, 4);
                    verifyPoints.Add(summaryTC = "Verify the 4th header in 'Team Rating History' tab: " + header + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (header = "Total Rating") == GeneralAction.Instance.RatingTabHeaderGetText(10, General.teamRatingHistory, 5);
                    verifyPoints.Add(summaryTC = "Verify the 5th header in 'Team Rating History' tab: " + header + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (header = "Conviction") == GeneralAction.Instance.RatingTabHeaderGetText(10, General.teamRatingHistory, 6);
                    verifyPoints.Add(summaryTC = "Verify the 6th header in 'Team Rating History' tab: " + header + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Verify the latest rating in 'Team Rating History' tab
                    verifyPoint = (data = "2025") == GeneralAction.Instance.RatingTabRowValuesGetText(10, General.teamRatingHistory, 1, 1);
                    verifyPoints.Add(summaryTC = "Verify (Year) the latest rating in 'Team Rating History' tab: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = //(data = "3.7") == GeneralAction.Instance.RatingTabRowValuesGetText(10, General.teamRatingHistory, 1, 2); // 0
                    Convert.ToDouble(GeneralAction.Instance.RatingTabRowValuesGetText(10, General.teamRatingHistory, 1, 2)) >= 3.0;
                    verifyPoints.Add(summaryTC = "Verify (Edge) the latest rating in 'Team Rating History' tab: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = //(data = "4.2") == GeneralAction.Instance.RatingTabRowValuesGetText(10, General.teamRatingHistory, 1, 3); // 0.5
                    Convert.ToDouble(GeneralAction.Instance.RatingTabRowValuesGetText(10, General.teamRatingHistory, 1, 3)) >= 3.0;
                    verifyPoints.Add(summaryTC = "Verify (Organization) the latest rating in 'Team Rating History' tab: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = //(data = "4.7") == GeneralAction.Instance.RatingTabRowValuesGetText(10, General.teamRatingHistory, 1, 4); // 1.0
                    Convert.ToDouble(GeneralAction.Instance.RatingTabRowValuesGetText(10, General.teamRatingHistory, 1, 4)) >= 3.0;
                    verifyPoints.Add(summaryTC = "Verify (Track Record) the latest rating in 'Team Rating History' tab: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = //(data = "4.1") == GeneralAction.Instance.RatingTabRowValuesGetText(10, General.teamRatingHistory, 1, 5); // 0.5
                    Convert.ToDouble(GeneralAction.Instance.RatingTabRowValuesGetText(10, General.teamRatingHistory, 1, 5)) >= 3.0;
                    verifyPoints.Add(summaryTC = "Verify (Total Rating) the latest rating in 'Team Rating History' tab: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = Convert.ToDouble(GeneralAction.Instance.RatingTabRowValuesGetText(10, General.teamRatingHistory, 1, 6)) >= 0;
                    verifyPoints.Add(summaryTC = "Verify (Conviction) the latest rating in 'Team Rating History' tab: > 5.0", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion
                }
                else 
                {
                    Console.WriteLine(summaryTC = "TC004 is only add Rating on Sandbox Site!!!");
                    test.Log(Status.Info, summaryTC);
                }
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
