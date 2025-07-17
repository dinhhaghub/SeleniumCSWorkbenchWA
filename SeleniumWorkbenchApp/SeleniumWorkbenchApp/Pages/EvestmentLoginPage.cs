using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using WorkbenchApp.UITest.Core.BaseClass;
using WorkbenchApp.UITest.Core.Selenium;

namespace WorkbenchApp.UITest.Pages
{
    internal class EvestmentLoginPage : BasePageElementMap
    {
        // Initiate variables
        internal static WebDriverWait? wait;

        // Initiate the By objects for elements
        internal static By loggingInText = By.XPath(@"//div[contains(.,'Logging in...')]");
        internal static By loadingIcon = By.XPath(@"//button[contains(@style, 'display: none')]");
        internal static By userNameEmailTxt = By.XPath(@"//input[@placeholder='username/email']");
        internal static By logInBtn = By.XPath(@"//button[@name='submit' and contains(@style, 'display: block')]");
        internal static By requestAnAccountBtn = By.XPath(@"//div[@class='actions']/a[3][contains(., 'Request An Account')]");
        internal static By dynamoSoftwarelearnMoreBtn = By.Id("learnMoreButton");

        // Initiate the elements
        public IWebElement txtUserNameEmail(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(userNameEmailTxt));
        }
        public IWebElement btnLogIn(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(logInBtn));
        }
        public IWebElement btnRequestAnAccount(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(requestAnAccountBtn));
        }
        public IWebElement btnDynamoSoftwarelearnMore(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(dynamoSoftwarelearnMoreBtn));
        }
    }

    internal sealed class EvestmentLoginAction : BasePage<EvestmentLoginAction, EvestmentLoginPage>
    {
        #region Constructor
        private EvestmentLoginAction() {}
        #endregion

        #region Items Action
        // Wait for the new tab
        public EvestmentLoginAction WaitForNewTab(int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(d => d.WindowHandles.Count == 2);
            }
            return this;
        }

        // verify elements
        public bool IsUserNameEmailTextboxShown(int timeoutInSeconds)
        {
            return this.Map.txtUserNameEmail(timeoutInSeconds).Displayed;
        }
        public bool IsLoginButtonShown(int timeoutInSeconds)
        {
            return this.Map.btnLogIn(timeoutInSeconds).Displayed;
        }
        public bool IsRequestAnAccountButtonShown(int timeoutInSeconds)
        {
            return this.Map.btnRequestAnAccount(timeoutInSeconds).Displayed;
        }
        public bool IsDynamoSoftwarelearnMoreButtonShown(int timeoutInSeconds)
        {
            return this.Map.btnDynamoSoftwarelearnMore(timeoutInSeconds).Displayed;
        }
        #endregion

        #region Built-in Actions
        #endregion
    }
}
