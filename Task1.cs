using RestSharp;
using System.Net;

namespace ABSA_Assignment
{
    class Task1
    {

        private REST rest { get; set; }
        private Report report { get; set; }

        public Task1(REST rest, Report report)
        {
            this.rest = rest;
            this.report = report;
        }

        public void GetList()
        {
            IRestResponse response = rest.Get("breeds/list/all");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //validate content
                report.Pass(response.Content);

                bool retriever = response.Content.Contains("retriever");

                report.Pass(retriever.ToString());
            }
            else
            {
                report.Fail("Didn't get a response");
            }
        }

        public void GetRetrieverList()
        {
            IRestResponse response = rest.Get("breed/retriever/list");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //validate content

                report.Pass(response.Content);
            }
            else
            {
                report.Fail("Didn't get a response");
            }
        }

        public void GetRandomImage()
        {
            IRestResponse response = rest.Get("/breed/retriever/golden/images/random");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //validate content

                report.Pass(response.Content);
            }
            else
            {
                report.Fail("Didn't get a response");
            }
        }


    }
}