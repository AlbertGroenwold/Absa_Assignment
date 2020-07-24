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
        private string path { get; set; }
        public Util(string path)
        {
            this.path = path;
        }

        private ChromeDriver driver { get; set; }

        public ChromeDriver LaunchDriver()
        {
            ChromeDriverService chrome = ChromeDriverService.CreateDefaultService(AppDomain.CurrentDomain.BaseDirectory);
            var chromeOptions = new ChromeOptions();

            chromeOptions.AddUserProfilePreference("download.default_directory", path);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("safebrowsing.enabled", true);

            driver = new ChromeDriver(chrome, chromeOptions);
            driver.Manage().Window.Maximize();

            return driver;
        }

        public string Navigate(string url)
        {
            driver.Navigate().GoToUrl(url);

            return null;
        }

        public string Click(By ele)
        {
            Find(ele).Click();
            return null;
        }

        public IWebElement Find(By ele, int num = 15)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(num));
            wait.Until(drv => drv.FindElement(ele));
            wait.Until(drv => drv.FindElement(ele).Displayed);
            wait.Until(drv => drv.FindElement(ele).Enabled);

            return driver.FindElement(ele);
        }

        public string Enter(By ele, string input)
        {
            IWebElement element = Find(ele);
            element.Click();
            element.Clear();
            element.SendKeys(input);

            return null;
        }

        public string Select(By ele, string input)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(drv => drv.FindElements(ele));

            SelectElement element = new SelectElement(Find(ele));
            element.SelectByText(input);

            return null;
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
                return "Could not close driver - " + e.StackTrace;
            }
        }

    }
}
