
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

        // 2. Get a leave request by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);

            if (leave == null)
                return NotFound(new { message = "Leave request not found." });

            return Ok(leave);
        }

        // 3. Create a leave request
        [HttpPost]
        public async Task<IActionResult> Create(LeaveRequest request)
        {
            if (request.EndDate.Date < request.StartDate.Date)
                return BadRequest(new { message = "End date must not be before start date." });

            request.Status = "Pending";

            _context.LeaveRequests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = request.Id },
                request);
        }

        // 4. Update a leave request
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LeaveRequest request)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);

            if (leave == null)
                return NotFound(new { message = "Leave request not found." });

            if (request.EndDate.Date < request.StartDate.Date)
                return BadRequest(new { message = "End date must not be before start date." });

            leave.EmployeeName = request.EmployeeName;
            leave.LeaveType = request.LeaveType;
            leave.StartDate = request.StartDate;
            leave.EndDate = request.EndDate;
            leave.Reason = request.Reason;
            leave.Status = request.Status;

            await _context.SaveChangesAsync();

            return Ok(leave);
        }

        // 5. Delete a leave request
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);

            if (leave == null)
                return NotFound(new { message = "Leave request not found." });

            _context.LeaveRequests.Remove(leave);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Leave request deleted successfully." });
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
