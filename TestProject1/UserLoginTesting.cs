using NUnit.Framework.Interfaces;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using RestSharp;
using log4net.Config;
using TestProject1;
using TestProject;

namespace HomeTask
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class UserLoginTesing:BaseTest
    {
        IWebDriver driver;

        RestResponse response;

        private static readonly ILog Logg = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [SetUp]
        public void StartBrowser()
        {
            BaseClass baseClass = new BaseClass();
            response = baseClass.BaseClassConfigure();

            BasicConfigurator.Configure();

            Logg.Info("Pre Condition for test");
        }

        private IWebElement username => driver.FindElement(By.XPath("//*[@id=\"user-name\"]"));
        private IWebElement password => driver.FindElement(By.XPath("//*[@id=\"password\"]"));
        private IWebElement submitbtn => driver.FindElement(By.XPath("//*[@id=\"login-button\"]"));

        [Test]
        public void UserLogin()
        {
            driver = new ChromeDriver("C:\\Users\\UPRABKA\\Documents\\C# traning\\5 - Selenium Web Driver\\chromedriver_win32\\");
            driver.Manage().Window.Maximize();
            driver.Url = "https://www.saucedemo.com/";
            username.SendKeys("standard_userr");
            password.SendKeys("secret_saucee");
            submitbtn.Click();

            Assert.AreEqual("https://www.saucedemo.com/inventory.html", driver.Url);

            Logg.Info("User logged sucesfully");
        }

        [TearDown]
        public void TestClean()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var TakeScreenshot = ((ITakesScreenshot)driver).GetScreenshot();
                Console.WriteLine(TakeScreenshot.GetType);
                TakeScreenshot.SaveAsFile("C:\\kash\\TestProject2\\ScreenShots\\error.png", ScreenshotImageFormat.Png);
            }
            else
            {
                Console.WriteLine("out");
            }
            driver.Quit();
            Logg.Info("Post Condition for test");
        }
    }
}
