using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium.Chrome;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABSA_Assignment
{
    public class Util
    {

        private static ExtentTest curTest;
        private static string path = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\Reports\";

        public static IRestResponse Get(string api)
        {
            var client = new RestClient(api);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            return response;
        }

        public static AventStack.ExtentReports.ExtentReports CreateReport(string test)
        {
            path += DateTime.Now.ToString("HH-mm dd-MM-yyyy") + @"\";
            var htmlReport = new ExtentHtmlReporter(path);
            var report = new AventStack.ExtentReports.ExtentReports();
            report.AttachReporter(htmlReport);
            report = CreateTest(test, report);
            report.Flush();

            return report;
        }

        public static AventStack.ExtentReports.ExtentReports CreateTest(string TestName, AventStack.ExtentReports.ExtentReports report = null)
        {
            if (report == null)
            {
                report = CreateReport(TestName);
            }
            else
            {
                if (curTest == null || curTest.Model.Name != TestName)
                {
                    curTest = report.CreateTest(TestName);
                    report.Flush();
                }
            }

            return report;
        }

        public static string Pass(string msg)
        {
            curTest.Pass(msg);
            return null;
        }

        public static string Fail(string msg)
        {
            curTest.Fail(msg);
            return null;
        }

        public static ChromeDriver LaunchDriver()
        {
            ChromeDriverService chrome = ChromeDriverService.CreateDefaultService(AppDomain.CurrentDomain.BaseDirectory);
            var chromeOptions = new ChromeOptions();

            chromeOptions.AddUserProfilePreference("download.default_directory", path);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("safebrowsing.enabled", true);

            ChromeDriver driver = new ChromeDriver(chrome, chromeOptions);
            driver.Manage().Window.Maximize();

            return driver;
        }

        public static string ShutDown(ChromeDriver driver, AventStack.ExtentReports.ExtentReports report = null)
        {
            try
            {
                driver.Close();
                driver.Quit();

                if (report != null) report.Flush();

                return null;
            }
            catch (Exception e)
            {
                return "Could not close driver - " + e.Message;
            }
        }

    }
}
