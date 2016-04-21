using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("member_tokens")]
    public class MemberToken
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("member_id")]
        public int MemberId { get; set; }

        [Column("token")]
        public string Token { get; set; }

        [Column("IssuedUtc")]
        public DateTime IssuedUtc { get; set; }

        [Column("ExpiresUtc")]
        public DateTime ExpiresUtc { get; set; }
    }
}
