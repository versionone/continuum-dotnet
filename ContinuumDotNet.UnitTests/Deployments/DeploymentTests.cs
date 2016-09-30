using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ContinuumDotNet.Deployments;
using ContinuumDotNet.Exceptions;
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
       
        [TestMethod]
        public void GettingAListOfDeploymentsByServerReturnsFilteredList()
        {
            var _cacheConnectionMock = new Mock<ICacheConnection>();
            const string  TEST_SERVER = "SVR_V1HOSTDEV01";
            _cacheConnectionMock.Setup(a => a.GetList(TEST_SERVER))
                .Returns(new List<string>() {"ABCDE", "DEFIJ"});
            var deploymentManager = new DeploymentManager(_cacheConnectionMock.Object);
            var sites = deploymentManager.WithServer("V1HOSTDEV01").GetAll();
            Assert.IsTrue(sites.FirstOrDefault(s => s.FlightCode == "ABCDE") != null);
        }

        [TestMethod]
        public void GettingADeploymentByFlightCodeReturnsFilteredList()
        {
            var _cacheConnectionMock = new Mock<ICacheConnection>();
            const string TEST_FLIGHT_CODE = "FC_ABCDE";
            const string TEST_SERVER  = "SVR_V1HOSTDEV01";
            _cacheConnectionMock.Setup(a => a.GetList(TEST_FLIGHT_CODE))
                .Returns(new List<string>() { "V1HOSTDEV01:Ultimate", "V1HOSTDEV02:Enterprise" });
            _cacheConnectionMock.Setup(a => a.GetList(TEST_SERVER))
                .Returns(new List<string>() { "ABCDE", "FGHIJ" });
            var deploymentManager = new DeploymentManager(_cacheConnectionMock.Object);
            var sites = deploymentManager.WithFlightCode("ABCDE").GetAll();
            Assert.IsTrue(sites.FirstOrDefault(s => s.ServerName == "V1HOSTDEV01" && s.Product == "Ultimate") != null);
        }

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
            cacheConnectionMock.Verify(c => c.PushLeft(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            cacheConnectionMock.Verify(c => c.UpsertHash(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
