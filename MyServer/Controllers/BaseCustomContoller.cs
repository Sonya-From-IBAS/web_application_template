using Microsoft.AspNetCore.Mvc;
using MyServer.Controllers.ActionResults;

namespace MyServer.Controllers
{
    public class BaseCustomContoller : ControllerBase
    {
        private const string JsonMimeType = "application/json";

        #region Protected methods
        protected JsonResult JsonOk()
        {
            return Json(new JsonResponse(ResponseTypeEnum.Ok), JsonMimeType);
        }

        protected JsonResult JsonOk(object data)
        {
            return Json(new JsonResponse(ResponseTypeEnum.Ok) { Data = data }, JsonMimeType);
        }

        protected JsonResult JsonWarn(string message)
        {
            return Json(new JsonResponse(ResponseTypeEnum.Warning) { Message = message }, JsonMimeType);
        }

        protected JsonResult JsonWarn(string[] messages)
        {
            return Json(new JsonResponse(ResponseTypeEnum.Warning) { Messages = messages }, JsonMimeType);
        }

        protected JsonResult JsonWarnButReturnData(object data, string message)
        {
            return Json(new JsonResponse(ResponseTypeEnum.Warning) { Message = message, Data = data }, JsonMimeType);
        }

        protected JsonResult JsonWarnButReturnData(object data, string[] messages)
        {
            return Json(new JsonResponse(ResponseTypeEnum.Warning) { Messages = messages, Data = data }, JsonMimeType);
        }

        protected JsonResult JsonError(string message)
        {
            return Json(new JsonResponse(ResponseTypeEnum.Error) { Message = message }, JsonMimeType);
        }

        protected JsonResult JsonError(string[] messages)
        {
            return Json(new JsonResponse(ResponseTypeEnum.Error) { Messages = messages }, JsonMimeType);
        }

        protected JsonResult JsonErrorButReturnData(object data, string message)
        {
            return Json(new JsonResponse(ResponseTypeEnum.Error) { Message = message, Data = data }, JsonMimeType);
        }

        protected JsonResult JsonErrorButReturnData(object data, string[] messages)
        {
            return Json(new JsonResponse(ResponseTypeEnum.Error) { Messages = messages, Data = data }, JsonMimeType);
        }

        protected JsonResult BlockingJsonError(string message)
        {
            return Json(new JsonResponse(ResponseTypeEnum.Error) { Message = message, Data = new { IsBlocking = true } }, JsonMimeType  );
        }

        protected OkObjectResult Ok(JsonResponse response)
        {
            return Ok((object)response);
        }

        protected BadRequestObjectResult BadRequest(JsonResponse response)
        {
            return BadRequest((object)response);
        }
        #endregion

        #region Private Methods 
        private JsonResult Json(object data, string contentType)
        {
            JsonResult jsonResult = new JsonResult(data);
            jsonResult.ContentType = contentType;

            return jsonResult;
        }
        #endregion
    } 
}
