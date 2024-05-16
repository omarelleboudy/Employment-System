using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Logging.Configuration
{
    public interface ICorrelationIdGenerator
    {
        public string GetCorrelationId();
        public void SetCorrelationId(string correlationId);
    }
}
