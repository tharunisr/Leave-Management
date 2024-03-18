using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LeaveManagement.Entity
{
    [Table("UserTypes")]
    public class UserTypeEntity
    {
        [Key]
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public ICollection<UserEntity> Users { get; set; }
    }
}