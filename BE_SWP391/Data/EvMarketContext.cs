using System;
using System.Collections.Generic;
using BE_SWP391.Models;
using BE_SWP391.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BE_SWP391.Data;

public partial class EvMarketContext : DbContext
{
    public EvMarketContext()
    {
    }

    public EvMarketContext(DbContextOptions<EvMarketContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminProfile> AdminProfiles { get; set; }

    public virtual DbSet<Battery> Batterys { get; set; }

    public virtual DbSet<BatteryMetaData> BatteryMetaDatas { get; set; }

    public virtual DbSet<Category> Categorys { get; set; }

    public virtual DbSet<ConsumerProfile> ConsumerProfiles { get; set; }

    public virtual DbSet<Datapackage> Datapackages { get; set; }

    public virtual DbSet<Download> Downloads { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<MetaData> MetaDatas { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<PricingPlan> PricingPlans { get; set; }

    public virtual DbSet<ProviderProfile> ProviderProfiles { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<RegionMetaData> RegionMetaDatas { get; set; }

    public virtual DbSet<RevenueShare> RevenueShares { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SubCategory> SubCategorys { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleMetaData> VehicleMetaDatas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost;uid=sa;pwd=1234567890;database= EV_Market;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminProfile>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__AdminPro__43AA4141208E5128");

            entity.Property(e => e.AdminId)
                .ValueGeneratedNever()
                .HasColumnName("admin_id");
            entity.Property(e => e.Permissions).HasColumnName("permissions");

            entity.HasOne(d => d.Admin).WithOne(p => p.AdminProfile)
                .HasForeignKey<AdminProfile>(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AdminProf__admin__403A8C7D");
        });

        modelBuilder.Entity<Battery>(entity =>
        {
            entity.HasKey(e => e.BatteryId).HasName("PK__Batterys__31C8DB8E45602F2B");

            entity.Property(e => e.BatteryId).HasColumnName("battery_id");
            entity.Property(e => e.BatteryType)
                .HasMaxLength(100)
                .HasColumnName("battery_type");
            entity.Property(e => e.CapacityKWh)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("capacity_kWh");
            entity.Property(e => e.ChargingTime)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("charging_time");
            entity.Property(e => e.CycleLife).HasColumnName("cycle_life");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(100)
                .HasColumnName("manufacturer");
        });

        modelBuilder.Entity<BatteryMetaData>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Battery___3213E83FB56DCF57");

            entity.ToTable("Battery_metaDatas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BatteryId).HasColumnName("battery_id");
            entity.Property(e => e.MetadataId).HasColumnName("metadata_id");

            entity.HasOne(d => d.Battery).WithMany(p => p.BatteryMetaData)
                .HasForeignKey(d => d.BatteryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Battery_m__batte__73BA3083");

            entity.HasOne(d => d.Metadata).WithMany(p => p.BatteryMetaData)
                .HasForeignKey(d => d.MetadataId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Battery_m__metad__74AE54BC");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__D54EE9B4AD7D1161");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(150)
                .HasColumnName("category_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<ConsumerProfile>(entity =>
        {
            entity.HasKey(e => e.ConsumerId).HasName("PK__Consumer__A6D5902D24B8B60E");

            entity.Property(e => e.ConsumerId)
                .ValueGeneratedNever()
                .HasColumnName("consumer_id");
            entity.Property(e => e.BillingAddress)
                .HasMaxLength(255)
                .HasColumnName("billing_address");
            entity.Property(e => e.DownloadLimit).HasColumnName("download_limit");
            entity.Property(e => e.SubscriptionLevel)
                .HasMaxLength(50)
                .HasColumnName("subscription_level");

            entity.HasOne(d => d.Consumer).WithOne(p => p.ConsumerProfile)
                .HasForeignKey<ConsumerProfile>(d => d.ConsumerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ConsumerP__consu__4316F928");
        });

        modelBuilder.Entity<Datapackage>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__Datapack__63846AE8FEA243C5");

            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.LastUpdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("last_update");
            entity.Property(e => e.MetadataId).HasColumnName("metadata_id");
            entity.Property(e => e.PackageName)
                .HasMaxLength(200)
                .HasColumnName("package_name");
            entity.Property(e => e.ReleaseDate).HasColumnName("release_date");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("active")
                .HasColumnName("status");
            entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");
            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.Version)
                .HasMaxLength(50)
                .HasColumnName("version");

            entity.HasOne(d => d.Metadata).WithMany(p => p.Datapackages)
                .HasForeignKey(d => d.MetadataId)
                .HasConstraintName("FK__Datapacka__metad__5CD6CB2B");

            entity.HasOne(d => d.Subcategory).WithMany(p => p.Datapackages)
                .HasForeignKey(d => d.SubcategoryId)
                .HasConstraintName("FK__Datapacka__subca__5BE2A6F2");

            entity.HasOne(d => d.User).WithMany(p => p.Datapackages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Datapacka__userI__5AEE82B9");
        });

        modelBuilder.Entity<Download>(entity =>
        {
            entity.HasKey(e => e.DownloadId).HasName("PK__Download__2EDDE1CD52F4E05E");

            entity.Property(e => e.DownloadId).HasColumnName("download_id");
            entity.Property(e => e.DownloadDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("download_date");
            entity.Property(e => e.FileHash)
                .HasMaxLength(255)
                .HasColumnName("file_hash");
            entity.Property(e => e.FileUrl)
                .HasMaxLength(255)
                .HasColumnName("file_url");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("success")
                .HasColumnName("status");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

            entity.HasOne(d => d.Package).WithMany(p => p.Downloads)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Downloads__packa__114A936A");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Downloads)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Downloads__trans__10566F31");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__7A6B2B8C6D516C01");

            entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Package).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedbacks__packa__6383C8BA");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedbacks__userI__628FA481");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoices__F58DFD499F145BE0");

            entity.HasIndex(e => e.InvoiceNumber, "UQ__Invoices__8081A63A3D92B513").IsUnique();

            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .HasDefaultValue("VND")
                .HasColumnName("currency");
            entity.Property(e => e.DueDate).HasColumnName("due_date");
            entity.Property(e => e.InvoiceNumber)
                .HasMaxLength(100)
                .HasColumnName("invoice_number");
            entity.Property(e => e.IssueDate).HasColumnName("issue_date");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("unpaid")
                .HasColumnName("status");
            entity.Property(e => e.TaxAmount)
                .HasDefaultValue(0.0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("tax_amount");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("total_amount");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoices__userID__01142BA1");
        });

        modelBuilder.Entity<MetaData>(entity =>
        {
            entity.HasKey(e => e.MetadataId).HasName("PK__MetaData__C1088FC4371DD347");

            entity.Property(e => e.MetadataId).HasColumnName("metadata_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FileFormat)
                .HasMaxLength(50)
                .HasColumnName("file_format");
            entity.Property(e => e.FileSize).HasColumnName("file_size");
            entity.Property(e => e.Keywords).HasColumnName("keywords");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__E059842FF4738129");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("sent_at");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("unread")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__userI__4CA06362");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.MethodId).HasName("PK__PaymentM__747727B621EE5B85");

            entity.ToTable("PaymentMethod");

            entity.Property(e => e.MethodId).HasColumnName("method_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.MethodName)
                .HasMaxLength(100)
                .HasColumnName("method_name");
            entity.Property(e => e.Provider)
                .HasMaxLength(100)
                .HasColumnName("provider");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("active")
                .HasColumnName("status");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

            entity.HasOne(d => d.Transaction).WithMany(p => p.PaymentMethods)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentMe__trans__160F4887");
        });

        modelBuilder.Entity<PricingPlan>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PK__PricingP__BE9F8F1D656AD993");

            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity.Property(e => e.AccessType)
                .HasMaxLength(20)
                .HasDefaultValue("one_time")
                .HasColumnName("access_type");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .HasDefaultValue("VND")
                .HasColumnName("currency");
            entity.Property(e => e.Discount)
                .HasDefaultValue(0.0m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("discount");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.PlanName)
                .HasMaxLength(100)
                .HasColumnName("plan_name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("price");

            entity.HasOne(d => d.Package).WithMany(p => p.PricingPlans)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PricingPl__packa__7A672E12");
        });

        modelBuilder.Entity<ProviderProfile>(entity =>
        {
            entity.HasKey(e => e.ProviderId).HasName("PK__Provider__00E213104B36A38A");

            entity.Property(e => e.ProviderId)
                .ValueGeneratedNever()
                .HasColumnName("provider_id");
            entity.Property(e => e.BankAccount)
                .HasMaxLength(100)
                .HasColumnName("bank_account");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .HasColumnName("company_name");
            entity.Property(e => e.ProviderRating)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("provider_rating");
            entity.Property(e => e.TaxId)
                .HasMaxLength(100)
                .HasColumnName("tax_id");
            entity.Property(e => e.TotalRevenue)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total_revenue");

            entity.HasOne(d => d.Provider).WithOne(p => p.ProviderProfile)
                .HasForeignKey<ProviderProfile>(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderP__provi__47DBAE45");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.RegionId).HasName("PK__Regions__01146BAEE375987F");

            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.RegionName)
                .HasMaxLength(100)
                .HasColumnName("region_name");
        });

        modelBuilder.Entity<RegionMetaData>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Region_m__3213E83F3C4D2A58");

            entity.ToTable("Region_metaDatas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MetadataId).HasColumnName("metadata_id");
            entity.Property(e => e.RegionId).HasColumnName("region_id");

            entity.HasOne(d => d.Metadata).WithMany(p => p.RegionMetaData)
                .HasForeignKey(d => d.MetadataId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Region_me__metad__693CA210");

            entity.HasOne(d => d.Region).WithMany(p => p.RegionMetaData)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Region_me__regio__68487DD7");
        });

        modelBuilder.Entity<RevenueShare>(entity =>
        {
            entity.HasKey(e => e.RevenueId).HasName("PK__RevenueS__3DF902E9F062C2D3");

            entity.Property(e => e.RevenueId).HasColumnName("revenue_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.DistributedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("distributed_at");
            entity.Property(e => e.ProviderId).HasColumnName("provider_id");
            entity.Property(e => e.SharePercentage)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("share_percentage");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Transaction).WithMany(p => p.RevenueShares)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RevenueSh__trans__0B91BA14");

            entity.HasOne(d => d.User).WithMany(p => p.RevenueShares)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RevenueSh__userI__0A9D95DB");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__760965CCC3F152EC");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.SubcategoryId).HasName("PK__SubCateg__F7A5CC26CD968344");

            entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.SubcategoryName)
                .HasMaxLength(150)
                .HasColumnName("subcategory_name");

            entity.HasOne(d => d.Category).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__SubCatego__categ__534D60F1");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__85C600AFF66433DC");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .HasDefaultValue("VND")
                .HasColumnName("currency");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("pending")
                .HasColumnName("status");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("transaction_date");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK__Transacti__invoi__06CD04F7");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__CB9A1CDFD0717570");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E6164C73E2505").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Users__F3DBC57248996B18").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("full_name");
            entity.Property(e => e.Organization)
                .HasMaxLength(150)
                .HasColumnName("organization");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__role_id__3D5E1FD2");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("PK__Vehicles__F2947BC173D43DC0");

            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");
            entity.Property(e => e.Brand)
                .HasMaxLength(100)
                .HasColumnName("brand");
            entity.Property(e => e.Model)
                .HasMaxLength(100)
                .HasColumnName("model");
            entity.Property(e => e.RangeKm).HasColumnName("range_km");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<VehicleMetaData>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vehicle___3213E83F38565A4D");

            entity.ToTable("Vehicle_metaDatas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MetadataId).HasColumnName("metadata_id");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

            entity.HasOne(d => d.Metadata).WithMany(p => p.VehicleMetaData)
                .HasForeignKey(d => d.MetadataId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vehicle_m__metad__6EF57B66");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.VehicleMetaData)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vehicle_m__vehic__6E01572D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
