using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data.Models;

public partial class AuthDbContext : DbContext
{
    public AuthDbContext()
    {
    }

    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessibilityProfile> AccessibilityProfiles { get; set; }

    public virtual DbSet<ConsentRecord> ConsentRecords { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ALECER\\SQLEXPRESS;Database=auth_db;User Id=AleDataBase;Password=123SPEI;Trusted_Connection=False;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CS_AS");

        modelBuilder.Entity<AccessibilityProfile>(entity =>
        {
            entity.ToTable("accessibility_profiles");

            entity.HasIndex(e => e.UserId, "UQ_accessibility_profiles_user_id").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ColorContrastRatio)
                .HasDefaultValue(4.5m)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("color_contrast_ratio");
            entity.Property(e => e.FontScale)
                .HasDefaultValue(1.0m)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("font_scale");
            entity.Property(e => e.NudgingLevel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("medium")
                .HasColumnName("nudging_level");
            entity.Property(e => e.ScreenReaderMode).HasColumnName("screen_reader_mode");
            entity.Property(e => e.Theme)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("light")
                .HasColumnName("theme");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VoiceFeedback).HasColumnName("voice_feedback");

            entity.HasOne(d => d.User).WithOne(p => p.AccessibilityProfile)
                .HasForeignKey<AccessibilityProfile>(d => d.UserId)
                .HasConstraintName("FK_accessibility_profiles_users");
        });

        modelBuilder.Entity<ConsentRecord>(entity =>
        {
            entity.ToTable("consent_records");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Granted).HasColumnName("granted");
            entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("timestamp");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.ConsentRecords)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_consent_records_users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ_users_email").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ_users_phone_number").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Alias)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("alias");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.DemoMode).HasColumnName("demo_mode");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.HashedPassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("hashed_password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.PreferredLanguage)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("es-MX")
                .HasColumnName("preferred_language");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("updated_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
