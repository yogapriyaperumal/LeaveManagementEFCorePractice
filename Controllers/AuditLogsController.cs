
using LeaveManagementSystemEFCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystemEFCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuditLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Get all audit logs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _context.AuditLogs
                .OrderByDescending(a => a.PerformedAt).AsNoTracking()
                .ToListAsync();

            return Ok(logs);
        }

        // 2. Get audit logs by leave request ID
        [HttpGet("leave/{leaveRequestId}")]
        public async Task<IActionResult> GetByLeaveRequestId(int leaveRequestId)
        {
            var logs = await _context.AuditLogs
                .Where(a => a.LeaveRequestId == leaveRequestId)
                .OrderByDescending(a => a.PerformedAt)
                .AsNoTracking()
                .ToListAsync();

            return Ok(logs);
        }
    }
}
