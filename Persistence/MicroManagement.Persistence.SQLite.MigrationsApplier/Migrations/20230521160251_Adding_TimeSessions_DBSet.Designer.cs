﻿// <auto-generated />
using System;
using MicroManagement.Persistence.SQLite.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MicroManagement.Persistence.SQLite.MigrationsApplier.Migrations
{
    [DbContext(typeof(MyMicroManagementDbContext))]
    [Migration("20230521160251_Adding_TimeSessions_DBSet")]
    partial class Adding_TimeSessions_DBSet
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("MicroManagement.Persistence.SQLite.Entities.ProjectEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ProjectsTable");
                });

            modelBuilder.Entity("MicroManagement.Persistence.SQLite.Entities.TimeSessionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TimeSessionsTable");
                });

            modelBuilder.Entity("ProjectEntityTimeSessionEntity", b =>
                {
                    b.Property<Guid>("ProjectsId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TimeSessionsId")
                        .HasColumnType("TEXT");

                    b.HasKey("ProjectsId", "TimeSessionsId");

                    b.HasIndex("TimeSessionsId");

                    b.ToTable("ProjectEntityTimeSessionEntity");
                });

            modelBuilder.Entity("ProjectEntityTimeSessionEntity", b =>
                {
                    b.HasOne("MicroManagement.Persistence.SQLite.Entities.ProjectEntity", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MicroManagement.Persistence.SQLite.Entities.TimeSessionEntity", null)
                        .WithMany()
                        .HasForeignKey("TimeSessionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
