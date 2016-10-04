using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Exceptions.Connection;
using ContinuumDotNet.Interfaces.Deployments;
using ContinuumDotNet.Interfaces.Utilities;
using ContinuumDotNet.Utilities;
using ContinuumDotNet.Utilities.Extensions;

namespace ContinuumDotNet.Deployments.Installers
{
    public class BaseInstaller : IInstaller
    {
        private string _instance;
        private string _product;
        private string _serverName;
        private string _version;
        private string _flightCode;
        private string _temporaryWorkFolder;
        private string _baseArtifactoryUrl;
        private string _demoDataRepositoryName;
        private string _installerRepositoryName;
        private string _buildSupportFilesRepositoryName;
        private string _licenseFilename;
        private string _installerFilename;
        private string _demoDataFilename;
        private string _userConfigFileName;
        private string _configRepositoryFolderName;
        private string _licenseRespositoryFolder;
        private string _installersRepositoryFolder;
        private string _userConfigFolderName;
        private string _demoDataRepositoryFolderName;
        private readonly IRemotePsRunner _psRunner;


        public BaseInstaller(IRemotePsRunner remotePsRunner)
        {
            _temporaryWorkFolder = Path.Combine("c:\\deployments", Guid.NewGuid().ToString());
            _psRunner = remotePsRunner;
        }

        public string DemoDataFilename
        {
            get { return _demoDataFilename; }
        }

        public string UserConfigFileName
        {
            get { return _userConfigFileName; }
        }

        public string UserConfigFolderName
        {
            get { return _userConfigFolderName; }
        }

        public string DemoDataRepositoryFolderName
        {
            get { return _demoDataRepositoryFolderName; }
        }

        public IRemotePsRunner PsRunner
        {
            get { return _psRunner; }
        }

        public string FlightCode
        {
            get { return _flightCode; }
        }

        public string ServerName
        {
            get { return _serverName; }
        }

        public string TemporaryWorkFolder
        {
            get { return _temporaryWorkFolder; }
        }

        public string BaseArtifactoryUrl
        {
            get { return _baseArtifactoryUrl; }
        }

        public string DemoDataRepositoryName
        {
            get { return _demoDataRepositoryName; }
        }

        public string InstallerRepositoryName
        {
            get { return _installerRepositoryName; }
        }

        public string BuildSupportFilesRepositoryName
        {
            get { return _buildSupportFilesRepositoryName; }
        }

        public string ConfigRepositoryFolderName
        {
            get { return _configRepositoryFolderName; }
        }

        public string LicenseRespositoryFolderName
        {
            get { return _licenseRespositoryFolder; }
        }

        public string InstallersRepositoryFolderName
        {
            get { return _installersRepositoryFolder; }
        }

        public string LicenseFilename
        {
            get { return _licenseFilename; }
        }

        public string InstallerFilename
        {
            get { return _installerFilename; }
        }

        public string Version
        {
            get { return _version; }
        }

        public string Instance => _instance;

        public string Product
        {
            get { return _product; }
        }

        public IInstaller WithTemporaryWorkFolder(string temporaryWorkFolder)
        {
            _temporaryWorkFolder = temporaryWorkFolder;
            return this;
        }

        public IInstaller WithBaseArtifactoryUrl(string baseArtifactoryUrl)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(baseArtifactoryUrl, UriKind.Absolute, out uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (!result)
            {
                throw new MissingOrInvalidUrlException(baseArtifactoryUrl);
            }
            _baseArtifactoryUrl = baseArtifactoryUrl;
            return this;
        }

        public IInstaller WithDemoDataRepositoryName(string demoDataRepositoryName)
        {
            _demoDataRepositoryName = demoDataRepositoryName;
            return this;
        }

        public IInstaller WithDemoDataRepositoryFolderName(string demoDataRepositoryFolderName)
        {
            _demoDataRepositoryFolderName = demoDataRepositoryFolderName;
            return this;
        }

        public IInstaller WithInstallerRepositoryName(string installerRepositoryName)
        {
            _installerRepositoryName = installerRepositoryName;
            return this;
        }

        public IInstaller WithBuildSupportFilesRepositoryName(string buildSupportFilesRepositoryName)
        {
            _buildSupportFilesRepositoryName = buildSupportFilesRepositoryName;
            return this;
        }

        public IInstaller WithLicenseFilename(string licenseFilename)
        {
            _licenseFilename = licenseFilename;
            return this;
        }

        public IInstaller WithInstallerFilename(string installerFilename)
        {
            _installerFilename = installerFilename;
            return this;
        }

        public IInstaller WithDemoDataFilename(string filename)
        {
            _demoDataFilename = filename;
            return this;
        }

        public IInstaller WithConfigRepositoryFolderName(string configRepositoryFolderName)
        {
            _configRepositoryFolderName = configRepositoryFolderName;
            return this;
        }

        public IInstaller WithLicenseRespositoryFolderName(string licenseRepositoryFolderName)
        {
            _licenseRespositoryFolder = licenseRepositoryFolderName;
            return this;
        }

        public IInstaller WithInstallersRepositoryFolderName(string installersRepositoryFolderName)
        {
            _installersRepositoryFolder = installersRepositoryFolderName;
            return this;
        }

        public IInstaller WithUserConfigFolderName(string userConfigFolderName)
        {
            _userConfigFolderName = userConfigFolderName;
            return this;
        }

        public IInstaller WithUserConfigFileName(string userConfigFilename)
        {
            _userConfigFileName = userConfigFilename;
            return this;
        }

        public IInstaller WithVersion(string version)
        {
            _version = version;
            return this;
        }

        public IInstaller WithProduct(string product)
        {
            _product = product;
            return this;
        }

        public IInstaller WithServerName(string serverName)
        {
            _serverName = serverName;
            return this;
        }

        public IInstaller WithFlightCode(string flightCode)
        {
            _flightCode = flightCode;
            return this;
        }

        public IInstaller WithInstance(string instance)
        {
            _instance = instance;
            return this;
        }

        public Dictionary<string, string> AdditionalParameters { get; set; }

        public IInstaller WithAdditionalParameter(string parameterName, string value)
        {
            if (AdditionalParameters == null)
            {
                AdditionalParameters = new Dictionary<string, string>();
            }
            if (AdditionalParameters.ContainsKey(parameterName))
            {
                AdditionalParameters[parameterName] = value;
            }
            else
            {
                AdditionalParameters.Add(parameterName, value);
            }
            return this;
        }

        public bool IsValidInstall()
        {
            throw new NotImplementedException();
        }

        public IDeployedSite Install()
        {
            try
            {
                SetupWorkspace();
                DownloadArtifacts();
                //DeployDemoData();
                //RunInstaller();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                CleanupWorkspace();
            }
            return new DeployedSite();
        }

        public IDeployedSite Uninstall()
        {
            throw new NotImplementedException();
        }

        public virtual void SetupWorkspace()
        {
            _psRunner.RunScript($"New-Item  -Force -ItemType directory -Path {TemporaryWorkFolder}");
        }

        protected string LicenceFileUrl
        {
            get
            {
                var licenseFileUrl = $"{BaseArtifactoryUrl}/{BuildSupportFilesRepositoryName}/{LicenseRespositoryFolderName}/{LicenseFilename}";
                return !licenseFileUrl.IsValidUrl() ? string.Empty : licenseFileUrl;
            }
        }

        protected string DemoDataFileUrl
        {
            get
            {
                var url = $"{BaseArtifactoryUrl}/{DemoDataRepositoryName}/{DemoDataRepositoryFolderName}/{_demoDataFilename}";
                return !url.IsValidUrl() ? string.Empty : url;
            }
        }

        protected string InstallerUrl
        {
            get
            {
                var url = $"{BaseArtifactoryUrl}/{InstallerRepositoryName}/{InstallersRepositoryFolderName}/{_installerFilename}";
                return !url.IsValidUrl() ? string.Empty : url;
            }
        }

        protected string UserConfigUrl
        {
            get
            {
                var url = $"{BaseArtifactoryUrl}/{BuildSupportFilesRepositoryName}/{ConfigRepositoryFolderName}/{_userConfigFileName}";
                return !url.IsValidUrl() ? string.Empty : url;
            }
        }

        public virtual void DownloadArtifacts()
        {
            var scriptStringBuilder = new StringBuilder();
            var webClientName = "$client";
            scriptStringBuilder.AppendLine($"{webClientName} = New-Object System.Net.WebClient");
            if (LicenceFileUrl.IsValidUrl())
            {
                scriptStringBuilder.AppendLine(RemotePsRunner.CreateDownloadCommand(webClientName, LicenceFileUrl,
                    Path.Combine(TemporaryWorkFolder, _licenseFilename)));
            }

            if (DemoDataFileUrl.IsValidUrl())
            {
                scriptStringBuilder.AppendLine(RemotePsRunner.CreateDownloadCommand(webClientName, DemoDataFileUrl,
                    Path.Combine(TemporaryWorkFolder, _demoDataFilename)));
            }

            if (InstallerUrl.IsValidUrl())
            {
                scriptStringBuilder.AppendLine(RemotePsRunner.CreateDownloadCommand(webClientName, InstallerUrl,
                    Path.Combine(TemporaryWorkFolder, _installerFilename)));
            }

            if (UserConfigUrl.IsValidUrl())
            {
                scriptStringBuilder.AppendLine(RemotePsRunner.CreateDownloadCommand(webClientName, UserConfigUrl,
                    Path.Combine(TemporaryWorkFolder, _userConfigFileName)));
            }

            _psRunner.RunScript(scriptStringBuilder.ToString());

        }

        public virtual void DownloadBuildSupportFiles()
        {
        }

        public virtual void DeployDemoData()
        {
        }

        public virtual void RunInstaller()
        {
        }

        public virtual void CleanupWorkspace()
        {
            _psRunner.RunScript($"Remove-Item {TemporaryWorkFolder} -Recurse -Force");
        }
    }
}
