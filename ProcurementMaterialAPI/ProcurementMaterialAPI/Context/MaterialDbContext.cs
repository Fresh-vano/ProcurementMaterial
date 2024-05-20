using Microsoft.EntityFrameworkCore;
using ProcurementMaterialAPI.ModelDB;
using System.Numerics;

namespace ProcurementMaterialAPI.Context
{
	public class MaterialDbContext : DbContext
	{
		public MaterialDbContext(DbContextOptions<MaterialDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<UserModel> User { get; set; }

		public DbSet<ModelDok_SF> Dok_SF { get; set; }

		public DbSet<InformationSystemsMatch> InformationSystemsMatch { get; set; }
	}
}
