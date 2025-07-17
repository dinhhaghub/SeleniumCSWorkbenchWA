using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WorkbenchApp.FunctionalTest.PredefinedScenarios
{
    internal class AddEditFundTests : BaseFunctionTest
    {
        #region Initiate variables
        #endregion

        #region TestMethod
        [Test, Category("API Smoke Tests")]
        public void ST001_AddPublicFund()
        {
            // Variables declare
            string manager_name = "QA_ApiAuto_Manager" + @"_" + DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_");
            var body = "{" + "\n" + "\"name\"" + " : " + "\"" + manager_name + "\"" + "\n" +
                       "}";

            // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
            if (workbenchApi.Contains("sandbox"))
            {
                #region Add Public Fund - Manager
                // Add Public Fund Manager
                var response = WorkbenchApi.AddManagerApi(body, msalIdtoken);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

                // Parse IRestResponse to JObject
                JObject? responseJs = JObject.Parse(response.Content);
                Assert.Multiple(() =>
                {
                    Assert.That(responseJs["name"].ToString(), Is.EqualTo(manager_name));
                    Assert.That(responseJs["source"].ToString(), Is.EqualTo("manual"));
                });
                #endregion

                #region Add Public Fund - Fund
                // Add Public Fund (Child)
                string fund_name = "QA_ApiAuto_Fund" + @"_" + DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_");
                string sub_asset_class = "US Growth Equity";
                string asset_class = "Private Equity";
                string date = DateTime.Now.Date.ToString("yyyy-MM-dd"), inception_date = date.Replace("/", "-");
                string latest_actual_value = "QA api auto Latest Actual Value";
                string business_street = "QA api auto Business Street";
                string business_city = "QA api auto Business City";
                string business_state = "QA api auto Business State";
                string business_zip = "QA api auto Business ZIP";
                string business_country = "JP";
                string data_redemption_frequency = "Monthly";
                string data_redemption_notice_days = "59 Days";
                string data_hard_lockup = "Yes";
                string data_hard_lockup_months = "35.01";
                string data_soft_lockup_months = "14.01";
                string data_redemption_fee = "16.01";
                string data_redemption_gate = "10.01";
                string data_redemption_gate_percent = "8.51";
                string data_percent_of_nav_available = "26.51";
                string data_side_pocket = "Yes";
                string data_redemption_note = "QA Api auto test notes Liq 111";
                string data_management_fee = "-10.51";
                string data_management_fee_frequency = "Monthly";
                string data_performance_fee = "-6.51";
                string data_hwm_status = "Yes";
                string data_catch_up = "Yes";
                string data_catch_up_rate = "-17.51";
                string data_crystalization_frequency = "2";
                string data_hurdle_status = "Yes";
                string data_hurdle_type = "Fixed";
                string data_benchmark = "MS133333.USD";
                string data_benchmark_name = "MSCI CHINA A ONSHORE Net Return Index USD";
                string data_hurdle_rate = "-5.51";
                string data_hurdle_hard_soft = "Hard";
                string data_hurdle_ramp_type = "Performance Dependent";
                int manager_id = int.Parse(responseJs["id"].ToString());
                string manager_source = "manual";
                body = "{"
                        + "\n" + "\"manager_name\"" + " : " + "\"" + manager_name + "\","
                        + "\n" + "\"fund_name\"" + " : " + "\"" + fund_name + "\","
                        + "\n" + "\"sub_asset_class\"" + " : " + "\"" + sub_asset_class + "\","
                        + "\n" + "\"asset_class\"" + " : " + "\"" + asset_class + "\","
                        + "\n" + "\"inception_date\"" + " : " + "\"" + inception_date + "\","
                        + "\n" + "\"latest_actual_value\"" + " : " + "\"" + latest_actual_value + "\","
                        + "\n" + "\"business_street\"" + " : " + "\"" + business_street + "\","
                        + "\n" + "\"business_city\"" + " : " + "\"" + business_city + "\","
                        + "\n" + "\"business_state\"" + " : " + "\"" + business_state + "\","
                        + "\n" + "\"business_zip\"" + " : " + "\"" + business_zip + "\","
                        + "\n" + "\"business_country\"" + " : " + "\"" + business_country + "\","
                        + "\n" + "\"data_redemption_frequency\"" + " : " + "\"" + data_redemption_frequency + "\","
                        + "\n" + "\"data_redemption_notice_days\"" + " : " + "\"" + data_redemption_notice_days + "\","
                        + "\n" + "\"data_hard_lockup\"" + " : " + "\"" + data_hard_lockup + "\","
                        + "\n" + "\"data_hard_lockup_months\"" + " : " + "\"" + data_hard_lockup_months + "\","
                        + "\n" + "\"data_soft_lockup_months\"" + " : " + "\"" + data_soft_lockup_months + "\","
                        + "\n" + "\"data_redemption_fee\"" + " : " + "\"" + data_redemption_fee + "\","
                        + "\n" + "\"data_redemption_gate\"" + " : " + "\"" + data_redemption_gate + "\","
                        + "\n" + "\"data_redemption_gate_percent\"" + " : " + "\"" + data_redemption_gate_percent + "\","
                        + "\n" + "\"data_percent_of_nav_available\"" + " : " + "\"" + data_percent_of_nav_available + "\","
                        + "\n" + "\"data_side_pocket\"" + " : " + "\"" + data_side_pocket + "\","
                        + "\n" + "\"data_redemption_note\"" + " : " + "\"" + data_redemption_note + "\","
                        + "\n" + "\"data_management_fee\"" + " : " + "\"" + data_management_fee + "\","
                        + "\n" + "\"data_management_fee_frequency\"" + " : " + "\"" + data_management_fee_frequency + "\","
                        + "\n" + "\"data_performance_fee\"" + " : " + "\"" + data_performance_fee + "\","
                        + "\n" + "\"data_hwm_status\"" + " : " + "\"" + data_hwm_status + "\","
                        + "\n" + "\"data_catch_up\"" + " : " + "\"" + data_catch_up + "\","
                        + "\n" + "\"data_catch_up_rate\"" + " : " + "\"" + data_catch_up_rate + "\","
                        + "\n" + "\"data_crystalization_frequency\"" + " : " + "\"" + data_crystalization_frequency + "\","
                        + "\n" + "\"data_hurdle_status\"" + " : " + "\"" + data_hurdle_status + "\","
                        + "\n" + "\"data_hurdle_type\"" + " : " + "\"" + data_hurdle_type + "\","
                        + "\n" + "\"data_benchmark\"" + " : " + "\"" + data_benchmark + "\","
                        + "\n" + "\"data_benchmark_name\"" + " : " + "\"" + data_benchmark_name + "\","
                        + "\n" + "\"data_hurdle_rate\"" + " : " + "\"" + data_hurdle_rate + "\","
                        + "\n" + "\"data_hurdle_hard_soft\"" + " : " + "\"" + data_hurdle_hard_soft + "\","
                        + "\n" + "\"data_hurdle_ramp_type\"" + " : " + "\"" + data_hurdle_ramp_type + "\","
                        + "\n" + "\"manager_id\"" + " : " + manager_id + ","
                        + "\n" + "\"manager_source\"" + " : " + "\"" + manager_source + "\"" + "\n" +
                       "}";
                var addPublicFund = WorkbenchApi.AddFundApi(body, msalIdtoken);
                Assert.That(addPublicFund.StatusCode, Is.EqualTo(HttpStatusCode.Created));

                // Parse IRestResponse to JObject
                JObject addPublicFundJs = JObject.Parse(addPublicFund.Content);
                Assert.Multiple(() =>
                {
                    Assert.That(addPublicFundJs["fund_name"].ToString(), Is.EqualTo(fund_name));
                    Assert.That(addPublicFundJs["manager_id"].ToString(), Is.EqualTo(manager_id.ToString()));
                    Assert.That(addPublicFundJs["manager_name"].ToString(), Is.EqualTo(manager_name));
                    Assert.That(addPublicFundJs["source"].ToString(), Is.EqualTo("manual"));
                    Assert.That(addPublicFundJs["sub_asset_class"].ToString(), Is.EqualTo(sub_asset_class));
                    Assert.That(addPublicFundJs["asset_class"].ToString(), Is.EqualTo(asset_class));
                    //Assert.AreEqual(inception_date, addPublicFundJs["inception_date"].ToString());
                    Assert.That(addPublicFundJs["latest_actual_value"].ToString(), Is.EqualTo(latest_actual_value));
                    Assert.That(addPublicFundJs["business_street"].ToString(), Is.EqualTo(business_street));
                    Assert.That(addPublicFundJs["business_city"].ToString(), Is.EqualTo(business_city));
                    Assert.That(addPublicFundJs["business_state"].ToString(), Is.EqualTo(business_state));
                    Assert.That(addPublicFundJs["business_zip"].ToString(), Is.EqualTo(business_zip));
                    Assert.That(addPublicFundJs["business_country"].ToString(), Is.EqualTo(business_country));
                });
                #endregion
            }
            else Console.WriteLine("Add Public Fund Api auto test is only add new Fund on Sandbox Site!!!");
        }

        [Test, Category("API Smoke Tests")]
        public void ST002_AddPrivateFund()
        {
            // Variables declare
            string firm = "QA_ApiAuto_FirmPriv" + @"_" + DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_");
            string manager_name = "QA_ApiAuto_ManagerPriv" + @"_" + DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_");
            string sub_asset_class = "FAD Real Estate";
            string date = DateTime.Now.Date.ToString("MM-dd-yyyy"), inception_date = date.Replace("/", "-");
            string latest_actual_value = "QA_ApiAuto_LAV";
            string business_street = "QA_ApiAuto_BusinessStreet";
            string business_city = "QA_ApiAuto_BusinessCity";
            string business_state = "QA_ApiAuto_BusinessState";
            string business_zip = "QA_ApiAuto_BusinessZIP";
            string business_country = "QA_ApiAuto_BusinessCountry";
            string business_contact = "QA_ApiAuto_BusinessContact";
            string business_email = "QA_ApiAuto_BusinessEmail";
            string business_phone = "123456789";
            var body = "{"
                        + "\n" + "\"firm\"" + " : " + "\"" + firm + "\","
                        + "\n" + "\"manager_name\"" + " : " + "\"" + manager_name + "\","
                        + "\n" + "\"sub_asset_class\"" + " : " + "\"" + sub_asset_class + "\","
                        + "\n" + "\"inception_date\"" + " : " + "\"" + inception_date + "\","
                        + "\n" + "\"latest_actual_value\"" + " : " + "\"" + latest_actual_value + "\","
                        + "\n" + "\"business_street\"" + " : " + "\"" + business_street + "\","
                        + "\n" + "\"business_city\"" + " : " + "\"" + business_city + "\","
                        + "\n" + "\"business_state\"" + " : " + "\"" + business_state + "\","
                        + "\n" + "\"business_zip\"" + " : " + "\"" + business_zip + "\","
                        + "\n" + "\"business_country\"" + " : " + "\"" + business_country + "\","
                        + "\n" + "\"business_contact\"" + " : " + "\"" + business_contact + "\","
                        + "\n" + "\"business_email\"" + " : " + "\"" + business_email + "\","
                        + "\n" + "\"business_phone\"" + " : " + "\"" + business_phone + "\"" + "\n" +
                       "}";

            // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
            if (workbenchApi.Contains("sandbox"))
            {
                #region Add Private Fund - Manager
                // Add Public Fund Manager
                var response = WorkbenchApi.AddManagerPrivateApi(body, msalIdtoken);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

                // Parse IRestResponse to JObject
                JObject? responseJs = JObject.Parse(response.Content);
                Assert.Multiple(() =>
                {
                    Assert.That(responseJs["manager_name"].ToString(), Is.EqualTo(manager_name));
                    Assert.That(responseJs["firm"].ToString(), Is.EqualTo(firm));
                    Assert.That(responseJs["source"].ToString(), Is.EqualTo("private_manual"));
                    Assert.That(responseJs["sub_asset_class"].ToString(), Is.EqualTo(sub_asset_class));
                    Assert.That(responseJs["latest_actual_value"].ToString(), Is.EqualTo(latest_actual_value));
                    Assert.That(responseJs["business_street"].ToString(), Is.EqualTo(business_street));
                    Assert.That(responseJs["business_city"].ToString(), Is.EqualTo(business_city));
                    Assert.That(responseJs["business_state"].ToString(), Is.EqualTo(business_state));
                    Assert.That(responseJs["business_zip"].ToString(), Is.EqualTo(business_zip));
                    Assert.That(responseJs["business_country"].ToString(), Is.EqualTo(business_country));
                    Assert.That(responseJs["business_contact"].ToString(), Is.EqualTo(business_contact));
                    Assert.That(responseJs["business_email"].ToString(), Is.EqualTo(business_email));
                    Assert.That(responseJs["business_phone"].ToString(), Is.EqualTo(business_phone));
                });
                #endregion

                #region Add Private Fund - Fund
                // Add Private Fund (Fund)
                string fund_name = "Fund 1 of QA_ApiAuto_FirmPriv";
                string strategy = "QA_ApiAuto_Strategy";
                string year_firm_founded = "QA_ApiAuto_YFM";
                string strategy_headquarters = "QA_ApiAuto_SH";
                string asset_class = "QA_ApiAuto_AC";
                string investment_stage = "QA_ApiAuto_IS";
                string industry_focus = "QA_ApiAuto_IF";
                string geographic_focus = "QA_ApiAuto_GF";
                string fund_size_expected_size_m = "188";
                string fund_size_expected_size_m_currency = "USD";
                int manager_id = int.Parse(responseJs["manager_id"].ToString());
                body = "{"
                        + "\n" + "\"fund_name\"" + " : " + "\"" + fund_name + "\","
                        + "\n" + "\"strategy\"" + " : " + "\"" + strategy + "\","
                        + "\n" + "\"year_firm_founded\"" + " : " + "\"" + year_firm_founded + "\","
                        + "\n" + "\"strategy_headquarters\"" + " : " + "\"" + strategy_headquarters + "\","
                        + "\n" + "\"asset_class\"" + " : " + "\"" + asset_class + "\","
                        + "\n" + "\"investment_stage\"" + " : " + "\"" + investment_stage + "\","
                        + "\n" + "\"industry_focus\"" + " : " + "\"" + industry_focus + "\","
                        + "\n" + "\"geographic_focus\"" + " : " + "\"" + geographic_focus + "\","
                        + "\n" + "\"fund_size_expected_size_m\"" + " : " + "\"" + fund_size_expected_size_m + "\","
                        + "\n" + "\"fund_size_expected_size_m_currency\"" + " : " + "\"" + fund_size_expected_size_m_currency + "\","
                        + "\n" + "\"manager_id\"" + " : " + manager_id + "\n" +
                       "}";

                response = WorkbenchApi.AddFundPrivateApi(body, msalIdtoken);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

                // Parse IRestResponse to JObject
                responseJs = JObject.Parse(response.Content);
                Assert.Multiple(() =>
                {
                    Assert.That(responseJs["fund"].ToString(), Is.EqualTo(""));
                    Assert.That(responseJs["manager_id"].ToString(), Is.EqualTo(manager_id.ToString()));
                    Assert.That(responseJs["source"].ToString(), Is.EqualTo("private_manual"));
                    Assert.That(responseJs["fund_name"].ToString(), Is.EqualTo(fund_name));
                    Assert.That(responseJs["strategy"].ToString(), Is.EqualTo(strategy));
                    Assert.That(responseJs["year_firm_founded"].ToString(), Is.EqualTo(year_firm_founded));
                    Assert.That(responseJs["strategy_headquarters"].ToString(), Is.EqualTo(strategy_headquarters));
                    Assert.That(responseJs["asset_class"].ToString(), Is.EqualTo(asset_class));
                    Assert.That(responseJs["investment_stage"].ToString(), Is.EqualTo(investment_stage));
                    Assert.That(responseJs["industry_focus"].ToString(), Is.EqualTo(industry_focus));
                    Assert.That(responseJs["geographic_focus"].ToString(), Is.EqualTo(geographic_focus));
                    Assert.That(responseJs["fund_size_expected_size_m"].ToString(), Is.EqualTo(fund_size_expected_size_m));
                    Assert.That(responseJs["fund_size_expected_size_m_currency"].ToString(), Is.EqualTo(fund_size_expected_size_m_currency));
                });
                #endregion
            }
            else Console.WriteLine("Add Private Fund Api auto test is only add new Fund on Sandbox Site!!!");
        }
        #endregion
    }
}
