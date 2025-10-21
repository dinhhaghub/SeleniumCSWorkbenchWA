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
        public IWebElement HighlightElement(IWebElement element, string? color = null, string? setOrRemoveAttr = null)
        {
            color ??= "blue";
            setOrRemoveAttr ??= "remove"; // chỉ định hành động "remove" hoặc "keep"

            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Browser;

            // Highlight
            js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", element, "border: 3px solid " + color + ";");
            Thread.Sleep(150);

            // Unhighlight nếu setOrRemoveAttr = "remove"
            if (setOrRemoveAttr.Equals("remove", StringComparison.OrdinalIgnoreCase))
            {
                js.ExecuteScript("arguments[0].removeAttribute('style');", element);
            }

            return element;
        }

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
            var iweb = this.Map.txtUserNameEmail(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Displayed;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute"); // keep highlight in red border if fail
                Driver.TakeScreenShot("ss_IsUserNameEmailTextboxShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsLoginButtonShown(int timeoutInSeconds)
        {
            var iweb = Map.btnLogIn(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Displayed;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute"); // keep highlight in red border if fail
                Driver.TakeScreenShot("ss_IsLoginButtonShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsRequestAnAccountButtonShown(int timeoutInSeconds)
        {
            var iweb = Map.btnRequestAnAccount(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Displayed;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute"); // keep highlight in red border if fail
                Driver.TakeScreenShot("ss_IsRequestAnAccountButtonShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsDynamoSoftwarelearnMoreButtonShown(int timeoutInSeconds)
        {
            var iweb = this.Map.btnDynamoSoftwarelearnMore(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Displayed;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute"); // keep highlight in red border if fail
                Driver.TakeScreenShot("ss_IsDynamoSoftwarelearnMoreButtonShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "remove"); // un-highlight
                return element;
            }
            return element;
        }
        #endregion

        #region Built-in Actions
        #endregion
    }
}
