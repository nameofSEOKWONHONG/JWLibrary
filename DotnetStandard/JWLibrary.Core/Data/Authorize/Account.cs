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

    [LiteDbFileName("./account.db")]
    [Table("accounts")]
    public class Account : IAccount {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// hashId
        /// </summary>
        /// <value></value>
        public string HashId { get; set; }
        /// <summary>
        /// userId
        /// </summary>
        /// <value></value>
        public string UserId { get; set; }
        /// <summary>
        /// password
        /// </summary>
        /// <value></value>
        public string Passwd { get; set; }
    }

    public interface IAccountSvc {
        IAccount GetAccount(Account account);
    }
}
