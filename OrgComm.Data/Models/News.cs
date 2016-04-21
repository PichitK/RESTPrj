using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("news")]
    public class News
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("company_id")]
        public int CompanyId { get; set; }

        [Column("like_count")]
        public int LikeCount { get; set; }

        [Column("comment_count")]
        public  int CommentCount { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_by")]
        public int? UpdatedBy { get; set; }

        [Column("updated_date")]
        public DateTime? UpdatedDate { get; set; }

        public virtual IList<NewsLike> Likes { get; set; }

        public virtual IList<NewsContent> Contents { get; set; }

        public virtual IList<NewsComment> Comments { get; set; }
    }
}
