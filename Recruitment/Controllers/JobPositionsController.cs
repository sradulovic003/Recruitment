using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.Features.JobPositions.Commands;
using Recruitment.Application.Features.JobPositions.Queries;

namespace Recruitment.API.Controllers
{
    [Route("job-positions")]
    [ApiController]
    public class JobPositionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public JobPositionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _mediator.Send(new GetJobPositionsQuery()));


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJobPositionCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }
    }
}
