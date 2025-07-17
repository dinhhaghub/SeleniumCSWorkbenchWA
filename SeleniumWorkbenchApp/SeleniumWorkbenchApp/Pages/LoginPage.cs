using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using WorkbenchApp.UITest.Core.BaseClass;
using WorkbenchApp.UITest.Core.BaseTestCase;
using WorkbenchApp.UITest.Core.Selenium;

namespace WorkbenchApp.UITest.Pages
{
    internal class LoginPage : BasePageElementMap
    {
        // load config.xml file
        internal static readonly string xmlFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Config\" + @"Config.xml");
        internal static XmlDocument xmlDocLoad(string xmlFilePath)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlFilePath);
                return doc;
            }
            catch (Exception e) { return null; }
        }
        internal static string? projectName = null, siteName = null, url = null, instanceName = null, datalakeApi = null, workbenchApi = null;
        internal static XmlDocument configurationFile()
        {
            XmlDocument xdoc = xmlDocLoad(xmlFilePath);
            // Parse an XML file (Get Project & Site name to run)
            foreach (XmlNode node in xdoc.DocumentElement.ChildNodes)
            {
                if (node.Name == "project_name")
                {
                    projectName = node.InnerText;
                    if (projectName == null || projectName == "" || (projectName != "webapp"))
                    {
                        Console.WriteLine("Project '" + projectName + "' does not exist!");
                        break;
                    }
                }
                if (node.Name == "site_name")
                {
                    siteName = node.InnerText;
                }
            }

            // Determine Project & Site name to run
            foreach (XmlNode node in xdoc.DocumentElement.ChildNodes)
            {
                if (node.Name == projectName)
                {
                    foreach (XmlNode siteNode in node.ChildNodes)
                    {
                        if (siteNode.Name == siteName)
                        {
                            url = siteNode["url"].InnerText;
                            instanceName = siteNode["instanceName"].InnerText;
                            datalakeApi = siteNode["webApis"].Attributes["datalakeApi"].Value;
                            workbenchApi = siteNode["webApis"].Attributes["WorkbenchApi"].Value;
                        }
                    }
                }
            }
            return xdoc;
        }

        // Initiate variables
        internal static readonly XDocument xdoc = XDocument.Load(@"Config\Config.xml");
        internal static string email = xdoc.XPathSelectElement("config/account/valid").Attribute("email").Value;
        internal static string password = xdoc.XPathSelectElement("config/account/valid").Attribute("password").Value;
        internal static string username = xdoc.XPathSelectElement("config/account/valid").Attribute("username").Value;
        internal static WebDriverWait? wait;
        internal static string totalEndowment = "Total Endowment";
        internal static string publicFund = "Public Fund";
        internal static string privateFund = "Private Fund";
        internal static string pipeline = "Pipeline";
        internal static string overview = "Overview";
        internal static string returnPublic = "Return (Public)";
        internal static string returnPrivate = "Return (Private)";
        internal static string risk = "Risk";
        internal static string liquidity = "Liquidity";

        // Initiate the By objects for elements
        internal static By loginWithMSAccountBtn = By.XPath(@"//button[.='Login with Microsoft Account']");
        internal static By signInEmailInputTxt = By.Name("loginfmt");
        internal static By nextBtn = By.Id("idSIButton9");
        internal static By passwordInputTxt = By.XPath(@"//input[@type='password']");
        internal static By columnChartTitle = By.XPath(@"//div[@class='historical-returns']//div[@class='chart-title']");
        internal static By columnChartDropdown = By.XPath(@"//div[@class='historical-returns']//p-dropdown");
        internal static By columnChart = By.XPath(@"//div[@class='historical-returns']//canvas");
        internal static By columnChartNote = By.XPath(@"//div[@class='historical-returns']/following-sibling::div[@class='chart-note']");
        internal static By pieChartGrayLoading = By.XPath(@"//app-endowment-overview[@class='ng-star-inserted']//div[@class='grid']/div[2]//p-skeleton");
        internal static By pieChartTitle = By.XPath(@"//div[@class='pie-chart']//div[@class='chart-title']");
        internal static By pieChart = By.XPath(@"//div[@class='pie-chart']//canvas[contains(@style,'display: block')]");
        internal static By pieChartNote = By.XPath(@"//div[@class='pie-chart']/following-sibling::div[@class='chart-note']");
        internal static By assetTable = By.XPath(@"//tbody[@class='p-element p-treetable-tbody']/tr");
        internal static By totalEndowmentTable = By.XPath(@"//table[@class='nav-total-endowment']");
        internal static By searchBoxTxt = By.XPath(@"//input[@role='searchbox']");
        internal static By uploadBtn = By.XPath(@"//button[@icon='pi pi-upload']");
        //internal static By piHeartIcon = By.XPath(@"//button[@icon='pi pi-heart']"); // KS-625 Remove this
        //internal static By piBellIcon = By.XPath(@"//button[@icon='pi pi-bell']"); // KS-625 Remove this
        //internal static By piQuestionCircleIcon = By.XPath(@"//button[@icon='pi pi-question-circle']"); // KS-625 Remove this
        internal static By accountUsername = By.XPath(@"//span[.='" + username + "']");
        internal static By avatarImage = By.XPath(@"//p-avatar//img[@src='/assets/images/avatar2.png']");
        internal static By logOutBtn = By.XPath(@"//span[.='Log Out']");
        internal static By fundSetupBtn = By.XPath(@"//button[@label='Fund Setup']");
        internal static By ownedbyKSCbx = By.XPath(@"//p-checkbox[@label='Owned by KS']");
        internal static By pageTitles(int number) => By.XPath(@"//ul[@role='tablist']/li[" + number + "]/a/span[1]");
        internal static By menuTitles(int number) => By.XPath(@"//div[@role='group']/div[" + number + "]/span[1]");
        internal static By addFilterBtn = By.XPath(@"//button[@label='Add Filter']");
        internal static By columnNamesInAssetTable(int number) => By.XPath(@"//thead[@class='p-treetable-thead']/tr/th[" + number + "]");
        internal static By rowValuesInAssetTable(int row, int column) => By.XPath(@"//tbody[@class='p-element p-treetable-tbody']/tr[" + row + "]/td[" + column + "]");

        // Initiate the elements
        public IWebElement btnLogin(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(loginWithMSAccountBtn));
        }
        public IWebElement txtEmail => Driver.Browser.FindElement(signInEmailInputTxt);
        public IWebElement btnNext(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(nextBtn));
        }
        public IWebElement txtPassword(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(passwordInputTxt));
        }
        public IWebElement btnSignIn(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(nextBtn)); // nextBtn & btnSignIn are the same Id
        }
        public IWebElement txtSearchBox(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(searchBoxTxt));
        }
        public IWebElement btnUpload(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(uploadBtn));
        }
        //public IWebElement iconPiHeart(int timeoutInSeconds) // // KS-625 Remove this
        //{
        //    wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
        //    return wait.Until(ExpectedConditions.ElementIsVisible(piHeartIcon));
        //}
        //public IWebElement iconPiBell(int timeoutInSeconds)
        //{
        //    wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
        //    return wait.Until(ExpectedConditions.ElementIsVisible(piBellIcon));
        //}
        //public IWebElement iconPiQuestionCircle(int timeoutInSeconds)
        //{
        //    wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
        //    return wait.Until(ExpectedConditions.ElementIsVisible(piQuestionCircleIcon));
        //}
        public IWebElement usernameAccount(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(accountUsername));
        }
        public IWebElement imageAvatar(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(avatarImage));
        }
        public IWebElement btnLogOut(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(logOutBtn));
        }
        public IWebElement dropdownLogOut(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(avatarImage));
        }
        public IWebElement btnFundSetup(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(fundSetupBtn));
        }
        public IWebElement cbxOwnedbyKS(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(ownedbyKSCbx));
        }
        public IWebElement titlesPage(int timeoutInSeconds, int number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(pageTitles(number)));
        }
        public IWebElement titlesMenu(int timeoutInSeconds, int number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(menuTitles(number)));
        }
        public IWebElement titleColumnChart(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(columnChartTitle));
        }
        public IWebElement dropdownColumnChart(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(columnChartDropdown));
        }
        public IWebElement chartColumn(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(columnChart));
        }
        public IWebElement noteColumnChart(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(columnChartNote));
        }
        public IWebElement titlePieChart(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(pieChartTitle));
        }
        public IWebElement chartPie(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(pieChart));
        }
        public IWebElement notePieChart(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(pieChartNote));
        }
        public IWebElement btnAddFilter(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(addFilterBtn));
        }
        public IWebElement assetTableColumnNames(int timeoutInSeconds, int number)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(columnNamesInAssetTable(number)));
        }
        public IWebElement assetTableRowValues(int timeoutInSeconds, int row, int column)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(rowValuesInAssetTable(row, column)));
        }
    }

    internal sealed class LoginAction : BasePage<LoginAction, LoginPage>
    {
        #region Constructor
        private LoginAction() { }
        #endregion

        #region Items Action
        // Wait for element visible
        public LoginAction WaitForElementVisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(element));
            }
            return this;
        }

        public LoginAction WaitForNewWindowOpen(int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                int previousWinCount = Driver.Browser.WindowHandles.Count;
                wait.Until(d => d.WindowHandles.Count == previousWinCount + 1);
            }
            return this;
        }

        // Wait for the popup Window closed
        public LoginAction WaitForPopupWindowClosed(int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                int previousWinCount = Driver.Browser.WindowHandles.Count;
                wait.Until(d => d.WindowHandles.Count < 2);
            }
            return this;
        }

        // Checking element exists or not
        public bool IsElementPresent(By by)
        {
            try
            {
                Driver.Browser.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        // Wait for element Invisible
        public LoginAction WaitForElementInvisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(element));
            }
            return this;
        }

        public LoginAction NavigateSite(string url)
        {
            Driver.Browser.Navigate().GoToUrl(string.Concat(url));
            return this;
        }

        public LoginAction ClickLogin(int timeoutInSeconds)
        {
            Map.btnLogin(timeoutInSeconds).Click();
            return this;
        }

        public bool IsLoginWithMSAccountBtnShown(int timeoutInSeconds)
        {
            return Map.btnLogin(timeoutInSeconds).Displayed;
        }

        // Email - Sign in to your account (Microsoft site)
        public LoginAction EnterEmail(string txt)
        {
            Map.txtEmail.SendKeys(txt);
            return this;
        }

        // Next button (Microsoft site - popup 1st)
        public LoginAction ClickNext(int timeoutInSeconds)
        {
            Map.btnNext(timeoutInSeconds).Click();
            return this;
        }
        public LoginAction EnterPassword(int timeoutInSeconds, string txt)
        {
            Map.txtPassword(timeoutInSeconds).SendKeys(txt);
            return this;
        }
        public LoginAction ClickSignIn(int timeoutInSeconds)
        {
            //this.Map.btnSignIn(timeoutInSeconds).Click(); // --> Element Click Intercepted Exception

            // Try with javascript if Element Click Intercepted Exception
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].click();", Map.btnSignIn(timeoutInSeconds));
            return this;
        }

        // Check if the popup MS still being shown, then switch to them main window to refresh browser
        public void CheckIfMSLoginPopupStillShown(int timeout)
        {
            string url = LoginPage.url;
            int winCount = Driver.Browser.WindowHandles.Count;
            string winHandleBefore = Driver.Browser.WindowHandles[0];
            Driver.Browser.SwitchTo().Window(winHandleBefore); // --> Switch to the main window
            Thread.Sleep(3000);
            int time = 0;
            if (winCount == 2 && IsElementPresent(LoginPage.loginWithMSAccountBtn))
            {
                while (winCount == 2 && IsElementPresent(LoginPage.loginWithMSAccountBtn) && time < timeout)
                {
                    ClickLogin(10);
                    
                    if (winCount == 1)
                    {
                        break;
                    }

                    time++;
                    Thread.Sleep(1000);
                }
            }

            winCount = Driver.Browser.WindowHandles.Count;
            if (winCount == 2 && IsElementPresent(LoginPage.avatarImage)) 
            {
                // Close MS Login popup
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles[1]).Close(); Thread.Sleep(500);
            }

            // Switch to the main window
            Driver.Browser.SwitchTo().Window(winHandleBefore);
            Thread.Sleep(500);
        }

        // verify elements
        public bool IsSearchBoxShown(int timeoutInSeconds)
        {
            return Map.txtSearchBox(timeoutInSeconds).Displayed;
        }
        public bool IsUploadButtonShown(int timeoutInSeconds)
        {
            return Map.btnUpload(timeoutInSeconds).Displayed;
        }
        //public bool IsPiHeartIconShown(int timeoutInSeconds) // // KS-625 Remove this
        //{
        //    return Map.iconPiHeart(timeoutInSeconds).Displayed;
        //}
        //public bool IsPiBellIconShown(int timeoutInSeconds)
        //{
        //    return Map.iconPiBell(timeoutInSeconds).Displayed;
        //}
        //public bool IsPiQuestionCircleIconShown(int timeoutInSeconds)
        //{
        //    return Map.iconPiQuestionCircle(timeoutInSeconds).Displayed;
        //}
        public bool IsUsernameAccountShown(int timeoutInSeconds)
        {
            return Map.usernameAccount(timeoutInSeconds).Displayed;
        }
        public bool IsAvatarImageShown(int timeoutInSeconds)
        {
            return Map.imageAvatar(timeoutInSeconds).Displayed;
        }
        public bool IsFundSetupButtonShown(int timeoutInSeconds)
        {
            return Map.btnFundSetup(timeoutInSeconds).Displayed;
        }
        public bool IsOwnedbyKSCheckboxShown(int timeoutInSeconds)
        {
            return Map.cbxOwnedbyKS(timeoutInSeconds).Displayed;
        }
        public string PageTitlesGetText(int timeoutInSeconds, int number)
        {
            return Map.titlesPage(timeoutInSeconds, number).Text;
        }
        public string MenuTitlesGetText(int timeoutInSeconds, int number)
        {
            return Map.titlesMenu(timeoutInSeconds, number).Text;
        }
        public string ColumnChartTitleGetText(int timeoutInSeconds)
        {
            return Map.titleColumnChart(timeoutInSeconds).Text;
        }
        public bool IsColumnChartDropdownShown(int timeoutInSeconds)
        {
            return Map.dropdownColumnChart(timeoutInSeconds).Displayed;
        }
        public bool IsHistoricalReturnsChartShown(int timeoutInSeconds)
        {
            return Map.chartColumn(timeoutInSeconds).Displayed;
        }
        public string ColumnChartNoteGetText(int timeoutInSeconds)
        {
            return Map.noteColumnChart(timeoutInSeconds).Text;
        }
        public string PieChartTitleGetText(int timeoutInSeconds)
        {
            return Map.titlePieChart(timeoutInSeconds).Text;
        }
        public bool IsPieChartShown(int timeoutInSeconds)
        {
            return Map.chartPie(timeoutInSeconds).Displayed;
        }
        public string PieChartNoteGetText(int timeoutInSeconds)
        {
            return Map.notePieChart(timeoutInSeconds).Text;
        }
        public bool IsAddFilterButtonShown(int timeoutInSeconds)
        {
            return Map.btnAddFilter(timeoutInSeconds).Displayed;
        }
        public string ColumnNamesInAssetTableGetText(int timeoutInSeconds, int number)
        {
            return Map.assetTableColumnNames(timeoutInSeconds, number).Text;
        }
        public string RowValuesInAssetTableGetText(int timeoutInSeconds, int row, int column)
        {
            return Map.assetTableRowValues(timeoutInSeconds, row, column).Text;
        }

        // Actions
        public LoginAction ClickFundSetupButton(int timeoutInSeconds)
        {
            Map.btnFundSetup(timeoutInSeconds).Click();
            return this;
        }
        public LoginAction ClickOwnedbyKSCheckbox(int timeoutInSeconds)
        {
            Map.cbxOwnedbyKS(timeoutInSeconds).Click();
            return this;
        }
        #endregion

        #region Built-in Actions
        public LoginAction ClicklogOut()
        {
            //this.Map.dropdownLogOut(10).Click();
            Map.imageAvatar(10).Click();
            WaitForElementVisible(10, LoginPage.logOutBtn);
            Map.btnLogOut(10).Click();
            return this;
        }

        public LoginAction LoginSiteNoGodaddy(int timeoutInSeconds, string? url = null)
        {
            // Check if user login with a specific url, if no then will get url from config.xml (by default)
            if (url == null) url = LoginPage.url;

            // Variables declare
            string email = LoginPage.email;
            string password = LoginPage.password;

            // Steps
            try
            {
                NavigateSite(url);
                WaitForElementVisible(timeoutInSeconds, LoginPage.loginWithMSAccountBtn); // 60

                // Store the current window handle (main window - Workbench)
                string winHandleBefore = Driver.Browser.CurrentWindowHandle;

                ClickLogin(timeoutInSeconds); // 10
                WaitForNewWindowOpen(timeoutInSeconds); // 15

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

                // Wait for the element of new window is opened (Email text field)
                WaitForElementVisible(timeoutInSeconds, LoginPage.signInEmailInputTxt); // 10

                // Enter Email and then click Next button
                EnterEmail(email).ClickNext(10);

                // Switch to new window opened
                Driver.Browser.SwitchTo().Window(winHandleLast);
                System.Threading.Thread.Sleep(3000); //WaitForElementVisible(10, LoginPage.signInBtn);

                // Enter Password and then Click on SignIn button ...
                EnterPassword(timeoutInSeconds, password) // 10
                .ClickSignIn(timeoutInSeconds) // 10
                .ClickNext(timeoutInSeconds); // 10

                // Check if the popup MS still being shown, then switch to them main window to click Login button again
                CheckIfMSLoginPopupStillShown(10); // 20

                // Wait for buttons is shown
                WaitForElementInvisible(timeoutInSeconds, LoginPage.pieChartGrayLoading) // 20
                                    .WaitForElementVisible(timeoutInSeconds, LoginPage.pieChart) // 10
                                    .WaitForElementVisible(timeoutInSeconds, LoginPage.assetTable) // 10
                                    .WaitForElementVisible(timeoutInSeconds, LoginPage.totalEndowmentTable); // 10
            }
            catch (Exception exception)
            {
                // Print exception
                Console.WriteLine(exception);

                // Warning
                BaseTestCase.ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong with Microsoft Login! Please check console log.");
            }

            return this;
        }
        #endregion
    }
}
