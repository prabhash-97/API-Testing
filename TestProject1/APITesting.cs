using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Net;

namespace TestProject1
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class APITests
    {
        RestResponse response;

        [SetUp]
        public void Setup()
        {
            BaseClass baseClass = new BaseClass();
            response = baseClass.BaseClassConfigure();
            Console.WriteLine("Pre Condition for test");
        }

        [Test]
        public void VerifyHttpStatusCode()
        {
            Console.WriteLine("Http Status Code : " + response.StatusCode);
            NUnit.Framework.Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }


        [Test]
        public void VerifyResponseHeader()
        {
            Console.WriteLine("Response Header : " + response.ContentType);
            NUnit.Framework.Assert.That(response.ContentType, Is.EqualTo("application/json"));
        }


        [Test]
        public void VerifyHttpResponseBody()
        {
            JArray jObj = (JArray)JsonConvert.DeserializeObject(response.Content.ToString());
            int count = jObj.Count;
            Console.WriteLine("Array Count : " + count);
            NUnit.Framework.Assert.That(count, Is.EqualTo(10));
        }

        [TearDown]
        public static void TestClean()
        {
            Console.WriteLine("Post Condition for test");
        }
    }
}