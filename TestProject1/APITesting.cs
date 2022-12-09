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

//[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace TestProject1
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class APITests
    {
        RestResponse response;

        IWebDriver driver;
        
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [SetUp]
        public void Setup()
        {
            BaseClass baseClass = new BaseClass();
            response = baseClass.BaseClassConfigure();

            //XmlConfigurator.Configure();
            BasicConfigurator.Configure();

            Log.Info("Pre Condition for test");
        }

        [Test]
        public void VerifyHttpStatusCode()
        {
            try
            {
                Log.Debug("Http Status Code : " + response.StatusCode);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
            catch
            {
                Log.Error("An error happened : Status Code is not OK");
            }
         
        }


        [Test]
        public void VerifyResponseHeader()
        {
            try
            {
                Log.Debug("Response Header : " + response.ContentType);
                Assert.That(response.ContentType, Is.EqualTo("application/json"));
            }
            catch
            {
                Log.Error("An error happened : Response Header is not application/json");
            }
            
        }


        [Test]
        public void VerifyHttpResponseBody()
        {
            try
            {
                JArray jObj = (JArray)JsonConvert.DeserializeObject(response.Content.ToString());
                int count = jObj.Count;
                Log.Debug("Array Count : " + count);
                Assert.That(count, Is.EqualTo(10));
            }
            catch
            {
                Log.Error("An error happened : Array Count not eqaul to 10");
            }
            
        }

        
        [TearDown]
        public void TestClean()
        {
            Log.Info("Post Condition for test");
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                Screenshot TakeScreenshot = ((ITakesScreenshot)driver).GetScreenshot();
                TakeScreenshot.SaveAsFile("C:\\kash\\TestProject1\\ScreenShots");
            }
        }
    }
}