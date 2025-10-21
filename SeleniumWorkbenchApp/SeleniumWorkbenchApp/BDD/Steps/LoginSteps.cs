using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using System.Security.Policy;
using System.Xml.Linq;
using WorkbenchApp.FunctionalTest;
using WorkbenchApp.UITest.Core;
using WorkbenchApp.UITest.Core.BaseTestCase;
using WorkbenchApp.UITest.Core.Selenium;
using WorkbenchApp.UITest.Pages;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WorkbenchApp.UITest.BDD.Steps
{
    [Binding]
    internal class LoginSteps : BaseTestCase
    {
        private static readonly string? url = LoginPage.url;

        [Obsolete]
        private readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;
        private string videoFileName = "LoginTest";

        public LoginSteps()
        {
            // Start recording video
            Driver.StartVideoRecord(videoFileName);

            test = rep.CreateTest("WorkbenchApp - Login Test");
        }

        [Given(@"I open the login page")]
        public void GivenIOpenTheLoginPage()
        {
            #region Variables declare
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            try
            {
                LoginAction.Instance.NavigateSite(url);
            }
            catch (Exception exception)
            {
                // Stop recording video
                Driver.StopVideoRecord();

                // Take screenshot
                Driver.TakeScreenShot("LoginTest_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));

                // Print exception
                System.Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                throw new Exception("Something wrong! Please check console log.");
            }
            #endregion
        }

        [When(@"I login with credentials ""(.*)""")]
        public async Task WhenILoginWithCredentialsAsync(string credentialKey)
        {
            #region Variables declare
            string email = LoginPage.email;
            string password = LoginPage.password;
            string username = LoginPage.username;
            string clientId = LoginPage.clientId;
            string tenantId = LoginPage.tenantId;
            string redirectUri = LoginPage.redirectUri;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            try
            {
                // Check if the environment is Production (Godaddy) then use MSIdentityClientAuthentcation to login
                if (url.Contains("ksbeimc"))
                {
                    // Gọi phương thức Login (Microsoft.Identity.Client.AuthentcationResult) để lấy token
                    var authResult = await Config.LoginAsync(email, password, clientId, tenantId, redirectUri);

                    if (authResult != null)
                    {
                        // Lấy idToken & accessToken từ MSAL
                        string idToken = authResult.IdToken;
                        string accessToken = authResult.AccessToken;

                        // Mở site
                        LoginAction.Instance.NavigateSite(url);

                        // Inject token vào localStorage
                        IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Browser;
                        js.ExecuteScript($@"window.localStorage.setItem('loggedIn', 'true');
                                        window.localStorage.setItem('msal.idtoken', '{idToken}');
                                        window.localStorage.setItem('msal.{clientId}.idtoken', '{idToken}');
                                        window.localStorage.setItem('msal.{clientId}.accesstoken', '{accessToken}');
                                        window.localStorage.setItem('GenD_Authenticated', JSON.stringify({{""uniqueId"":""{authResult.Account.HomeAccountId.Identifier}"",""tokenType"":""id_token""}}));
                                    ");

                        // Refresh lại để apply
                        Driver.Browser.Navigate().Refresh();

                        // Wait for Pie Chart, Asset and Total Endowment table are loaded Done
                        LoginAction.Instance.WaitForElementInvisible(30, LoginPage.pieChartGrayLoading)
                                            .WaitForElementVisible(30, LoginPage.pieChart)
                                            .WaitForElementVisible(30, LoginPage.assetTable)
                                            .WaitForElementVisible(30, LoginPage.totalEndowmentTable);
                    }
                }
                else 
                {
                    LoginAction.Instance.NavigateSite(url);

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
                }
            }
            catch (Exception exception)
            {
                // Stop recording video
                Driver.StopVideoRecord();

                // Take screenshot
                Driver.TakeScreenShot("LoginTest_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));

                // Print exception
                System.Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                throw new Exception($"Login failed with credentials {credentialKey}: {exception.Message}", exception);
            }
            #endregion
        }

        [Then(@"I should see the dashboard page")]
        public void ThenIShouldSeeTheDashboardPage()
        {
            #region Variables declare
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            try
            {
                #region Verify all buttons, icons ... are displayed after login successfully
                //Assert.That(LoginAction.Instance.IsSearchBoxShown(10), Is.True, "Dashboard page was not displayed after login.");

                // Verify Search Box is shown after login successfully
                verifyPoint = LoginAction.Instance.IsSearchBoxShown(10);
                verifyPoints.Add(summaryTC = "Verify Search Box is shown after login successfully", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Upload button is shown after login successfully
                verifyPoint = LoginAction.Instance.IsUploadButtonShown(10);
                verifyPoints.Add(summaryTC = "Verify Upload button is shown after login successfully", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

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
                verifyPoint = LoginAction.Instance.PageTitlesGetText(10, 1, titles);
                verifyPoints.Add(summaryTC = "Verify the 1st page title is shown after login successfully: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Public Fund Page titles are shown after login successfully
                titles = LoginPage.publicFund;
                verifyPoint = LoginAction.Instance.PageTitlesGetText(10, 2, titles);
                verifyPoints.Add(summaryTC = "Verify the 2nd page title is shown after login successfully: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Private Fund Page titles are shown after login successfully
                titles = LoginPage.privateFund;
                verifyPoint = LoginAction.Instance.PageTitlesGetText(10, 3, titles);
                verifyPoints.Add(summaryTC = "Verify the 3rd page title is shown after login successfully: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Pipeline Page titles are shown after login successfully
                titles = LoginPage.pipeline;
                verifyPoint = LoginAction.Instance.PageTitlesGetText(10, 4, titles);
                verifyPoints.Add(summaryTC = "Verify the 4th page title is shown after login successfully: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Menu titles are shown after login successfully
                // Verify Overview menu is shown after login successfully
                titles = LoginPage.overview;
                verifyPoint = LoginAction.Instance.MenuTitlesGetText(10, 1, titles);
                verifyPoints.Add(summaryTC = "Verify the 1st menu title is shown after login successfully: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Return (Public) menu is shown after login successfully
                titles = LoginPage.returnPublic;
                verifyPoint = LoginAction.Instance.MenuTitlesGetText(10, 2, titles);
                verifyPoints.Add(summaryTC = "Verify the 2nd menu title is shown after login successfully: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Return (Private) menu is shown after login successfully
                titles = LoginPage.returnPrivate;
                verifyPoint = LoginAction.Instance.MenuTitlesGetText(10, 3, titles);
                verifyPoints.Add(summaryTC = "Verify the 3rd menu title is shown after login successfully: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Risk menu is shown after login successfully
                titles = LoginPage.risk;
                verifyPoint = LoginAction.Instance.MenuTitlesGetText(10, 4, titles);
                verifyPoints.Add(summaryTC = "Verify the 4th menu title is shown after login successfully: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Liquidity menu is shown after login successfully
                titles = LoginPage.liquidity;
                verifyPoint = LoginAction.Instance.MenuTitlesGetText(10, 5, titles);
                verifyPoints.Add(summaryTC = "Verify the 5th menu title is shown after login successfully: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Charts
                // Verify Column chart title
                titles = "Endowment Historical Returns";
                verifyPoint = LoginAction.Instance.ColumnChartTitleGetText(10, titles);
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
                verifyPoint = LoginAction.Instance.ColumnChartNoteGetText(10, titles);
                verifyPoints.Add(summaryTC = "Verify Historical Returns Chart Note is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify Pie chart title
                titles = "Asset Class Breakdown";
                verifyPoint = LoginAction.Instance.PieChartTitleGetText(10, titles);
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
                verifyPoint = LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 1, titles);
                verifyPoints.Add(summaryTC = "Verify the 1st column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 2nd column name
                titles = "Benchmark";
                verifyPoint = LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 2, titles);
                verifyPoints.Add(summaryTC = "Verify the 2nd column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 3rd column name
                titles = "Beta";
                verifyPoint = LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 3, titles);
                verifyPoints.Add(summaryTC = "Verify the 3rd column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 4th column name
                titles = "Est Daily NAV";
                verifyPoint = LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 4, titles);
                verifyPoints.Add(summaryTC = "Verify the 4th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 5th column name
                titles = "% of FAD";
                verifyPoint = LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 5, titles);
                verifyPoints.Add(summaryTC = "Verify the 5th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 6th column name
                titles = "MTD";
                verifyPoint = LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 6, titles);
                verifyPoints.Add(summaryTC = "Verify the 6th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 7th column name
                titles = "QTD";
                verifyPoint = LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 7, titles);
                verifyPoints.Add(summaryTC = "Verify the 7th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 8th column name
                titles = "FYTD";
                verifyPoint = LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 8, titles);
                verifyPoints.Add(summaryTC = "Verify the 8th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 9th column name
                titles = "3 Years";
                verifyPoint = LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 9, titles);
                verifyPoints.Add(summaryTC = "Verify the 9th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 10th column name
                titles = "5 Years";
                verifyPoint = LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 10, titles);
                verifyPoints.Add(summaryTC = "Verify the 10th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 11th column name
                titles = "10 Years";
                verifyPoint = LoginAction.Instance.ColumnNamesInAssetTableGetText(10, 11, titles);
                verifyPoints.Add(summaryTC = "Verify the 11th column name in Asset table is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Row Values in Asset Table
                // Verify the 1st row in Asset column is shown
                titles = "Financial Assets"; // KS-666: Renamne "Financial Assets Division" to ""Financial Assets"
                verifyPoint = LoginAction.Instance.RowValuesInAssetTableGetText(10, 1, 1, titles);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Asset column is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify the 1st row in Benchmark column is shown
                titles = "Total FAD Policy Index";
                verifyPoint = LoginAction.Instance.RowValuesInAssetTableGetText(10, 1, 2, titles);
                verifyPoints.Add(summaryTC = "Verify the 1st row value in Benchmark column is shown: '" + titles + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Logout
                // Click logout
                LoginAction.Instance.ClicklogOut();

                // Verify the login button is shown (Login With Microsoft Account)
                verifyPoint = LoginAction.Instance.IsLoginWithMSAccountBtnShown(10);
                verifyPoints.Add(summaryTC = "Verify Login button (Login With Microsoft Account) is displayed after logout", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
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

                // Take screenshot
                Driver.TakeScreenShot("LoginTest_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));

                // Print exception
                System.Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                throw new Exception("Something wrong! Please check console log.");
            }
            #endregion
        }
    }
}
