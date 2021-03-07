using Microsoft.Office.Interop.Outlook;
using MimeKit;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OpenBots.Core.Utilities.CommonUtilities
{
    public static class StringMethods
    {
        #region Data Encryption
        private const string SecurityKey = "Openb@ts_password_123";

        /// <summary>
        /// Encrypt the plain text to un-readable format.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="additionalEntropy">The additional entropy.</param>
        /// <returns></returns>
        public static string EncryptText(string plainText, string additionalEntropy)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var entropyBytes = Encoding.UTF8.GetBytes(additionalEntropy);
            var encryptedBytes = ProtectedData.Protect(plainTextBytes, entropyBytes, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// Decrypt the encrypted/un-readable text back to the readable format.
        /// </summary>
        /// <param name="encryptedText">The encrypted text.</param>
        /// <param name="additionalEntropy">The additional entropy.</param>
        /// <returns></returns>
        public static string DecryptText(string encryptedText, string additionalEntropy)
        {
            
            try
            {
                var encryptedBytes = Convert.FromBase64String(encryptedText);
                var entropyBytes = Encoding.UTF8.GetBytes(additionalEntropy);
                var decryptedBytes = ProtectedData.Unprotect(encryptedBytes, entropyBytes, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch
            {
                return encryptedText;
            }
        }
        #endregion Data Encryption

        public static string ToBase64(this string text)
        {
            return ToBase64(text, Encoding.UTF8);
        }

        public static string ToBase64(this string text, Encoding encoding)
        {
            byte[] textAsBytes = encoding.GetBytes(text);
            return Convert.ToBase64String(textAsBytes);
        }

        public static bool TryParseBase64(this string text, out string decodedText)
        {
            return TryParseBase64(text, Encoding.UTF8, out decodedText);
        }

        public static bool TryParseBase64(this string text, Encoding encoding, out string decodedText)
        {
            if (string.IsNullOrEmpty(text))
            {
                decodedText = text;
                return false;
            }

            try
            {
                byte[] textAsBytes = Convert.FromBase64String(text);
                decodedText = encoding.GetString(textAsBytes);
                return true;
            }
            catch (System.Exception)
            {
                decodedText = null;
                return false;
            }
        }

        public static string ConvertObjectToString(object obj, Type type)
        {
            if (obj == null)
                return "null";

            try
            {
                switch (type.FullName)
                {
                    case "System.Security.SecureString":
                        return "*Secure String*";
                    case "System.Data.DataTable":
                        return ConvertDataTableToString((DataTable)obj);
                    case "System.Data.DataRow":
                        return ConvertDataRowToString((DataRow)obj);
                    case "System.__ComObject":
                        return ConvertMailItemToString((MailItem)obj);
                    case "MimeKit.MimeMessage":
                        return ConvertMimeMessageToString((MimeMessage)obj);
                    case "OpenQA.Selenium.Remote.RemoteWebElement":
                        return ConvertIWebElementToString((IWebElement)obj);
                    case "System.Drawing.Bitmap":
                        return ConvertBitmapToString((Bitmap)obj);
                    case "Open3270.TN3270.XMLScreenField":
                        return ConvertXMLScreenFieldToString(obj);
                    case string a when a.Contains("System.Collections.Generic.List`1"):
                        return ConvertListToString(obj);
                    case string a when a.Contains("System.Collections.Generic.Dictionary`2"):
                        return ConvertDictionaryToString(obj);
                    case string a when a.Contains("System.Collections.Generic.KeyValuePair`2"):
                        return ConvertKeyValuePairToString(obj);
                    case "":
                        return "null";
                    default:
                        return obj.ToString();
                }
                
            }
            catch (System.Exception ex)
            {
                return $"Error converting {type} to string - {ex.Message}";
            }           
        }

        public static string ConvertDataTableToString(DataTable dt)
        {
            if (dt == null)
                return "Null";

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[[");

            if (dt.Columns.Count == 0)
                return stringBuilder.Append("]]").ToString();

            for (int i = 0; i < dt.Columns.Count - 1; i++)
                stringBuilder.AppendFormat("{0}, ", dt.Columns[i].ColumnName);

            stringBuilder.AppendFormat("{0}]]", dt.Columns[dt.Columns.Count - 1].ColumnName);
            stringBuilder.AppendLine();

            foreach (DataRow rows in dt.Rows)
            {
                stringBuilder.Append("[");

                for (int i = 0; i < dt.Columns.Count - 1; i++)
                    stringBuilder.AppendFormat("{0}, ", rows[i]);
          
                stringBuilder.AppendFormat("{0}]", rows[dt.Columns.Count - 1]);
                stringBuilder.AppendLine();
            }
            stringBuilder.Length = stringBuilder.Length - 2;
            return stringBuilder.ToString();
        }

        public static string ConvertDataRowToString(DataRow row)
        {
            if (row == null)
                return "Null";

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");

            for (int i = 0; i < row.ItemArray.Length - 1; i++)
                stringBuilder.AppendFormat("{0}, ", row.ItemArray[i]);

            stringBuilder.AppendFormat("{0}]", row.ItemArray[row.ItemArray.Length - 1]);
            return stringBuilder.ToString();
        }

        public static string ConvertMailItemToString(MailItem mail)
        {
            if (mail == null)
                return "Null";

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"[Subject: {mail.Subject}, \n" +
                                  $"Sender: {mail.SenderName}, \n" +
                                  $"Sent On: {mail.SentOn}, \n" +
                                  $"Unread: {mail.UnRead}, \n" +
                                  $"Attachments({mail.Attachments.Count})");

            if (mail.Attachments.Count > 0)
            {
                stringBuilder.Append(" [");
                foreach (Attachment attachment in mail.Attachments)
                    stringBuilder.Append($"{attachment.FileName}, ");

                //trim final comma
                stringBuilder.Length = stringBuilder.Length - 2;
                stringBuilder.Append("]");
            }

            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

        public static string ConvertMimeMessageToString(MimeMessage message)
        {
            if (message == null)
                return "Null";

            int attachmentCount = 0;
            foreach (var attachment in message.Attachments)
                attachmentCount += 1;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"[Subject: {message.Subject}, \n" +
                                  $"Sender: {message.From}, \n" +
                                  $"Sent On: {message.Date}, \n" +
                                  $"Attachments({attachmentCount})");

            if (attachmentCount > 0)
            {
                stringBuilder.Append(" [");
                foreach (var attachment in message.Attachments)
                    stringBuilder.Append($"{attachment.ContentDisposition?.FileName}, " ??
                                         "attached-message.eml, ");

                //trim final comma
                stringBuilder.Length = stringBuilder.Length - 2;
                stringBuilder.Append("]");
            }

            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

        public static string ConvertIWebElementToString(IWebElement element)
        {
            if (element == null)
                return "Null";

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"[Text: {element.Text}, \n" +
                                 $"Tag Name: {element.TagName}, \n" +
                                 $"Location: {element.Location}, \n" +
                                 $"Size: {element.Size}, \n" +
                                 $"Displayed: {element.Displayed}, \n" +
                                 $"Enabled: {element.Enabled}, \n" +
                                 $"Selected: {element.Selected}]");
            return stringBuilder.ToString();
        }

        public static string ConvertBitmapToString(Bitmap bitmap)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Size({bitmap.Width}, {bitmap.Height})");
            return stringBuilder.ToString();
        }

        public static string ConvertXMLScreenFieldToString(dynamic field, int index = -1)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"[Row: {field.Location.top}, Col: {field.Location.left}, \n" +
                                 $"Field Length: {field.Location.length}, \n" +
                                 $"Field Text: {field.Text}");
            if (index != -1)
                stringBuilder.Append($", \nField Index: {index}");

            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

        public static string ConvertListToString(object list)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Type type = list.GetType().GetGenericArguments()[0];

            if (type == typeof(string))
            {
                List<string> stringList = (List<string>)list;
                stringBuilder.Append($"Count({stringList.Count}) [");

                for (int i = 0; i < stringList.Count - 1; i++)
                    stringBuilder.AppendFormat("{0}, ", stringList[i]);

                if (stringList.Count > 0)
                    stringBuilder.AppendFormat("{0}]", stringList[stringList.Count - 1]);
                else
                    stringBuilder.Length = stringBuilder.Length - 2;
            }
            else if (type == typeof(DataTable))
            {
                List<DataTable> dataTableList = ((List<DataTable>)list).Take(3).ToList();
                stringBuilder.Append($"Count({dataTableList.Count}) \n[");

                for (int i = 0; i < dataTableList.Count - 1; i++)
                    stringBuilder.AppendFormat("{0}, \n", ConvertDataTableToString(dataTableList[i]));

                if (dataTableList.Count > 0)
                    stringBuilder.AppendFormat("{0}]", ConvertDataTableToString(dataTableList[dataTableList.Count - 1]));
                else
                    stringBuilder.Length = stringBuilder.Length - 3;
            }
            else if (type == typeof(MailItem))
            {
                List<MailItem> mailItemList = ((List<MailItem>)list).Take(3).ToList();

                stringBuilder.Append($"Count({mailItemList.Count}) \n[");

                for (int i = 0; i < mailItemList.Count - 1; i++)
                    stringBuilder.AppendFormat("{0}, \n", ConvertMailItemToString(mailItemList[i]));

                if (mailItemList.Count > 0)
                    stringBuilder.AppendFormat("{0}]", ConvertMailItemToString(mailItemList[mailItemList.Count - 1]));
                else
                    stringBuilder.Length = stringBuilder.Length - 3;
            }
            else if (type == typeof(MimeMessage))
            {
                List<MimeMessage> mimeMessageList = ((List<MimeMessage>)list).Take(3).ToList();
                stringBuilder.Append($"Count({mimeMessageList.Count}) \n[");

                for (int i = 0; i < mimeMessageList.Count - 1; i++)
                    stringBuilder.AppendFormat("{0}, \n", ConvertMimeMessageToString(mimeMessageList[i]));

                if (mimeMessageList.Count > 0)
                    stringBuilder.AppendFormat("{0}]", ConvertMimeMessageToString(mimeMessageList[mimeMessageList.Count - 1]));
                else
                    stringBuilder.Length = stringBuilder.Length - 3;
            }
            else if (type == typeof(IWebElement))
            {
                List<IWebElement> elementList = ((List<IWebElement>)list).Take(3).ToList();
                stringBuilder.Append($"Count({elementList.Count}) \n[");

                for (int i = 0; i < elementList.Count - 1; i++)
                    stringBuilder.AppendFormat("{0}, \n", ConvertIWebElementToString(elementList[i]));

                if (elementList.Count > 0)
                    stringBuilder.AppendFormat("{0}]", ConvertIWebElementToString(elementList[elementList.Count - 1]));
                else
                    stringBuilder.Length = stringBuilder.Length - 3;
            }
            else if (type.FullName == "Open3270.TN3270.XMLScreenField")
            {
                List<dynamic> fieldList = ((List<dynamic>)list).ToList();

                stringBuilder.Append($"Count({fieldList.Count}) \n[");

                for (int i = 0; i < fieldList.Count - 1; i++)
                    stringBuilder.AppendFormat("{0}, \n", ConvertXMLScreenFieldToString(fieldList[i], i));

                if (fieldList.Count > 0)
                    stringBuilder.AppendFormat("{0}]", ConvertXMLScreenFieldToString(fieldList[fieldList.Count - 1], fieldList.Count - 1));
                else
                    stringBuilder.Length = stringBuilder.Length - 3;
            }
            else
                return list.ToString();

            return stringBuilder.ToString();
        }

        public static string ConvertDictionaryToString(object dictionary)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Type type = dictionary.GetType().GetGenericArguments()[1];
            dynamic stringDictionary = null;

            if (type == typeof(string))
            {
                stringDictionary = (Dictionary<string, string>)dictionary;
                stringBuilder.Append($"Count({stringDictionary.Count}) [");

                foreach (KeyValuePair<string, string> pair in stringDictionary)
                    stringBuilder.AppendFormat("[{0}, {1}], ", pair.Key, pair.Value);
            }
            else if (type == typeof(DataTable))
            {
                stringDictionary = (Dictionary<string, DataTable>)dictionary;
                stringBuilder.Append($"Count({stringDictionary.Count}) [");

                foreach (KeyValuePair<string, DataTable> pair in stringDictionary)
                    stringBuilder.AppendFormat("[{0}, \n{1}], ", pair.Key, ConvertDataTableToString(pair.Value));
            }
            else if (type == typeof(MailItem))
            {
                stringDictionary = (Dictionary<string, MailItem>)dictionary;
                stringBuilder.Append($"Count({stringDictionary.Count}) [");

                foreach (KeyValuePair<string, MailItem> pair in stringDictionary)
                    stringBuilder.AppendFormat("[{0}, \n{1}], ", pair.Key, ConvertMailItemToString(pair.Value));
            }
            else if (type == typeof(MimeMessage))
            {
                stringDictionary = (Dictionary<string, MimeMessage>)dictionary;
                stringBuilder.Append($"Count({stringDictionary.Count}) [");

                foreach (KeyValuePair<string, MimeMessage> pair in stringDictionary)
                    stringBuilder.AppendFormat("[{0}, \n{1}], ", pair.Key, ConvertMimeMessageToString(pair.Value));
            }
            else if (type == typeof(IWebElement))
            {
                stringDictionary = (Dictionary<string, IWebElement>)dictionary;
                stringBuilder.Append($"Count({stringDictionary.Count}) [");

                foreach (KeyValuePair<string, IWebElement> pair in stringDictionary)
                    stringBuilder.AppendFormat("[{0}, \n{1}], ", pair.Key, ConvertIWebElementToString(pair.Value));
            }
            else if (type == typeof(object))
            {
                stringDictionary = (Dictionary<string, object>)dictionary;
                stringBuilder.Append($"Count({stringDictionary.Count}) [");

                foreach (KeyValuePair<string, object> pair in stringDictionary)
                    stringBuilder.AppendFormat("[{0}, {1}], ", pair.Key, pair.Value == null ?
                                                string.Empty : pair.Value.ToString());
            }
            else
                return dictionary.ToString();

            if (stringDictionary.Count > 0)
            {
                stringBuilder.Length = stringBuilder.Length - 2;
                stringBuilder.Append("]");
            }
            else
                stringBuilder.Length = stringBuilder.Length - 2;

            return stringBuilder.ToString();
        }

        public static string ConvertKeyValuePairToString(object pair)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Type type = pair.GetType().GetGenericArguments()[1];

            if (type == typeof(string))
            {
                KeyValuePair<string, string> stringPair = (KeyValuePair<string,string>)pair;
                stringBuilder.AppendFormat("[{0}, {1}]", stringPair.Key, stringPair.Value);
            }
            else if (type == typeof(DataTable))
            {
                KeyValuePair<string, DataTable> stringPair = (KeyValuePair<string, DataTable>)pair;
                stringBuilder.AppendFormat("[{0}, {1}]", stringPair.Key, ConvertDataTableToString(stringPair.Value));
            }
            else if (type == typeof(MailItem))
            {
                KeyValuePair<string, MailItem> stringPair = (KeyValuePair<string, MailItem>)pair;
                stringBuilder.AppendFormat("[{0}, {1}]", stringPair.Key, ConvertMailItemToString(stringPair.Value));
            }
            else if (type == typeof(MimeMessage))
            {
                KeyValuePair<string, MimeMessage> stringPair = (KeyValuePair<string, MimeMessage>)pair;
                stringBuilder.AppendFormat("[{0}, {1}]", stringPair.Key, ConvertMimeMessageToString(stringPair.Value));
            }
            else if (type == typeof(IWebElement))
            {
                KeyValuePair<string, IWebElement> stringPair = (KeyValuePair<string, IWebElement>)pair;
                stringBuilder.AppendFormat("[{0}, {1}]", stringPair.Key, ConvertIWebElementToString(stringPair.Value));
            }
            else
                return pair.ToString();

            return stringBuilder.ToString();
        }
    }
}
