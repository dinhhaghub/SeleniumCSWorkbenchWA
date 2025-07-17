using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WorkbenchApp.FunctionalTest.PredefinedScenarios
{
    internal class GetFundTests : BaseFunctionTest
    {
        #region Initiate variables
        internal static string? managerId;
        internal static int fundId;
        internal static string? dataSource;
        #endregion

        #region TestMethod
        [Test, Category("API Smoke Tests")]
        public void ST001_GetSearchFund()
        {
            // Variables declare
            const string isOwnedByKS = "false";
            const string search_text = "Citadel Multi Strategy Funds";

            // Search Fund
            var body = "{" + "\n" + "\"isOwnedByKS\"" + " : " + isOwnedByKS + ","
                           + "\n" + "\"search_text\"" + " : " + "\"" + search_text + "\"" + "\n" +
                       "}";
            var searchFund = WorkbenchApi.SearchFund(body, msalIdtoken);
            Assert.That(searchFund.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to List JObject
            List<JObject>? searchFundJs = JsonConvert.DeserializeObject<List<JObject>>(searchFund.Content);
            Assert.That(searchFundJs, Has.Count.GreaterThanOrEqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(searchFundJs[0]["fund_id"].ToString, Is.EqualTo("14590"));
                Assert.That(searchFundJs[0]["fund_name"].ToString, Is.EqualTo(search_text));
                Assert.That(searchFundJs[0]["manager_id"].ToString, Is.EqualTo("357"));
                Assert.That(searchFundJs[0]["manager_name"].ToString, Is.EqualTo("Citadel Advisors LLC"));
                Assert.That(searchFundJs[0]["source"].ToString, Is.EqualTo("ALB"));
            });
        }

        [Test, Category("API Smoke Tests")]
        public void ST002_GetFundDataALBById()
        {
            // Variables declare
            fundId = 14590;
            dataSource = "ALB";

            // Get Fund Data by Id (source=ALB)
            var fundData = WorkbenchApi.GetFundDataById(fundId, dataSource, msalIdtoken);
            Assert.That(fundData.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject fundDataJs = JObject.Parse(fundData.Content);

            // Check if api of Sandbox or Staging then get data (on that site)
            int? count = null;
            if (workbenchApi.Contains("sandbox")) { count = 166; }
            if (workbenchApi.Contains("conceptia")) { count = 166; }
            Assert.That(fundDataJs.Count, Is.EqualTo(count));
            Assert.Multiple(() =>
            {
                Assert.That(fundDataJs["fund_id"].ToString, Is.EqualTo(fundId.ToString()));
                Assert.That(fundDataJs["data_source"].ToString, Is.EqualTo("ALBOURNE"));
                Assert.That(fundDataJs["source"].ToString, Is.EqualTo("ALB"));
                Assert.That(fundDataJs["data_catch_up"].ToString, Is.EqualTo("False"));
            });
        }

        [Test, Category("API Smoke Tests")]
        public void ST003_GetFundStatusALBById()
        {
            // Variables declare
            fundId = 14590;
            dataSource = "ALB";

            // Get Fund Status by Id (source=ALB)
            var fundStatus = WorkbenchApi.GetFundStatusById(fundId, dataSource, msalIdtoken);
            Assert.That(fundStatus.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject fundStatusJs = JObject.Parse(fundStatus.Content);
            string? first_aum_date = null;
            if (workbenchApi.Contains("sandbox")) { first_aum_date = "1998-01-31 00:00:00.000"; }
            if (workbenchApi.Contains("conceptia")) { first_aum_date = ""; }
            Assert.That(fundStatusJs.Count, Is.EqualTo(2)); // Update base on KS-517
            Assert.Multiple(() =>
            {
                Assert.That(fundStatusJs["manual"]["fund_id"].ToString, Is.EqualTo(fundId.ToString()));
                Assert.That(fundStatusJs[dataSource]["fund_id"].ToString, Is.EqualTo(fundId.ToString()));
                Assert.That(fundStatusJs["manual"]["first_aum_date"].ToString, Is.EqualTo(first_aum_date)); // --> no data on Staging
                Assert.That(fundStatusJs["manual"]["first_ror_date"].ToString, Is.EqualTo("1995-07-31 00:00:00.000"));
                Assert.That(fundStatusJs["manual"]["fund_source"].ToString, Is.EqualTo(dataSource));
                Assert.That(fundStatusJs["manual"]["data_source"].ToString, Is.EqualTo("manual"));
                Assert.That(fundStatusJs[dataSource]["data_source"].ToString, Is.EqualTo("ALBOURNE"));
            });
        }

        [Test, Category("API Smoke Tests")]
        public void ST004_GetFundDataPrivateById()
        {
            // Variables declare
            managerId = "VGO%20Capital%20Partners";

            // Get Fund Data by Id (source=ALB)
            var fundData = WorkbenchApi.GetFundDataPrivateById(managerId, msalIdtoken);
            Assert.That(fundData.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to List JObject
            List<JObject>? fundDataJs = JsonConvert.DeserializeObject<List<JObject>>(fundData.Content);
            Assert.That(fundDataJs.Count, Is.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(fundDataJs[0]["firm"].ToString, Is.EqualTo("VGO Capital Partners"));
                Assert.That(fundDataJs[1]["firm"].ToString, Is.EqualTo("VGO Capital Partners"));
                Assert.That(fundDataJs[0]["strategy"].ToString, Is.EqualTo("European Mezzanine"));
                Assert.That(fundDataJs[1]["strategy"].ToString, Is.EqualTo("European Mezzanine"));
                Assert.That(fundDataJs[0]["fund_name"].ToString, Is.EqualTo("VGO Special Situations Fund I"));
                Assert.That(fundDataJs[1]["fund_name"].ToString, Is.EqualTo("VGO Special Situations Fund II"));
                Assert.That(fundDataJs[0]["fund_size_expected_size_m"].ToString, Is.EqualTo(""));
                Assert.That(fundDataJs[1]["fund_size_expected_size_m"].ToString, Is.EqualTo("288"));
            });
        }

        [Test, Category("API Smoke Tests")]
        public void ST005_GetFundStatusPrivateById()
        {
            // Variables declare
            managerId = "VGO%20Capital%20Partners";
            dataSource = "cambridge"; // old: ALB
            string? fundInforTotalRecordsCount = null;

            // Get Fund Status by Id (source=ALB)
            var fundStatus = WorkbenchApi.GetFundStatusPrivateById(managerId, dataSource, msalIdtoken);
            Assert.That(fundStatus.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject fundStatusJs = JObject.Parse(fundStatus.Content);
            Assert.That(fundStatusJs.Count, Is.EqualTo(2));
            // Check if api of Sandbox or Staging then get data (on that site)
            if (workbenchApi.Contains("sandbox")) { fundInforTotalRecordsCount = "15";}
            if (workbenchApi.Contains("conceptia")) { fundInforTotalRecordsCount = "2";}
            Assert.Multiple(() =>
            {
                Assert.That(fundStatusJs["dealInfor"]["manager_id"].ToString, Is.EqualTo("VGO Capital Partners"));
                Assert.That(fundStatusJs["dealInfor"]["total_records"].ToString, Is.EqualTo("23"));
                Assert.That(fundStatusJs["fundInfor"]["manager_id"].ToString, Is.EqualTo("VGO Capital Partners"));
                Assert.That(fundStatusJs["fundInfor"]["total_records"].ToString, Is.EqualTo(fundInforTotalRecordsCount));
            });
        }

        [Test, Category("API Smoke Tests")]
        public void ST006_GetFundDataSolovisById()
        {
            // Variables declare
            fundId = 565;
            dataSource = "solovis";

            // Get Fund Data by Id (source=Solovis)
            var fundData = WorkbenchApi.GetFundDataById(fundId, dataSource, msalIdtoken);
            Assert.That(fundData.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject fundDataJs = JObject.Parse(fundData.Content);
            Assert.That(fundDataJs.Count, Is.EqualTo(47));
            Assert.Multiple(() =>
            {
                Assert.That(fundDataJs["fund_id"].ToString, Is.EqualTo(fundId.ToString()));
                Assert.That(fundDataJs["fund_name"].ToString, Is.EqualTo("Laurion Capital Ltd."));
                Assert.That(fundDataJs["asset_class_0"].ToString, Is.EqualTo("Absolute Return"));
                //Assert.That(fundDataJs["asset_class_1"].ToString, Is.EqualTo("Relative Value"));
                Assert.That(fundDataJs["asset_class_2"].ToString, Is.EqualTo("Relative Value"));
                Assert.That(fundDataJs["asset_class_3"].ToString, Is.EqualTo("Relative Value"));
                Assert.That(fundDataJs["manager_name"].ToString, Is.EqualTo("Laurion Capital Management LP"));
                Assert.That(fundDataJs["manager_sub_strategy"].ToString, Is.EqualTo(""));
                Assert.That(fundDataJs["data_fund_status"].ToString, Is.EqualTo(""));
                Assert.That(fundDataJs["ks_date"].ToString, Is.EqualTo("2020-12-29"));
                Assert.That(fundDataJs["data_inception_date"].ToString, Is.EqualTo("2005-09-30"));
                Assert.That(fundDataJs["ror_start_date"].ToString, Is.EqualTo("2005-09-30"));
                Assert.That(fundDataJs["tracking_error"].ToString, Is.EqualTo("0.12"));
                Assert.That(fundDataJs["data_fund_type"].ToString, Is.EqualTo("Accounting Fund"));
                Assert.That(fundDataJs["fund_owner"].ToString, Is.EqualTo("KS"));
                Assert.That(fundDataJs["strategy"].ToString, Is.EqualTo("Absolute Return"));
            });
        }

        [Test, Category("API Smoke Tests")]
        public void ST007_GetFundStatusSolovisById()
        {
            // Variables declare
            fundId = 565;
            dataSource = "solovis";

            // Get Fund Status by Id (source=Solovis)
            var fundStatus = WorkbenchApi.GetFundStatusById(fundId, dataSource, msalIdtoken);
            Assert.That(fundStatus.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject fundStatusJs = JObject.Parse(fundStatus.Content);
            int? count = null;
            if (workbenchApi.Contains("sandbox")) { count = 2; }
            if (workbenchApi.Contains("conceptia")) { count = 1; }
            Assert.That(fundStatusJs.Count, Is.EqualTo(count)); // Update base on KS-517
            Assert.That(fundStatusJs[dataSource]["fund_id"].ToString, Is.EqualTo(fundId.ToString()));
        }

        [Test, Category("API Smoke Tests")]
        public void ST008_GetFundDataAlternativesEvestmentById()
        {
            // Variables declare
            fundId = 632785;
            dataSource = "aevest";

            // Get Fund Data by Id (source=Evestment)
            var fundData = WorkbenchApi.GetFundDataById(fundId, dataSource, msalIdtoken);
            Assert.That(fundData.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject fundDataJs = JObject.Parse(fundData.Content);
            //Assert.That(fundDataJs.Count, Is.EqualTo(26));
            Assert.Multiple(() =>
            {
                Assert.That(fundDataJs["fund_id"].ToString, Is.EqualTo(fundId.ToString()));
                Assert.That(fundDataJs["fund_name"].ToString, Is.EqualTo("Advent Global Partners"));
                Assert.That(fundDataJs["manager_name"].ToString, Is.EqualTo("Advent Capital Management, LLC"));
                Assert.That(fundDataJs["data_inception_date"].ToString, Is.EqualTo("2001-08-01"));
                Assert.That(fundDataJs["source"].ToString, Is.EqualTo("aevest"));
                Assert.That(fundDataJs["data_source"].ToString, Is.EqualTo("evestment"));
            });
        }

        [Test, Category("API Smoke Tests")]
        public void ST009_GetFundStatusAlternativesEvestmentById()
        {
            // Variables declare
            fundId = 632785;
            dataSource = "aevest";

            // Get Fund Status by Id (source=Evestment)
            var fundStatus = WorkbenchApi.GetFundStatusById(fundId, dataSource, msalIdtoken);
            Assert.That(fundStatus.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject fundStatusJs = JObject.Parse(fundStatus.Content);
            Assert.That(fundStatusJs.Count, Is.EqualTo(1)); // Update base on KS-517
            Assert.That(fundStatusJs[dataSource]["productid"].ToString, Is.EqualTo(fundId.ToString()));
        }

        [Test, Category("API Smoke Tests")]
        public void ST010_GetFundDataTraditionalEvestmentById()
        {
            // Variables declare
            fundId = 660843; // John...2035
            dataSource = "evest";

            // Get Fund Data by Id (source=Evestment)
            var fundData = WorkbenchApi.GetFundDataById(fundId, dataSource, msalIdtoken);
            Assert.That(fundData.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject fundDataJs = JObject.Parse(fundData.Content);
            //Assert.That(fundDataJs.Count, Is.EqualTo(23));
            Assert.Multiple(() =>
            {
                Assert.That(fundDataJs["fund_id"].ToString, Is.EqualTo(fundId.ToString()));
                Assert.That(fundDataJs["fund_name"].ToString, Is.EqualTo("John Hancock Multimanager Lifetime Portfolios 2035"));
                Assert.That(fundDataJs["manager_name"].ToString, Is.EqualTo("John Hancock Investments"));
                Assert.That(fundDataJs["data_inception_date"].ToString, Is.EqualTo("2006-10-30"));
                Assert.That(fundDataJs["source"].ToString, Is.EqualTo("evest"));
                Assert.That(fundDataJs["data_source"].ToString, Is.EqualTo("evestment"));
            });
        }

        [Test, Category("API Smoke Tests")]
        public void ST011_GetFundStatusTraditionalEvestmentById()
        {
            // Variables declare
            fundId = 660843; // John...2035
            dataSource = "evest";

            // Get Fund Status by Id (source=Evestment)
            var fundStatus = WorkbenchApi.GetFundStatusById(fundId, dataSource, msalIdtoken);
            Assert.That(fundStatus.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject fundStatusJs = JObject.Parse(fundStatus.Content);
            Assert.That(fundStatusJs.Count, Is.EqualTo(1)); // Update base on KS-517
            Assert.That(fundStatusJs[dataSource]["productid"].ToString, Is.EqualTo(fundId.ToString()));
        }
        #endregion
    }
}
