using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.storage
{
    public interface IStorage<T>
    {
        void save(T t);
        T load();
    }
}
