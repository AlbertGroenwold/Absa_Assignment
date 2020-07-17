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
    class Task2 : Util
    {
        private readonly IList<string> users = new List<string>();

        private readonly int maxLoops = 5;

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

        private  readonly By save = By.XPath("//button[text()='Save']");

        public void Run()
        {
            CreateTest("ValidateUserIsPresent", "Task2");

            LaunchDriver();

            Navigate(url);

            ValidateUserIsPresent("User Name", "novak");

            CreateTest("Create new users");

            var users = LoadJson().Users;

            IList<string> newUserName = new List<string>();

            foreach (User user in users)
            {

                Click(addUser);

                Enter(firstName, user.FirstName);

                Enter(lastName, user.LastName);

                string value = getUser();
                newUserName.Add(value);

                Enter(userName, value);

                Enter(password, user.Password);

                if (user.Customer.Contains("AAA")) Click(customerAAA);
                else if (user.Customer.Contains("BBB")) Click(customerBBB);

                Select(role, user.Role);

                Enter(email, user.Email);

                Enter(cellPhone, user.CellPhone);

                Pass("User details entered");

                Click(save);
            }

            CreateTest("Validate new users");

            ValidateNewUsersIsPresent(newUserName);

            EndTest("Task 2 Completed");

            ShutDown();
        }

        

        private string getUser(int num = 1)
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

        private DataTable LoadTable()
        {
            IList<IWebElement> cols = FindElements(header);
            IList<IWebElement> rows = FindElements(body);

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

        private string ValidateUserIsPresent(string colName, string value)
        {
            DataTable table = LoadTable();

            foreach (DataRow row in table.Rows) users.Add(row["User Name"].ToString()); ;

            foreach (DataRow row in table.Rows)
            {
                if (row[colName].ToString().Equals(value)) return Pass("User '" + value + "' is present");
            }

            return Fail("Could not find the user");
        }

        private string ValidateNewUsersIsPresent(IList<string> users)
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
                Fail("Could not load data");
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
