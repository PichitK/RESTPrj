using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("uploads")]
    public class Upload
    {
        public enum UploadType
        {
            Other = 0,
            Video,
            Audio,
            Photo
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Column("id")]
        public string Id { get; set; }
        
        [Column("member_id")]
        public int MemberId { get; set; }
        
        [Column("file")]
        public byte[] File { get; set; }

        [Column("size")]
        public long Size { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("filename")]
        public string Filename { get; set; }

        [Column("media_type")]
        public string MediaType { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

    }
}
