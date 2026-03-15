using System;
using System.Collections.Generic;
using WebShopDomain.Model;
using Microsoft.EntityFrameworkCore;

namespace WebShopInfrastructure.Models;

public partial class DbWebShopContext : DbContext
{
    public DbWebShopContext()
    {
    }

    public DbWebShopContext(DbContextOptions<DbWebShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Cartitem> Cartitems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Changetype> Changetypes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderhistory> Orderhistories { get; set; }

    public virtual DbSet<Orderitem> Orderitems { get; set; }

    public virtual DbSet<Orderstatus> Orderstatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=lab1db;Username=bmaletskij;Password=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cart_pkey");

            entity.ToTable("cart");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Customer).WithMany(p => p.Carts)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("cart_customerid_fkey");
        });

        modelBuilder.Entity<Cartitem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cartitem_pkey");

            entity.ToTable("cartitem");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cartid).HasColumnName("cartid");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Cart).WithMany(p => p.Cartitems)
                .HasForeignKey(d => d.Cartid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("cartitem_cartid_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.Cartitems)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("cartitem_productid_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pkey");

            entity.ToTable("category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(100)
                .HasColumnName("categoryname");

            entity.Property(e => e.Categoryinfo)
        .HasMaxLength(255)
        .HasColumnName("categoryinfo");
        });

        modelBuilder.Entity<Changetype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("changetype_pkey");

            entity.ToTable("changetype");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_pkey");

            entity.ToTable("customer");

            entity.HasIndex(e => e.Email, "customer_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adress)
                .HasMaxLength(255)
                .HasColumnName("adress");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(150)
                .HasColumnName("fullname");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .HasColumnName("phone");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Order_pkey");

            entity.ToTable("Order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Orderdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("orderdate");
            entity.Property(e => e.Orderstatusid).HasColumnName("orderstatusid");
            entity.Property(e => e.Totalamount)
                .HasPrecision(10, 2)
                .HasColumnName("totalamount");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("Order_customerid_fkey");

            entity.HasOne(d => d.Orderstatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderstatusid)
                .HasConstraintName("Order_orderstatusid_fkey");
        });

        modelBuilder.Entity<Orderhistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orderhistory_pkey");

            entity.ToTable("orderhistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Changedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("changedat");
            entity.Property(e => e.Changedtypeid).HasColumnName("changedtypeid");
            entity.Property(e => e.Newvalue)
                .HasMaxLength(100)
                .HasColumnName("newvalue");
            entity.Property(e => e.Oldvalue)
                .HasMaxLength(100)
                .HasColumnName("oldvalue");
            entity.Property(e => e.Orderid).HasColumnName("orderid");

            entity.HasOne(d => d.Changedtype).WithMany(p => p.Orderhistories)
                .HasForeignKey(d => d.Changedtypeid)
                .HasConstraintName("orderhistory_changedtypeid_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderhistories)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("orderhistory_orderid_fkey");
        });

        modelBuilder.Entity<Orderitem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orderitem_pkey");

            entity.ToTable("orderitem");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Totalprice)
                .HasPrecision(10, 2)
                .HasColumnName("totalprice");
            entity.Property(e => e.Unitprice)
                .HasPrecision(10, 2)
                .HasColumnName("unitprice");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("orderitem_orderid_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("orderitem_productid_fkey");
        });

        modelBuilder.Entity<Orderstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orderstatus_pkey");

            entity.ToTable("orderstatus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Statusname)
                .HasMaxLength(50)
                .HasColumnName("statusname");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Availableqty).HasColumnName("availableqty");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Price) 
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("product_categoryid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
