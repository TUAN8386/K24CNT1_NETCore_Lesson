using Microsoft.EntityFrameworkCore;
using DinhAnhTuan2410900083_exam.Models;

namespace DinhAnhTuan2410900083_exam.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<DATStudent> DATStudents { get; set; }
    }
}