using System;
using System.Linq;
using System.Net;
using Comparator.Models.Entities;
using Comparator.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Comparator.Tests.Service
{
    [TestClass]
    public class DiffServiceTest
    {
        private BinaryData EmptyBinaryData { get; set; }
        private BinaryData LeftOnlyBinaryData { get; set; }
        private BinaryData RightOnlyBinaryData { get; set; }
        private BinaryData CompleteBinaryData { get; set; }

        private DiffService DiffService { get; set; }


        [TestInitialize]
        public async void Setup()
        {
            EmptyBinaryData = null;
            LeftOnlyBinaryData = PopulateLeftOnlyBinaryData();
            RightOnlyBinaryData = PopulateRightOnlyBinaryData();
            CompleteBinaryData = PopulateCompleteBinaryData();

            DiffService = new DiffService(new MemoryCacher(), new CompareService());

            await DiffService.Put(new DataRequest()
            {
                Data = "AAA==",
                DataType = BinaryDataType.LEFT,
                BinaryDataId = 11
            });

            await DiffService.Put(new DataRequest()
            {
                Data = "BBB==",
                DataType = BinaryDataType.RIGHT,
                BinaryDataId = 22
            });

            await DiffService.Put(new DataRequest()
            {
                Data = "CCC==",
                DataType = BinaryDataType.LEFT,
                BinaryDataId = 33
            });

            await DiffService.Put(new DataRequest()
            {
                Data = "CCC==",
                DataType = BinaryDataType.RIGHT,
                BinaryDataId = 33
            });

            await DiffService.Put(new DataRequest()
            {
                Data = "CCC==",
                DataType = BinaryDataType.LEFT,
                BinaryDataId = 44
            });

            await DiffService.Put(new DataRequest()
            {
                Data = "DD==",
                DataType = BinaryDataType.RIGHT,
                BinaryDataId = 44
            });

            await DiffService.Put(new DataRequest()
            {
                Data = "AAABBBCCCDDD==",
                DataType = BinaryDataType.LEFT,
                BinaryDataId = 55
            });

            await DiffService.Put(new DataRequest()
            {
                Data = "ABABXXCCYYYD==",
                DataType = BinaryDataType.RIGHT,
                BinaryDataId = 55
            });
        }

        private static BinaryData PopulateLeftOnlyBinaryData()
        {
            return new BinaryData()
            {
                Id = 11,
                Left = PopulateSingleData(BinaryDataType.LEFT)
            };
        }

        private static BinaryData PopulateRightOnlyBinaryData()
        {
            return new BinaryData()
            {
                Id = 22,
                Right = PopulateSingleData(BinaryDataType.RIGHT)
            };
        }

        private static BinaryData PopulateCompleteBinaryData()
        {
            return new BinaryData()
            {
                Id = 1,
                Right = PopulateSingleData(BinaryDataType.RIGHT),
                Left = PopulateSingleData(BinaryDataType.LEFT)
            };
        }

        private static SingleData PopulateSingleData(string type, string data=null)
        {
            return new SingleData()
            {
                Data = data??"AAA==",
                DataType = type,
            };
        }

        [TestMethod]
        public void PutInsertLeftSuccessfullyTest()
        {
        }

        [TestMethod]
        public void PutInsertRightSuccessfullyTest()
        {
        }

        [TestMethod]
        public void PutUpdateLeftsuccessfullyTest()
        {
        }

        [TestMethod]
        public void PutUpdateRightSuccessfullyTest()
        {
            
        }


        [TestMethod]
        public void GetComparisonBinaryDataIsNullTest()
        {
            // Setup.

            // Execute.
            var result = DiffService.GetComparison(99).Result;

            // Assert.
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Message, HttpStatusCode.NotFound.ToString());
        }

        [TestMethod]
        public void GetComparisonBinaryDataOnlyHasRightTest()
        {
            // Setup.

            // Execute.
            var result = DiffService.GetComparison(22).Result;

            // Assert.
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Message, HttpStatusCode.NotFound.ToString());
        }

        [TestMethod]
        public void GetComparisonBinaryDataOnlyHasLeftTest()
        {
            // Setup.

            // Execute.
            var result = DiffService.GetComparison(11).Result;

            // Assert.
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Message, HttpStatusCode.NotFound.ToString());
        }

        [TestMethod]
        public void GetComparisonBinaryDataHasDifferentLengthTest()
        {
            // Setup.

            // Execute.
            var result = DiffService.GetComparison(44).Result;

            // Assert.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.DiffResultType, DiffResultType.SizeDoNotMatch);
        }

        [TestMethod]
        public void GetComparisonBinaryDataRightAndLeftAreSimilarTest()
        {
            // Setup.

            // Execute.
            var result = DiffService.GetComparison(33).Result;

            // Assert.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.DiffResultType, DiffResultType.IsEqual);
        }

        [TestMethod]
        public void GetComparisonBinaryDataHasDifferencesTest()
        {
            // Setup.

            // Execute.
            var result = DiffService.GetComparison(55).Result;

            // Assert.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.DiffResultType, DiffResultType.ContentDoNotMatch);
            Assert.AreEqual(result.Diffs.Count(), 3);
        }

    }
}
