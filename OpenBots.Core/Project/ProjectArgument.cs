using OpenBots.Core.Enums;
using System;

namespace OpenBots.Core.Script
{
    [Serializable]
    public class ProjectArgument
    {
        /// <summary>
        /// name that will be used to identify the argument
        /// </summary>
        public string ArgumentName { get; set; }

        /// <summary>
        /// type of the argument or current index
        /// </summary>
        public Type ArgumentType { get; set; }

        /// <summary>
        /// value of the argument or current index
        /// </summary>
        public object ArgumentValue { get; set; }

        /// <summary>
        /// direction of the argument or current index
        /// </summary>
        public ScriptArgumentDirection Direction { get; set; } = ScriptArgumentDirection.In;
    }
}
