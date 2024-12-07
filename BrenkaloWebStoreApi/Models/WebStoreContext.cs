using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BrenkaloWebStoreApi.Models;

public partial class WebStoreContext : DbContext
{
    public WebStoreContext()
    {
    }

    public WebStoreContext(DbContextOptions<WebStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryTranslation> CategoryTranslations { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDescription> ProductDescriptions { get; set; }

    public virtual DbSet<ProductDescriptionType> ProductDescriptionTypes { get; set; }

    public virtual DbSet<ProductPicture> ProductPictures { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<ProductTranslation> ProductTranslations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    public virtual DbSet<Vatrate> Vatrates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=WebStore.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Enabled)
                .HasDefaultValue(1)
                .HasColumnName("enabled");
            entity.Property(e => e.ParentId)
                .HasDefaultValueSql("NULL")
                .HasColumnName("parent_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<CategoryTranslation>(entity =>
        {
            entity.HasIndex(e => new { e.CategoryId, e.LanguageId }, "IX_CategoryTranslations_category_id_language_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasDefaultValueSql("NULL")
                .HasColumnName("description");
            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Category).WithMany(p => p.CategoryTranslations).HasForeignKey(d => d.CategoryId);

            entity.HasOne(d => d.Language).WithMany(p => p.CategoryTranslations).HasForeignKey(d => d.LanguageId);
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasIndex(e => e.Code, "IX_Languages_code").IsUnique();

            entity.HasIndex(e => new { e.Code, e.Name }, "IX_Languages_code_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.CodeTable)
                .HasDefaultValueSql("NULL")
                .HasColumnName("code_table");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Enabled)
                .HasDefaultValue(1)
                .HasColumnName("enabled");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BillingAddress)
                .HasDefaultValueSql("NULL")
                .HasColumnName("billing_address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerEmail).HasColumnName("customer_email");
            entity.Property(e => e.CustomerName).HasColumnName("customer_name");
            entity.Property(e => e.CustomerNotes)
                .HasDefaultValueSql("NULL")
                .HasColumnName("customer_notes");
            entity.Property(e => e.CustomerPhone)
                .HasDefaultValueSql("NULL")
                .HasColumnName("customer_phone");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0.0)
                .HasColumnName("discount_amount");
            entity.Property(e => e.OrderShippingMethod).HasColumnName("order_shipping_method");
            entity.Property(e => e.OrderStatusId).HasColumnName("order_status_id");
            entity.Property(e => e.PaymentMethod)
                .HasDefaultValue("credit_card")
                .HasColumnName("payment_method");
            entity.Property(e => e.ShippingAddress).HasColumnName("shipping_address");
            entity.Property(e => e.ShippingTrackingCode)
                .HasDefaultValueSql("NULL")
                .HasColumnName("shipping_tracking_code");
            entity.Property(e => e.TotalAmount).HasColumnName("total_amount");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId)
                .HasDefaultValueSql("NULL")
                .HasColumnName("user_id");
            entity.Property(e => e.VatAmount)
                .HasDefaultValue(0.0)
                .HasColumnName("vat_amount");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.Sku, "IX_Products_sku").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AverageRating)
                .HasDefaultValue(0.0)
                .HasColumnName("average_rating");
            entity.Property(e => e.Brand)
                .HasDefaultValueSql("NULL")
                .HasColumnName("brand");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Enabled)
                .HasDefaultValue(1)
                .HasColumnName("enabled");
            entity.Property(e => e.IsFeatured)
                .HasDefaultValue(0)
                .HasColumnName("is_featured");
            entity.Property(e => e.IsOnSale)
                .HasDefaultValue(0)
                .HasColumnName("is_on_sale");
            entity.Property(e => e.IsVisible)
                .HasDefaultValue(1)
                .HasColumnName("is_visible");
            entity.Property(e => e.ItemStorage)
                .HasDefaultValue(0)
                .HasColumnName("item_storage");
            entity.Property(e => e.MainPictureUrl)
                .HasDefaultValueSql("NULL")
                .HasColumnName("main_picture_url");
            entity.Property(e => e.MainProductUrl)
                .HasDefaultValueSql("NULL")
                .HasColumnName("main_product_url");
            entity.Property(e => e.Manufacturer)
                .HasDefaultValueSql("NULL")
                .HasColumnName("manufacturer");
            entity.Property(e => e.MaximumOrderQuantity)
                .HasDefaultValueSql("NULL")
                .HasColumnName("maximum_order_quantity");
            entity.Property(e => e.MinimumOrderQuantity)
                .HasDefaultValue(1)
                .HasColumnName("minimum_order_quantity");
            entity.Property(e => e.ModelNumber)
                .HasDefaultValueSql("NULL")
                .HasColumnName("model_number");
            entity.Property(e => e.NumberOfReviews)
                .HasDefaultValue(0)
                .HasColumnName("number_of_reviews");
            entity.Property(e => e.Popularity)
                .HasDefaultValue(0)
                .HasColumnName("popularity");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.SaleEndDate)
                .HasDefaultValueSql("NULL")
                .HasColumnName("sale_end_date");
            entity.Property(e => e.SalePrice)
                .HasDefaultValueSql("NULL")
                .HasColumnName("sale_price");
            entity.Property(e => e.SaleStartDate)
                .HasDefaultValueSql("NULL")
                .HasColumnName("sale_start_date");
            entity.Property(e => e.Sku).HasColumnName("sku");
            entity.Property(e => e.StockStatus)
                .HasDefaultValue("in_stock")
                .HasColumnName("stock_status");
            entity.Property(e => e.SubcategoryId)
                .HasDefaultValueSql("NULL")
                .HasColumnName("subcategory_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.VatId)
                .HasDefaultValueSql("NULL")
                .HasColumnName("vat_id");

            entity.HasOne(d => d.Category).WithMany(p => p.ProductCategories).HasForeignKey(d => d.CategoryId);

            entity.HasOne(d => d.Subcategory).WithMany(p => p.ProductSubcategories)
                .HasForeignKey(d => d.SubcategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Vat).WithMany(p => p.Products)
                .HasForeignKey(d => d.VatId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ProductDescription>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DescriptionTypeId).HasColumnName("description_type_id");
            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.DescriptionType).WithMany(p => p.ProductDescriptions).HasForeignKey(d => d.DescriptionTypeId);

            entity.HasOne(d => d.Language).WithMany(p => p.ProductDescriptions).HasForeignKey(d => d.LanguageId);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductDescriptions).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<ProductDescriptionType>(entity =>
        {
            entity.HasIndex(e => e.TypeName, "IX_ProductDescriptionTypes_type_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Enabled)
                .HasDefaultValue(1)
                .HasColumnName("enabled");
            entity.Property(e => e.TypeName).HasColumnName("type_name");
        });

        modelBuilder.Entity<ProductPicture>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Alt)
                .HasDefaultValueSql("NULL")
                .HasColumnName("alt");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasDefaultValueSql("NULL")
                .HasColumnName("description");
            entity.Property(e => e.Enabled)
                .HasDefaultValue(1)
                .HasColumnName("enabled");
            entity.Property(e => e.IsDefault)
                .HasDefaultValue(0)
                .HasColumnName("is_default");
            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.ParentPictureId)
                .HasDefaultValueSql("NULL")
                .HasColumnName("parent_picture_id");
            entity.Property(e => e.PictureType)
                .HasDefaultValue("full")
                .HasColumnName("picture_type");
            entity.Property(e => e.PictureUrl).HasColumnName("picture_url");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.SortOrder)
                .HasDefaultValue(0)
                .HasColumnName("sort_order");
            entity.Property(e => e.Title)
                .HasDefaultValueSql("NULL")
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.ParentPicture).WithMany(p => p.InverseParentPicture)
                .HasForeignKey(d => d.ParentPictureId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPictures).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ReviewText).HasColumnName("review_text");
            entity.Property(e => e.Stars).HasColumnName("stars");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Username).HasColumnName("username");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductReviews).HasForeignKey(d => d.ProductId);

            entity.HasOne(d => d.User).WithMany(p => p.ProductReviews).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<ProductTranslation>(entity =>
        {
            entity.HasIndex(e => new { e.ProductId, e.LanguageId }, "IX_ProductTranslations_product_id_language_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(0)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Language).WithMany(p => p.ProductTranslations).HasForeignKey(d => d.LanguageId);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductTranslations).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_Users_email").IsUnique();

            entity.HasIndex(e => e.UserGuid, "IX_Users_userGUID").IsUnique();

            entity.HasIndex(e => e.Username, "IX_Users_username").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Enabled)
                .HasDefaultValue(1)
                .HasColumnName("enabled");
            entity.Property(e => e.Firstname)
                .HasDefaultValueSql("NULL")
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasDefaultValueSql("NULL")
                .HasColumnName("lastname");
            entity.Property(e => e.Pwd).HasColumnName("pwd");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserGuid).HasColumnName("userGUID");
            entity.Property(e => e.UserLanguageId)
                .HasDefaultValueSql("NULL")
                .HasColumnName("user_language_id");
            entity.Property(e => e.UserRole).HasColumnName("user_role");
            entity.Property(e => e.Username).HasColumnName("username");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressLine1).HasColumnName("address_line1");
            entity.Property(e => e.AddressLine2).HasColumnName("address_line2");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.Enabled)
                .HasDefaultValue(1)
                .HasColumnName("enabled");
            entity.Property(e => e.IsDefault)
                .HasDefaultValue(0)
                .HasColumnName("is_default");
            entity.Property(e => e.PostalCode).HasColumnName("postal_code");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserAddresses).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.RequestIp)
                .HasDefaultValueSql("NULL")
                .HasColumnName("request_IP");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ValidUntil).HasColumnName("valid_until");

            entity.HasOne(d => d.User).WithMany(p => p.UserSessions).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Vatrate>(entity =>
        {
            entity.ToTable("VATRates");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.Enabled)
                .HasDefaultValue(1)
                .HasColumnName("enabled");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.VatRate).HasColumnName("vat_rate");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
