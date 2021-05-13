using OpenBots.Core.ChromeNative.Extension;
using OpenBots.Core.UI.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmExtentionsManager : UIForm
    {
        private bool _isChromeNativeMessagingInstalled = false;
        public string ErrorMessage { get; set; }
        public frmExtentionsManager()
        {
            InitializeComponent();
        }

        private void frmExtentionsManager_Load(object sender, EventArgs e)
        {
            //Determine if CNM is installed or not here, and set the flag accordingly here
            ChromeExtensionRegistryManager registryManager = new ChromeExtensionRegistryManager();
            _isChromeNativeMessagingInstalled = registryManager.IsExtensionInstalled();
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
            //Install here
            string serverMenifestPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"NativeServer\com.openbots.chromeserver.message-manifest.json");
            ChromeExtensionRegistryManager registryManager = new ChromeExtensionRegistryManager();
            registryManager.PathValue = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Extension\kkepankimcahnjamnimeijpplgjpmdpp_main.crx");
            registryManager.VersionValue = "1.0";
            registryManager.NativeServerKey = serverMenifestPath;

            //Update Server Manifest
            string json = File.ReadAllText(serverMenifestPath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            jsonObj["path"] = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"NativeServer\OpenBots.NativeServer.exe"); 
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(serverMenifestPath, output);


            DialogResult = DialogResult.OK;
        }
    }
}
