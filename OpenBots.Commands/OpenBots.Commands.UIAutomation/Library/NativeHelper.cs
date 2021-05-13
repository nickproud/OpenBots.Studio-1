using Newtonsoft.Json;
using OpenBots.Core.ChromeNative.Extension;
using OpenBots.Core.ChromeNativeClient;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.User32;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.UIAutomation.Library
{
    public static class NativeHelper
    {
		public const string SearchParameterSample = "XPath : \"//*[@id='features']/div[2]/div/h2/div[\" + var1 + \"]/div\"" +
													 "\n\tRelative XPath : //*[@id='features']" +
													 "\n\tID: \"1\"" +
													 "\n\tName: \"my\" + var2 + \"Name\"" +
													 "\n\tTag Name: \"h1\"" +
													 "\n\tClass Name: \"myClass\"" +
													 "\n\tCSS Selector: \"[attribute=value]\"" +
													 "\n\tLink Text: \"https://www.mylink.com/\"";

		public async static Task<WebElement> DataTableToWebElement(DataTable SearchParametersDT, IAutomationEngineInstance engine) 
        {
			WebElement webElement = new WebElement();
			webElement.XPath = (SearchParametersDT.Rows[0].ItemArray[0].ToString().ToLower() == "true") ?
				(string)await SearchParametersDT.Rows[0].ItemArray[2].ToString().EvaluateCode(engine) : "";
			webElement.RelXPath = (SearchParametersDT.Rows[1].ItemArray[0].ToString().ToLower() == "true") ?
				(string)await SearchParametersDT.Rows[1].ItemArray[2].ToString().EvaluateCode(engine) : "";
			webElement.ID = (SearchParametersDT.Rows[2].ItemArray[0].ToString().ToLower() == "true") ?
				(string)await SearchParametersDT.Rows[2].ItemArray[2].ToString().EvaluateCode(engine) : "";
			webElement.Name = (SearchParametersDT.Rows[3].ItemArray[0].ToString().ToLower() == "true") ?
				(string)await SearchParametersDT.Rows[3].ItemArray[2].ToString().EvaluateCode(engine) : "";
			webElement.TagName = (SearchParametersDT.Rows[4].ItemArray[0].ToString().ToLower() == "true") ?
				(string)await SearchParametersDT.Rows[4].ItemArray[2].ToString().EvaluateCode(engine) : "";
			webElement.ClassName = (SearchParametersDT.Rows[5].ItemArray[0].ToString().ToLower() == "true") ?
				(string)await SearchParametersDT.Rows[5].ItemArray[2].ToString().EvaluateCode(engine) : "";
			webElement.LinkText = (SearchParametersDT.Rows[6].ItemArray[0].ToString().ToLower() == "true") ?
				(string)await SearchParametersDT.Rows[6].ItemArray[2].ToString().EvaluateCode(engine) : "";
			webElement.CssSelector = (SearchParametersDT.Rows[7].ItemArray[0].ToString().ToLower() == "true") ?
				(string)await SearchParametersDT.Rows[7].ItemArray[2].ToString().EvaluateCode(engine) : "";
			return webElement;
		}

		public static DataTable WebElementToDataTable(WebElement webElement)
		{
			DataTable SearchParameters = NewSearchParameterDataTable();
			SearchParameters.Rows.Add(true, "\"XPath\"", $"\"{webElement.XPath}\"");
			SearchParameters.Rows.Add(true, "\"Relative XPath\"", $"\"{webElement.RelXPath}\"");
			SearchParameters.Rows.Add(false, "\"ID\"", $"\"{webElement.ID}\"");
			SearchParameters.Rows.Add(false, "\"Name\"", $"\"{webElement.Name}\"");
			SearchParameters.Rows.Add(false, "\"Tag Name\"", $"\"{webElement.TagName}\"");
			SearchParameters.Rows.Add(false, "\"Class Name\"", $"\"{webElement.ClassName}\"");
			SearchParameters.Rows.Add(false, "\"Link Text\"", $"\"{webElement.LinkText}\"");
			SearchParameters.Rows.Add(false, "\"CSS Selector\"", $"\"{webElement.CssSelector}\"");
			return SearchParameters;
		}

		public static DataTable WebElementToSeleniumDataTable(WebElement webElement)
		{
			DataTable SearchParameters = NewSearchParameterDataTable();
			SearchParameters.Rows.Add(true, "\"XPath\"", $"\"{webElement.XPath}\"");
			SearchParameters.Rows.Add(true, "\"Relative XPath\"", $"\"{webElement.RelXPath}\"");
			SearchParameters.Rows.Add(false, "\"ID\"", $"\"{webElement.ID}\"");
			SearchParameters.Rows.Add(false, "\"Name\"", $"\"{webElement.Name}\"");
			SearchParameters.Rows.Add(false, "\"Tag Name\"", $"\"{webElement.TagName}\"");
			SearchParameters.Rows.Add(false, "\"Class Name\"", $"\"{webElement.ClassName}\"");
			SearchParameters.Rows.Add(false, "\"Link Text\"", $"\"{webElement.LinkText}\"");
			var _cssSelectors = webElement.CssSelectors.Split(',').ToList();
			for (int i = 0; i < _cssSelectors.Count; i++)
				SearchParameters.Rows.Add(false, $"\"CSS Selector {i + 1}\"", $"\"{_cssSelectors[i]}\"");
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
		public static void GetUIElement(object sender, EventArgs e, DataTable nativeSearchParameters, IfrmCommandEditor editor)
		{
			ChromeExtensionRegistryManager registryManager = new ChromeExtensionRegistryManager();
			bool isChromeNativeMessagingInstalled = registryManager.IsExtensionInstalled();
			if (isChromeNativeMessagingInstalled) 
			{
				try
				{
					User32Functions.BringChromeWindowToTop();

					string webElementStr;
					NativeRequest.ProcessRequest("getelement", "", out webElementStr);
					if (!string.IsNullOrEmpty(webElementStr))
					{
						WebElement webElement = JsonConvert.DeserializeObject<WebElement>(webElementStr);
						DataTable SearchParameters = WebElementToDataTable(webElement);

						if (SearchParameters != null)
						{
							nativeSearchParameters.Rows.Clear();

							foreach (DataRow rw in SearchParameters.Rows)
								nativeSearchParameters.ImportRow(rw);
						}
					}
				}
				catch (Exception ex)
				{
					// Throw Error in Message Box
					if(ex.Message.Contains("Pipe hasn't been connected yet."))
                    {
						var result = ((Form)editor).Invoke(new Action(() =>
						{
							editor.ShowMessage("Chrome Native Extension is not installed! \n Please visit: https://chrome.google.com/webstore/detail/openbots-web-automation/kkepankimcahnjamnimeijpplgjpmdpp/related", "MessageBox", DialogType.OkOnly, 10);
						}
						));
					}
                    else
                    {
						var result = ((Form)editor).Invoke(new Action(() =>
						{
							editor.ShowMessage(ex.Message, "MessageBox", DialogType.OkOnly, 10);
						}
						));
					}
				}
				finally
				{
					Process process = Process.GetCurrentProcess();
					User32Functions.ActivateWindow(process.MainWindowTitle);
				}
			}
            else
            {
				var result = ((Form)editor).Invoke(new Action(() =>
				{
					editor.ShowMessage("Chrome Native Extension is not installed! \n Please use Extension Manager to install Chrome Native Extension", "MessageBox", DialogType.OkOnly, 10);
				}
				));
			}
		}

		public static CommandItemControl NativeChromeRecorderControl(DataTable NativeSearchParameters, IfrmCommandEditor editor)
        {
			CommandItemControl helperControl = new CommandItemControl("ChromeRecorder", Resources.command_camera, "Chrome Element Recorder");
			helperControl.Click += new EventHandler((s, e) => GetUIElement(s, e, NativeSearchParameters, editor));
			return helperControl;
		}

		public static DataTable CreateSearchParametersDT()
        {
			DataTable searchParamtersDT = new DataTable();
			searchParamtersDT.Columns.Add("Enabled");
			searchParamtersDT.Columns.Add("Parameter Name");
			searchParamtersDT.Columns.Add("Parameter Value");
			searchParamtersDT.TableName = DateTime.Now.ToString("SearchParametersDT" + DateTime.Now.ToString("MMddyy.hhmmss"));
			return searchParamtersDT;
		}

		public static void AddDefaultSearchRows(DataTable searchParametersDT)
        {
			if (searchParametersDT.Rows.Count == 0)
			{
				searchParametersDT.Rows.Add(false, "\"XPath\"", "");
				searchParametersDT.Rows.Add(false, "\"Relative XPath\"", "");
				searchParametersDT.Rows.Add(false, "\"ID\"", "");
				searchParametersDT.Rows.Add(false, "\"Name\"", "");
				searchParametersDT.Rows.Add(false, "\"Tag Name\"", "");
				searchParametersDT.Rows.Add(false, "\"Class Name\"", "");
				searchParametersDT.Rows.Add(false, "\"Link Text\"", "");
				searchParametersDT.Rows.Add(true, "\"CSS Selector\"", "");
			}
		}

		public static string GetSearchNameValue(DataTable searchParametersDT)
        {
			string searchParameterName = (from rw in searchParametersDT.AsEnumerable()
										  where rw.Field<string>("Enabled") == "True"
										  select rw.Field<string>("Parameter Name")).FirstOrDefault();

			string searchParameterValue = (from rw in searchParametersDT.AsEnumerable()
										   where rw.Field<string>("Enabled") == "True"
										   select rw.Field<string>("Parameter Value")).FirstOrDefault();

			return $"{searchParameterName} = {searchParameterValue}";
		}
	}
}
