
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystemEFCore.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        
        public int? LeaveRequestId { get; set; }

      
        [Required]
        [StringLength(50)]
        public string Action { get; set; } = string.Empty;

       
        [Required]
        [StringLength(100)]
        public string PerformedBy { get; set; } = string.Empty;

        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

        
        [StringLength(500)]
        public string? Details { get; set; }
    }
}
