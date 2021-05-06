//using OpenBots.Core.ChromeNativeMessaging.Extension;
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
            //ChromeExtensionRegistryManager registryManager = new ChromeExtensionRegistryManager();
            //_isChromeNativeMessagingInstalled = registryManager.IsExtensionInstalled();
            if (_isChromeNativeMessagingInstalled)
                btnInstallChromeNativeMessaging.Text = "Uninstall";
            else if (!_isChromeNativeMessagingInstalled)
                btnInstallChromeNativeMessaging.Text = "Install";
        }

        private void btnInstallChromeNativeMessaging_Click(object sender, EventArgs e)
        {
            if (btnInstallChromeNativeMessaging.Text == "Install")
            {
                //Install here
                //var converted = BrowserExtensions.CreateForChrome(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Extension\OBExtension"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Extension\OBExtension.crx"));
                //ChromeExtensionRegistryManager registryManager = new ChromeExtensionRegistryManager();
                //registryManager.PathValue = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,@"Resources\Extension\OBExtension.crx");
                //registryManager.VersionValue = "1.0";
            }
            else
            {
                //Uninstall here
                //ChromeExtensionRegistryManager registryManager = new ChromeExtensionRegistryManager();
                //registryManager.DeleteSubKey();
            }

            //if installation/uninstallation succeeds, return a DialogResult.Ok
            //else set the ErrorMessage property, set DialogResult to DialogResult.Cancel and report back to the studio reference
            //Setting the DialogResult property will close the form. 

            DialogResult = DialogResult.OK;
        }
    }
}
