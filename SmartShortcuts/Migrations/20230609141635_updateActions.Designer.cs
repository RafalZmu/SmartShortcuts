﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartShortcuts.Models;

#nullable disable

namespace SmartShortcuts.Migrations
{
    [DbContext(typeof(ShortcutsContext))]
    [Migration("20230609141635_updateActions")]
    partial class updateActions
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("SmartShortcuts.Models.Action", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ShortcutID")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ShortcutID");

                    b.ToTable("Actions");
                });

            modelBuilder.Entity("SmartShortcuts.Models.Shortcut", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT");

                    b.Property<string>("ShortcutKeys")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Shortcuts");
                });

            modelBuilder.Entity("SmartShortcuts.Models.Action", b =>
                {
                    b.HasOne("SmartShortcuts.Models.Shortcut", "Shortcut")
                        .WithMany("Actions")
                        .HasForeignKey("ShortcutID");

                    b.Navigation("Shortcut");
                });

            modelBuilder.Entity("SmartShortcuts.Models.Shortcut", b =>
                {
                    b.Navigation("Actions");
                });
#pragma warning restore 612, 618
        }
    }
}