using Business.Base;
using Business.DTOs;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IVacancyService
    {
        public Task<IResult<VacancyDTO>> GetVacancyAsync(string vacancyId);
        public Task<IResult> DeactivateVacancy(string vacancyId, string EmployerId);
        public Task<IResult<VacancyDTO>> CreateVacancyAsync(VacancyModel vacancyModel, string EmployerId);
        public Task<IResult<VacancyDTO>> UpdateVacancyAsync(VacancyModel vacancyModel, string EmployerId);
        public Task<IResult<List<VacancyDTO>>> GetVacanciesByEmployerAsync(string employerId, bool includeExpired = false);
        public Task<IResult<List<VacancyDTO>>> SearchVacanciesAsync(string vacancyTitle);
        public Task<IResult<List<VacancyDTO>>> ArchiveExpiredVacanciesAsync();
    }
}
