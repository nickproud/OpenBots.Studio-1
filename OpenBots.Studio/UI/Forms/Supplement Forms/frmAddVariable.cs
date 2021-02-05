using OpenBots.Core.Script;
using OpenBots.Core.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmAddVariable : UIForm
    {
        public List<ScriptVariable> ScriptVariables { get; set; }
        public List<ScriptArgument> ScriptArguments { get; set; }
        private bool _isEditMode;
        private string _editingVariableName;
        private Dictionary<string, List<Type>> _groupedTypes;

        public frmAddVariable(Dictionary<string, List<Type>> groupedTypes)
        {
            InitializeComponent();
            _groupedTypes = groupedTypes;
        }

        public frmAddVariable(string variableName, string variableValue, Type variableType, Dictionary<string, List<Type>> groupedTypes)
        {
            InitializeComponent();
            _groupedTypes = groupedTypes;
            Text = "edit variable";
            lblHeader.Text = "edit variable";
            txtVariableName.Text = variableName;
            txtDefaultValue.Text = variableValue;
            btnDefaultType.Text = variableType.ToString();
            btnDefaultType.Tag = variableType;

            _isEditMode = true;
            _editingVariableName = variableName;
        }

        private void frmAddVariable_Load(object sender, EventArgs e)
        {

        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            txtVariableName.Text = txtVariableName.Text.Trim();
            if (txtVariableName.Text == string.Empty)
            {
                lblVariableNameError.Text = "Variable Name not provided";
                return;
            }

            string newVariableName = txtVariableName.Text;
            var existingVariable = ScriptVariables.Where(var => var.VariableName == newVariableName).FirstOrDefault();
            var existingArgument = ScriptArguments.Where(arg => arg.ArgumentName == newVariableName).FirstOrDefault();
            if (existingVariable != null || existingArgument != null)
            {
                if (!_isEditMode || existingVariable.VariableName != _editingVariableName)
                {
                    lblVariableNameError.Text = "A Variable or Argument with this name already exists";
                    return;
                }
            }

            if (txtVariableName.Text.StartsWith("{") || txtVariableName.Text.EndsWith("}"))
            {
                lblVariableNameError.Text = "Variable markers '{' and '}' should not be included";
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnDefaultType_Click(object sender, EventArgs e)
        {
            frmTypes typeForm = new frmTypes(_groupedTypes);
            typeForm.ShowDialog();

            if (typeForm.DialogResult == DialogResult.OK)
            {
                btnDefaultType.Text = typeForm.SelectedType.ToString();
                btnDefaultType.Tag = typeForm.SelectedType;
            }
        }
    }
}
