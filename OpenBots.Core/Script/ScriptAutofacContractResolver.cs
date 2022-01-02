using Autofac;
using Newtonsoft.Json.Serialization;
using OpenBots.Core.Command;
using System;
using System.Linq;

namespace OpenBots.Core.Script
{
    public class ScriptAutofacContractResolver : DefaultContractResolver
    {
        private readonly IContainer _container;

        public ScriptAutofacContractResolver(IContainer container)
        {
            _container = container;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            // use Autofac to create types that have been registered with it
            var commandType = GetTypeByName(_container, objectType.Name);
            if (commandType != null)
            {
                 JsonObjectContract contract = base.CreateObjectContract(commandType);
                contract.DefaultCreator = () => Activator.CreateInstance(commandType);
                return contract;
            }
            else if (objectType.BaseType != null && objectType.BaseType.Name == "ScriptCommand")
            {
                JsonObjectContract contract = base.CreateObjectContract(objectType);
                contract.DefaultCreator = () => null;
                return contract;
            }

            return base.CreateObjectContract(objectType);
        }

        public static Type GetTypeByName(IContainer container, string typeName)
        {
            using (var scope = container.BeginLifetimeScope())
            {
                var types = scope.ComponentRegistry.Registrations
                            .Where(r => typeof(ScriptCommand).IsAssignableFrom(r.Activator.LimitType))
                            .Select(r => r.Activator.LimitType).ToList();

                var commandType = types.Where(x => x.Name == typeName).FirstOrDefault();

                return commandType;
            }
        }
    }
}
