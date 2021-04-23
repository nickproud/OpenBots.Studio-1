using OpenBots.Core.Utilities.CommonUtilities;
using OpenQA.Selenium;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using ExcelApplication = Microsoft.Office.Interop.Excel.Application;
using WordApplication = Microsoft.Office.Interop.Word.Application;

namespace OpenBots.Core.Script
{
    public static class ScriptDefaultTypes
    {
        public static Dictionary<string, Type> DefaultVarArgTypes = new Dictionary<string, Type>()
        {
            { "More Options...", typeof(MoreOptions) },
            { typeof(string).GetRealTypeName(), typeof(string) },
            { typeof(int).GetRealTypeName(), typeof(int) },
            { typeof(double).GetRealTypeName(), typeof(double) },
            { typeof(bool).GetRealTypeName(), typeof(bool) },
            { typeof(object).GetRealTypeName(), typeof(object) },
            { typeof(List<string>).GetRealTypeName(), typeof(List<string>) },
            { typeof(Dictionary<string, string>).GetRealTypeName(), typeof(Dictionary<string, string>) },
            { typeof(DataTable).GetRealTypeName(), typeof(DataTable) },
            { typeof(DataRow).GetRealTypeName(), typeof(DataRow) }         
        };

        public static Dictionary<string, Type> DefaultInstanceTypes = new Dictionary<string, Type>()
        {
            { "More Options...", typeof(MoreOptions) },
            { typeof(IWebDriver).ToString(), typeof(IWebDriver) },
            { typeof(ExcelApplication).ToString(), typeof(ExcelApplication) },
            { typeof(WordApplication).ToString(), typeof(WordApplication) },
            { typeof(InternetExplorer).ToString(), typeof(InternetExplorer) },
            { typeof(OleDbConnection).ToString(), typeof(OleDbConnection) },
            { typeof(Stopwatch).ToString(), typeof(Stopwatch) }
        };
    }

    public static class MoreOptions
    {

    }
}
