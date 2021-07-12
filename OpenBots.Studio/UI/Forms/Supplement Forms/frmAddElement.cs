using OpenBots.Core.Script;
using OpenBots.Core.UI.Forms;
using OpenBots.UI.CustomControls.CustomUIControls;
using OpenBots.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmAddElement : UIForm
    {
        public ScriptContext ScriptContext { get; set; }
        public DataTable ElementValueDT { get; set; }
        private bool _isEditMode;
        private string _editingVariableName;
        public List<ScriptElement> ElementsCopy { get; set; }

        public frmAddElement()
        {
            InitializeComponent();
            ElementValueDT = new DataTable();
            ElementValueDT.Columns.Add("Enabled");
            ElementValueDT.Columns.Add("Parameter Name");
            ElementValueDT.Columns.Add("Parameter Value");
            ElementValueDT.TableName = DateTime.Now.ToString("ElementValueDT" + DateTime.Now.ToString("MMddyy.hhmmss"));

            ElementValueDT.Rows.Add(true, "\"XPath\"", "");
            ElementValueDT.Rows.Add(true, "\"Relative XPath\"", "");
            ElementValueDT.Rows.Add(false, "\"ID\"", "");
            ElementValueDT.Rows.Add(false, "\"Name\"", "");
            ElementValueDT.Rows.Add(false, "\"Tag Name\"", "");
            ElementValueDT.Rows.Add(false, "\"Class Name\"", "");
            ElementValueDT.Rows.Add(false, "\"Link Text\"", "");
            ElementValueDT.Rows.Add(false, "\"CSS Selector\"", "");
        }

        public frmAddElement(string elementName, DataTable elementValueDT)
        {
            InitializeComponent();

            Text = "edit element";
            lblHeader.Text = "edit element";
            txtElementName.Text = elementName;
            ElementValueDT = elementValueDT;

            _isEditMode = true;
            _editingVariableName = elementName;          
        }

        private void frmAddElement_Load(object sender, EventArgs e)
        {
            ScriptContext.AddIntellisenseControls(Controls);
        }

        private void frmAddElement_FormClosing(object sender, FormClosingEventArgs e)
        {
            ScriptContext.RemoveIntellisenseControls(Controls);
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            txtElementName.Text = txtElementName.Text.Trim();
            if (txtElementName.Text == string.Empty)
            {
                lblElementNameError.Text = "Element Name not provided"; 
                return;
            }

            string newElementName = txtElementName.Text;

            ScriptElement existingElement;
            if (ElementsCopy != null)
                existingElement = ElementsCopy.Where(var => var.ElementName == newElementName).FirstOrDefault();
            else
                existingElement = ScriptContext.Elements.Where(var => var.ElementName == newElementName).FirstOrDefault();

            if (existingElement != null)                
            {
                if (!_isEditMode || existingElement.ElementName != _editingVariableName)
                {
                    lblElementNameError.Text = "An Element with this name already exists";
                    return;
                }               
            }

            if (txtElementName.Text.StartsWith("<") || txtElementName.Text.EndsWith(">"))
            {
                lblElementNameError.Text = "Element markers '<' and '>' should not be included";
                return;
            }

            dgvDefaultValue.EndEdit();
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void dgvDefaultValue_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ScriptContext.ActiveGridView = (UIDataGridView)sender;

            var txtBox = e.Control as TextBox;
            if (txtBox != null)
            {
                //Remove an existing event-handler, if present, to avoid adding multiple handlers when the editing control is reused
                txtBox.KeyDown -= new KeyEventHandler(ScriptContext.CodeInput_KeyDown);
                txtBox.TextChanged -= new EventHandler(ScriptContext.CodeDGVInput_TextChanged);
                txtBox.Leave -= new EventHandler(ScriptContext.CodeInput_Leave);

                //Add the event handler
                txtBox.KeyDown += new KeyEventHandler(ScriptContext.CodeInput_KeyDown);
                txtBox.TextChanged += new EventHandler(ScriptContext.CodeDGVInput_TextChanged);
                txtBox.Leave += new EventHandler(ScriptContext.CodeInput_Leave);
            }
        }       
    }
}
