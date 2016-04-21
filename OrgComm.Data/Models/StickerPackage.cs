using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("sticker_packages")]
    public class StickerPackage
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("thumbnail")]
        public byte[] Thumbnail { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
        
        public virtual IList<StickerItem> Items { get; set; }
    }
}
