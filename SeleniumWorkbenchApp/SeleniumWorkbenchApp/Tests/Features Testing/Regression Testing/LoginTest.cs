using AventStack.ExtentReports;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using WorkbenchApp.UITest.Core.BaseTestCase;
using WorkbenchApp.UITest.Core.Selenium;
using WorkbenchApp.UITest.Pages;

namespace WorkbenchApp.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(1)]
    internal class LoginTest : BaseTestCase
    {
        // Variables declare
        private static string? url = null;
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;

        [SetUp]
        public override void SetupTest()
        {
            //Data-driven for site testing
            verifyPoints.Clear();
            Driver.StartBrowser();
            LoginPage.configurationFile();
            url = LoginPage.url;
            LoginAction.Instance.NavigateSite(url);
        }

        [Test, Category("Regression Testing")]
        public void TC001_login_with_valid_account()
        {
            #region Variables declare
            string email = LoginPage.email;
            string password = LoginPage.password;
            string username = LoginPage.username;
            const string ownedByKSText = "Owned by KS";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Login Test");
            try
            {
                #region login steps
                // Store the current window handle (main window - Workbench)
                string winHandleBefore = Driver.Browser.CurrentWindowHandle;

                // Wait for Login button is shown
                LoginAction.Instance.WaitForElementVisible(10, LoginPage.loginWithMSAccountBtn);

                // Verify the login button is shown (Login With Microsoft Account)
                verifyPoint = LoginAction.Instance.IsLoginWithMSAccountBtnShown(10);
                verifyPoints.Add(summaryTC = "Verify Login button (Login With Microsoft Account) is displayed", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click Login button
                LoginAction.Instance.ClickLogin(10);

                // Wait for the new window is opened
                LoginAction.Instance.WaitForNewWindowOpen(15);

                // Store all the opened window into the 'list'
                string winHandleLast = Driver.Browser.WindowHandles.Last();
                List<string> listWindow = Driver.Browser.WindowHandles.ToList();
                string lastWindowHandle = "";
                foreach (var handle in listWindow)
                {
                    //Switch to the desired window first and then execute commands using driver
                    Driver.Browser.SwitchTo().Window(handle);
                    lastWindowHandle = handle;
                }

                // Switch to new window opened
                Driver.Browser.SwitchTo().Window(lastWindowHandle); Thread.Sleep(1000);

                // Wait for the element of new window is opened
                LoginAction.Instance.WaitForElementVisible(10, LoginPage.signInEmailInputTxt);

                // Enter Email and then click Next button
                LoginAction.Instance.EnterEmail(email).ClickNext(10);

                // Switch to new window opened
                Driver.Browser.SwitchTo().Window(winHandleLast);
                System.Threading.Thread.Sleep(3000); //LoginAction.Instance.WaitForElementVisible(10, LoginPage.signInBtn);

                // Enter Password and then Click on SignIn button ...
                LoginAction.Instance.EnterPassword(10, password)
                                    .ClickSignIn(10)
                                    .ClickNext(10);

                // Check if the popup MS still being shown, then switch to them main window to click login (with MS) button again
                LoginAction.Instance.CheckIfMSLoginPopupStillShown(10);

                // Wait for Pie Chart, Asset and Total Endowment table are loaded Done
                LoginAction.Instance.WaitForElementInvisible(30, LoginPage.pieChartGrayLoading)
                                    .WaitForElementVisible(30, LoginPage.pieChart)
                                    .WaitForElementVisible(30, LoginPage.assetTable)
                                    .WaitForElementVisible(30, LoginPage.totalEndowmentTable);
                #endregion

                #region Verify all buttons, icons ... are displayed after login successfully
                // Verify Search Box is shown after login successfully
                verifyPoint = LoginAction.Instance.IsSearchBoxShown(10);
                verifyPoints.Add(summaryTC = "Verify Search Box is shown after login successfully", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Upload button is shown after login successfully
                verifyPoint = LoginAction.Instance.IsUploadButtonShown(10);
                verifyPoints.Add(summaryTC = "Verify Upload button is shown after login successfully", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                //// Verify Heart icon is shown after login successfully --> KS-625 Remove the notification bell, heart and question mark icons
                //verifyPoint = LoginAction.Instance.IsPiHeartIconShown(10);
                //verifyPoints.Add(summaryTC = "Verify Heart icon is shown after login successfully", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                //// Verify Bell icon is shown after login successfully --> KS-625 Remove
                //verifyPoint = LoginAction.Instance.IsPiBellIconShown(10);
                //verifyPoints.Add(summaryTC = "Verify Bell icon is shown after login successfully", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                //// Verify Question Circle icon is shown after login successfully --> KS-625 Remove
                //verifyPoint = LoginAction.Instance.IsPiQuestionCircleIconShown(10);
                //verifyPoints.Add(summaryTC = "Verify Question Circle icon is shown after login successfully", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                // Verify Fund Setup button is shown after login successfully
                verifyPoint = LoginAction.Instance.IsFundSetupButtonShown(10);
                verifyPoints.Add(summaryTC = "Verify Fund Setup button is shown after login successfully", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Owned by KS Checkbox is shown after login successfully
                verifyPoint = LoginAction.Instance.IsOwnedbyKSCheckboxShown(10);
                verifyPoints.Add(summaryTC = "Verify Owned by KS Checkbox is shown after login successfully", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Page titles are shown after login successfully
                // Verify Total Endowment page is shown after login successfully
                string titles = LoginPage.totalEndowment;
                verifyPoint = titles == LoginAction.Instance.PageTitlesGetText(10, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st page title is shown after login successfully: '"+ titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Public Fund Page titles are shown after login successfully
                titles = LoginPage.publicFund;
                verifyPoint = titles == LoginAction.Instance.PageTitlesGetText(10, 2);
                verifyPoints.Add(summaryTC = "Verify the 2nd page title is shown after login successfully: '"+ titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Private Fund Page titles are shown after login successfully
                titles = LoginPage.privateFund;
                verifyPoint = titles == LoginAction.Instance.PageTitlesGetText(10, 3);
                verifyPoints.Add(summaryTC = "Verify the 3rd page title is shown after login successfully: '"+ titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Pipeline Page titles are shown after login successfully
                titles = LoginPage.pipeline;
                verifyPoint = titles == LoginAction.Instance.PageTitlesGetText(10, 4);
                verifyPoints.Add(summaryTC = "Verify the 4th page title is shown after login successfully: '"+ titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Menu titles are shown after login successfully
                // Verify Overview menu is shown after login successfully
                titles = LoginPage.overview;
                verifyPoint = titles == LoginAction.Instance.MenuTitlesGetText(10, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st menu title is shown after login successfully: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Return (Public) menu is shown after login successfully
                titles = LoginPage.returnPublic;
                verifyPoint = titles == LoginAction.Instance.MenuTitlesGetText(10, 2);
                verifyPoints.Add(summaryTC = "Verify the 2nd menu title is shown after login successfully: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Return (Private) menu is shown after login successfully
                titles = LoginPage.returnPrivate;
                verifyPoint = titles == LoginAction.Instance.MenuTitlesGetText(10, 3);
                verifyPoints.Add(summaryTC = "Verify the 3rd menu title is shown after login successfully: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Risk menu is shown after login successfully
                titles = LoginPage.risk;
                verifyPoint = titles == LoginAction.Instance.MenuTitlesGetText(10, 4);
                verifyPoints.Add(summaryTC = "Verify the 4th menu title is shown after login successfully: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Liquidity menu is shown after login successfully
                titles = LoginPage.liquidity;
                verifyPoint = titles == LoginAction.Instance.MenuTitlesGetText(10, 5);
                verifyPoints.Add(summaryTC = "Verify the 5th menu title is shown after login successfully: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Charts
                // Verify Column chart title
                titles = "Endowment Historical Returns";
                verifyPoint = titles == LoginAction.Instance.ColumnChartTitleGetText(10);
                verifyPoints.Add(summaryTC = "Verify Historical Returns Chart title is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Historical Returns Chart dropdown is shown
                verifyPoint = LoginAction.Instance.IsColumnChartDropdownShown(10);
                verifyPoints.Add(summaryTC = "Verify Historical Returns Chart dropdown is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Historical Returns Chart is shown
                verifyPoint = LoginAction.Instance.IsHistoricalReturnsChartShown(10);
                verifyPoints.Add(summaryTC = "Verify Historical Returns Chart is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Column chart Note
                titles = "Financial Assets"; // KS-666: Renamne "Financial Assets Division" to ""Financial Assets"
                verifyPoint = titles == LoginAction.Instance.ColumnChartNoteGetText(10);
                verifyPoints.Add(summaryTC = "Verify Historical Returns Chart Note is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Pie chart title
                titles = "Asset Class Breakdown";
                verifyPoint = titles == LoginAction.Instance.PieChartTitleGetText(10);
                verifyPoints.Add(summaryTC = "Verify Pie Chart title is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Pies Chart is shown
                verifyPoint = LoginAction.Instance.IsPieChartShown(10);
                verifyPoints.Add(summaryTC = "Verify Pie Chart is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Pie chart Note --> Remove this label (KS-658)
                //titles = "Asset Allocation";
                //verifyPoint = titles == LoginAction.Instance.PieChartNoteGetText(10);
                //verifyPoints.Add(summaryTC = "Verify Pie Chart Note is shown: '" + titles + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                //// Verify Add Filter button is shown after login successfully
                //verifyPoint = LoginAction.Instance.IsAddFilterButtonShown(10);
                //verifyPoints.Add(summaryTC = "Verify Add Filter button is shown after login successfully", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Column Names in Asset Table
                // Verify the 1st column name
                titles = "Asset";
                verifyPoint = titles == LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 2nd column name
                titles = "Benchmark";
                verifyPoint = titles == LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 2);
                verifyPoints.Add(summaryTC = "Verify the 2nd column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 3rd column name
                titles = "Beta";
                verifyPoint = titles == LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 3);
                verifyPoints.Add(summaryTC = "Verify the 3rd column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 4th column name
                titles = "Est Daily NAV";
                verifyPoint = titles == LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 4);
                verifyPoints.Add(summaryTC = "Verify the 4th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 5th column name
                titles = "% of FAD";
                verifyPoint = titles == LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 5);
                verifyPoints.Add(summaryTC = "Verify the 5th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 6th column name
                titles = "MTD";
                verifyPoint = titles == LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 6);
                verifyPoints.Add(summaryTC = "Verify the 6th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 7th column name
                titles = "QTD";
                verifyPoint = titles == LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 7);
                verifyPoints.Add(summaryTC = "Verify the 7th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 8th column name
                titles = "FYTD";
                verifyPoint = titles == LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 8);
                verifyPoints.Add(summaryTC = "Verify the 8th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 9th column name
                titles = "3 Years";
                verifyPoint = titles == LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 9);
                verifyPoints.Add(summaryTC = "Verify the 9th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 10th column name
                titles = "5 Years";
                verifyPoint = titles == LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 10);
                verifyPoints.Add(summaryTC = "Verify the 10th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 11th column name
                titles = "10 Years";
                verifyPoint = titles == LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 11);
                verifyPoints.Add(summaryTC = "Verify the 11th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Row Values in Asset Table
                // Verify the 1st row in Asset column is shown
                titles = "Financial Assets"; // KS-666: Renamne "Financial Assets Division" to ""Financial Assets"
                verifyPoint = LoginAction.Instance.RowValuesInAssetTableGetText(10, 1, 1).Contains(titles);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Asset column is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 1st row in Benchmark column is shown
                titles = "Total FAD Policy Index";
                verifyPoint = titles == LoginAction.Instance.RowValuesInAssetTableGetText(10, 1, 2);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Benchmark column is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                // Click logout
                LoginAction.Instance.ClicklogOut();

                // Verify the login button is shown (Login With Microsoft Account)
                verifyPoint = LoginAction.Instance.IsLoginWithMSAccountBtnShown(10);
                verifyPoints.Add(summaryTC = "Verify Login button (Login With Microsoft Account) is displayed after logout", verifyPoint);
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
