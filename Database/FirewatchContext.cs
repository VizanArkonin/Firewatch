using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using Firewatch.Database.Models;

namespace Firewatch.Database;

/// <summary>
/// Application DB context
/// NOTE: We use SQLite by default, since it is normal for raspberry devices to often go offline, so we'd like to make sure we store the
/// accumulated telemetry.
/// For the time being there are no mechanisms to sync it with some outside service/db - it is to be done when the need arises.
/// </summary>
public class FirewatchContext : DbContext
{
    public string DbPath { get; }

    public DbSet<Metric> Metrics { get; set; }

    public FirewatchContext()
    {
        this.DbPath = System.IO.Path.Join(Environment.CurrentDirectory, "firewatch.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Metric>().HasData(
            new Metric
            {
                Id = 1,
                Name = "Percent",
                Description = "Percentage of resource used",
                UnitOfMeasurement = "%"
            },
            new Metric
            {
                Id = 2,
                Name = "Megabytes Used",
                Description = "Amount of Megabytes used by resource",
                UnitOfMeasurement = "mB"
            },
            new Metric
            {
                Id = 3,
                Name = "Total Megabytes available",
                Description = "Total amount of Megabytes available to resource",
                UnitOfMeasurement = "mB"
            },
            new Metric
            {
                Id = 4,
                Name = "Bytes sent",
                Description = "Amount of bytes, sent by given resource",
                UnitOfMeasurement = "b"
            },
            new Metric
            {
                Id = 5,
                Name = "Bytes received",
                Description = "Amount of bytes, received by given resource",
                UnitOfMeasurement = "b"
            },
            new Metric
            {
                Id = 6,
                Name = "Bytes sent per second",
                Description = "The speed of transmission for given resource",
                UnitOfMeasurement = "b/sec"
            },
            new Metric
            {
                Id = 7,
                Name = "Bytes received per second",
                Description = "The speed of reception for given resource",
                UnitOfMeasurement = "b/sec"
            },
            new Metric
            {
                Id = 8,
                Name = "Temperature",
                Description = "Resource temperature",
                UnitOfMeasurement = "°C"
            }
        );
    }
}