using System;
using System.Collections.Generic;
using ContinuumDotNet.Exceptions.Connection;
using ContinuumDotNet.Interfaces.Deployments;
using ContinuumDotNet.Interfaces.Utilities;
using ContinuumDotNet.Utilities;

namespace ContinuumDotNet.Deployments.Installers
{
    public class LifecycleInstaller : BaseInstaller
    {
        public LifecycleInstaller(IRemotePsRunner remotePsRunner) : base(remotePsRunner)
        {
        }

        public override void RunInstaller()
        {
            base.RunInstaller();
        }

        public override void DeployDemoData()
        {
            base.DeployDemoData();
        }
    }
}
