using OpenBots.Core.UI.Forms;
using OpenBots.Utilities;
using System;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmTextEditor : UIForm
    {
        public ScriptContext ScriptContext { get; set; }

        public frmTextEditor(ScriptContext scriptContext, string text)
        {
            InitializeComponent();
            ScriptContext = scriptContext;
            txtEditor.Text = text;
        }

        private void frmTextEditor_Load(object sender, EventArgs e)
        {
            ScriptContext.AddIntellisenseControls(Controls);
            txtEditor.SelectionStart = txtEditor.Text.Length;
        }

        private void frmTextEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            ScriptContext.RemoveIntellisenseControls(Controls);
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void txtEditor_TextChanged(object sender, EventArgs e)
        {
            ScriptContext.CodeTBXInput_TextChanged(sender, e);
        }

        private void txtEditor_KeyDown(object sender, KeyEventArgs e)
        {
            ScriptContext.CodeInput_KeyDown(sender, e);
        }
    }
}
