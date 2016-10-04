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
            const string instanceName = "testInstance";
            var lifecycleInstaller = new LifecycleInstaller().WithInstance(instanceName);
            Assert.AreEqual(instanceName, lifecycleInstaller.Instance);
        }

        [TestMethod]
        public void CallingWithProductSetsProduct()
        {
            const string productName = "testProduct";
            var lifecycleInstaller = new LifecycleInstaller().WithProduct(productName);
            Assert.AreEqual(productName, lifecycleInstaller.Product);
        }

        [TestMethod]
        public void CallingWithVersionSetsVersion()
        {
            const string version = "testVersion";
            var lifecycleInstaller = new LifecycleInstaller().WithVersion(version);
            Assert.AreEqual(version, lifecycleInstaller.Version);
        }

        [TestMethod]
        public void CallingWithServerNameSetsServerName()
        {
            const string serverName = "testserver";
            var lifecycleInstaller = new LifecycleInstaller().WithServerName(serverName);
            Assert.AreEqual(serverName, lifecycleInstaller.ServerName);
        }

        [TestMethod]
        public void CallingWithFlightCodeSetsFlightCode()
        {
            const string flightCode = "flightcode";
            var lifecycleInstaller = new LifecycleInstaller().WithFlightCode(flightCode);
            Assert.AreEqual(flightCode, lifecycleInstaller.FlightCode);
        }

        [TestMethod]
        public void CallingWithTemporaryWorkFolderSetsTemporaryWorkFolder()
        {
            const string temporaryWorkFolder = @"c:\temp";
            var lifecycleInstaller = new LifecycleInstaller().WithTemporaryWorkFolder(temporaryWorkFolder);
            Assert.AreEqual(temporaryWorkFolder, lifecycleInstaller.TemporaryWorkFolder);
        }

        [TestMethod]
        public void CallingWithBaseArtifactoryUrlSetsWithBaseArtifactoryUrl()
        {
            const string baseUrl = @"http://url";
            var lifecycleInstaller = new LifecycleInstaller().WithBaseArtifactoryUrl(baseUrl);
            Assert.AreEqual(baseUrl, lifecycleInstaller.BaseArtifactoryUrl);
        }

        [TestMethod]
        [ExpectedException(typeof (MissingOrInvalidUrlException))]
        public void CallingWithBaseArtifactoryUrlWithABadUrlThrowsMissingOrInvalidUrlException()
        {
            const string baseUrl = @"httpThisshouldnotwork";
            var lifecycleInstaller = new LifecycleInstaller().WithBaseArtifactoryUrl(baseUrl);
            Assert.AreEqual(baseUrl, lifecycleInstaller.BaseArtifactoryUrl);
        }

        [TestMethod]
        public void CallingWithDemoDataRepositoryNameSetsDemoDataRepositoryName()
        {
            const string demoDataRepositoryName = @"demo-data-repo";
            var lifecycleInstaller = new LifecycleInstaller().WithDemoDataRepositoryName(demoDataRepositoryName);
            Assert.AreEqual(demoDataRepositoryName, lifecycleInstaller.DemoDataRepositoryName);
        }

        [TestMethod]
        public void CallingWithInstallerRepositoryNameSetsInstallerRepositoryName()
        {
            const string installerRepositoryName = @"installer-repo";
            var lifecycleInstaller = new LifecycleInstaller().WithInstallerRepositoryName(installerRepositoryName);
            Assert.AreEqual(installerRepositoryName, lifecycleInstaller.InstallerRepositoryName);
        }

        [TestMethod]
        public void CallingWithBuildSupportFilesRepositoryNameSetsBuildSupprtFilesRepositoryName()
        {
            const string buildSupportFilesRepositoryName = @"build-support-repo";
            var lifecycleInstaller = new LifecycleInstaller().WithBuildSupportFilesRepositoryName(buildSupportFilesRepositoryName);
            Assert.AreEqual(buildSupportFilesRepositoryName, lifecycleInstaller.BuildSupportFilesRepositoryName);
        }

        [TestMethod]
        public void CallingWithLicenseFilenameSetsLicenseFilename()
        {
            const string licenseFilename = @"license";
            var lifecycleInstaller = new LifecycleInstaller().WithLicenseFilename(licenseFilename);
            Assert.AreEqual(licenseFilename, lifecycleInstaller.LicenseFilename);
        }

        [TestMethod]
        public void CallingWithInstallerFilenameSetsInstallerFilename()
        {
            const string installerFilename = "filename";
            var lifecycleInstaller = new LifecycleInstaller().WithInstallerFilename(installerFilename);
            Assert.AreEqual(installerFilename, lifecycleInstaller.InstallerFilename);
        }

        [TestMethod]
        public void CallingWithDemoDataFilenameSetsDemoDataFilename()
        {
            const string demoDataFilename = "filename";
            var lifecycleInstaller = new LifecycleInstaller().WithDemoDataFilename(demoDataFilename);
            Assert.AreEqual(demoDataFilename, lifecycleInstaller.DemoDataFilename);
        }

        [TestMethod]
        public void CanSetUserConfigFolderName()
        {
            const string userConfigFolderName = "userconfig";
            var lifecycleInstaller = new LifecycleInstaller().WithUserConfigFolderName(userConfigFolderName);
            Assert.AreEqual(userConfigFolderName, lifecycleInstaller.UserConfigFolderName);
        }

        [TestMethod]
        public void CanSetUserConfigFilename()
        {
            const string userConfigFilename = "userconfigfilename";
            var lifecycleInstaller = new LifecycleInstaller().WithUserConfigFileName(userConfigFilename);
            Assert.AreEqual(userConfigFilename, lifecycleInstaller.UserConfigFileName);
        }

        [TestMethod]
        public void CanSetDemoDataRepositoryFolderName()
        {
            const string demoDataRepositoryFolderName = "demodatafolder";
            var lifecycleInstaller = new LifecycleInstaller().WithDemoDataRepositoryFolderName(demoDataRepositoryFolderName);
            Assert.AreEqual(demoDataRepositoryFolderName, lifecycleInstaller.DemoDataRepositoryFolderName);
        }

        [TestMethod]
        public void CanSetConfigRepositoryFolderName()
        {
            var configRepositoryFolderName = Guid.NewGuid().ToString();
            var lifecycleInstaller = new LifecycleInstaller().WithConfigRepositoryFolderName(configRepositoryFolderName);
            Assert.AreEqual(configRepositoryFolderName, lifecycleInstaller.ConfigRepositoryFolderName);
        }

        [TestMethod]
        public void CanSetLicenseRepositoryFolderName()
        {
            var licenseRepositoryFolderName = Guid.NewGuid().ToString();;
            var lifecycleInstaller = new LifecycleInstaller().WithLicenseRespositoryFolderName(licenseRepositoryFolderName);
            Assert.AreEqual(licenseRepositoryFolderName, lifecycleInstaller.LicenseRespositoryFolderName);
        }

        [TestMethod]
        public void CanSetInstallersRepositoryFolderName()
        {
            var installersRepositoryFolderName = Guid.NewGuid().ToString(); ;
            var lifecycleInstaller = new LifecycleInstaller().WithInstallersRepositoryFolderName(installersRepositoryFolderName);
            Assert.AreEqual(installersRepositoryFolderName, lifecycleInstaller.InstallersRepositoryFolderName);
        }


        [TestMethod]
        public void CanAddAParameter()
        {
            var parameter = "parameter";
            var value = "value";
            var lifecycleInstaller = new LifecycleInstaller().WithAdditionalParameter(parameter, value);
            Assert.IsTrue(lifecycleInstaller.AdditionalParameters.ContainsKey(parameter) &&
                          lifecycleInstaller.AdditionalParameters.Count == 1 &&
                          lifecycleInstaller.AdditionalParameters[parameter].Equals(value));;
        }

        [TestMethod]
        public void AddingAParameterTwiceOverwritesPreviousValue()
        {
            var parameter = "parameter";
            var oldValue = "vaoldlue";
            var newValue = "newvalue";
            var lifecycleInstaller = new LifecycleInstaller().WithAdditionalParameter(parameter, oldValue);
            lifecycleInstaller.WithAdditionalParameter(parameter, newValue);
            Assert.IsTrue(lifecycleInstaller.AdditionalParameters.ContainsKey(parameter) &&
                          lifecycleInstaller.AdditionalParameters.Count == 1 &&
                          lifecycleInstaller.AdditionalParameters[parameter].Equals(newValue)); ;
        }
    }
}
