using System.ComponentModel.DataAnnotations;

namespace LeaveManagementEFCore.Models;

public class LeaveRequest
{
    public int Id { get; set; }

  
    public string EmployeeName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LeaveType { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [Required]
    [StringLength(500)]
    public string Reason { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Pending";
}