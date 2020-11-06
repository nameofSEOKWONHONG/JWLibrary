namespace JWLibrary.Core.Data {

    public interface IAccount {
        int Id { get; set; }
        string HashId { get; set; }
        string UserId { get; set; }
        string Passwd { get; set; }
    }
}