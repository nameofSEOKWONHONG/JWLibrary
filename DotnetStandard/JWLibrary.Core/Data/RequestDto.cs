using Newtonsoft.Json;
using System;

namespace JWLibrary.Core {
    public class RequestDto<TDto>
        where TDto : class, new() {
        public RequestDto() {

        }

        [JsonProperty("dto")]
        public TDto Dto { get; set; }

        public string WRITER_ID { get; set; }
        public DateTime WRITE_DT { get; set; } = DateTime.Now;
        public string EDITOR_ID { get; set; }
        public DateTime EDIT_DT { get; set; } = DateTime.Now;
    }
}
