using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OpenFinanceService.Data.Models;

public partial class OpenFinanceDbContext : DbContext
{
    public OpenFinanceDbContext()
    {
    }

    public OpenFinanceDbContext(DbContextOptions<OpenFinanceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ExternalProduct> ExternalProducts { get; set; }

    public virtual DbSet<OpenFinanceConnection> OpenFinanceConnections { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ALECER\\SQLEXPRESS;Database=openfinance_db;User Id=AleDataBase;Password=123SPEI;Trusted_Connection=False;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CS_AS");

        modelBuilder.Entity<ExternalProduct>(entity =>
        {
            entity.ToTable("external_products");

            entity.HasIndex(e => e.ConnectionId, "IX_products_connection_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Balance)
                .HasColumnType("decimal(19, 4)")
                .HasColumnName("balance");
            entity.Property(e => e.ConnectionId).HasColumnName("connection_id");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("currency");
            entity.Property(e => e.LastSync)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("last_sync");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.NextPaymentAmount)
                .HasColumnType("decimal(19, 4)")
                .HasColumnName("next_payment_amount");
            entity.Property(e => e.NextPaymentDate).HasColumnName("next_payment_date");
            entity.Property(e => e.ProductType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("product_type");
            entity.Property(e => e.Provider)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("provider");

            entity.HasOne(d => d.Connection).WithMany(p => p.ExternalProducts)
                .HasForeignKey(d => d.ConnectionId)
                .HasConstraintName("FK_external_products_connections");
        });

        modelBuilder.Entity<OpenFinanceConnection>(entity =>
        {
            entity.ToTable("open_finance_connections");

            entity.HasIndex(e => e.UserId, "IX_connections_user_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.AuthToken).HasColumnName("auth_token");
            entity.Property(e => e.LastSync)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("last_sync");
            entity.Property(e => e.ProviderName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("provider_name");
            entity.Property(e => e.Scopes)
                .IsUnicode(false)
                .HasColumnName("scopes");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("ACTIVE")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
