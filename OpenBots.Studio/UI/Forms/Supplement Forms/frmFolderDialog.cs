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
using OpenBots.Core.Settings;
using OpenBots.Core.UI.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmFolderDialog : UIForm
    {
        public frmFolderDialog(string message, string title, string folderPath)
        {
            InitializeComponent();
       
            lblMessage.Text = message;
            Text = title;
            txtFolderLocation.Text = folderPath;               
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Path.Combine(txtFolderLocation.Text, "OB Scripts")))
            {
                try
                {
                    Directory.CreateDirectory(Path.Combine(txtFolderLocation.Text, "OB Scripts"));
                    var settings = new ApplicationSettings().GetOrCreateApplicationSettings();
                    settings.ClientSettings.ScriptsFolder = Path.Combine(txtFolderLocation.Text, "OB Scripts");
                    settings.Save();
                    DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }   
        }

        private void btnFolderManager_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtFolderLocation.Text = fbd.SelectedPath;
                txtFolderLocation.Focus();
            }
        }

        private void txtFolderLocation_KeyDown(object sender, KeyEventArgs e)
        {
         if (e.KeyCode == Keys.Enter)
            {
                uiBtnOk_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}