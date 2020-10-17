using Newtonsoft.Json;
using System;

namespace JWLibrary.Core {
    /// <summary>
    /// base request class
    /// </summary>
    /// <typeparam name="TRequestDto"></typeparam>
    public partial class RequestBase<TRequestDto> {
        [JsonProperty("requestDto")]
        public TRequestDto RequestDto { get; set; }
    }

    /// <summary>
    /// single query request class
    /// </summary>
    /// <typeparam name="TRequestDto"></typeparam>
    public partial class RequestDto<TRequestDto> : RequestBase<TRequestDto> {
        public RequestDto() {
        }
        public string WRITER_ID { get; set; }
        public DateTime WRITE_DT { get; set; } = DateTime.Now;
        public string EDITOR_ID { get; set; }
        public DateTime EDIT_DT { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// paging query request class
    /// </summary>
    /// <typeparam name="TRequestDto"></typeparam>
    public partial class PagingRequestDto<TRequestDto> : RequestDto<TRequestDto> {
        public PagingRequestDto() { 
            
        }

        public int Page { get; set; }
        public int Size { get; set; }
        public int PageNumber { get; set; }
    }
}