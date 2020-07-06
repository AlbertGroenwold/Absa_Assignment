using AventStack.ExtentReports;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABSA_Assignment
{
    class Task1 : Util
    {

        public static AventStack.ExtentReports.ExtentReports run()
        {
            var report = GetList();

            GetRetrieverList(report);

            GetRandomImage(report);

            return report;
        }

        public static AventStack.ExtentReports.ExtentReports GetList()
        {

            AventStack.ExtentReports.ExtentReports report = CreateTest("Get List");

            IRestResponse response = Get("https://dog.ceo/api/breeds/list/all");

            Pass(response.Content);

            bool retriever = response.Content.Contains("retriever");

            Pass(retriever.ToString());

            return report;
        }

        public static AventStack.ExtentReports.ExtentReports GetRetrieverList(AventStack.ExtentReports.ExtentReports report)
        {
            CreateTest("Get Retriever List", report);

            IRestResponse response = Get("https://dog.ceo/api/breed/retriever/list");

            Pass(response.Content);

            return report;

        }

        public static AventStack.ExtentReports.ExtentReports GetRandomImage(AventStack.ExtentReports.ExtentReports report)
        {

            CreateTest("Get Random Image", report);

            IRestResponse response = Get("https://dog.ceo/api/breed/retriever/golden/images/random");

            Pass(response.Content);

            return report;

        }


    }
}