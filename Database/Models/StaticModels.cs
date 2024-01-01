using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firewatch.Database.Models;

/// <summary>
/// A type representation of temeletry metric - a type and measurement combination, used to identify temeletry in the database.
/// </summary>
public class Metric
{
    [Key]
    [Column(Order = 1)]
    public int      Id                  { get; private set; }
    [Column(Order = 2)]
    public string   Name                { get; private set; }
    [Column(Order = 3)]
    public string   Description         { get; private set; }
    [Column(Order = 4)]
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

/// <summary>
/// Sort-of static model - a few base rows are default, but others can be added by user (like external drives)
/// </summary>
public class Resource
{
    [Key]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int      Id                  { get; private set; }
    [Column(Order = 2)]
    public string   Name                { get; private set; }
    [Column(Order = 3)]
    public string   Description         { get; private set; }

    public Resource() {}

    public Resource(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}