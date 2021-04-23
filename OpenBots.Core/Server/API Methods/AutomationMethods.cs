using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace OpenBots.Core.Server.API_Methods
{
    public class AutomationMethods
    {
        public static Automation UploadAutomation(RestClient client, string name, string filePath, string automationEngine)
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

            return JsonConvert.DeserializeObject<Automation>(response.Content);
        }

        public static void UpdateParameters(RestClient client, Guid? automationId, IEnumerable<AutomationParameter> automationParameters)
        {
            var request = new RestRequest("api/v1/Automations/{automationId}/UpdateParameters", Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddUrlSegment("automationId", automationId.ToString());
            request.AddJsonBody(automationParameters);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");
        }
    }
}
