using OpenBots.Core.Enums;
using System;

namespace OpenBots.Core.Script
{
    [Serializable]
    public class ScriptArgument
    {
        /// <summary>
        /// name that will be used to identify the argument
        /// </summary>
        public string ArgumentName { get; set; }

        /// <summary>
        /// direction of the argument or current index
        /// </summary>
        public ScriptArgumentDirection Direction { get; set; }


        /// <summary>
        /// type of the argument or current index
        /// </summary>
        public Type ArgumentType { get; set; }

        /// <summary>
        /// value of the argument or current index
        /// </summary>
        public object ArgumentValue { get; set; }

        /// <summary>
        /// name of the variable assigned to an Out Argument
        /// </summary>
        public string AssignedVariable { get; set; }

        /// <summary>
        /// To check if the value is a secure string
        /// </summary>
        public bool IsSecureString { get; set; }
    }
}
