using RestSharp;

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

            //CreateTest("Get List","Task1");

            IRestResponse response = rest.Get("breeds/list/all");
            var s  = response.StatusCode;//validate status
            //validate content


            report.Pass(response.Content);

            bool retriever = response.Content.Contains("retriever");

            report.Pass(retriever.ToString());
        }

        public void GetRetrieverList()
        {
            //CreateTest("Get Retriever List");

            IRestResponse response = rest.Get("breed/retriever/list");
            var s = response.StatusCode;//validate status
            //validate content

            report.Pass(response.Content);
        }

        public void GetRandomImage()
        {

            //CreateTest("Get Random Image");

            IRestResponse response = rest.Get("/breed/retriever/golden/images/random");
            var s = response.StatusCode;//validate status
            //validate content

            report.Pass(response.Content);
        }


    }
}