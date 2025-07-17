using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkbenchApp.FunctionalTest.UIStepsDataManagement
{
    internal class Api
    {
        #region GET, POST, PUT, DELETE
        internal static IRestResponse GetObject(string url, string fEOrBEApi, string? msalIdToken=null)
        {
            var client = new RestClient(fEOrBEApi); // fEOrBEApi --> Datalake or Workbench Api
            client.Timeout = -1;
            var request = new RestRequest(url, Method.GET);
            request.AddHeader("x-access-token", "" + msalIdToken + "");
            var response = Send(client, request);
            return response;
        }
        internal static IRestResponse PostObject(string url, string fEOrBEApi, string? body = null, string? msalIdToken = null)
        {
            var client = new RestClient(fEOrBEApi);
            client.Timeout = -1;
            var request = new RestRequest(url, Method.POST);
            request.AddHeader("x-access-token", "" + msalIdToken + "");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = Send(client, request);
            return response;
        }
        internal static IRestResponse PutObject(string url, string fEOrBEApi, string? body = null, string? msalIdToken = null)
        {
            var client = new RestClient(fEOrBEApi);
            client.Timeout = -1;
            var request = new RestRequest(url, Method.PUT);
            request.AddHeader("x-access-token", "" + msalIdToken + "");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = Send(client, request);
            return response;
        }
        internal static IRestResponse DeleteObject(string url, string fEOrBEApi, string msalIdToken)
        {
            var client = new RestClient(fEOrBEApi);
            client.Timeout = -1;
            var request = new RestRequest(url, Method.DELETE);
            request.AddHeader("x-access-token", "" + msalIdToken + "");
            var response = Send(client, request);
            return response;
        }
        #endregion

        #region Common
        internal static IRestResponse Send(RestClient? client, RestRequest? request)
        {
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
    }
}
