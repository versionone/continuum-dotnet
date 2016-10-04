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
using ContinuumDotNet.Deployments.Installers;
using ContinuumDotNet.Flow.Artifacts;
using ContinuumDotNet.Flow.Pipelines;
using ContinuumDotNetCLI.CommandLineOptions;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;
using ContinuumDotNet.Utilities;
using Naos.WinRM;

namespace ContinuumDotNetCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var remotePsRunner = new RemotePsRunner("dev", "continuum-windows", "", "lchost-12");

            var psResult = remotePsRunner.RunScript("Write-Output $env:computername");

            var lifecycleInstaller = new LifecycleInstaller();
            lifecycleInstaller.WithBaseArtifactoryUrl("http://artifactory/artifactory")
                .WithBuildSupportFilesRepositoryName("lifecycle-build-support")
                .WithConfigRepositoryFolderName("config-files")
                .WithDemoDataRepositoryName("lifecycle-build-support")
                .WithDemoDataRepositoryFolderName("demo-data")
                .WithDemoDataFilename("EnterpriseDemo-160.bak")
                .WithInstallerRepositoryName("lifecycle-installers")
                .WithInstallersRepositoryFolderName("core/gulp")
                .WithInstallerFilename("VersionOne.Setup-Ultimate-16.2.8.18.exe")
                .WithLicenseFilename("VersionOne.Development.lic")
                .WithLicenseRespositoryFolderName("license-files")
                .WithUserConfigFileName("user.Enterprise.config")
                .Install();

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
