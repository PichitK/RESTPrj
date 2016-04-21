using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("sticker_items")]
    public class StickerItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("sticker_id")]
        public int StickerId { get; set; }
        
        [Column("image")]
        public byte[] Image{ get; set; }

        [Column("extension")]
        public string Extension { get; set; }
    }
}
