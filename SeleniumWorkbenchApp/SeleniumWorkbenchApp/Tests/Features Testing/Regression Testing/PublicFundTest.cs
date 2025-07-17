using AventStack.ExtentReports;
using NUnit.Framework;
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
    [TestFixture, Order(3)]
    internal class PublicFundTest : BaseTestCase
    {
        // Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;

        [Test, Category("Regression Testing")]
        public void TC001_PublicFund_default_data()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string? datePublicFund = null; 
            string? equityBeta = null;
            string? risk = null;
            string? percLiquidAsset = null;
            string? nav = null;
            string? totalUnfundedCommitments = null;
            string? firstRowDataEstDailyNAV = null;
            string? sixthRowDataEstDailyNAV = null;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Public Fund Test - TC001");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Go to Public Fund page
                NavigationAction.Instance.ClickPageNames(10, Navigation.publicFund);

                // Wait for Asset & Public Fund table are displayed
                GeneralAction.Instance.WaitForElementVisible(20, LoginPage.assetTable)
                                       .WaitForElementVisible(20, General.tableHeaderName(PublicFundPage.publicFundHeadertable));

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {
                    // Data in 'Public Fund' table header
                    datePublicFund = "2025";
                    equityBeta = "0.55";
                    risk = "14.0%";
                    percLiquidAsset = "61.0%";
                    nav = "5.18 Billion";
                    totalUnfundedCommitments = "1.1 Billion";

                    // Row Values in Asset Table (Overview tab)
                    firstRowDataEstDailyNAV = "2,359,202,297";
                    sixthRowDataEstDailyNAV = "2,822,381,340";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    // Data in 'Public Fund' table header
                    datePublicFund = "2025";
                    equityBeta = "0.56";
                    risk = "14.0%";
                    percLiquidAsset = "61.5%";
                    nav = "5.20 Billion";
                    totalUnfundedCommitments = "1.1 Billion";

                    // Row Values in Asset Table (Overview tab)
                    firstRowDataEstDailyNAV = "2,387,093,273";
                    sixthRowDataEstDailyNAV = "2,816,562,917";
                }

                #region Verify Data in 'Public Fund' table header
                verifyPoint = GeneralAction.Instance.DateInTableHeaderNameGetText(10, PublicFundPage.publicFundHeadertable).Contains(datePublicFund);
                verifyPoints.Add(summaryTC = "Verify date of '" + PublicFundPage.publicFundHeadertable + "' in the table header is shown: '" + datePublicFund + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = Convert.ToDouble(GeneralAction.Instance.DataInTableHeaderGetText(10, PublicFundPage.publicFundHeadertable, Navigation.equityBeta)) > 0.00 
                    && Convert.ToDouble(GeneralAction.Instance.DataInTableHeaderGetText(10, PublicFundPage.publicFundHeadertable, Navigation.equityBeta)) < 3.00;
                verifyPoints.Add(summaryTC = "Verify data of '" + PublicFundPage.publicFundHeadertable + "' - '" + Navigation.equityBeta + "' is shown: '" + equityBeta + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                //string title = "14.2%";
                verifyPoint = double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PublicFundPage.publicFundHeadertable, Navigation.risk).Replace("%", "")) > 12.0
                    && double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PublicFundPage.publicFundHeadertable, Navigation.risk).Replace("%", "")) < 16.0;
                verifyPoints.Add(summaryTC = "Verify data of '" + PublicFundPage.publicFundHeadertable + "' - '" + Navigation.risk + "' is shown: '" + risk + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PublicFundPage.publicFundHeadertable, Navigation.percLiquidAsset).Replace("%", "")) > 1.0 
                    && double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PublicFundPage.publicFundHeadertable, Navigation.percLiquidAsset).Replace("%", "")) < 300.0;
                verifyPoints.Add(summaryTC = "Verify data of '" + PublicFundPage.publicFundHeadertable + "' - '" + Navigation.percLiquidAsset + "' is shown: '" + percLiquidAsset + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PublicFundPage.publicFundHeadertable, Navigation.nav).Replace(" Billion", "")) > 4.00 
                    && double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PublicFundPage.publicFundHeadertable, Navigation.nav).Replace(" Billion", "")) < 7.00;
                verifyPoints.Add(summaryTC = "Verify data of '" + PublicFundPage.publicFundHeadertable + "' - '" + Navigation.nav + "' is shown: '" + nav + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PublicFundPage.publicFundHeadertable, Navigation.totalUnfundedCommitments).Replace(" Million", "").Replace(" Billion", "")) > 0.00 
                     && double.Parse(GeneralAction.Instance.DataInTableHeaderGetText(10, PublicFundPage.publicFundHeadertable, Navigation.totalUnfundedCommitments).Replace(" Million", "").Replace(" Billion", "")) < 1000.00;
                verifyPoints.Add(summaryTC = "Verify data of '" + PublicFundPage.publicFundHeadertable + "' - '" + Navigation.totalUnfundedCommitments + "' is shown: '" + totalUnfundedCommitments + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Menu titles are shown after login successfully
                //string title = PublicFundPage.overview; --> KS 671 remove Overview tab
                //verifyPoint = title == GeneralAction.Instance.MenuTitlesGetText(10, 1);
                //verifyPoints.Add(summaryTC = "Verify the 1st menu/tab title is shown after login successfully: '" + title + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                /* Remove these sub tab/menu titles base on ticket KS-510
                title = PublicFundPage.fee;
                verifyPoint = title == GeneralAction.Instance.MenuTitlesGetText(10, 2);
                verifyPoints.Add(summaryTC = "Verify the 2nd menu/tab title is shown after login successfully: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = PublicFundPage.peer;
                verifyPoint = title == GeneralAction.Instance.MenuTitlesGetText(10, 3);
                verifyPoints.Add(summaryTC = "Verify the 3rd menu/tab title is shown after login successfully: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = PublicFundPage.notes;
                verifyPoint = title == GeneralAction.Instance.MenuTitlesGetText(10, 4);
                verifyPoints.Add(summaryTC = "Verify the 4th menu/tab title is shown after login successfully: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = PublicFundPage.dataStatus;
                verifyPoint = title == GeneralAction.Instance.MenuTitlesGetText(10, 5);
                verifyPoints.Add(summaryTC = "Verify the 5th menu/tab title is shown after login successfully: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); */
                #endregion

                #region Verify Column Names in Asset Table (Overview tab)
                //// Click on Overview tab
                //NavigationAction.Instance.ClickPageNames(10, PublicFundPage.overview); --> KS 671 remove Overview tab

                /* Verify Add Filter button is shown after login successfully // --> Remove this button (KS-528)
                verifyPoint = GeneralAction.Instance.IsAddFilterButtonShown(10);
                verifyPoints.Add(summaryTC = "Verify Add Filter button is shown after login successfully", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); */

                // Verify Column Names in Asset Table
                string title = "Asset";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Est Daily NAV"; // Benchmark --> removed this column
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 2);
                verifyPoints.Add(summaryTC = "Verify the 2nd column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Edge Rating";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 3);
                verifyPoints.Add(summaryTC = "Verify the 3rd column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Org Rating";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 4);
                verifyPoints.Add(summaryTC = "Verify the 4th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "TR Rating";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 5);
                verifyPoints.Add(summaryTC = "Verify the 5th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Total Weighted Rating";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 6);
                verifyPoints.Add(summaryTC = "Verify the 6th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Conviction";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 7);
                verifyPoints.Add(summaryTC = "Verify the 7th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Tracking Error";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 8);
                verifyPoints.Add(summaryTC = "Verify the 7th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Gross Manager IR";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 9);
                verifyPoints.Add(summaryTC = "Verify the 8th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Gross Manager Alpha";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 10);
                verifyPoints.Add(summaryTC = "Verify the 9th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Risk Adj Strategy Alpha";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 11);
                verifyPoints.Add(summaryTC = "Verify the 10th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Gross Total Alpha";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 12);
                verifyPoints.Add(summaryTC = "Verify the 11th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Fees";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 13);
                verifyPoints.Add(summaryTC = "Verify the 12th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Liquidity Cost";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 14);
                verifyPoints.Add(summaryTC = "Verify the 13th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Net Total Alpha";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 15);
                verifyPoints.Add(summaryTC = "Verify the 14th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Net IR";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 16);
                verifyPoints.Add(summaryTC = "Verify the 15th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Row Values in Asset Table (Overview tab)
                // Verify row values in Asset column
                title = "Public Equities";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Asset column is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Absolute Return";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 6, 1);
                verifyPoints.Add(summaryTC = "Verify the 6th row value in Asset column is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify row value in 'Est Daily NAV' column
                verifyPoint = Int64.Parse(GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 2).Replace(",", "")) > 1000000000
                    && Int64.Parse(GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 2).Replace(",", "")) < 4000000000;
                verifyPoints.Add(summaryTC = "Verify the 1st row value in 'Est Daily NAV' column is shown: '" + firstRowDataEstDailyNAV + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = Int64.Parse(GeneralAction.Instance.RowValuesInAssetTableGetText(10, 6, 2).Replace(",", "")) > 1000000000
                    && Int64.Parse(GeneralAction.Instance.RowValuesInAssetTableGetText(10, 6, 2).Replace(",", "")) < 4000000000;
                verifyPoints.Add(summaryTC = "Verify the 6th row value in 'Est Daily NAV' column is shown: '" + sixthRowDataEstDailyNAV + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                /* // Verify row value in 'Gross Manager IR' column
                verifyPoint = firstRowGrossManagerIR == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 8);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in 'Gross Manager IR' column is shown: '" + firstRowGrossManagerIR + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = secondRowGrossManagerIR == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 2, 8);
                verifyPoints.Add(summaryTC = "Verify the 2nd row value in 'Gross Manager IR' column is shown: '" + secondRowGrossManagerIR + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify row value in 'Gross Manager Alpha' column
                verifyPoint = firstRowGrossManagerAlpha == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 9);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in 'Gross Manager Alpha' column is shown: '" + firstRowGrossManagerAlpha + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = secondRowGrossManagerAlpha == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 2, 9);
                verifyPoints.Add(summaryTC = "Verify the 2nd row value in 'Gross Manager Alpha' column is shown: '" + secondRowGrossManagerAlpha + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); */
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
        public void TC002_PublicFund_sort_Asset()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string? dataGroupByAssetClass2 = null;
            string? dataGroupByAssetClass3 = null;
            string? dataGroupByAssetClass4 = null;
            string? dataGroupByAssetClass5 = null;
            string? dataGroupByAssetClass6 = null;
            string? dataGroupByAssetClassZtoA7 = null;
            string? dataGroupByAssetClassZtoA8 = null;
            string? dataGroupByAssetClassZtoA9 = null;
            string? dataGroupByAssetClassZtoA10 = null;
            string? dataGroupByAssetClassZtoA11 = null;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Public Fund Test - TC002");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Go to Public Fund page
                NavigationAction.Instance.ClickPageNames(10, Navigation.publicFund);

                // Wait for Asset & Public Fund table are displayed
                GeneralAction.Instance.WaitForElementVisible(20, LoginPage.assetTable)
                                       .WaitForElementVisible(20, General.tableHeaderName(PublicFundPage.publicFundHeadertable));

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {
                    dataGroupByAssetClass2 = "Equity Hedge";
                    dataGroupByAssetClass3 = "Event Driven";
                    dataGroupByAssetClass4 = "Macro";
                    dataGroupByAssetClass5 = "Other (AR)";
                    dataGroupByAssetClass6 = "Relative Value";
                    dataGroupByAssetClassZtoA7 = "Relative Value";
                    dataGroupByAssetClassZtoA8 = "Other (AR)";
                    dataGroupByAssetClassZtoA9 = "Macro";
                    dataGroupByAssetClassZtoA10 = "Event Driven";
                    dataGroupByAssetClassZtoA11 = "Equity Hedge";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    dataGroupByAssetClass2 = "Equity Hedge";
                    dataGroupByAssetClass3 = "Event Driven";
                    dataGroupByAssetClass4 = "Macro";
                    dataGroupByAssetClass5 = "Other (AR)";
                    dataGroupByAssetClass6 = "Relative Value";
                    dataGroupByAssetClassZtoA7 = "Relative Value";
                    dataGroupByAssetClassZtoA8 = "Other (AR)";
                    dataGroupByAssetClassZtoA9 = "Macro";
                    dataGroupByAssetClassZtoA10 = "Event Driven";
                    dataGroupByAssetClassZtoA11 = "Equity Hedge";
                }

                #region Sort A to Z at column 'Asset' (Group By Asset Class)
                // Click sort button at column 'Asset'
                GeneralAction.Instance.ClickSortAssetTableButton(10, General.asset)
                                      .WaitForElementInvisible(10, General.assetTableSortIconStatus(General.asset, General.noSort))
                                      .WaitForElementVisible(10, General.assetTableSortIconStatus(General.asset, General.sortZtoA)); //old: sortAtoZ

                // Verify row values in Asset column
                string title = "Absolute Return";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = dataGroupByAssetClass2) == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 2, 1);
                verifyPoints.Add(summaryTC = "Verify the 2nd row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = dataGroupByAssetClass3) == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 3, 1);
                verifyPoints.Add(summaryTC = "Verify the 3rd row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = dataGroupByAssetClass4) == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 4, 1);
                verifyPoints.Add(summaryTC = "Verify the 4th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = dataGroupByAssetClass5) == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 5, 1);
                verifyPoints.Add(summaryTC = "Verify the 5th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = dataGroupByAssetClass6) == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 6, 1);
                verifyPoints.Add(summaryTC = "Verify the 6th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Public Equities";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 7, 1);
                verifyPoints.Add(summaryTC = "Verify the 7th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Domestic Equity";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 8, 1);
                verifyPoints.Add(summaryTC = "Verify the 8th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Emerging Markets Equity";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 9, 1);
                verifyPoints.Add(summaryTC = "Verify the 9th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Global Equity";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 10, 1);
                verifyPoints.Add(summaryTC = "Verify the 10th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Non-US Developed Equity";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 11, 1);
                verifyPoints.Add(summaryTC = "Verify the 11th row value in Asset column is shown (A->Z): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Sort Z to A at column 'Asset' (Group By Asset Class)
                // Click sort button at column 'Asset'
                GeneralAction.Instance.ClickSortAssetTableButton(10, General.asset)
                                      .WaitForElementInvisible(10, General.assetTableSortIconStatus(General.asset, General.sortZtoA)) // Old: sortAtoZ
                                      .WaitForElementVisible(10, General.assetTableSortIconStatus(General.asset, General.sortAtoZ)); // Old: sortZtoA

                // Verify row values in Asset column
                title = "Public Equities";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Non-US Developed Equity";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 2, 1);
                verifyPoints.Add(summaryTC = "Verify the 2nd row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Global Equity";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 3, 1);
                verifyPoints.Add(summaryTC = "Verify the 3rd row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Emerging Markets Equity";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 4, 1);
                verifyPoints.Add(summaryTC = "Verify the 4th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Domestic Equity";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 5, 1);
                verifyPoints.Add(summaryTC = "Verify the 5th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "Absolute Return";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 6, 1);
                verifyPoints.Add(summaryTC = "Verify the 6th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = dataGroupByAssetClassZtoA7) == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 7, 1);
                verifyPoints.Add(summaryTC = "Verify the 7th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = dataGroupByAssetClassZtoA8) == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 8, 1);
                verifyPoints.Add(summaryTC = "Verify the 8th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = dataGroupByAssetClassZtoA9) == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 9, 1);
                verifyPoints.Add(summaryTC = "Verify the 9th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = dataGroupByAssetClassZtoA10) == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 10, 1);
                verifyPoints.Add(summaryTC = "Verify the 10th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = (title = dataGroupByAssetClassZtoA11) == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 11, 1);
                verifyPoints.Add(summaryTC = "Verify the 11th row value in Asset column is shown (Z->A): '" + title + "'", verifyPoint);
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
        public void TC003_PublicFund_groupByAssetClass_and_ShowFundsOnly()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string? titleRow3 = null;
            string? titleRow4 = null;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Public Fund Test - TC003");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Go to Public Fund page
                NavigationAction.Instance.ClickPageNames(10, Navigation.publicFund);

                // Wait for Asset & Public Fund table are displayed
                GeneralAction.Instance.WaitForElementVisible(20, LoginPage.assetTable)
                                       .WaitForElementVisible(20, General.tableHeaderName(PublicFundPage.publicFundHeadertable));

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {
                    titleRow3 = "AKO European Long-only Fund Limited";
                    titleRow4 = "Alphadyne Global Rates Fund II, Ltd.";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    titleRow3 = "AKO European Long-only Fund Limited";
                    titleRow4 = "Alphadyne Global Rates Fund II, Ltd.";
                }

                #region Verify 'Show Funds Only' button working correctly
                // Click on 'Show Funds Only' button
                GeneralAction.Instance.ClickGroupUngroupShowFundButton(10, PublicFundPage.showFundsOnly);

                // Click sort button at column 'Asset' (Z->A)
                GeneralAction.Instance.ClickSortAssetTableButton(10, General.asset)
                                      .WaitForElementInvisible(10, General.assetTableSortIconStatus(General.asset, General.noSort))
                                      .WaitForElementVisible(10, General.assetTableSortIconStatus(General.asset, General.sortZtoA)); // old: sortAtoZ

                // Verify row values in Asset column
                string title = "Abrams Capital Partners II, L.P.";
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Asset column is shown (Show Funds Only - button) (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                title = "ACK Asset Partners II, LP"; 
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 2, 1);
                verifyPoints.Add(summaryTC = "Verify the 2nd row value in Asset column is shown (Show Funds Only - button) (Z->A): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = titleRow3 == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 3, 1);
                verifyPoints.Add(summaryTC = "Verify the 3nd row value in Asset column is shown (Show Funds Only - button) (Z->A): '" + titleRow3 + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = titleRow4 == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 4, 1);
                verifyPoints.Add(summaryTC = "Verify the 4th row value in Asset column is shown (Show Funds Only - button) (Z->A): '" + titleRow4 + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify 'Group By Asset Class' button working correctly
                // Click on 'Group By Asset Class' button
                GeneralAction.Instance.ClickGroupUngroupShowFundButton(10, PublicFundPage.groupByAssetClass);

                // Verify row values in Asset column
                title = "Public Equities"; // Absolute Return
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 1, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Asset column is shown (Group By Asset Class - button): '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                //title = "Emerging Markets Equity";
                //verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 2, 1);
                //verifyPoints.Add(summaryTC = "Verify the 2nd row value in Asset column is shown (Group By Asset Class - button): '" + title + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                //title = "Non-US Developed Equity";
                //verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 3, 1);
                //verifyPoints.Add(summaryTC = "Verify the 3rd row value in Asset column is shown (Group By Asset Class - button): '" + title + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                title = "Global Equity"; // Macro
                verifyPoint = title == GeneralAction.Instance.RowValuesInAssetTableGetText(10, 4, 1);
                verifyPoints.Add(summaryTC = "Verify the 4th row value in Asset column is shown (Group By Asset Class - button): '" + title + "'", verifyPoint);
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
        public void TC004_PublicFund_Rating()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string userName = LoginPage.username;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Public Fund Test - TC004");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Go to Public Fund page
                NavigationAction.Instance.ClickPageNames(10, Navigation.publicFund);

                // Wait for Asset & Public Fund table are displayed
                GeneralAction.Instance.WaitForElementVisible(20, LoginPage.assetTable)
                                       .WaitForElementVisible(20, General.tableHeaderName(PublicFundPage.publicFundHeadertable));

                // Check if the data of Sandbox or Staging(Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab"))
                {
                    // Click on 'Macro' Asset to expand
                    GeneralAction.Instance.ClickToExpandAssetName(10, "Macro")
                                          .WaitForElementVisible(10, General.assetName("Brevan Howard Alpha Strategies Fund, L.P."));

                    //// Click on 'Brevan Howard Asset Management, LLP' Asset to expand
                    //GeneralAction.Instance.ClickToExpandAssetName(10, "Brevan Howard Alpha Strategies Fund, L.P.")
                    //                      .WaitForElementVisible(10, General.assetName("Brevan Howard Alpha Strategies Fund, L.P."));

                    // Click on 'Brevan Howard Alpha Strategies Fund, L.P.' Asset to expand
                    GeneralAction.Instance.ClickToExpandAssetName(10, "Brevan Howard Alpha Strategies Fund, L.P.")
                                          .WaitForElementVisible(10, General.ratingAssetNameDialog("Brevan Howard Alpha Strategies Fund, L.P."));

                    // Click on a TabName in Rating Dialog
                    GeneralAction.Instance.ClickTabNameInRatingDialog(10, "Ratings");

                    // Slide slider on Rating tab
                    GeneralAction.Instance.SlideSliderLabelRating(10, "Edge", 20) // --> 0.0
                                          .SlideSliderLabelRating(10, "Organization", 18) // --> 0.5
                                          .SlideSliderLabelRating(10, "Track Record", 17); // --> 1

                    // Select an item in 'Conviction' dropdown
                    GeneralAction.Instance.ClickAndSelectItemInRatingDropdown(10, "Conviction", "Medium");

                    // Click on 'Save Data' button 
                    GeneralAction.Instance.ClickSaveRatingButton(10)
                                          .WaitForLoadingIconToDisappear(10, General.loadingSpinner);

                    // Verify the Rating
                    // Click on 'Brevan Howard Alpha Strategies Fund, L.P.' Asset again to expand
                    GeneralAction.Instance.ClickToExpandAssetName(10, "Brevan Howard Alpha Strategies Fund, L.P.")
                                          .WaitForElementVisible(10, General.ratingAssetNameDialog("Brevan Howard Alpha Strategies Fund, L.P."));

                    // Click on a TabName in Rating Dialog
                    GeneralAction.Instance.ClickTabNameInRatingDialog(10, General.ratingHistory);

                    #region Verify headers in Rating History tab
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

                    #region Verify the latest rating in Rating History tab
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

                    verifyPoint = (data = "Medium") == GeneralAction.Instance.RatingTabRowValuesGetText(10, General.ratingHistory, 1, 6);
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

                    verifyPoint = (data = "0.0") == GeneralAction.Instance.RatingTabRowValuesGetText(10, General.teamRatingHistory, 1, 2);
                    verifyPoints.Add(summaryTC = "Verify (Edge) the latest rating in 'Team Rating History' tab: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (data = "0.5") == GeneralAction.Instance.RatingTabRowValuesGetText(10, General.teamRatingHistory, 1, 3);
                    verifyPoints.Add(summaryTC = "Verify (Organization) the latest rating in 'Team Rating History' tab: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (data = "1.0") == GeneralAction.Instance.RatingTabRowValuesGetText(10, General.teamRatingHistory, 1, 4);
                    verifyPoints.Add(summaryTC = "Verify (Track Record) the latest rating in 'Team Rating History' tab: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (data = "0.5") == GeneralAction.Instance.RatingTabRowValuesGetText(10, General.teamRatingHistory, 1, 5);
                    verifyPoints.Add(summaryTC = "Verify (Total Rating) the latest rating in 'Team Rating History' tab: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    verifyPoint = (data = "8.3") == GeneralAction.Instance.RatingTabRowValuesGetText(10, General.teamRatingHistory, 1, 6);
                    verifyPoints.Add(summaryTC = "Verify (Conviction) the latest rating in 'Team Rating History' tab: " + data + "", verifyPoint);
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

        [Test, Category("Regression Testing")]
        public void TC005_PublicFund_IconLinkToFundDetails()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string userName = LoginPage.username;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Public Fund Test - TC005");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Go to Public Fund page
                NavigationAction.Instance.ClickPageNames(10, Navigation.publicFund);

                // Wait for Asset & Public Fund table are displayed
                GeneralAction.Instance.WaitForElementVisible(20, LoginPage.assetTable)
                                       .WaitForElementVisible(20, General.tableHeaderName(PublicFundPage.publicFundHeadertable));

                // Click on 'Show Funds Only' button
                GeneralAction.Instance.ClickGroupUngroupShowFundButton(10, PublicFundPage.showFundsOnly);

                // Click sort button at column 'Asset' (A->Z)
                GeneralAction.Instance.ClickSortAssetTableButton(10, General.asset)
                                      .WaitForElementInvisible(10, General.assetTableSortIconStatus(General.asset, General.noSort))
                                      .WaitForElementVisible(10, General.assetTableSortIconStatus(General.asset, General.sortZtoA)); // old: sortAtoZ

                // Click on an icon to table to link Fund
                string assetName = "Abrams Capital Partners II, L.P.";
                GeneralAction.Instance.ClickAssetNameButton(10, assetName, "last()");

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

                // Verify link the user to the respective Fund Details Page
                verifyPoint = assetName == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 2, 1);
                verifyPoints.Add(summaryTC = "Verify Fund name is shown correctly after clicking on the 'link' icon: " + assetName + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
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
