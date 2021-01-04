using OpenBots.Core.Script;
using OpenBots.Core.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmAddArgument : UIForm
    {
        public List<ScriptArgument> ScriptArguments { get; set; }
        private bool _isEditMode;
        private string _editingArgumentName;
        public frmAddArgument()
        {
            InitializeComponent();
        }

        public frmAddArgument(string argumentName, ScriptArgumentDirection argumentDirection, string argumentValue)
        {
            InitializeComponent();
            Text = "edit argument";
            lblHeader.Text = "edit argument";
            txtArgumentName.Text = argumentName;
            cbxDefaultDirection.Text = argumentDirection.ToString();
            txtDefaultValue.Text = argumentValue;

            _isEditMode = true;
            _editingArgumentName = argumentName.Replace("{", "").Replace("}", "");
        }

        private void frmAddArgument_Load(object sender, EventArgs e)
        {

        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            txtArgumentName.Text = txtArgumentName.Text.Trim();
            if (txtArgumentName.Text == string.Empty)
            {
                lblArgumentNameError.Text = "Argument Name not provided";
                return;
            }

            string newArgumentName = txtArgumentName.Text.Replace("{", "").Replace("}", "");
            var existingArgument = ScriptArguments.Where(var => var.ArgumentName == newArgumentName).FirstOrDefault();
            if (existingArgument != null)
            {
                if (!_isEditMode || existingArgument.ArgumentName != _editingArgumentName)
                {
                    lblArgumentNameError.Text = "An Argument with this name already exists";
                    return;
                }
            }

            if (!txtArgumentName.Text.StartsWith("{") || !txtArgumentName.Text.EndsWith("}"))
            {
                lblArgumentNameError.Text = "Argument markers '{' and '}' must be included";
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
