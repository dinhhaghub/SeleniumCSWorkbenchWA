using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Xml;
using Microsoft.Identity.Client;

namespace WorkbenchApp.FunctionalTest
{
    internal class Config
    {

        // Initiate variables (to get msal.idtoken)
        internal static readonly string? userName = ConfigurationManager.AppSettings.Get("userName");
        internal static readonly string? password = ConfigurationManager.AppSettings.Get("password");
        internal static readonly string? clientId = ConfigurationManager.AppSettings.Get("clientId");
        internal static readonly string? tenantId = ConfigurationManager.AppSettings.Get("tenantId");
        internal static string authority = $"https://login.microsoftonline.com/{tenantId}";
        internal static string[] scopes = new[] { "Files.Read.All", "openid", "profile", "User.Read", "email" };
        internal static readonly string? redirectUri = ConfigurationManager.AppSettings.Get("redirectUri");
        internal static IPublicClientApplication? app;

        // Login to get msal.idtoken - MSAL (Microsoft Authentication Library)
        internal static async Task<AuthenticationResult> LoginAsync()
        {
            app = PublicClientApplicationBuilder.Create(clientId)
                 .WithAuthority(authority)
                 .WithRedirectUri(redirectUri) // Redirect URI for desktop/mobile apps
                 .Build();
            AuthenticationResult? result = await app.AcquireTokenByUsernamePassword(scopes, userName, password).ExecuteAsync();
            return result;
        }

        // AppSettings get value from .config file
        internal static readonly string? projectName = ConfigurationManager.AppSettings.Get("ProjectName");
        internal static readonly string? siteName = ConfigurationManager.AppSettings.Get("SiteName");
        internal static readonly string? msalIdtoken = LoginAsync().Result.IdToken;

        // load config.xml file
        internal static readonly string xmlFilePath = Path.GetFullPath(@"../../../../WorkbenchApp.FunctionalTest/" + @"App.config");
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
        internal static string? datalakeApi = null, workbenchApi = null, bearerToken = null;
        internal static XmlDocument configurationFile()
        {
            XmlDocument xdoc = xmlDocLoad(xmlFilePath);
            // Parse an XML file (Get Project & Site name to run)
            foreach (XmlNode node in xdoc.DocumentElement.ChildNodes)
            {
                if (node.Name == "siteSettings")
                {
                    foreach (XmlNode siteSettingsNode in node.ChildNodes)
                    {
                        if (siteSettingsNode.Name == projectName)
                        {
                            foreach (XmlNode settingsNode in siteSettingsNode.ChildNodes)
                            {
                                if (settingsNode["instanceName"].Attributes["value"].Value == siteName)
                                {
                                    XmlNode? currentNode = settingsNode.SelectSingleNode("instanceName[@value='" + siteName + "']");
                                    XmlNode? parentNode = currentNode.ParentNode;
                                    datalakeApi = parentNode["datalake"].Attributes["value"].Value;
                                    workbenchApi = parentNode["workbench"].Attributes["value"].Value;
                                    bearerToken = parentNode["bearertoken"].Attributes["value"].Value;
                                }
                            }
                        }
                    }
                }
            }
            return xdoc;
        }
    }
}
