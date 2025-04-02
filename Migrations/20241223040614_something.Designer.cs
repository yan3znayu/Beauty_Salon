﻿// <auto-generated />
using System;
using Beauty_Salon.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Beauty_Salon.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20241223040614_something")]
    partial class something
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Beauty_Salon.Models.Favorites", b =>
                {
                    b.Property<int>("Favorite_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Favorite_ID"));

                    b.Property<int>("Service_ID")
                        .HasColumnType("integer");

                    b.Property<int>("User_ID")
                        .HasColumnType("integer");

                    b.HasKey("Favorite_ID");

                    b.HasIndex("Service_ID");

                    b.HasIndex("User_ID");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Grades", b =>
                {
                    b.Property<int>("Grades_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Grades_ID"));

                    b.Property<int>("Grade")
                        .HasColumnType("integer");

                    b.Property<int>("Specialists_ID")
                        .HasColumnType("integer");

                    b.Property<int>("User_ID")
                        .HasColumnType("integer");

                    b.HasKey("Grades_ID");

                    b.HasIndex("Specialists_ID");

                    b.HasIndex("User_ID");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Reservations", b =>
                {
                    b.Property<int>("Reservation_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Reservation_ID"));

                    b.Property<DateTime>("Appointment_Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<TimeSpan>("Appointment_Time")
                        .HasColumnType("interval");

                    b.Property<DateTime>("Reserv_Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Service_ID")
                        .HasColumnType("integer");

                    b.Property<int>("Specialist_ID")
                        .HasColumnType("integer");

                    b.Property<int>("User_ID")
                        .HasColumnType("integer");

                    b.HasKey("Reservation_ID");

                    b.HasIndex("Service_ID");

                    b.HasIndex("Specialist_ID");

                    b.HasIndex("User_ID");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Roles", b =>
                {
                    b.Property<int>("Role_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Role_ID"));

                    b.Property<string>("Role_Name")
                        .HasColumnType("text");

                    b.HasKey("Role_ID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Services", b =>
                {
                    b.Property<int>("Service_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Service_ID"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ImagePath")
                        .HasColumnType("text");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<string>("Service_Name")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Service_ID");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Specialists", b =>
                {
                    b.Property<int>("Specialists_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Specialists_ID"));

                    b.Property<float>("Average_Grade")
                        .HasColumnType("real");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Specialist_Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Specialists_ID");

                    b.ToTable("Specialists");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Statuses", b =>
                {
                    b.Property<int>("Status_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Status_ID"));

                    b.Property<string>("Status_Name")
                        .HasColumnType("text");

                    b.HasKey("Status_ID");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Users", b =>
                {
                    b.Property<int>("User_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("User_ID"));

                    b.Property<DateTime>("Create_Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Password_Hash")
                        .HasColumnType("text");

                    b.Property<string>("Password_Salt")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<int>("Role_ID")
                        .HasColumnType("integer");

                    b.Property<int>("Status_ID")
                        .HasColumnType("integer");

                    b.Property<string>("User_Fullname")
                        .HasColumnType("text");

                    b.Property<string>("User_Name")
                        .HasColumnType("text");

                    b.HasKey("User_ID");

                    b.HasIndex("Role_ID");

                    b.HasIndex("Status_ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Favorites", b =>
                {
                    b.HasOne("Beauty_Salon.Models.Services", "Service")
                        .WithMany("Favorites")
                        .HasForeignKey("Service_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Beauty_Salon.Models.Users", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("User_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Grades", b =>
                {
                    b.HasOne("Beauty_Salon.Models.Specialists", "Specialist")
                        .WithMany("Grades")
                        .HasForeignKey("Specialists_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Beauty_Salon.Models.Users", "User")
                        .WithMany("Grades")
                        .HasForeignKey("User_ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Specialist");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Reservations", b =>
                {
                    b.HasOne("Beauty_Salon.Models.Services", "Service")
                        .WithMany("Reservations")
                        .HasForeignKey("Service_ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Beauty_Salon.Models.Specialists", "Specialist")
                        .WithMany("Reservations")
                        .HasForeignKey("Specialist_ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Beauty_Salon.Models.Users", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("User_ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Service");

                    b.Navigation("Specialist");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Users", b =>
                {
                    b.HasOne("Beauty_Salon.Models.Roles", "Role")
                        .WithMany("Users")
                        .HasForeignKey("Role_ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Beauty_Salon.Models.Statuses", "Status")
                        .WithMany("Users")
                        .HasForeignKey("Status_ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Roles", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Services", b =>
                {
                    b.Navigation("Favorites");

                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Specialists", b =>
                {
                    b.Navigation("Grades");

                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Statuses", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Beauty_Salon.Models.Users", b =>
                {
                    b.Navigation("Favorites");

                    b.Navigation("Grades");

                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
