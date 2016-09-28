using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuumDotNet.Interfaces.Common
{
    public interface IHasCreationDate
    {
        DateTime CreationDateInUtc { get; }
    }
}
