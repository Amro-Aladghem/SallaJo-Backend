namespace Domain.Entities;

public class UserType
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<Person> Persons { get; set; } = new List<Person>();
}
