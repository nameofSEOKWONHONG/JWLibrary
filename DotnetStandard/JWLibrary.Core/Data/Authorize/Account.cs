using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWLibrary.Core.Data {
    public interface IAccount {
        int Id {get;set;}
        string HashId {get;set;}
        string UserId {get;set;}
        string Passwd { get; set; }
    }

    [Table("accounts")]
    public class Account : IAccount {
        public int Id { get; set; }
        public string HashId { get; set; }
        public string UserId { get; set; }
        public string Passwd { get; set; }
    }

    public interface IAccountSvc {
        IAccount GetAccount(Account account);
    }
}
