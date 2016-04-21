using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("notes")]
    public class Note
    {
        public enum NoteType
        {
            Text = 0,
            Image
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("member_id")]
        public int MemberId { get; set; }

        [Column("text")]
        public string Text { get; set; }
        
        [Column("image")]
        public byte[] Image { get; set; }
        
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}
