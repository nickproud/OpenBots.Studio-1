using OpenBots.Core.Model.ApplicationModel;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.Data;

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
            { typeof(Dictionary<string, object>).GetRealTypeName(), typeof(Dictionary<string, object>) },
            { typeof(DataTable).GetRealTypeName(), typeof(DataTable) },
            { typeof(DataRow).GetRealTypeName(), typeof(DataRow) },         
            { typeof(OBAppInstance).GetRealTypeName(), typeof(OBAppInstance) }         
        };
    }

    public static class MoreOptions
    {

    }
}
