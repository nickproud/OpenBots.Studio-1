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
using OpenBots.Core.Project;
using OpenBots.Core.Server.User;
using OpenBots.Core.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public static async Task InstallPackage(string packageId, string version, Dictionary<string, string> projectDependenciesDict)
        {
            var packageSources = new ApplicationSettings().GetOrCreateApplicationSettings().ClientSettings.PackageSourceDT;
            var packageVersion = NuGetVersion.Parse(version);
            var nuGetFramework = NuGetFramework.ParseFolder("net48");
            var settings = NuGet.Configuration.Settings.LoadDefaultSettings(root: null);
            var sourceRepositoryProvider = new SourceRepositoryProvider(new PackageSourceProvider(settings), Repository.Provider.GetCoreV3());

            using (var cacheContext = new SourceCacheContext())
            {
                var repositories = new List<SourceRepository>();
                foreach (DataRow row in packageSources.Rows)
                {
                    if (row[0].ToString() == "True")
                    {
                        var sourceRepo = sourceRepositoryProvider.CreateRepository(new PackageSource(row[2].ToString(), row[1].ToString(), true));
                        repositories.Add(sourceRepo);
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
                var packagesToInstall = resolver.Resolve(resolverContext, CancellationToken.None)
                    .Select(p => availablePackages.Single(x => PackageIdentityComparer.Default.Equals(x, p)));
                string appDataPath = new DirectoryInfo(EnvironmentSettings.GetEnvironmentVariable()).Parent.FullName;
                var packagePathResolver = new PackagePathResolver(Path.Combine(appDataPath, "packages"));
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
                ISet<SourcePackageDependencyInfo> availablePackages)
        {
            if (availablePackages.Contains(package)) return;

            foreach (var sourceRepository in repositories)
            {
                var dependencyInfoResource = await sourceRepository.GetResourceAsync<DependencyInfoResource>();
                var dependencyInfo = await dependencyInfoResource.ResolvePackage(
                    package, framework, cacheContext, logger, CancellationToken.None);

                if (dependencyInfo == null) continue;

                availablePackages.Add(dependencyInfo);
                foreach (var dependency in dependencyInfo.Dependencies)
                {
                    await GetPackageDependencies(
                        new PackageIdentity(dependency.Id, dependency.VersionRange.MinVersion),
                        framework, cacheContext, logger, repositories, availablePackages);
                }
            }
        }

        public static List<string> LoadPackageAssemblies(string configPath, bool throwException = false)
        {
            List<string> assemblyPaths = new List<string>();
            var dependencies = JsonConvert.DeserializeObject<Project>(File.ReadAllText(configPath)).Dependencies;

            string appDataPath = new DirectoryInfo(EnvironmentSettings.GetEnvironmentVariable()).Parent.FullName;
            string packagePath = Path.Combine(appDataPath, "packages");
            var packagePathResolver = new PackagePathResolver(packagePath);

            var nuGetFramework = NuGetFramework.ParseFolder("net48");
            var settings = Settings.LoadDefaultSettings(root: null);

            var sourceRepositoryProvider = new SourceRepositoryProvider(new PackageSourceProvider(settings), Repository.Provider.GetCoreV3());
            var localRepo = sourceRepositoryProvider.CreateRepository(new PackageSource(packagePath, "Local OpenBots Repo", true));
            
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
                                    if (!assemblyPaths.Contains(Path.Combine(packagePath, $"{packageToInstall.Id}.{packageToInstall.Version}", path)))
                                        assemblyPaths.Add(Path.Combine(packagePath, $"{packageToInstall.Id}.{packageToInstall.Version}", path));
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
                        MessageBox.Show($"Unable to load {packagePath}\\{dependency.Key}.{dependency.Value}. " +
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
    }
}