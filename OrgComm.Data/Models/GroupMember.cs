using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("group_members")]
    public class GroupMember
    {
        //See type 'friend_status' of table 'lookup' for reference value
        public enum JoinedStatusType
        {
            Invited = 0,
            Requested = 1,
            Active = 2,
            Block = 3,
            Suspend = 4
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Column("group_id", Order = 0)]
        public int GroupId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Column("member_id", Order = 1)]
        public int MemberId { get; set; }

        [Column("inviter_member_id")]
        public int? InviterMemberId { get; set; }

        [Column("inviter_message")]
        public string InviterMessage { get; set; }

        [Column("joined_status")]
        public int JoinedStatus { get; set; }

        [Column("joined_date")]
        public DateTime? JoinedDate { get; set; }
    }
}
