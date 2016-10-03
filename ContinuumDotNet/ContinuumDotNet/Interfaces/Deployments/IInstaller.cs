using System.Collections.Generic;

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
        string LicenseFilename { get; }

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
        IInstaller WithInstallerRepositoryName(string installerRepositoryName);
        IInstaller WithBuildSupportFilesRepositoryName(string buildSupportFilesRepositoryName);
        IInstaller WithLicenseFilename(string licenseFilename);
        IInstaller WithInstallerFilename(string installerFilename);

        IInstaller WithInstance(string instance);
        IInstaller WithAdditionalParameter(string parameterName, string value);
    }
}