using JWLibrary.ApiCore.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWLibrary.ApiCore.Controllers {
    [ApiVersion("1.0", Deprecated = true)] //비추천 공지함.
    [ApiVersion("1.1")]
    [ApiVersion("2.0")]
    public class VerionExampleController : JControllerBase<VerionExampleController> {
        public VerionExampleController(ILogger<VerionExampleController> logger) : base(logger) {
        }

        [HttpGet]
        public string Hello() {
            return "hello version 0.0";
        }

        [HttpGet]
        [Route("{id}")]
        [MapToApiVersion("2.0")]
        public string Hello(int id) {
            return "hello id" + id;
        }
    }
}
