using OpenBots.Core.UI.Forms;
using System;
using System.Windows.Forms;

namespace OpenBots.Commands.Terminal.Forms
{
    public partial class frmTerminalInputBox : UIForm
    {
        public frmTerminalInputBox(string prompt, string title, bool isPassword = false)
        {
            InitializeComponent();
            Text = title;
            lblInput.Text = prompt;

            if (isPassword)
                txtInput.PasswordChar = '*';
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtInput.Text.Trim() == string.Empty)
                return;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
