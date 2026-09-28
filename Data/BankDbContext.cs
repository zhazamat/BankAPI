using BankAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Data;

public class BankDbContext : DbContext
{
    public BankDbContext(DbContextOptions<BankDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Phone> Phones => Set<Phone>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Card> Cards => Set<Card>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Clients");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.MiddleName)
                .HasMaxLength(100);

            entity.Property(x => x.Email)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.BirthDate)
                .IsRequired();
        });

        modelBuilder.Entity<Phone>(entity =>
        {
            entity.ToTable("Phones");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Number)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Type)
                .IsRequired();

            entity.HasOne(x => x.Client)
                .WithMany(x => x.Phones)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Accounts");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.AccountNumber)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Currency)
                .HasMaxLength(3)
                .IsRequired();

            entity.Property(x => x.Balance)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(x => x.IsClosed)
                .IsRequired();

            entity.HasIndex(x => x.AccountNumber)
                .IsUnique();

            entity.HasOne(x => x.Client)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.ToTable("Cards");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.CardNumber)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.ExpirationDate)
                .IsRequired();

            entity.Property(x => x.Type)
                .IsRequired();

            entity.Property(x => x.IsBlocked)
                .IsRequired();

            entity.HasIndex(x => x.CardNumber)
                .IsUnique();

            entity.HasOne(x => x.Account)
                .WithMany(x => x.Cards)
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}