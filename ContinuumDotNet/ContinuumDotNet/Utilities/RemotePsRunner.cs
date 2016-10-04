using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Interfaces.Utilities;
using Naos.WinRM;

namespace ContinuumDotNet.Utilities
{
    public class RemotePsRunner : IRemotePsRunner
    {
        private readonly PSCredential _remotePsCredential;
        public RemotePsRunner(string domain, string username, string password, string serverName)
        {
            _remotePsCredential = string.IsNullOrEmpty(domain) ? 
                new PSCredential(username, password.ToSecureString()) : 
                new PSCredential($"{domain}\\{username}", password.ToSecureString());
            ServerName = serverName;
        }

        public string ServerName { get; set; }

        public PsResult RunScript(string script)
        {
            var connectionInfo = new WSManConnectionInfo(new Uri($"http://{ServerName}:5985/wsman"), "http://schemas.microsoft.com/powershell/Microsoft.PowerShell", _remotePsCredential);
            var resultStringBuilder = new StringBuilder();

            var result = new PsResult();

            using (var rs = RunspaceFactory.CreateRunspace(connectionInfo))
            {
                rs.Open();
                var ps = PowerShell.Create();
                ps.Runspace = rs;
                ps.AddScript(script);

                foreach (var stdoutLine in ps.Invoke())
                {
                    resultStringBuilder.AppendLine(stdoutLine.ToString());
                }

                result.StdOut = resultStringBuilder.ToString();

                if (ps.HadErrors)
                {
                    resultStringBuilder = new StringBuilder();
                    foreach (var stdErrLine in ps.Streams.Error)
                    {
                        resultStringBuilder.AppendLine(stdErrLine.ToString());
                    }
                    result.StdErr = resultStringBuilder.ToString();
                }
                

            }
            return result;
        }

        public static string CreateDownloadCommand(string clientVariable, string source, string target)
        {
            return $"{clientVariable}.DownloadFile(\"{source}\", \"{target}\")";
        }
    }
}
