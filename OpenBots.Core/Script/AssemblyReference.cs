namespace OpenBots.Core.Script
{
    public class AssemblyReference
    {
        public string AssemblyName { get; set; }
        public string AssemblyVersion { get; set; }

        public AssemblyReference(string assemblyName, string assemblyVersion)
        {
            AssemblyName = assemblyName;
            AssemblyVersion = assemblyVersion;
        }
    }
}
