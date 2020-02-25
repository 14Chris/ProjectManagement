﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProjectManagment.Models;

namespace ProjectManagment.Models.Migrations
{
    [DbContext(typeof(ProjectManagmentContext))]
    [Migration("20200225161054_add-task-state")]
    partial class addtaskstate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ProjectManagment.Models.Models.Event", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("description")
                        .HasColumnType("text");

                    b.Property<DateTime>("end_date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("id_project")
                        .HasColumnType("integer");

                    b.Property<string>("note")
                        .HasColumnType("text");

                    b.Property<DateTime>("start_date")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("id_project");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("ProjectManagment.Models.Models.Project", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("creation_date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("id_creator")
                        .HasColumnType("integer");

                    b.Property<byte[]>("image")
                        .HasColumnType("bytea");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("id_creator");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("ProjectManagment.Models.Models.Task", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("id_project")
                        .HasColumnType("integer");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<int>("state")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.HasIndex("id_project");

                    b.ToTable("Task");
                });

            modelBuilder.Entity("ProjectManagment.Models.Models.Token", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("id_user")
                        .HasColumnType("integer");

                    b.Property<string>("token")
                        .HasColumnType("text");

                    b.Property<int>("type")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.HasIndex("id_user");

                    b.ToTable("Token");
                });

            modelBuilder.Entity("ProjectManagment.Models.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("active")
                        .HasColumnType("boolean");

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.Property<string>("first_name")
                        .HasColumnType("text");

                    b.Property<string>("last_name")
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .HasColumnType("text");

                    b.Property<byte[]>("profile_picture")
                        .HasColumnType("bytea");

                    b.HasKey("id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ProjectManagment.Models.Models.Event", b =>
                {
                    b.HasOne("ProjectManagment.Models.Models.Project", "Project")
                        .WithMany("Events")
                        .HasForeignKey("id_project")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectManagment.Models.Models.Project", b =>
                {
                    b.HasOne("ProjectManagment.Models.Models.User", "Creator")
                        .WithMany("Projects")
                        .HasForeignKey("id_creator")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectManagment.Models.Models.Task", b =>
                {
                    b.HasOne("ProjectManagment.Models.Models.Project", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("id_project")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectManagment.Models.Models.Token", b =>
                {
                    b.HasOne("ProjectManagment.Models.Models.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("id_user")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
