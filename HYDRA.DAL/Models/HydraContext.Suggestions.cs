using Microsoft.EntityFrameworkCore;

namespace HYDRA.DAL.Models;

public partial class HydraContext
{
    public virtual DbSet<GameSuggestion> GameSuggestions { get; set; } = null!;
    public virtual DbSet<GameSuggestionVote> GameSuggestionVotes { get; set; } = null!;

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameSuggestion>(entity =>
        {
            entity.ToTable("GameSuggestions");
            entity.HasKey(e => e.SuggestionId);
            entity.Property(e => e.SuggestionId).HasColumnName("SuggestionID");
            entity.Property(e => e.Title).HasMaxLength(255).IsRequired();
            entity.Property(e => e.PlatformText).HasMaxLength(255);
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.ApprovedByUserId).HasColumnName("ApprovedByUserID");
            entity.Property(e => e.Status).HasDefaultValue((byte)0);

            entity.Property(e => e.RawgSlug).HasMaxLength(200);
            entity.Property(e => e.RawgName).HasMaxLength(255);
            entity.Property(e => e.RawgBackgroundImg).HasMaxLength(600);
            entity.Property(e => e.RawgPlatforms).HasMaxLength(600);
            entity.Property(e => e.RawgGenres).HasMaxLength(400);

            entity.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(e => e.ApprovedByUser)
                .WithMany()
                .HasForeignKey(e => e.ApprovedByUserId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<GameSuggestionVote>(entity =>
        {
            entity.ToTable("GameSuggestionVotes");
            entity.HasKey(e => new { e.SuggestionId, e.UserId });
            entity.Property(e => e.SuggestionId).HasColumnName("SuggestionID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.HasIndex(v => new { v.SuggestionId, v.UserId }).IsUnique();
            entity.HasOne(v => v.Suggestion)
                .WithMany(s => s.Votes)
                .HasForeignKey(v => v.SuggestionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(v => v.User)
                .WithMany()
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
