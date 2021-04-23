//Copyright (c) 2019 Jason Bayldon
//Modifications - Copyright (c) 2020 OpenBots Inc.
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using OpenBots.Core.Script;
using OpenBots.Core.UI.Forms;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.UI.Forms.Supplement_Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenBots.UI.Forms
{
    public partial class frmScriptVariables : UIForm
    {
        public List<ScriptVariable> ScriptVariables { get; set; }
        public List<ScriptArgument> ScriptArguments { get; set; }
        public string ScriptName { get; set; }
        public string LastModifiedVariableName { get; set; }
        private TreeNode _userVariableParentNode;
        private string _leadingValue = "Default Value: ";
        private string _emptyValue = "(no default value)";
        private string _leadingType = "Type: ";
        private TypeContext _typeContext;

        #region Initialization and Form Load
        public frmScriptVariables(TypeContext typeContext)
        {
            InitializeComponent();
            _typeContext = typeContext;
            LastModifiedVariableName = string.Empty;
        }
        private void frmScriptVariables_Load(object sender, EventArgs e)
        {
           //initialize
            _userVariableParentNode = InitializeNodes("My Task Variables", ScriptVariables);
            lblMainLogo.Text = ScriptName + " variables";
        }

        private TreeNode InitializeNodes(string parentName, List<ScriptVariable> variables)
        {
            //create a root node (parent)
            TreeNode parentNode = new TreeNode(parentName);

            //add each item to parent
            foreach (var item in variables)
            {
                AddUserVariableNode(parentNode, item.VariableName, (string)item.VariableValue, item.VariableType);
            }

            //add parent to treeview
            tvScriptVariables.Nodes.Add(parentNode);

            //return parent and utilize if needed
            return parentNode;
        }

        #endregion

        #region Add/Cancel Buttons
        private void uiBtnOK_Click(object sender, EventArgs e)
        {
            ResetVariables();

            //return success result
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            //cancel and close
            DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region Add/Edit Variables
        private void uiBtnNew_Click(object sender, EventArgs e)
        {
            //create variable editing form
            frmAddVariable addVariableForm = new frmAddVariable(_typeContext);
            addVariableForm.ScriptVariables = ScriptVariables;
            addVariableForm.ScriptArguments = ScriptArguments;

            ExpandUserVariableNode();

            //validate if user added variable
            if (addVariableForm.ShowDialog() == DialogResult.OK)
            {
                //add newly edited node
                AddUserVariableNode(_userVariableParentNode, addVariableForm.txtVariableName.Text, addVariableForm.txtDefaultValue.Text,
                    (Type)addVariableForm.cbxDefaultType.Tag);
                LastModifiedVariableName = addVariableForm.txtVariableName.Text;
                ResetVariables();
            }

            addVariableForm.Dispose();
        }

        private void tvScriptVariables_DoubleClick(object sender, EventArgs e)
        {
            //handle double clicks outside
            if (tvScriptVariables.SelectedNode == null)
            {
                return;
            }

            //if parent was selected return
            if (tvScriptVariables.SelectedNode.Parent == null)
            {
                //user selected top parent
                return;
            }

            //top node check
            var topNode = GetSelectedTopNode();

            if (topNode.Text != "My Task Variables")
            {
                return;
            }

            string variableName, variableValue;
            Type variableType;
            TreeNode parentNode;

            if(tvScriptVariables.SelectedNode.Nodes.Count == 0)
            {
                parentNode = tvScriptVariables.SelectedNode.Parent;
                variableName = tvScriptVariables.SelectedNode.Parent.Text;
                variableValue = tvScriptVariables.SelectedNode.Parent.Nodes[0].Text.Replace(_leadingValue, "").Replace(_emptyValue, "");
                variableType = (Type)tvScriptVariables.SelectedNode.Parent.Nodes[1].Tag;
            }
            else
            {
                parentNode = tvScriptVariables.SelectedNode;
                variableName = tvScriptVariables.SelectedNode.Text;
                variableValue = tvScriptVariables.SelectedNode.Nodes[0].Text.Replace(_leadingValue, "").Replace(_emptyValue, "");
                variableType = (Type)tvScriptVariables.SelectedNode.Nodes[1].Tag;
            }

            if (variableName == "ProjectPath")
                return;

            //create variable editing form
            frmAddVariable addVariableForm = new frmAddVariable(variableName, variableValue, variableType, _typeContext);
            addVariableForm.ScriptVariables = ScriptVariables;
            addVariableForm.ScriptArguments = ScriptArguments;

            ExpandUserVariableNode();

            //validate if user added variable
            if (addVariableForm.ShowDialog() == DialogResult.OK)
            {
                //remove parent
                parentNode.Remove();

                //add newly edited node
                AddUserVariableNode(_userVariableParentNode, addVariableForm.txtVariableName.Text, addVariableForm.txtDefaultValue.Text,
                    (Type)addVariableForm.cbxDefaultType.Tag);
                LastModifiedVariableName = addVariableForm.txtVariableName.Text;
                ResetVariables();
            }

            addVariableForm.Dispose();
        }

        private void AddUserVariableNode(TreeNode parentNode, string variableName, string variableText, Type variableType)
        {
            //add new node and sort
            var childNode = new TreeNode(variableName);

            if (variableText == string.Empty)
            {
                variableText = _emptyValue;
            }

            childNode.Nodes.Add(_leadingValue + variableText);

            TreeNode typeNode = new TreeNode
            {
                Name = "Type",
                Text = _leadingType + variableType.GetRealTypeFullName(),
                Tag = variableType
            };

            childNode.Nodes.Add(typeNode);

            parentNode.Nodes.Add(childNode);
            tvScriptVariables.Sort();
            ExpandUserVariableNode();            
        }

        private void ExpandUserVariableNode()
        {
            if (_userVariableParentNode != null)
            {
                _userVariableParentNode.ExpandAll();
            }
        }

        private void tvScriptVariables_KeyDown(object sender, KeyEventArgs e)
        {
            //handling outside
            if (tvScriptVariables.SelectedNode == null)
            {
                return;
            }

            //if parent was selected return
            if (tvScriptVariables.SelectedNode.Parent == null)
            {
                //user selected top parent
                return;
            }

            //top node check
            var topNode = GetSelectedTopNode();

            if (topNode.Text != "My Task Variables")
            {
                return;
            }

            //if user selected delete
            if (e.KeyCode == Keys.Delete)
            {
                //determine which node is the parent
                TreeNode parentNode;
                if (tvScriptVariables.SelectedNode.Nodes.Count == 0)
                {
                    parentNode = tvScriptVariables.SelectedNode.Parent;
                }
                else
                {
                    parentNode = tvScriptVariables.SelectedNode;
                }

                if (parentNode.Text == "ProjectPath")
                    return;

                //remove parent node
                parentNode.Remove();
                ResetVariables();
            }
        }

        private TreeNode GetSelectedTopNode()
        {
            TreeNode node = tvScriptVariables.SelectedNode;
            while (node.Parent != null)
            {
                node = node.Parent;
            }
            return node;
        }
        #endregion       

        private void ResetVariables()
        {
            //remove all variables
            ScriptVariables.Clear();

            //loop each variable and add
            for (int i = 0; i < _userVariableParentNode.Nodes.Count; i++)
            {
                //get name and value
                var variableName = _userVariableParentNode.Nodes[i].Text;
                var variableValue = _userVariableParentNode.Nodes[i].Nodes[0].Text.Replace(_leadingValue, "").Replace(_emptyValue, "");
                var variableType = (Type)_userVariableParentNode.Nodes[i].Nodes[1].Tag;

                //add to list
                ScriptVariables.Add(new ScriptVariable() 
                { 
                    VariableName = variableName, 
                    VariableValue = variableValue,
                    VariableType = variableType
                });
            }
        }
    }
}