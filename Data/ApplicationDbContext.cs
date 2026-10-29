
using LeaveManagementSystemEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystemEFCore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }
    }
}
