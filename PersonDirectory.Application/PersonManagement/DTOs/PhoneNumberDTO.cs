using PersonDirectory.Domain.PersonManagement;
using PersonDirectory.Domain.PersonManagement.ValueObjects;

namespace PersonDirectory.Application.PersonManagement.DTOs;

public class PhoneNumberDTO
{
    public PhoneNumberType Type { get; set; }
    public string Number { get; set; } = string.Empty;

    public PhoneNumber ToDomainModel() => new(Type, Number);

    public static PhoneNumberDTO From(PhoneNumber phoneNumber) =>
        new()
        {
            Number = phoneNumber.Number,
            Type = phoneNumber.PhoneNumberType
        };
}
