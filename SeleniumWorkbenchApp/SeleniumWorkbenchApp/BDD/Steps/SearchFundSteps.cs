using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using WorkbenchApp.FunctionalTest;
using WorkbenchApp.UITest.Core.BaseTestCase;
using WorkbenchApp.UITest.Core.Selenium;
using WorkbenchApp.UITest.Generals;
using WorkbenchApp.UITest.Pages;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WorkbenchApp.UITest.BDD.Steps
{
    [Binding]
    internal class SearchFundSteps : BaseTestCase
    {
        private static readonly string? url = LoginPage.url;
        [Obsolete]
        private readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;
        private string videoFileName = "SearchFundTest";

        public SearchFundSteps()
        {
            // Start recording video
            Driver.StartVideoRecord(videoFileName);

            test = rep.CreateTest("WorkbenchApp - Search Test");
        }

        [Given(@"I am logged in and on the Total Endowment page")]
        public async Task GivenIAmLoggedInAndOnTheTotalEndowmentPageAsync()
        {
            #region Variables declare
            string email = LoginPage.email;
            string password = LoginPage.password;
            string username = LoginPage.username;
            string clientId = LoginPage.clientId;
            string tenantId = LoginPage.tenantId;
            string redirectUri = LoginPage.redirectUri;
            //const string ownedByKSText = "Owned by KS";
            #endregion

            #region Workflow scenario
            try
            {
                #region login steps
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
                #endregion
            }
            catch (Exception exception)
            {
                // Stop recording video
                Driver.StopVideoRecord();

                // Take screenshot
                Driver.TakeScreenShot("SearchTest_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));

                // Print exception
                System.Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                throw new Exception("Something wrong! Please check console log.");
            }
            #endregion
        }

        [When(@"I search for the Albourne fund")]
        public void WhenISearchForTheAlbourneFund()
        {
            #region Variables declare
            const string managerName = "Citadel Advisors LLC";
            const string fundName = "Citadel Multi Strategy Funds";
            const string sourceIcon = "A";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            try
            {
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
            }
            catch (Exception exception)
            {
                // Stop recording video
                Driver.StopVideoRecord();

                // Take screenshot
                Driver.TakeScreenShot("SearchTest_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));

                // Print exception
                System.Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                throw new Exception("Something wrong! Please check console log.");
            }
            #endregion
        }

        [Then(@"I should see the Albourne fund in the new tab")]
        public void ThenIShouldSeeTheAlbourneFundInTheNewTab()
        {
            #region Variables declare
            //const string managerName = "Citadel Advisors LLC";
            const string fundName = "Citadel Multi Strategy Funds";
            //const string sourceIcon = "A";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            try
            {
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
                if (url.Contains("lab"))
                {
                    // Upload Source: Manual
                    /// Exposure Manual (index = 1)
                    verifyPoint = SearchFundAction.Instance.ValueDataStatusGetText(10, 1, SearchFundPage.rowCounts, rowCount = "1")
                        && SearchFundAction.Instance.ValueDataStatusGetText(10, 1, SearchFundPage.startDate, startDate = "04/30/2020")
                        && SearchFundAction.Instance.ValueDataStatusGetText(10, 1, SearchFundPage.endDate, endDate = "04/30/2020");
                    verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Manual' - Exposure: '" + rowCount + "'; '" + startDate + "'; '" + endDate + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    /// Verify Fund Returns Manual (index = 2)
                    /// Fund AUM Manual (index = 3)
                    verifyPoint = SearchFundAction.Instance.ValueDataStatusGetText(10, 3, SearchFundPage.rowCounts, rowCount = "285")
                        && SearchFundAction.Instance.ValueDataStatusGetText(10, 3, SearchFundPage.startDate, startDate = "01/31/1998")
                        && SearchFundAction.Instance.ValueDataStatusGetText(10, 3, SearchFundPage.endDate, endDate = "09/30/2021");
                    verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Manual' - Fund AUM: '" + rowCount + "'; '" + startDate + "'; '" + endDate + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Upload Source: Albourne
                    /// Exposure Albourne (index = 6)
                    /// Fund Returns Albourne (index = 7)
                    /// Fund AUM Albourne (index = 8)
                }
                if (url.Contains("conceptia"))
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
                verifyPoint = SearchFundAction.Instance.ValueDataStatusGetText(10, 2, SearchFundPage.rowCounts, rowCount = "314")
                        && SearchFundAction.Instance.ValueDataStatusGetText(10, 2, SearchFundPage.startDate, startDate = "07/31/1995")
                        && SearchFundAction.Instance.ValueDataStatusGetText(10, 2, SearchFundPage.endDate, endDate = "08/31/2021");
                verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Manual' - Fund Returns: '" + rowCount + "'; '" + startDate + "'; '" + endDate + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Fund AUM Manual (index = 3) --> No verify

                // Upload Source: Albourne
                /// Exposure Albourne (index = 6) --> None
                /// Fund Returns Albourne (index = 7)
                verifyPoint = Convert.ToInt64(SearchFundAction.Instance.ValueDataStatusGetText(10, 7, SearchFundPage.rowCounts)) > 100
                    && SearchFundAction.Instance.ValueDataStatusGetText(10, 7, SearchFundPage.startDate, startDate = "07/31/1995")
                    && SearchFundAction.Instance.ValueDataStatusGetText(10, 7, SearchFundPage.endDate).Contains(endDate = "2025");
                verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Albourne' - Fund Returns: 'RowCounts > 100'; '" + startDate + "'; 'EndDate contains (" + endDate + ")'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                /// Fund AUM Albourne (index = 8)
                verifyPoint = Convert.ToInt64(SearchFundAction.Instance.ValueDataStatusGetText(10, 8, SearchFundPage.rowCounts)) > 100
                    && SearchFundAction.Instance.ValueDataStatusGetText(10, 8, SearchFundPage.startDate, startDate = "01/31/1998")
                    && SearchFundAction.Instance.ValueDataStatusGetText(10, 8, SearchFundPage.endDate).Contains(endDate = "202");
                verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Albourne' - Fund AUM: 'RowCounts > 100'; '" + startDate + "'; 'EndDate contains (" + endDate + ")'", verifyPoint);
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
                Driver.TakeScreenShot("SearchTest_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));

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
