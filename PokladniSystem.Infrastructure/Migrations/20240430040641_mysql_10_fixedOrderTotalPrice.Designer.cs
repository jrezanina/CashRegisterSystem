﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokladniSystem.Infrastructure.Database;

#nullable disable

namespace PokladniSystem.Infrastructure.Migrations
{
    [DbContext(typeof(CRSDbContext))]
    [Migration("20240430040641_mysql_10_fixedOrderTotalPrice")]
    partial class mysql_10_fixedOrderTotalPrice
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Potraviny"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Drogerie"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Pečivo"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Mléčné výrobky"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Nápoje"
                        });
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ContactId")
                        .HasColumnType("int");

                    b.Property<string>("DIC")
                        .HasColumnType("longtext");

                    b.Property<string>("ICO")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("Company");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ContactId = 1,
                            DIC = "CZ70883521",
                            ICO = "70883521",
                            Name = "Univerzita Tomáše Bati ve Zlíně"
                        });
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BuildingNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Street")
                        .HasColumnType("longtext");

                    b.Property<string>("Web")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Contacts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BuildingNumber = "5555",
                            City = "Zlín",
                            Email = "podatelna@utb.cz",
                            Phone = "+420 576 038 120",
                            PostalCode = "760 01",
                            Street = "nám. T. G. Masaryka",
                            Web = "www.utb.cz"
                        },
                        new
                        {
                            Id = 2,
                            BuildingNumber = "4511",
                            City = "Zlín",
                            Email = "dekanat@fai.utb.cz",
                            Phone = "+420 576 035 221",
                            PostalCode = "760 05",
                            Street = "Nad Stráněmi",
                            Web = "www.fai.utb.cz"
                        });
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTimeCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ReceiptSrc")
                        .HasColumnType("longtext");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<double?>("TotalPrice")
                        .HasColumnType("double");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("EanCode")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("PriceSale")
                        .HasColumnType("double");

                    b.Property<double>("PriceVAT")
                        .HasColumnType("double");

                    b.Property<double>("PriceVATFree")
                        .HasColumnType("double");

                    b.Property<string>("SellerCode")
                        .HasColumnType("longtext");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("VATRateId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VATRateId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Rohlík tukový 43g",
                            PriceSale = 2.5,
                            PriceVAT = 1.6799999999999999,
                            PriceVATFree = 1.5,
                            SellerCode = "2207",
                            ShortName = "Rohlík tukový 43g",
                            VATRateId = 2
                        },
                        new
                        {
                            Id = 2,
                            Name = "Chléb konzumní 1000g",
                            PriceSale = 39.899999999999999,
                            PriceVAT = 30.800000000000001,
                            PriceVATFree = 27.5,
                            SellerCode = "2701",
                            ShortName = "Chléb konzumní 1000g",
                            VATRateId = 2
                        },
                        new
                        {
                            Id = 3,
                            Name = "Kobliha s náplní meruňka 70g",
                            PriceSale = 11.5,
                            PriceVAT = 7.2800000000000002,
                            PriceVATFree = 6.5,
                            SellerCode = "2003",
                            ShortName = "Kobliha s n.mer. 70g",
                            VATRateId = 2
                        },
                        new
                        {
                            Id = 4,
                            Name = "Francouzská bageta 150g",
                            PriceSale = 15.9,
                            PriceVAT = 11.199999999999999,
                            PriceVATFree = 10.0,
                            SellerCode = "2024",
                            ShortName = "Franc. bageta 150g",
                            VATRateId = 2
                        },
                        new
                        {
                            Id = 5,
                            EanCode = "9788071963455",
                            Name = "Trvanlivé mléko plnotučné 3,5% 1l",
                            PriceSale = 24.899999999999999,
                            PriceVAT = 17.920000000000002,
                            PriceVATFree = 16.0,
                            ShortName = "T.mléko pol. 3,5% 1l",
                            VATRateId = 2
                        },
                        new
                        {
                            Id = 6,
                            EanCode = "4014400901191",
                            Name = "Smetanový jogurt borůvka 150g",
                            PriceSale = 15.9,
                            PriceVAT = 10.42,
                            PriceVATFree = 9.3000000000000007,
                            ShortName = "Smet.jog.bor. 150g",
                            VATRateId = 2
                        },
                        new
                        {
                            Id = 7,
                            EanCode = "4014400400007",
                            Name = "Smetanový jogurt jahoda 150g",
                            PriceSale = 15.9,
                            PriceVAT = 10.42,
                            PriceVATFree = 9.3000000000000007,
                            ShortName = "Smet.jog.jah. 150g",
                            VATRateId = 2
                        },
                        new
                        {
                            Id = 8,
                            EanCode = "7622210606754",
                            Name = "Zakysaná smetana 15% 200g",
                            PriceSale = 14.9,
                            PriceVAT = 12.880000000000001,
                            PriceVATFree = 11.5,
                            ShortName = "Zak.smetana 15% 200g",
                            VATRateId = 2
                        },
                        new
                        {
                            Id = 9,
                            EanCode = "5900259128515",
                            Name = "Smetana ke šlehání 31% 200g",
                            PriceSale = 37.899999999999999,
                            PriceVAT = 28.559999999999999,
                            PriceVATFree = 25.5,
                            ShortName = "Smet.ke šl. 31% 200g",
                            VATRateId = 2
                        },
                        new
                        {
                            Id = 10,
                            EanCode = "4000512363835",
                            Name = "Tvaroh polotučný 250g",
                            PriceSale = 32.899999999999999,
                            PriceVAT = 25.420000000000002,
                            PriceVATFree = 22.699999999999999,
                            ShortName = "Tvaroh pol. 250g",
                            VATRateId = 2
                        },
                        new
                        {
                            Id = 11,
                            EanCode = "8000500179864",
                            Name = "Eidam 30% plátky 100g",
                            PriceSale = 32.899999999999999,
                            PriceVAT = 22.399999999999999,
                            PriceVATFree = 20.0,
                            ShortName = "Eidam 30% plát. 100g",
                            VATRateId = 2
                        },
                        new
                        {
                            Id = 12,
                            EanCode = "8594050910072",
                            Name = "Zubní pasta 75ml",
                            PriceSale = 99.900000000000006,
                            PriceVAT = 67.760000000000005,
                            PriceVATFree = 56.0,
                            ShortName = "Zubní pasta 75ml",
                            VATRateId = 3
                        },
                        new
                        {
                            Id = 13,
                            EanCode = "8594003849626",
                            Name = "Tekuté mýdlo dezinfekční 250ml",
                            PriceSale = 54.899999999999999,
                            PriceVAT = 39.200000000000003,
                            PriceVATFree = 35.0,
                            ShortName = "Tek.mýdlo dez. 250ml",
                            VATRateId = 2
                        },
                        new
                        {
                            Id = 14,
                            EanCode = "5053990161669",
                            Name = "Šampon 400ml",
                            PriceSale = 109.90000000000001,
                            PriceVAT = 84.700000000000003,
                            PriceVATFree = 70.0,
                            ShortName = "Šampon 400ml",
                            VATRateId = 3
                        },
                        new
                        {
                            Id = 15,
                            EanCode = "54491472",
                            Name = "Cola 500ml",
                            PriceSale = 24.899999999999999,
                            PriceVAT = 18.149999999999999,
                            PriceVATFree = 15.0,
                            ShortName = "Cola 500ml",
                            VATRateId = 3
                        },
                        new
                        {
                            Id = 16,
                            EanCode = "20504755",
                            Name = "Limonáda 500ml",
                            PriceSale = 21.899999999999999,
                            PriceVAT = 15.73,
                            PriceVATFree = 13.0,
                            ShortName = "Limonáda 500ml",
                            VATRateId = 3
                        });
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.ProductCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductCategories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            ProductId = 1
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 3,
                            ProductId = 1
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 1,
                            ProductId = 2
                        },
                        new
                        {
                            Id = 4,
                            CategoryId = 3,
                            ProductId = 2
                        },
                        new
                        {
                            Id = 5,
                            CategoryId = 1,
                            ProductId = 3
                        },
                        new
                        {
                            Id = 6,
                            CategoryId = 3,
                            ProductId = 3
                        },
                        new
                        {
                            Id = 7,
                            CategoryId = 1,
                            ProductId = 4
                        },
                        new
                        {
                            Id = 8,
                            CategoryId = 3,
                            ProductId = 4
                        },
                        new
                        {
                            Id = 9,
                            CategoryId = 1,
                            ProductId = 5
                        },
                        new
                        {
                            Id = 10,
                            CategoryId = 4,
                            ProductId = 5
                        },
                        new
                        {
                            Id = 11,
                            CategoryId = 1,
                            ProductId = 6
                        },
                        new
                        {
                            Id = 12,
                            CategoryId = 4,
                            ProductId = 6
                        },
                        new
                        {
                            Id = 13,
                            CategoryId = 1,
                            ProductId = 7
                        },
                        new
                        {
                            Id = 14,
                            CategoryId = 4,
                            ProductId = 7
                        },
                        new
                        {
                            Id = 15,
                            CategoryId = 1,
                            ProductId = 8
                        },
                        new
                        {
                            Id = 16,
                            CategoryId = 4,
                            ProductId = 8
                        },
                        new
                        {
                            Id = 17,
                            CategoryId = 1,
                            ProductId = 9
                        },
                        new
                        {
                            Id = 18,
                            CategoryId = 4,
                            ProductId = 9
                        },
                        new
                        {
                            Id = 19,
                            CategoryId = 1,
                            ProductId = 10
                        },
                        new
                        {
                            Id = 20,
                            CategoryId = 4,
                            ProductId = 10
                        },
                        new
                        {
                            Id = 21,
                            CategoryId = 1,
                            ProductId = 11
                        },
                        new
                        {
                            Id = 22,
                            CategoryId = 4,
                            ProductId = 11
                        },
                        new
                        {
                            Id = 23,
                            CategoryId = 2,
                            ProductId = 12
                        },
                        new
                        {
                            Id = 24,
                            CategoryId = 2,
                            ProductId = 13
                        },
                        new
                        {
                            Id = 25,
                            CategoryId = 2,
                            ProductId = 14
                        },
                        new
                        {
                            Id = 26,
                            CategoryId = 1,
                            ProductId = 15
                        },
                        new
                        {
                            Id = 27,
                            CategoryId = 5,
                            ProductId = 15
                        },
                        new
                        {
                            Id = 28,
                            CategoryId = 1,
                            ProductId = 16
                        },
                        new
                        {
                            Id = 29,
                            CategoryId = 5,
                            ProductId = 16
                        });
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ContactId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("Stores");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            ContactId = 2,
                            Name = "Fakulta aplikované informatiky"
                        });
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.Supply", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("StoreId");

                    b.ToTable("Supplies");
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.VATRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Rate")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("VATRates");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Rate = 0
                        },
                        new
                        {
                            Id = 2,
                            Rate = 12
                        },
                        new
                        {
                            Id = 3,
                            Rate = 21
                        });
                });

            modelBuilder.Entity("PokladniSystem.Infrastructure.Identity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("PokladniSystem.Infrastructure.Identity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<int?>("StoreId")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("StoreId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("PokladniSystem.Infrastructure.Identity.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("PokladniSystem.Infrastructure.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("PokladniSystem.Infrastructure.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("PokladniSystem.Infrastructure.Identity.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PokladniSystem.Infrastructure.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("PokladniSystem.Infrastructure.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.Company", b =>
                {
                    b.HasOne("PokladniSystem.Domain.Entities.Contact", "Contact")
                        .WithMany()
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contact");
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.Order", b =>
                {
                    b.HasOne("PokladniSystem.Domain.Entities.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PokladniSystem.Infrastructure.Identity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Store");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.OrderItem", b =>
                {
                    b.HasOne("PokladniSystem.Domain.Entities.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PokladniSystem.Domain.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.Product", b =>
                {
                    b.HasOne("PokladniSystem.Domain.Entities.VATRate", "VATRate")
                        .WithMany()
                        .HasForeignKey("VATRateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VATRate");
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.ProductCategory", b =>
                {
                    b.HasOne("PokladniSystem.Domain.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PokladniSystem.Domain.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.Store", b =>
                {
                    b.HasOne("PokladniSystem.Domain.Entities.Contact", "Contact")
                        .WithMany()
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contact");
                });

            modelBuilder.Entity("PokladniSystem.Domain.Entities.Supply", b =>
                {
                    b.HasOne("PokladniSystem.Domain.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PokladniSystem.Domain.Entities.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId");

                    b.Navigation("Product");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("PokladniSystem.Infrastructure.Identity.User", b =>
                {
                    b.HasOne("PokladniSystem.Domain.Entities.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId");

                    b.Navigation("Store");
                });
#pragma warning restore 612, 618
        }
    }
}
