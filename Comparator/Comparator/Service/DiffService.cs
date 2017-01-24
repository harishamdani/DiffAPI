using System;
using System.Net;
using System.Threading.Tasks;
using Comparator.Models.Entities;
using Comparator.Models.ViewModels;

namespace Comparator.Service
{
    public class DiffService : IDiffService
    {
        private MemoryCacher Cacher { get; }

        private ICompareService CompareService { get; }

        public DiffService(
            MemoryCacher cacher,
            ICompareService compareService)
        {
            Cacher = cacher;
            CompareService = compareService;
        }

        /// <summary>
        /// Put the data to the BinaryData.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<BaseResponse> Put(DataRequest request)
        {
            // if request object is valid, add object to cache.
            var binaryData = (BinaryData) Cacher.GetValue(request.BinaryDataId.ToString())
                             ?? new BinaryData { Id = request.BinaryDataId };

            if (string.Equals(
                request.DataType, 
                BinaryDataType.LEFT, 
                StringComparison.OrdinalIgnoreCase))
            {
                binaryData.Left = new SingleData
                {
                    Data = request.Data,
                    DataType = request.DataType
                };
            }

            if (string.Equals(
                request.DataType, 
                BinaryDataType.RIGHT, 
                StringComparison.OrdinalIgnoreCase))
            {
                binaryData.Right = new SingleData
                {
                    Data = request.Data,
                    DataType = request.DataType
                };
            }

            Cacher.Set(
                binaryData.Id.ToString(), 
                binaryData, 
                DateTimeOffset.UtcNow.AddYears(1));

            return new BaseResponse()
            {
                IsSuccess = true,
                Message = "Insert data is successful."
            };
        }

        /// <summary>
        /// Gets the comparison of both left and right binary data by ID.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<CompareResponse> GetComparison(int id)
        {
            var binaryData = (BinaryData)Cacher.GetValue(id.ToString());

            if (binaryData == null)
            {
                return new CompareResponse
                {
                    IsSuccess = false,
                    Message = HttpStatusCode.NotFound.ToString()
                };
            }

            // If ID is found:
            // 1. Check if ID has left and right component.
            // 1. A. if ID has only left or right, return immediately.
            if (binaryData.Left == null
                || binaryData.Right == null
                || string.IsNullOrWhiteSpace(binaryData.Left.Data)
                || string.IsNullOrWhiteSpace(binaryData.Right.Data))
            {
                return new CompareResponse
                {
                    IsSuccess = false,
                    Message = HttpStatusCode.NotFound.ToString()
                };
            }

            // 2. Check if Left and Right has the same length.
            // 2. A. if Left/Right has different length, return immediately.
            if (binaryData.Left.Data.Length != binaryData.Right.Data.Length)
            {
                return new CompareResponse
                {
                    IsSuccess = true,
                    DiffResultType = DiffResultType.SizeDoNotMatch
                };
            }

            // 3. Check Differences in the string.
            // 3. A. if no differences at all. return immediately.
            if (string.Equals(
                binaryData.Left.Data,
                binaryData.Right.Data
                ))
            {
                return new CompareResponse
                {
                    IsSuccess = true,
                    DiffResultType = DiffResultType.IsEqual
                };
            }

            // 4. Find all offset in the Left/Right comparison and return
            //    all the differences for each offset(offset, length).
            var diffs = CompareService.Compare(
                binaryData.Left.Data, 
                binaryData.Right.Data);

            return new CompareResponse
            {
                IsSuccess = true,
                DiffResultType = DiffResultType.ContentDoNotMatch,
                Diffs = diffs
            };
        }
    }
}