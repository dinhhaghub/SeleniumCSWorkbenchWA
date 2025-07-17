using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WorkbenchApp.FunctionalTest.PredefinedScenarios
{
    [TestFixture]
    internal class BaseFunctionTest
    {
        public static string? bearerToken = null;
        public static string? msalIdtoken = null;
        public static string? datalakeApi = null;
        public static string? workbenchApi = null;

        [SetUp]
        public void Setup()
        {
            Config.configurationFile();
            bearerToken = Config.bearerToken;
            msalIdtoken = Config.msalIdtoken;
            datalakeApi = Config.datalakeApi;
            workbenchApi = Config.workbenchApi;
        }
    }
}
