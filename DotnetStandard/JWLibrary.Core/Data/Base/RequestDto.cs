using Newtonsoft.Json;
using System;

namespace JWLibrary.Core {

    /// <summary>
    ///     base request class
    /// </summary>
    /// <typeparam name="TRequestDto"></typeparam>
    public class RequestBase<TRequestDto> {
        [JsonProperty("requestDto")]
        public TRequestDto Dto { get; set; }
    }

    /// <summary>
    ///     single query request class
    /// </summary>
    /// <typeparam name="TRequestDto"></typeparam>
    public class RequestDto<TRequestDto> : RequestBase<TRequestDto> {
        public string WRITER_ID { get; set; }
        public DateTime WRITE_DT { get; set; } = DateTime.Now;
        public string EDITOR_ID { get; set; }
        public DateTime EDIT_DT { get; set; } = DateTime.Now;
    }

    /// <summary>
    ///     paging query request class
    /// </summary>
    /// <typeparam name="TRequestDto"></typeparam>
    public class PagingRequestDto<TRequestDto> : RequestDto<TRequestDto> {
        public int Page { get; set; }
        public int Size { get; set; }
    }
}