using TherapistService.Entities;
using Microsoft.EntityFrameworkCore;

namespace TherapistService.Data;

public class TherapistServiceContext(DbContextOptions<TherapistServiceContext> options) : DbContext(options)
{
	public DbSet<Therapist> Therapists { get; set; }
	public DbSet<Location> Locations { get; set; }
	public DbSet<VisitPlan> VisitPlans { get; set; }
	public DbSet<OnlinePlan> OnlinePlans { get; set; }
	public DbSet<Specialty> Specialties { get; set; }
	public DbSet<Clinic> Clinics { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.OnModelCreatingBuilder();
	}
}
