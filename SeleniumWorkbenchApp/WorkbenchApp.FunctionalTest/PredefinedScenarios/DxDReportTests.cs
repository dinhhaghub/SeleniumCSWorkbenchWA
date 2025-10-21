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
    internal class DxDReportTests : BaseFunctionTest
    {
        #region Initiate variables
        internal static string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"PredefinedScenarios\Documents\");
        internal static string? fileName = null;
        #endregion

        #region TestMethod
        [Test, Category("API Smoke Tests")]
        public void ST001_DxDReport()
        {
            #region Variables declare
            const string source = "cambridge";
            const string currency = "USD";
            const string effective_date = "2021-06-30";
            const string main_weight = "Custom Weight";
            const string attribution_category_1 = "Equal Weight";
            const string attribution_category_2 = "% of Fund";
            const string attribution_category_3 = "% of Fund";
            const string custom_risk_benchmarks = "[{"
                                                  + "\n" + "\"benchmark_id\"" + " : " + "\"" + "SPTR Index.USD" + "\","
                                                  + "\n" + "\"beta\"" + " : " + "1,"
                                                  + "\n" + "\"exposure\"" + " : " + "0.8,"
                                                  + "\n" + "\"return_type\"" + " : " + "\"\"" + "\n" +
                                                  "}]";
            const string manager = "VGO Capital Partners";
            const string funds = "[" + "\n" +
                                    "{" + "\n" + "\"fund_name\"" + " : " + "\"" + "VGO Special Situations Fund I" + "\","
                                        + "\n" + "\"fund_size\"" + " : " + "0" + "\n"
                                  + "}," + "\n" +
                                    "{" + "\n" + "\"fund_name\"" + " : " + "\"" + "VGO Special Situations Fund II" + "\","
                                        + "\n" + "\"fund_size\"" + " : " + "288" + "\n"
                                  + "}" + "\n" +
                                 "]";
            var body = "{" + "\n" + "\"source\"" + " : " + "\"" + source + "\","
                           + "\n" + "\"currency\"" + " : " + "\"" + currency + "\","
                           + "\n" + "\"effective_date\"" + " : " + "\"" + effective_date + "\","
                           + "\n" + "\"main_weight\"" + " : " + "" + "\"" + main_weight + "\","
                           + "\n" + "\"attribution_category_1\"" + " : " + "\"" + attribution_category_1 + "\","
                           + "\n" + "\"attribution_category_2\"" + " : " + "\"" + attribution_category_2 + "\","
                           + "\n" + "\"attribution_category_3\"" + " : " + "\"" + attribution_category_3 + "\","
                           + "\n" + "\"custom_risk_benchmarks\"" + " : " + custom_risk_benchmarks + ","
                           + "\n" + "\"manager\"" + " : " + "\"" + manager + "\","
                           + "\n" + "\"funds\"" + " : " + funds + "\n" +
                       "}";
            #endregion

            #region Check if api of Sandbox or Staging then get data (on that site)
            if (workbenchApi.Contains("sandbox"))
            {
                fileName = "DxDReportOutput.json";
            }
            if (workbenchApi.Contains("conceptia"))
            {
                fileName = "DxDReportStagingOutput.json";
            }
            #endregion

            #region Run Tests
            // Get DxD Report
            var response = WorkbenchApi.DxDReport(body, msalIdtoken);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs["statusCode"].ToString(), Is.EqualTo("200"));
            Assert.That(responseJs["headers"]["content-type"].ToString(), Is.EqualTo("application/json"));
            //Assert.AreEqual("0.4.6", dxDReportJs["headers"]["version"].ToString());
            JObject dxDReportJsBL = JObject.Parse(File.ReadAllText(filePath + fileName));
            dxDReportJsBL["headers"]["date"] = responseJs["headers"]["date"];
            Assert.That(dxDReportJsBL["headers"]["date"], Is.EqualTo(responseJs["headers"]["date"]));
            Assert.That(dxDReportJsBL["body"]["pme"], Is.EqualTo(responseJs["body"]["pme"]));
            Assert.That(dxDReportJsBL["body"]["gross_total_alpha"], Is.EqualTo(responseJs["body"]["gross_total_alpha"]));

            // Kiểm tra field tồn tại
            Assert.That(responseJs["body"]["fund_table"], Is.Not.Null, "fund_table field is missing");
            Assert.That(responseJs["body"]["results_table"], Is.Not.Null, "results_table field is missing");
            Assert.That(responseJs["body"]["final_row"], Is.Not.Null, "final_row field is missing");
            // (tuỳ chọn) kiểm tra kiểu dữ liệu
            Assert.That(responseJs["body"]["fund_table"], Is.TypeOf<JArray>(), "fund_table is not an array");
            //var sortReportJs = new JObject(dxDReportJs.Properties().OrderBy(p => (string?)p.Name));
            //var sortReportJsBL = new JObject(dxDReportJsBL.Properties().OrderBy(p => (string?)p.Name));
            //Assert.AreEqual(sortReportJs, sortReportJsBL);
            #endregion
        }
        #endregion
    }
}
