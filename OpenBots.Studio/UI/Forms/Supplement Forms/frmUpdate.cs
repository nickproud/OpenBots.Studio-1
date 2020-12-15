using OpenBots.Core.UI.Forms;
using OpenBots.Utilities;
using System;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmUpdate : UIForm
    {
        public frmUpdate(ManifestUpdate manifest)
        {
            InitializeComponent();
            lblLocal.Text = "Your Version: " + manifest.LocalVersionProper.ToString();
            lblRemote.Text = "Latest Version: " + manifest.RemoteVersionProper.ToString();
        }

        private void Update_Load(object sender, EventArgs e)
        {
        
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
