using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Connection;
using ContinuumDotNet.Exceptions.Connection;
using ContinuumDotNet.Flow.Pipelines;
using ContinuumDotNet.Interfaces.Connection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using StructureMap;

namespace ContinuumDotNet.UnitTests.Connection
{
    [TestClass]
    public class ConnectionTests
    {
        Mock<IRestRequest> _moqRestRequest = new Mock<IRestRequest>();
        private string _token="test-token";

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        [ExpectedException(typeof(MissingUrlException))]
        public void MissingUrlThrowsError()
        {
            var restClient = new RestClient();
            var continuumConnection = new ContinuumConnection(restClient, _token);
            continuumConnection.MakeRequest(_moqRestRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingTokenException))]
        public void MissingTokenThrowsException()
        {
            var restClient = new RestClient();
            var continuumConnection = new ContinuumConnection(restClient, String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingTokenException))]
        public void ValidRequestsCallsExecuteOnce()
        {
            var mockRestClient = new Mock<IRestClient>();
            var continuumConnection = new ContinuumConnection(mockRestClient.Object, String.Empty);
            continuumConnection.MakeRequest(_moqRestRequest.Object);
            mockRestClient.Verify(a => a.Execute(_moqRestRequest.Object), Times.Once);
        }

    }
}
