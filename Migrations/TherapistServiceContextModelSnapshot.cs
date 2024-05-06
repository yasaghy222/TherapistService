﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TherapistService.Data;

#nullable disable

namespace TherapistService.Migrations
{
    [DbContext(typeof(TherapistServiceContext))]
    partial class TherapistServiceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TherapistService.Entities.Clinic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifyAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Clinics");
                });

            modelBuilder.Entity("TherapistService.Entities.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<float>("Latitudes")
                        .HasColumnType("real");

                    b.Property<float>("Longitudes")
                        .HasColumnType("real");

                    b.Property<DateTime>("ModifyAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("TherapistService.Entities.OnlinePlan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<byte>("DayOfWeek")
                        .HasColumnType("tinyint");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time");

                    b.Property<DateTime>("ModifyAt")
                        .HasColumnType("datetime2");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time");

                    b.Property<Guid>("TherapistId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TherapistId");

                    b.ToTable("OnlinePlans");
                });

            modelBuilder.Entity("TherapistService.Entities.Specialty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifyAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Specialties");
                });

            modelBuilder.Entity("TherapistService.Entities.Therapist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("AcceptVisit")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ClinicId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailValidate")
                        .HasColumnType("bit");

                    b.Property<string>("Family")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Gender")
                        .HasColumnType("tinyint");

                    b.Property<bool>("HasTelCounseling")
                        .HasColumnType("bit");

                    b.Property<bool>("HasTextCounseling")
                        .HasColumnType("bit");

                    b.Property<byte>("LineStatus")
                        .HasColumnType("tinyint");

                    b.Property<int>("MedicalSysCode")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifyAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneValidate")
                        .HasColumnType("bit");

                    b.Property<float>("Rate")
                        .HasColumnType("real");

                    b.Property<int>("RaterCount")
                        .HasColumnType("int");

                    b.Property<Guid>("SpecialtyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint");

                    b.Property<int>("SuccessConsolationCount")
                        .HasColumnType("int");

                    b.Property<int>("SuccessReservationCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClinicId");

                    b.HasIndex("SpecialtyId");

                    b.ToTable("Therapists");
                });

            modelBuilder.Entity("TherapistService.Entities.VisitPlan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time");

                    b.Property<DateTime>("ModifyAt")
                        .HasColumnType("datetime2");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("TherapistId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TherapistId");

                    b.ToTable("VisitPlans");
                });

            modelBuilder.Entity("TherapistService.Entities.Clinic", b =>
                {
                    b.HasOne("TherapistService.Entities.Location", "Location")
                        .WithMany("Clinics")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");
                });

            modelBuilder.Entity("TherapistService.Entities.OnlinePlan", b =>
                {
                    b.HasOne("TherapistService.Entities.Therapist", "Therapist")
                        .WithMany("OnlinePlans")
                        .HasForeignKey("TherapistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Therapist");
                });

            modelBuilder.Entity("TherapistService.Entities.Therapist", b =>
                {
                    b.HasOne("TherapistService.Entities.Clinic", "Clinic")
                        .WithMany("Therapists")
                        .HasForeignKey("ClinicId");

                    b.HasOne("TherapistService.Entities.Specialty", "Specialty")
                        .WithMany("Therapists")
                        .HasForeignKey("SpecialtyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clinic");

                    b.Navigation("Specialty");
                });

            modelBuilder.Entity("TherapistService.Entities.VisitPlan", b =>
                {
                    b.HasOne("TherapistService.Entities.Therapist", "Doctor")
                        .WithMany("VisitPlans")
                        .HasForeignKey("TherapistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("TherapistService.Entities.Clinic", b =>
                {
                    b.Navigation("Therapists");
                });

            modelBuilder.Entity("TherapistService.Entities.Location", b =>
                {
                    b.Navigation("Clinics");
                });

            modelBuilder.Entity("TherapistService.Entities.Specialty", b =>
                {
                    b.Navigation("Therapists");
                });

            modelBuilder.Entity("TherapistService.Entities.Therapist", b =>
                {
                    b.Navigation("OnlinePlans");

                    b.Navigation("VisitPlans");
                });
#pragma warning restore 612, 618
        }
    }
}