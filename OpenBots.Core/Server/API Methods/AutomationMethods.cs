using RestSharp;
using System.Net.Http;

namespace OpenBots.Core.Server.API_Methods
{
    public class AutomationMethods
    {
        public static void UploadAutomation(RestClient client, string name, string filePath, string automationEngine)
        {
            var request = new RestRequest("api/v1/Automations", Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "multipart/form-data"); 
            request.AddFile("File", filePath);
            request.AddParameter("Name", name);
            request.AddParameter("AutomationEngine", automationEngine);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");
        }
    }
}
