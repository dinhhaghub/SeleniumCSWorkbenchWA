//using Microsoft.Office.Interop.Excel;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;
using System.Xml.Linq;
using System.Xml.XPath;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
//using static MongoDB.Driver.WriteConcern;
//using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;
using System;
//using SharpCompress.Common;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using WorkbenchApp.UITest.Pages;
using System.Reflection;
using System.Xml;

namespace SeleniumGendKS.Core.DBConnection
{
    internal static class DatabaseConnection
    {
        // load config.xml file
        internal static readonly string xmlFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Config\" + @"Config.xml");
        internal static XmlDocument xmlDocLoad(string xmlFilePath)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlFilePath);
                return doc;
            }
            catch (Exception e) { return null; }
        }
        internal static string? projectName = null, siteName = null, url = null, instanceName = null, datalakeApi = null, workbenchApi = null;
        internal static XmlDocument configurationFile()
        {
            XmlDocument xdoc = xmlDocLoad(xmlFilePath);
            // Parse an XML file (Get Project & Site name to run)
            foreach (XmlNode node in xdoc.DocumentElement.ChildNodes)
            {
                if (node.Name == "project_name")
                {
                    projectName = node.InnerText;
                    if (projectName == null || projectName == "" || (projectName != "webapp"))
                    {
                        Console.WriteLine("Project '" + projectName + "' does not exist!");
                        break;
                    }
                }
                if (node.Name == "site_name")
                {
                    siteName = node.InnerText;
                }
            }

            // Determine Project & Site name to run
            foreach (XmlNode node in xdoc.DocumentElement.ChildNodes)
            {
                if (node.Name == projectName)
                {
                    foreach (XmlNode siteNode in node.ChildNodes)
                    {
                        if (siteNode.Name == siteName)
                        {
                            url = siteNode["url"].InnerText;
                            instanceName = siteNode["instanceName"].InnerText;
                            datalakeApi = siteNode["webApis"].Attributes["datalakeApi"].Value;
                            workbenchApi = siteNode["webApis"].Attributes["WorkbenchApi"].Value;
                        }
                    }
                }
            }
            return xdoc;
        }

        // Initiate variables
        internal static readonly XDocument xdoc = XDocument.Load(@"Config\Config.xml");
        internal static string bearerToken = xdoc.XPathSelectElement("config/tokens").Attribute("bearerToken").Value;

        #region JSON Methods - Convert/Change/Update/Remove/Sort/Export 
        // Export Result to Json File
        public static void SaveResultToJsonFile(string filePath, IRestResponse sendResponse)
        {
            List<object> jsonData = JsonConvert.DeserializeObject<List<object>>(sendResponse.Content);
            var jsonExport = JsonConvert.SerializeObject(jsonData);
            System.IO.File.WriteAllText(filePath, jsonExport);
        }

        // Converting a csv file to json
        public static string ConvertCsvFileToJsonObject(string path)
        {
            var csv = new List<string[]>();
            var lines = File.ReadAllLines(path);

            foreach (string line in lines)
                csv.Add(line.Split(','));

            var properties = lines[0].Split(',');

            var listObjResult = new List<Dictionary<string, string>>();

            for (int i = 1; i < lines.Length; i++)
            {
                var objResult = new Dictionary<string, string>();
                for (int j = 0; j < properties.Length; j++)
                    objResult.Add(properties[j], csv[i][j]);

                listObjResult.Add(objResult);
            }

            return JsonConvert.SerializeObject(listObjResult);
        }

        // Converting a csv file to json (when CSV contains quoted strings containing commas)
        public static string ConvertCsvFileToJsonObjectContainsQuoted(string path)
        {
            var csv = new List<string[]>();
            var lines = File.ReadAllLines(path);

            foreach (string line in lines)
                csv.Add(Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"));


            var properties = lines[0].Split(',');

            var listObjResult = new List<Dictionary<string, string>>();

            for (int i = 1; i < lines.Length; i++)
            {
                var objResult = new Dictionary<string, string>();
                for (int j = 0; j < properties.Length; j++)
                    objResult.Add(properties[j], csv[i][j]);

                listObjResult.Add(objResult);
            }

            return JsonConvert.SerializeObject(listObjResult);
        }

        // Add field name(KEY/PROPERTY) in JSON
        public static List<JObject> AddFieldNameInJObject(List<JObject> jsObject, string propertyName, JToken? value)
        {
            foreach (var item in jsObject)
            {
                item.Add(propertyName, value);
            }
            return jsObject;
        }

        // Change/Update field names(KEYS) in JSON
        public static List<JObject> ChangeFieldNameInJObject(List<JObject> jsObject, string currentName, string newName)
        {
            foreach (var item in jsObject)
            {
                item[newName] = item[currentName];
                item.Property(currentName).Remove();
            }
            return jsObject;
        }

        // Change/Update values in JSON
        public static List<JObject> ChangeValuesInJObject(List<JObject> jsObject, string fieldName, JToken? newValue)
        {
            foreach (var item in jsObject.Properties())
            {
                if (item.Name.Equals(fieldName))
                {
                    item.Value = newValue; // or item.Value.Replace(newValue)
                }
            }
            return jsObject;
        }

        // Replace text string (ex: %) of string contains values in JSON
        public static List<JObject> ReplaceContainTextValueInJObject(List<JObject> jsObject, string fieldName, string oldValueTxt, string newValueTxt)
        {
            foreach (var item in jsObject.Properties())
            {
                if (item.Name.Equals(fieldName))
                {
                    var getValue = item.Value.ToString();
                    if (getValue.Contains(oldValueTxt)) 
                    {
                        if (newValueTxt == null)
                        { 
                            item.Value = null; 
                        }
                        else 
                        {
                            var storedRemoveValue = getValue.Replace(oldValueTxt, newValueTxt);
                            item.Value = storedRemoveValue;
                        }
                    }
                }
            }
            return jsObject;
        }

        public static List<JObject> ReplaceTextValueInJObject(List<JObject> jsObject, string fieldName, string oldValue, string? newValue=null)
        {
            foreach (var item in jsObject.Properties())
            {
                if (item.Name.Equals(fieldName))
                {
                    if (item.Value.ToString() == oldValue)
                    {
                        item.Value = newValue;
                    }
                }
            }
            return jsObject;
        }

        // Check if no value then give Null values(Number-double) in JSON
        public static List<JObject> ReplaceNumberValueInJObject(List<JObject> jsObject, string fieldName, double oldValue, string newValue)
        {
            foreach (var item in jsObject.Properties())
            {
                if (item.Name.Equals(fieldName))
                {
                    if (((double)item.Value) == oldValue)
                    {
                        item.Value = newValue;
                    }
                }
            }
            return jsObject;
        }

        // Remove/Trim all white space from the beginning or end of a string values in JSON
        public static List<JObject> RemoveWhiteSpaceBeginAndEndValuesInJObject(List<JObject> jsObject, string fieldName)
        {
            foreach (var item in jsObject.Properties())
            {
                if (item.Name.Equals(fieldName))
                {
                    var removeWhiteSpaceBeginAndEndString = item.Value.ToString().Trim(); // or item.Value.Replace(newValue)
                    item.Value = removeWhiteSpaceBeginAndEndString;
                }
            }
            return jsObject;
        }

        // Remove field names(KEYS) in JSON
        public static List<JObject> RemoveFieldNameInJObject(List<JObject> jsObject, string currentName)
        {
            foreach (var item in jsObject)
            {
                item.Property(currentName).Remove();
            }
            return jsObject;
        }

        // Change data type (string --> double) (To remove double quotes in JSON)
        public static List<JObject> ChangeToDoubleTypeForValueInJObject(List<JObject> jsObject, string fieldName)
        {
            foreach (var item in jsObject)
            {
                if (item.ContainsKey(fieldName))
                {
                    var getValue = item.GetValue(fieldName);
                    var storedValue = getValue.ToObject<double>();
                    item.GetValue(fieldName).Replace(storedValue);
                }
            }
            return jsObject;
        }

        // Change data type(string --> double) with a specific format(To remove double quotes in JSON)
        public static List<JObject> ChangeToFormatDoubleAndIntegerWithNullValueInJObject(List<JObject> jsObject, string fieldName, string format, string? curEmptyValue=null, string? fillEmptyValue = null)
        {
            #region Way 1 
            foreach (var item in jsObject)
            {
                if (item.ContainsKey(fieldName))
                {
                    var getValue = item.Property(fieldName).Value;
                    if (getValue.ToString() == curEmptyValue || getValue.ToString() == "")
                    {
                        getValue.Replace(fillEmptyValue);
                    }

                    else
                    {
                        var convertString = ((double)item.Property(fieldName).Value).ToString(format);
                        if (convertString.Contains("."))
                        {
                            convertString = convertString.Contains(".") ? convertString.TrimStart('0').TrimEnd('0').TrimEnd('.') : convertString.TrimStart('0');
                            item.Property(fieldName).Value = Convert.ToDouble(convertString);
                        }
                        else
                        {
                            item.Property(fieldName).Value = Convert.ToInt32(convertString);
                        }
                    }
                }
            }
            #endregion

            #region Way 2
            //foreach (var item in jsObject.Properties())
            //{
            //    if (item.Name.Equals(fieldName))
            //    {
            //        var getValue = item.Value;
            //        if (getValue.ToString() == curEmptyValue || getValue.ToString()=="") 
            //        {
            //            item.Value = fillEmptyValue;
            //        }
            //        else
            //        {
            //            var convertString = ((double)item.Value).ToString(format); // Ex: format = "0.##"
            //            if (convertString.Contains("."))
            //            {
            //                convertString = convertString.Contains(".") ? convertString.TrimStart('0').TrimEnd('0').TrimEnd('.') : convertString.TrimStart('0');
            //                item.Value = Convert.ToDouble(convertString);
            //            }
            //            else
            //            {
            //                item.Value = Convert.ToInt32(convertString);
            //            }
            //        }
            //    }
            //}
            #endregion

            return jsObject;
        }

        // Change data type (string --> integer) (To remove double quotes in JSON)
        public static List<JObject> ChangeToIntegerTypeForValueInJObject(List<JObject> jsObject, string fieldName)
        {
            foreach (var item in jsObject)
            {
                if (item.ContainsKey(fieldName))
                {
                    var getValue = item.GetValue(fieldName);
                    var storedValue = getValue.ToObject<int>();
                    item.GetValue(fieldName).Replace(storedValue);
                }
            }
            return jsObject;
        }

        // Change/Update DateTime format in JSON
        public static List<JObject> ChangeDateFormatInJObject(List<JObject> jsObject, string dateField, string formatDateTime)
        {
            foreach (var item in jsObject.Properties())
            {
                if (item.Name.Equals(dateField))
                {
                    var convert = ((DateTime)item.Value).ToString(formatDateTime);
                    item.Value.Replace(convert);
                }
            }
            return jsObject;
        }

        // Change/Update DateTime format (with null value) in JSON
        public static List<JObject> ChangeDateFormatWithNullInJObject(List<JObject> jsObject, string dateField, string formatDateTime, string? curEmptyValue = null, string? fillEmptyValue=null)
        {
            foreach (var item in jsObject)
            {
                var getValue = item.Property(dateField).Value;
                if (item.Property(dateField).Value.ToString() == curEmptyValue || item.Property(dateField).Value.ToString() =="")
                {
                    item.GetValue(dateField).Replace(fillEmptyValue);
                }
                else
                {
                    var convert = ((DateTime)getValue).ToString(formatDateTime);
                    item.GetValue(dateField).Replace(convert);
                }
            }
            return jsObject;
        }

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

        #region Datalake API
        // Query tables in database (datalake api)
        public static IRestResponse GetDLAPIAllTables(string database)
        {
            configurationFile();
            var client = new RestClient(datalakeApi);
            client.Timeout = -1;
            var request = new RestRequest("/api/datalake", Method.POST);
            request.AddHeader("contain", "");
            request.AddHeader("Authorization", "Bearer " + bearerToken + "");
            request.AddHeader("Content-Type", "application/json");
            var body = "{" + "\n" + "\"schema\"" + " : " + "\"" + database + "\"," + "\n" + "\"sql\"" + " : " + "\"show tables\"" + "\n" + "}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var myException = new ApplicationException(message, response.ErrorException);
                throw myException;
            }
            return response;
        }

        // Query data (ansi sql) in database (datalake api)
        public static IRestResponse GetDatalakeANSISQLQueries(string database, string query)
        {
            configurationFile();
            var client = new RestClient(datalakeApi);
            client.Timeout = -1;
            var request = new RestRequest("/api/datalake", Method.POST);
            request.AddHeader("contain", "");
            request.AddHeader("Authorization", "Bearer " + bearerToken + "");
            request.AddHeader("Content-Type", "application/json");
            var body = "{" + "\n" + "\"schema\"" + " : " + "\"" + database + "\"," + "\n" 
                                  + "\"sql\"" + " : " + "\"" + query + "\"" + "\n" + 
                       "}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var myException = new ApplicationException(message, response.ErrorException);
                throw myException;
            }
            return response;
        }
        #endregion

        #region Workbench API
        /*// Get List Share Class by fund id (workbench api)
        public static IRestResponse GetListShareClassWorkbenchAPI(string fundid)
        {
            var client = new RestClient(WorkbenchApi);
            client.Timeout = -1;
            var request = new RestRequest("/fund/share_class?fund_id=" + fundid + "", Method.GET);
            request.AddHeader("x-access-token", "" + msalIdtoken + "");
            IRestResponse response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var myException = new ApplicationException(message, response.ErrorException);
                throw myException;
            }

            return response;
        }

        // Get Fund Status by fund id (workbench api)
        public static IRestResponse GetFundStatusWorkbenchAPI(string fundid)
        {
            var client = new RestClient(WorkbenchApi);
            client.Timeout = -1;
            var request = new RestRequest("/fund_Status?fund_id=" + fundid + "", Method.GET);
            request.AddHeader("x-access-token", "" + msalIdtoken + "");
            IRestResponse response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var myException = new ApplicationException(message, response.ErrorException);
                throw myException;
            }

            return response;
        }

        // Get Fund Data by fund id (workbench api)
        public static IRestResponse GetFundDataWorkbenchAPI(string fundid)
        {
            var client = new RestClient(WorkbenchApi);
            client.Timeout = -1;
            var request = new RestRequest("/fund_data?fund_id=" + fundid + "", Method.GET);
            request.AddHeader("x-access-token", "" + msalIdtoken + "");
            IRestResponse response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var myException = new ApplicationException(message, response.ErrorException);
                throw myException;
            }

            return response;
        }

        // Search Fund by text (workbench api)
        public static IRestResponse SearchFundByTextWorkbenchAPI()
        {
            var client = new RestClient(WorkbenchApi);
            client.Timeout = -1;
            var request = new RestRequest("/search/fund", Method.POST);
            request.AddHeader("x-access-token", "" + msalIdtoken + "");
            request.AddHeader("Content-Type", "application/json");
            var body = @"{" + "\n" +            @"  ""search_text"": ""abc""" + "\n" +            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var myException = new ApplicationException(message, response.ErrorException);
                throw myException;
            }

            return response;
        }

        // Search Faceset by text (workbench api)
        public static IRestResponse SearchFacesetByTextWorkbenchAPI()
        {
            var client = new RestClient(WorkbenchApi);
            client.Timeout = -1;
            var request = new RestRequest("/search/faceset", Method.POST);
            request.AddHeader("x-access-token", "" + msalIdtoken + "");
            request.AddHeader("Content-Type", "application/json");
            var body = @"{" + "\n" + @"  ""search_text"": ""s&p""" + "\n" + @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var myException = new ApplicationException(message, response.ErrorException);
                throw myException;
            }

            return response;
        }

        // Get Single Report Manager
        public static IRestResponse GetSingleReportManagerWorkbenchAPI()
        {
            const string dataSourceValue = "Albourne";
            const string strategyValue = "Developed Equity";
            const string projected_teValue = "0.1";
            const string lastDateValue = "2021-09-30";
            const string customValue = "2018-09-01";
            const string custom_start_date_1Value = "2018-09-01";
            const string custom_start_date_2Value = "2016-09-01";
            const string ks_incepValue = "1999-03-01";
            const string indexNameValue = "[\"S & P 500 Industrials(Sector) Net Total Return Index\",\"MSCI Japan Net Total Return USD Index\",\"CASH\"]";
            const string benchmarkIdsValue = "[\"SP125.USD\",\"939200.USD\",\"79499152.USD\"]";
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
            const string hurdle_typeValue = "0";
            const string ramp_typeValue = "0";
            const string benchmarkValue = "SP565.USD";
            const string fundIdValue = "38338";
            const string rfrateIdValue = "79499152.USD";

            var client = new RestClient(WorkbenchApi);
            client.Timeout = -1;
            var request = new RestRequest("/calc/single_manager_report", Method.POST);
            request.AddHeader("x-access-token", "" + msalIdtoken + "");
            request.AddHeader("Content-Type", "application/json");
            var body = "{" + "\n" + "\"data_source\"" + " : " + "\"" + dataSourceValue + "\","
                           + "\n" + "\"strategy\"" + " : " + "\"" + strategyValue + "\","
                           + "\n" + "\"projected_te\"" + " : " + "" + projected_teValue + ","
                           + "\n" + "\"lastDate\"" + " : " + "\"" + lastDateValue + "\","
                           + "\n" + "\"custom\"" + " : " + "\"" + customValue + "\","
                           + "\n" + "\"custom_start_date_1\"" + " : " + "\"" + custom_start_date_1Value + "\","
                           + "\n" + "\"custom_start_date_2\"" + " : " + "\"" + custom_start_date_2Value + "\","
                           + "\n" + "\"ks_incep\"" + " : " + "\"" + ks_incepValue + "\","
                           + "\n" + "\"indexName\"" + " : " + "" + indexNameValue + ","
                           + "\n" + "\"benchmarkIds\"" + " : " + "" + benchmarkIdsValue + ","
                           + "\n" + "\"proj_beta\"" + " : " + "" + proj_betaValue + ","
                           + "\n" + "\"proj_exposure\"" + " : " + "" + proj_exposureValue + ","
                           + "\n" + "\"mgt_fee\"" + " : " + "" + mgt_feeValue + ","
                           + "\n" + "\"mgt_fee_freq\"" + " : " + "" + mgt_fee_freqValue + ","
                           + "\n" + "\"perf_fee\"" + " : " + "" + perf_feeValue + ","
                           + "\n" + "\"hwm_status\"" + " : " + "" + hwm_statusValue + ","
                           + "\n" + "\"catch_up\"" + " : " + "" + catch_upValue + ","
                           + "\n" + "\"catch_up_perc_soft\"" + " : " + "" + catch_up_perc_softValue + ","
                           + "\n" + "\"crystialized_paid\"" + " : " + "" + crystialized_paidValue + ","
                           + "\n" + "\"hurdle_status\"" + " : " + "" + hurdle_statusValue + ","
                           + "\n" + "\"hurdle_fixed\"" + " : " + "" + hurdle_fixedValue + ","
                           + "\n" + "\"perf_return\"" + " : " + "" + perf_returnValue + ","
                           + "\n" + "\"hurdle_type\"" + " : " + "" + hurdle_typeValue + ","
                           + "\n" + "\"ramp_type\"" + " : " + "" + ramp_typeValue + ","
                           + "\n" + "\"benchmark\"" + " : " + "\"" + benchmarkValue + "\","
                           + "\n" + "\"fundId\"" + " : " + "" + fundIdValue + ","
                           + "\n" + "\"rfrateId\"" + " : " + "\"" + rfrateIdValue + "\"" + "\n" +
                       "}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var myException = new ApplicationException(message, response.ErrorException);
                throw myException;
            }

            return response;
        }

        // Query Data upload CSV File
        public static IRestResponse GetDataCSVFileWorkbenchAPI(string fundid)
        {
            // Databse
            const string dbname = "ks_model";
            const string dbnameTemp = "gend_ks_db";

            // Tables
            const string fundAum = "manual_fund_aum";
            const string fundReturn = "manual_fund_ror";
            const string fundExposure = "manual_fund_exposure";

            var client = new RestClient(WorkbenchApi);
            client.Timeout = -1;
            var request = new RestRequest("/executeQuery", Method.POST);
            request.AddHeader("x-access-token", "" + msalIdtoken + "");
            request.AddHeader("Content-Type", "application/json");
            var body = "{" + "\n" + "\"queryString\"" + " : " + "\"SELECT * FROM " + dbname + "." + fundAum + " WHERE fund_id=" + fundid + "\"" + "\n" +
                       "}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var myException = new ApplicationException(message, response.ErrorException);
                throw myException;
            }

            return response;
        }

        // Query Data upload PDF/CRBM File
        public static IRestResponse GetDataPDFcrbmFileWorkbenchAPI()
        {
            // Database
            const string dbname = "ks_model";

            // Historical Exposure - PDF (tables)
            const string exposure_by_region = "manual_exposure_by_region";
            const string exposure_by_sector = "manual_exposure_by_sector";
            const string exposure_summary = "manual_exposure_summary";

            // Performance History - PDF (tables)
            const string performance_summary = "manual_performance_summary";
            const string performance_summary_year_aggregation = "manual_performance_summary_year_aggregation";

            // Benchmark - XLSX (tables)
            const string crbmReturn = "crbm_monthly_return";

            var client = new RestClient(WorkbenchApi);
            client.Timeout = -1;
            var request = new RestRequest("/executeQuery", Method.POST);
            request.AddHeader("x-access-token", "" + msalIdtoken + "");
            request.AddHeader("Content-Type", "application/json");
            var body = "{" + "\n" + "\"queryString\"" + " : " + "\"SELECT * FROM " + dbname + "." + crbmReturn + " \"" + "\n" +
                       "}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var myException = new ApplicationException(message, response.ErrorException);
                throw myException;
            }

            return response;
        }

        // Query Add/Edit Fund & Manager
        public static IRestResponse GetAddEditFundWorkbenchAPI()
        {
            // Databse
            const string dbnameTemp = "gend_ks_db";

            // Tables
            const string tbManager = "manager";
            const string tbFund = "fund_manual";

            var client = new RestClient(WorkbenchApi);
            client.Timeout = -1;
            var request = new RestRequest("/executeQuery", Method.POST);
            request.AddHeader("x-access-token", "" + msalIdtoken + "");
            request.AddHeader("Content-Type", "application/json");
            var body = "{" + "\n" + "\"queryString\"" + " : " + "\"SELECT * FROM " + dbnameTemp + "." + tbManager + " WHERE name like 'Dinh%'\"" + "\n" +
                       "}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var myException = new ApplicationException(message, response.ErrorException);
                throw myException;
            }

            return response;
        }


        // Get msal.idtoken (In-Progress)
        public static void GetmsalIdToken()
        {
            string email = "";
            string password = "";
            var client = new RestClient("https://sso.godaddy.com/?domain=.....");
            client.Authenticator = new HttpBasicAuthenticator(email, password);

            var request = new RestRequest("Resource", Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("client_id", "");
            //add other parameters

            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
        */
        #endregion
    }
}
