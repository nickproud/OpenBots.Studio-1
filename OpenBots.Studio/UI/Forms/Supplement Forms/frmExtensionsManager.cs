using OpenBots.Core.ChromeNative.Extension;
using OpenBots.Core.Enums;
using OpenBots.Core.IO;
using OpenBots.Core.UI.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmExtentionsManager : UIForm
    {
        public string ErrorMessage { get; set; }
        private bool _isChromeNativeMessagingInstalled = false;
        private string _localAppDataExtensionsDirectory;
        private string _programFilesExtensionsDirectory;

        public frmExtentionsManager()
        {
            InitializeComponent();
            _localAppDataExtensionsDirectory = Folders.GetFolder(FolderType.LocalAppDataExtensionsFolder);
            _programFilesExtensionsDirectory = Folders.GetFolder(FolderType.ProgramFilesExtensionsFolder);
        }

        private void frmExtentionsManager_Load(object sender, EventArgs e)
        {
            //Determine if CNM is installed or not here, and set the flag accordingly here
            _isChromeNativeMessagingInstalled = new ChromeExtensionRegistryManager().IsExtensionInstalled();
            if (_isChromeNativeMessagingInstalled) 
            {
                btnInstallChromeNativeMessaging.Text = "Installed";
                btnInstallChromeNativeMessaging.Enabled = false;
            }
            else
            {
                btnInstallChromeNativeMessaging.Enabled = true;
                btnInstallChromeNativeMessaging.Text = "Install";
            }
        }

        private void btnInstallChromeNativeMessaging_Click(object sender, EventArgs e)
        {
            try
            {
                //Install here
                string serverMenifestPath = Path.Combine(_localAppDataExtensionsDirectory, "Native Chrome", "com.openbots.chromeserver.message-manifest.json");
                ChromeExtensionRegistryManager registryManager = new ChromeExtensionRegistryManager();
                registryManager.PathValue = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Extensions", "kkepankimcahnjamnimeijpplgjpmdpp_main.crx");
                registryManager.VersionValue = "1.0";
                registryManager.NativeServerKey = serverMenifestPath;

                //Update Server Manifest
                string json = File.ReadAllText(serverMenifestPath);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                jsonObj["path"] = Path.Combine(_programFilesExtensionsDirectory, "Native Chrome", "OpenBots.NativeServer.exe");
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(serverMenifestPath, output);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                DialogResult = DialogResult.Cancel;
            }         
        }
    }
}
