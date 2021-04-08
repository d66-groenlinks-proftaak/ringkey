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
    [Migration("20210408085407_AddLocked")]
    partial class AddLocked
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("AccountRole", b =>
                {
                    b.Property<Guid>("AccountId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("RolesId")
                        .HasColumnType("char(36)");

                    b.HasKey("AccountId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("AccountRole");
                });

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

            modelBuilder.Entity("ringkey.Common.Models.Accounts.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Perm")
                        .HasColumnType("int");

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("ringkey.Common.Models.BannedWord", b =>
                {
                    b.Property<string>("Word")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Word");

                    b.ToTable("BannedWords");
                });

            modelBuilder.Entity("ringkey.Common.Models.Messages.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("MessageId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Type")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.ToTable("Attachment");
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

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("Pinned")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Processed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.Property<bool>("locked")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ParentId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("ringkey.Common.Models.Messages.MessageTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Icon")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("MessageId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.ToTable("Tag");
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
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("AccountRole", b =>
                {
                    b.HasOne("ringkey.Common.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ringkey.Common.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ringkey.Common.Models.Accounts.Permission", b =>
                {
                    b.HasOne("ringkey.Common.Models.Role", null)
                        .WithMany("Permissions")
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("ringkey.Common.Models.Messages.Attachment", b =>
                {
                    b.HasOne("ringkey.Common.Models.Messages.Message", null)
                        .WithMany("Attachments")
                        .HasForeignKey("MessageId");
                });

            modelBuilder.Entity("ringkey.Common.Models.Messages.Message", b =>
                {
                    b.HasOne("ringkey.Common.Models.Account", "Author")
                        .WithMany("Messages")
                        .HasForeignKey("AuthorId");

                    b.HasOne("ringkey.Common.Models.Messages.Message", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("Author");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("ringkey.Common.Models.Messages.MessageTag", b =>
                {
                    b.HasOne("ringkey.Common.Models.Messages.Message", "Message")
                        .WithMany("Tags")
                        .HasForeignKey("MessageId");

                    b.Navigation("Message");
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

            modelBuilder.Entity("ringkey.Common.Models.Account", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("Reports");
                });

            modelBuilder.Entity("ringkey.Common.Models.Messages.Message", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Children");

                    b.Navigation("Reports");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("ringkey.Common.Models.Role", b =>
                {
                    b.Navigation("Permissions");
                });
#pragma warning restore 612, 618
        }
    }
}
