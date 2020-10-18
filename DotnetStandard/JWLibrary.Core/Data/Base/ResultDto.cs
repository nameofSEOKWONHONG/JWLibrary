using Newtonsoft.Json;
using System;

namespace JWLibrary.Core {
    public partial class ResultDtoBase<TResultDto> {
        [JsonProperty("resultDto")]
        public TResultDto ResultDto { get; set; }
    }
    public partial class ResultDto<TResultDto> : ResultDtoBase<TResultDto> {

    }
    public partial class PagingResultDto<TResultDto> : ResultDto<TResultDto> {
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalCount { get; set; }
        public int TotalPageNumber { 
            get {
                return (int)Math.Ceiling((double)TotalCount / Size);
            }
        }
    }
}