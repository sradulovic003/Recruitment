using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recruitment.API.Services.Screening;
using Recruitment.Domain.Repositories;
using Recruitment.Infrastructure.Identity;

namespace Recruitment.API.Controllers
{
    [Route("screening")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ScreeningController : ControllerBase
    {
        private readonly IScreeningService _screeningService;
        private readonly IUnitOfWork _uow;
        private readonly UserManager<User> _userManager;

        private const string UploadsRoot = @"C:\Users\Sara\source\repos\Recruitment\Recruitment";

        public ScreeningController(IScreeningService screeningService, IUnitOfWork uow, UserManager<User> userManager)
        {
            _screeningService = screeningService;
            _uow = uow;
            _userManager = userManager;
        }

        [HttpPost("application/{applicationId}")]
        public async Task<IActionResult> ScreenOne(long applicationId)
        {
            var application = _uow.Applications.GetByIdWithDetails(applicationId);
            if (application == null)
                return NotFound("Prijava ne postoji.");

            var advert = _uow.JobAdverts.GetByIdWithDetails(application.JobAdvertId);
            if (advert == null)
                return NotFound("Oglas ne postoji.");

            var cv = application.Documents.FirstOrDefault(d =>
                d.DocumentType == Domain.Enums.DocType.CV);
            if (cv == null)
                return BadRequest("Prijava nema prilozen CV.");

            var fullCvPath = Path.Combine(
                UploadsRoot,
                cv.FilePath.Replace('/', Path.DirectorySeparatorChar));

            if (!System.IO.File.Exists(fullCvPath))
                return BadRequest($"CV fajl nije pronadjen: {fullCvPath}");

            var requirements = $"Pozicija: {advert.JobPosition?.Title}\n" +
                               $"Opis: {advert.JobPosition?.Description}\n" +
                               $"Uslovi: {advert.JobPosition?.Requirements}";

            var report = await _screeningService.ScreenOneCvAsync(fullCvPath, requirements);

            if (report == null)
                return StatusCode(503, "Screening servis nije dostupan.");

            return Ok(report);
        }

        [HttpPost("advert/{jobAdvertId}")]
        public async Task<IActionResult> ScreenAll(long jobAdvertId)
        {
            var advert = _uow.JobAdverts.GetByIdWithDetails(jobAdvertId);
            if (advert == null)
                return NotFound("Oglas ne postoji.");

            var applications = _uow.Applications.GetByJobAdvert(jobAdvertId).ToList();
            if (!applications.Any())
                return BadRequest("Nema prijava za ovaj oglas.");

            var cvPaths = new List<string>();
            var imenaPoPath = new Dictionary<string, string>();

            foreach (var app in applications)
            {
                var appWithDocs = _uow.Applications.GetByIdWithDetails(app.Id);
                var cv = appWithDocs?.Documents.FirstOrDefault(d =>
                    d.DocumentType == Domain.Enums.DocType.CV);

                if (cv != null)
                {
                    var fullPath = Path.Combine(
                        UploadsRoot,
                        cv.FilePath.Replace('/', Path.DirectorySeparatorChar));

                    if (System.IO.File.Exists(fullPath))
                    {
                        cvPaths.Add(fullPath);

                        var user = await _userManager.FindByIdAsync(app.UserId);
                        var ime = user != null
                            ? $"{user.FirstName} {user.LastName}".Trim()
                            : "Nepoznat kandidat";
                        imenaPoPath[fullPath] = ime;
                    }
                }
            }

            if (!cvPaths.Any())
                return BadRequest("Nijedna prijava nema pronadjen CV fajl.");

            var requirements = $"Pozicija: {advert.JobPosition?.Title}\n" +
                               $"Opis: {advert.JobPosition?.Description}\n" +
                               $"Uslovi: {advert.JobPosition?.Requirements}";

            var ranking = await _screeningService.ScreenAllCvsAsync(cvPaths, requirements, imenaPoPath);

            if (ranking == null)
                return StatusCode(503, "Screening servis nije dostupan.");

            return Ok(ranking);
        }
    }
}
