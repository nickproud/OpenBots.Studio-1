using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using OpenBots.Server.SDK.Api;
using System;
using System.Collections.Generic;
using System.IO;
using static OpenBots.Core.Server.User.EnvironmentSettings;

namespace OpenBots.Commands.Server.HelperMethods
{
    public class ServerEmailMethods
    {
        public static void SendServerEmail(string token, string serverUrl, string organizationId, EmailMessage emailMessage, List<string> attachments, string accountName)
        {
            var apiInstance = new EmailsApi(serverUrl);
            apiInstance.Configuration.AccessToken = token;

            try
            {
                if (attachments != null)
                {
                    List<FileStream> attachmentsList = new List<FileStream>();
                    var emailMessageJson = JsonConvert.SerializeObject(emailMessage);

                    foreach (var attachment in attachments)
                    {
                        FileStream _file = new FileStream(attachment, FileMode.Open, FileAccess.Read);
                        attachmentsList.Add(_file);
                    }

                    apiInstance.ApiVapiVersionEmailsSendPostAsyncWithHttpInfo(apiVersion, organizationId, emailMessageJson, attachmentsList, accountName).Wait();

                    foreach (var file in attachmentsList)
                    {
                        file.Flush();
                        file.Dispose();
                        file.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling EmailsApi.ApiVapiVersionEmailsSendPostAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }
    }
}
