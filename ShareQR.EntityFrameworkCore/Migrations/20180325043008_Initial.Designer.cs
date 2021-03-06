﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ShareQR.SQLite;
using System;

namespace ShareQR.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(ShareQRDbContext))]
    [Migration("20180325043008_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("ShareQR.Models.QRCodeItem", b =>
                {
                    b.Property<string>("Data")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Path");

                    b.HasKey("Data");

                    b.ToTable("QRCodeItems");
                });
#pragma warning restore 612, 618
        }
    }
}
