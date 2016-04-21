using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("friends")]
    public class Friend
    {
        //See type 'friend_status' of table 'lookup' for reference value
        public enum StatusType
        {
            Inactive = 0,
            Requested = 1,
            Active = 2,
            Block = 3,
            Hide = 4
        }

        [Key]
        [Column("member_id", Order = 1)]
        public int MemberId { get; set; }

        [Key]
        [Column("friend_member_id", Order = 2)]
        public int FriendMemberId { get; set; }

        [Column("friend_status")]
        public int Status { get; set; }

        [Column("is_favourite")]
        public bool IsFavourite { get; set; }

        [Column("added_date")]
        public DateTime? AddedDate { get; set; }

        [Column("updated_date")]
        public DateTime? UpdatedDate { get; set; }
    }
}
