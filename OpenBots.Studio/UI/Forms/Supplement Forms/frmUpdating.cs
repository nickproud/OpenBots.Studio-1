using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmUpdating : Form
    {
        private string _topLevelFolder = Application.StartupPath;
        private string _localPackagePath;

        public frmUpdating(string packageURL)
        {
            InitializeComponent();
            bgwUpdate.RunWorkerAsync(packageURL);
        }

        private void frmUpdating_Load(object sender, EventArgs e)
        {
        }

        private void bgwUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            //get package
            bgwUpdate.ReportProgress(0, "Setting Up...");

            //define update folder
            var tempUpdateFolder = _topLevelFolder + "\\temp\\";

            //delete existing
            if (Directory.Exists(tempUpdateFolder))
                Directory.Delete(tempUpdateFolder, true);

            //create folder
            Directory.CreateDirectory(tempUpdateFolder);

            //cast arg to string
            string packageURL = (string)e.Argument;

            bgwUpdate.ReportProgress(0, "Downloading Update...");

            //create uri and download package
            Uri uri = new Uri(packageURL);
            _localPackagePath = Path.Combine(tempUpdateFolder, Path.GetFileName(uri.LocalPath));

            //if package exists for some reason then delete
            if (File.Exists(_localPackagePath))
                File.Delete(_localPackagePath);

            //create web client
            WebClient newWebClient = new WebClient();

            //download file
            newWebClient.DownloadFile(uri, _localPackagePath);
        }

        private void bgwUpdate_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblUpdate.Text = e.UserState.ToString();
        }

        private void bgwUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error is null)
            {
                Process.Start(_localPackagePath);
                Close();
            }
            else
                MessageBox.Show(e.Error.ToString());
        }
    }

    
}
