using log4net;
using log4net.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using ReportPortal.Shared;
using RestSharp;
using System;
using System.IO;
using System.Net;
using TestProject;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Chrome;

//[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace TestProject1
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class APITests :BaseTest
    {
        RestResponse response;

        private static readonly ILog Logg = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [SetUp]
        public void Setup()
        {
            BaseClass baseClass = new BaseClass();
            response = baseClass.BaseClassConfigure();

            BasicConfigurator.Configure();

            Logg.Info("Pre Condition for test");
        }

        [Test]
        public void VerifyHttpStatusCode()
        {
            try
            {
                Logg.Debug("Http Status Code : " + response.StatusCode);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
            catch(Exception ex)
            {
                Logg.Error("An error happened : Status Code is not OK" + ex);
            }
         
        }

        [Test]
        public void VerifyResponseHeader()
        {
            try
            {
                Logg.Debug("Response Header : " + response.ContentType);
                Assert.That(response.ContentType, Is.EqualTo("application/json"));
            }
            catch
            {
                Logg.Error("An error happened : Response Header is not application/json");
            }
            
        }


        [Test]
        public void VerifyHttpResponseBody()
        {
            try
            {
                JArray jObj = (JArray)JsonConvert.DeserializeObject(response.Content.ToString());
                int count = jObj.Count;
                Logg.Debug("Array Count : " + count);
                Assert.That(count, Is.EqualTo(10));
            }
            catch
            {
                Logg.Error("An error happened : Array Count not eqaul to 10");
            }
            
        }

        [Test]
        public async Task VerifyHttpStatusCodeWriteToFile()
        {
            try
            {
                var expectedResult = HttpStatusCode.OK;

                logger.Log(Microsoft.Extensions.Logging.LogLevel.Debug, "Http Status code test: Actual output: " + response.StatusCode);
                WriteToFile(Microsoft.Extensions.Logging.LogLevel.Debug.ToString(), "Http Status code test: Actual output: " + response.StatusCode);

                Assert.AreEqual(expectedResult, response.StatusCode);

            }
            catch
            {
                logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, "Error occured while running Http Status code  Test");
                WriteToFile(Microsoft.Extensions.Logging.LogLevel.Error.ToString(), "Error occured while running User data count Test");
            }
        }


        [TearDown]
        public void TestClean()
        {
            Logg.Info("Post Condition for test");
        }
    }
}