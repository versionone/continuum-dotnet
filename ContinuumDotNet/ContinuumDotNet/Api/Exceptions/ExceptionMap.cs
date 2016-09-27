using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Exceptions;
using ContinuumDotNet.Exceptions.Flow;

namespace ContinuumDotNet.Api.Exceptions
{
    public class ExceptionMap
    {
        public static Dictionary<string, Exception> Exceptions = new Dictionary<string, Exception>()
        {
            {@"Pipeline Instance not found using identifier \[.*\]", new InvalidPipelineIdException()},
            {"EmptyParameter", new MissingParameterException() }
        };
    }
}
