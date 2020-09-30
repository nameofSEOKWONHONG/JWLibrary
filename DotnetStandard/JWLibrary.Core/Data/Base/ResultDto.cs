namespace JWLibrary.Core {
    public partial class ResultDtoBase<TDto> {
        private TDto Dto { get; set; }
    }
    public partial class ResultDto<TDto> : ResultDtoBase<TDto> {

    }
}