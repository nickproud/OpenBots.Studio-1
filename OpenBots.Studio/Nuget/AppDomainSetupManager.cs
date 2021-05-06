using Autofac;
using OpenBots.Core.Command;
using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Studio.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenBots.Nuget
{
    public class AppDomainSetupManager
    {
        public static ContainerBuilder LoadBuilder(List<string> assemblyPaths, Dictionary<string, List<Type>> groupedTypes, Dictionary<string, AssemblyReference> allNamespaces,
            Dictionary<string, AssemblyReference> importedNamespaces)
        {
            List<Assembly> existingAssemblies = new List<Assembly>();
            foreach(var path in assemblyPaths)
            {
                try
                {
                    var name = AssemblyName.GetAssemblyName(path).Name;

                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    var existingAssembly = assemblies.Where(x => x.GetName().Name == name &&
                                                                 x.GetName().Version.ToString() == AssemblyName.GetAssemblyName(path).Version.ToString())
                                                     .FirstOrDefault();

                    if (existingAssembly == null && name != "RestSharp" && name != "WebDriver" && name != "OpenBots.Core")
                    {
                        //has to be LoadFile because package manager can't update/uninstall assemblies if LoadFrom
                        var assembly = Assembly.LoadFrom(path);
                        existingAssemblies.Add(assembly);
                    }
                    else if (existingAssembly != null)
                        existingAssemblies.Add(existingAssembly);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            //TODO: limit types to one in loaded assemblies. Previously getting all assemblies instead of just the ones in existingAssemblies because mscorlib was missing
            TypeMethods.GenerateAllNamespaces(AppDomain.CurrentDomain.GetAssemblies().ToList(), allNamespaces);
            TypeMethods.GenerateAllVariableTypes(NamespaceMethods.GetAssemblies(importedNamespaces), groupedTypes);


            //if no commands have been loaded, at least include OpenBots.Core to access the BrokenCodeCommand
            if (existingAssemblies.Count == 0)
                existingAssemblies.Add(Assembly.GetAssembly(typeof(BrokenCodeCommentCommand)));

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(existingAssemblies.ToArray())
                                                   .Where(t => t.IsAssignableTo<ScriptCommand>())
                                                   .Named<ScriptCommand>(t => t.Name)
                                                   .AsImplementedInterfaces();
            return builder;
        }
    }
}
