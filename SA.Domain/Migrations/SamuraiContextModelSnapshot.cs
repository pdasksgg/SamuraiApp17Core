﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SA.Data;

namespace SA.Data.Migrations
{
    [DbContext(typeof(SamuraiContext))]
    partial class SamuraiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SA.Model.Battle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.ToTable("Battles");
                });

            modelBuilder.Entity("SA.Model.Quote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastModified");

                    b.Property<int>("SamuraiId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("SamuraiId");

                    b.ToTable("Quotes");
                });

            modelBuilder.Entity("SA.Model.Samurai", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Samurais");
                });

            modelBuilder.Entity("SA.Model.SamuraiBattle", b =>
                {
                    b.Property<int>("BattleId");

                    b.Property<int>("SamuraiId");

                    b.Property<DateTime>("LastModified");

                    b.HasKey("BattleId", "SamuraiId");

                    b.HasIndex("SamuraiId");

                    b.ToTable("SamuraiBattles");
                });

            modelBuilder.Entity("SA.Model.SecretIdentity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("RealName");

                    b.Property<int>("SamuraiId");

                    b.HasKey("Id");

                    b.HasIndex("SamuraiId")
                        .IsUnique();

                    b.ToTable("SecretIdentity");
                });

            modelBuilder.Entity("SA.Model.Quote", b =>
                {
                    b.HasOne("SA.Model.Samurai", "Samurai")
                        .WithMany("Quotes")
                        .HasForeignKey("SamuraiId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SA.Model.SamuraiBattle", b =>
                {
                    b.HasOne("SA.Model.Battle", "Battle")
                        .WithMany("SamuraiBattles")
                        .HasForeignKey("BattleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SA.Model.Samurai", "Samurai")
                        .WithMany("SamuraiBattles")
                        .HasForeignKey("SamuraiId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SA.Model.SecretIdentity", b =>
                {
                    b.HasOne("SA.Model.Samurai", "Samurai")
                        .WithOne("SecretIdentity")
                        .HasForeignKey("SA.Model.SecretIdentity", "SamuraiId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
