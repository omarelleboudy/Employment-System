using Business.Base;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.ComponentModel.DataAnnotations;

namespace EmploymentSystem.Controllers
{
    [Route("api/[controller]")]

    public class VacancyApplicationController : ControllerBase
    {
        private readonly IVacancyApplicationService _vacancyApplicationService;

        public VacancyApplicationController(IVacancyApplicationService vacancyApplicationService)
        {
            _vacancyApplicationService = vacancyApplicationService;
        }

        [HttpPost]
        [Authorize(Roles = "Applicant")]
        [Route("Apply")]
        public async Task<IActionResult> ApplyForVacancy([FromForm] ApplicationModel model)
        {
            var loggedInApplicantId = this.User.Claims.First(i => i.Type == "UserId").Value;

            var payload = await _vacancyApplicationService.ApplyForVacancyAsync(model, loggedInApplicantId);

            return payload.Success ? Ok(payload) : BadRequest(payload);
        }

        [HttpGet]
        [Authorize(Roles = "Applicant")]
        [Route("GetApplicantApplications")]
        public async Task<IActionResult> GetApplications()
        {
            var loggedInApplicantId = this.User.Claims.First(i => i.Type == "UserId").Value;
            var payload = await _vacancyApplicationService.GetMyApplications(loggedInApplicantId);

            return payload.Success ? Ok(payload) : NotFound(payload);
        }

        [HttpGet]
        [Authorize(Roles = "Employer")]
        [Route("GetVacancyApplications")]
        [OutputCache(Duration = 600, PolicyName = "OutputCacheWithAuthPolicy", VaryByQueryKeys = new[] { "*" })]
        public async Task<IActionResult> GetVacancyApplications([Required] [FromQuery]  string vacancyId)
        {
            var loggedInEmployer = this.User.Claims.First(i => i.Type == "UserId").Value;

            var payload = await _vacancyApplicationService.GetCertianVacancyApplications(vacancyId, loggedInEmployer);

            return payload.Success ? Ok(payload) : NotFound(payload);
        }

    }
}
