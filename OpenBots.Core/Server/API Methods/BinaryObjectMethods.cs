using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace OpenBots.Core.Server.API_Methods
{
    public class BinaryObjectMethods
    {
        public static void DownloadBinaryObject(RestClient client, Guid? binaryObjectID, string directoryPath, string fileName)
        {
            var request = new RestRequest("api/v1/BinaryObjects/{id}/download", Method.GET);
            request.AddUrlSegment("id", binaryObjectID.ToString());
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");

            byte[] file = response.RawBytes;
            File.WriteAllBytes(Path.Combine(directoryPath, fileName), file);
        }

        public static void UpdateBinaryObject(RestClient client, Guid? binaryObjectID, string filePath)
        {
            var request = new RestRequest("api/v1/BinaryObjects/{id}/upload", Method.PUT);
            request.AddUrlSegment("id", binaryObjectID.ToString());
            request.AddParameter("id", binaryObjectID.ToString());
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddFile("File", filePath);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");
        }

        public static BinaryObject GetBinaryObject(RestClient client, Guid? binaryObjectID, Guid? correlationEntityId = null)
        {
            var request = new RestRequest("api/v1/BinaryObjects/{id}", Method.GET);
            request.AddUrlSegment("id", binaryObjectID.ToString());
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");

            return JsonConvert.DeserializeObject<BinaryObject>(response.Content);
        }

        public static List<BinaryObject> GetBinaryObjectsByCorrelationEntityId(RestClient client, Guid? correlationEntityId = null)
        {
            var request = new RestRequest("api/v1/BinaryObjects", Method.GET);
            if (correlationEntityId != null || correlationEntityId != Guid.Empty)
                request.AddQueryParameter("$filter", $"CorrelationEntityId eq guid'{correlationEntityId}'");

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");

            var deserializer = new JsonDeserializer();
            var output = deserializer.Deserialize<Dictionary<string, string>>(response);
            var items = output["items"];
            return JsonConvert.DeserializeObject<List<BinaryObject>>(items);
        }
    }
}
