namespace Domain.Entities;

public class Country
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<Governorate> Governorates { get; set; } = new List<Governorate>();
    public ICollection<Store> Stores { get; set; } = new List<Store>();
    public ICollection<Person> Persons { get; set; } = new List<Person>();
}
