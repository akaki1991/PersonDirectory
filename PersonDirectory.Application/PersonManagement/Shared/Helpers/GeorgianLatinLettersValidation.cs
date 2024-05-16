using FluentValidation;
using FluentValidation.Validators;

namespace PersonDirectory.Application.PersonManagement.Shared.Helpers;

public class GeorgianLatinLettersValidation<T, TCollectionElement> : PropertyValidator<T, string>
{
    public override string Name => "GeorgianLatinLettersValidation";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "{PropertyName} must be only in Georgian or only in Latin letters.";

    public override bool IsValid(ValidationContext<T> context, string value)
    {
        var georgianLetters = LanguageLetters.GeorgianLetters;
        var englishLetters = LanguageLetters.EnglishLetters;

        bool hasGeorgian = value.Any(c => georgianLetters.Contains(c));
        bool hasEnglish = value.Any(c => englishLetters.Contains(c));

        return hasEnglish ^ hasGeorgian;
    }
}
