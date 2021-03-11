﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ringkey.Data;

namespace ringkey.Data.Migrations
{
    [DbContext(typeof(RingkeyDbContext))]
    partial class RingkeyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("ringkey.Common.Models.Messages.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Author")
                        .HasColumnType("longtext");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<long>("Created")
                        .HasColumnType("bigint");

                    b.Property<string>("Parent")
                        .HasColumnType("longtext");

                    b.Property<bool>("Processed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Message");
                });
#pragma warning restore 612, 618
        }
    }
}
