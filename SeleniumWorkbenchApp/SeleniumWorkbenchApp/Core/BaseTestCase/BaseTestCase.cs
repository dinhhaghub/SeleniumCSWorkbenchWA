using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WorkbenchApp.UITest.Core.Selenium;
using WorkbenchApp.UITest.Pages;

namespace WorkbenchApp.UITest.Core.BaseTestCase
{
    [TestFixture]
    internal class BaseTestCase
    {
        private TestContext? context;
        public static bool verifyPoint;
        public static Dictionary<string, bool> verifyPoints = new Dictionary<string, bool>();

        [Obsolete]
        internal static ExtentSparkReporter? htmlReporter; // ExtentHtmlReporter (V4), ExtentV3HtmlReporter (V3)
        public static ExtentReports? extent;
        public static ExtentTest? test;

        public TestContext TestContext
        {
            get { return context; }
            set { context = value; }
        }

        [SetUp]
        public virtual void SetupTest()
        {
            verifyPoints.Clear();
            Driver.StartBrowser();
            LoginPage.configurationFile();
        }

        [Obsolete]
        public static ExtentReports ExtReportgetInstance(string? fileName = null)
        {
            // Default fileName is 'Results'
            if (fileName == null) fileName = "Results";

            if (extent == null)
            {
                LoginPage.configurationFile();
                string instanceName = LoginPage.instanceName;
                string fileExportPath = Path.GetFullPath(@"../../../../TestResults/");
                string reportFile = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_") + @"_" + fileName + ".html";
                htmlReporter = new ExtentSparkReporter(fileExportPath + reportFile); // ExtentV3HtmlReporter
                extent = new ExtentReports();
                extent.AttachReporter(htmlReporter);
                extent.AddSystemInfo("OS", "Windows");
                extent.AddSystemInfo("Host Name", "GenD-KS");
                extent.AddSystemInfo("Environment", instanceName);

                // Load Extent Report Config xml file
                string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Reports\");
                string extentConfigPath = filePath + "ExtentReportConfig.xml";
                htmlReporter.LoadConfig(extentConfigPath);
            }
            return extent;
        }

        public static void ExtReportResult(bool verifyPoint, string descriptionTC)
        {
            if (verifyPoint == false)
            {
                test.Log(Status.Fail, descriptionTC);
            }
            if (verifyPoint == true)
            {
                test.Log(Status.Pass, descriptionTC);
            }
        }

        [TearDown]
        public virtual void TeardownTest()
        {
            Driver.StopBrowser();
            EndReport();

            #region Result (Passed or Failed)
            bool result = true;
            foreach (var verify in verifyPoints)
            {
                string step_result = verify.Value ? "Pass" : "Fail";
                Console.WriteLine(verify.Key + " : " + step_result);
                result = result && verify.Value;
            }
            Assert.That(result);
            #endregion
        }

        public void EndReport()
        {
            // Write content to report file
            extent.Flush();

            // Get file (Result) path
            string fileExportPath = Path.GetFullPath(@"../../../../TestResults/");

            // Rename a file (default name of file result is 'index.html') --> This only applies to ExtentReport V4
            if (File.Exists(fileExportPath + "index.html"))
            {
                string fileName = "Results";
                string reportFile = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_") + @"_" + fileName + ".html";
                File.Move(fileExportPath + "index.html", fileExportPath + reportFile);
            }

            // Open Report html file after executing all TCs
            var file = new DirectoryInfo(fileExportPath).GetFiles("*.html").OrderByDescending(f => f.LastWriteTime).First();
            var chrome64 = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            var chrome32 = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            if (File.Exists(chrome64))
            {
                //Solution 1:
                //System.Diagnostics.Process.Start(chrome64, " --start-maximized " + @"" + file.ToString() + "" + " --incognito");

                //Solution 2:
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Minimized;
                startInfo.FileName = chrome64;
                startInfo.Arguments = " --start-maximized " + @"" + file.ToString() + "" + " --incognito";
                process.StartInfo = startInfo;
                process.Start();
                Thread.Sleep(1000);
                IntPtr hwnd = FindWindowByCaption(IntPtr.Zero, "AutomationTesting.in - Google Chrome");
                ShowWindow(hwnd, 6); // minimize = 6; maximize =3
            }
            if (File.Exists(chrome32))
            {
                //Solution 1:
                //System.Diagnostics.Process.Start(chrome32, " --start-maximized " + @"" + file.ToString() + "" + " --incognito");

                //Solution 2:
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = chrome32;
                startInfo.Arguments = " --start-maximized " + @"" + file.ToString() + "" + " --incognito";
                process.StartInfo = startInfo;
                process.Start();
                Thread.Sleep(1000);
                IntPtr hwnd = FindWindowByCaption(IntPtr.Zero, "AutomationTesting.in - Google Chrome");
                ShowWindow(hwnd, 6); // minimize = 6; maximize =3
            }
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
    }
}
