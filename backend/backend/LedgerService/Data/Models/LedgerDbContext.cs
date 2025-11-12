using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LedgerService.Data.Models;

public partial class LedgerDbContext : DbContext
{
    public LedgerDbContext()
    {
    }

    public LedgerDbContext(DbContextOptions<LedgerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Beneficiary> Beneficiaries { get; set; }

    public virtual DbSet<SecurityAlert> SecurityAlerts { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ALECER\\SQLEXPRESS;Database=ledger_db;User Id=AleDataBase;Password=123SPEI;Trusted_Connection=False;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CS_AS");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("accounts");

            entity.HasIndex(e => e.AccountNumber, "UQ_accounts_account_number").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("account_number");
            entity.Property(e => e.AccountType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("DEFAULT")
                .HasColumnName("account_type");
            entity.Property(e => e.Balance)
                .HasColumnType("decimal(19, 4)")
                .HasColumnName("balance");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("MXN")
                .HasColumnName("currency");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("last_updated");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<Beneficiary>(entity =>
        {
            entity.ToTable("beneficiaries");

            entity.HasIndex(e => new { e.UserId, e.AccountNumber }, "UQ_beneficiaries_user_account").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("account_number");
            entity.Property(e => e.Alias)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("alias");
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("bank_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.IsFavorite).HasColumnName("is_favorite");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<SecurityAlert>(entity =>
        {
            entity.ToTable("security_alerts");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Acknowledged).HasColumnName("acknowledged");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Flag)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("flag");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .HasColumnName("message");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Transaction).WithMany(p => p.SecurityAlerts)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK_security_alerts_transactions");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("transactions");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(19, 4)")
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("MXN")
                .HasColumnName("currency");
            entity.Property(e => e.FromAccountId).HasColumnName("from_account_id");
            entity.Property(e => e.RiskFlags)
                .IsUnicode(false)
                .HasColumnName("risk_flags");
            entity.Property(e => e.SpeiReference)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("spei_reference");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("PENDING")
                .HasColumnName("status");
            entity.Property(e => e.ToBeneficiaryId).HasColumnName("to_beneficiary_id");

            entity.HasOne(d => d.FromAccount).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.FromAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transactions_accounts");

            entity.HasOne(d => d.ToBeneficiary).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ToBeneficiaryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transactions_beneficiaries");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
