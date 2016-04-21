using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("groups")]
    public class Group
    {
        public enum GroupType
        {
            Other = 0,
            Company
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("founder_id")]
        public int FounderId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("subtitle")]
        public string SubTitle { get; set; }

        [Column("wel_msg")]
        public string WelcomeMessage { get; set; }

        [Column("logo")]
        public byte[] Logo { get; set; }
        
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_date")]
        public DateTime? UpdatedDate { get; set; }

    }
}
