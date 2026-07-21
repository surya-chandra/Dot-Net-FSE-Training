






using EFCoreLab03.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFCoreLab03.Migrations;

[DbContext(typeof(ApplicationDbContext))]
partial class ApplicationDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("EFCoreLab03.Models.Category", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");
            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<string>("Description")
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            b.HasKey("Id");
            b.HasIndex("Name").IsUnique();
            b.ToTable("Categories");
        });

        modelBuilder.Entity("EFCoreLab03.Models.Supplier", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");
            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<string>("Email")
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("nvarchar(150)");

            b.Property<string>("Phone")
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            b.HasKey("Id");
            b.ToTable("Suppliers");
        });

        modelBuilder.Entity("EFCoreLab03.Models.Product", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");
            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<int>("CategoryId").HasColumnType("int");
            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");
            b.Property<decimal>("Price").HasColumnType("decimal(18,2)");
            b.Property<int>("StockQuantity").HasColumnType("int");
            b.Property<int>("SupplierId").HasColumnType("int");

            b.HasKey("Id");
            b.HasIndex("CategoryId");
            b.HasIndex("SupplierId");
            b.ToTable("Products");
        });

        modelBuilder.Entity("EFCoreLab03.Models.Product", b =>
        {
            b.HasOne("EFCoreLab03.Models.Category", "Category")
                .WithMany("Products")
                .HasForeignKey("CategoryId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            b.HasOne("EFCoreLab03.Models.Supplier", "Supplier")
                .WithMany("Products")
                .HasForeignKey("SupplierId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            b.Navigation("Category");
            b.Navigation("Supplier");
        });

        modelBuilder.Entity("EFCoreLab03.Models.Category", b =>
        {
            b.Navigation("Products");
        });

        modelBuilder.Entity("EFCoreLab03.Models.Supplier", b =>
        {
            b.Navigation("Products");
        });
#pragma warning restore 612, 618
    }
}
