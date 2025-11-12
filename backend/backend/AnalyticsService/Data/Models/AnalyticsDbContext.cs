using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Data.Models;

public partial class AnalyticsDbContext : DbContext
{
    public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Nudge> Nudges { get; set; }

    public virtual DbSet<UsageEvent> UsageEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CS_AS");

        modelBuilder.Entity<Nudge>(entity =>
        {
            entity.ToTable("nudges");

            entity.HasIndex(e => e.UserId, "IX_nudges_user_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Accepted).HasColumnName("accepted");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .HasColumnName("message");
            entity.Property(e => e.NudgeType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nudge_type");
            entity.Property(e => e.Screen)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("screen");
            entity.Property(e => e.TriggerReason)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("trigger_reason");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<UsageEvent>(entity =>
        {
            entity.ToTable("usage_events");

            entity.HasIndex(e => new { e.UserId, e.Timestamp }, "IX_usage_events_user_timestamp").IsDescending(false, true);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.EventType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("event_type");
            entity.Property(e => e.Screen)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("screen");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("timestamp");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
