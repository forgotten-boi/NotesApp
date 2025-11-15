using Microsoft.EntityFrameworkCore;
namespace TagsApi.Data;
 

public class TagsDbContext(DbContextOptions<TagsDbContext> options) : DbContext(options)
{
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(70);
            entity.Property(e => e.Color).IsRequired().HasMaxLength(10);
            entity.Property(e => e.NoteId).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e=>e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).IsRequired(false);

            entity.HasIndex(e => e.NoteId);
        });
    }
}