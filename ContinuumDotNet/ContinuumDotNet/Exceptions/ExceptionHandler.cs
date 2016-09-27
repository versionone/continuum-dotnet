using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContinuumDotNet.Api.Exceptions;
using ContinuumDotNet.Interfaces.Exceptions;

namespace ContinuumDotNet.Exceptions
{
    public class ExceptionHandler : IExceptionHandler
    {
        public Exception FindError(string infoMessage)
        {
            foreach (var regexMessage in ExceptionMap.Exceptions.Keys)
            {
                var regex = new Regex(regexMessage);
                var match = regex.Match(infoMessage);
                if (match.Success)
                {
                    return ExceptionMap.Exceptions[regexMessage];
                }
            }
            return new Exception(infoMessage);
        }

        public Exception FindError(string errorCode, string errorMessage)
        {
            foreach (var regexMessage in ExceptionMap.Exceptions.Keys)
            {
                var regex = new Regex(regexMessage);
                var match = regex.Match(errorCode);
                if (match.Success)
                {
                    return ExceptionMap.Exceptions[regexMessage];
                }
            }
            return new Exception(errorMessage);
        }
    }
}
