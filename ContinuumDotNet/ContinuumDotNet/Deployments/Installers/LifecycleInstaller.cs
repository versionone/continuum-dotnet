using System;
using System.Collections.Generic;
using ContinuumDotNet.Exceptions.Connection;
using ContinuumDotNet.Interfaces.Deployments;

namespace ContinuumDotNet.Deployments.Installers
{
    public class LifecycleInstaller : IInstaller
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
            if (AdditionalParameters == null) {  AdditionalParameters = new Dictionary<string, string>();}
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
    }
}
