using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuumDotNet.Interfaces.Data
{
    public interface IHasQuery<T, U>
    {
        T GetOne();
        U GetAll();
    }
}
