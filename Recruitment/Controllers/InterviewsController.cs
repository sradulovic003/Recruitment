using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.Features.Interviews.Commands;

namespace Recruitment.API.Controllers
{
    [Route("interviews")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class InterviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InterviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Schedule([FromBody] ScheduleInterviewCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(new { result.Message, result.InterviewId });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _mediator.Send(new DeleteInterviewCommand(id));
            return success ? NoContent() : NotFound();
        }
    }
}
