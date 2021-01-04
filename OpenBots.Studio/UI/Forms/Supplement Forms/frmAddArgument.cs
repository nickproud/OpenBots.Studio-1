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
        public List<ScriptVariable> ScriptVariables { get; set; }
        public List<ScriptArgument> ScriptArguments { get; set; }
        private bool _isEditMode;
        private string _editingArgumentName;
        public frmAddArgument()
        {
            InitializeComponent();
            cbxDefaultDirection.SelectedIndex = 0;
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
            var existingVariable = ScriptVariables.Where(var => var.VariableName == newArgumentName).FirstOrDefault();
            var existingArgument = ScriptArguments.Where(var => var.ArgumentName == newArgumentName).FirstOrDefault();
            if (existingArgument != null || existingVariable != null)
            {
                if (!_isEditMode || existingArgument.ArgumentName != _editingArgumentName)
                {
                    lblArgumentNameError.Text = "An Argument or Variable with this name already exists";
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

        private void cbxDefaultDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDefaultDirection.Text == "Out" || cbxDefaultDirection.Text == "InOut")
            {
                txtDefaultValue.Text = "";
                txtDefaultValue.Enabled = false;
            }
            else
                txtDefaultValue.Enabled = true;               
        }

        private void cbxDefaultDirection_Click(object sender, EventArgs e)
        {
            ComboBox clickedDropdownBox = (ComboBox)sender;
            clickedDropdownBox.DroppedDown = true;
        }
    }
}
