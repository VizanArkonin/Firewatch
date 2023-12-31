using Microsoft.EntityFrameworkCore;

using Firewatch.Database.Models;
using Firewatch.Core.Logging;

namespace Firewatch.Database;

/// <summary>
/// Application DB context
/// NOTE: We use SQLite by default, since it is normal for raspberry devices to often go offline, so we'd like to make sure we store the
/// accumulated telemetry.
/// For the time being there are no mechanisms to sync it with some outside service/db - it is to be done when the need arises.
/// </summary>
public class FirewatchContext : DbContext
{
    private LoggingChannel          Log                 { get; set; }
    public string                   DbPath              { get; }

    /// Static tables
    public DbSet<Metric>            Metrics             { get; set; }
    public DbSet<Resource>          Resources           { get; set; }

    /// Dynamic tables
    public DbSet<TickTelemetry>     Telemetry           { get; set; }
    public DbSet<HourlyTelemetry>   HourlyTelemetry     { get; set; }
    public DbSet<DailyTelemetry>    DailyTelemetry      { get; set; }
    public DbSet<MonthlyTelemetry>  MonthlyTelemetry    { get; set; }

    public FirewatchContext()
    {
        this.DbPath = System.IO.Path.Join(Environment.CurrentDirectory, "firewatch.db");
    }

    public void SetLogger(LoggingChannel loggingChannel) => Log = loggingChannel;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
        // Debugging options - enable full trace and direct it to a logger
        //optionsBuilder.EnableSensitiveDataLogging(true);
        //optionsBuilder.LogTo(Log.Trace);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Metric>().HasData(
            new Metric(1, "Percent", "Percentage of resource used", "%"),
            new Metric(2, "Megabytes Used", "Amount of Megabytes used by resource", "mB"),
            new Metric(3, "Total Megabytes available", "Total amount of Megabytes available to resource", "mB"),
            new Metric(4, "Bytes sent", "Amount of bytes, sent by given resource", "b"),
            new Metric(5, "Bytes received", "Amount of bytes, received by given resource", "b"),
            new Metric(6, "Bytes sent per second", "The speed of transmission for given resource", "b/sec"),
            new Metric(7, "Bytes received per second", "The speed of reception for given resource", "b/sec"),
            new Metric(8, "Temperature", "Resource temperature", "°C")
        );

        modelBuilder.Entity<Resource>().HasData(
            new Resource(1, "CPU", "System CPU. Static resource"),
            new Resource(2, "RAM", "System RAM. Static resource"),
            new Resource(3, "Disk", "System Disk. Static resource")
        );
    }
}