using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace Node.js
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {            
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("Node.js entry point called", "Information");            

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        private void StartNode()
        {
            var port = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["httpIn"].IPEndpoint.Port;
            var ip = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["httpIn"].IPEndpoint.Address.ToString();
            ProcessStartInfo startInfo = new ProcessStartInfo("node.exe", "app.js");
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.EnvironmentVariables.Add("AZURE_PORT", port.ToString());
            startInfo.EnvironmentVariables.Add("AZURE_IP", ip);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            Trace.WriteLine(">>>> Assigned port:" + port);
            try
            {
                Process exeProcess = new Process();
                exeProcess.StartInfo = startInfo;
                exeProcess.ErrorDataReceived += (sender, evt) => Trace.WriteLine(evt.Data);
                exeProcess.OutputDataReceived += (sender, evt) => Trace.WriteLine(evt.Data);
                exeProcess.Start();
                exeProcess.BeginErrorReadLine();
                exeProcess.BeginOutputReadLine();
            }
            catch (Exception ex)
            {
                // Log error.
                Trace.TraceError(ex.Message);
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            StartNode();
            return base.OnStart();
        }
    }
}
