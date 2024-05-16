using PersonDirectory.Application.PersonManagement.DTOs;
using PersonDirectory.Domain.PersonManagement;

namespace PersonDirectory.Application.PersonManagement.Shared.Helpers;

public static class DataValidationHelpers
{
    public static bool IsValidPhoneNumbers(this IEnumerable<PhoneNumberDTO>? phoneNumbers)
    {
        if (phoneNumbers == null || !phoneNumbers.Any())
            return true;

        return phoneNumbers.All(x => x.Number.All(char.IsDigit)) &&
               phoneNumbers.All(x => x.Type != PhoneNumberType.None && x.Number.Length >= 4 && x.Number.Length <= 50);
    }
}
