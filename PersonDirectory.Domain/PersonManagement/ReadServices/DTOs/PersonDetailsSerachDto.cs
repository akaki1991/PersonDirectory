namespace PersonDirectory.Domain.PersonManagement.ReadServices.DTOs;

public class PersonDetailsSerachDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PersonalNumber { get; set; }
    public string? City { get; set; }
    public DateTime? DateOfBirthStartRange { get; set; }
    public DateTime? DateOfBirthEndRange { get; set; }
    public string? PhoneNumber { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
}