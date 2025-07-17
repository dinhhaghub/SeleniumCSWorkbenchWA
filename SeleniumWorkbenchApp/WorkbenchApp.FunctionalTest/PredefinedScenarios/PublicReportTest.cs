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
    internal class PublicReportTest : BaseFunctionTest
    {
        #region Initiate variables
        internal static string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"PredefinedScenarios\Documents\");
        internal static string? fileName = null;
        #endregion

        #region TestMethod
        [Test, Category("API Smoke Tests")]
        public void ST001_PublicReport_Albourne()
        {
            #region Variables declare
            const string dataSourceValue = "Albourne";
            const string fund_source = "ALB";
            const string strategyValue = "Developed Equity";
            const string projected_teValue = "0.1";
            const string lastDateValue = "2023-01-31";
            const string customValue = "2018-09-01";
            const string custom_start_date_1Value = "2020-02-01";
            const string custom_start_date_2Value = "2016-10-01";
            const string custom_end_date_1 = "2023-01-31";
            const string custom_end_date_2 = "2021-09-30";
            const string ks_incepValue = "1999-03-01";
            const string indexNameValue = "[\"S&P 500 Industrials TR Index\",\"MSCI Japan Net TR Index (USD)\",\"FTSE 3 Month T-Bill Index\",\"CASH\"]";
            const string benchmarkIdsValue = "[\"SPTRINDU Index.USD\",\"NDDUJN Index.USD\",\"SBMMTB3 Index.USD\"]";
            const string proj_betaValue = "[1,1,1]";
            const string proj_exposureValue = "[0.8,-0.2,0.4]";
            const string mgt_feeValue = "0.01";
            const string mgt_fee_freqValue = "0";
            const string perf_feeValue = "0.2";
            const string hwm_statusValue = "1";
            const string catch_upValue = "0";
            const string catch_up_perc_softValue = "0.1";
            const string crystialized_paidValue = "1";
            const string hurdle_statusValue = "1";
            const string hurdle_fixedValue = "0";
            const string perf_returnValue = "0.1";
            const int hurdle_fixed_percentage = 0;
            const string hurdle_typeValue = "0";
            const string ramp_typeValue = "0";
            const string benchmarkValue = "SPTRHLTH Index.USD";
            const string fundIdValue = "38338"; // Abrams Capital Partners II
            const string rfrateIdValue = "SBMMTB3 Index.USD";
            const string lockupValue = "Soft";
            const int lockup_length_monthsValue = 6;
            const string liquidity_frequencyValue = "Semi-Annually";
            const double investor_gate_pctValue = 0.8;
            const double side_pocket_probabilityValue = 0.5;
            const double side_pocket_maxValue = 0.15;
            const string option_start_date_1Value = "Last 3 Years";
            const string option_start_date_2Value = "Last 5 Years";
            const string benchmark_hurdleIds = "[]";
            const string hurdle_exposure = "[]";
            const string cash_benchmarkValue = "SBMMTB3 Index.USD";
            var body = "{" + "\n" + "\"lastDate\"" + " : " + "\"" + lastDateValue + "\","
                           + "\n" + "\"data_source\"" + " : " + "\"" + dataSourceValue + "\","
                           + "\n" + "\"strategy\"" + " : " + "\"" + strategyValue + "\","
                           + "\n" + "\"projected_te\"" + " : " + "" + projected_teValue + ","
                           + "\n" + "\"custom_start_date_1\"" + " : " + "\"" + custom_start_date_1Value + "\","
                           + "\n" + "\"custom_start_date_2\"" + " : " + "\"" + custom_start_date_2Value + "\","
                           + "\n" + "\"custom_end_date_1\"" + " : " + "\"" + custom_end_date_1 + "\","
                           + "\n" + "\"custom_end_date_2\"" + " : " + "\"" + custom_end_date_2 + "\","
                           + "\n" + "\"ks_incep\"" + " : " + "\"" + ks_incepValue + "\","
                           + "\n" + "\"fund_source\"" + " : " + "\"" + fund_source + "\","
                           + "\n" + "\"custom\"" + " : " + "\"" + customValue + "\","
                           + "\n" + "\"benchmark\"" + " : " + "\"" + benchmarkValue + "\","
                           + "\n" + "\"mgt_fee\"" + " : " + "" + mgt_feeValue + ","
                           + "\n" + "\"mgt_fee_freq\"" + " : " + "" + mgt_fee_freqValue + ","
                           + "\n" + "\"perf_fee\"" + " : " + "" + perf_feeValue + ","
                           + "\n" + "\"hwm_status\"" + " : " + "" + hwm_statusValue + ","
                           + "\n" + "\"hurdle_status\"" + " : " + "" + hurdle_statusValue + ","
                           + "\n" + "\"ramp_type\"" + " : " + "" + ramp_typeValue + ","
                           + "\n" + "\"hurdle_fixed\"" + " : " + "" + hurdle_fixedValue + ","
                           + "\n" + "\"perf_return\"" + " : " + "" + perf_returnValue + ","
                           + "\n" + "\"hurdle_fixed_percentage\"" + ": " + hurdle_fixed_percentage + ","
                           + "\n" + "\"hurdle_type\"" + " : " + "" + hurdle_typeValue + ","
                           + "\n" + "\"catch_up\"" + " : " + "" + catch_upValue + ","
                           + "\n" + "\"catch_up_perc_soft\"" + " : " + "" + catch_up_perc_softValue + ","
                           + "\n" + "\"crystialized_paid\"" + " : " + "" + crystialized_paidValue + ","
                           + "\n" + "\"lockup\"" + " : " + "\"" + lockupValue + "\","
                           + "\n" + "\"lockup_length_months\"" + " : " + "" + lockup_length_monthsValue + ","
                           + "\n" + "\"liquidity_frequency\"" + " : " + "\"" + liquidity_frequencyValue + "\","
                           + "\n" + "\"investor_gate_pct\"" + " : " + "" + investor_gate_pctValue + ","
                           + "\n" + "\"side_pocket_probability\"" + " : " + "" + side_pocket_probabilityValue + ","
                           + "\n" + "\"side_pocket_max\"" + " : " + "" + side_pocket_maxValue + ","
                           + "\n" + "\"fundId\"" + " : " + "" + fundIdValue + ","
                           + "\n" + "\"option_start_date_1\"" + " : " + "\"" + option_start_date_1Value + "\","
                           + "\n" + "\"option_start_date_2\"" + " : " + "\"" + option_start_date_2Value + "\","
                           + "\n" + "\"benchmark_hurdleIds\"" + ": " + benchmark_hurdleIds + ","
                           + "\n" + "\"hurdle_exposure\"" + ": " + hurdle_exposure + ","
                           + "\n" + "\"benchmarkIds\"" + " : " + "" + benchmarkIdsValue + ","
                           + "\n" + "\"indexName\"" + " : " + "" + indexNameValue + ","
                           + "\n" + "\"rfrateId\"" + " : " + "\"" + rfrateIdValue + "\","
                           + "\n" + "\"proj_beta\"" + " : " + "" + proj_betaValue + ","
                           + "\n" + "\"proj_exposure\"" + " : " + "" + proj_exposureValue + ","
                           + "\n" + "\"cash_benchmark\"" + " : " + "\"" + cash_benchmarkValue + "\"" + "\n" +
                       "}";
            #endregion

            #region Check if api of Sandbox or Staging then get data (on that site)
            if (workbenchApi.Contains("sandbox"))
            {
                fileName = "SinglePublicReportOutput.json";
            }
            if (workbenchApi.Contains("conceptia"))
            {
                fileName = "SinglePublicReportStagingOutput.json";
            }
            #endregion

            #region Run Tests
            // Get Single Public Report
            var response = WorkbenchApi.SinglePublicReport(body, msalIdtoken);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs["Yearly_Return_Metrics"][0]["Year"].ToString(), Is.EqualTo("1999"));
            Assert.That(responseJs["Yearly_Return_Metrics"].Count(), Is.EqualTo(25));
            Assert.That(responseJs["Return_by_Duration"].Count(), Is.EqualTo(40));
            Assert.That(responseJs["Autoscore"].Count(), Is.EqualTo(3));
            Assert.That(responseJs["Annual_Return_Section"].Count(), Is.EqualTo(5));
            Assert.That(responseJs["Annual_Risk_Section"].Count(), Is.EqualTo(5));
            Assert.That(responseJs["Drawdown_Statistics"].Count(), Is.EqualTo(5));
            Assert.That(responseJs["Rolling_Correlation"].Count(), Is.EqualTo(4));
            Assert.That(responseJs["Annualized_Correlation"].Count(), Is.EqualTo(4));
            Assert.That(responseJs["Drawdown_Metrics"].Count(), Is.EqualTo(7));
            Assert.That(responseJs["Drawdown_Graph"].Count(), Is.EqualTo(574));
            Assert.That(responseJs["Correlation_Graph"].Count(), Is.EqualTo(1104));
            Assert.That(responseJs["Monthly_Return_Histograms"].Count(), Is.EqualTo(287));
            Assert.That(responseJs["Fund_Aums"].Count(), Is.EqualTo(48));
            Assert.That(responseJs["Raw_Data"].Count(), Is.EqualTo(287));
            Assert.That(responseJs["fund_info"].Count(), Is.EqualTo(8));
            //JObject reportJsBL = JObject.Parse(File.ReadAllText(filePath + fileName));
            //reportJsBL["fund_info"]["Fund AUM"] = responseJs["fund_info"]["Fund AUM"];
            //var sortReportJs = new JObject(responseJs.Properties().OrderBy(p => (string?)p.Name));
            //var sortReportJsBL = new JObject(reportJsBL.Properties().OrderBy(p => (string?)p.Name));
            //Assert.That(sortReportJsBL, Is.EqualTo(sortReportJs));
            #endregion
        }

        [Test, Category("API Smoke Tests")]
        public void ST002_PublicReport_Solovis()
        {
            #region Variables declare
            const string lastDate = "2022-12-31";
            const string data_source = "solovis";
            const string strategy = "Developed Equity";
            const string projected_te = "0.12";
            const string custom_start_date_1 = "2020-01-01";
            const string custom_start_date_2 = "2020-12-31";
            const string custom_end_date_1 = "2022-12-31";
            const string custom_end_date_2 = "2022-11-30";
            const string ks_incep = "2005-09-30";
            const string benchmark = "SPTR Index.USD";
            const string mgt_fee = "0.02";
            const string mgt_fee_freq = "0";
            const string perf_fee = "0.2";
            const string hwm_status = "1";
            const string hurdle_status = "0";
            const string ramp_type = "1";
            const string hurdle_fixed = "1";
            const string hurdle_type = "0";
            const string perf_return = "0.1";
            const int hurdle_fixed_percentage = 0;
            const string catch_up = "0";
            const string catch_up_perc_soft = "0.1";
            const string crystialized_paid = "1";
            const string fundId = "565";
            const string fund_source = "solovis";
            const string custom = "2019-10-01";
            const string benchmarkIds = "[\"SBMMTB3 Index.USD\",\"M1WDW$GI Index.USD\"]";
            const string indexName = "[\"FTSE 3 Month T-Bill Index\",\"MSCI ACWI IMI with USA Net TR Index (USD)\"]";
            const string rfrateId = "SBMMTB3 Index.USD";
            const string proj_beta = "[1,1]";
            const string proj_exposure = "[0.9,0.1]";
            const string lockupValue = "Hard";
            const int lockup_length_monthsValue = 6;
            const string liquidity_frequencyValue = "Quarterly";
            const double investor_gate_pctValue = 0.8;
            const double side_pocket_probabilityValue = 1;
            const double side_pocket_maxValue = 0.15;
            const string option_start_date_1Value = "Last 3 Years";
            const string option_start_date_2Value = "Last 5 Years";
            const string benchmark_hurdleIds = "[]";
            const string hurdle_exposure = "[]";
            const string cash_benchmarkValue = "SBMMTB3 Index.USD";
            var body = "{" + "\n" + "\"lastDate\"" + " : " + "\"" + lastDate + "\","
                           + "\n" + "\"data_source\"" + " : " + "\"" + data_source + "\","
                           + "\n" + "\"strategy\"" + " : " + "\"" + strategy + "\","
                           + "\n" + "\"projected_te\"" + " : " + "" + projected_te + ","
                           + "\n" + "\"custom_start_date_1\"" + " : " + "\"" + custom_start_date_1 + "\","
                           + "\n" + "\"custom_start_date_2\"" + " : " + "\"" + custom_start_date_2 + "\","
                           + "\n" + "\"custom_end_date_1\"" + " : " + "\"" + custom_end_date_1 + "\","
                           + "\n" + "\"custom_end_date_2\"" + " : " + "\"" + custom_end_date_2 + "\","
                           + "\n" + "\"ks_incep\"" + " : " + "\"" + ks_incep + "\","
                           + "\n" + "\"benchmark\"" + " : " + "\"" + benchmark + "\","
                           + "\n" + "\"mgt_fee\"" + " : " + "" + mgt_fee + ","
                           + "\n" + "\"mgt_fee_freq\"" + " : " + "" + mgt_fee_freq + ","
                           + "\n" + "\"perf_fee\"" + " : " + "" + perf_fee + ","
                           + "\n" + "\"hwm_status\"" + " : " + "" + hwm_status + ","
                           + "\n" + "\"hurdle_status\"" + " : " + "" + hurdle_status + ","
                           + "\n" + "\"ramp_type\"" + " : " + "" + ramp_type + ","
                           + "\n" + "\"hurdle_fixed\"" + " : " + "" + hurdle_fixed + ","
                           + "\n" + "\"hurdle_type\"" + " : " + "" + hurdle_type + ","
                           + "\n" + "\"perf_return\"" + " : " + "" + perf_return + ","
                           + "\n" + "\"hurdle_fixed_percentage\"" + ": " + hurdle_fixed_percentage + ","
                           + "\n" + "\"catch_up\"" + " : " + "" + catch_up + ","
                           + "\n" + "\"catch_up_perc_soft\"" + " : " + "" + catch_up_perc_soft + ","
                           + "\n" + "\"crystialized_paid\"" + " : " + "" + crystialized_paid + ","
                           + "\n" + "\"lockup\"" + " : " + "\"" + lockupValue + "\","
                           + "\n" + "\"lockup_length_months\"" + " : " + "" + lockup_length_monthsValue + ","
                           + "\n" + "\"liquidity_frequency\"" + " : " + "\"" + liquidity_frequencyValue + "\","
                           + "\n" + "\"investor_gate_pct\"" + " : " + "" + investor_gate_pctValue + ","
                           + "\n" + "\"side_pocket_probability\"" + " : " + "" + side_pocket_probabilityValue + ","
                           + "\n" + "\"side_pocket_max\"" + " : " + "" + side_pocket_maxValue + ","
                           + "\n" + "\"fundId\"" + " : " + "\"" + fundId + "\","
                           + "\n" + "\"option_start_date_1\"" + " : " + "\"" + option_start_date_1Value + "\","
                           + "\n" + "\"option_start_date_2\"" + " : " + "\"" + option_start_date_2Value + "\","
                           + "\n" + "\"benchmark_hurdleIds\"" + ": " + benchmark_hurdleIds + ","
                           + "\n" + "\"hurdle_exposure\"" + ": " + hurdle_exposure + ","
                           + "\n" + "\"fund_source\"" + " : " + "\"" + fund_source + "\","
                           + "\n" + "\"custom\"" + " : " + "\"" + custom + "\","
                           + "\n" + "\"benchmarkIds\"" + " : " + "" + benchmarkIds + ","
                           + "\n" + "\"indexName\"" + " : " + "" + indexName + ","
                           + "\n" + "\"rfrateId\"" + " : " + "\"" + rfrateId + "\","
                           + "\n" + "\"proj_beta\"" + " : " + "" + proj_beta + ","
                           + "\n" + "\"proj_exposure\"" + " : " + "" + proj_exposure + ","
                           + "\n" + "\"cash_benchmark\"" + " : " + "\"" + cash_benchmarkValue + "\"" + "\n" +
                       "}";
            #endregion

            #region Check if api of Sandbox or Staging then get data (on that site)
            if (workbenchApi.Contains("sandbox"))
            {
                fileName = "SinglePublicReportSolovisSandboxOutput.json";
            }
            if (workbenchApi.Contains("conceptia"))
            {
                fileName = "SinglePublicReportSolovisOutput.json";
            }
            #endregion

            #region Run Tests
            // Get Single Public Report
            var response = WorkbenchApi.SinglePublicReport(body, msalIdtoken);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs["Yearly_Return_Metrics"][0]["Year"].ToString(), Is.EqualTo("2005"));
            Assert.That(responseJs["Yearly_Return_Metrics"].Count(), Is.EqualTo(18));
            Assert.That(responseJs["Return_by_Duration"].Count(), Is.EqualTo(40));
            Assert.That(responseJs["Autoscore"].Count(), Is.EqualTo(3));
            Assert.That(responseJs["Annual_Return_Section"].Count(), Is.EqualTo(4));
            Assert.That(responseJs["Annual_Risk_Section"].Count(), Is.EqualTo(4));
            Assert.That(responseJs["Drawdown_Statistics"].Count(), Is.EqualTo(4));
            Assert.That(responseJs["Rolling_Correlation"].Count(), Is.EqualTo(3));
            Assert.That(responseJs["Annualized_Correlation"].Count(), Is.EqualTo(3));
            Assert.That(responseJs["Drawdown_Metrics"].Count(), Is.EqualTo(6));
            Assert.That(responseJs["Drawdown_Graph"].Count(), Is.EqualTo(416));
            Assert.That(responseJs["Correlation_Graph"].Count(), Is.EqualTo(591));
            Assert.That(responseJs["Monthly_Return_Histograms"].Count(), Is.EqualTo(208));
            Assert.That(responseJs["Fund_Aums"].Count(), Is.EqualTo(25));
            Assert.That(responseJs["Raw_Data"].Count(), Is.EqualTo(208));
            Assert.That(responseJs["fund_info"].Count(), Is.EqualTo(7));
            //JObject reportJsBL = JObject.Parse(File.ReadAllText(filePath + fileName));
            //reportJsBL["fund_info"]["Fund NAV"] = responseJs["fund_info"]["Fund NAV"];
            //var sortReportJs = new JObject(responseJs.Properties().OrderBy(p => (string?)p.Name));
            //var sortReportJsBL = new JObject(reportJsBL.Properties().OrderBy(p => (string?)p.Name));
            //Assert.That(sortReportJsBL, Is.EqualTo(sortReportJs));
            #endregion
        }

        [Test, Category("API Smoke Tests")]
        public void ST003_PublicReport_AlternativesEvestment()
        {
            #region Variables declare
            const string lastDate = "2020-08-01";
            const string data_source = "evestment";
            const string strategy = "Developed Equity";
            const string projected_te = "0.1";
            const string custom_start_date_1 = "2017-09-01";
            const string custom_start_date_2 = "2015-09-01";
            const string custom_end_date_1 = "2020-08-01";
            const string custom_end_date_2 = "2020-08-01";
            const string ks_incep = "2016-07-01";
            const string benchmark = "SPTR Index.USD";
            const string mgt_fee = "0.01";
            const string mgt_fee_freq = "1";
            const string perf_fee = "0.125";
            const string hwm_status = "1";
            const string hurdle_status = "1";
            const string ramp_type = "1";
            const string hurdle_fixed = "0";
            const string hurdle_type = "0";
            const string perf_return = "0.1";
            const int hurdle_fixed_percentage = 0;
            const string catch_up = "0";
            const string catch_up_perc_soft = "0.1";
            const string crystialized_paid = "1";
            const string fundId = "1445498";
            const string fund_source = "aevest";
            const string option_start_date_1 = "Last 3 Years";
            const string option_start_date_2 = "Last 5 Years";
            const string benchmark_hurdleIds = "[]";
            const string hurdle_exposure = "[]";
            const string benchmarkIds = "[\"SBMMTB3 Index.USD\"]";
            const string indexName = "[\"CASH\"]";
            const string rfrateId = "SBMMTB3 Index.USD";
            const string proj_beta = "[1]";
            const string proj_exposure = "[1]";
            const string lockupValue = "None";
            const int lockup_length_monthsValue = 6;
            const string liquidity_frequencyValue = "Daily";
            const double investor_gate_pctValue = 0.8;
            const double side_pocket_probabilityValue = 0.5;
            const double side_pocket_maxValue = 0.15;
            const string cash_benchmarkValue = "SBMMTB3 Index.USD";
            var body = "{" + "\n" + "\"lastDate\"" + " : " + "\"" + lastDate + "\","
                           + "\n" + "\"data_source\"" + " : " + "\"" + data_source + "\","
                           + "\n" + "\"strategy\"" + " : " + "\"" + strategy + "\","
                           + "\n" + "\"projected_te\"" + " : " + "" + projected_te + ","
                           + "\n" + "\"custom_start_date_1\"" + " : " + "\"" + custom_start_date_1 + "\","
                           + "\n" + "\"custom_start_date_2\"" + " : " + "\"" + custom_start_date_2 + "\","
                           + "\n" + "\"custom_end_date_1\"" + " : " + "\"" + custom_end_date_1 + "\","
                           + "\n" + "\"custom_end_date_2\"" + " : " + "\"" + custom_end_date_2 + "\","
                           + "\n" + "\"ks_incep\"" + " : " + "\"" + ks_incep + "\","
                           + "\n" + "\"benchmark\"" + " : " + "\"" + benchmark + "\","
                           + "\n" + "\"mgt_fee\"" + " : " + "" + mgt_fee + ","
                           + "\n" + "\"mgt_fee_freq\"" + " : " + "" + mgt_fee_freq + ","
                           + "\n" + "\"perf_fee\"" + " : " + "" + perf_fee + ","
                           + "\n" + "\"hwm_status\"" + " : " + "" + hwm_status + ","
                           + "\n" + "\"hurdle_status\"" + " : " + "" + hurdle_status + ","
                           + "\n" + "\"ramp_type\"" + " : " + "" + ramp_type + ","
                           + "\n" + "\"hurdle_fixed\"" + " : " + "" + hurdle_fixed + ","
                           + "\n" + "\"hurdle_type\"" + " : " + "" + hurdle_type + ","
                           + "\n" + "\"perf_return\"" + " : " + "" + perf_return + ","
                           + "\n" + "\"hurdle_fixed_percentage\"" + ": " + hurdle_fixed_percentage + ","
                           + "\n" + "\"catch_up\"" + " : " + "" + catch_up + ","
                           + "\n" + "\"catch_up_perc_soft\"" + " : " + "" + catch_up_perc_soft + ","
                           + "\n" + "\"crystialized_paid\"" + " : " + "" + crystialized_paid + ","
                           + "\n" + "\"lockup\"" + " : " + "\"" + lockupValue + "\","
                           + "\n" + "\"lockup_length_months\"" + " : " + "" + lockup_length_monthsValue + ","
                           + "\n" + "\"liquidity_frequency\"" + " : " + "\"" + liquidity_frequencyValue + "\","
                           + "\n" + "\"investor_gate_pct\"" + " : " + "" + investor_gate_pctValue + ","
                           + "\n" + "\"side_pocket_probability\"" + " : " + "" + side_pocket_probabilityValue + ","
                           + "\n" + "\"side_pocket_max\"" + " : " + "" + side_pocket_maxValue + ","
                           + "\n" + "\"fundId\"" + " : " + "" + fundId + ","
                           + "\n" + "\"fund_source\"" + " : " + "\"" + fund_source + "\","
                           + "\n" + "\"option_start_date_1\"" + " : " + "\"" + option_start_date_1 + "\","
                           + "\n" + "\"option_start_date_2\"" + " : " + "\"" + option_start_date_2 + "\","
                           + "\n" + "\"benchmark_hurdleIds\"" + ": " + benchmark_hurdleIds + ","
                           + "\n" + "\"hurdle_exposure\"" + ": " + hurdle_exposure + ","
                           + "\n" + "\"benchmarkIds\"" + " : " + "" + benchmarkIds + ","
                           + "\n" + "\"indexName\"" + " : " + "" + indexName + ","
                           + "\n" + "\"rfrateId\"" + " : " + "\"" + rfrateId + "\","
                           + "\n" + "\"proj_beta\"" + " : " + "" + proj_beta + ","
                           + "\n" + "\"proj_exposure\"" + " : " + "" + proj_exposure + ","
                           + "\n" + "\"cash_benchmark\"" + " : " + "\"" + cash_benchmarkValue + "\"" + "\n" +
                       "}";
            #endregion

            #region Check if api of Sandbox or Staging then get data (on that site)
            if (workbenchApi.Contains("sandbox"))
            {
                fileName = "SinglePublicReportAEvestmentSandboxOutput.json";
            }
            if (workbenchApi.Contains("conceptia"))
            {
                fileName = "SinglePublicReportAEvestmentOutput.json";
            }
            #endregion

            #region Run Tests
            // Get Single Public Report
            var response = WorkbenchApi.SinglePublicReport(body, msalIdtoken);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs["Yearly_Return_Metrics"][0]["Year"].ToString(), Is.EqualTo("2016"));
            #endregion
        }

        [Test, Category("API Smoke Tests")]
        public void ST004_PublicReport_TraditionalEvestment()
        {
            #region Variables declare
            const string lastDate = "2023-01-01";
            const string data_source = "evestment";
            const string strategy = "Developed Equity";
            const string projected_te = "0.1";
            const string custom_start_date_1 = "2020-02-01";
            const string custom_start_date_2 = "2018-02-01";
            const string custom_end_date_1 = "2023-01-01";
            const string custom_end_date_2 = "2023-01-01";
            const string ks_incep = "2006-10-30";
            const string benchmark = "SPTR Index.USD";
            const string mgt_fee = "0.02";
            const string mgt_fee_freq = "1";
            const string perf_fee = "0";
            const string hwm_status = "1";
            const string hurdle_status = "0";
            const string ramp_type = "0";
            const string hurdle_fixed = "1";
            const string hurdle_type = "0";
            const string perf_return = "0.1";
            const int hurdle_fixed_percentage = 0;
            const string catch_up = "0";
            const string catch_up_perc_soft = "0.1";
            const string crystialized_paid = "1";
            const string fundId = "660843";
            const string fund_source = "evest";
            const string option_start_date_1 = "Last 3 Years";
            const string option_start_date_2 = "Last 5 Years";
            const string benchmark_hurdleIds = "[]";
            const string hurdle_exposure = "[]";
            const string benchmarkIds = "[\"SBMMTB3 Index.USD\"]";
            const string indexName = "[\"CASH\"]";
            const string rfrateId = "SBMMTB3 Index.USD";
            const string proj_beta = "[1]";
            const string proj_exposure = "[1]";
            const string lockupValue = "Hard";
            const int lockup_length_monthsValue = 6;
            const string liquidity_frequencyValue = "Semi-Annually";
            const double investor_gate_pctValue = 0.8;
            const double side_pocket_probabilityValue = 0.5;
            const double side_pocket_maxValue = 0.15;
            const string cash_benchmarkValue = "SBMMTB3 Index.USD";
            var body = "{" + "\n" + "\"lastDate\"" + " : " + "\"" + lastDate + "\","
                           + "\n" + "\"data_source\"" + " : " + "\"" + data_source + "\","
                           + "\n" + "\"strategy\"" + " : " + "\"" + strategy + "\","
                           + "\n" + "\"projected_te\"" + " : " + "" + projected_te + ","
                           + "\n" + "\"custom_start_date_1\"" + " : " + "\"" + custom_start_date_1 + "\","
                           + "\n" + "\"custom_start_date_2\"" + " : " + "\"" + custom_start_date_2 + "\","
                           + "\n" + "\"custom_end_date_1\"" + " : " + "\"" + custom_end_date_1 + "\","
                           + "\n" + "\"custom_end_date_2\"" + " : " + "\"" + custom_end_date_2 + "\","
                           + "\n" + "\"ks_incep\"" + " : " + "\"" + ks_incep + "\","
                           + "\n" + "\"benchmark\"" + " : " + "\"" + benchmark + "\","
                           + "\n" + "\"mgt_fee\"" + " : " + "" + mgt_fee + ","
                           + "\n" + "\"mgt_fee_freq\"" + " : " + "" + mgt_fee_freq + ","
                           + "\n" + "\"perf_fee\"" + " : " + "" + perf_fee + ","
                           + "\n" + "\"hwm_status\"" + " : " + "" + hwm_status + ","
                           + "\n" + "\"hurdle_status\"" + " : " + "" + hurdle_status + ","
                           + "\n" + "\"ramp_type\"" + " : " + "" + ramp_type + ","
                           + "\n" + "\"hurdle_fixed\"" + " : " + "" + hurdle_fixed + ","
                           + "\n" + "\"hurdle_type\"" + " : " + "" + hurdle_type + ","
                           + "\n" + "\"perf_return\"" + " : " + "" + perf_return + ","
                           + "\n" + "\"hurdle_fixed_percentage\"" + ": " + hurdle_fixed_percentage + ","
                           + "\n" + "\"catch_up\"" + " : " + "" + catch_up + ","
                           + "\n" + "\"catch_up_perc_soft\"" + " : " + "" + catch_up_perc_soft + ","
                           + "\n" + "\"crystialized_paid\"" + " : " + "" + crystialized_paid + ","
                           + "\n" + "\"lockup\"" + " : " + "\"" + lockupValue + "\","
                           + "\n" + "\"lockup_length_months\"" + " : " + "" + lockup_length_monthsValue + ","
                           + "\n" + "\"liquidity_frequency\"" + " : " + "\"" + liquidity_frequencyValue + "\","
                           + "\n" + "\"investor_gate_pct\"" + " : " + "" + investor_gate_pctValue + ","
                           + "\n" + "\"side_pocket_probability\"" + " : " + "" + side_pocket_probabilityValue + ","
                           + "\n" + "\"side_pocket_max\"" + " : " + "" + side_pocket_maxValue + ","
                           + "\n" + "\"fundId\"" + " : " + "" + fundId + ","
                           + "\n" + "\"fund_source\"" + " : " + "\"" + fund_source + "\","
                           + "\n" + "\"option_start_date_1\"" + " : " + "\"" + option_start_date_1 + "\","
                           + "\n" + "\"option_start_date_2\"" + " : " + "\"" + option_start_date_2 + "\","
                           + "\n" + "\"benchmark_hurdleIds\"" + ": " + benchmark_hurdleIds + ","
                           + "\n" + "\"hurdle_exposure\"" + ": " + hurdle_exposure + ","
                           + "\n" + "\"benchmarkIds\"" + " : " + "" + benchmarkIds + ","
                           + "\n" + "\"indexName\"" + " : " + "" + indexName + ","
                           + "\n" + "\"rfrateId\"" + " : " + "\"" + rfrateId + "\","
                           + "\n" + "\"proj_beta\"" + " : " + "" + proj_beta + ","
                           + "\n" + "\"proj_exposure\"" + " : " + "" + proj_exposure + ","
                           + "\n" + "\"cash_benchmark\"" + " : " + "\"" + cash_benchmarkValue + "\"" + "\n" +
                       "}";
            #endregion

            #region Check if api of Sandbox or Staging then get data (on that site)
            if (workbenchApi.Contains("sandbox"))
            {
                fileName = "SinglePublicReportTEvestmentSandboxOutput.json";
            }
            if (workbenchApi.Contains("conceptia"))
            {
                fileName = "SinglePublicReportTEvestmentOutput.json";
            }
            #endregion

            #region Run Tests
            // Get Single Public Report
            var response = WorkbenchApi.SinglePublicReport(body, msalIdtoken);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs["Yearly_Return_Metrics"][0]["Year"].ToString(), Is.EqualTo("2006"));
            #endregion
        }
        #endregion
    }
}
