using Data.Entities;
using Data.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class VacancyRepository : WriteRepository<Vacancy, string>, IVacancyRepository
    {
        public VacancyRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
