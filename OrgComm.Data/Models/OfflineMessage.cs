using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("offline_messages")]
    public class OfflineMessage
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("member_id")]
        public int MemberId { get; set; }

        [Column("data")]
        public string Data { get; set; }
        
        [Column("type")]
        public int Type { get; set; }
        
        [Column("get_flag")]
        public bool GetFlag { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}
