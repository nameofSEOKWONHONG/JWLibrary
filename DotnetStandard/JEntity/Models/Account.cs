using JWLibrary.Core.Data;
using JWLibrary.Database;
using LiteDbFlex;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWService.Data.Models {
    [LiteDbTable("account.db", "accounts")]
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
}
