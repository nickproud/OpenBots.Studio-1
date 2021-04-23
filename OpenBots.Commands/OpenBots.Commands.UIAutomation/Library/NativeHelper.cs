using OpenBots.Core.ChromeNativeClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBots.Commands.NativeMessaging
{
    public class NativeHelper
    {
        public static WebElement DataTableToWebElement(DataTable SearchParametersDT) 
        {
			WebElement webElement = new WebElement();
			webElement.XPath = (SearchParametersDT.Rows[0].ItemArray[0].ToString().ToLower() == "true") ?
				SearchParametersDT.Rows[0].ItemArray[2].ToString() : "";
			webElement.RelXPath = (SearchParametersDT.Rows[1].ItemArray[0].ToString().ToLower() == "true") ?
				SearchParametersDT.Rows[1].ItemArray[2].ToString() : "";
			webElement.ID = (SearchParametersDT.Rows[2].ItemArray[0].ToString().ToLower() == "true") ?
				SearchParametersDT.Rows[2].ItemArray[2].ToString() : "";
			webElement.Name = (SearchParametersDT.Rows[3].ItemArray[0].ToString().ToLower() == "true") ?
				SearchParametersDT.Rows[3].ItemArray[2].ToString() : "";
			webElement.TagName = (SearchParametersDT.Rows[4].ItemArray[0].ToString().ToLower() == "true") ?
				SearchParametersDT.Rows[4].ItemArray[2].ToString() : "";
			webElement.ClassName = (SearchParametersDT.Rows[5].ItemArray[0].ToString().ToLower() == "true") ?
				SearchParametersDT.Rows[5].ItemArray[2].ToString() : "";
			webElement.LinkText = (SearchParametersDT.Rows[6].ItemArray[0].ToString().ToLower() == "true") ?
				SearchParametersDT.Rows[6].ItemArray[2].ToString() : "";
			webElement.CssSelector = (SearchParametersDT.Rows[7].ItemArray[0].ToString().ToLower() == "true") ?
				SearchParametersDT.Rows[7].ItemArray[2].ToString() : "";
			return webElement;
		}

		public static DataTable WebElementToDataTable(WebElement webElement)
		{
			DataTable SearchParameters = NewSearchParameterDataTable();
			SearchParameters.Rows.Add(false, "XPath", webElement.XPath);
			SearchParameters.Rows.Add(true, "Relative XPath", webElement.RelXPath);
			SearchParameters.Rows.Add(false, "ID", webElement.ID);
			SearchParameters.Rows.Add(false, "Name", webElement.Name);
			SearchParameters.Rows.Add(false, "Tag Name", webElement.TagName);
			SearchParameters.Rows.Add(false, "Class Name", webElement.ClassName);
			SearchParameters.Rows.Add(false, "Link Text", webElement.LinkText);
			SearchParameters.Rows.Add(false, "CSS Selector", webElement.CssSelector);
			return SearchParameters;
		}

		public static DataTable NewSearchParameterDataTable()
		{
			DataTable searchParameters = new DataTable();
			searchParameters.Columns.Add("Enabled");
			searchParameters.Columns.Add("Parameter Name");
			searchParameters.Columns.Add("Parameter Value");
			searchParameters.TableName = DateTime.Now.ToString("UIASearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
			return searchParameters;
		}
    }
}
