using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using RestSharp;
using System.Collections.Generic;
using System.Net.Http;

namespace OpenBots.Core.Server.API_Methods
{
    public class ServerEmailMethods
    {
        public static void SendServerEmail(RestClient client, EmailMessage emailMessage, string attachments, string accountName)
        {
            var request = new RestRequest("api/v1/Emails/send", Method.POST);
            if (!string.IsNullOrEmpty(accountName))
                request.AddQueryParameter("accountName", accountName);
            var emailMessageJson = JsonConvert.SerializeObject(emailMessage);
            request.AddParameter("EmailMessageJson", emailMessageJson);
            request.RequestFormat = DataFormat.Json;

            if (!string.IsNullOrEmpty(attachments))
            {
                var splitAttachments = attachments.Split(';');
                foreach (var attachment in splitAttachments)
                    request.AddFile("Files", attachment);
            }

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");
        }

        public static List<EmailAddress> GetEmailList(string recipients)
        {
            var emailList = new List<EmailAddress>();

            if (!string.IsNullOrEmpty(recipients))
            {
                var splitRecipients = recipients.Split(';');
                foreach (var recipient in splitRecipients)
                {
                    var email = new EmailAddress()
                    {
                        Name = recipient,
                        Address = recipient
                    };
                    emailList.Add(email);
                }
            }

            return emailList;
        }
    }
}
