using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ContinuumDotNet.Deployments;
using ContinuumDotNet.Deployments.Installers;
using ContinuumDotNet.Exceptions;
using ContinuumDotNet.Exceptions.Connection;
using ContinuumDotNet.Exceptions.Flow;
using ContinuumDotNet.Flow.Pipelines;
using ContinuumDotNet.Interfaces.Connection;
using ContinuumDotNet.Interfaces.Utilities;
using ContinuumDotNet.UnitTests.TestData.Flow.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace ContinuumDotNet.UnitTests.Deployments
{
    [TestClass]
    public class DeploymentTests
    {
       
        //[TestMethod]
        //public void GettingAListOfDeploymentsByServerReturnsFilteredList()
        //{
        //    var _cacheConnectionMock = new Mock<ICacheConnection>();
        //    const string  TEST_SERVER = "SVR_V1HOSTDEV01";
        //    _cacheConnectionMock.Setup(a => a.GetList(TEST_SERVER))
        //        .Returns(new List<string>() {"ABCDE", "DEFIJ"});
        //    var deploymentManager = new DeploymentManager(_cacheConnectionMock.Object);
        //    var sites = deploymentManager.WithServer("V1HOSTDEV01").GetAll();
        //    Assert.IsTrue(sites.FirstOrDefault(s => s.FlightCode == "ABCDE") != null);
        //}

        [TestMethod]
        public void GettingADeploymentByEmptyFlightCodeReturnsEmptyList()
        {
            var _cacheConnectionMock = new Mock<ICacheConnection>();
            _cacheConnectionMock.Setup(a => a.GetList(It.IsAny<string>()))
                .Returns(new List<string>());
            var deploymentManager = new DeploymentManager(_cacheConnectionMock.Object);
            var sites = deploymentManager.WithFlightCode("ABCDE").GetAll();
            Assert.IsTrue(sites.Count == 0);
        }

        [TestMethod]
        public void GettingADeploymentByEmptyServerNameReturnsEmptyList()
        {
            var _cacheConnectionMock = new Mock<ICacheConnection>();
            _cacheConnectionMock.Setup(a => a.GetList(It.IsAny<string>()))
                .Returns(new List<string>());
            var deploymentManager = new DeploymentManager(_cacheConnectionMock.Object);
            var sites = deploymentManager.WithServer("ABCDE").GetAll();
            Assert.IsTrue(sites.Count == 0);
        }

        [TestMethod]
        public void AddingAnItemToAListDoesNotAddDuplicateIfAlreadyExists()
        {
            var _cacheConnectionMock = new Mock<ICacheConnection>();
            _cacheConnectionMock.Setup(a => a.GetList(It.IsAny<string>()))
                .Returns(new List<string>());
            var deploymentManager = new DeploymentManager(_cacheConnectionMock.Object);
            var sites = deploymentManager.WithServer("ABCDE").GetAll();
            var deployedSite = new DeployedSite();
            Assert.IsTrue(sites.Count == 0);
        }

        [TestMethod]
        public void GettingASiteByFlightCodeHasCacheConnection()
        {
            var _cacheConnectionMock = new Mock<ICacheConnection>();
            _cacheConnectionMock.Setup(a => a.GetList(It.IsAny<string>()))
                .Returns(new List<string>());
            var deploymentManager = new DeploymentManager(_cacheConnectionMock.Object);
            var sites = deploymentManager.WithServer("ABCDE").GetAll();
            var deployedSite = new DeployedSite();
            Assert.IsTrue(sites.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingParameterException))]
        public void CreatingANewDeployedSiteWithoutServerNameThrowsMissingParameterException()
        {
            var cacheConnectionMock = new Mock<ICacheConnection>();
            var deploymentManager = new DeploymentManager(cacheConnectionMock.Object);
            deploymentManager.CreateDeployedSite()
                .WithProduct("Ultimate")
                .WithFlightCode("ServerName")
                .WithVersion("1.0.0.0")
                .Deploy();
        }

        [TestMethod]
        [ExpectedException(typeof(MissingParameterException))]
        public void CreatingANewDeployedSiteWithoutFlightCodeThrowsMissingParameterException()
        {
            var cacheConnectionMock = new Mock<ICacheConnection>();
            var deploymentManager = new DeploymentManager(cacheConnectionMock.Object);
            deploymentManager.CreateDeployedSite()
                .WithProduct("Ultimate")
                .WithServerName("ServerName")
                .WithVersion("1.0.0.0")
                .Deploy();
        }

        [TestMethod]
        [ExpectedException(typeof(MissingParameterException))]
        public void CreatingANewDeployedSiteWithoutVersionThrowsMissingParameterException()
        {
            var cacheConnectionMock = new Mock<ICacheConnection>();
            var deploymentManager = new DeploymentManager(cacheConnectionMock.Object);
            deploymentManager.CreateDeployedSite()
                .WithProduct("Ultimate")
                .WithServerName("ServerName")
                .WithFlightCode("1.0.0.0")
                .Deploy();
        }

        [TestMethod]
        [ExpectedException(typeof(MissingParameterException))]
        public void CreatingANewDeployedSiteWithoutProductThrowsMissingParameterException()
        {
            var cacheConnectionMock = new Mock<ICacheConnection>();
            var deploymentManager = new DeploymentManager(cacheConnectionMock.Object);
            deploymentManager.CreateDeployedSite()
                .WithServerName("Ultimate")
                .WithServerName("ServerName")
                .WithFlightCode("1.0.0.0")
                .Deploy();
        }

        [TestMethod]
        public void DeployingASiteUpdatesCache()
        {
            var cacheConnectionMock = new Mock<ICacheConnection>();
            var deploymentManager = new DeploymentManager(cacheConnectionMock.Object);
            deploymentManager.CreateDeployedSite()
                .WithServerName("Server")
                .WithVersion("1.0.0.0")
                .WithFlightCode("ABCDE")
                .WithProduct("Ultimate")
                .Deploy();
            cacheConnectionMock.Verify(c => c.PushLeft(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            cacheConnectionMock.Verify(c => c.UpsertHash(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        }

        [TestMethod]
        public void CallingWithBranchNameUpdatesBranchName()
        {
            var branchName = "MYBRANCH";
            var cacheConnectionMock = new Mock<ICacheConnection>();
            var deploymentManager = new DeploymentManager(cacheConnectionMock.Object);
            var site = deploymentManager.CreateDeployedSite().WithBranchName(branchName);
            Assert.AreEqual(branchName, site.BranchName);
        }

        [TestMethod]
        public void PassingCreationDateInUtcUpdatesCreationDate()
        {
            var cacheConnectionMock = new Mock<ICacheConnection>();
            var deploymentManager = new DeploymentManager(cacheConnectionMock.Object);
            var dateTimeNowInUtc = DateTime.UtcNow;
            var site = deploymentManager.CreateDeployedSite().WithCreationDateInUtc(dateTimeNowInUtc);
            Assert.AreEqual(dateTimeNowInUtc, site.CreationDateInUtc);
        }

        [TestMethod]
        public void PassingNoCreationDateUpdatesToNow()
        {
            var cacheConnectionMock = new Mock<ICacheConnection>();
            var deploymentManager = new DeploymentManager(cacheConnectionMock.Object);
            var site = deploymentManager.CreateDeployedSite();
            Assert.AreEqual(site.CreationDateInUtc.Date, DateTime.UtcNow.Date);
        }

        [TestMethod]
        public void CallingWithInstanceSetsInstance()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string instanceName = "testInstance";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithInstance(instanceName);
            Assert.AreEqual(instanceName, lifecycleInstaller.Instance);
        }

        [TestMethod]
        public void CallingWithProductSetsProduct()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string productName = "testProduct";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithProduct(productName);
            Assert.AreEqual(productName, lifecycleInstaller.Product);
        }

        [TestMethod]
        public void CallingWithVersionSetsVersion()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string version = "testVersion";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithVersion(version);
            Assert.AreEqual(version, lifecycleInstaller.Version);
        }

        [TestMethod]
        public void CallingWithServerNameSetsServerName()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string serverName = "testserver";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithServerName(serverName);
            Assert.AreEqual(serverName, lifecycleInstaller.ServerName);
        }

        [TestMethod]
        public void CallingWithFlightCodeSetsFlightCode()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string flightCode = "flightcode";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithFlightCode(flightCode);
            Assert.AreEqual(flightCode, lifecycleInstaller.FlightCode);
        }

        [TestMethod]
        public void CallingWithTemporaryWorkFolderSetsTemporaryWorkFolder()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string temporaryWorkFolder = @"c:\temp";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithTemporaryWorkFolder(temporaryWorkFolder);
            Assert.AreEqual(temporaryWorkFolder, lifecycleInstaller.TemporaryWorkFolder);
        }

        [TestMethod]
        public void CallingWithBaseArtifactoryUrlSetsWithBaseArtifactoryUrl()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string baseUrl = @"http://url";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithBaseArtifactoryUrl(baseUrl);
            Assert.AreEqual(baseUrl, lifecycleInstaller.BaseArtifactoryUrl);
        }

        [TestMethod]
        [ExpectedException(typeof (MissingOrInvalidUrlException))]
        public void CallingWithBaseArtifactoryUrlWithABadUrlThrowsMissingOrInvalidUrlException()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string baseUrl = @"httpThisshouldnotwork";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithBaseArtifactoryUrl(baseUrl);
            Assert.AreEqual(baseUrl, lifecycleInstaller.BaseArtifactoryUrl);
        }

        [TestMethod]
        public void CallingWithDemoDataRepositoryNameSetsDemoDataRepositoryName()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string demoDataRepositoryName = @"demo-data-repo";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithDemoDataRepositoryName(demoDataRepositoryName);
            Assert.AreEqual(demoDataRepositoryName, lifecycleInstaller.DemoDataRepositoryName);
        }

        [TestMethod]
        public void CallingWithInstallerRepositoryNameSetsInstallerRepositoryName()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string installerRepositoryName = @"installer-repo";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithInstallerRepositoryName(installerRepositoryName);
            Assert.AreEqual(installerRepositoryName, lifecycleInstaller.InstallerRepositoryName);
        }

        [TestMethod]
        public void CallingWithBuildSupportFilesRepositoryNameSetsBuildSupprtFilesRepositoryName()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string buildSupportFilesRepositoryName = @"build-support-repo";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithBuildSupportFilesRepositoryName(buildSupportFilesRepositoryName);
            Assert.AreEqual(buildSupportFilesRepositoryName, lifecycleInstaller.BuildSupportFilesRepositoryName);
        }

        [TestMethod]
        public void CallingWithLicenseFilenameSetsLicenseFilename()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string licenseFilename = @"license";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithLicenseFilename(licenseFilename);
            Assert.AreEqual(licenseFilename, lifecycleInstaller.LicenseFilename);
        }

        [TestMethod]
        public void CallingWithInstallerFilenameSetsInstallerFilename()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string installerFilename = "filename";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithInstallerFilename(installerFilename);
            Assert.AreEqual(installerFilename, lifecycleInstaller.InstallerFilename);
        }

        [TestMethod]
        public void CallingWithDemoDataFilenameSetsDemoDataFilename()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string demoDataFilename = "filename";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithDemoDataFilename(demoDataFilename);
            Assert.AreEqual(demoDataFilename, lifecycleInstaller.DemoDataFilename);
        }

        [TestMethod]
        public void CanSetUserConfigFolderName()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string userConfigFolderName = "userconfig";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithUserConfigFolderName(userConfigFolderName);
            Assert.AreEqual(userConfigFolderName, lifecycleInstaller.UserConfigFolderName);
        }

        [TestMethod]
        public void CanSetUserConfigFilename()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string userConfigFilename = "userconfigfilename";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithUserConfigFileName(userConfigFilename);
            Assert.AreEqual(userConfigFilename, lifecycleInstaller.UserConfigFileName);
        }

        [TestMethod]
        public void CanSetDemoDataRepositoryFolderName()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            const string demoDataRepositoryFolderName = "demodatafolder";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithDemoDataRepositoryFolderName(demoDataRepositoryFolderName);
            Assert.AreEqual(demoDataRepositoryFolderName, lifecycleInstaller.DemoDataRepositoryFolderName);
        }

        [TestMethod]
        public void CanSetConfigRepositoryFolderName()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            var configRepositoryFolderName = Guid.NewGuid().ToString();
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithConfigRepositoryFolderName(configRepositoryFolderName);
            Assert.AreEqual(configRepositoryFolderName, lifecycleInstaller.ConfigRepositoryFolderName);
        }

        [TestMethod]
        public void CanSetLicenseRepositoryFolderName()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            var licenseRepositoryFolderName = Guid.NewGuid().ToString();;
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithLicenseRespositoryFolderName(licenseRepositoryFolderName);
            Assert.AreEqual(licenseRepositoryFolderName, lifecycleInstaller.LicenseRespositoryFolderName);
        }

        [TestMethod]
        public void CanSetInstallersRepositoryFolderName()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            var installersRepositoryFolderName = Guid.NewGuid().ToString(); ;
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithInstallersRepositoryFolderName(installersRepositoryFolderName);
            Assert.AreEqual(installersRepositoryFolderName, lifecycleInstaller.InstallersRepositoryFolderName);
        }


        [TestMethod]
        public void CanAddAParameter()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            var parameter = "parameter";
            var value = "value";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithAdditionalParameter(parameter, value);
            Assert.IsTrue(lifecycleInstaller.AdditionalParameters.ContainsKey(parameter) &&
                          lifecycleInstaller.AdditionalParameters.Count == 1 &&
                          lifecycleInstaller.AdditionalParameters[parameter].Equals(value));;
        }

        [TestMethod]
        public void AddingAParameterTwiceOverwritesPreviousValue()
        {
            var remotePsRunnerMock = new Mock<IRemotePsRunner>();
            var parameter = "parameter";
            var oldValue = "vaoldlue";
            var newValue = "newvalue";
            var lifecycleInstaller = new LifecycleInstaller(remotePsRunnerMock.Object).WithAdditionalParameter(parameter, oldValue);
            lifecycleInstaller.WithAdditionalParameter(parameter, newValue);
            Assert.IsTrue(lifecycleInstaller.AdditionalParameters.ContainsKey(parameter) &&
                          lifecycleInstaller.AdditionalParameters.Count == 1 &&
                          lifecycleInstaller.AdditionalParameters[parameter].Equals(newValue)); ;
        }
    }
}
