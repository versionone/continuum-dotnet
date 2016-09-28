using System;
using System.Text;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ContinuumDotNet.Connection;
using ContinuumDotNet.Exceptions.Connection;
using ContinuumDotNet.Flow.Pipelines;
using ContinuumDotNet.Interfaces.Connection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using System.Net;
using ContinuumDotNet.Exceptions;
using ContinuumDotNet.Exceptions.Flow;
using ContinuumDotNet.UnitTests.TestData.Flow.Pipelines;

namespace ContinuumDotNet.UnitTests.Flow.Pipelines
{
    /// <summary>
    /// Summary description for PipelineTests
    /// </summary>
    [TestClass]
    public class PipelineTests
    {
        public PipelineTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GettingAPipelineInstanceCallsMakeRequestOnce()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            var pipelineInstanceManager = new PipelineInstanceManager(continuumConnectionMock.Object);
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)200, Content = "{ \"Response\": {\"name\": \"Test\"}}"});

            pipelineInstanceManager.ById("abcde").GetOne();
            continuumConnectionMock.Verify(a => a.MakeRequest(It.IsAny<IRestRequest>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPipelineIdException))]
        public void PassingAnInvalidPipelineIdShouldThrowInvalidPipelineIdException()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            var pipelineInstanceManager = new PipelineInstanceManager(continuumConnectionMock.Object);
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)280 , Content = @"Pipeline Instance not found using identifier [ABCDE]" });

            pipelineInstanceManager.ById("ABCDE").GetOne();
        }

        [TestMethod]
        [ExpectedException(typeof(MissingParameterException))]
        public void PassingAnEmptyPipelineIdShouldThrowMissingPipelineIdException()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            var pipelineInstanceManager = new PipelineInstanceManager(continuumConnectionMock.Object);
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)400, Content = "{\n    \"ErrorCode\": \"EmptyParameter\",\n    \"ErrorDetail\": \"Required parameter 'pi' empty.\",\n    \"ErrorMessage\": \"\",\n    \"Method\": \"get_pipelineinstance\",\n    \"Response\": \"\"\n}" });

            pipelineInstanceManager.ById("").GetOne();
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void InvalidTokenThrowsANotAuthorizedException()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            var pipelineInstanceManager = new PipelineInstanceManager(continuumConnectionMock.Object);
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)401 });
            pipelineInstanceManager.ById("").GetOne();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void NotFoundExceptionThrowsException()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            var pipelineInstanceManager = new PipelineInstanceManager(continuumConnectionMock.Object);
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)281 });
            pipelineInstanceManager.ById("ABCDEF").GetOne();
        }

        [TestMethod]
        public void InstantiatingPipelineInstanceWithConnectionPersists()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            var pipelineInstance = new PipelineInstance(continuumConnectionMock.Object);
            Assert.IsNotNull(pipelineInstance.Connection);
        }

        [TestMethod]
        public void NameIsProperlyDeserializedFromPipelineInstance()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)200, Content = SimplePipelineInstance.DATA});
            var pipelineInstanceManager = new PipelineInstanceManager(continuumConnectionMock.Object);
            var pipelineInstance = pipelineInstanceManager.ById("ABCDE").GetOne();
            Assert.AreEqual("SIMPLE_PIPELINE", pipelineInstance.Name);
        }

        [TestMethod]
        public void IdIsProperlyDeserializedFromPipelineInstance()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)200, Content = SimplePipelineInstance.DATA });
            var pipelineInstanceManager = new PipelineInstanceManager(continuumConnectionMock.Object);
            var pipelineInstance = pipelineInstanceManager.ById("ABCDE").GetOne();
            Assert.AreEqual("57ea95e008a29402a371866b", pipelineInstance.Id);
        }

        [TestMethod]
        public void CodeIsProperlyDeserializedFromPipelineInstance()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)200, Content = SimplePipelineInstance.DATA });
            var pipelineInstanceManager = new PipelineInstanceManager(continuumConnectionMock.Object);
            var pipelineInstance = pipelineInstanceManager.ById("ABCDE").GetOne();
            Assert.AreEqual("ABCDE", pipelineInstance.FlightCode);
        }

        [TestMethod]
        public void NumberIsProperlyDeserializedFromPipelineInstance()
        {
            var continuumConnectionMock = new Mock<IContinuumConnection>();
            continuumConnectionMock.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { StatusCode = (HttpStatusCode)200, Content = SimplePipelineInstance.DATA });
            var pipelineInstanceManager = new PipelineInstanceManager(continuumConnectionMock.Object);
            var pipelineInstance = pipelineInstanceManager.ById("ABCDE").GetOne();
            Assert.AreEqual(262, pipelineInstance.Number);
        }
    }
}
