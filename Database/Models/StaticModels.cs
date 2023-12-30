using System.ComponentModel.DataAnnotations;

namespace Firewatch.Database.Models;

/// <summary>
/// A type representation of temeletry metric - a type and measurement combination, used to identify temeletry in the database.
/// </summary>
public class Metric
{
    [Key]
    public int      Id                  { get; private set; }
    public string   Name                { get; private set; }
    public string   Description         { get; private set; }
    public string   UnitOfMeasurement   { get; private set; }

    public Metric() {}

    public Metric(int id, string name, string description, string untOfMeasurement)
    {
        Id = id;
        Name = name;
        Description = description;
        UnitOfMeasurement = untOfMeasurement;
    }
}