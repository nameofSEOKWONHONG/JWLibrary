using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace JWLibrary.ApiCore {
    //public class JModelBinder<T> : IModelBinder
    //    where T : class, new() {
    //    public async Task BindModelAsync(ModelBindingContext bindingContext) {
    //        if (bindingContext == null)
    //            throw new ArgumentNullException(nameof(bindingContext));

    //        var values = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

    //        var valueFromBody = string.Empty;
    //        using (StreamReader reader
    //                  = new StreamReader(bindingContext.HttpContext.Request.Query., Encoding.UTF8)) {
    //            valueFromBody = await reader.ReadToEndAsync();
    //        }
    //        var result = JsonConvert.DeserializeObject<T>(valueFromBody);
    //        bindingContext.Result = ModelBindingResult.Success(result);
    //    }
    //}
    public class JPostModelBinder<T> : IModelBinder
        where T : class, new() {

        public async Task BindModelAsync(ModelBindingContext bindingContext) {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var valueFromBody = string.Empty;
            using (StreamReader reader
                      = new StreamReader(bindingContext.HttpContext.Request.Body, Encoding.UTF8)) {
                valueFromBody = await reader.ReadToEndAsync();
            }
            var result = JsonConvert.DeserializeObject<T>(valueFromBody);
            bindingContext.Result = ModelBindingResult.Success(result);
        }
    }
}