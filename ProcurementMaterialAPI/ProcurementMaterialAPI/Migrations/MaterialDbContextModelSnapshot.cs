﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProcurementMaterialAPI.Context;

#nullable disable

namespace ProcurementMaterialAPI.Migrations
{
    [DbContext(typeof(MaterialDbContext))]
    partial class MaterialDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProcurementMaterialAPI.ModelDB.InformationSystemsMatch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BEI")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<int>("CountEnd")
                        .HasColumnType("int");

                    b.Property<int>("CountOutgo")
                        .HasColumnType("int");

                    b.Property<string>("DepartmentCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GroupMaterialCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GroupMaterialName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaterialName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaterialNom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubGroupMaterialCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubGroupMaterialName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("SumEnd")
                        .HasColumnType("real");

                    b.Property<float>("SumOutgo")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("InformationSystemsMatch");
                });

            modelBuilder.Entity("ProcurementMaterialAPI.ModelDB.ModelDok_SF", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direction")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EI")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("INN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("buisnes_consignee")
                        .HasColumnType("int");

                    b.Property<int>("buisnes_number")
                        .HasColumnType("int");

                    b.Property<double>("cost")
                        .HasColumnType("float");

                    b.Property<DateOnly>("date_budat")
                        .HasColumnType("date");

                    b.Property<int>("fact_number")
                        .HasColumnType("int");

                    b.Property<int>("fact_pos")
                        .HasColumnType("int");

                    b.Property<string>("material")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("material_group")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("material_group_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("material_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("material_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("normalization")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("quan")
                        .HasColumnType("float");

                    b.HasKey("id");

                    b.ToTable("Dok_SF");
                });

            modelBuilder.Entity("ProcurementMaterialAPI.ModelDB.UserModel", b =>
                {
                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.Property<string>("UserShortName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserName");

                    b.ToTable("User");
                });
#pragma warning restore 612, 618
        }
    }
}
