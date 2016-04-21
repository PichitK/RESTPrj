using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("news_contents")]
    public class NewsContent
    {
        public enum ContentType
        {
            Text = 0,
            Image = 1
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("news_id")]
        public int NewsId { get; set; }

        [Column("data")]
        public byte[] Data { get; set; }

        [Column("type")]
        public int Type { get; set; }
    }
}
