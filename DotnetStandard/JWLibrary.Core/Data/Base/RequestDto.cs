using Newtonsoft.Json;
using System;

namespace JWLibrary.Core {

    public class RequestBase<TDto> {
        [JsonProperty("dto")]
        public TDto Dto { get; set; }
    }
    public class RequestDto<TDto> : RequestBase<TDto> {
        public RequestDto() {
        }
        public string WRITER_ID { get; set; }
        public DateTime WRITE_DT { get; set; } = DateTime.Now;
        public string EDITOR_ID { get; set; }
        public DateTime EDIT_DT { get; set; } = DateTime.Now;
    }
}