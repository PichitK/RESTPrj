using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("news_likes")]
    public class NewsLike
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("news_id")]
        public int NewsId { get; set; }

        [Column("member_id")]
        public int MemberId { get; set; }

        [Column("like_type")]
        public int LikeType { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}
