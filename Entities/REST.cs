using RestSharp;

namespace ABSA_Assignment
{
    class REST
    {

        private string baseAPI;

        public REST(string baseAPI)
        {
            this.baseAPI = baseAPI;
        }

        public IRestResponse Get(string api)
        {
            var client = new RestClient(baseAPI + api) { Timeout = -1 };

            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            return response;
        }

    }
}
