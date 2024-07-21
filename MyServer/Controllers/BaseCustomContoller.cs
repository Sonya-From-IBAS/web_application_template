using Microsoft.AspNetCore.Mvc;

namespace MyServer.Controllers
{
    public class BaseCustomContoller: ControllerBase
    {
        public ActionResult JsonOk(object result)
        {
            return Ok(new JsonResult(result));
        }
    }
}
