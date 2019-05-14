using OAuth.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OAuth.WebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        [Authorize(Roles ="user")]
        [HttpPost]
        public HttpResponseMessage AuthRoleUser(RequestModel requestModel)
        {
            return Request.CreateResponse(HttpStatusCode.OK, GetResult(requestModel));
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public HttpResponseMessage AuthRoleAdmin(RequestModel requestModel)
        {
            return Request.CreateResponse(HttpStatusCode.OK, GetResult(requestModel));
        }

        [Authorize(Users ="JohnSmith")]
        [HttpPost]
        public HttpResponseMessage AuthUserJohnSmith(RequestModel requestModel)
        {
            return Request.CreateResponse(HttpStatusCode.OK, GetResult(requestModel));
        }

        [Authorize(Users = "UserUnknown")]
        [HttpPost]
        public HttpResponseMessage AuthUserUnknown(RequestModel requestModel)
        {
            return Request.CreateResponse(HttpStatusCode.OK, GetResult(requestModel));
        }

        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage AuthAnonymous(RequestModel requestModel)
        {
            return Request.CreateResponse(HttpStatusCode.OK, GetResult(requestModel));
        }

        private object GetResult(RequestModel requestModel)
        {
            return new ResponseModel
            {
                Result = requestModel.Text.ToUpper(),
                Length = requestModel.Text.Length
            };
        }
    }
    
    
}
