﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PasswordManager.Data;

#nullable disable

namespace PasswordManager.Migrations
{
    [DbContext(typeof(PasswordDbContext))]
    [Migration("20230201212430_postgresMigrations10")]
    partial class postgresMigrations10
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseSerialColumns(modelBuilder);

            modelBuilder.Entity("PasswordManager.Models.AccountModel", b =>
                {
                    b.Property<string>("accountId")
                        .HasColumnType("text");

                    b.Property<string>("aesIV")
                        .HasColumnType("text");

                    b.Property<string>("aesKey")
                        .HasColumnType("text");

                    b.Property<string>("insertedDateTime")
                        .HasColumnType("text");

                    b.Property<string>("lastModifiedDateTime")
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("userId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("accountId");

                    b.HasIndex("userId");

                    b.ToTable("PasswordTableEF");
                });

            modelBuilder.Entity("PasswordManager.Models.UserModel", b =>
                {
                    b.Property<string>("userId")
                        .HasColumnType("text");

                    b.Property<string>("aesIV")
                        .HasColumnType("text");

                    b.Property<string>("aesKey")
                        .HasColumnType("text");

                    b.Property<string>("currentJwtToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("tokenCreated")
                        .HasColumnType("text");

                    b.Property<string>("tokenExpires")
                        .HasColumnType("text");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("userId");

                    b.ToTable("UserTableEF");
                });

            modelBuilder.Entity("PasswordManager.Models.AccountModel", b =>
                {
                    b.HasOne("PasswordManager.Models.UserModel", "user")
                        .WithMany("accounts")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("PasswordManager.Models.UserModel", b =>
                {
                    b.Navigation("accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
