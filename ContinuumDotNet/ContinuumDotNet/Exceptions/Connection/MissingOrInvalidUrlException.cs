using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuumDotNet.Exceptions.Connection
{
    public class MissingOrInvalidUrlException : Exception
    {
        public string Url { get; set;  }

        public MissingOrInvalidUrlException(string url)
        {
            Url = url;
        }

        public override string Message
        {
            get { return $"Invalid URL={Url}"; }
        }
    }
}
