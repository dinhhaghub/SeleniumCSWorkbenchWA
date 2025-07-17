using RestSharp;
using WorkbenchApp.FunctionalTest.UIStepsDataManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace WorkbenchApp.FunctionalTest
{
    internal class DatalakeApi
    {
        // Initiate variables
        //internal static readonly XDocument xdoc = XDocument.Load(@"Config\Config.xml");

        // Api url (Environment)
        //internal static string datalakeApi = xdoc.XPathSelectElement("config/webApis").Attribute("datalakeApi").Value;
        internal static string datalakeApi = Config.datalakeApi;
        private static IRestResponse? response;

        #region Query Private Fund Manual Api
        // POST - Query Data in Data Lake
        public static IRestResponse QueryDataInDataLake(string url, string db, string query, string bearerToken)
        {
            var body = "{" + "\n" + "\"schema\"" + " : " + "\"" + db + "\"," + "\n"
                                  + "\"sql\"" + " : " + "\"" + query + "\"" + "\n" +
                       "}";
            response = Api.PostObject(url, datalakeApi, body, bearerToken); // ex: url = "/file-mapping"
            return response;
        }
        #endregion
    }
}