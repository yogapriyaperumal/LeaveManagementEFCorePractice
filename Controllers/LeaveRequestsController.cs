
using LeaveManagementSystemEFCore.Data;
using LeaveManagementSystemEFCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystemEFCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Get all leave requests
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var leaves = await _context.LeaveRequests.ToListAsync();

            return Ok(leaves);
        }

        // 2. Get leave request by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);

            if (leave == null)
                return NotFound(new { message = "Leave request not found." });

            return Ok(leave);
        }

        // 3. Create leave request
        [HttpPost]
        public async Task<IActionResult> Create(LeaveRequest request)
        {
            if (request.EndDate.Date < request.StartDate.Date)
                return BadRequest(new
                {
                    message = "End date must not be before start date."
                });

            request.Status = "Pending";

            _context.LeaveRequests.Add(request);

            await _context.SaveChangesAsync();

            // Create audit log
            var audit = new AuditLog
            {
                LeaveRequestId = request.Id,
                Action = "Created",
                PerformedBy = request.EmployeeName,
                Details = "New leave request submitted."
            };

            _context.AuditLogs.Add(audit);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = request.Id },
                request);
        }

        // 4. Update leave request
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LeaveRequest request)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);

            if (leave == null)
                return NotFound(new { message = "Leave request not found." });

            if (request.EndDate.Date < request.StartDate.Date)
                return BadRequest(new
                {
                    message = "End date must not be before start date."
                });

            leave.EmployeeName = request.EmployeeName;
            leave.LeaveType = request.LeaveType;
            leave.StartDate = request.StartDate;
            leave.EndDate = request.EndDate;
            leave.Reason = request.Reason;

            // Keep existing status unchanged during normal update
            // Status changes can be handled through approval/rejection APIs.

            var audit = new AuditLog
            {
                LeaveRequestId = leave.Id,
                Action = "Updated",
                PerformedBy = leave.EmployeeName,
                Details = "Leave request details updated."
            };

            _context.AuditLogs.Add(audit);

            await _context.SaveChangesAsync();

            return Ok(leave);
        }

        // 5. Delete leave request
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);

            if (leave == null)
                return NotFound(new { message = "Leave request not found." });

            // Save audit information before deleting the request
            var audit = new AuditLog
            {
                LeaveRequestId = leave.Id,
                Action = "Deleted",
                PerformedBy = leave.EmployeeName,
                Details = "Leave request deleted."
            };

            _context.AuditLogs.Add(audit);

            _context.LeaveRequests.Remove(leave);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Leave request deleted successfully."
            });
        }

        // 6. Filter leave requests by status
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(string status)
        {
            var leaves = await _context.LeaveRequests
                .Where(l => l.Status.ToLower() == status.ToLower())
                .ToListAsync();

            return Ok(leaves);
        }
    }
}
