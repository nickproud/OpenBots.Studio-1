using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using FileModel = OpenBots.Core.Server.Models.File;

namespace OpenBots.Core.Server.API_Methods
{
    public class FileMethods
    {
        public static void DownloadFile(RestClient client, Guid? fileID, string directoryPath, string fileName)
        {
            var request = new RestRequest("api/v1/Files/{id}/download", Method.GET);
            request.AddUrlSegment("id", fileID.ToString());
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");

            byte[] file = response.RawBytes;
            File.WriteAllBytes(Path.Combine(directoryPath, fileName), file);
        }

        public static FileModel GetFile(RestClient client, Guid? id, string driveName = null)
        {
            var request = new RestRequest($"api/v1/Files/{id}", Method.GET);
            request.RequestFormat = DataFormat.Json;

            if (!string.IsNullOrEmpty(driveName))
                request.AddQueryParameter("driveName", driveName);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");

            var deserializer = new JsonDeserializer();
            var output = deserializer.Deserialize<Dictionary<string, string>>(response);
            var items = output["items"];
            var file = JsonConvert.DeserializeObject<List<FileModel>>(items)[0];
            return file;
        }
    }
}
