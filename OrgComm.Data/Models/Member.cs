using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrgComm.Data.Models
{
    [Table("members")]
    public class Member
    {
        public enum StatusType
        {
            New = 0,
            Activated,
            Suspended
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("facebook_id")]
        public string FacebookId { get; set; }

        [Column("salt")]
        public string Salt { get; set; }

        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Column("member_status")]
        public int? MemberStatus { get; set; }

        [Column("registered_date")]
        public DateTime RegisteredDate { get; set; }

        [Column("activation_key")]
        public string ActivationKey { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("nickname")]
        public string Nickname { get; set; }

        [Column("display_name")]
        public string DisplayName { get; set; }

        [Column("gender")]
        public string Gender { get; set; }
        
        [Column("phone")]
        public string Phone { get; set; }

        [Column("photo")]
        public byte[] Photo { get; set; }

        [Column("longitude")]
        public string Longitude { get; set; }

        [Column("latitude")]
        public string Latitude { get; set; }

        [Required]
        [Column("company_id")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }        
        public virtual Company Company { get; set; }

        [Required]
        [Column("department_id")]
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }        
        public virtual Department Department { get; set; }

        [Required]
        [Column("position_id")]
        [ForeignKey("Position")]
        public int PositionId { get; set; }        
        public virtual Position Position { get; set; }

        [Required]
        [Column("employee_id")]
        public string EmployeeId { get; set; }

        [Column("updated_date")]
        public DateTime? UpdatedDate { get; set; }

        [Column("del_flag")]
        public bool DelFlag { get; set; }
        
        //public virtual ICollection<Friend> Friends { get; set; } //Use join instead of include(mapping for eager loading)
    }
}
