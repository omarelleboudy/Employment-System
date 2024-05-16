using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class VacancyModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DaysAvailable { get; set; }
        public int MaxApplicants { get; set; }

        public VacancyModel()
        {
            Id = string.Empty;
            Title = string.Empty;
            Description = string.Empty;
            DaysAvailable = 0;
            MaxApplicants = 0;
        }

        public List<string> Errors()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(Title))
            {
                errors.Add("Title is required.");
            }

            if (string.IsNullOrEmpty(Description))
            {
                errors.Add("Description is required.");
            }

            if (DaysAvailable <= 0)
            {
                errors.Add("DaysAvailable must be greater than 0.");
            }

            if (MaxApplicants <= 0)
            {
                errors.Add("MaxApplicants must be greater than 0.");
            }

            return errors;
        }

    }
}
