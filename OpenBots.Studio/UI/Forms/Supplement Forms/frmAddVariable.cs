﻿using Microsoft.CodeAnalysis;
using OpenBots.Core.Script;
using OpenBots.Core.UI.Forms;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Utilities;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmAddVariable : UIForm
    {
        public ScriptContext ScriptContext { get; set; }
        private bool _isEditMode;
        private string _editingVariableName;
        private TypeContext _typeContext;
        private Type _preEditType;
        private ToolTip _typeToolTip;
        public List<ScriptVariable> VariablesCopy { get; set; }
        private CodeDomProvider _provider;

        public frmAddVariable(TypeContext typeContext)
        {
            InitializeComponent();
            _typeContext = typeContext;

            cbxDefaultType.DataSource = new BindingSource(_typeContext.DefaultTypes, null);
            cbxDefaultType.DisplayMember = "Key";
            cbxDefaultType.ValueMember = "Value";

            cbxDefaultType.SelectedValue = typeof(string);
            cbxDefaultType.Tag = typeof(string);

            _preEditType = typeof(string);
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
            _typeToolTip = AddTypeToolTip();
            _typeToolTip.SetToolTip(cbxDefaultType, _preEditType.GetRealTypeName());
            _provider = CodeDomProvider.CreateProvider("C#");
            ScriptContext.AddIntellisenseControls(Controls);
        }

        private void frmAddVariable_FormClosing(object sender, FormClosingEventArgs e)
        {
            ScriptContext.RemoveIntellisenseControls(Controls);
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            txtVariableName.ForeColor = Color.SteelBlue;
            txtDefaultValue.ForeColor = Color.SteelBlue;
            lblVariableNameError.Text = "";
            lblVariableValueError.Text = "";

            txtVariableName.Text = txtVariableName.Text.Trim();
            if (txtVariableName.Text == string.Empty)
            {
                lblVariableNameError.Text = "Variable Name not provided";
                txtVariableName.ForeColor = Color.Red;
                return;
            }

            if (!_provider.IsValidIdentifier(txtVariableName.Text))
            {
                lblVariableNameError.Text = "Variable Name is invalid";
                txtVariableName.ForeColor = Color.Red;
                return;
            }

            if (!_provider.IsValidIdentifier(txtVariableName.Text))
            {
                lblVariableNameError.Text = "Variable Name is invalid";
                return;
            }

            string newVariableName = txtVariableName.Text;
            var existingVariable = VariablesCopy.Where(var => var.VariableName == newVariableName).FirstOrDefault();
            var existingArgument = ScriptContext.Arguments.Where(arg => arg.ArgumentName == newVariableName).FirstOrDefault();
            if (existingVariable != null || existingArgument != null)
            {
                if (!_isEditMode || existingVariable.VariableName != _editingVariableName)
                {
                    lblVariableNameError.Text = "A Variable or Argument with this name already exists";
                    txtVariableName.ForeColor = Color.Red;
                    return;
                }
            }

            if (txtVariableName.Text.StartsWith("{") || txtVariableName.Text.EndsWith("}"))
            {
                lblVariableNameError.Text = "Variable markers '{' and '}' should not be included";
                txtVariableName.ForeColor = Color.Red;
                return;
            }

            var result = ScriptContext.EvaluateVariable(newVariableName, (Type)cbxDefaultType.Tag, txtDefaultValue.Text);
            if (!result.Success)
            {
                lblVariableValueError.Text = result.Diagnostics.ToList().Where(x => x.DefaultSeverity == DiagnosticSeverity.Error).FirstOrDefault()?.ToString();
                txtDefaultValue.ForeColor = Color.Red;
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
                frmTypes typeForm = new frmTypes(_typeContext);
                typeForm.ShowDialog();

                if (typeForm.DialogResult == DialogResult.OK)
                {
                    if (!_typeContext.DefaultTypes.ContainsKey(typeForm.SelectedType.GetRealTypeName()))
                    {
                        _typeContext.DefaultTypes.Add(typeForm.SelectedType.GetRealTypeName(), typeForm.SelectedType);
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

                typeForm.Dispose();
            }
            else
                cbxDefaultType.Tag = cbxDefaultType.SelectedValue;

            _preEditType = (Type)cbxDefaultType.SelectedValue;
            _typeToolTip.SetToolTip(cbxDefaultType, _preEditType.GetRealTypeName());
        }

        public ToolTip AddTypeToolTip()
        {
            ToolTip typeToolTip = new ToolTip();
            typeToolTip.IsBalloon = false;
            typeToolTip.ShowAlways = true;
            typeToolTip.AutoPopDelay = 5000;
            return typeToolTip;
        }

        private void txtDefaultValue_TextChanged(object sender, EventArgs e)
        {
            ScriptContext.CodeTBXInput_TextChanged(sender, e);
        }

        private void txtDefaultValue_KeyDown(object sender, KeyEventArgs e)
        {
            ScriptContext.CodeInput_KeyDown(sender, e);
        }       
    }
}
