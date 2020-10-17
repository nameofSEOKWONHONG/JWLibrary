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
}
