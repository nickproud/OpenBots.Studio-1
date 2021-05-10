using OpenBots.Core.Infrastructure;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using ExcelApplication = Microsoft.Office.Interop.Excel.Application;
using WordApplication = Microsoft.Office.Interop.Word.Application;

namespace OpenBots.Core.Utilities.CommonUtilities
{
    public static class InstanceMethods
    {
        public static bool InstanceExists(this string instanceName, IAutomationEngineInstance engine)
        {          
            try
            {
                object appObject = null;
                var existingVar = engine.AutomationEngineContext.Variables.Where(x => x.VariableName == instanceName).FirstOrDefault();
                if (existingVar != null)
                    appObject = existingVar.VariableValue;
                var existingArg = engine.AutomationEngineContext.Arguments.Where(x => x.ArgumentName == instanceName).FirstOrDefault();
                if (existingArg != null)
                    appObject = existingArg.ArgumentValue;

                if (appObject != null)
                {                   
                    string appType = appObject.GetType().ToString();

                    switch (appType.ToString())
                    {
                        case "Microsoft.Office.Interop.Excel.ApplicationClass":
                            var excelApp = (ExcelApplication)appObject;
                            if (excelApp.Application != null)
                                return true;
                            break;
                        case "Microsoft.Office.Interop.Word.ApplicationClass":
                            var wordApp = (WordApplication)appObject;
                            if (wordApp.Application != null)
                                return true;
                            break;
                        case "OpenQA.Selenium.Chrome.ChromeDriver":
                            var chromeDriver = (ChromeDriver)appObject;
                            if (chromeDriver.CurrentWindowHandle != null)
                                return true;
                            break;
                        case "OpenQA.Selenium.Firefox.FirefoxDriver":
                            var firefoxDriver = (FirefoxDriver)appObject;
                            if (firefoxDriver.CurrentWindowHandle != null)
                                return true;
                            break;
                        case "OpenQA.Selenium.IE.InternetExplorerDriver":
                            var ieDriver = (InternetExplorerDriver)appObject;
                            if (ieDriver.CurrentWindowHandle != null)
                                return true;
                            break;
                        default:
                            throw new InvalidComObjectException($"App instance '{appType}' not supported.");
                    }                                                             
                }
                return false;                    
            }
            catch (Exception ex)
            {
                if (ex is InvalidComObjectException)
                    throw ex;

                return false;
            }
        }
    }
}
