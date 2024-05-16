using Data.Entities;
using Data.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class VacancyApplicationRepository : WriteRepository<VacancyApplication, string>, IVacancyApplicationRepository
    {
        public VacancyApplicationRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
