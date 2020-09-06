namespace JWLibrary.Core {

    public class ResultDto<TDto>
       where TDto : class {
        private TDto Dto { get; set; }
    }
}