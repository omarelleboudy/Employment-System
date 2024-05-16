using Business.Base;
using Business.DTOs;
using Business.Models;
using Data.Entities;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Business.Services
{
    public class VacancyService : IVacancyService
    {
        private readonly IVacancyRepository _vacancyRepository;

        public VacancyService(IVacancyRepository vacancyRepository)
        {
            _vacancyRepository = vacancyRepository;
        }
        public async Task<IResult<List<VacancyDTO>>> ArchiveExpiredVacanciesAsync()
        {
            var result = new Result<List<VacancyDTO>>();
            // Get all Expired Vacancies.
            var expiredVacancies = await _vacancyRepository.QueryAsync(query =>
            query.Where(v => v.ExpiredAt <= DateTime.UtcNow && v.IsActive && !v.IsArchived )
            );

            // If no expired vacancies found, return error not found.
            if (expiredVacancies == null || !expiredVacancies.Any()) 
                return result.FailedWithError(new Error(ErrorCode.NotFound, ErrorMessage.NO_EXPIRED_VACANCY_FOUND));

            // Archive all expired vacancies.
            foreach (var vacancy in expiredVacancies)
            {
                vacancy.IsArchived = true;
                vacancy.UpdatedAt = DateTime.UtcNow;
            }
            // Update the database.
            var success = await _vacancyRepository.UpdateRangeAsync(expiredVacancies.ToList());

            // If database update failed, return error.
            if (!success) return result.FailedWithError(new Error(ErrorCode.Error, ErrorMessage.DATABASE_UPDATE_FAILED));

            // Convert expired vacancies to DTOs.
            List<VacancyDTO> expiredVacancyDTOs = new List<VacancyDTO>();

            foreach (var vacancy in expiredVacancies.ToList())
            {
                VacancyDTO vacancyDTO = MapVacancyEntityToDTO(vacancy);
                expiredVacancyDTOs.Add(vacancyDTO);
            }
            return result.SucceededWithPayload(expiredVacancyDTOs);
        }

        public async Task<IResult<VacancyDTO>> CreateVacancyAsync(VacancyModel vacancyModel, string EmployerId)
        {
            var result = new Result<VacancyDTO>();
            // Validate Vacancy Model.
            if (vacancyModel.Errors().Any())
                return result.FailedWithError(new Error(ErrorCode.InvalidData, ErrorMessage.VACANCY_DATA_INVALID));
            // Create Vacancy Entity.
            var vacancy = new Vacancy
            {
                Title = vacancyModel.Title,
                Description = vacancyModel.Description,
                MaxApplicants = vacancyModel.MaxApplicants,
                EmployerId = EmployerId,
                ExpiredAt = DateTime.UtcNow.AddDays(vacancyModel.DaysAvailable)
            };
            // Add Vacancy to database.
            var entity = await _vacancyRepository.AddAsync(vacancy);
            // If database update failed, return error.
            if (entity == null)
                return result.FailedWithError(new Error(ErrorCode.Error, ErrorMessage.DATABASE_UPDATE_FAILED));
            // Convert Vacancy Entity to DTO.
            VacancyDTO vacancyDTO = MapVacancyEntityToDTO(vacancy);

            return result.SucceededWithPayload(vacancyDTO);
        }

        public async Task<IResult> DeactivateVacancy(string vacancyId, string EmployerId)
        {
            var result = new Result<VacancyDTO>();
            // retrieve Vacancy
            var vacancy = await _vacancyRepository.GetAsync(v=>v.Id == vacancyId && v.IsActive);
            if (vacancy == null)
                return result.FailedWithError(new Error(ErrorCode.NotFound, ErrorMessage.VACANCY_NOT_FOUND));

            if (vacancy.EmployerId != EmployerId)
                return result.FailedWithError(new Error(ErrorCode.NotAuthorized, ErrorMessage.VACANCY_NOT_OWNED));

            // Update DB Vacancy
            vacancy.IsActive = false;
            vacancy.UpdatedAt = DateTime.UtcNow;

            var success = await _vacancyRepository.UpdateAsync(vacancy);
            // If failed, return update db failed error
            if (!success)
                return result.FailedWithError(new Error(ErrorCode.Error, ErrorMessage.DATABASE_UPDATE_FAILED));
            // Map Entity to DTO
            var vacancyDTO = MapVacancyEntityToDTO(vacancy);

            return result.SucceededWithPayload(vacancyDTO);
        }

        public async Task<IResult<List<VacancyDTO>>> GetVacanciesByEmployerAsync(string employerId, bool includeExpired)
        {
            var result = new Result<List<VacancyDTO>>();
            // Get Employer Vacancies.
            var employerVacancies = includeExpired ?
                await _vacancyRepository.QueryAsync(query =>
            query.Where(v => v.EmployerId == employerId && v.IsActive))
                : await _vacancyRepository.QueryAsync(query =>
            query.Where(v => v.EmployerId == employerId && v.ExpiredAt > DateTime.UtcNow && v.IsActive));

            // If no vacancies found, return error not found.
            if (employerVacancies == null || !employerVacancies.Any()) return result.FailedWithError(new Error(ErrorCode.NotFound, ErrorMessage.VACANCY_NOT_FOUND));

            // Convert  vacancies to DTOs.
            List<VacancyDTO> employerVacanciesDTO = new List<VacancyDTO>();

            foreach (var vacancy in employerVacancies.ToList())
            {
                VacancyDTO vacancyDTO = MapVacancyEntityToDTO(vacancy);
                employerVacanciesDTO.Add(vacancyDTO);
            }
            return result.SucceededWithPayload(employerVacanciesDTO);
        }


        public async Task<IResult<VacancyDTO>> GetVacancyAsync(string vacancyId)
        {
            var result = new Result<VacancyDTO>();

            var vacancyQuery = await _vacancyRepository.QueryAsync(query => query.Where(v => v.Id == vacancyId && v.IsActive).Include(v => v.Employer));
            if (vacancyQuery == null || !vacancyQuery.Any())
                return result.FailedWithError(new Error(ErrorCode.NotFound, ErrorMessage.VACANCY_NOT_FOUND));

            var vacancy = vacancyQuery.FirstOrDefault();

            var vacancyDTO = MapVacancyEntityToDTO(vacancy);

            return result.SucceededWithPayload(vacancyDTO);
        }

        public async Task<IResult<List<VacancyDTO>>> SearchVacanciesAsync(string vacancyTitle)
        {
            var result = new Result<List<VacancyDTO>>();

            // Get all Vacancies where name contains vacancyName.
            var employerVacancies = await _vacancyRepository.QueryAsync(query =>
            query.Where(v => v.Title.Contains(vacancyTitle) && v.ExpiredAt > DateTime.UtcNow && v.IsActive).Include(v=>v.Employer));
            // If no vacancies found, return error not found.
            if (employerVacancies == null || !employerVacancies.Any()) return result.FailedWithError(new Error(ErrorCode.NotFound, ErrorMessage.VACANCY_NOT_FOUND));

            // Convert  vacancies to DTOs.
            List<VacancyDTO> searchedVacancies = new List<VacancyDTO>();

            foreach (var vacancy in employerVacancies.ToList())
            {
                VacancyDTO vacancyDTO = MapVacancyEntityToDTO(vacancy);
                searchedVacancies.Add(vacancyDTO);
            }
            return result.SucceededWithPayload(searchedVacancies);
        }

        public async Task<IResult<VacancyDTO>> UpdateVacancyAsync(VacancyModel vacancyModel, string EmployerId)
        {
            var result = new Result<VacancyDTO>();
            // retrieve Vacancy
            var vacancy = await _vacancyRepository.GetAsync(v=>v.Id == vacancyModel.Id && v.IsActive);
            // If vacancy not found, return error not found.
            if (vacancy == null)
                return result.FailedWithError(new Error(ErrorCode.NotFound, ErrorMessage.VACANCY_NOT_FOUND));

            if (vacancy.EmployerId != EmployerId)
                return result.FailedWithError(new Error(ErrorCode.NotAuthorized, ErrorMessage.VACANCY_NOT_OWNED));
            // Update DB Vacancy
            vacancy.Title = vacancyModel.Title;
            vacancy.Description = vacancyModel.Description;
            vacancy.MaxApplicants = vacancyModel.MaxApplicants;
            vacancy.ExpiredAt = DateTime.UtcNow.AddDays(vacancyModel.DaysAvailable);
            vacancy.UpdatedAt = DateTime.UtcNow;

            var success = await _vacancyRepository.UpdateAsync(vacancy);
            // If failed, return update db failed error
            if (!success)
                return result.FailedWithError(new Error(ErrorCode.Error, ErrorMessage.DATABASE_UPDATE_FAILED));
            // Map Entity to DTO
            var vacancyDTO = MapVacancyEntityToDTO(vacancy);

            return result.SucceededWithPayload(vacancyDTO);
        }

        private static VacancyDTO MapVacancyEntityToDTO(Vacancy? vacancy)
        {
            if (vacancy == null) return new VacancyDTO();

            return new VacancyDTO
            {
                EmployerId = vacancy.EmployerId ?? "",
                VacancyId = vacancy.Id,
                Title = vacancy.Title,
                IsActive = vacancy.IsActive,
                Description = vacancy.Description,
                CreatedAt = vacancy.CreatedAt,
                UpdatedAt = vacancy.UpdatedAt,
                ExpiredAt = vacancy.ExpiredAt,
                IsArchived = vacancy.IsArchived,
                CurrentApplicants = vacancy.CurrentApplicants,
                MaxApplicants = vacancy.MaxApplicants,
                EmployerName = vacancy.Employer?.Name ?? ""
            };
        }

    }
}
