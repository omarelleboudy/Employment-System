using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Logging.Configuration
{
    public class CorrelationIdGenerator : ICorrelationIdGenerator
    {
        private string _correlationId = Guid.NewGuid().ToString();
        public string GetCorrelationId()
        {
            return _correlationId;
        }

        public void SetCorrelationId(string correlationId)
        {
            _correlationId = correlationId;
        }
    }
}
