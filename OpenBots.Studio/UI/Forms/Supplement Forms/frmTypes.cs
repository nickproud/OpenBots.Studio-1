using OpenBots.Core.Script;
using OpenBots.Core.UI.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmTypes : UIForm
    {
        public Type SelectedType { get; set; }

        private TreeView _tvTypesCopy;
        private string _txtTypeWatermark = "Type Here to Search";
        private TypeContext _typeContext;
        private Type _preEditType;

        public frmTypes(TypeContext typeContext)
        {
            InitializeComponent();
            _typeContext = typeContext;
        }

        private void frmTypes_Load(object sender, EventArgs e)
        {
            

            foreach(var group in _typeContext.GroupedTypes)
            {
                var groupNode = new TreeNode();
                groupNode.Name = group.Key;
                groupNode.Text = group.Key;
                tvTypes.Nodes.Add(groupNode);

                foreach(var type in group.Value)
                {
                    var typeNode = new TreeNode();
                    typeNode.Name = type.FullName;
                    typeNode.Text = type.FullName;
                    typeNode.ToolTipText = type.Name;
                    typeNode.Tag = type;
                    groupNode.Nodes.Add(typeNode);
                }
            }

            tvTypes.Sort();

            _tvTypesCopy = new TreeView();
            CopyTreeView(tvTypes, _tvTypesCopy);
        }


        public void CopyTreeView(TreeView originalTreeView, TreeView copiedTreeView)
        {
            TreeNode copiedTreeNode;
            foreach (TreeNode originalTreeNode in originalTreeView.Nodes)
            {
                copiedTreeNode = new TreeNode(originalTreeNode.Text);
                CopyTreeViewNodes(copiedTreeNode, originalTreeNode);
                copiedTreeView.Nodes.Add(copiedTreeNode);
            }
        }

        public void CopyTreeViewNodes(TreeNode copiedTreeNode, TreeNode originalTreeNode)
        {
            TreeNode copiedChildNode;
            foreach (TreeNode originalChildNode in originalTreeNode.Nodes)
            {
                copiedChildNode = new TreeNode(originalChildNode.Text);
                copiedChildNode.Name = originalChildNode.Name;
                copiedChildNode.ToolTipText = originalChildNode.ToolTipText;
                copiedChildNode.Tag = originalChildNode.Tag;
                copiedTreeNode.Nodes.Add(copiedChildNode);
            }
        }

        private void txtTypeSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtTypeSearch.Text == _txtTypeWatermark)
                return;

            bool childNodefound = false;

            //blocks repainting tree until all controls are loaded
            tvTypes.BeginUpdate();
            tvTypes.Nodes.Clear();
            if (txtTypeSearch.Text != string.Empty)
            {
                foreach (TreeNode parentNodeCopy in _tvTypesCopy.Nodes)
                {
                    TreeNode searchedParentNode = new TreeNode(parentNodeCopy.Text);
                    tvTypes.Nodes.Add(searchedParentNode);

                    foreach (TreeNode childNodeCopy in parentNodeCopy.Nodes)
                    {
                        if (childNodeCopy.Text.ToLower().Contains(txtTypeSearch.Text.ToLower()))
                        {
                            var searchedChildNode = new TreeNode(childNodeCopy.Text);
                            searchedChildNode.ToolTipText = childNodeCopy.ToolTipText;
                            searchedChildNode.Tag = childNodeCopy.Tag;
                            searchedParentNode.Nodes.Add(searchedChildNode);
                            childNodefound = true;
                        }
                    }
                    if (!childNodefound && !(parentNodeCopy.Text.ToLower().Contains(txtTypeSearch.Text.ToLower())))
                    {
                        tvTypes.Nodes.Remove(searchedParentNode);
                    }
                    else if (parentNodeCopy.Text.ToLower().Contains(txtTypeSearch.Text.ToLower()))
                    {
                        searchedParentNode.Nodes.Clear();
                        foreach (TreeNode childNodeCopy in parentNodeCopy.Nodes)
                        {
                            var searchedChildNode = new TreeNode(childNodeCopy.Text);
                            searchedChildNode.ToolTipText = childNodeCopy.ToolTipText;
                            searchedChildNode.Tag = childNodeCopy.Tag;
                            searchedParentNode.Nodes.Add(searchedChildNode);
                        }
                    }
                    childNodefound = false;
                }
            }
            else
            {
                foreach (TreeNode parentNodeCopy in _tvTypesCopy.Nodes)
                {
                    tvTypes.Nodes.Add((TreeNode)parentNodeCopy.Clone());
                }
                tvTypes.CollapseAll();
            }

            //enables redrawing tree after all controls have been added
            tvTypes.EndUpdate();
        }

        private void txtTypeSearch_Enter(object sender, EventArgs e)
        {
            if (txtTypeSearch.Text == _txtTypeWatermark)
            {
                txtTypeSearch.Text = "";
                txtTypeSearch.ForeColor = Color.Black;
            }
        }

        private void txtTypeSearch_Leave(object sender, EventArgs e)
        {
            if (txtTypeSearch.Text == "")
            {
                txtTypeSearch.Text = _txtTypeWatermark;
                txtTypeSearch.ForeColor = Color.LightGray;
            }
        }

        private void uiBtnClearTypeSearch_Click(object sender, EventArgs e)
        {
            txtTypeSearch.Clear();
        }

        private void uiBtnExpand_Click(object sender, EventArgs e)
        {
            tvTypes.ExpandAll();
        }

        private void uiBtnCollapse_Click(object sender, EventArgs e)
        {
            tvTypes.CollapseAll();
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            //handle double clicks outside
            if (tvTypes.SelectedNode == null)
                return;

            //exit if parent node is clicked
            if (tvTypes.SelectedNode.Parent == null)
                return;

            if (tvTypes.SelectedNode.Nodes.Count == 0)
            {
                SelectedType = (Type)tvTypes.SelectedNode.Tag;

                if (SelectedType.IsGenericType)
                {
                    SelectedType = ConstructGenericType();
                }
                DialogResult = DialogResult.OK;
            }
        }

        private Type ConstructGenericType()
        {
            List<Type> genericArguments = new List<Type>();
            foreach(Control control in flpTypeConstruction.Controls)
            {
                if (control is ComboBox)
                {
                    genericArguments.Add((Type)control.Tag);
                }
            }

            return SelectedType.MakeGenericType(genericArguments.ToArray());
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void tvTypes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //handle double clicks outside
            if (tvTypes.SelectedNode == null)
                return;

            //exit if parent node is clicked
            if (tvTypes.SelectedNode.Parent == null)
                return;

            if (tvTypes.SelectedNode.Nodes.Count == 0)
            {
                SelectedType = (Type)tvTypes.SelectedNode.Tag;
                if (SelectedType.IsGenericType)
                {
                    LoadGenericPanel();
                }
            }
        }

        private void LoadGenericPanel()
        {
            flpTypeConstruction.Controls.Clear();
            foreach (Control control in flpTypeConstruction.Controls)
                control.Dispose();

            flpTypeConstruction.Controls.Add(NewTypeLabel(SelectedType.Name));

            foreach(var arg in SelectedType.GetGenericArguments())
                flpTypeConstruction.Controls.Add(NewTypeComboBox());
            //Type[] typeArgs = { typeof(string), typeof(int) };
            //var newType = SelectedType.MakeGenericType(typeArgs);
   
        }

        private Label NewTypeLabel(string text)
        {
            Label lblType = new Label();
            lblType.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblType.ForeColor = Color.Black;
            lblType.Text = text;
            lblType.AutoSize = true;
            return lblType;
        }

        private ComboBox NewTypeComboBox()
        {
            ComboBox cbxType = new ComboBox();
            cbxType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxType.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbxType.ForeColor = Color.SteelBlue;
            cbxType.Width = 200;
            cbxType.Tag = typeof(string);
            cbxType.SelectionChangeCommitted += new EventHandler(cbxDefaultType_SelectionChangeCommitted);

            cbxType.DataSource = new BindingSource(_typeContext.DefaultTypes, null);
            cbxType.DisplayMember = "Key";
            cbxType.ValueMember = "Value";

            cbxType.SelectedValue = typeof(string);
            cbxType.Tag = typeof(string);
            return cbxType;
        }

        private void cbxDefaultType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var cbxType = (ComboBox)sender;
            if (((Type)cbxType.SelectedValue).Name == "MoreOptions")
            {
                frmTypes typeForm = new frmTypes(_typeContext);
                typeForm.ShowDialog();

                if (typeForm.DialogResult == DialogResult.OK)
                {
                    if (!_typeContext.DefaultTypes.ContainsKey(typeForm.SelectedType.FullName))
                    {
                        _typeContext.DefaultTypes.Add(typeForm.SelectedType.FullName, typeForm.SelectedType);
                        cbxType.DataSource = new BindingSource(_typeContext.DefaultTypes, null);
                    }

                    cbxType.SelectedValue = typeForm.SelectedType;
                    cbxType.Tag = typeForm.SelectedType;
                }
                else
                {
                    cbxType.SelectedValue = _preEditType;
                    cbxType.Tag = _preEditType;
                }
            }
            else
                cbxType.Tag = cbxType.SelectedValue;

            _preEditType = (Type)cbxType.SelectedValue;
        }
    }
}
