﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using api.Data;

#nullable disable

namespace api.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220918185941_ChurchList")]
    partial class ChurchList
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("api.Data.Church", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.ToTable("Churches");
                });

            modelBuilder.Entity("api.Repositories.QrCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsSecret")
                        .HasColumnType("boolean");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.Property<int>("QrCodeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("QrCodes");
                });

            modelBuilder.Entity("api.Repositories.Score", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.Property<int?>("TeamId")
                        .HasColumnType("integer");

                    b.Property<int?>("TeamId1")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("TeamId1");

                    b.ToTable("Score");
                });

            modelBuilder.Entity("api.Repositories.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ChurchName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("FirstScannedQrCode")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastScannedQrCode")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Members")
                        .HasColumnType("integer");

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("api.Repositories.QrCode", b =>
                {
                    b.OwnsOne("api.Repositories.FunFact", "FunFact", b1 =>
                        {
                            b1.Property<int>("QrCodeId")
                                .HasColumnType("integer");

                            b1.Property<string>("Content")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Title")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("QrCodeId");

                            b1.ToTable("QrCodes");

                            b1.WithOwner()
                                .HasForeignKey("QrCodeId");
                        });

                    b.Navigation("FunFact")
                        .IsRequired();
                });

            modelBuilder.Entity("api.Repositories.Score", b =>
                {
                    b.HasOne("api.Repositories.Team", null)
                        .WithMany("Posts")
                        .HasForeignKey("TeamId");

                    b.HasOne("api.Repositories.Team", null)
                        .WithMany("SecretsFound")
                        .HasForeignKey("TeamId1");
                });

            modelBuilder.Entity("api.Repositories.Team", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("SecretsFound");
                });
#pragma warning restore 612, 618
        }
    }
}