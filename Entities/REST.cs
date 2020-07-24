using RestSharp;

namespace ABSA_Assignment
{
    class REST
    {

        private readonly string basAPI = "https://dog.ceo/api/";

        public IRestResponse Get(string api)
        {
            var client = new RestClient(basAPI + api) { Timeout = -1 };

            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            return response;
        }

    }
}
