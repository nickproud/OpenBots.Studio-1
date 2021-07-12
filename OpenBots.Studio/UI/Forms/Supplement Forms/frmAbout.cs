//Copyright (c) 2019 Jason Bayldon
//Modifications - Copyright (c) 2020 OpenBots Inc.
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenBots.Core.Server_Documents.User;
using OpenBots.Server.SDK.HelperMethods;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            DateTime buildDate = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location);

            lblAppVersion.Text = $"Version: {Application.ProductVersion}";
            lblBuildDate.Text = $"Build Date: {buildDate:MM-dd-yyyy}";
            lblMachineName.Text = $"Machine Name: {SystemInfo.MachineName}";
            lblMacAddress.Text = $"Mac Address: {SystemInfo.GetMacAddress()}";

            var environmentSettings = new EnvironmentSettings();
            if (environmentSettings.GetEnvironmentVariable() == null)
            {
                lblServer.Text = "Server: Agent environment variable not found";
                return;
            }

            string agentSettingsPath = Path.Combine(environmentSettings.GetEnvironmentVariable(), environmentSettings.SettingsFileName);

            if (!File.Exists(agentSettingsPath))
            {
                lblIPAddress.Text = "IP Address: Agent settings file not found";
                lblServer.Text = "Server: Agent settings file not found";
            }
            else
            {
                try
                {
                    environmentSettings.Load();
                }
                catch
                {
                    /* Suppress Exception ("Agent is not connected") to avoid crashing and still load the Agent settings
                     to be used in the following statements */
                }

                if (string.IsNullOrEmpty(environmentSettings.AgentId))
                {
                    lblIPAddress.Text = "IP Address: Agent is not connected";
                    lblServer.Text = "Server: Agent is not connected";
                }
                else
                {
                    lblIPAddress.Text = $"IP Address: {GetPublicIP(environmentSettings)}";
                    lblServer.Text = $"Server: {environmentSettings.ServerUrl}";
                }
            }           
        }

        private string GetPublicIP(EnvironmentSettings environmentSettings)
        {
            try
            {
                AuthMethods authMethods = new AuthMethods();
                authMethods.Initialize(environmentSettings.ServerType, environmentSettings.OrganizationName, environmentSettings.ServerUrl, environmentSettings.Username, environmentSettings.Password);

                return authMethods.Ping();
            }
            catch(Exception)
            {
                return "Server is not responding";
            }
        }
    }
}
