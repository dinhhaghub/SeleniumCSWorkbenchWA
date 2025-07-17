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
    internal class RatingTests : BaseFunctionTest
    {
        #region Initiate variables
        internal static string? managerId;
        #endregion

        #region TestMethod
        [Test, Category("API Smoke Tests")]
        public void ST001_AddRating()
        {
            // Variables declare
            const int ratingId = 24860; // solovis public
            const string ratingName = "Brevan Howard Alpha Strategies Fund, L.P."; // manager: Brevan Howard Asset Management, LLP
            const string source = "solovis";
            const string type = "fund";
            int edge = 0;
            double organization = 0.5;
            int trackRecord = 1;
            const string conviction = "H";
            int lastRatingByPerson = 0;
            int latestRating = 1;
            var body = "{" + "\n" + "\"ratingId\"" + " : " + ratingId + ","
                           + "\n" + "\"ratingName\"" + " : " + "\"" + ratingName + "\"" + ","
                           + "\n" + "\"source\"" + " : " + "\"" + source + "\"" + ","
                           + "\n" + "\"type\"" + " : " + "\"" + type + "\"" + ","
                           + "\n" + "\"edge\"" + " : " + edge + ","
                           + "\n" + "\"organization\"" + " : " + organization + ","
                           + "\n" + "\"trackRecord\"" + " : " + trackRecord + ","
                           + "\n" + "\"conviction\"" + " : " + "\"" + conviction + "\"" + ","
                           + "\n" + "\"lastRatingByPerson\"" + " : " + lastRatingByPerson + ","
                           + "\n" + "\"latestRating\"" + " : " + "\"" + latestRating + "\"" + "\n" +
                       "}";

            // Check if the data of Sandbox or Staging(Conceptia) site then verify data (on that site)
            if (workbenchApi.Contains("lab"))
            {
                // Add Rating
                var response = WorkbenchApi.AddRating(body, msalIdtoken);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                // Parse IRestResponse to JObject
                JObject? responseJs = JObject.Parse(response.Content);
                Assert.That(responseJs, Has.Count.GreaterThanOrEqualTo(10));
                Assert.Multiple(() =>
                {
                    Assert.That(responseJs["id"].ToString, Is.EqualTo(ratingId.ToString()));
                    Assert.That(responseJs["rating_name"].ToString(), Is.EqualTo(ratingName));
                    Assert.That(responseJs["latest"].ToString, Is.EqualTo("1"));
                    Assert.That(responseJs["edge"].ToString, Is.EqualTo(edge.ToString()));
                    Assert.That(responseJs["organization"].ToString, Is.EqualTo(organization.ToString()));
                    Assert.That(responseJs["track_record"].ToString, Is.EqualTo(trackRecord.ToString()));
                    Assert.That(responseJs["conviction"].ToString, Is.EqualTo(conviction));
                });
            } 
            else Console.WriteLine("ST001 is only add Rating on Sandbox Site!!!");
        }

        [Test, Category("API Smoke Tests")]
        public void ST002_GetFundRating()
        {
            // Variables declare
            const string managerName = "Brevan Howard Asset Management, LLP"; // Solovis Public
            const string ratingId = "24860";
            const string type = "fund";
            const string source = "solovis";

            // Get Fund Rating
            var getFundRating = WorkbenchApi.GetFundRating(ratingId, type, source, msalIdtoken);
            Assert.That(getFundRating.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to List JObject
            List<JObject>? getFundRatingJs = JsonConvert.DeserializeObject<List<JObject>>(getFundRating.Content);
            Assert.That(getFundRatingJs, Has.Count.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(getFundRatingJs[0]["fund_id"].ToString, Is.EqualTo(ratingId));
                //Assert.That(getFundRatingJs[0]["asset_class_0"].ToString, Is.EqualTo("Absolute Return")); // no check due to irr was changed
            });
        }
        #endregion
    }
}
