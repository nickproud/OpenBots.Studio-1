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

        public static string GetRealTypeFullName(this Type t)
        {
            if (!t.IsGenericType)
                return t.FullName;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(t.FullName.Substring(0, t.FullName.IndexOf('`')));
            stringBuilder.Append('<');
            bool appendComma = false;
            foreach (Type arg in t.GetGenericArguments())
            {
                if (appendComma) 
                    stringBuilder.Append(',');
                stringBuilder.Append(GetRealTypeFullName(arg));
                appendComma = true;
            }
            stringBuilder.Append('>');
            return stringBuilder.ToString();
        }

        public static string GetRealTypeName(this Type t)
        {
            if (!t.IsGenericType)
                return t.Name;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(t.Name.Substring(0, t.Name.IndexOf('`')));
            stringBuilder.Append('<');
            bool appendComma = false;
            foreach (Type arg in t.GetGenericArguments())
            {
                if (appendComma) 
                    stringBuilder.Append(',');
                stringBuilder.Append(GetRealTypeName(arg));
                appendComma = true;
            }
            stringBuilder.Append('>');
            return stringBuilder.ToString();
        }

        public static string ConvertObjectToString(object obj, Type type)
        {
            if (obj == null)
                return "null";

            try
            {
                switch (type.FullName)
                {
                    case "System.String":
                        return $"\"{obj}\"";
                    case "System.Boolean":
                        return obj.ToString().ToLower();
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
                    case string a when a.StartsWith("System.Collections.Generic.List`1"):
                        return ConvertListToString(obj);
                    case string a when a.StartsWith("System.Collections.Generic.Dictionary`2"):
                        return ConvertDictionaryToString(obj);
                    case string a when a.StartsWith("System.Collections.Generic.KeyValuePair`2"):
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
                return "null";

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
            if (row == null || row.RowState == DataRowState.Detached)
                return "null";

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
                return "null";

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
                return "null";

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
                return "null";

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
            Type type = list.GetType();
            
            if (type.IsGenericType)
                type = type.GetGenericArguments()[0];
            else
                return "null";

            if (type == typeof(string) || type.IsPrimitive)
            {
                dynamic stringList = list;
                stringBuilder.Append($"Count({stringList.Count}) [");

                for (int i = 0; i < stringList.Count - 1; i++)
                    stringBuilder.AppendFormat("{0}, ", ConvertObjectToString(stringList[i], type));

                if (stringList.Count > 0)
                    stringBuilder.AppendFormat("{0}]", ConvertObjectToString(stringList[stringList.Count - 1], type));
                else
                    stringBuilder.Length = stringBuilder.Length - 2;
            }
            else
            {
                dynamic complexList = list;
                stringBuilder.Append($"Count({complexList.Count}) \n[");

                for (int i = 0; i < 4; i++)
                {
                    if (i < complexList.Count - 1)
                        stringBuilder.AppendFormat("{0}, \n", ConvertObjectToString(complexList[i], type));
                }

                if (complexList.Count > 0)
                    stringBuilder.AppendFormat("{0}]", ConvertObjectToString(complexList[complexList.Count - 1], type));
                else
                    stringBuilder.Length = stringBuilder.Length - 3;

                if (complexList.Count > 5)
                    stringBuilder.Append("...");
            }

            return stringBuilder.ToString();
        }

        public static string ConvertDictionaryToString(object dictionary)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Type type = dictionary.GetType();
            Type keyType;
            Type valueType;

            if (type.IsGenericType)
            {
                keyType = type.GetGenericArguments()[0];
                valueType = type.GetGenericArguments()[1];
            }
            else
                return "null";

            if ((keyType == typeof(string) || keyType.IsPrimitive) && (valueType == typeof(string) || valueType.IsPrimitive))
            {
                dynamic stringDictionary = dictionary;
                stringBuilder.Append($"Count({stringDictionary.Count}) [");

                foreach (dynamic pair in stringDictionary)
                    stringBuilder.AppendFormat("[{0}, {1}], ", ConvertObjectToString(pair.Key, keyType), ConvertObjectToString(pair.Value, valueType));

                if (stringDictionary.Count > 0)
                {
                    stringBuilder.Length = stringBuilder.Length - 2;
                    stringBuilder.Append("]");
                }
                else
                    stringBuilder.Length = stringBuilder.Length - 2;
            }
            else
            {
                dynamic complexDictionary = dictionary;
                stringBuilder.Append($"Count({complexDictionary.Count}) [");

                int count = 0;
                foreach (dynamic pair in complexDictionary)
                {
                    if (count < 5)
                    {
                        stringBuilder.AppendFormat("\n[{0}, {1}], ", ConvertObjectToString(pair.Key, keyType), ConvertObjectToString(pair.Value, valueType));
                        count++;
                    }
                }

                if (complexDictionary.Count > 0)
                {
                    stringBuilder.Length = stringBuilder.Length - 2;
                    stringBuilder.Append("]");
                }
                else
                    stringBuilder.Length = stringBuilder.Length - 2;

                if (complexDictionary.Count > 5)
                {
                    stringBuilder.Length = stringBuilder.Length - 1;
                    stringBuilder.Append("...");
                }
            }

            return stringBuilder.ToString();
        }

        public static string ConvertKeyValuePairToString(object pair)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Type type = pair.GetType();
            Type keyType;
            Type valueType;

            if (type.IsGenericType)
            {
                keyType = type.GetGenericArguments()[0];
                valueType = type.GetGenericArguments()[1];
            }
            else
                return "null";

            dynamic dynamicPair = pair;

            stringBuilder.AppendFormat("[{0}, {1}]", ConvertObjectToString(dynamicPair.Key, keyType), ConvertObjectToString(dynamicPair.Value, valueType));

            return stringBuilder.ToString();
        }
    }
}
