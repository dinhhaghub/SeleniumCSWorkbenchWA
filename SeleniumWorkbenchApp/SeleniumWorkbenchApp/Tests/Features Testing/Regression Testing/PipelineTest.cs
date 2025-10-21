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
    [TestFixture, Order(12)]
    internal class PipelineTest : BaseTestCase
    {
        // Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;

        [Test, Category("Regression Testing")]
        public void TC001_Pipeline_Page()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string? datePipeline = null;
            string? on2_OnePager = null;
            string? on3_Memo = null;
            string? on1_PreOnePager = null;
            string videoFileName = "PipelineTestTC001";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Pipeline Page - TC001");
            try 
            {
                // Start recording video
                Driver.StartVideoRecord(videoFileName);

                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Go to Pipeline page
                NavigationAction.Instance.ClickPageNames(10, Navigation.pipeline);

                // Wait for Pipeline table is displayed
                GeneralAction.Instance.WaitForElementVisible(10, PipelinePage.pipelineTable); Thread.Sleep(1000);

                if (urlInstance.Contains("lab")) 
                {
                    datePipeline = "2025";
                    on2_OnePager = "10";
                    on3_Memo = "3";
                    on1_PreOnePager = "89";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    datePipeline = "2025";
                    on2_OnePager = "10";
                    on3_Memo = "3";
                    on1_PreOnePager = "89";
                }

                #region Verify Data in 'Pipeline' table header
                verifyPoint = GeneralAction.Instance.DateInTableHeaderNameGetText(10, PipelinePage.pipelineHeadertable, datePipeline);
                verifyPoints.Add(summaryTC = "Verify date of '" + PipelinePage.pipelineHeadertable + "' in the table header is shown: '" + datePipeline + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                verifyPoint = Convert.ToInt64(GeneralAction.Instance.DataInTableHeaderGetText(10, PipelinePage.pipelineHeadertable, Navigation.on2_OnePager)) > 0
                    && Convert.ToInt64(GeneralAction.Instance.DataInTableHeaderGetText(10, PipelinePage.pipelineHeadertable, Navigation.on2_OnePager)) < 100000000;
                verifyPoints.Add(summaryTC = "Verify data of '" + PipelinePage.pipelineHeadertable + "' - '" + Navigation.on2_OnePager + "' is shown: '" + on2_OnePager + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot(PipelinePage.pipelineHeadertable + "_" + Navigation.on2_OnePager + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToInt64(GeneralAction.Instance.DataInTableHeaderGetText(10, PipelinePage.pipelineHeadertable, Navigation.on3_Memo)) > 0
                    && Convert.ToInt64(GeneralAction.Instance.DataInTableHeaderGetText(10, PipelinePage.pipelineHeadertable, Navigation.on3_Memo)) < 100000000;
                verifyPoints.Add(summaryTC = "Verify data of '" + PipelinePage.pipelineHeadertable + "' - '" + Navigation.on3_Memo + "' is shown: '" + on3_Memo + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot(PipelinePage.pipelineHeadertable + "_" + Navigation.on3_Memo + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = Convert.ToInt64(GeneralAction.Instance.DataInTableHeaderGetText(10, PipelinePage.pipelineHeadertable, Navigation.on1_PreOnePager)) > 0
                   && Convert.ToInt64(GeneralAction.Instance.DataInTableHeaderGetText(10, PipelinePage.pipelineHeadertable, Navigation.on1_PreOnePager)) < 100000000;
                verifyPoints.Add(summaryTC = "Verify data of '" + PipelinePage.pipelineHeadertable + "' - '" + Navigation.on1_PreOnePager + "' is shown: '" + on1_PreOnePager + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot(PipelinePage.pipelineHeadertable + "_" + Navigation.on1_PreOnePager + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                #endregion

                #region Verify Column Names in Asset Table
                // Verify Column Names in Asset Table
                string title = "Fund Name";
                verifyPoint = title == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 1);
                verifyPoints.Add(summaryTC = "Verify the 1st column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("Pipeline_HeaderInAssetTable_" + title + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (title= "Manager Name") == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 2);
                verifyPoints.Add(summaryTC = "Verify the 2nd column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("Pipeline_HeaderInAssetTable_" + title + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (title = "Asset Class") == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 3);
                verifyPoints.Add(summaryTC = "Verify the 3rd column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("Pipeline_HeaderInAssetTable_" + title + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (title = "Sub Assetclass 1") == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 4);
                verifyPoints.Add(summaryTC = "Verify the 4th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("Pipeline_HeaderInAssetTable_" + title + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (title = "Geography") == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 5);
                verifyPoints.Add(summaryTC = "Verify the 5th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("Pipeline_HeaderInAssetTable_" + title + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }

                verifyPoint = (title = "Responsible") == GeneralAction.Instance.ColumnNamesInAssetTableGetText(10, 6);
                verifyPoints.Add(summaryTC = "Verify the 6th column name in Asset table is shown: '" + title + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                if (verifyPoint == false) { Driver.TakeScreenShot("Pipeline_HeaderInAssetTable_" + title + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt")); }
                #endregion

                #region Verify Row Values in Asset Table
                // Verify row values in Asset column
                string getCountOnePager = GeneralAction.Instance.DataInTableHeaderGetText(10, PipelinePage.pipelineHeadertable, Navigation.on2_OnePager);
                string assetNameStartsWith = "One Pager (";
                verifyPoint = GeneralAction.Instance.RowNameInAssetTableGetText(10, assetNameStartsWith, title = assetNameStartsWith + getCountOnePager + ")"); // RowValuesInAssetTableGetText(10, 1, 1);
                verifyPoints.Add(summaryTC = "Verify The total value in the 'One Pager (total)' group displays correctly: '" + title + "'", verifyPoint);
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
