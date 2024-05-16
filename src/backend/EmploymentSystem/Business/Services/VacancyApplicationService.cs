using Business.Base;
using Business.DTOs;
using Business.Models;
using Data.Entities;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Business.Services
{
    public class VacancyApplicationService : IVacancyApplicationService
    {
        private readonly IVacancyApplicationRepository _vacancyApplicationRepository;
        private readonly IVacancyRepository _vacancyRepository;

        public VacancyApplicationService(IVacancyRepository vacancyRepository, IVacancyApplicationRepository vacancyApplicationRepository)
        {
            _vacancyApplicationRepository = vacancyApplicationRepository;
            _vacancyRepository = vacancyRepository;
        }
        public async Task<IResult<List<ApplicationDTO>>> GetMyApplications(string applicantId)
        {
            var result = new Result<List<ApplicationDTO>>();

            var applications = await _vacancyApplicationRepository.QueryAsync(query =>
            query.Where(v => v.ApplicantId == applicantId).Include(a => a.Vacancy));

            if (applications == null || !applications.Any())
                return result.FailedWithError(new Error(ErrorCode.NotFound, ErrorMessage.APPLICANT_APPLICATIONS_NOT_FOUND));

            var applicationsDTO = new List<ApplicationDTO>();
            foreach (var application in applications)
            {
                var applicationDTO = MapApplicationEntityToDTO(application, application.Vacancy);
                applicationsDTO.Add(applicationDTO);
            }
            return result.SucceededWithPayload(applicationsDTO);
        }
        // Difference betwee ndates in hours

        public async Task<IResult<ApplicationDTO>> ApplyForVacancyAsync(ApplicationModel applicationModel, string applicantId)
        {
            var result = new Result<ApplicationDTO>();
            if (applicationModel.Errors().Any())
                return result.FailedWithErrors(ErrorCode.InvalidData, applicationModel.Errors());


            var vacancy = await _vacancyRepository.GetAsync(v => v.Id == applicationModel.VacancyId && v.IsActive);
            if (vacancy == null)
                return result.FailedWithError(new Error(ErrorCode.NotFound, ErrorMessage.VACANCY_NOT_FOUND));

            var recentUserApplication = await _vacancyApplicationRepository.QueryAsync(query =>
                query.Where(v => v.ApplicantId == applicantId && v.VacancyId == applicationModel.VacancyId));
            
            if(recentUserApplication.Any())
                return result.FailedWithError(new Error(ErrorCode.LimitViolation, ErrorMessage.ALREADY_APPLIED));

            if (vacancy.ExpiredAt <= DateTime.UtcNow)
                return result.FailedWithError(new Error(ErrorCode.Error, ErrorMessage.VACANCY_EXPIRED));

            if (vacancy.CurrentApplicants == vacancy.MaxApplicants)
                return result.FailedWithError(new Error(ErrorCode.LimitViolation, ErrorMessage.VACANCY_FULL));

            string resumeBase64 = GetBase64ResumeData(applicationModel);

            var application = new VacancyApplication
            {
                VacancyId = applicationModel.VacancyId,
                Name = applicationModel.FullName,
                ApplicantId = applicantId,
                Email = applicationModel.Email,
                CoverLetter = applicationModel.CoverLetter,
                AdditionalInformation = applicationModel.AdditionalInformation,
                Resume = resumeBase64,
                CreatedAt = DateTime.UtcNow,
            };
            var entity = await _vacancyApplicationRepository.AddAsync(application);

            if (entity == null)
                return result.FailedWithError(new Error(ErrorCode.Error, ErrorMessage.DATABASE_UPDATE_FAILED));

            vacancy.CurrentApplicants++;
            var success = await _vacancyRepository.UpdateAsync(vacancy);

            var applicationDTO = MapApplicationEntityToDTO(entity, vacancy);

            return result.SucceededWithPayload(applicationDTO);
        }

        public async Task<IResult<List<ApplicationDTO>>> GetCertianVacancyApplications(string vacancyId, string EmployerId)
        {
            var result = new Result<List<ApplicationDTO>>();

            var vacancy = await _vacancyRepository.GetAsync(v => v.Id == vacancyId && v.IsActive);
            if (vacancy == null)
                return result.FailedWithError(new Error(ErrorCode.NotFound, ErrorMessage.VACANCY_NOT_FOUND));

            if (vacancy.EmployerId != EmployerId)
                return result.FailedWithError(new Error(ErrorCode.NotAuthorized, ErrorMessage.VACANCY_NOT_OWNED));

            var applications = await _vacancyApplicationRepository.QueryAsync(query =>
            query.Where(v => v.VacancyId == vacancyId));

            if (applications == null || !applications.Any())
                return result.FailedWithError(new Error(ErrorCode.NotFound, ErrorMessage.VACANCY_APPLICATIONS_NOT_FOUND));

            var applicationsDTO = new List<ApplicationDTO>();

            foreach (var application in applications.ToList())
            {
                var applicationDTO = MapApplicationEntityToDTO(application, vacancy);
                applicationsDTO.Add(applicationDTO);
            }
            return result.SucceededWithPayload(applicationsDTO);
        }

        private static string GetBase64ResumeData(ApplicationModel applicationModel)
        {
            var resumeBase64 = string.Empty;

            using (var ms = new MemoryStream())
            {
                applicationModel.Resume.CopyTo(ms);
                var fileBytes = ms.ToArray();
                resumeBase64 = Convert.ToBase64String(fileBytes);
            }

            return resumeBase64;
        }

        private ApplicationDTO MapApplicationEntityToDTO(VacancyApplication entity, Vacancy vacancy)
        {
            return new ApplicationDTO
            {
                ResumeAsBase64 = entity.Resume,
                ResumeContentType = "application/pdf",
                ApplicationId = entity.Id,
                ApplicantId = entity.ApplicantId ?? "",
                VacancyId = entity.VacancyId ?? "",
                Name = entity.Name,
                Email = entity.Email,
                CoverLetter = entity.CoverLetter,
                AdditionalInformation = entity.AdditionalInformation,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                VacancyName = vacancy.Title
            };
        }


    }
}
