using Microsoft.EntityFrameworkCore;
using RecordingGrpcService.Domain.Entities;

namespace RecordingGrpcService.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<RecordingSession> RecordingSessions { get; set; } = default!;
    public DbSet<RecordingChunk> RecordingChunks { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
