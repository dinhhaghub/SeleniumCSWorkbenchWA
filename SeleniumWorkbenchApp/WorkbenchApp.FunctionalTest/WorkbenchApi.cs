using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Xml.Linq;
using System.Xml.XPath;
using WorkbenchApp.FunctionalTest.UIStepsDataManagement;

namespace WorkbenchApp.FunctionalTest
{
    internal class WorkbenchApi
    {
        // Initiate variables
        //internal static readonly XDocument xdoc = XDocument.Load(@"Config\Config.xml");

        // Api url (Environment)
        //internal static string workbenchApi = xdoc.XPathSelectElement("config/webApis").Attribute("WorkbenchApi").Value;
        internal static string workbenchApi = Config.workbenchApi;
        private static IRestResponse? response;

        #region Endowment Api
        // POST - Get Summary NAV
        internal static IRestResponse GetSummaryNAV(string body, string? msIdToken = null)
        {
            response = Api.PostObject("/fund/summary_nav", workbenchApi, body, msIdToken);
            return response;
        }

        // GET - Get portfolio_page
        internal static IRestResponse GetPortfolioPage(string? msIdToken=null)
        {
            response = Api.GetObject("/portfolio_page", workbenchApi, msIdToken);
            return response;
        }
        #endregion

        #region File Mapping Api
        // POST - Create File Mapping
        public static IRestResponse CreateFileMapping(string? fileMappingName = null, string? dataType = null, int? fundId = null, string? msIdToken = null)
        {
            var body = "{" + "\n" + "\"file_mapping_name\"" + " : " + "\"" + fileMappingName + "\","
                           + "\n" + "\"data_type\"" + " : " + "\"" + dataType + "\","
                           + "\n" + "\"fund_id\"" + " : " + "\"" + fundId + "\","
                           + "\n" + "\"fund_name\"" + " : " + "\"Citadel Multi Strategy Funds\","
                           + "\n" + "\"source\"" + " : " + "\"dha source\","
                           + "\n" + "\"field_mappings\"" + " : " + "[{"
                                  + "\n" + "\"field_name\"" + " : " + "\"fund aum\","
                                  + "\n" + "\"sql_field_name\"" + " : " + "\"aum\","
                                  + "\n" + "\"data_type\"" + " : " + "\"number\"" + "\n" + "}]"
                           + "\n" + "}";
            response = Api.PostObject("/file-mapping", workbenchApi, body, msIdToken);
            return response;
        }

        // GET - Get File Mapping By Id
        public static IRestResponse GetFileMappingById(string? fileMappingId = null, string? msIdToken = null)
        {
            response = Api.GetObject("/file-mapping/" + fileMappingId + "", workbenchApi, msIdToken);
            return response;
        }

        // PUT - Update File Mapping
        public static IRestResponse UpdateFileMappingById(string? fileMappingId = null, string? fileMappingName = null, string? dataType = null, int? fundId = null, string? msIdToken = null)
        {
            var body = "{" + "\n" + "\"file_mapping_name\"" + " : " + "\"" + fileMappingName + "\","
                           + "\n" + "\"data_type\"" + " : " + "\"" + dataType + "\","
                           + "\n" + "\"fund_id\"" + " : " + "\"" + fundId + "\","
                           + "\n" + "\"fund_name\"" + " : " + "\"Citadel Multi Strategy Funds\","
                           + "\n" + "\"source\"" + " : " + "\"dha source1\","
                           + "\n" + "\"field_mappings\"" + " : " + "[{"
                                  + "\n" + "\"field_name\"" + " : " + "\"fund returns\","
                                  + "\n" + "\"sql_field_name\"" + " : " + "\"aum\","
                                  + "\n" + "\"data_type\"" + " : " + "\"number\"" + "\n" + "}]"
                           + "\n" + "}";
            response = Api.PutObject("/file-mapping/" + fileMappingId + "", workbenchApi, body, msIdToken);
            return response;
        }

        // DELETE - Delete File Mapping By Id
        public static IRestResponse DeleteFileMappingById(string? fileMappingId = null, string? msIdToken = null)
        {
            response = Api.DeleteObject("/file-mapping/" + fileMappingId + "", workbenchApi, msIdToken);
            return response;
        }
        #endregion

        #region Share Class Api
        // GET - Get Share Class By Id
        public static IRestResponse GetShareClassByFundId(int? fundId = null, string? msIdToken = null)
        {
            response = Api.GetObject("/fund/share_class?fund_id=" + fundId + "", workbenchApi, msIdToken);
            return response;
        }
        #endregion

        #region Fund (Search/Get) Api
        // GET - Get Fund Status by Id
        public static IRestResponse GetFundStatusById(int? fundId = null, string? fundSource = null, string? msIdToken = null)
        {
            response = Api.GetObject("/fund_Status?fund_id=" + fundId + "&fund_source=" + fundSource + "", workbenchApi, msIdToken);
            return response;
        }

        // GET - Get Fund Data by Id
        public static IRestResponse GetFundDataById(int? fundId = null, string? fundSource = null, string? msIdToken = null)
        {
            response = Api.GetObject("/fund_Data?fund_id=" + fundId + "&fund_source=" + fundSource + "", workbenchApi, msIdToken);
            return response;
        }

        // GET - Get Fund Benchmark by Id
        public static IRestResponse GetFundBenchMarkById(int? fundId = null, string? fundSource = null, string? msIdToken = null)
        {
            response = Api.GetObject("/fund_benchmark?fund_id=" + fundId + "&fund_source=" + fundSource + "", workbenchApi, msIdToken);
            return response;
        }

        // GET - Get Fund Status (Private) by Id
        public static IRestResponse GetFundStatusPrivateById(string? managerId = null, string? fundSource = null, string? msIdToken = null)
        {
            response = Api.GetObject("/private-equity/manager-status?manager_id=" + managerId + "&manager_source=" + fundSource + "", workbenchApi, msIdToken);
            return response;
        }

        // GET - Get Fund Data (Private) by Id
        public static IRestResponse GetFundDataPrivateById(string? managerId = null, string? msIdToken = null)
        {
            response = Api.GetObject("/managers/" + managerId + "/cambridge/funds", workbenchApi, msIdToken);
            return response;
        }

        // POST - Search Fund
        public static IRestResponse SearchFund(string body, string? msIdToken = null)
        {
            response = Api.PostObject("/search/fund", workbenchApi, body, msIdToken);
            return response;
        }

        // POST - Search Fund
        public static IRestResponse SearchFaceset(string faceset, string? msIdToken = null)
        {
            var body = "{" + "\n" + "\"search_text\"" + " : " + "\"" + faceset + "\""
                           + "\n" + "}";
            response = Api.PostObject("/search/faceset", workbenchApi, body, msIdToken);
            return response;
        }
        #endregion

        #region Rating Api
        // POST - Add Rating
        public static IRestResponse AddRating(string body, string? msIdToken = null)
        {
            response = Api.PostObject("/rating", workbenchApi, body, msIdToken);
            return response;
        }

        // GET - Get Fund Rating
        public static IRestResponse GetFundRating(string ratingId, string type, string source, string? msIdToken = null)
        {
            response = Api.GetObject("rating/rating_ir?id=" + ratingId + "&type=" + type + "&source=" + source + "", workbenchApi, msIdToken);
            return response;
        }
        #endregion

        #region Single Public Report Api
        // POST - Single Public Report
        public static IRestResponse SinglePublicReport(string body, string? msIdToken = null)
        {
            response = Api.PostObject("/calc/single_manager_report", workbenchApi, body, msIdToken);
            return response;
        }
        #endregion

        #region DxD Report Api
        // POST - DxD Report
        public static IRestResponse DxDReport(string body, string? msIdToken = null)
        {
            response = Api.PostObject("/calc/deal_by_deal", workbenchApi, body, msIdToken);
            return response;
        }
        #endregion

        #region Single Private Report Api
        // POST - Single Private Report
        public static IRestResponse SinglePrivateReport(string body, string? msIdToken = null)
        {
            response = Api.PostObject("/calc/single_manager_report_private", workbenchApi, body, msIdToken);
            return response;
        }
        #endregion

        #region Add Fund Api
        // POST - Add Manager Api
        public static IRestResponse AddManagerApi(string body, string? msIdToken = null)
        {
            response = Api.PostObject("/managers", workbenchApi, body, msIdToken);
            return response;
        }

        // POST - Add Manager Api (Private Fund)
        public static IRestResponse AddManagerPrivateApi(string body, string? msIdToken = null)
        {
            response = Api.PostObject("/private-managers", workbenchApi, body, msIdToken);
            return response;
        }

        // POST - Add Fund Api
        public static IRestResponse AddFundApi(string body, string? msIdToken = null)
        {
            response = Api.PostObject("/funds", workbenchApi, body, msIdToken);
            return response;
        }

        // POST - Add Fund Api (Private Fund)
        public static IRestResponse AddFundPrivateApi(string body, string? msIdToken = null)
        {
            response = Api.PostObject("/private-funds", workbenchApi, body, msIdToken);
            return response;
        }
        #endregion

        #region Edit Fund Api
        // PUT - Edit Fund Api
        public static IRestResponse EditFundApi(string fundId, string body, string? msIdToken = null)
        {
            response = Api.PutObject("/funds?fund_id=" + fundId + "&source=manual", workbenchApi, body, msIdToken);
            return response;
        }

        // PUT - Edit Manager Api (Private Fund)
        public static IRestResponse EditManagerPrivateApi(string managerId, string body, string? msIdToken = null)
        {
            response = Api.PutObject("/private-managers/" + managerId + "", workbenchApi, body, msIdToken);
            return response;
        }

        // PUT - Edit Fund Api (Private Fund)
        public static IRestResponse EditFundPrivateApi(string fundId, string body, string? msIdToken = null)
        {
            response = Api.PutObject("/private-funds/" + fundId + "", workbenchApi, body, msIdToken);
            return response;
        }
        #endregion

        #region Helpful Methods
        // Sort the Properties By Name
        private static JObject Sort(JObject jObj)
        {
            var props = jObj.Properties().ToList();
            foreach (var prop in props)
            {
                prop.Remove();
            }

            foreach (var prop in props.OrderBy(p => p.Name))
            {
                jObj.Add(prop);
                if (prop.Value is JObject)
                    Sort((JObject)prop.Value);
            }
            return jObj;
        }

        // Sort the Properties By Name for a list of JObject
        public static List<JObject> SortPropertiesByName(List<JObject> jsObject)
        {
            foreach (var item in jsObject)
            {
                Sort(item);
            }
            return jsObject;
        }
        #endregion
    }
}
