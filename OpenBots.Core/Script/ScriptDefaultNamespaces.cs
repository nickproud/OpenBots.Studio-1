using MimeKit;
using OpenBots.Core.Model.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using ExcelApplication = Microsoft.Office.Interop.Excel.Application;
using OutlookApplication = Microsoft.Office.Interop.Outlook.Application;
using WordApplication = Microsoft.Office.Interop.Word.Application;

namespace OpenBots.Core.Script
{
    public class ScriptDefaultNamespaces
    {
        public static Dictionary<string, List<AssemblyReference>> DefaultNamespaces = new Dictionary<string, List<AssemblyReference>>()
        {
            //all default namespaces are part of mscorlib
            { "Microsoft.Office.Interop.Excel", new List<AssemblyReference> { GetAssemblyReference(typeof(ExcelApplication)) } },
            { "Microsoft.Office.Interop.Outlook", new List<AssemblyReference> {GetAssemblyReference(typeof(OutlookApplication)) } },
            { "Microsoft.Office.Interop.Word", new List<AssemblyReference> { GetAssemblyReference(typeof(WordApplication)) } },
            { "MimeKit", new List<AssemblyReference> { GetAssemblyReference(typeof(MimeMessage)) } },
            { "OpenBots.Core.Model.ApplicationModel", new List<AssemblyReference> { GetAssemblyReference(typeof(OBAppInstance)) } },
            { "System", new List<AssemblyReference> { GetAssemblyReference(typeof(string)) } },
            { "System.Collections.Generic", new List<AssemblyReference> { GetAssemblyReference(typeof(string)) } },
            { "System.Data", new List<AssemblyReference> { GetAssemblyReference(typeof(DataTable)), GetAssemblyReference(typeof(DataRowComparer)) } },
            { "System.Drawing", new List<AssemblyReference> { GetAssemblyReference(typeof(Bitmap)) } },
            { "System.Linq", new List<AssemblyReference> { GetAssemblyReference(typeof(IQueryable)) } },
            { "System.Security", new List<AssemblyReference> { GetAssemblyReference(typeof(SecureString)) } },
            { "System.Text", new List<AssemblyReference> { GetAssemblyReference(typeof(string)) } },
            { "System.Text.RegularExpressions", new List<AssemblyReference> { GetAssemblyReference(typeof(Regex)) } },
            { "System.Threading.Tasks", new List<AssemblyReference> { GetAssemblyReference(typeof(string)) } },
            { "System.Windows.Forms", new List<AssemblyReference> { GetAssemblyReference(typeof(MessageBox)) } }
        };

        private static AssemblyReference GetAssemblyReference(Type type)
        {
            return new AssemblyReference(Assembly.GetAssembly(type).GetName().Name,
                                         Assembly.GetAssembly(type).GetName().Version.ToString());
        }
    }
}
