using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace ABSA_Assignment
{
    class Task2
    {

        private readonly string url = "http://www.way2automation.com/angularjs-protractor/webtables/";

        private readonly By header = By.XPath(@"//tr[@class= 'smart-table-header-row']//th");
        private readonly By body = By.XPath(@"//table[@table-title='Smart Table example']//tbody//tr//td");
        private readonly By addUser = By.XPath("//button[@type='add']");

        private readonly By firstName = By.XPath("//input[@name='FirstName']");
        private readonly By lastName = By.XPath("//input[@name='LastName']");
        private readonly By userName = By.XPath("//input[@name='UserName']");
        private readonly By password = By.XPath("//input[@name='Password']");
        private readonly By customerAAA = By.XPath("//input[@value='15']");
        private readonly By customerBBB = By.XPath("//input[@value='16']");
        private readonly By role = By.XPath("//select[@name='RoleId']");
        private readonly By email = By.XPath("//input[@name='Email']");
        private readonly By cellPhone = By.XPath("//input[@name='Mobilephone']");

        private readonly By save = By.XPath("//button[text()='Save']");


        private Driver util;
        private Report report;

        private IList<string> newUserName;
        private readonly int maxLoops = 5;
        private readonly IList<string> users = new List<string>();

        public Task2(Driver util, Report report)
        {
            this.util = util;
            this.report = report;
        }

        public void AddUsers()
        {
            
            var users = LoadJson().Users;

            newUserName = new List<string>();

            foreach (User user in users)
            {

                util.Click(addUser);

                util.Enter(firstName, user.FirstName);

                util.Enter(lastName, user.LastName);

                string value = getUser();
                newUserName.Add(value);

                util.Enter(userName, value);

                util.Enter(password, user.Password);

                if (user.Customer.Contains("AAA")) util.Click(customerAAA);
                else if (user.Customer.Contains("BBB")) util.Click(customerBBB);

                util.Select(role, user.Role);

                util.Enter(email, user.Email);

                util.Enter(cellPhone, user.CellPhone);

                report.Pass("User details entered");

                util.Click(save);
            }

            
        }

        public string getUser(int num = 1)
        {
            string user = "";
            bool go = false;

            while (!go && num <= maxLoops)
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

        public DataTable LoadTable()
        {
            IList<IWebElement> cols = util.FindElements(header);
            IList<IWebElement> rows = util.FindElements(body);

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

        public string ValidateUserIsPresent(string colName, string value)
        {

            util.Navigate(url);

            DataTable table = LoadTable();

            foreach (DataRow row in table.Rows) users.Add(row["User Name"].ToString()); ;

            foreach (DataRow row in table.Rows)
            {
                if (row[colName].ToString().Equals(value)) return report.Pass("User '" + value + "' is present");
            }

            return report.Fail("Could not find the user");
        }

        public string ValidateNewUsersIsPresent()
        {
            DataTable table = LoadTable();

            foreach (string user in newUserName)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (row["User Name"].ToString().Equals(user)) { report.Pass(user + " has been added"); break; }
                }
            }

            return null;
        }

        private Rootobject LoadJson()
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
                report.Fail("Could not load data");
                return null;
            }
        }

        private class Rootobject
        {
            public User[] Users { get; set; }
        }

        private class User
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Password { get; set; }
            public string Customer { get; set; }
            public string Role { get; set; }
            public string Email { get; set; }
            public string CellPhone { get; set; }
        }


    }
}
