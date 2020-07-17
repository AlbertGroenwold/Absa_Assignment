using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ABSA_Assignment
{
    public class Util
    {

        private ExtentTest curTest;
        private string path;
        private AventStack.ExtentReports.ExtentReports report;
        private ChromeDriver driver;

        public IRestResponse Get(string api)
        {
            var client = new RestClient(api);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            return response;
        }

        public void CreateReport(string test, string task)
        {
            path = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\Reports\"+task+@"\" + DateTime.Now.ToString("HH-mm dd-MM-yyyy") + @"\";
            var htmlReport = new ExtentHtmlReporter(path);
            report = new AventStack.ExtentReports.ExtentReports();
            report.AttachReporter(htmlReport);
            CreateTest(test);
            report.Flush();
        }

        public void CreateTest(string TestName, string task = null)
        {
            if (report == null)
            {
                CreateReport(TestName,task);
            }
            else
            {
                report.Flush();
                if (curTest == null || curTest.Model.Name != TestName)
                {
                    curTest = report.CreateTest(TestName);
                    report.Flush();
                }
            }
        }

        public string Pass(string msg)
        {
            curTest.Pass(msg);
            return null;
        }

        public string Fail(string msg)
        {
            curTest.Fail(msg);
            return null;
        }

        public void EndTest(string msg)
        {
            curTest.Pass(msg);
            report.Flush();
        }

        public void LaunchDriver()
        {
            ChromeDriverService chrome = ChromeDriverService.CreateDefaultService(AppDomain.CurrentDomain.BaseDirectory);
            var chromeOptions = new ChromeOptions();

            chromeOptions.AddUserProfilePreference("download.default_directory", path);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("safebrowsing.enabled", true);

            driver = new ChromeDriver(chrome, chromeOptions);
            driver.Manage().Window.Maximize();
        }

        public string Navigate(string url)
        {
            driver.Navigate().GoToUrl(url);

            return null;
        }

        public string Click(By ele)
        {
            try
            {
                driver.FindElement(ele).Click();
                return null;
            }
            catch
            {
                return Fail("Could not click - " + ele.ToString()); ;
            }
        }

        public string Wait(By ele, int num)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(num));
                wait.Until(drv => drv.FindElement(ele));
                wait.Until(drv => drv.FindElement(ele).Displayed);
                wait.Until(drv => drv.FindElement(ele).Enabled);

                return null;
            }
            catch
            {
                return Fail("Could not wait for - " + ele.ToString()); ;
            }
        }

        public string Enter(By ele, string input)
        {
            try
            {
                IWebElement element = driver.FindElement(ele);
                element.Click();
                element.Clear();
                element.SendKeys(input);

                return null;
            }
            catch
            {
                return Fail("Could not enter " + input + " into " + ele.ToString()); ;
            }
        }

        public string Select(By ele, string input)
        {
            try
            {
                SelectElement element = new SelectElement(driver.FindElement(ele));
                element.SelectByText(input);

                return null;
            }
            catch
            {
                return Fail("Could not select " + input + " from " + ele.ToString()); ;
            }
        }

        public IList<IWebElement> FindElements(By ele)
        {
            IList<IWebElement> list = driver.FindElements(ele).ToList();

            return list;
        }

        public string ShutDown()
        {
            try
            {
                driver.Close();
                driver.Quit();

                return null;
            }
            catch (Exception e)
            {
                return "Could not close driver - " + e.Message;
            }
        }

    }
}
