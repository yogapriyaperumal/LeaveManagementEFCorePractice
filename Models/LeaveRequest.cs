
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystemEFCore.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }

        [Required]
        public string EmployeeName { get; set; } = string.Empty;

        [Required]
        public string LeaveType { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";
    }
}
