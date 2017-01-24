using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Comparator.Controllers;
using Comparator.Models.Entities;
using Comparator.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Comparator.Tests.Controllers
{
    [TestClass]
    public class DiffControllerTest
    {
        private MemoryCacher Cacher { get; set; }

        private ICompareService CompareService { get; set; }

        private IDiffService DiffService { get; set; }

        private DiffController DiffController { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Cacher = new MemoryCacher();
            CompareService = new CompareService();

            DiffService = new DiffService(Cacher, CompareService);
            DiffController = new DiffController(DiffService) {Request = new HttpRequestMessage()};
            DiffController.Request.SetConfiguration(new HttpConfiguration());

        }

        [TestMethod]
        public void Right_IdIsNegativeTest()
        {
            var result = DiffController.Right(-2, new SimpleDataRequest()).Result;

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void Left_IdIsNegativeTest()
        {
            var result = DiffController.Left(-2, new SimpleDataRequest()).Result;

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void Right_RequestIsNullTest()
        {
            var result = DiffController.Right(2, null).Result;

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void Left_RequestIsNullTest()
        {
            var result = DiffController.Left(2, null).Result;

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void Right_DataIsNullTest()
        {
            var result = DiffController.Right(2, new SimpleDataRequest() { data = null }).Result;
            
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void Left_DataIsNullTest()
        {
            var result = DiffController.Left(2, new SimpleDataRequest() { data = null }).Result;

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void Right_IsPutSuccessfullyTest()
        {
            var result = DiffController.Right(3, new SimpleDataRequest()
            {
                data = "AAAA=="
            }).Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessStatusCode);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod]
        public void Left_IsPutSuccessfullyTest()
        {
            var result = DiffController.Right(3, new SimpleDataRequest()
            {
                data = "BBBB=="
            }).Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessStatusCode);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod]
        public void GetComparison_IdIsNegativeTest()
        {
            var result = DiffController.Get(-3).Result;

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void GetComparison_NotFoundTest()
        {
            // Setup.

            // Execute.
            var result = DiffController.Get(99).Result;

            
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async void GetComparison_DataNotCompleteTest()
        {
            // Setup.
            var putResult = DiffController.Right(5, new SimpleDataRequest()
            {
                data = "AAAA=="
            }).Result;

            // Execute.
            var getResult = DiffController.Get(5).Result;

            // Assert.
            Assert.IsNotNull(getResult);
            Assert.AreEqual(getResult.StatusCode, HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void GetComparison_DataSizeNotMatchTest()
        {
            // Setup.
            var putRightResult = DiffController.Right(5, new SimpleDataRequest()
            {
                data = "AAAA=="
            }).Result;

            var putLeftResult = DiffController.Left(5, new SimpleDataRequest()
            {
                data = "BB=="
            }).Result;

            // Execute.
            var getResult = DiffController.Get(5).Result;

            // Assert.
            Assert.IsNotNull(getResult);
            Assert.AreEqual(getResult.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(getResult.Content);

            var content = getResult.Content.ReadAsAsync<SimpleDiffResponse>().Result;
            Assert.AreEqual(content.diffResultType, DiffResultType.SizeDoNotMatch);
        }

        [TestMethod]
        public void GetComparison_DataIsSimilarTest()
        {
            // Setup.
            var putRightResult = DiffController.Right(5, new SimpleDataRequest()
            {
                data = "AAAA=="
            }).Result;

            var putLeftResult = DiffController.Left(5, new SimpleDataRequest()
            {
                data = "AAAA=="
            }).Result;

            // Execute.
            var getResult = DiffController.Get(5).Result;

            // Assert.
            Assert.IsNotNull(getResult);
            Assert.AreEqual(getResult.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(getResult.Content);

            var content = getResult.Content.ReadAsAsync<SimpleDiffResponse>().Result;
            Assert.AreEqual(content.diffResultType, DiffResultType.IsEqual);
        }

        [TestMethod]
        public void GetComparison_DataHasDifferencesTest()
        {
            // Setup.
            var putRightResult = DiffController.Right(5, new SimpleDataRequest()
            {
                data = "ABAB=="
            }).Result;

            var putLeftResult = DiffController.Left(5, new SimpleDataRequest()
            {
                data = "AAAA=="
            }).Result;

            // Execute.
            var getResult = DiffController.Get(5).Result;

            // Assert.
            Assert.IsNotNull(getResult);
            Assert.AreEqual(getResult.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(getResult.Content);

            var content = getResult.Content.ReadAsAsync<DiffResultTypeWithDiffsResponse>().Result;
            Assert.AreEqual(content.diffResultType, DiffResultType.ContentDoNotMatch);
            Assert.IsNotNull(content.diffs);
            Assert.AreEqual(content.diffs.Count(), 2);
        }

        
    }

    public class SimpleDiffResponse
    {
        public string diffResultType { get; set; }
    }

    public class DiffResultTypeWithDiffsResponse : SimpleDiffResponse
    {
        public IEnumerable<DiffDetail> diffs { get; set; } 
    }
}
