using RestSharp;

namespace ABSA_Assignment
{
    class Task1 : Util
    {

        public void Run()
        {
            GetList();

            GetRetrieverList();

            GetRandomImage();

            EndTest("Task 1 Completed");
        }

        public void GetList()
        {

            CreateTest("Get List","Task1");

            IRestResponse response = Get("https://dog.ceo/api/breeds/list/all");//validate status

            Pass(response.Content);//validate content

            bool retriever = response.Content.Contains("retriever");

            Pass(retriever.ToString());
        }

        public void GetRetrieverList()
        {
            CreateTest("Get Retriever List");

            IRestResponse response = Get("https://dog.ceo/api/breed/retriever/list");

            Pass(response.Content);
        }

        public void GetRandomImage()
        {

            CreateTest("Get Random Image");

            IRestResponse response = Get("https://dog.ceo/api/breed/retriever/golden/images/random");

            Pass(response.Content);
        }


    }
}