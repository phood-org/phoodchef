using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace phoodchef.Models.DTOs
{
    public class ReturnWrapper : IHttpActionResult
    {
        private ReturnObj returnObj {get;set;}
        private HttpStatusCode StatusCode { get; set; }

        public ReturnWrapper(dynamic data)
        {

            StatusCode = HttpStatusCode.OK;
            returnObj = new ReturnObj
            {
                IsSuccess = true,
                FormattedMessage = new List<string>(),
                Data = data
            };
        }

        public ReturnWrapper(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            returnObj = new ReturnObj
            {
                IsSuccess = false,
                FormattedMessage = new List<string> { message },
                Data = null
            };
        }

        public ReturnWrapper(HttpStatusCode statusCode, List<string> message)
        {
            StatusCode = statusCode;
            returnObj = new ReturnObj
            {
                IsSuccess = false,
                FormattedMessage = message,
                Data = null
            };
        }

        public ReturnWrapper(dynamic data, List<string> messageList)
        {
            StatusCode = HttpStatusCode.OK;
            returnObj = new ReturnObj
            {
                IsSuccess = true,
                FormattedMessage = messageList,
                Data = data
            };
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                Content = new ObjectContent<ReturnObj>(returnObj, new JsonMediaTypeFormatter()),
                StatusCode = StatusCode
            };

            return Task.FromResult(response);
        }
    }

    public class ReturnObj {
        public bool IsSuccess { get; set; }
        public List<string> FormattedMessage { get; set; }
        public dynamic Data { get; set; }
    }

}