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
        private TypeContext _typeContext;
        private Type _preEditType;

        public frmAddVariable(TypeContext typeContext)
        {
            InitializeComponent();
            _typeContext = typeContext;

            cbxDefaultType.DataSource = new BindingSource(_typeContext.DefaultTypes, null);
            cbxDefaultType.DisplayMember = "Key";
            cbxDefaultType.ValueMember = "Value";

            cbxDefaultType.SelectedValue = typeof(string);
            cbxDefaultType.Tag = typeof(string);
        }

        public frmAddVariable(string variableName, string variableValue, Type variableType, TypeContext typeContext)
        {
            InitializeComponent();
            _typeContext = typeContext;
            cbxDefaultType.DataSource = new BindingSource(_typeContext.DefaultTypes, null);
            cbxDefaultType.DisplayMember = "Key";
            cbxDefaultType.ValueMember = "Value";

            Text = "edit variable";
            lblHeader.Text = "edit variable";
            txtVariableName.Text = variableName;
            txtDefaultValue.Text = variableValue;
            cbxDefaultType.SelectedValue = variableType;
            cbxDefaultType.Tag = variableType;

            _preEditType = variableType;

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

        private void cbxDefaultType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (((Type)cbxDefaultType.SelectedValue).Name == "MoreOptions")
            {
                frmTypes typeForm = new frmTypes(_typeContext.GroupedTypes);
                typeForm.ShowDialog();

                if (typeForm.DialogResult == DialogResult.OK)
                {
                    if (!_typeContext.DefaultTypes.ContainsKey(typeForm.SelectedType.FullName))
                    {
                        _typeContext.DefaultTypes.Add(typeForm.SelectedType.FullName, typeForm.SelectedType);
                        cbxDefaultType.DataSource = new BindingSource(_typeContext.DefaultTypes, null);
                    }

                    cbxDefaultType.SelectedValue = typeForm.SelectedType;
                    cbxDefaultType.Tag = typeForm.SelectedType;
                }
                else
                {
                    cbxDefaultType.SelectedValue = _preEditType;
                    cbxDefaultType.Tag = _preEditType;
                }
            }
            else
                cbxDefaultType.Tag = cbxDefaultType.SelectedValue;

            _preEditType = (Type)cbxDefaultType.SelectedValue;

            if (_preEditType == typeof(string) || _preEditType.IsPrimitive)
                txtDefaultValue.ReadOnly = false;
            else
            {
                txtDefaultValue.ReadOnly = true;
                txtDefaultValue.Text = "";
            }
        }
    }
}
