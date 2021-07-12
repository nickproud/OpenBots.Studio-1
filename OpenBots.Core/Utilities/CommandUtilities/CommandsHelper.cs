using OpenBots.Core.Command;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Model.ApplicationModel;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.User32;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Core.Utilities.FormsUtilities;
using OpenQA.Selenium;
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

			newCommand.CommandIcon = null;
			GC.Collect();

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
            }

            return newAutomationCommand;
        }

		public static string GetDescription(Type type)
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

        public async static Task<bool> ElementExists(IAutomationEngineInstance engine, OBAppInstance instance, string searchMethod, string parameterName, 
            string searchOption, int timeout)
        {
            //get engine reference
            List<string[]> seleniumSearchParamRows = new List<string[]>();
            seleniumSearchParamRows.Add(new string[]
            {
                string.Empty, $"\"{searchMethod}\"", parameterName
            });

            //get selenium instance driver
            var seleniumInstance = (IWebDriver)instance.Value;

            try
            {
                //search for element
                var element = await FindElement(engine, seleniumInstance, seleniumSearchParamRows, searchOption, timeout);

				//element exists
				if (element == null)
					return false;
				else
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
				string parameterName = (string)await row[1].ToString().EvaluateCode(engine);

				switch (parameterName)
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

		public async static Task<bool> DetermineStatementTruth(IAutomationEngineInstance engine, string ifActionType, DataTable IfActionParameterTable, string condition = "false")
		{
			bool ifResult = false;
			if (ifActionType == null)
				ifResult = (bool)await condition.EvaluateCode(engine);
			else
			{
				switch (ifActionType)
				{
					case "Number Compare":				
						string num1 = ((from rw in IfActionParameterTable.AsEnumerable()
										  where rw.Field<string>("Parameter Name") == "Number1"
										  select rw.Field<string>("Parameter Value")).FirstOrDefault());
						string numOperand = ((from rw in IfActionParameterTable.AsEnumerable()
										   where rw.Field<string>("Parameter Name") == "Operand"
										   select rw.Field<string>("Parameter Value")).FirstOrDefault());
						string num2 = ((from rw in IfActionParameterTable.AsEnumerable()
										  where rw.Field<string>("Parameter Name") == "Number2"
										  select rw.Field<string>("Parameter Value")).FirstOrDefault());

						var cdecValue1 = Convert.ToDecimal(await num1.EvaluateCode(engine));
						var cdecValue2 = Convert.ToDecimal(await num2.EvaluateCode(engine));

						switch (numOperand)
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
						break;
					case "Date Compare":			
						string date1 = ((from rw in IfActionParameterTable.AsEnumerable()
										  where rw.Field<string>("Parameter Name") == "Date1"
										  select rw.Field<string>("Parameter Value")).FirstOrDefault());
						string dateOperand = ((from rw in IfActionParameterTable.AsEnumerable()
										   where rw.Field<string>("Parameter Name") == "Operand"
										   select rw.Field<string>("Parameter Value")).FirstOrDefault());
						string date2 = ((from rw in IfActionParameterTable.AsEnumerable()
										  where rw.Field<string>("Parameter Name") == "Date2"
										  select rw.Field<string>("Parameter Value")).FirstOrDefault());

						var dt1 = (DateTime)await date1.EvaluateCode(engine);
						var dt2 = (DateTime)await date2.EvaluateCode(engine);

						switch (dateOperand)
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
						break;
					case "Text Compare":				
						string text1 = ((from rw in IfActionParameterTable.AsEnumerable()
										  where rw.Field<string>("Parameter Name") == "Text1"
										  select rw.Field<string>("Parameter Value")).FirstOrDefault());
						string textOperand = ((from rw in IfActionParameterTable.AsEnumerable()
										   where rw.Field<string>("Parameter Name") == "Operand"
										   select rw.Field<string>("Parameter Value")).FirstOrDefault());
						string text2 = ((from rw in IfActionParameterTable.AsEnumerable()
										  where rw.Field<string>("Parameter Name") == "Text2"
										  select rw.Field<string>("Parameter Value")).FirstOrDefault());

						string caseSensitive = ((from rw in IfActionParameterTable.AsEnumerable()
												 where rw.Field<string>("Parameter Name") == "Case Sensitive"
												 select rw.Field<string>("Parameter Value")).FirstOrDefault());

						text1 = (string)await text1.EvaluateCode(engine);
						text2 = (string)await text2.EvaluateCode(engine);

						if (caseSensitive == "No")
						{
							text1 = text1.ToUpper();
							text2 = text2.ToUpper();
						}

						switch (textOperand)
						{
							case "contains":
								ifResult = text1.Contains(text2);
								break;

							case "does not contain":
								ifResult = !text1.Contains(text2);
								break;

							case "is equal to":
								ifResult = text1 == text2;
								break;

							case "is not equal to":
								ifResult = text1 != text2;
								break;
						}
						break;
					case "Has Value":				
						string variableName = ((from rw in IfActionParameterTable.AsEnumerable()
												where rw.Field<string>("Parameter Name") == "Variable Name"
												select rw.Field<string>("Parameter Value")).FirstOrDefault());

						dynamic actualVariable = variableName.EvaluateCode(engine);

						if (actualVariable.Result != null)
							ifResult = true;
						else
							ifResult = false;
						break;
					case "Is Numeric":				
						string isNumericVariableName = ((from rw in IfActionParameterTable.AsEnumerable()
												where rw.Field<string>("Parameter Name") == "Variable Name"
												select rw.Field<string>("Parameter Value")).FirstOrDefault());

						ifResult = decimal.TryParse((await isNumericVariableName.EvaluateCode(engine)).ToString(), out decimal decimalResult);
						break;
					case "Error Occured":			
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
						break;
					case "Error Did Not Occur":				
						//get line number
						string userLineNumber2 = ((from rw in IfActionParameterTable.AsEnumerable()
												  where rw.Field<string>("Parameter Name") == "Line Number"
												  select rw.Field<string>("Parameter Value")).FirstOrDefault());
						//convert to int
						int lineNumber2 = (int)await userLineNumber2.EvaluateCode(engine);

						//determine if error happened
						if (engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber2).Count() == 0)
							ifResult = true;
						else
						{
							var error = engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber2).FirstOrDefault();
							error.ErrorMessage.SetVariableValue(engine, "Error.Message");
							error.LineNumber.ToString().SetVariableValue(engine, "Error.Line");
							error.StackTrace.SetVariableValue(engine, "Error.StackTrace");

							ifResult = false;
						}
						break;
					case "Window Name Exists":				
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
						break;
					case "Active Window Name Is":			
						string activeWindowName = ((from rw in IfActionParameterTable.AsEnumerable()
											  where rw.Field<string>("Parameter Name") == "Window Name"
											  select rw.Field<string>("Parameter Value")).FirstOrDefault());

						string variablizedActiveWindowName = (string)await activeWindowName.EvaluateCode(engine);

						var currentWindowTitle = User32Functions.GetActiveWindowTitle();

						if (currentWindowTitle == variablizedActiveWindowName)
							ifResult = true;
						break;
					case "File Exists":
						string fileName = ((from rw in IfActionParameterTable.AsEnumerable()
											where rw.Field<string>("Parameter Name") == "File Path"
											select rw.Field<string>("Parameter Value")).FirstOrDefault());

						string trueWhenFileExists = ((from rw in IfActionParameterTable.AsEnumerable()
													  where rw.Field<string>("Parameter Name") == "True When"
													  select rw.Field<string>("Parameter Value")).FirstOrDefault());

						var userFileSelected = (string)await fileName.EvaluateCode(engine);

						bool fileExistCheck = false;
						if (trueWhenFileExists == "It Does Exist")
							fileExistCheck = true;

						if (File.Exists(userFileSelected) == fileExistCheck)
							ifResult = true;
						break;
					case "Folder Exists":			
						string folderName = ((from rw in IfActionParameterTable.AsEnumerable()
											  where rw.Field<string>("Parameter Name") == "Folder Path"
											  select rw.Field<string>("Parameter Value")).FirstOrDefault());

						string trueWhenFolderExists = ((from rw in IfActionParameterTable.AsEnumerable()
													  where rw.Field<string>("Parameter Name") == "True When"
													  select rw.Field<string>("Parameter Value")).FirstOrDefault());

						var userFolderSelected = (string)await folderName.EvaluateCode(engine);

						bool folderExistCheck = false;
						if (trueWhenFolderExists == "It Does Exist")
							folderExistCheck = true;

						if (Directory.Exists(userFolderSelected) == folderExistCheck)
							ifResult = true;
						break;
					case "Selenium Web Element Exists":				
						string instanceName = ((from rw in IfActionParameterTable.AsEnumerable()
												where rw.Field<string>("Parameter Name") == "Selenium Instance Name"
												select rw.Field<string>("Parameter Value")).FirstOrDefault());

						OBAppInstance instance = (OBAppInstance)await instanceName.EvaluateCode(engine);

						string parameterName = ((from rw in IfActionParameterTable.AsEnumerable()
												 where rw.Field<string>("Parameter Name") == "Element Search Parameter"
												 select rw.Field<string>("Parameter Value")).FirstOrDefault());

						string searchMethod = ((from rw in IfActionParameterTable.AsEnumerable()
												where rw.Field<string>("Parameter Name") == "Element Search Method"
												select rw.Field<string>("Parameter Value")).FirstOrDefault());

						string timeoutStr = ((from rw in IfActionParameterTable.AsEnumerable()
											  where rw.Field<string>("Parameter Name") == "Timeout (Seconds)"
											  select rw.Field<string>("Parameter Value")).FirstOrDefault());

						int timeout = (int)await timeoutStr.EvaluateCode(engine);

						string trueWhenElementExists = (from rw in IfActionParameterTable.AsEnumerable()
														where rw.Field<string>("Parameter Name") == "True When"
														select rw.Field<string>("Parameter Value")).FirstOrDefault();

						bool elementExists = await ElementExists(engine, instance, searchMethod, parameterName, "Find Element", timeout);
						ifResult = elementExists;

						if (trueWhenElementExists == "It Does Not Exist")
							ifResult = !ifResult;
						break;
					case "GUI Element Exists":			
						string guiWindowName = (from rw in IfActionParameterTable.AsEnumerable()
											 where rw.Field<string>("Parameter Name") == "Window Name"
											 select rw.Field<string>("Parameter Value")).FirstOrDefault();
						windowName = (string)await guiWindowName.EvaluateCode(engine);

						string elementSearchParam = (from rw in IfActionParameterTable.AsEnumerable()
													 where rw.Field<string>("Parameter Name") == "Element Search Parameter"
													 select rw.Field<string>("Parameter Value")).FirstOrDefault();
						elementSearchParam = (string)await elementSearchParam.EvaluateCode(engine);

						string elementSearchMethod = (from rw in IfActionParameterTable.AsEnumerable()
													  where rw.Field<string>("Parameter Name") == "Element Search Method"
													  select rw.Field<string>("Parameter Value")).FirstOrDefault();
						elementSearchMethod = (string)await elementSearchMethod.EvaluateCode(engine);

						string trueWhenGUIElementExists = (from rw in IfActionParameterTable.AsEnumerable()
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

						if (trueWhenGUIElementExists == "It Does Not Exist")
							ifResult = !ifResult;
						break;
					case "Image Element Exists":				
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
						string imageTimeoutString = (from rw in IfActionParameterTable.AsEnumerable()
												where rw.Field<string>("Parameter Name") == "Timeout (Seconds)"
												select rw.Field<string>("Parameter Value")).FirstOrDefault();
						int imageTimeout = (int)await imageTimeoutString.EvaluateCode(engine);

						var element = FindImageElement(capturedImage, accuracy, engine, DateTime.Now.AddSeconds(imageTimeout));
						FormsHelper.ShowAllForms(engine.EngineContext.IsDebugMode);

						if (element != null)
							ifResult = true;
						else
							ifResult = false;

						if (trueWhenImageExists == "It Does Not Exist")
							ifResult = !ifResult;
						break;
					case "App Instance Exists":				
						string appInstanceName = (from rw in IfActionParameterTable.AsEnumerable()
											   where rw.Field<string>("Parameter Name") == "Instance Name"
											   select rw.Field<string>("Parameter Value")).FirstOrDefault();


						string trueWhenAppInstanceExists = (from rw in IfActionParameterTable.AsEnumerable()
													  where rw.Field<string>("Parameter Name") == "True When"
													  select rw.Field<string>("Parameter Value")).FirstOrDefault();

						var appInstanceObj = (OBAppInstance)await appInstanceName.EvaluateCode(engine);

						if (appInstanceObj == null)
							ifResult = false;
						else
							ifResult = appInstanceObj.Value.InstanceExists();

						if (trueWhenAppInstanceExists == "It Does Not Exist")
							ifResult = !ifResult;
						break;
					default:
						throw new Exception("If type not recognized!");
				}
			}
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
