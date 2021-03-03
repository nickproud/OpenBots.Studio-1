using OpenBots.Core.UI.Forms;
using System;
using System.Windows.Forms;

namespace OpenBots.Commands.Terminal.Forms
{
    public partial class frmTerminalSettings : UIForm
    {
        public string Host { get { return txtHost.Text; } }
        public string Port { get { return txtPort.Text; } }
        public string TerminalType { get { return txtTerminalType.Text; } }
        public bool UseSsl { get { return cbxUseSSL.Checked; } }

        public frmTerminalSettings()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
