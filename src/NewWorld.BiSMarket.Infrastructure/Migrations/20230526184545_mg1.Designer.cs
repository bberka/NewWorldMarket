﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewWorld.BiSMarket.Infrastructure;

#nullable disable

namespace NewWorld.BiSMarket.Infrastructure.Migrations
{
    [DbContext(typeof(MarketDbContext))]
    [Migration("20230526184545_mg1")]
    partial class mg1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NewWorld.BiSMarket.Core.Entity.CompleteOrderRequest", b =>
                {
                    b.Property<Guid>("OrderGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsCompletionVerifiedByOrderOwner")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCompletionVerifiedByRequester")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.HasKey("OrderGuid", "UserGuid");

                    b.HasIndex("UserGuid");

                    b.ToTable("CompleteOrderRequests");
                });

            modelBuilder.Entity("NewWorld.BiSMarket.Core.Entity.Image", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Bytes")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OcrItemDataResult")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OcrTextResult")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Guid");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("NewWorld.BiSMarket.Core.Entity.Order", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Attributes")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<DateTime?>("CancelledDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CompletedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EstimatedDeliveryTimeHour")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("GearScore")
                        .HasColumnType("int");

                    b.Property<int>("GemId")
                        .HasColumnType("int");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<Guid>("ImageGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsEmptySocket")
                        .HasColumnType("bit");

                    b.Property<bool>("IsGemChangeable")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLimitedToVerifiedUsers")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNamed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsValid")
                        .HasColumnType("bit");

                    b.Property<byte>("ItemType")
                        .HasColumnType("tinyint");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LevelRequirement")
                        .HasColumnType("int");

                    b.Property<string>("Perks")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("Rarity")
                        .HasColumnType("int");

                    b.Property<int>("Region")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Server")
                        .HasColumnType("int");

                    b.Property<int>("Tier")
                        .HasColumnType("int");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.HasIndex("ImageGuid");

                    b.HasIndex("UserGuid");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("NewWorld.BiSMarket.Core.Entity.User", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DiscordId")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("InGameName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("bit");

                    b.Property<bool>("IsValid")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVerifiedAccount")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<int>("Region")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Server")
                        .HasColumnType("int");

                    b.Property<string>("SteamId")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Guid");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NewWorld.BiSMarket.Core.Entity.CompleteOrderRequest", b =>
                {
                    b.HasOne("NewWorld.BiSMarket.Core.Entity.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewWorld.BiSMarket.Core.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NewWorld.BiSMarket.Core.Entity.Order", b =>
                {
                    b.HasOne("NewWorld.BiSMarket.Core.Entity.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewWorld.BiSMarket.Core.Entity.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NewWorld.BiSMarket.Core.Entity.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
