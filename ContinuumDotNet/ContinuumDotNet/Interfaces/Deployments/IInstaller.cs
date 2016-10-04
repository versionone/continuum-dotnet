using System.Collections.Generic;
using ContinuumDotNet.Interfaces.Utilities;

namespace ContinuumDotNet.Interfaces.Deployments
{
    public interface IInstaller
    {
        string ServerName { get; }
        string Version { get; }
        string Product { get; }
        string TemporaryWorkFolder { get; }
        string BaseArtifactoryUrl { get; }
        string DemoDataRepositoryName { get; }
        string InstallerRepositoryName { get; }
        string BuildSupportFilesRepositoryName { get; }
        string ConfigRepositoryFolderName { get; }
        string LicenseRespositoryFolderName { get; }
        string InstallersRepositoryFolderName { get; }
        string LicenseFilename { get; }
        string DemoDataFilename { get; }
        string UserConfigFileName { get; }
        string UserConfigFolderName { get; }
        string DemoDataRepositoryFolderName { get; }

        IRemotePsRunner PsRunner { get; }

        string FlightCode { get; }

        string Instance { get; }

        string InstallerFilename { get; }
        Dictionary<string, string> AdditionalParameters { get; }

        IInstaller WithServerName(string serverName);
        IInstaller WithFlightCode(string flightCode);
        IInstaller WithVersion(string version);
        IInstaller WithProduct(string product);
        IInstaller WithTemporaryWorkFolder(string temporaryWorkFolder);
        IInstaller WithBaseArtifactoryUrl(string baseArtifactoryUrl);
        IInstaller WithDemoDataRepositoryName(string demoDataRepositoryName);
        IInstaller WithDemoDataRepositoryFolderName(string demoDataRepositoryFolderName);
        IInstaller WithInstallerRepositoryName(string installerRepositoryName);
        IInstaller WithBuildSupportFilesRepositoryName(string buildSupportFilesRepositoryName);
        IInstaller WithLicenseFilename(string licenseFilename);
        IInstaller WithInstallerFilename(string installerFilename);

        IInstaller WithDemoDataFilename(string filename);
        IInstaller WithConfigRepositoryFolderName(string configRepositoryFolderName);
        IInstaller WithLicenseRespositoryFolderName(string licenseRepositoryFolderName);
        IInstaller WithInstallersRepositoryFolderName (string installersRepositoryFolderName);
        IInstaller WithUserConfigFolderName(string userConfigFolderName);
        IInstaller WithUserConfigFileName (string userConfigFilename);

        IInstaller WithInstance(string instance);
        IInstaller WithAdditionalParameter(string parameterName, string value);

        bool IsValidInstall();
        IDeployedSite Install();
        IDeployedSite Uninstall();

        void SetupWorkspace();
        void DownloadArtifacts();
        void DownloadBuildSupportFiles();
        void DeployDemoData();
        void RunInstaller();
        void CleanupWorkspace();
    }
}