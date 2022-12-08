using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class BaseClass
    {
        public RestResponse BaseClassConfigure()
        {
            RestClient client = new RestClient("https://jsonplaceholder.typicode.com");
            RestRequest request = new RestRequest("users", Method.Get);
            Console.WriteLine("Request Method:" + request.Method);
            NUnit.Framework.Assert.That("" + request.Method + "", Is.EqualTo("Get"));

            RestResponse response = client.Execute(request);
            return response;
        }
    }
}
