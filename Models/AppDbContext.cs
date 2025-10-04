using CNPMNC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CNPMNC.Data
{
	public class AppDbContext : IdentityDbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<UserAccount> UserAccounts { get; set; } = null!;  // ✅ Thêm = null!
		public DbSet<Ticket> Tickets { get; set; } = null!;  // ✅ Thêm = null!
	}
}