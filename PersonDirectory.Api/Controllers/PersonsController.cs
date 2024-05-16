using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonDirectory.Application.PersonManagement.Commmands;
using PersonDirectory.Application.PersonManagement.Queries;
using PersonDirectory.Application.Shared;
using PersonDirectory.Shared;

namespace PersonDirectory.Api.Controllers;

/// <inheritdoc/>

[ApiController]
[Route("v1/[Controller]")]
public class PersonsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Creates person
    /// </summary>
    /// 
    /// <response code="200">Person was created successfully</response>
    /// <response code="400">Request has missing/invalid values</response>
    /// <response code="500">An error occur.Try it again.</response>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// 

    [HttpPost]
    [ProducesResponseType(typeof(AddPersonCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddPerson([FromBody] AddPersonCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return Created("persons/{personId}", new { personId = result.Id });
    }

    /// <summary>
    /// Retrieves person by Id
    /// </summary>
    /// 
    /// <response code="200">Person was retrieved successfully</response>
    /// <response code="400">Request has missing/invalid values</response>
    /// <response code="500">An error occur.Try it again.</response>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// 

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AddPersonCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPerson(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _mediator.Send(new GetPersonQuery(id), cancellationToken));
        }
        catch (AppException ex) when (ex.ErrorCode == ErrorCodes.PersonNotFound)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Retrieves filtered person list
    /// </summary>
    /// 
    /// <response code="200">Person list was retrieved successfully</response>
    /// <response code="400">Request has missing/invalid values</response>
    /// <response code="500">An error occur.Try it again.</response>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// 

    [HttpGet]
    [ProducesResponseType(typeof(AddPersonCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPerson([FromQuery] PersonsQuery query, CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(query, cancellationToken));

    /// <summary>
    /// Changes person data
    /// </summary>
    /// 
    /// <response code="200">Person Data was changed successfully</response>
    /// <response code="400">Request has missing/invalid values</response>
    /// <response code="500">An error occur.Try it again.</response>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// 

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ChangePersonCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangePerson([FromRoute] Guid id, [FromBody] ChangePersonCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command with { Id = id }, cancellationToken);

        return Created("persons/{id}", new { result.Id });
    }

    /// <summary>
    /// Deletes person
    /// </summary>
    /// 
    /// <response code="204">Person was deleted successfully</response>
    /// <response code="400">Request has missing/invalid values</response>
    /// <response code="500">An error occur.Try it again.</response>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// 

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePerson([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeletePersonCommand(id), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Creates person relation
    /// </summary>
    /// 
    /// <response code="200">Person relation was created successfully</response>
    /// <response code="400">Request has missing/invalid values</response>
    /// <response code="500">An error occur.Try it again.</response>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// 

    [HttpPost("{id}/relations")]
    [ProducesResponseType(typeof(AddRelatedPersonCommmandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddPersonRealtion(Guid id, [FromBody] AddRelatedPersonCommmand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command with { PersonId = id }, cancellationToken);

        return Created("persons/{personId}", new { result.PersonId });
    }

    /// <summary>
    /// Updates person's relation
    /// </summary>
    /// 
    /// <response code="200">Person's relation was updated successfully</response>
    /// <response code="400">Request has missing/invalid values</response>
    /// <response code="500">An error occur.Try it again.</response>
    /// <param name="id"></param>
    /// <param name="relatedPersonId"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// 

    [HttpPut("{id}/relations/{relatedPersonId}")]
    [ProducesResponseType(typeof(ChangeRelationShipCommandReuslt), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangePersonRealtion(Guid id, Guid relatedPersonId, [FromBody] ChangeRelationShipCommand command, CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(command with { PersonId = id, RelatedPersonId = relatedPersonId }, cancellationToken));

    /// <summary>
    /// Deletes person's relation
    /// </summary>
    /// 
    /// <response code="200">Person's relation was deleted successfully</response>
    /// <response code="400">Request has missing/invalid values</response>
    /// <response code="500">An error occur.Try it again.</response>
    /// <param name="id"></param>
    /// <param name="relatedPersonId"></param>
    /// <param name="cancellationToken"></param>
    /// 

    [HttpDelete("{id}/relations/{relatedPersonId}")]
    [ProducesResponseType(typeof(ChangeRelationShipCommandReuslt), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePersonRealtion(Guid id, Guid relatedPersonId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteRelationshipCommand(id, relatedPersonId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Uploads person's photo
    /// </summary>
    /// 
    /// <response code="200">Flow completed successfully</response>
    /// <response code="400">You did something wrong!</response>
    /// <response code="500">An error occur.Try it again.</response>
    /// <param name="personId"></param>
    /// <param name="photo"></param>
    /// <param name="cancellationToken"></param>
    /// 

    [HttpPost]
    [Route("physical-people/{personId}/photo")]
    [ProducesResponseType(typeof(AddPersonPhotoCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadPersonPhoto(Guid personId, IFormFile photo, CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new AddPersonPhotoCommand(personId, photo), cancellationToken));

    /// <summary>
    /// Get related persons report
    /// </summary>
    /// 
    /// <response code="200">Report was fetched</response>
    /// <response code="400">Request has missing/invalid values</response>
    /// <response code="500">An error occur.Try it again.</response>
    /// <param name="cancellationToken"></param>
    /// 

    [HttpGet]
    [Route("persons/relations/report")]
    [ProducesResponseType(typeof(RelationsReportQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRelatedPeopleReport(CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new RelationsReportQuery(), cancellationToken));
}
