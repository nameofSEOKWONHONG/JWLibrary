using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Writers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.ApiCore.Dto {
    public class RequestDto<TDto>
        where TDto : class, new() {
        public RequestDto() {

        }

        [JsonProperty("dto")]
        TDto Dto { get; set; }

        public string WRITER_ID { get; set; }
        public DateTime WRITE_DT { get; set; } = DateTime.Now;
        public string EDITOR_ID { get; set; }
        public DateTime EDIT_DT { get; set; } = DateTime.Now;
    }

    public class EnableBodyRewind : Attribute, IAuthorizationFilter {

        public async void OnAuthorization(AuthorizationFilterContext context) {
            var bodyStr = "";
            var req = context.HttpContext.Request;
            var items = context.HttpContext.Items;

            // Allows using several time the stream in ASP.Net Core
            req.EnableBuffering();

            // Arguments: Stream, Encoding, detect encoding, buffer size
            // AND, the most important: keep stream opened
            using (StreamReader reader
                      = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true)) {
                bodyStr = await reader.ReadToEndAsync();
            }

            // Rewind, so the core is not lost when it looks the body for the request
            req.Body.Position = 0;

            // Do whatever work with bodyStr here
        }
    }
}
