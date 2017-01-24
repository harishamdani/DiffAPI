using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Comparator.Models.Entities;
using Comparator.Service;

namespace Comparator.Controllers
{
    [RoutePrefix("v1/diff")]
    public class DiffController : ApiController
    {
        private IDiffService DiffService { get; }

        public DiffController(IDiffService diffService)
        {
            DiffService = diffService;
        }

        /// <summary>
        /// Put the right side of the binary data.
        /// </summary>
        /// <param name="id">The ID of the binary data.</param>
        /// <param name="request">the object which contains data properties.</param>
        /// <returns>
        /// HTTP Status Code: 'Created' if succeed.
        /// HTTP Status Code: 'Bad Request/Internal Server Error' if it doesn't succeed (depends on the error cause).
        /// </returns>
        [HttpPut, HttpPost]
        [Route("{id:int}/right")]
        public async Task<HttpResponseMessage> Right(int id, [FromBody] SimpleDataRequest request)
        {
            if (id < 0 || request == null || string.IsNullOrWhiteSpace(request.data))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var result = await DiffService.Put(new DataRequest()
            {
                BinaryDataId = id,
                Data = request.data,
                DataType = BinaryDataType.RIGHT
            });

            return Request.CreateResponse(
                result.IsSuccess 
                    ? HttpStatusCode.Created 
                    : HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Put the left side of the binary data.
        /// </summary>
        /// <param name="id">The ID of the binary data.</param>
        /// <param name="request">the object which contains data properties.</param>
        /// <returns>
        /// HTTP Status Code: 'Created' if succeed.
        /// HTTP Status Code: 'Bad Request/Internal Server Error' if it doesn't succeed (depends on the error cause).
        /// </returns>
        [HttpPut, HttpPost]
        [Route("{id:int}/left")]
        public async Task<HttpResponseMessage> Left(int id, [FromBody] SimpleDataRequest request)
        {
            if (id < 0 || request == null || string.IsNullOrWhiteSpace(request.data))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var result = await DiffService.Put(new DataRequest()
            {
                BinaryDataId = id,
                Data = request.data,
                DataType = BinaryDataType.LEFT
            });

            return Request.CreateResponse(
                result.IsSuccess
                    ? HttpStatusCode.Created
                    : HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Gets the comparison of both binary datas of the binary ID.
        /// </summary>
        /// <param name="id">The binary which store the left and right data to be compared.</param>
        /// <returns>
        /// If both left and right data exist and has the same length, It will return the comparison result.
        /// 
        /// </returns>
        [HttpGet]
        //[Route("{id:int}")]
        public async Task<HttpResponseMessage> Get(int id)
        {
            if (id < 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var result = await DiffService.GetComparison(id);

            if (!result.IsSuccess)
            {
                return Request.CreateResponse((HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.Message, true));
            }

            return result.Diffs == null || !result.Diffs.Any()
                ? Request.CreateResponse(
                    HttpStatusCode.OK,
                    new
                    {
                        diffResultType = result.DiffResultType
                    })
                : Request.CreateResponse(
                    HttpStatusCode.OK,
                    new
                    {
                        diffResultType = result.DiffResultType,
                        diffs = result.Diffs
                    });
        }
    }
}