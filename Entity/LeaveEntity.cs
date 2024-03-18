using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LeaveManagement.Entity
{
    [Table("Leaves")]
    public class LeaveEntity
    {
        [Key]
        public int LeaveId { get; set; }
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        //[NotInFuture]
        public DateTime StartDate {  get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        //[NotInFuture]
        public DateTime EndDate { get; set; }

        public int NumberOfDays { get; set; }

        public string Status {  get; set; }
        public UserEntity User { get; set; }
        public int UserId { get; set; }
        public string ApprovedBy { get; set; }
        [Required]
        public string Reason { get; set; }
    }
}