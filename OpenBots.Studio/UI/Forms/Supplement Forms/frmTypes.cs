using OpenBots.Core.UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmTypes : UIForm
    {
        public Dictionary<string, List<Type>> GroupedTypes { get; set; } = new Dictionary<string, List<Type>>();
        public Type SelectedType { get; set; }

        private TreeView _tvTypesCopy;
        private string _txtTypeWatermark = "Type Here to Search";

        public frmTypes(Dictionary<string, List<Type>> groupedTypes)
        {
            InitializeComponent();
            GroupedTypes = groupedTypes;
        }

        private void frmTypes_Load(object sender, EventArgs e)
        {
            

            foreach(var group in GroupedTypes)
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

        private void tvTypes_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
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
                DialogResult = DialogResult.OK;
            }           
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
    }
}
