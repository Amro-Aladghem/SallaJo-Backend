namespace Domain.Entities;

public class Color
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }

    public ICollection<Store> StoresAsPrimary { get; set; } = new List<Store>();
    public ICollection<Store> StoresAsSecondary { get; set; } = new List<Store>();
}
