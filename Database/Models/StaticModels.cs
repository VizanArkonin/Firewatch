namespace Firewatch.Database.Models;

/// <summary>
/// A type representation of temeletry metric - a type and measurement combination, used to identify temeletry in the database.
/// </summary>
public class Metric
{
    public int      Id                  { get; set; }
    public string   Name                { get; set; }
    public string   Description         { get; set; }
    public string   UnitOfMeasurement   { get; set; }
}