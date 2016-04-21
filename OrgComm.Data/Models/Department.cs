using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace OrgComm.Data.Models
{
    [Table("department")]
    public class Department
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("company_id")]
        public int CompanyId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("note")]
        public string Note { get; set; }
    }
}
