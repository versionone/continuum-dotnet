using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace ContinuumDotNetCLI.CommandLineOptions
{
    class CommonOptions
    {
        [Option('s', "Server name")]
        public string ServerName { get; set; }

        [Option('f', "Flight code")]
        public string FlightCode { get; set; }

        [Option('v', "Version")]
        public string Version { get; set; }

        [Option('p', "Product")]
        public string Product { get; set; }
    }

    [Verb("deploy", HelpText = "Deploy a site to a server")]
    class DeployOptions
    {
    }

}
