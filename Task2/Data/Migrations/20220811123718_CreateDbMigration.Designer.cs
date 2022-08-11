﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Task2.Data;

#nullable disable

namespace Task2.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220811123718_CreateDbMigration")]
    partial class CreateDbMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Task2.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("AccountGroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Task2.Models.AccountBalance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccountGroupId")
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<double>("Credit")
                        .HasColumnType("float");

                    b.Property<double>("Debit")
                        .HasColumnType("float");

                    b.Property<double>("IncomingActive")
                        .HasColumnType("float");

                    b.Property<double>("IncomingPassive")
                        .HasColumnType("float");

                    b.Property<int>("LoadedListId")
                        .HasColumnType("int");

                    b.Property<double>("OutgoingActive")
                        .HasColumnType("float");

                    b.Property<double>("OutgoingPassive")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("AccountsBalances");
                });

            modelBuilder.Entity("Task2.Models.AccountGroup", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("OperationsClassId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AccountGroups");
                });

            modelBuilder.Entity("Task2.Models.AccountGroupBalance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccountGroupId")
                        .HasColumnType("int");

                    b.Property<double>("Credit")
                        .HasColumnType("float");

                    b.Property<double>("Debit")
                        .HasColumnType("float");

                    b.Property<double>("IncomingActive")
                        .HasColumnType("float");

                    b.Property<double>("IncomingPassive")
                        .HasColumnType("float");

                    b.Property<int>("LoadedListId")
                        .HasColumnType("int");

                    b.Property<double>("OutgoingActive")
                        .HasColumnType("float");

                    b.Property<double>("OutgoingPassive")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("AccountGroupsBalances");
                });

            modelBuilder.Entity("Task2.Models.FullListBalance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("Credit")
                        .HasColumnType("float");

                    b.Property<double>("Debit")
                        .HasColumnType("float");

                    b.Property<double>("IncomingActive")
                        .HasColumnType("float");

                    b.Property<double>("IncomingPassive")
                        .HasColumnType("float");

                    b.Property<int>("LoadedListId")
                        .HasColumnType("int");

                    b.Property<double>("OutgoingActive")
                        .HasColumnType("float");

                    b.Property<double>("OutgoingPassive")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("FullListsBalances");
                });

            modelBuilder.Entity("Task2.Models.LoadedList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PeriodFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PeriodTo")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("LoadedLists");
                });

            modelBuilder.Entity("Task2.Models.OperationsClass", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OperationsClasses");
                });

            modelBuilder.Entity("Task2.Models.OperationsClassBalance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("Credit")
                        .HasColumnType("float");

                    b.Property<double>("Debit")
                        .HasColumnType("float");

                    b.Property<double>("IncomingActive")
                        .HasColumnType("float");

                    b.Property<double>("IncomingPassive")
                        .HasColumnType("float");

                    b.Property<int>("LoadedListId")
                        .HasColumnType("int");

                    b.Property<int>("OperationsClassId")
                        .HasColumnType("int");

                    b.Property<double>("OutgoingActive")
                        .HasColumnType("float");

                    b.Property<double>("OutgoingPassive")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("OperationsClassesBalances");
                });
#pragma warning restore 612, 618
        }
    }
}