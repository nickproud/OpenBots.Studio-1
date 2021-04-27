using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OpenBots.UI.CustomControls
{
    public class CommandControlValidationContext
    {
        public Type[] CompatibleTypes { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDropDown { get; set; }
        public bool IsInstance { get; set; }
        public bool IsImageCapture { get; set; }
        public string ParameterName { get; set; }
        public ScriptCommand Command { get; set; }

        public CommandControlValidationContext(string parameterName, ScriptCommand command)
        {
            ParameterName = parameterName;
            Command = command;

            var variableProperties = Command.GetType().GetProperties().Where(f => f.Name == ParameterName).FirstOrDefault();
            var compatibleTypesAttributesAssigned = variableProperties.GetCustomAttributes(typeof(CompatibleTypes), true);
            var requiredAttributesAssigned = variableProperties.GetCustomAttributes(typeof(RequiredAttribute), true);

            if (compatibleTypesAttributesAssigned.Length > 0)
            {
                var attribute = (CompatibleTypes)compatibleTypesAttributesAssigned[0];
                CompatibleTypes = attribute.CompTypes;
            }
            else
                IsDropDown = true;

            if (requiredAttributesAssigned.Length > 0)
                IsRequired = true;

            if (parameterName == "v_InstanceName")
                IsInstance = true;

            if (parameterName == "v_ImageCapture")
                IsImageCapture = true;
        }
    }
}
