using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorkbenchApp.FunctionalTest.PredefinedScenarios
{
    [TestFixture]
    internal class PrivateReportTests : BaseFunctionTest
    {
        #region Initiate variables
        internal static string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"PredefinedScenarios\Documents\");
        internal static string? fileName = null;
        #endregion

        #region TestMethod
        [Test, Category("API Smoke Tests")]
        public void ST001_PrivateReport_SourceManual()
        {
            #region Variables declare
            const string custom_risk_benchmarks = "[{"
                                                  + "\n" + "\"benchmark_id\"" + " : " + "\"" + "SPTR Index.USD" + "\","
                                                  + "\n" + "\"beta\"" + " : " + "1,"
                                                  + "\n" + "\"exposure\"" + " : " + "0.8,"
                                                  + "\n" + "\"return_type\"" + " : " + "\"Gross\"" + "\n" +
                                                  "}]";
            const string effective_date = "2021-06-30";
            const string ca_asset_class = "Buyout";
            const string ca_geo = "Africa";
            const string manager = "VGO Capital Partners";
            const string data_source = "Manual";
            var body = "{" + "\n" + "\"custom_risk_benchmarks\"" + " : " + custom_risk_benchmarks + ","
                           + "\n" + "\"effective_date\"" + " : " + "\"" + effective_date + "\","
                           + "\n" + "\"ca_asset_class\"" + " : " + "\"" + ca_asset_class + "\","
                           + "\n" + "\"ca_geo\"" + " : " + "\"" + ca_geo + "\","
                           + "\n" + "\"manager\"" + " : " + "\"" + manager + "\","
                           + "\n" + "\"data_source\"" + " : " + "\"" + data_source + "\"" + "\n" +
                       "}";
            #endregion

            #region Check if api of Sandbox or Staging then get data (on that site)
            if (workbenchApi.Contains("sandbox"))
            {
                fileName = "SinglePrivateReportManagerOutput.json";
            }
            if (workbenchApi.Contains("conceptia"))
            {
                fileName = "SinglePrivateReportManagerStagingOutput.json";
            }
            #endregion

            #region Run Tests
            // Get Single Private Report
            var response = WorkbenchApi.SinglePrivateReport(body, msalIdtoken);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs["statusCode"].ToString(), Is.EqualTo("200"));
            Assert.That(responseJs["headers"]["content-type"].ToString(), Is.EqualTo("application/json"));
            Assert.That(responseJs["body"]["base"].Count(), Is.EqualTo(2));
            //JObject privateReportJsBL = JObject.Parse(File.ReadAllText(filePath + fileName));
            //privateReportJsBL["headers"]["date"] = responseJs["headers"]["date"];
            //privateReportJsBL.SelectToken("headers.version")?.Parent.Remove();
            //responseJs.SelectToken("headers.version")?.Parent.Remove();
            //var sortReportJs = new JObject(responseJs.Properties().OrderBy(p => (string?)p.Name));
            //var sortReportJsBL = new JObject(privateReportJsBL.Properties().OrderBy(p => (string?)p.Name));
            //Assert.That(sortReportJsBL, Is.EqualTo(sortReportJs));
            #endregion
        }

        [Test, Category("API Smoke Tests")]
        public void ST002_PrivateReport_SourceCambridge()
        {
            #region Variables declare
            const string custom_risk_benchmarks = "[{"
                                                  + "\n" + "\"benchmark_id\"" + " : " + "\"" + "SPTR Index.USD" + "\","
                                                  + "\n" + "\"beta\"" + " : " + "1,"
                                                  + "\n" + "\"exposure\"" + " : " + "0.8,"
                                                  + "\n" + "\"return_type\"" + " : " + "\"Gross\"" + "\n" +
                                                  "}]";
            const string effective_date = "2021-06-30";
            const string ca_asset_class = "Venture";
            const string ca_geo = "United States";
            const string manager = "GSR Ventures";
            const string data_source = "Cambridge";
            var body = "{" + "\n" + "\"custom_risk_benchmarks\"" + " : " + custom_risk_benchmarks + ","
                           + "\n" + "\"effective_date\"" + " : " + "\"" + effective_date + "\","
                           + "\n" + "\"ca_asset_class\"" + " : " + "\"" + ca_asset_class + "\","
                           + "\n" + "\"ca_geo\"" + " : " + "\"" + ca_geo + "\","
                           + "\n" + "\"manager\"" + " : " + "\"" + manager + "\","
                           + "\n" + "\"data_source\"" + " : " + "\"" + data_source + "\"" + "\n" +
                       "}";
            #endregion

            #region Check if api of Sandbox or Staging then get data (on that site)
            if (workbenchApi.Contains("sandbox"))
            {
                fileName = "SinglePrivateReportCambridgeOutput.json";
            }
            if (workbenchApi.Contains("conceptia"))
            {
                fileName = "SinglePrivateReportCambridgeStagingOutput.json";
            }
            #endregion

            #region Run Tests
            // Get Single Private Report
            var response = WorkbenchApi.SinglePrivateReport(body, msalIdtoken);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs["statusCode"].ToString(), Is.EqualTo("200"));
            Assert.That(responseJs["headers"]["content-type"].ToString(), Is.EqualTo("application/json"));
            Assert.That(responseJs["body"]["base"].Count(), Is.EqualTo(1));
            //JObject privateReportJsBL = JObject.Parse(File.ReadAllText(filePath + fileName));
            //privateReportJsBL["headers"]["date"] = responseJs["headers"]["date"];
            //privateReportJsBL.SelectToken("headers.version")?.Parent.Remove();
            //responseJs.SelectToken("headers.version")?.Parent.Remove();
            //var sortReportJs = new JObject(responseJs.Properties().OrderBy(p => (string?)p.Name));
            //var sortReportJsBL = new JObject(privateReportJsBL.Properties().OrderBy(p => (string?)p.Name));
            //Assert.That(sortReportJsBL, Is.EqualTo(sortReportJs));
            #endregion
        }
        #endregion
    }
}
