using System;
using System.Linq;
using System.Net;
using ContinuumDotNet.Exceptions;
using ContinuumDotNet.Exceptions.Flow;
using ContinuumDotNet.Flow.Artifacts;
using ContinuumDotNet.Flow.Pipelines;
using ContinuumDotNet.Interfaces.Connection;
using ContinuumDotNet.UnitTests.TestData.Flow.Artifacts;
using ContinuumDotNet.UnitTests.TestData.Flow.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace ContinuumDotNet.UnitTests.Flow.Artifacts
{
    /// <summary>
    /// Summary description for PipelineTests
    /// </summary>
    [TestClass]
    public class ArtifactTests
    {
        [TestMethod]
        public void GettingAnArtifactListMakeRequestOnce()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            var artifactManager = new ArtifactManager(continuumConnectionMock.Object);
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)200, Content = UltimateArtifactList.DATA });

            artifactManager.WithName("Ultimate").GetAll();
            continuumConnectionMock.Verify(a => a.MakeRequest(It.IsAny<IRestRequest>()), Times.Once);
        }

        [TestMethod]
        public void GettingAistOfArtifactsReturnsAList()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            var artifactManager = new ArtifactManager(continuumConnectionMock.Object);
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)200, Content = UltimateArtifactList.DATA });

            var artifacts = artifactManager.WithName("Ultimate").GetAll();
            Assert.AreEqual(6,artifacts.Count);
        }

        [TestMethod]
        public void GettingAistOfArtifactsDeserializesNameProperty()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            var artifactManager = new ArtifactManager(continuumConnectionMock.Object);
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)200, Content = UltimateArtifactList.DATA });

            var artifacts = artifactManager.WithName("Ultimate").GetAll();
            Assert.IsTrue(artifacts.FirstOrDefault(a=>a.Name.Equals("Ultimate"))!=null);
        }

        [TestMethod]
        public void GettingAistOfArtifactsDeserializesVersionProperty()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            var artifactManager = new ArtifactManager(continuumConnectionMock.Object);
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)200, Content = UltimateArtifactList.DATA });

            var artifacts = artifactManager.WithName("Ultimate").GetAll();
            Assert.IsTrue(artifacts.FirstOrDefault(a => a.Version.Equals("16.3.0.6")) != null);
        }

        [TestMethod]
        public void GettingAistOfArtifactsDeserializesRevisionProperty()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            var artifactManager = new ArtifactManager(continuumConnectionMock.Object);
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)200, Content = UltimateArtifactList.DATA });

            var artifacts = artifactManager.WithName("Ultimate").GetAll();
            Assert.IsTrue(artifacts.FirstOrDefault(a => a.Revision.Equals(5)) != null);
        }

        [TestMethod]
        public void GettingAistOfArtifactsDeserializesCreateDateProperty()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            var artifactManager = new ArtifactManager(continuumConnectionMock.Object);
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)200, Content = UltimateArtifactList.DATA });

            var artifacts = artifactManager.WithName("Ultimate").GetAll();
            Assert.IsTrue(artifacts.FirstOrDefault(a => a.CreationDateInUtc.Equals(DateTime.Parse("2016-09-27 7:21:58 PM"))) != null);
        }

        [TestMethod]
        public void GettingAistOfArtifactsDeserializesIdProperty()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            var artifactManager = new ArtifactManager(continuumConnectionMock.Object);
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)200, Content = UltimateArtifactList.DATA });

            var artifacts = artifactManager.WithName("Ultimate").GetAll();
            Assert.IsTrue(artifacts.FirstOrDefault(a => a.Id.Equals("57eaae124975e90094e18e69")) != null);
        }

    }
}
