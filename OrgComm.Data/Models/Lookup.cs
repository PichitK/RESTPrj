using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("lookup")]
    public class Lookup
    {
        //See type 'type' of table 'lookup' for reference value
        public enum LookupType
        {
            FriendStatus = 1,
            GroupMemberJoinedStatus = 2,
            GroupType = 3,
            UploadType = 4,
            NoteType = 5,
            NewsContentType = 6
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("type_id")]
        public int TypeId { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("value")]
        public int Value { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}
