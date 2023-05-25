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
    [Migration("20230518145443_AddIDToKey")]
    partial class AddIDToKey
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("SmartShortcuts.Models.Key", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT");

                    b.Property<string>("KeyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ShortcutID")
                        .HasColumnType("TEXT");

                    b.Property<string>("VKCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ShortcutID");

                    b.ToTable("Key");
                });

            modelBuilder.Entity("SmartShortcuts.Models.Shortcut", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Action")
                        .HasColumnType("TEXT");

                    b.Property<string>("ShortcutToDisplay")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Shortcuts");
                });

            modelBuilder.Entity("SmartShortcuts.Models.Key", b =>
                {
                    b.HasOne("SmartShortcuts.Models.Shortcut", null)
                        .WithMany("ShortcutKeys")
                        .HasForeignKey("ShortcutID");
                });

            modelBuilder.Entity("SmartShortcuts.Models.Shortcut", b =>
                {
                    b.Navigation("ShortcutKeys");
                });
#pragma warning restore 612, 618
        }
    }
}
