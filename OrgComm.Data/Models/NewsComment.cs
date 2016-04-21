using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("news_comments")]
    public class NewsComment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("member_id")]
        public int MemberId { get; set; }

        [Column("news_id")]
        public int NewsId { get; set; }

        [Column("text")]
        public string Text{ get; set; }

        [Column("type")]
        public int Type { get; set; }
        
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}
