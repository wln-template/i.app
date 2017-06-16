using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Models;

namespace app.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20170616035652_v0")]
    partial class v0
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Models.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<string>("Key")
                        .HasMaxLength(100);

                    b.Property<string>("Value")
                        .HasMaxLength(512);

                    b.HasKey("Id");

                    b.ToTable("Setting");
                });
        }
    }
}
