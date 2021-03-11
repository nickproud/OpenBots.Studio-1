using Autofac;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Versioning;
using OpenBots.Core.Enums;
using OpenBots.Core.IO;
using OpenBots.Core.Project;
using OpenBots.Core.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Nuget
{
    public class NugetPackageManager
    {
        public static async Task<List<NuGetVersion>> GetPackageVersions(string packageId, string source, bool includePrerelease)
        {
            ILogger logger = NullLogger.Instance;
            CancellationToken cancellationToken = CancellationToken.None;

            SourceCacheContext cache = new SourceCacheContext();
            SourceRepository repository = Repository.Factory.GetCoreV3(source);
            FindPackageByIdResource resource = await repository.GetResourceAsync<FindPackageByIdResource>();

            IEnumerable<NuGetVersion> versions = await resource.GetAllVersionsAsync(
                packageId,
                cache,
                logger,
                cancellationToken);

            if (includePrerelease)
                return versions.ToList();
            else
                return versions.Where(x => x.IsPrerelease == false).ToList();
        }

        public static async Task<List<IPackageSearchMetadata>> SearchPackages(string packageKeyword, string source, bool includePrerelease)
        {
            ILogger logger = NullLogger.Instance;
            CancellationToken cancellationToken = CancellationToken.None;

            SourceRepository repository = Repository.Factory.GetCoreV3(source);
            PackageSearchResource resource = await repository.GetResourceAsync<PackageSearchResource>();
            SearchFilter searchFilter = new SearchFilter(includePrerelease: includePrerelease);

            IEnumerable<IPackageSearchMetadata> results = await resource.SearchAsync(
                packageKeyword,
                searchFilter,
                skip: 0,
                take: 35,
                logger,
                cancellationToken);

            return results.ToList();
        }

        public static async Task<List<IPackageSearchMetadata>> GetPackageMetadata(string packageId, string source, bool includePrerelease)
        {
            ILogger logger = NullLogger.Instance;
            CancellationToken cancellationToken = CancellationToken.None;

            SourceCacheContext cache = new SourceCacheContext();
            SourceRepository repository = Repository.Factory.GetCoreV3(source);
            PackageMetadataResource resource = await repository.GetResourceAsync<PackageMetadataResource>();

            IEnumerable<IPackageSearchMetadata> packages = await resource.GetMetadataAsync(
                packageId,
                includePrerelease: includePrerelease,
                includeUnlisted: true,
                cache,
                logger,
                cancellationToken);

            return packages.ToList();
        }

        public static async Task DownloadPackage(string packageId, string version, string packageLocation, string packageName, string feed)
        {
            ILogger logger = NullLogger.Instance;
            CancellationToken cancellationToken = CancellationToken.None;

            SourceCacheContext cache = new SourceCacheContext();
            SourceRepository repository = Repository.Factory.GetCoreV3(feed);
            FindPackageByIdResource resource = await repository.GetResourceAsync<FindPackageByIdResource>();

            NuGetVersion packageVersion = new NuGetVersion(version);
            using (MemoryStream packageStream = new MemoryStream()) {

                bool success = await resource.CopyNupkgToStreamAsync(
                packageId,
                packageVersion,
                packageStream,
                cache,
                logger,
                cancellationToken);

                var path = Path.Combine(packageLocation, packageName + ".nupkg");
                using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    packageStream.WriteTo(fileStream);
                }
            }
        }

        public static async Task DownloadPackage(string packageId, string version, string packageLocation, string packageName, List<SourceRepository> repositories)
        {
            ILogger logger = NullLogger.Instance;
            CancellationToken cancellationToken = CancellationToken.None;

            SourceCacheContext cache = new SourceCacheContext();

            foreach (var repository in repositories)
            {
                FindPackageByIdResource resource = await repository.GetResourceAsync<FindPackageByIdResource>();

                NuGetVersion packageVersion = new NuGetVersion(version);
                using (MemoryStream packageStream = new MemoryStream())
                {

                    bool success = await resource.CopyNupkgToStreamAsync(
                    packageId,
                    packageVersion,
                    packageStream,
                    cache,
                    logger,
                    cancellationToken);

                    if (success)
                    {
                        var path = Path.Combine(packageLocation, packageName + ".nupkg");
                        using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            packageStream.WriteTo(fileStream);
                        }
                        break;
                    }                   
                }
            }
        }

        public static async Task InstallPackage(string packageId, string version, Dictionary<string, string> projectDependenciesDict, string installDefaultSource = "")
        {
            var packageSources = new ApplicationSettings().GetOrCreateApplicationSettings().ClientSettings.PackageSourceDT;
            var packageVersion = NuGetVersion.Parse(version);
            var nuGetFramework = NuGetFramework.ParseFolder("net48");
            var settings = Settings.LoadDefaultSettings(root: null);
            var sourceRepositoryProvider = new SourceRepositoryProvider(new PackageSourceProvider(settings), Repository.Provider.GetCoreV3());

            using (var cacheContext = new SourceCacheContext())
            {
                var repositories = new List<SourceRepository>();
                if (!string.IsNullOrEmpty(installDefaultSource))
                {
                    var sourceRepo = sourceRepositoryProvider.CreateRepository(new PackageSource(installDefaultSource, "Default Packages Source", true));
                    repositories.Add(sourceRepo);
                }                  
                else
                {
                    foreach (DataRow row in packageSources.Rows)
                    {
                        if (row[0].ToString() == "True")
                        {
                            var sourceRepo = sourceRepositoryProvider.CreateRepository(new PackageSource(row[2].ToString(), row[1].ToString(), true));
                            repositories.Add(sourceRepo);
                        }
                    }
                }
                
                var availablePackages = new HashSet<SourcePackageDependencyInfo>(PackageIdentityComparer.Default);
                await GetPackageDependencies(
                    new PackageIdentity(packageId, packageVersion),
                    nuGetFramework, cacheContext, NullLogger.Instance, repositories, availablePackages);

                var resolverContext = new PackageResolverContext(
                    DependencyBehavior.Lowest,
                    new[] { packageId },
                    Enumerable.Empty<string>(),
                    Enumerable.Empty<PackageReference>(),
                    Enumerable.Empty<PackageIdentity>(),
                    availablePackages,
                    sourceRepositoryProvider.GetRepositories().Select(s => s.PackageSource),
                    NullLogger.Instance);

                var resolver = new PackageResolver();

                //TODO: Installation failure happens here if package isn't found. Failure is being caught but not reported because it's asynchronous 
                var packagesToInstall = resolver.Resolve(resolverContext, CancellationToken.None)
                    .Select(p => availablePackages.Single(x => PackageIdentityComparer.Default.Equals(x, p)));
                
                var packagePathResolver = new PackagePathResolver(Folders.GetFolder(FolderType.LocalAppDataPackagesFolder));
                var packageExtractionContext = new PackageExtractionContext(
                    PackageSaveMode.Defaultv3,
                    XmlDocFileSaveMode.None,
                    ClientPolicyContext.GetClientPolicy(settings, NullLogger.Instance),
                    NullLogger.Instance);

                var frameworkReducer = new FrameworkReducer();
                PackageReaderBase packageReader;
                PackageDownloadContext downloadContext = new PackageDownloadContext(cacheContext);

                foreach(var packageToInstall in packagesToInstall)
                {
                    var installedPath = packagePathResolver.GetInstalledPath(packageToInstall);
                    if (installedPath == null)
                    {
                        var downloadResource = await packageToInstall.Source.GetResourceAsync<DownloadResource>(CancellationToken.None);
                        var downloadResult = await downloadResource.GetDownloadResourceResultAsync(
                            packageToInstall,
                            downloadContext,
                            SettingsUtility.GetGlobalPackagesFolder(settings),
                            NullLogger.Instance, CancellationToken.None);

                        await PackageExtractor.ExtractPackageAsync(
                            downloadResult.PackageSource,
                            downloadResult.PackageStream,
                            packagePathResolver,
                            packageExtractionContext,
                            CancellationToken.None);

                        packageReader = downloadResult.PackageReader;
                    }
                    else
                        packageReader = new PackageFolderReader(installedPath);

                    if (packageToInstall.Id == packageId)
                    {
                        if (projectDependenciesDict.ContainsKey(packageToInstall.Id))
                            projectDependenciesDict[packageToInstall.Id] = packageToInstall.Version.ToString();
                        else
                            projectDependenciesDict.Add(packageToInstall.Id, packageToInstall.Version.ToString());
                    }
                }
            }            
        }

        public static async Task GetPackageDependencies(PackageIdentity package,
                NuGetFramework framework,
                SourceCacheContext cacheContext,
                ILogger logger,
                IEnumerable<SourceRepository> repositories,
                ISet<SourcePackageDependencyInfo> availablePackages,
                bool ignoreCommandPackages = false)
        {
            if (availablePackages.Contains(package)) 
                return;

            foreach (var sourceRepository in repositories)
            {
                var dependencyInfoResource = await sourceRepository.GetResourceAsync<DependencyInfoResource>();
                var dependencyInfo = await dependencyInfoResource.ResolvePackage(
                    package, framework, cacheContext, logger, CancellationToken.None);

                if (dependencyInfo == null) 
                    continue;

                if (!(ignoreCommandPackages && (dependencyInfo.Id.StartsWith("OpenBots.Commands") || dependencyInfo.Id.StartsWith("OpenBots.Core"))))
                    availablePackages.Add(dependencyInfo);
   
                foreach (var dependency in dependencyInfo.Dependencies)
                {
                    await GetPackageDependencies(
                        new PackageIdentity(dependency.Id, dependency.VersionRange.MinVersion),
                        framework, cacheContext, logger, repositories, availablePackages, ignoreCommandPackages);
                }
            }
        }

        public static List<string> LoadPackageAssemblies(string configPath, bool throwException = false)
        {
            string packagesPath = Folders.GetFolder(FolderType.LocalAppDataPackagesFolder);
            List<string> assemblyPaths = new List<string>();

            var dependencies = JsonConvert.DeserializeObject<Project>(File.ReadAllText(configPath)).Dependencies;
            var packagePathResolver = new PackagePathResolver(packagesPath);

            var nuGetFramework = NuGetFramework.ParseFolder("net48");
            var settings = Settings.LoadDefaultSettings(root: null);

            var sourceRepositoryProvider = new SourceRepositoryProvider(new PackageSourceProvider(settings), Repository.Provider.GetCoreV3());
            var localRepo = sourceRepositoryProvider.CreateRepository(new PackageSource(packagesPath, "Local OpenBots Repo", true));
            
            var resolver = new PackageResolver();
            var frameworkReducer = new FrameworkReducer();
            var repositories = new List<SourceRepository>
            {
                localRepo
            };

            Parallel.ForEach(dependencies, async dependency =>
            {
                try
                {
                    using (var cacheContext = new SourceCacheContext())
                    {
                        var availablePackages = new HashSet<SourcePackageDependencyInfo>(PackageIdentityComparer.Default);
                        await GetPackageDependencies(
                            new PackageIdentity(dependency.Key, NuGetVersion.Parse(dependency.Value)),
                            nuGetFramework, cacheContext, NullLogger.Instance, repositories, availablePackages);

                        var resolverContext = new PackageResolverContext(
                            DependencyBehavior.Lowest,
                            new[] { dependency.Key },
                            Enumerable.Empty<string>(),
                            Enumerable.Empty<PackageReference>(),
                            Enumerable.Empty<PackageIdentity>(),
                            availablePackages,
                            sourceRepositoryProvider.GetRepositories().Select(s => s.PackageSource),
                            NullLogger.Instance);

                        var packagesToInstall = resolver.Resolve(resolverContext, CancellationToken.None)
                            .Select(p => availablePackages.Single(x => PackageIdentityComparer.Default.Equals(x, p)));

                        foreach (var packageToInstall in packagesToInstall)
                        {
                            PackageReaderBase packageReader = new PackageFolderReader(packagePathResolver.GetInstalledPath(packageToInstall));

                            var nearest = frameworkReducer.GetNearest(nuGetFramework, packageReader.GetLibItems().Select(x => x.TargetFramework));

                            var packageListAssemblyPaths = packageReader.GetLibItems()
                                .Where(x => x.TargetFramework.Equals(nearest))
                                .SelectMany(x => x.Items.Where(i => i.EndsWith(".dll"))).ToList();

                            if (packageListAssemblyPaths != null)
                            {
                                foreach (string path in packageListAssemblyPaths)
                                {
                                    if (!assemblyPaths.Contains(Path.Combine(packagesPath, $"{packageToInstall.Id}.{packageToInstall.Version}", path)))
                                        assemblyPaths.Add(Path.Combine(packagesPath, $"{packageToInstall.Id}.{packageToInstall.Version}", path));
                                }                                   
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Only true for scheduled and attended executions
                    if (throwException)
                    {
                        MessageBox.Show($"Unable to load {packagesPath}\\{dependency.Key}.{dependency.Value}. " +
                                        "Please install this package using the OpenBots Studio Package Manager.", "Error");

                        Application.Exit();
                    }
                                       
                    else
                        Console.WriteLine(ex);
                }
            });

            try
            {
                return FilterAssemblies(assemblyPaths);
            }
            catch (Exception)
            {
                //try again
                return LoadPackageAssemblies(configPath, throwException);
            }
        }

        private static List<string> FilterAssemblies(List<string> assemblyPaths)
        {
            List<string> filteredPaths = new List<string>();
            foreach (string path in assemblyPaths)
            {
                if (filteredPaths.Where(a => a.Contains(path.Split('/').Last()) && FileVersionInfo.GetVersionInfo(a).FileVersion ==
                                        FileVersionInfo.GetVersionInfo(path).FileVersion).FirstOrDefault() == null)
                    filteredPaths.Add(path);
            }

            return filteredPaths;
        }

        #region Install Default Packages

        //moves all package files from OpenBots.Packages to Program Files (x86)/OpenBots Inc/packages
        public static List<string> MovePackagesToProgramFiles()
        {
            List<string> defaultCommandsList = Project.DefaultCommandGroups;

            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string openBotsPackagesBuildPath = Path.Combine(new DirectoryInfo(projectDirectory).Parent.Parent.Parent.FullName, "OpenBots.Packages");

            string programPackagesSource = Folders.GetFolder(FolderType.ProgramFilesPackagesFolder);
            if (!Directory.Exists(programPackagesSource))
                Directory.CreateDirectory(programPackagesSource);

            string applicationVersion = Application.ProductVersion;
            var commandVersion = Regex.Matches(applicationVersion, @"\d+\.\d+\.\d+")[0].ToString();

            List<string> packageFilePaths = Directory.GetFiles(openBotsPackagesBuildPath)
                                                     .Where(x => x.EndsWith(commandVersion + ".nupkg") &&
                                                     (defaultCommandsList.Contains(Path.GetFileNameWithoutExtension(x).Split('.')[2]) ||
                                                     Path.GetFileNameWithoutExtension(x).Split('.')[1] == "Core"))
                                                     .ToList();

            foreach (string packagePath in packageFilePaths)
            {
                string fileName = new FileInfo(packagePath).Name;
                File.Copy(packagePath, Path.Combine(programPackagesSource, fileName), true);
            }

            List<string> newPackageFilePaths = Directory.GetFiles(programPackagesSource).Where(x => x.EndsWith(commandVersion + ".nupkg")).ToList();
            return newPackageFilePaths;
        }

        //determines all command package dependencies and downloads their .nupkg files to Program Files
        public static async Task DownloadCommandDependencyPackages()
        {
            string programPackagesSource = Folders.GetFolder(FolderType.ProgramFilesPackagesFolder);
            List<string> newPackageFilePaths = MovePackagesToProgramFiles();
            string nugetSourcePath = "https://api.nuget.org/v3/index.json";
            string gallerySourcePath = "https://gallery.openbots.io/v3/command.json";

            List<string> packageList = new List<string>();

            var settings = Settings.LoadDefaultSettings(root: null);
            var sourceRepositoryProvider = new SourceRepositoryProvider(new PackageSourceProvider(settings), Repository.Provider.GetCoreV3());
            var repositories = new List<SourceRepository>();
            var sourceRepo = sourceRepositoryProvider.CreateRepository(new PackageSource(programPackagesSource, "Program Files", true));
            var nugetRepo = sourceRepositoryProvider.CreateRepository(new PackageSource(nugetSourcePath, "Nuget", true));
            var galleryRepo = sourceRepositoryProvider.CreateRepository(new PackageSource(gallerySourcePath, "Gallery", true));
            repositories.Add(sourceRepo);
            repositories.Add(nugetRepo);
            repositories.Add(galleryRepo);

            try
            {

                foreach (string packagePath in newPackageFilePaths)
                {
                    var matches = Regex.Matches(packagePath, @"(\w+\.\w+\.*\w*)\.(\d+\.\d+\.\d+)");
                    string packageId = matches[0].Groups[1].Value;
                    string version = matches[0].Groups[2].Value;

                    var packageVersion = NuGetVersion.Parse(version);
                    var nuGetFramework = NuGetFramework.ParseFolder("net48");                   
                                   
                    using (var cacheContext = new SourceCacheContext())
                    {                        

                        var availablePackages = new HashSet<SourcePackageDependencyInfo>(PackageIdentityComparer.Default);
                        await GetPackageDependencies(
                            new PackageIdentity(packageId, packageVersion),
                            nuGetFramework, cacheContext, NullLogger.Instance, repositories, availablePackages, true);

                        foreach (var package in availablePackages)
                            packageList.Add($"{package.Id}*{package.Version}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            List<string> filteredPackageList = packageList.Distinct().ToList();

            foreach (var package in filteredPackageList)
                await DownloadPackage(package.Split('*')[0], package.Split('*')[1], programPackagesSource, $"{package.Split('*')[0]}.{package.Split('*')[1]}", repositories);
        }
        #endregion

        #region First Time User Scan
        public static async Task SetupFirstTimeUserEnvironment()
        {
            bool isSplashLabelVisible = false;
            string packagesPath = Folders.GetFolder(FolderType.LocalAppDataPackagesFolder);
            string programPackagesSource = Folders.GetFolder(FolderType.ProgramFilesPackagesFolder);

            if (!Directory.Exists(programPackagesSource))
                throw new DirectoryNotFoundException($"Unable to find '{programPackagesSource}'.");

            var commandVersion = Regex.Matches(Application.ProductVersion, @"\d+\.\d+\.\d+")[0].ToString();

            Dictionary<string, string> dependencies = Project.DefaultCommandGroups.ToDictionary(x => $"OpenBots.Commands.{x}", x => commandVersion);

            List<string> existingOpenBotsPackages = Directory.GetDirectories(packagesPath)
                                                             .Where(x => new DirectoryInfo(x).Name.StartsWith("OpenBots"))
                                                             .ToList();
            foreach(var dep in dependencies)
            {
                string existingDirectory = existingOpenBotsPackages.Where(x => new DirectoryInfo(x).Name.Equals($"{dep.Key}.{dep.Value}"))
                                                                   .FirstOrDefault();
                if (existingDirectory == null)
                {
                    if (!isSplashLabelVisible)
                    {
                        Program.SplashForm.lblFirstTimeSetup.Visible = true;
                        Program.SplashForm.Refresh();
                        isSplashLabelVisible = true;
                    }
                    
                    await InstallPackage(dep.Key, dep.Value, new Dictionary<string, string>(), programPackagesSource);
                }
            }
        }
        #endregion
    }
}