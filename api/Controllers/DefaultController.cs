using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [Route("")]
        [HttpHead]
        [HttpGet]
        public ActionResult Ready()
        {
            return Ok();
        }
    }
}
