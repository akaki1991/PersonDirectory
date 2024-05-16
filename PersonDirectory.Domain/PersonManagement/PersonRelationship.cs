using PersonDirectory.Shared.Models;

namespace PersonDirectory.Domain.PersonManagement;

public class PersonRelationship : Entity
{
    public PersonRelationship() { }

    public PersonRelationship(Guid targetPersonId,
        Guid relatedPersonId, 
        PersonRelationshipType personRelationshipType)
    {
        TargetPersonId = targetPersonId;
        RelatedPersonId = relatedPersonId;
        PersonRelationshipType = personRelationshipType;
    }

    public Guid TargetPersonId { get; private set; }
    public Guid RelatedPersonId { get; private set; }
    public PersonRelationshipType PersonRelationshipType { get; private set; }

    public virtual Person? TargetPerson { get; set; }
    public virtual Person? RelatedPerson { get; set; }

    public void ChangeRelationshipType(PersonRelationshipType personRelationshipType) =>
        PersonRelationshipType = personRelationshipType;
}
