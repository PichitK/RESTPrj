using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("member_devices")]
    public class MemberDevice
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("member_id")]
        public int MemberId { get; set; }

        [Column("os_id")]
        public int OSId { get; set; }
        
        [Column("token")]
        public string Token { get; set; }

        [Column("createdDate")]
        public DateTime CreatedDate{ get; set; }
    }
}
