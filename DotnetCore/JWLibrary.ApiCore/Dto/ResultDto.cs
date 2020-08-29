using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWLibrary.ApiCore.Dto {
    public class ResultDto<TDto>
       where TDto : class {
       TDto Dto { get; set; }
    }
}
