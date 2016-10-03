using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using ContinuumDotNet.Api;
using ContinuumDotNet.Connections;
using ContinuumDotNet.Deployments;
using ContinuumDotNet.Flow.Artifacts;
using ContinuumDotNet.Flow.Pipelines;
using ContinuumDotNetCLI.CommandLineOptions;

namespace ContinuumDotNetCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            CommonOptions commonOptions = new CommonOptions();
            Parser.Default.ParseArguments<CommonOptions>(args).MapResult((CommonOptions opts) =>
            {
                commonOptions = opts;
                return 0;
            }, errs => 1);
            Parser.Default.ParseArguments<DeployOptions>(args).MapResult((DeployOptions opts) => Deploy(opts, commonOptions), errs => 1);
        }

        static int Deploy(DeployOptions deployOptions, CommonOptions commonOptions)
        {
            var deploymentManager = new DeploymentManager("redis");
            deploymentManager.CreateDeployedSite()
                .WithProduct(commonOptions.Product)
                .WithFlightCode(commonOptions.FlightCode)
                .WithVersion(commonOptions.Version)
                .WithServerName(commonOptions.ServerName)
                .Deploy();
            return 0;
        }
    }
}
