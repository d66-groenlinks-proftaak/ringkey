﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ringkey.Data;

namespace ringkey.Data.Migrations
{
    [DbContext(typeof(RingkeyDbContext))]
    [Migration("20210311194336_MessagesRequiresAccount")]
    partial class MessagesRequiresAccount
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("ringkey.Common.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("ringkey.Common.Models.BannedWord", b =>
                {
                    b.Property<string>("Word")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Word");

                    b.ToTable("BannedWords");
                });

            modelBuilder.Entity("ringkey.Common.Models.Messages.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("AuthorId")
                        .HasColumnType("char(36)");

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

                    b.HasIndex("AuthorId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("ringkey.Common.Models.Report", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("MessageId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ReportMessage")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("MessageId");

                    b.ToTable("Report");
                });

            modelBuilder.Entity("ringkey.Common.Models.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("ringkey.Common.Models.Messages.Message", b =>
                {
                    b.HasOne("ringkey.Common.Models.Account", "Author")
                        .WithMany("Messages")
                        .HasForeignKey("AuthorId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("ringkey.Common.Models.Report", b =>
                {
                    b.HasOne("ringkey.Common.Models.Account", "Account")
                        .WithMany("Reports")
                        .HasForeignKey("AccountId");

                    b.HasOne("ringkey.Common.Models.Messages.Message", "Message")
                        .WithMany("Reports")
                        .HasForeignKey("MessageId");

                    b.Navigation("Account");

                    b.Navigation("Message");
                });

            modelBuilder.Entity("ringkey.Common.Models.Role", b =>
                {
                    b.HasOne("ringkey.Common.Models.Account", "Account")
                        .WithMany("Roles")
                        .HasForeignKey("AccountId");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ringkey.Common.Models.Account", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("Reports");

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("ringkey.Common.Models.Messages.Message", b =>
                {
                    b.Navigation("Reports");
                });
#pragma warning restore 612, 618
        }
    }
}
