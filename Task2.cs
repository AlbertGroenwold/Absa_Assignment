using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace ABSA_Assignment
{
    class Task2 : Util
    {
        private static ChromeDriver driver;

        private static IList<string> users = new List<string>();

        private static int maxLoop = 5;

        private static readonly string url = "http://www.way2automation.com/angularjs-protractor/webtables/";

        private static readonly By header = By.XPath(@"//tr[@class= 'smart-table-header-row']//th");
        private static readonly By body = By.XPath(@"//table[@table-title='Smart Table example']//tbody//tr//td");
        private static readonly By addUser = By.XPath("//button[@type='add']");

        private static readonly By firstName = By.XPath("//input[@name='FirstName']");
        private static readonly By lastName = By.XPath("//input[@name='LastName']");
        private static readonly By userName = By.XPath("//input[@name='UserName']");
        private static readonly By password = By.XPath("//input[@name='Password']");
        private static readonly By customerAAA = By.XPath("//input[@value='15']");
        private static readonly By customerBBB = By.XPath("//input[@value='16']");
        private static readonly By role = By.XPath("//select[@name='RoleId']");
        private static readonly By email = By.XPath("//input[@name='Email']");
        private static readonly By cellPhone = By.XPath("//input[@name='Mobilephone']");

        private static readonly By save = By.XPath("//button[text()='Save']");

        public static AventStack.ExtentReports.ExtentReports run()
        {
            var report = CreateTest("ValidateUserIsPresent", null, "Task2");

            driver = LaunchDriver();

            Navigate(url);

            ValidateUserIsPresent("User Name", "novak");

            CreateTest("Create new users", report);

            var users = LoadJson().users;

            IList<string> newUserName = new List<string>();

            foreach (User user in users)
            {

                Click(addUser);

                Enter(firstName, user.firstName);

                Enter(lastName, user.lastName);

                string value = getUser();
                newUserName.Add(value);

                Enter(userName, value);

                Enter(password, user.password);

                if (user.customer.Contains("AAA")) Click(customerAAA);
                else if (user.customer.Contains("BBB")) Click(customerBBB);

                Select(role, user.role);

                Enter(email, user.email);

                Enter(cellPhone, user.cellPhone);

                Pass("User details entered");

                Click(save);
            }

            CreateTest("Validate new users", report);

            ValidateNewUsersIsPresent(newUserName);

            Pass("Task 2 Completed");

            ShutDown(driver);

            return report;
        }

        private static string Navigate(string url)
        {
            driver.Navigate().GoToUrl(url);

            return null;
        }

        private static string Click(By ele)
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

        private static string Wait(By ele, int num)
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

        private static string Enter(By ele, string input)
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

        private static string Select(By ele, string input)
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

        private static string getUser(int num = 1)
        {
            string user = "";
            bool go = false;

            while (!go && num <= maxLoop)
            {
                user = "User" + num;
                if (users.Contains(user))
                {
                    go = false;
                    ++num;
                }
                else
                {
                    go = true;
                }
            }

            users.Add(user);

            return user;
        }

        private static DataTable LoadTable()
        {
            IList<IWebElement> cols = driver.FindElements(header).ToList();
            IList<IWebElement> rows = driver.FindElements(body).ToList();

            DataTable table = new DataTable();

            foreach (var col in cols) table.Columns.Add(col.Text);

            for (int i = 0; i < rows.Count; i += cols.Count)
            {
                DataRow row = table.NewRow();

                for (int j = 0; j < cols.Count; j++)
                {
                    row[j] = rows[i + j].Text;
                }

                table.Rows.Add(row);
            }

            return table;
        }

        private static string ValidateUserIsPresent(string colName, string value)
        {
            DataTable table = LoadTable();

            foreach (DataRow row in table.Rows) users.Add(row["User Name"].ToString()); ;

            foreach (DataRow row in table.Rows)
            {
                if (row[colName].ToString().Equals(value)) return Pass("User '" + value + "' is present");
            }

            return Fail("Could not find the user");
        }

        private static string ValidateNewUsersIsPresent(IList<string> users)
        {
            DataTable table = LoadTable();

            foreach (string user in users)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (row["User Name"].ToString().Equals(user)) { Pass(user + "has been added"); break; }
                }
            }

            return null;
        }

        private static Rootobject LoadJson()
        {
            try
            {
                using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data.json"))
                {
                    string json = r.ReadToEnd();
                    return JsonConvert.DeserializeObject<Rootobject>(json);
                }

            }
            catch
            {
                Fail("Could not load data");
                return null;
            }
        }
        private class Rootobject
        {
            public User[] users { get; set; }
        }

        private class User
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string password { get; set; }
            public string customer { get; set; }
            public string role { get; set; }
            public string email { get; set; }
            public string cellPhone { get; set; }
        }


    }
}
