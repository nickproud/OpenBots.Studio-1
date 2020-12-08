using Autofac;
using OpenBots.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OpenBots.Nuget
{
    public class AppDomainSetupManager
    {       
        public static ContainerBuilder LoadBuilder(List<string> assemblyPaths)
        {
            List<Assembly> existingAssemblies = new List<Assembly>();
            Parallel.ForEach(assemblyPaths, path =>
            {
                try
                {
                    var assemblyinfo = AssemblyName.GetAssemblyName(path);
                    var name = assemblyinfo.Name;
                    var version = assemblyinfo.Version.ToString();

                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    var existingAssembly = assemblies.Where(x => x.GetName().Name == name &&
                                                                 x.GetName().Version.ToString() == version)
                                                     .FirstOrDefault();

                    if ((existingAssembly == null && name != "OpenBots.Engine" && name != "RestSharp"))
                    {
                        var assembly = Assembly.LoadFrom(path);
                        existingAssemblies.Add(assembly);
                    }
                    else if (name != "OpenBots.Engine" && name != "RestSharp")
                        existingAssemblies.Add(existingAssembly);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });

            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(existingAssemblies.ToArray())
                                                   .Where(t => t.IsAssignableTo<ScriptCommand>())
                                                   .Named<ScriptCommand>(t => t.Name)
                                                   .AsImplementedInterfaces();
            return builder;
        }
    }
}
