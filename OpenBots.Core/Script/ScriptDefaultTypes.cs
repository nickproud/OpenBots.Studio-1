using System;
using System.Collections.Generic;
using System.Data;

namespace OpenBots.Core.Script
{
    public static class ScriptDefaultTypes
    {
        //public static List<Type> DefaultTypes = new List<Type>()
        //{
        //    typeof(string),
        //    typeof(int),
        //    typeof(DataTable),
        //    typeof(DataRow)
        //};

        public static Dictionary<string, Type> DefaultTypes = new Dictionary<string, Type>()
        {
            { "More Options...", typeof(MoreOptions) },
            { typeof(string).ToString(), typeof(string) },
            { typeof(int).ToString(), typeof(int) },
            { typeof(DataTable).ToString(), typeof(DataTable) },
            { typeof(DataRow).ToString(), typeof(DataRow) }         
        };
    }

    public static class MoreOptions
    {

    }
}
