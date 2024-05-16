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
    public class VacancyController : ControllerBase
    {
        private readonly IVacancyService _vacancyService;

        public VacancyController(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }

        [HttpGet]
        [Authorize]
        [Route("Get")]
        [OutputCache(Duration = 60, PolicyName = "OutputCacheWithAuthPolicy", VaryByQueryKeys = new[] { "*" })]
        public async Task<IActionResult> GetVacancy([FromQuery] string vacancyId)
        {
            var payload = await _vacancyService.GetVacancyAsync(vacancyId);

            return payload.Success ? Ok(payload) : NotFound(payload);
        }
        [HttpDelete]
        [Authorize(Roles = "Employer")]
        [Route("Deactivate")]
        public async Task<IActionResult> DeleteVacancy([FromQuery] string vacancyId)
        {
            var loggedInEmployer = this.User.Claims.First(i => i.Type == "UserId").Value;

            var payload = await _vacancyService.DeactivateVacancy(vacancyId, loggedInEmployer);

            return payload.Success ? Ok(payload) : NotFound(payload);
        }
        [HttpPost]
        [Authorize(Roles = "Employer")]
        [Route("Create")]
        public async Task<IActionResult> CreateVacancy([FromBody] VacancyModel model)
        {
            var loggedInEmployer = this.User.Claims.First(i => i.Type == "UserId").Value;

            var payload = await _vacancyService.CreateVacancyAsync(model, loggedInEmployer);

            return payload.Success ? Ok(payload) : BadRequest(payload);
        }
        [HttpPut]
        [Authorize(Roles = "Employer")]
        [Route("Update")]
        public async Task<IActionResult> UpdateVacancy([FromBody] VacancyModel model)
        {
            var loggedInEmployer = this.User.Claims.First(i => i.Type == "UserId").Value;

            var payload = await _vacancyService.UpdateVacancyAsync(model, loggedInEmployer);

            return payload.Success ? Ok(payload) : BadRequest(payload);
        }
        [HttpGet]
        [Authorize]
        [Route("GetVacanciesByEmployer")]
        public async Task<IActionResult> GetVacanciesByEmployer([FromQuery] string employerId, [FromQuery] bool includeExpired = false)
        {
            var payload = await _vacancyService.GetVacanciesByEmployerAsync(employerId, includeExpired);

            return payload.Success ? Ok(payload) : BadRequest(payload);
        }
        [HttpGet]
        [Authorize]
        [Route("Search")]
        [OutputCache(Duration = 60, PolicyName = "OutputCacheWithAuthPolicy", VaryByQueryKeys = new[] { "*" })]
        public async Task<IActionResult> Search([FromQuery] [Required] string vacancyTitle)
        {
            var payload = await _vacancyService.SearchVacanciesAsync(vacancyTitle);
            return payload.Success ? Ok(payload) : NotFound(payload);
        }
    }
}
