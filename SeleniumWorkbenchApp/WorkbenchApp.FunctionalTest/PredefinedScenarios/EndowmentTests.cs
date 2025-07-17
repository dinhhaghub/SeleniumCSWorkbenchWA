using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace WorkbenchApp.FunctionalTest.PredefinedScenarios
{
    [TestFixture]
    internal class EndowmentTests : BaseFunctionTest
    {
        #region Initiate variables
        #endregion

        #region TestMethod
        [Test, Category("API Smoke Tests")]
        public void ST001_GetSummaryNAV()
        {
            // Variables declare
            int? count = null;
            int? countClassSummary = null;
            const string run_mode = "indirect";
            const string group_by = "asset_class_0";

            // Get summary nav
            string? body = "{" + "\n" + "\"run_mode\"" + " : " + "\"" + run_mode + "\"" + ","
                               + "\n" + "\"group_by\"" + " : " + "\"" + group_by + "\"" + "\n" +
                           "}";

            IRestResponse? getResponse = WorkbenchApi.GetSummaryNAV(body, msalIdtoken);
            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject getResponseJs = JObject.Parse(getResponse.Content);

            // Check if api of Sandbox or Staging then get data (on that site)
            if (workbenchApi.Contains("sandbox")) { count = 9; countClassSummary = 7; }
            if (workbenchApi.Contains("conceptia")) { count = 9; countClassSummary = 7; }

            Assert.That(getResponseJs.Count, Is.EqualTo(count));
            JObject? sortPropertyByName = new(getResponseJs.Properties().OrderBy(p => (string?)p.Name));
            Assert.That(sortPropertyByName["class_summary"].Count, Is.EqualTo(countClassSummary));
            Assert.That(sortPropertyByName["class_summary"][0]["asset_class"].ToString, Is.EqualTo("Absolute Return"));
            //Assert.That(sortPropertyByName["class_summary"][1]["asset_class"].ToString, Is.EqualTo("Cash"));
        }

        [Test, Category("API Smoke Tests")]
        public void ST002_GetPortfolioPage()
        {
            // Variables declare
            int? count = null;
            string? secondDataAssetTable = null;
            string? financialAssets = null;

            // Get portfolio page
            IRestResponse? getResponse = WorkbenchApi.GetPortfolioPage(msalIdtoken);
            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject getResponseJs = JObject.Parse(getResponse.Content);

            if (workbenchApi.Contains("sandbox")) { count = 3; }
            if (workbenchApi.Contains("conceptia")) { count = 3; }
            Assert.That(getResponseJs.Count, Is.EqualTo(count));

            // Sort property (by name) to verify the return value
            JObject? sortPropertyByName = new(getResponseJs.Properties().OrderBy(p => (string?)p.Name));

            // Check if api of Sandbox or Staging then get data (on that site)
            if (workbenchApi.Contains("sandbox")) { count = 472; secondDataAssetTable = "Real Assets"; financialAssets = "Financial Assets"; }
            if (workbenchApi.Contains("conceptia")) { count = 472; secondDataAssetTable = "Real Assets"; financialAssets = "Financial Assets"; }
            Assert.That(sortPropertyByName["main_table"].Count, Is.GreaterThanOrEqualTo(count));
            Assert.That(sortPropertyByName["pie_chart"].Count, Is.EqualTo(9));
            
            // Sort main_table in Js to verify the return value
            List<JToken>? mainTableSort = sortPropertyByName["main_table"].OrderBy(o => o.SelectToken("order_row")).ToList();
            Assert.That(mainTableSort[0]["asset"].ToString, Is.EqualTo(financialAssets));
            Assert.That(mainTableSort[1]["asset"].ToString, Is.EqualTo(secondDataAssetTable));

            // Sort pie_chart in Js to verify the return value
            List<JToken>? sortPieChartsort = sortPropertyByName["pie_chart"].OrderBy(o => o.SelectToken("asset")).ToList();
            Assert.That(sortPieChartsort[0]["asset"].ToString, Is.EqualTo("Absolute Return"));
        }
        #endregion
    }
}
