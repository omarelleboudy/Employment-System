using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Base
{
    internal static class ErrorMessage
    {
        // general
        public static string PERMISSIONS_DENAYED = "You don't have enough permission to do trigger that action.";
        public static string DATABASE_UPDATE_FAILED = "Failed to update the database.";

        // User
        public static string USER_EXIST = "User already exists.";
        public static string EMAIL_EXIST = "Email address already exist.";
        public static string USER_NOT_EXIST = "User doesn't exist.";
        public static string PASSWORD_INCORRECT = "Your password is incorrect.";
        public static string PROVIDED_DATA_IS_NOT_VALID = "Provided User data is not valid. Please check it and try again.";
   
        // Vacancy
        public static string VACANCY_DATA_INVALID = "The provided vacancy data is not valid.";
        public static string VACANCY_NOT_EXIST = "Vacancy doesn't exist.";
        public static string VACANCY_ALREADY_EXIST = "Vacancy already exist.";
        public static string VACANCY_NOT_FOUND = "Vacancy not found.";
        public static string VACANCY_NOT_OWNED = "Cannot access or update vacancy details for a vacancy that is not posted by you.";
        public static string NO_EXPIRED_VACANCY_FOUND = "There are no expired vacancies to archive.";
        public static string VACANCY_FULL = "Vacancy applications are at capacity.";
        public static string VACANCY_EXPIRED = "This position is no longer accepting applications.";

        // Applicant
        public static string ALREADY_APPLIED = "You have already applied to this vacancy.";
        public static string APPLICANT_APPLICATIONS_NOT_FOUND = "You have no applications.";

        // Vacancy Applications

        public static string VACANCY_APPLICATIONS_NOT_FOUND = "No applications for this vacancy were found.";

    }
}
