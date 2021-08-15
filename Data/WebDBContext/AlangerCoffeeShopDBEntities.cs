using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OnlineShopDemo.Data.WebDBContext
{
    public partial class OnlineShopDemoDBEntities : DbContext
    {
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<OrderItemPool> OrderItemPool { get; set; }
        public virtual DbSet<OrderRecord> OrderRecord { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductSetRecord> ProductSetRecord { get; set; }
        public virtual DbSet<ShoppingCartPool> ShoppingCartPool { get; set; }

        //Constructor for dependency injection interface in Startup.
        public OnlineShopDemoDBEntities(DbContextOptions<OnlineShopDemoDBEntities> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryID)
                    .HasColumnName("CategoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CategoryIntroduction).HasColumnType("text");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.DbaddDate)
                    .HasColumnName("DBAddDate")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<OrderItemPool>(entity =>
            {
                entity.HasKey(e => new { e.ItemRecordID, e.OrderID })
                    .HasName("PK_OrderItemPool");

                entity.ToTable("OrderItemPool");

                entity.HasIndex(e => e.ItemRecordID)
                    .HasName("ItemRecordID")
                    .IsUnique();

                entity.HasIndex(e => e.OrderID)
                    .HasName("OrderItems Belonging");

                entity.Property(e => e.ItemRecordID)
                    .HasColumnName("ItemRecordID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrderID)
                    .HasColumnName("OrderID")
                    .HasColumnType("varchar(40)");

                entity.Property(e => e.OrderedPrice).HasColumnType("decimal(18,0)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.Quanitiy).HasColumnType("int(11)");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnType("varchar(40)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItemPool)
                    .HasForeignKey(d => d.OrderID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("OrderItems Belonging");
            });

            modelBuilder.Entity<OrderRecord>(entity =>
            {
                entity.HasKey(e => e.OrderID)
                    .HasName("OrderID");

                entity.ToTable("OrderRecord");

                entity.Property(e => e.OrderID)
                    .HasColumnName("OrderID")
                    .HasColumnType("varchar(40)");

                entity.Property(e => e.EmailAddress).HasColumnType("varchar(100)");

                entity.Property(e => e.IsShipped)
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.OrderCreateDate)
                    .HasColumnType("timestamp(6)")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.OrdererName)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.ShippingAddress)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.ShippingDate).HasColumnType("timestamp(6)");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,0)");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnType("varchar(30)");

            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => new { e.ProductID, e.CategoryID })
                    .HasName("PK_Products");

                entity.ToTable("Products");

                entity.HasIndex(e => e.CategoryID)
                    .HasName("Belongs to");

                entity.Property(e => e.ProductID)
                    .HasColumnName("ProductID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CategoryID)
                    .HasColumnName("CategoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DbaddDate)
                    .HasColumnName("DBAddDate")
                    .HasColumnType("date");

                entity.Property(e => e.Introduction).HasColumnType("text");

                entity.Property(e => e.Notes).HasColumnType("varchar(100)");

                entity.Property(e => e.PackageSize).HasColumnType("varchar(12)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.PublishDate).HasColumnType("date");

                entity.Property(e => e.Status).HasColumnType("varchar(5)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Belongs to");

                

            });

            modelBuilder.Entity<ProductSetRecord>(entity =>
            {
                entity.HasKey(e => new { e.ProductID, e.CategoryID, e.ProductSetID, e.SetCategory })
                    .HasName("PK_ProductSetRecord");

                entity.ToTable("ProductSetRecord");

                /*entity.HasIndex(e => new { e.ProductSetID, e.SetCategory })
                    .HasName("ProductsBelongsToSst");*/

                entity.Property(e => e.ProductID)
                    .HasColumnName("ProductID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CategoryID)
                    .HasColumnName("CategoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductSetID)
                    .HasColumnName("ProductSetID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SetCategory).HasColumnName("SetCategory").HasColumnType("int(11)");

                entity.Property(e => e.Quantity).HasColumnName("Quantity").HasColumnType("int(11)");

                entity.HasOne(p => p.Product)
                .WithMany(d => d.ProductsINProductSet).HasForeignKey(e => new { e.ProductID, e.CategoryID }).OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("PRIMARY");

                entity.HasOne(p => p.ProductSet).WithMany(d => d.ProductSet).HasForeignKey(e => new { e.ProductSetID, e.SetCategory }).OnDelete(DeleteBehavior.Restrict).HasConstraintName("ProductsBelongsToSst");

            });
            

            modelBuilder.Entity<ShoppingCartPool>(entity =>
            {
                entity.HasKey(e => new { e.ItemID, e.ProductID, e.CategoryID })
                    .HasName("PK_ShoppingCartPool");

                entity.ToTable("ShoppingCartPool");

                entity.HasIndex(e => e.ItemID)
                    .HasName("ItemID")
                    .IsUnique();

                entity.HasIndex(e => new { e.ProductID, e.CategoryID })
                    .HasName("PoolItemProduct");

                entity.Property(e => e.ItemID)
                    .HasColumnName("ItemID")
                    .HasColumnType("varchar(40)");

                entity.Property(e => e.ProductID)
                    .HasColumnName("ProductID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CategoryID)
                    .HasColumnName("CategoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("tinyint(1)");
                    //.HasDefaultValueSql("int(0)");

                entity.Property(e => e.IsOrdered)
                    .HasColumnType("tinyint(1)");
                    //.HasDefaultValueSql("int(0)");

                entity.Property(e => e.Quantity).HasColumnType("int(11)");

                entity.Property(e => e.RecordCreateDate)
                    .HasColumnType("timestamp(6)")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                entity.Property(e => e.UserID)
                    .IsRequired()
                    .HasColumnName("UserID")
                    .HasColumnType("varchar(40)");
            });
        }
    }
}