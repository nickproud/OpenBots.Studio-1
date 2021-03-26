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
using OpenBots.Core.Enums;
using OpenBots.Core.Script;
using OpenBots.Core.UI.Forms;
using OpenBots.UI.Forms.Supplement_Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenBots.UI.Forms
{
    public partial class frmScriptArguments : UIForm
    {
        public List<ScriptVariable> ScriptVariables { get; set; }
        public List<ScriptArgument> ScriptArguments { get; set; }
        public string ScriptName { get; set; }
        public string LastModifiedArgumentName { get; set; }
        private TreeNode _userArgumentParentNode;
        private string _leadingValue = "Default Value: ";
        private string _emptyValue = "(no default value)";

        private string _leadingDirection = "Direction: ";
        private string _leadingType = "Type: ";
        private TypeContext _typeContext;

        #region Initialization and Form Load
        public frmScriptArguments(TypeContext typeContext)
        {
            InitializeComponent();
            _typeContext = typeContext;
            LastModifiedArgumentName = string.Empty;
        }
        private void frmScriptArguments_Load(object sender, EventArgs e)
        {
           //initialize
            _userArgumentParentNode = InitializeNodes("My Task Arguments", ScriptArguments);
            lblMainLogo.Text = ScriptName + " arguments";
            ExpandUserArgumentNode();
        }

        private TreeNode InitializeNodes(string parentName, List<ScriptArgument> arguments)
        {
            //create a root node (parent)
            TreeNode parentNode = new TreeNode(parentName);

            //add each item to parent
            foreach (var item in arguments)
            {
                AddUserArgumentNode(parentNode, item.ArgumentName, item.Direction, (string)item.ArgumentValue, item.ArgumentType);
            }

            //add parent to treeview
            tvScriptArguments.Nodes.Add(parentNode);

            //return parent and utilize if needed
            return parentNode;
        }

        #endregion

        #region Add/Cancel Buttons
        private void uiBtnOK_Click(object sender, EventArgs e)
        {
            ResetArguments();

            //return success result
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            //cancel and close
            DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region Add/Edit Arguments
        private void uiBtnNew_Click(object sender, EventArgs e)
        {
            //create argument editing form
            frmAddArgument addArgumentForm = new frmAddArgument(_typeContext);
            addArgumentForm.ScriptArguments = ScriptArguments;
            addArgumentForm.ScriptVariables = ScriptVariables;

            ExpandUserArgumentNode();

            //validate if user added argument
            if (addArgumentForm.ShowDialog() == DialogResult.OK)
            {
                //add newly edited node
                AddUserArgumentNode(_userArgumentParentNode, addArgumentForm.txtArgumentName.Text, 
                    (ScriptArgumentDirection)Enum.Parse(typeof(ScriptArgumentDirection), addArgumentForm.cbxDefaultDirection.Text),
                    addArgumentForm.txtDefaultValue.Text, (Type)addArgumentForm.cbxDefaultType.Tag);
                LastModifiedArgumentName = addArgumentForm.txtArgumentName.Text;
                ResetArguments();
            }

            addArgumentForm.Dispose();
        }

        private void tvScriptArguments_DoubleClick(object sender, EventArgs e)
        {
            //handle double clicks outside
            if (tvScriptArguments.SelectedNode == null)
            {
                return;
            }

            //if parent was selected return
            if (tvScriptArguments.SelectedNode.Parent == null)
            {
                //user selected top parent
                return;
            }

            //top node check
            var topNode = GetSelectedTopNode();

            if (topNode.Text != "My Task Arguments")
            {
                return;
            }

            string argumentName, argumentValue;
            ScriptArgumentDirection argumentDirection;
            Type argumentType;
            TreeNode parentNode;

            if(tvScriptArguments.SelectedNode.Nodes.Count == 0)
            {
                parentNode = tvScriptArguments.SelectedNode.Parent;
                argumentName = tvScriptArguments.SelectedNode.Parent.Text;
                argumentValue = tvScriptArguments.SelectedNode.Parent.Nodes[0].Text.Replace(_leadingValue, "").Replace(_emptyValue, "");
                argumentDirection = (ScriptArgumentDirection)Enum.Parse(typeof(ScriptArgumentDirection), tvScriptArguments.SelectedNode.Parent.Nodes[1].Text.Replace(_leadingDirection, ""));
                argumentType = (Type)tvScriptArguments.SelectedNode.Parent.Nodes[2].Tag;
            }
            else
            {
                parentNode = tvScriptArguments.SelectedNode;
                argumentName = tvScriptArguments.SelectedNode.Text;
                argumentValue = tvScriptArguments.SelectedNode.Nodes[0].Text.Replace(_leadingValue, "").Replace(_emptyValue, "");
                argumentDirection = (ScriptArgumentDirection)Enum.Parse(typeof(ScriptArgumentDirection), tvScriptArguments.SelectedNode.Nodes[1].Text.Replace(_leadingDirection, ""));
                argumentType = (Type)tvScriptArguments.SelectedNode.Nodes[2].Tag;
            }

            if (argumentName == "ProjectPath")
                return;

            //create argument editing form
            frmAddArgument addArgumentForm = new frmAddArgument(argumentName, argumentDirection, argumentValue, argumentType, _typeContext);
            addArgumentForm.ScriptArguments = ScriptArguments;
            addArgumentForm.ScriptVariables = ScriptVariables;

            ExpandUserArgumentNode();

            //validate if user added argument
            if (addArgumentForm.ShowDialog() == DialogResult.OK)
            {
                //remove parent
                parentNode.Remove();

                //add newly edited node
                AddUserArgumentNode(_userArgumentParentNode, addArgumentForm.txtArgumentName.Text,
                    (ScriptArgumentDirection)Enum.Parse(typeof(ScriptArgumentDirection), addArgumentForm.cbxDefaultDirection.Text),
                    addArgumentForm.txtDefaultValue.Text, (Type)addArgumentForm.cbxDefaultType.Tag);
                LastModifiedArgumentName = addArgumentForm.txtArgumentName.Text;
                ResetArguments();
            }

            addArgumentForm.Dispose();
        }

        private void AddUserArgumentNode(TreeNode parentNode, string argumentName, ScriptArgumentDirection argumentDirection, string argumentText, Type argumentType)
        {
            //add new node and sort
            var childNode = new TreeNode(argumentName);

            if (argumentText == string.Empty)
            {
                argumentText = _emptyValue;
            }

            childNode.Nodes.Add(_leadingValue + argumentText);
            childNode.Nodes.Add(_leadingDirection + argumentDirection.ToString());

            TreeNode typeNode = new TreeNode
            {
                Name = "Type",
                Text = _leadingType + argumentType.ToString(),
                Tag = argumentType
            };

            childNode.Nodes.Add(typeNode);
            
            parentNode.Nodes.Add(childNode);
            tvScriptArguments.Sort();
            ExpandUserArgumentNode();
        }

        private void ExpandUserArgumentNode()
        {
            if (_userArgumentParentNode != null)
            {
                _userArgumentParentNode.ExpandAll();
            }
        }

        private void tvScriptArguments_KeyDown(object sender, KeyEventArgs e)
        {
            //handling outside
            if (tvScriptArguments.SelectedNode == null)
            {
                return;
            }

            //if parent was selected return
            if (tvScriptArguments.SelectedNode.Parent == null)
            {
                //user selected top parent
                return;
            }

            //top node check
            var topNode = GetSelectedTopNode();

            if (topNode.Text != "My Task Arguments")
            {
                return;
            }

            //if user selected delete
            if (e.KeyCode == Keys.Delete)
            {
                //determine which node is the parent
                TreeNode parentNode;
                if (tvScriptArguments.SelectedNode.Nodes.Count == 0)
                {
                    parentNode = tvScriptArguments.SelectedNode.Parent;
                }
                else
                {
                    parentNode = tvScriptArguments.SelectedNode;
                }

                if (parentNode.Text == "ProjectPath")
                    return;

                //remove parent node
                parentNode.Remove();
                ResetArguments();
            }
        }

        private TreeNode GetSelectedTopNode()
        {
            TreeNode node = tvScriptArguments.SelectedNode;
            while (node.Parent != null)
            {
                node = node.Parent;
            }
            return node;
        }
        #endregion

        private void ResetArguments()
        {
            //remove all variables
            ScriptArguments.Clear();

            //loop each variable and add
            for (int i = 0; i < _userArgumentParentNode.Nodes.Count; i++)
            {
                //get name and value
                var argumentName = _userArgumentParentNode.Nodes[i].Text;
                var argumentDirection = (ScriptArgumentDirection)Enum.Parse(typeof(ScriptArgumentDirection), _userArgumentParentNode.Nodes[i].Nodes[1].Text.Replace(_leadingDirection, ""));
                var argumentValue = _userArgumentParentNode.Nodes[i].Nodes[0].Text.Replace(_leadingValue, "").Replace(_emptyValue, "");
                var argumentType = (Type)_userArgumentParentNode.Nodes[i].Nodes[2].Tag;

                //add to list
                ScriptArguments.Add(new ScriptArgument()
                {
                    ArgumentName = argumentName,
                    Direction = argumentDirection,
                    ArgumentValue = argumentValue,
                    ArgumentType = argumentType
                });
            }
        }
    }
}