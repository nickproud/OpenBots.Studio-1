using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text.RegularExpressions;
using DataTable = System.Data.DataTable;
using ExcelApplication = Microsoft.Office.Interop.Excel.Application;
using OutlookApplication = Microsoft.Office.Interop.Outlook.Application;
using WordApplication = Microsoft.Office.Interop.Word.Application;

namespace OpenBots.Core.Script
{
    public class ScriptDefaultNamespaces
    {
        public static Dictionary<string, AssemblyReference> DefaultNamespaces = new Dictionary<string, AssemblyReference>()
        {
            //all default namespaces are part of mscorlib
            { "Microsoft.Office.Interop.Excel", GetAssemblyReference(typeof(ExcelApplication)) },
            { "Microsoft.Office.Interop.Outlook", GetAssemblyReference(typeof(OutlookApplication)) },
            { "Microsoft.Office.Interop.Word", GetAssemblyReference(typeof(WordApplication)) },
            { "MimeKit", GetAssemblyReference(typeof(MimeMessage)) },
            { "System", GetAssemblyReference(typeof(string)) },
            { "System.Collections.Generic", GetAssemblyReference(typeof(string)) },
            { "System.Data", GetAssemblyReference(typeof(DataTable)) },
            { "System.Linq", GetAssemblyReference(typeof(IQueryable)) },
            { "System.Security", GetAssemblyReference(typeof(SecureString)) },
            { "System.Text", GetAssemblyReference(typeof(string)) },
            { "System.Text.RegularExpressions", GetAssemblyReference(typeof(Regex)) },
            { "System.Threading.Tasks", GetAssemblyReference(typeof(string)) }
        };

        private static AssemblyReference GetAssemblyReference(Type type)
        {
            return new AssemblyReference(Assembly.GetAssembly(type).GetName().Name,
                                         Assembly.GetAssembly(type).GetName().Version.ToString());
        }
    }
}
