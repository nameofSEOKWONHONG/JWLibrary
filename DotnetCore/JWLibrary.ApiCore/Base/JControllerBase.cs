using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWLibrary.ApiCore.Base {
    [ApiController]
    [Route("japi/[controller]/[action]")]

    public class JControllerBase : ControllerBase {
    }
}
