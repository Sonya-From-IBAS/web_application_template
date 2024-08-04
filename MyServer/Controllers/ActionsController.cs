using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ActionsController : BaseCustomContoller
    {
        [HttpGet("get-actions")]
        public IActionResult Actions()
        {
            return JsonOk("Only authorized users");
        }
    }
}
