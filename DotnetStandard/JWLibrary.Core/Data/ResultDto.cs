namespace JWLibrary.Core {
    public class ResultDtoBase<TDto> {
        private TDto Dto { get; set; }
    }
    public class ResultDto<TDto> : ResultDtoBase<TDto> {

    }
}