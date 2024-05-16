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
    public interface IVacancyApplicationService
    {
        public Task<IResult<ApplicationDTO>> ApplyForVacancyAsync(ApplicationModel applicationModel, string applicantId);
        public Task<IResult<List<ApplicationDTO>>> GetMyApplications(string applicantId);
        public Task<IResult<List<ApplicationDTO>>> GetCertianVacancyApplications(string vacancyId, string EmployerId);
    }
}
