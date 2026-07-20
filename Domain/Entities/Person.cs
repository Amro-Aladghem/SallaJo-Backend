namespace Domain.Entities;

public class Person
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset? ExpiredTokenTime { get; set; }
    public DateTimeOffset? LastLoggedInTime { get; set; }
    public string? Phone { get; set; }
    public int UserTypeId { get; set; }
    public int? CountryId { get; set; }
    public int? GovernorateId { get; set; }

    public UserType UserType { get; set; } = null!;
    public Country? Country { get; set; }
    public Governorate? Governorate { get; set; }
}
