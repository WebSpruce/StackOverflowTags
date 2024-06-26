﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StackOverflowTags.Data;

#nullable disable

namespace StackOverflowTags.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.3");

            modelBuilder.Entity("StackOverflowTags.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "count");

                    b.Property<bool>("Has_Synonyms")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "has_synonyms");

                    b.Property<bool>("Is_Moderator_Only")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "is_moderator_only");

                    b.Property<bool>("Is_Required")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "is_required");

                    b.Property<DateTime?>("Last_Activity_Date")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "last_activity_date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<double>("Percentage")
                        .HasColumnType("REAL")
                        .HasAnnotation("Relational:JsonPropertyName", "percentage");

                    b.Property<string>("Synonyms")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "synonyms");

                    b.Property<int?>("TagsId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("User_Id")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "user_id");

                    b.HasKey("Id");

                    b.HasIndex("TagsId");

                    b.ToTable("Tag");

                    b.HasAnnotation("Relational:JsonPropertyName", "items");
                });

            modelBuilder.Entity("StackOverflowTags.Models.Tags", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("StackOverflowTags.Models.Tag", b =>
                {
                    b.HasOne("StackOverflowTags.Models.Tags", null)
                        .WithMany("Items")
                        .HasForeignKey("TagsId");
                });

            modelBuilder.Entity("StackOverflowTags.Models.Tags", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
