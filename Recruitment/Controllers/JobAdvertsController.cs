using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.Features.JobAdverts.Commands;
using Recruitment.Application.Features.JobAdverts.Queries;

namespace Recruitment.API.Controllers
{
    [Route("job-adverts")]
    [ApiController]
    public class JobAdvertsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobAdvertsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetActive()
        {
            var result = await _mediator.Send(new GetActiveJobAdvertsQuery());
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
            => Ok(await _mediator.Send(new GetAllJobAdvertsQuery()));

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetJobAdvertByIdQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJobAdvertCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateJobAdvertCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id u putanji i telu se ne poklapaju.");

            var success = await _mediator.Send(command);
            return success ? NoContent() : NotFound();
        }

 
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(long id)
        {
            var success = await _mediator.Send(new DeactivateJobAdvertCommand(id));
            return success ? NoContent() : NotFound();
        }
    }
}
