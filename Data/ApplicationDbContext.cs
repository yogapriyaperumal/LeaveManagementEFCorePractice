using LeaveManagementEFCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LeaveManagementSystemEFCore.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<LeaveRequest> LeaveRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.EmployeeName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.LeaveType)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.Reason)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
        });
    }
}