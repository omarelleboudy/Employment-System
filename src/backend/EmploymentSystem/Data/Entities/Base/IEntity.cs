using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Base
{
    public interface IEntity<T> where T : IComparable<T>, IEquatable<T>
    {
        public T Id { get; set; }
    }
}
