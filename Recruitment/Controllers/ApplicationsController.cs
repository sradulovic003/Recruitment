using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.Features.Applications.Commands;
using Recruitment.Application.Features.Applications.Queries;
using Recruitment.Domain.Enums;
using Recruitment.Infrastructure.Identity;
using System.Security.Claims;

namespace Recruitment.API.Controllers
{
    [Route("applications")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<User> _userManager;

        public ApplicationsController(IMediator mediator, IWebHostEnvironment env, UserManager<User> userManager)
        {
            _mediator = mediator;
            _env = env;
            _userManager = userManager;
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost]
        public async Task<IActionResult> Apply(
            [FromForm] long jobAdvertId,
            IFormFile cv,
            IFormFile? coverLetter)
        {
            // ko aplicira - iz tokena
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var documents = new List<ApplicationDocumentInput>();

            // CV je obavezan
            if (cv == null || cv.Length == 0)
                return BadRequest("CV je obavezan.");
            documents.Add(new ApplicationDocumentInput(await SaveFileAsync(cv), DocType.CV));

            // propratno pismo je opciono
            if (coverLetter != null && coverLetter.Length > 0)
                documents.Add(new ApplicationDocumentInput(await SaveFileAsync(coverLetter), DocType.CoverLetter));

            var result = await _mediator.Send(new CreateApplicationCommand(userId, jobAdvertId, documents));

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(new { result.Message, result.ApplicationId });
        }

        [Authorize(Roles = "Candidate")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _mediator.Send(new GetMyApplicationsQuery(userId));
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("by-advert/{jobAdvertId}")]
        public async Task<IActionResult> GetByAdvert(long jobAdvertId)
        {
            var result = await _mediator.Send(new GetApplicationsByJobAdvertQuery(jobAdvertId));
            var enriched = new List<object>();
            foreach (var item in result)
            {
                var user = await _userManager.FindByIdAsync(item.CandidateId);
                var ime = user != null ? $"{user.FirstName} {user.LastName}".Trim() : "Nepoznat";
                enriched.Add(new
                {
                    item.Id,
                    item.CandidateId,
                    candidateName = ime,
                    item.AppliedAt,
                    item.Status,
                    item.DocumentsCount,
                    item.Interviews
                });
            }
            return Ok(enriched);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(long id, [FromBody] ApplicationStatus newStatus)
        {
            var success = await _mediator.Send(new ChangeApplicationStatusCommand(id, newStatus));
            return success ? NoContent() : NotFound();
        }

        // pomocna metoda - snimi fajl u folder uploads/ i vrati putanju
        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var uploadsDir = Path.Combine(_env.ContentRootPath, "uploads");
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var fullPath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("uploads", fileName);   // putanja koja se cuva u bazi da bi radilo na svim racunarima
        }
    }
}
