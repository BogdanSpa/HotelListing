// <auto-generated />
using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HotelListing.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220418095713_SeedingSomeData")]
    partial class SeedingSomeData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("HotelListing.Data.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Countries");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Romania"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Jamaica"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Franta"
                        });
                });

            modelBuilder.Entity("HotelListing.Data.Hotel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Hotels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Sector1",
                            CountryId = 1,
                            Name = "Continental",
                            Rating = 3.2000000000000002
                        },
                        new
                        {
                            Id = 2,
                            Address = "Cetatuie",
                            CountryId = 1,
                            Name = "Belvedere",
                            Rating = 7.2000000000000002
                        },
                        new
                        {
                            Id = 3,
                            Address = "Paris",
                            CountryId = 3,
                            Name = "Melody",
                            Rating = 8.0999999999999996
                        },
                        new
                        {
                            Id = 4,
                            Address = "Negril",
                            CountryId = 2,
                            Name = "Sandals Resort",
                            Rating = 5.0999999999999996
                        });
                });

            modelBuilder.Entity("HotelListing.Data.Hotel", b =>
                {
                    b.HasOne("HotelListing.Data.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });
#pragma warning restore 612, 618
        }
    }
}
