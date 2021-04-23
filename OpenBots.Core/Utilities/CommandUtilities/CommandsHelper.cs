using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.User32;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Core.Utilities.FormsUtilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace OpenBots.Core.Utilities.CommandUtilities
{
    public static class CommandsHelper
    {
        public static AutomationCommand ConvertToAutomationCommand(Type commandClass)
        {
            var groupingAttribute = commandClass.GetCustomAttributes(typeof(CategoryAttribute), true);
            string groupAttribute = "";
            if (groupingAttribute.Length > 0)
            {
                var attributeFound = (CategoryAttribute)groupingAttribute[0];
                groupAttribute = attributeFound.Category;
            }

            //Instantiate Class
            ScriptCommand newCommand = (ScriptCommand)Activator.CreateInstance(commandClass);

            AutomationCommand newAutomationCommand = null;
            //If command is enabled, pull for display and configuration
            if (newCommand.CommandEnabled)
            {
                newAutomationCommand = new AutomationCommand();
                newAutomationCommand.CommandClass = commandClass;
                newAutomationCommand.Command = newCommand;
                newAutomationCommand.DisplayGroup = groupAttribute;
                newAutomationCommand.FullName = string.Join(" - ", groupAttribute, newCommand.SelectionName);
                newAutomationCommand.ShortName = newCommand.SelectionName;
                newAutomationCommand.Description = GetDescription(commandClass);
				newAutomationCommand.CommandIcon = newCommand.CommandIcon;

                //if (userPrefs.ClientSettings.PreloadBuilderCommands)
                //{
                //    //newAutomationCommand.RenderUIComponents();
                //}
            }

            return newAutomationCommand;
        }
        private static string GetDescription(Type type)
        {
            var descriptions = (DescriptionAttribute[])type.GetCustomAttributes(typeof(DescriptionAttribute), true);

            if (descriptions.Length == 0)
                return string.Empty;
            return descriptions[0].Description;
        }

        public async static Task<AutomationElement> SearchForGUIElement(IAutomationEngineInstance engine, DataTable uiaSearchParams, string variableWindowName)
        {
            User32Functions.ActivateWindow(variableWindowName);
            //create search params
            var searchParams = from rw in uiaSearchParams.AsEnumerable()
                               where rw.Field<string>("Enabled") == "True"
                               select rw;

            //create and populate condition list
            var conditionList = new List<Condition>();
            foreach (var param in searchParams)
            {
                var parameterName = (string)param["Parameter Name"];
                var parameterValueString = (string)param["Parameter Value"];

                dynamic parameterValue = await parameterValueString.EvaluateCode(engine);

                PropertyCondition propCondition;
                if (bool.TryParse(parameterValue, out bool bValue))
                    propCondition = CreatePropertyCondition(parameterName, bValue);
                else
                    propCondition = CreatePropertyCondition(parameterName, parameterValue);

                conditionList.Add(propCondition);
            }

            //concatenate or take first condition
            Condition searchConditions;
            if (conditionList.Count > 1)
                searchConditions = new AndCondition(conditionList.ToArray());
            else
                searchConditions = conditionList[0];

            //find window
            var windowElement = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, variableWindowName));

            //if window was not found
            if (windowElement == null)
                throw new Exception("Window named '" + variableWindowName + "' was not found!");

            //find required handle based on specified conditions
            var element = windowElement.FindFirst(TreeScope.Descendants, searchConditions);
            return element;
        }
        private static PropertyCondition CreatePropertyCondition(string propertyName, object propertyValue)
        {
            switch (propertyName)
            {
                case "AcceleratorKey":
                    return new PropertyCondition(AutomationElement.AcceleratorKeyProperty, propertyValue);
                case "AccessKey":
                    return new PropertyCondition(AutomationElement.AccessKeyProperty, propertyValue);
                case "AutomationId":
                    return new PropertyCondition(AutomationElement.AutomationIdProperty, propertyValue);
                case "ClassName":
                    return new PropertyCondition(AutomationElement.ClassNameProperty, propertyValue);
                case "FrameworkId":
                    return new PropertyCondition(AutomationElement.FrameworkIdProperty, propertyValue);
                case "HasKeyboardFocus":
                    return new PropertyCondition(AutomationElement.HasKeyboardFocusProperty, propertyValue);
                case "HelpText":
                    return new PropertyCondition(AutomationElement.HelpTextProperty, propertyValue);
                case "IsContentElement":
                    return new PropertyCondition(AutomationElement.IsContentElementProperty, propertyValue);
                case "IsControlElement":
                    return new PropertyCondition(AutomationElement.IsControlElementProperty, propertyValue);
                case "IsEnabled":
                    return new PropertyCondition(AutomationElement.IsEnabledProperty, propertyValue);
                case "IsKeyboardFocusable":
                    return new PropertyCondition(AutomationElement.IsKeyboardFocusableProperty, propertyValue);
                case "IsOffscreen":
                    return new PropertyCondition(AutomationElement.IsOffscreenProperty, propertyValue);
                case "IsPassword":
                    return new PropertyCondition(AutomationElement.IsPasswordProperty, propertyValue);
                case "IsRequiredForForm":
                    return new PropertyCondition(AutomationElement.IsRequiredForFormProperty, propertyValue);
                case "ItemStatus":
                    return new PropertyCondition(AutomationElement.ItemStatusProperty, propertyValue);
                case "ItemType":
                    return new PropertyCondition(AutomationElement.ItemTypeProperty, propertyValue);
                case "LocalizedControlType":
                    return new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, propertyValue);
                case "Name":
                    return new PropertyCondition(AutomationElement.NameProperty, propertyValue);
                case "NativeWindowHandle":
                    return new PropertyCondition(AutomationElement.NativeWindowHandleProperty, propertyValue);
                case "ProcessID":
                    return new PropertyCondition(AutomationElement.ProcessIdProperty, propertyValue);
                default:
                    throw new NotImplementedException("Property Type '" + propertyName + "' not implemented");
            }
        }

        public async static Task<bool> ElementExists(IAutomationEngineInstance engine, string instanceName, string searchMethod, string parameterName, 
            string searchOption, int timeout)
        {
            //get engine reference
            List<string[]> seleniumSearchParamRows = new List<string[]>();
            seleniumSearchParamRows.Add(new string[]
            {
                string.Empty, searchMethod, parameterName
            });

            //get stored app object
            var browserObject = instanceName.GetAppInstance(engine);

            //get selenium instance driver
            var seleniumInstance = (ChromeDriver)browserObject;

            try
            {
                //search for element
                var element = await FindElement(engine, seleniumInstance, seleniumSearchParamRows, searchOption, timeout);

                //element exists
                return true;
            }
            catch (Exception)
            {
                //element does not exist
                return false;
            }
        }

        public async static Task<object> FindElement(IAutomationEngineInstance engine, IWebDriver seleniumInstance, List<string[]> searchParameterRows, 
            string searchOption, int timeout)
        {
            var wait = new WebDriverWait(seleniumInstance, new TimeSpan(0, 0, timeout));
            object element;

            List<By> byList = new List<By>();
            By by;

            foreach (var row in searchParameterRows)
            {
                string parameter = (string)await row[2].ToString().EvaluateCode(engine);
                switch (row[1].ToString())
                {
                    case string a when a.ToLower().Contains("xpath"):
                        by = By.XPath(parameter);
                        break;

                    case string a when a.ToLower().Contains("id"):
                        by = By.Id(parameter);
                        break;

                    case string a when a.ToLower().Contains("tag name"):
                        by = By.TagName(parameter);
                        break;

                    case string a when a.ToLower().Contains("class name"):
                        by = By.ClassName(parameter);
                        break;

                    case string a when a.ToLower().Contains("name"):
                        by = By.Name(parameter);
                        break;

                    case string a when a.ToLower().Contains("css selector"):
                        by = By.CssSelector(parameter);
                        break;

                    case string a when a.ToLower().Contains("link text"):
                        by = By.LinkText(parameter);
                        break;

                    default:
                        throw new Exception("Element Search Type was not found: " + row[1].ToString());
                }
                byList.Add(by);
            }

            var byall = new ByAll(byList.ToArray());
            bool elementFound;

            if (searchOption == "Find Element")
            {
                try
                {
                    elementFound = wait.Until(condition =>
                    {
                        try
                        {
							if (engine.IsCancellationPending)
								throw new Exception("Element search cancelled");
                            var elementToBeDisplayed = seleniumInstance.FindElement(byall);
                            return elementToBeDisplayed.Displayed;
                        }
                        catch (StaleElementReferenceException)
                        {
                            return false;
                        }
                        catch (NoSuchElementException)
                        {
                            return false;
                        }
                    });
                }
                catch (Exception)
                {
					//element not found during wait period
					return null;
                }

                element = seleniumInstance.FindElement(byall);
            }
            else
            {
                try
                {
                    elementFound = wait.Until(condition =>
                    {
                        try
                        {
                            var elementsToBeDisplayed = seleniumInstance.FindElements(byall);
                            return elementsToBeDisplayed.First().Displayed;
                        }
                        catch (StaleElementReferenceException)
                        {
                            return false;
                        }
                        catch (NoSuchElementException)
                        {
                            return false;
                        }
                    });
                }
                catch (Exception)
                {
					//elements not found during wait period
					return null;
                }

                element = seleniumInstance.FindElements(byall);
            }

            return element;
        }

		public async static Task<bool> DetermineStatementTruth(IAutomationEngineInstance engine, string ifActionType, DataTable IfActionParameterTable)
		{
			bool ifResult = false;

			if (ifActionType == "Number Compare")
			{
				string value1 = ((from rw in IfActionParameterTable.AsEnumerable()
								  where rw.Field<string>("Parameter Name") == "Number1"
								  select rw.Field<string>("Parameter Value")).FirstOrDefault());
				string operand = ((from rw in IfActionParameterTable.AsEnumerable()
								   where rw.Field<string>("Parameter Name") == "Operand"
								   select rw.Field<string>("Parameter Value")).FirstOrDefault());
				string value2 = ((from rw in IfActionParameterTable.AsEnumerable()
								  where rw.Field<string>("Parameter Name") == "Number2"
								  select rw.Field<string>("Parameter Value")).FirstOrDefault());

				var cdecValue1 = Convert.ToDecimal(await value1.EvaluateCode(engine));
				var cdecValue2 = Convert.ToDecimal(await value2.EvaluateCode(engine));

				switch (operand)
				{
					case "is equal to":
						ifResult = cdecValue1 == cdecValue2;
						break;

					case "is not equal to":
						ifResult = cdecValue1 != cdecValue2;
						break;

					case "is greater than":
						ifResult = cdecValue1 > cdecValue2;
						break;

					case "is greater than or equal to":
						ifResult = cdecValue1 >= cdecValue2;
						break;

					case "is less than":
						ifResult = cdecValue1 < cdecValue2;
						break;

					case "is less than or equal to":
						ifResult = cdecValue1 <= cdecValue2;
						break;
				}
			}
			else if (ifActionType == "Date Compare")
			{
				string value1 = ((from rw in IfActionParameterTable.AsEnumerable()
								  where rw.Field<string>("Parameter Name") == "Date1"
								  select rw.Field<string>("Parameter Value")).FirstOrDefault());
				string operand = ((from rw in IfActionParameterTable.AsEnumerable()
								   where rw.Field<string>("Parameter Name") == "Operand"
								   select rw.Field<string>("Parameter Value")).FirstOrDefault());
				string value2 = ((from rw in IfActionParameterTable.AsEnumerable()
								  where rw.Field<string>("Parameter Name") == "Date2"
								  select rw.Field<string>("Parameter Value")).FirstOrDefault());

				var dt1 = (DateTime)await value1.EvaluateCode(engine);
				var dt2 = (DateTime)await value2.EvaluateCode(engine);

				switch (operand)
				{
					case "is equal to":
						ifResult = dt1 == dt2;
						break;

					case "is not equal to":
						ifResult = dt1 != dt2;
						break;

					case "is greater than":
						ifResult = dt1 > dt2;
						break;

					case "is greater than or equal to":
						ifResult = dt1 >= dt2;
						break;

					case "is less than":
						ifResult = dt1 < dt2;
						break;

					case "is less than or equal to":
						ifResult = dt1 <= dt2;
						break;
				}
			}
			else if (ifActionType == "Text Compare")
			{
				string value1 = ((from rw in IfActionParameterTable.AsEnumerable()
								  where rw.Field<string>("Parameter Name") == "Text1"
								  select rw.Field<string>("Parameter Value")).FirstOrDefault());
				string operand = ((from rw in IfActionParameterTable.AsEnumerable()
								   where rw.Field<string>("Parameter Name") == "Operand"
								   select rw.Field<string>("Parameter Value")).FirstOrDefault());
				string value2 = ((from rw in IfActionParameterTable.AsEnumerable()
								  where rw.Field<string>("Parameter Name") == "Text2"
								  select rw.Field<string>("Parameter Value")).FirstOrDefault());

				string caseSensitive = ((from rw in IfActionParameterTable.AsEnumerable()
										 where rw.Field<string>("Parameter Name") == "Case Sensitive"
										 select rw.Field<string>("Parameter Value")).FirstOrDefault());

				value1 = (string)await value1.EvaluateCode(engine);
				value2 = (string)await value2.EvaluateCode(engine);

				if (caseSensitive == "No")
				{
					value1 = value1.ToUpper();
					value2 = value2.ToUpper();
				}

				switch (operand)
				{
					case "contains":
						ifResult = value1.Contains(value2);
						break;

					case "does not contain":
						ifResult = !value1.Contains(value2);
						break;

					case "is equal to":
						ifResult = value1 == value2;
						break;

					case "is not equal to":
						ifResult = value1 != value2;
						break;
				}
			}
			else if (ifActionType == "Has Value")
			{
				string variableName = ((from rw in IfActionParameterTable.AsEnumerable()
										where rw.Field<string>("Parameter Name") == "Variable Name"
										select rw.Field<string>("Parameter Value")).FirstOrDefault());

				var actualVariable = variableName.EvaluateCode(engine);

				if (actualVariable != null)
					ifResult = true;
				else
					ifResult = false;
			}
			else if (ifActionType == "Is Numeric")
			{
				string variableName = ((from rw in IfActionParameterTable.AsEnumerable()
										where rw.Field<string>("Parameter Name") == "Variable Name"
										select rw.Field<string>("Parameter Value")).FirstOrDefault());

				ifResult = decimal.TryParse((await variableName.EvaluateCode(engine)).ToString(), out decimal decimalResult);
			}
			else if (ifActionType == "Error Occured")
			{
				//get line number
				string userLineNumber = ((from rw in IfActionParameterTable.AsEnumerable()
										  where rw.Field<string>("Parameter Name") == "Line Number"
										  select rw.Field<string>("Parameter Value")).FirstOrDefault());

				//convert to int
				int lineNumber = (int)await userLineNumber.EvaluateCode(engine);

				//determine if error happened
				if (engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).Count() > 0)
				{

					var error = engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).FirstOrDefault();
					error.ErrorMessage.SetVariableValue(engine, "Error.Message");
					error.LineNumber.ToString().SetVariableValue(engine, "Error.Line");
					error.StackTrace.SetVariableValue(engine, "Error.StackTrace");

					ifResult = true;
				}
				else
					ifResult = false;
			}
			else if (ifActionType == "Error Did Not Occur")
			{
				//get line number
				string userLineNumber = ((from rw in IfActionParameterTable.AsEnumerable()
										  where rw.Field<string>("Parameter Name") == "Line Number"
										  select rw.Field<string>("Parameter Value")).FirstOrDefault());
				//convert to int
				int lineNumber = (int)await userLineNumber.EvaluateCode(engine);

				//determine if error happened
				if (engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).Count() == 0)
					ifResult = true;
				else
				{
					var error = engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).FirstOrDefault();
					error.ErrorMessage.SetVariableValue(engine, "Error.Message");
					error.LineNumber.ToString().SetVariableValue(engine, "Error.Line");
					error.StackTrace.SetVariableValue(engine, "Error.StackTrace");

					ifResult = false;
				}
			}
			else if (ifActionType == "Window Name Exists")
			{
				//get user supplied name
				string windowName = ((from rw in IfActionParameterTable.AsEnumerable()
									  where rw.Field<string>("Parameter Name") == "Window Name"
									  select rw.Field<string>("Parameter Value")).FirstOrDefault());
				//variable translation
				string variablizedWindowName = (string)await windowName.EvaluateCode(engine);

				//search for window
				IntPtr windowPtr = User32Functions.FindWindow(variablizedWindowName);

				//conditional
				if (windowPtr != IntPtr.Zero)
					ifResult = true;
			}
			else if (ifActionType == "Active Window Name Is")
			{
				string windowName = ((from rw in IfActionParameterTable.AsEnumerable()
									  where rw.Field<string>("Parameter Name") == "Window Name"
									  select rw.Field<string>("Parameter Value")).FirstOrDefault());

				string variablizedWindowName = (string)await windowName.EvaluateCode(engine);

				var currentWindowTitle = User32Functions.GetActiveWindowTitle();

				if (currentWindowTitle == variablizedWindowName)
					ifResult = true;
			}
			else if (ifActionType == "File Exists")
			{

				string fileName = ((from rw in IfActionParameterTable.AsEnumerable()
									where rw.Field<string>("Parameter Name") == "File Path"
									select rw.Field<string>("Parameter Value")).FirstOrDefault());

				string trueWhenFileExists = ((from rw in IfActionParameterTable.AsEnumerable()
											  where rw.Field<string>("Parameter Name") == "True When"
											  select rw.Field<string>("Parameter Value")).FirstOrDefault());

				var userFileSelected = (string)await fileName.EvaluateCode(engine);

				bool existCheck = false;
				if (trueWhenFileExists == "It Does Exist")
					existCheck = true;

				if (File.Exists(userFileSelected) == existCheck)
					ifResult = true;
			}
			else if (ifActionType == "Folder Exists")
			{
				string folderName = ((from rw in IfActionParameterTable.AsEnumerable()
									  where rw.Field<string>("Parameter Name") == "Folder Path"
									  select rw.Field<string>("Parameter Value")).FirstOrDefault());

				string trueWhenFileExists = ((from rw in IfActionParameterTable.AsEnumerable()
											  where rw.Field<string>("Parameter Name") == "True When"
											  select rw.Field<string>("Parameter Value")).FirstOrDefault());

				var userFolderSelected = (string)await folderName.EvaluateCode(engine);

				bool existCheck = false;
				if (trueWhenFileExists == "It Does Exist")
					existCheck = true;

				if (Directory.Exists(userFolderSelected) == existCheck)
					ifResult = true;
			}
			else if (ifActionType == "Web Element Exists")
			{
				string instanceName = ((from rw in IfActionParameterTable.AsEnumerable()
										where rw.Field<string>("Parameter Name") == "Selenium Instance Name"
										select rw.Field<string>("Parameter Value")).FirstOrDefault());

				string parameterName = ((from rw in IfActionParameterTable.AsEnumerable()
										 where rw.Field<string>("Parameter Name") == "Element Search Parameter"
										 select rw.Field<string>("Parameter Value")).FirstOrDefault());

				string searchMethod = ((from rw in IfActionParameterTable.AsEnumerable()
										where rw.Field<string>("Parameter Name") == "Element Search Method"
										select rw.Field<string>("Parameter Value")).FirstOrDefault());

				string timeout = ((from rw in IfActionParameterTable.AsEnumerable()
								   where rw.Field<string>("Parameter Name") == "Timeout (Seconds)"
								   select rw.Field<string>("Parameter Value")).FirstOrDefault());

				string trueWhenElementExists = (from rw in IfActionParameterTable.AsEnumerable()
												where rw.Field<string>("Parameter Name") == "True When"
												select rw.Field<string>("Parameter Value")).FirstOrDefault();

				bool elementExists = await ElementExists(engine, instanceName, searchMethod, parameterName, "Find Element", int.Parse(timeout));
				ifResult = elementExists;

				if (trueWhenElementExists == "It Does Not Exist")
					ifResult = !ifResult;
			}
			else if (ifActionType == "GUI Element Exists")
			{
				string windowName = (from rw in IfActionParameterTable.AsEnumerable()
									  where rw.Field<string>("Parameter Name") == "Window Name"
									  select rw.Field<string>("Parameter Value")).FirstOrDefault();
				windowName = (string)await windowName.EvaluateCode(engine);

				string elementSearchParam = (from rw in IfActionParameterTable.AsEnumerable()
											  where rw.Field<string>("Parameter Name") == "Element Search Parameter"
											  select rw.Field<string>("Parameter Value")).FirstOrDefault();
				elementSearchParam = (string)await elementSearchParam.EvaluateCode(engine);

				string elementSearchMethod = (from rw in IfActionParameterTable.AsEnumerable()
											   where rw.Field<string>("Parameter Name") == "Element Search Method"
											   select rw.Field<string>("Parameter Value")).FirstOrDefault();
				elementSearchMethod = (string)await elementSearchMethod.EvaluateCode(engine);

				string trueWhenElementExists = (from rw in IfActionParameterTable.AsEnumerable()
												where rw.Field<string>("Parameter Name") == "True When"
												select rw.Field<string>("Parameter Value")).FirstOrDefault();

				string timeoutString = (from rw in IfActionParameterTable.AsEnumerable()
								  where rw.Field<string>("Parameter Name") == "Timeout (Seconds)"
								  select rw.Field<string>("Parameter Value")).FirstOrDefault();

				//set up search parameter table
				var uiASearchParameters = new DataTable();
				uiASearchParameters.Columns.Add("Enabled");
				uiASearchParameters.Columns.Add("Parameter Name");
				uiASearchParameters.Columns.Add("Parameter Value");
				uiASearchParameters.Rows.Add(true, elementSearchMethod, elementSearchParam);

				int vTimeout = (int)await timeoutString.EvaluateCode(engine);
				AutomationElement handle = null;
				var timeToEnd = DateTime.Now.AddSeconds(vTimeout);
				while (timeToEnd >= DateTime.Now)
				{
					try
					{
						handle = await SearchForGUIElement(engine, uiASearchParameters, windowName);
						break;
					}
					catch (Exception)
					{
						engine.ReportProgress("Element Not Yet Found... " + (timeToEnd - DateTime.Now).Seconds + "s remain");
						Thread.Sleep(500);
					}
				}

				if (handle is null)
					ifResult = false;
				else
					ifResult = true;

				if (trueWhenElementExists == "It Does Not Exist")
					ifResult = !ifResult;
			}
			else if (ifActionType == "Image Element Exists")
			{
				string imageName = (from rw in IfActionParameterTable.AsEnumerable()
									where rw.Field<string>("Parameter Name") == "Captured Image Variable"
									select rw.Field<string>("Parameter Value")).FirstOrDefault();
				string accuracyString;
				double accuracy;
				try
				{
					accuracyString = (from rw in IfActionParameterTable.AsEnumerable()
											 where rw.Field<string>("Parameter Name") == "Accuracy (0-1)"
											 select rw.Field<string>("Parameter Value")).FirstOrDefault();
					accuracy = (double)await accuracyString.EvaluateCode(engine);
					if (accuracy > 1 || accuracy < 0)
						throw new ArgumentOutOfRangeException("Accuracy value is out of range (0-1)");
				}
				catch (Exception)
				{
					throw new InvalidDataException("Accuracy value is invalid");
				}

				string trueWhenImageExists = (from rw in IfActionParameterTable.AsEnumerable()
											  where rw.Field<string>("Parameter Name") == "True When"
											  select rw.Field<string>("Parameter Value")).FirstOrDefault();

				var capturedImage = (Bitmap)await imageName.EvaluateCode(engine);
				string timeoutString = (from rw in IfActionParameterTable.AsEnumerable()
										 where rw.Field<string>("Parameter Name") == "Timeout (Seconds)"
										 select rw.Field<string>("Parameter Value")).FirstOrDefault();
				int timeout = (int)await timeoutString.EvaluateCode(engine);

				var element = FindImageElement(capturedImage, accuracy, engine, DateTime.Now.AddSeconds(timeout));
				FormsHelper.ShowAllForms(engine.AutomationEngineContext.IsDebugMode);

				if (element != null)
					ifResult = true;
				else
					ifResult = false;

				if (trueWhenImageExists == "It Does Not Exist")
					ifResult = !ifResult;
			}
			else if (ifActionType == "App Instance Exists")
			{
				string instanceName = (from rw in IfActionParameterTable.AsEnumerable()
									   where rw.Field<string>("Parameter Name") == "Instance Name"
									   select rw.Field<string>("Parameter Value")).FirstOrDefault();


				string trueWhenImageExists = (from rw in IfActionParameterTable.AsEnumerable()
											  where rw.Field<string>("Parameter Name") == "True When"
											  select rw.Field<string>("Parameter Value")).FirstOrDefault();

				ifResult = instanceName.InstanceExists(engine);

				if (trueWhenImageExists == "It Does Not Exist")
					ifResult = !ifResult;
			}
			else
				throw new Exception("If type not recognized!");

			return ifResult;
		}

		public static ImageElement FindImageElement(Bitmap smallBmp, double accuracy, IAutomationEngineInstance engine, DateTime timeToEnd, bool isCaptureTest = false)
		{
			FormsHelper.HideAllForms();

			var lastRecordedTime = DateTime.Now;
			dynamic element = null;
			double tolerance = 1.0 - accuracy;

			Bitmap bigBmp = ImageMethods.Screenshot();

			Bitmap smallTestBmp = new Bitmap(smallBmp);

			Bitmap bigTestBmp = new Bitmap(bigBmp);
			Graphics bigTestGraphics = Graphics.FromImage(bigTestBmp);

			BitmapData smallData =
			  smallBmp.LockBits(new Rectangle(0, 0, smallBmp.Width, smallBmp.Height),
					   ImageLockMode.ReadOnly,
					   PixelFormat.Format24bppRgb);
			BitmapData bigData =
			  bigBmp.LockBits(new Rectangle(0, 0, bigBmp.Width, bigBmp.Height),
					   ImageLockMode.ReadOnly,
					   PixelFormat.Format24bppRgb);

			int smallStride = smallData.Stride;
			int bigStride = bigData.Stride;

			int bigWidth = bigBmp.Width;
			int bigHeight = bigBmp.Height - smallBmp.Height + 1;
			int smallWidth = smallBmp.Width * 3;
			int smallHeight = smallBmp.Height;

			int margin = Convert.ToInt32(255.0 * tolerance);

			unsafe
			{
				byte* pSmall = (byte*)(void*)smallData.Scan0;
				byte* pBig = (byte*)(void*)bigData.Scan0;

				int smallOffset = smallStride - smallBmp.Width * 3;
				int bigOffset = bigStride - bigBmp.Width * 3;

				bool matchFound = true;

				for (int y = 0; y < bigHeight; y++)
				{
					if (engine != null && engine.IsCancellationPending)
						break;

					if (engine != null && lastRecordedTime.Second != DateTime.Now.Second)
                    {
						engine.ReportProgress("Element Not Yet Found... " + (timeToEnd - DateTime.Now).Seconds + "s remain");
						lastRecordedTime = DateTime.Now;
					}

					if (timeToEnd <= DateTime.Now)
						break;

					for (int x = 0; x < bigWidth; x++)
					{
						byte* pBigBackup = pBig;
						byte* pSmallBackup = pSmall;

						//Look for the small picture.
						for (int i = 0; i < smallHeight; i++)
						{
							int j = 0;
							matchFound = true;
							for (j = 0; j < smallWidth; j++)
							{
								//With tolerance: pSmall value should be between margins.
								int inf = pBig[0] - margin;
								int sup = pBig[0] + margin;
								if (sup < pSmall[0] || inf > pSmall[0])
								{
									matchFound = false;
									break;
								}

								pBig++;
								pSmall++;
							}

							if (!matchFound)
								break;

							//We restore the pointers.
							pSmall = pSmallBackup;
							pBig = pBigBackup;

							//Next rows of the small and big pictures.
							pSmall += smallStride * (1 + i);
							pBig += bigStride * (1 + i);
						}

						//If match found, we return.
						if (matchFound)
						{
							element = new ImageElement
							{
								LeftX = x,
								MiddleX = x + smallBmp.Width / 2,
								RightX = x + smallBmp.Width,
								TopY = y,
								MiddleY = y + smallBmp.Height / 2,
								BottomY = y + smallBmp.Height
							};

							if (isCaptureTest)
                            {
								element.SmallTestImage = smallTestBmp;
								element.BigTestImage = bigTestBmp;
							}

							break;
						}
						//If no match found, we restore the pointers and continue.
						else
						{
							pBig = pBigBackup;
							pSmall = pSmallBackup;
							pBig += 3;
						}
					}

					if (matchFound)
						break;

					pBig += bigOffset;
				}
			}

			bigBmp.UnlockBits(bigData);
			smallBmp.UnlockBits(smallData);
			bigTestGraphics.Dispose();
			return element;
		}
	}
}
