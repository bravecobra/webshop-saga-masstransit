﻿// <auto-generated />
using System;
using Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Catalog.Persistence.Migrations
{
    [DbContext(typeof(CatalogDbContext))]
    partial class CatalogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Catalog.Domain.Products.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Catalog.Domain.Products.Product", b =>
                {
                    b.OwnsOne("Catalog.Domain.Products.PriceInfo.PriceInfo", "PriceInfo", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTimeOffset>("LastChanged")
                                .HasColumnType("datetimeoffset");

                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("Id");

                            b1.HasIndex("ProductId")
                                .IsUnique();

                            b1.ToTable("Prices");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");

                            b1.OwnsOne("Catalog.Domain.Products.PriceInfo.Price", "LatestPrice", b2 =>
                                {
                                    b2.Property<Guid>("PriceInfoId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<float>("Amount")
                                        .HasColumnType("real");

                                    b2.Property<string>("Currency")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.HasKey("PriceInfoId");

                                    b2.ToTable("Prices");

                                    b2.WithOwner()
                                        .HasForeignKey("PriceInfoId");
                                });

                            b1.Navigation("LatestPrice")
                                .IsRequired();
                        });

                    b.OwnsOne("Catalog.Domain.Products.StockInfo.StockInfo", "StockInfo", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("CurrentAmountAvailable")
                                .HasColumnType("int");

                            b1.Property<DateTimeOffset>("LastChanged")
                                .HasColumnType("datetimeoffset");

                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("Id");

                            b1.HasIndex("ProductId")
                                .IsUnique();

                            b1.ToTable("StockInfo");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.Navigation("PriceInfo")
                        .IsRequired();

                    b.Navigation("StockInfo")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
