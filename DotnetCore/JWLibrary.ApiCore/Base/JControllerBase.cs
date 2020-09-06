using Microsoft.AspNetCore.Mvc;

namespace JWLibrary.ApiCore.Base {

    /// <summary>
    /// base controller
    /// </summary>
    [ApiController]
    [Route("japi/[controller]/[action]")]
    public class JControllerBase : ControllerBase {
    }
}