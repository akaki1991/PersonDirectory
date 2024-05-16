namespace PersonDirectory.Shared;

public enum ErrorCodes
{
    None = 0,
    PersonNotFound = 1,
    PersonAndRelatedPersonAreSame = 2,
    RelatedPersonNotFound = 3,
    RelationshipAlreadyExists = 4,
    RelationshipDoesNotExist = 5,
    FileDoesNotExist = 6,
    CityNotFound = 7,
    InvalidId = 8,
    InvalidPhoneNumber = 9,
    InvalidFirstName = 10,
    InvalidLastName = 11,
    InvalidGender = 12,
    InvalidPersonalNumber = 13,
    InvalidAge = 14,
    InvalidFileName = 15,
    InvalidPerson = 16,
    InvalidRelationshipType = 17,
    InvalidCityName = 18,
    InvalidPageSize = 19,
    InvalidPage = 20,
    InvalidPersonId = 21,
    InvalidRelatedPersonId = 22,
}
