namespace Domain.Entities;

public class Governorate
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int CountryId { get; set; }

    public Country Country { get; set; } = null!;
    public ICollection<Store> Stores { get; set; } = new List<Store>();
    public ICollection<Person> Persons { get; set; } = new List<Person>();
}
