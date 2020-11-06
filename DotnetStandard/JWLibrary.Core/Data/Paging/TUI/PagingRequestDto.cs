namespace JWLibrary.Core.Data.TUI {

    public class TUIPagingRequestDto<T> : RequestDto<T>, ITUIPagingBase, ITUIPagingOption {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int VisiblePages { get; set; }
        public int Page { get; set; }
        public bool CenterAlign { get; set; }
        public string FirstItemClassName { get; set; }
        public string LastItemClassName { get; set; }
        public TUITemplage Template { get; set; }
    }
}