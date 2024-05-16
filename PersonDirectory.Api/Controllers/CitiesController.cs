using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonDirectory.Application.CityManagement.Commands;
using PersonDirectory.Application.CityManagement.Queries;

namespace PersonDirectory.Api.Controllers;

/// <inheritdoc/>

[ApiController]
[Route("v1/[Controller]")]
public class CitiesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Creates city
    /// </summary>
    /// 
    /// <response code="200">City was created successfully</response>
    /// <response code="400">Request has missing/invalid values</response>
    /// <response code="500">An error occur.Try it again.</response>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// 

    [HttpPost]
    [ProducesResponseType(typeof(AddCityCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddCity([FromBody] AddCityCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return Created("cities/{id}", new { result.Id });
    }

    /// <summary>
    /// Retrieves list of cities
    /// </summary>
    /// 
    /// <response code="200">Cities were retrieved successfully</response>
    /// <response code="400">Request has missing/invalid values</response>
    /// <response code="500">An error occur.Try it again.</response>
    /// <param name="cancellationToken"></param>
    /// 

    [HttpGet]
    [ProducesResponseType(typeof(CitiesQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCities(CancellationToken cancellationToken) =>
         Ok(await _mediator.Send(new CitiesQuery(), cancellationToken));

    /// <summary>
    /// Deletes city by Id
    /// </summary>
    /// 
    /// <response code="204">City was deleted successfully</response>
    /// <response code="400">Request has missing/invalid values</response>
    /// <response code="500">An error occur.Try it again.</response>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// 

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCity(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCityCommand(id), cancellationToken);

        return NoContent();
    }
}
