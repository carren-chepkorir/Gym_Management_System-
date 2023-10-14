﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShapeShift.Models;

#nullable disable

namespace ShapeShift.Migrations
{
    [DbContext(typeof(GymDbContext))]
    [Migration("20230919134030_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ShapeShift.Models.Attendance", b =>
                {
                    b.Property<int>("AttendanceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AttendanceID"));

                    b.Property<DateTime>("AttendanceDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("MemberID")
                        .HasColumnType("int");

                    b.Property<bool>("Present")
                        .HasColumnType("bit");

                    b.Property<int>("ShiftID")
                        .HasColumnType("int");

                    b.HasKey("AttendanceID");

                    b.HasIndex("MemberID");

                    b.HasIndex("ShiftID");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("ShapeShift.Models.Member", b =>
                {
                    b.Property<int>("MemberID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MemberID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TrainerID")
                        .HasColumnType("int");

                    b.HasKey("MemberID");

                    b.HasIndex("TrainerID");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("ShapeShift.Models.Shift", b =>
                {
                    b.Property<int>("ShiftID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShiftID"));

                    b.Property<string>("ShiftName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ShiftTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TrainerID")
                        .HasColumnType("int");

                    b.HasKey("ShiftID");

                    b.HasIndex("TrainerID");

                    b.ToTable("Shifts");
                });

            modelBuilder.Entity("ShapeShift.Models.ShiftRequest", b =>
                {
                    b.Property<int>("ShiftRequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShiftRequestId"));

                    b.Property<int>("MemberID")
                        .HasColumnType("int");

                    b.Property<DateTime>("RequestedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ShiftID")
                        .HasColumnType("int");

                    b.HasKey("ShiftRequestId");

                    b.HasIndex("MemberID");

                    b.HasIndex("ShiftID");

                    b.ToTable("ShiftRequests");
                });

            modelBuilder.Entity("ShapeShift.Models.Trainer", b =>
                {
                    b.Property<int>("TrainerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TrainerID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TrainerID");

                    b.ToTable("Trainers");
                });

            modelBuilder.Entity("ShapeShift.Models.Attendance", b =>
                {
                    b.HasOne("ShapeShift.Models.Member", "Member")
                        .WithMany("Attendances")
                        .HasForeignKey("MemberID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShapeShift.Models.Shift", "Shift")
                        .WithMany()
                        .HasForeignKey("ShiftID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("ShapeShift.Models.Member", b =>
                {
                    b.HasOne("ShapeShift.Models.Trainer", "Trainer")
                        .WithMany("Members")
                        .HasForeignKey("TrainerID");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("ShapeShift.Models.Shift", b =>
                {
                    b.HasOne("ShapeShift.Models.Trainer", "Trainer")
                        .WithMany("Shifts")
                        .HasForeignKey("TrainerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("ShapeShift.Models.ShiftRequest", b =>
                {
                    b.HasOne("ShapeShift.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShapeShift.Models.Shift", "Shift")
                        .WithMany()
                        .HasForeignKey("ShiftID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("ShapeShift.Models.Member", b =>
                {
                    b.Navigation("Attendances");
                });

            modelBuilder.Entity("ShapeShift.Models.Trainer", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("Shifts");
                });
#pragma warning restore 612, 618
        }
    }
}
