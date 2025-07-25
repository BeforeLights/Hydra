using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace HYDRA.DAL.Models;

public partial class HydraContext : DbContext
{
    public HydraContext()
    {
    }

    public HydraContext(DbContextOptions<HydraContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameImage> GameImages { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<LibraryEntry> LibraryEntries { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Platform> Platforms { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=(local);Database=HYDRA;uid=sa;pwd=1234567890;TrustServerCertificate=True;");

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Build a config object, using our appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .Build();

            // Use the connection string from the config file
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("HYDRA_DB"));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.Property(e => e.CartItemId).HasColumnName("CartItemID");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.DateAdded).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.GameId).HasColumnName("GameID");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK_CartItems_ShoppingCarts");

            entity.HasOne(d => d.Game).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Games");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasIndex(e => e.Name, "UQ__Companie__737584F69C0B445E").IsUnique();

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.CoverArtPath).HasMaxLength(512);
            entity.Property(e => e.IsForSale).HasDefaultValue(true);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Publisher).WithMany(p => p.Games)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK_Games_Companies");

            entity.HasMany(d => d.Developers).WithMany(p => p.GamesNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "GameDeveloper",
                    r => r.HasOne<Company>().WithMany()
                        .HasForeignKey("DeveloperId")
                        .HasConstraintName("FK_GameDevelopers_Companies"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .HasConstraintName("FK_GameDevelopers_Games"),
                    j =>
                    {
                        j.HasKey("GameId", "DeveloperId");
                        j.ToTable("GameDevelopers");
                        j.IndexerProperty<int>("GameId").HasColumnName("GameID");
                        j.IndexerProperty<int>("DeveloperId").HasColumnName("DeveloperID");
                    });

            entity.HasMany(d => d.Genres).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GameGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .HasConstraintName("FK_GameGenres_Genres"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .HasConstraintName("FK_GameGenres_Games"),
                    j =>
                    {
                        j.HasKey("GameId", "GenreId");
                        j.ToTable("GameGenres");
                        j.IndexerProperty<int>("GameId").HasColumnName("GameID");
                        j.IndexerProperty<int>("GenreId").HasColumnName("GenreID");
                    });

            entity.HasMany(d => d.Platforms).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GamePlatform",
                    r => r.HasOne<Platform>().WithMany()
                        .HasForeignKey("PlatformId")
                        .HasConstraintName("FK_GamePlatforms_Platforms"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .HasConstraintName("FK_GamePlatforms_Games"),
                    j =>
                    {
                        j.HasKey("GameId", "PlatformId");
                        j.ToTable("GamePlatforms");
                        j.IndexerProperty<int>("GameId").HasColumnName("GameID");
                        j.IndexerProperty<int>("PlatformId").HasColumnName("PlatformID");
                    });
        });

        modelBuilder.Entity<GameImage>(entity =>
        {
            entity.HasKey(e => e.ImageId);

            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.ImagePath).HasMaxLength(512);

            entity.HasOne(d => d.Game).WithMany(p => p.GameImages)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GameImages_Games");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasIndex(e => e.Name, "UQ__Genres__737584F6D05193D9").IsUnique();

            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<LibraryEntry>(entity =>
        {
            entity.Property(e => e.LibraryEntryId).HasColumnName("LibraryEntryID");
            entity.Property(e => e.DateAdded).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.HoursPlayed).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.LibraryEntries)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LibraryEntries_Games");

            entity.HasOne(d => d.Status).WithMany(p => p.LibraryEntries)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LibraryEntries_Statuses");

            entity.HasOne(d => d.User).WithMany(p => p.LibraryEntries)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LibraryEntries_Users");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PriceAtTimeOfPurchase).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Game).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Games");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Orders");
        });

        modelBuilder.Entity<Platform>(entity =>
        {
            entity.HasIndex(e => e.Name, "UQ__Platform__737584F69D373C94").IsUnique();

            entity.Property(e => e.PlatformId).HasColumnName("PlatformID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B61603FC66780").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.CartId);

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ShoppingCarts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShoppingCarts_Users");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasIndex(e => e.StatusName, "UQ__Statuses__05E7698ACCBE03AD").IsUnique();

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4B51038CE").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534B8EB1AE0").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.RoleId)
                .HasDefaultValue(1)
                .HasColumnName("RoleID");
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
