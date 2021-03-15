using OpenBots.Core.Script;
using OpenBots.Core.UI.Forms;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.Security;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public partial class frmHTMLDisplayForm : UIForm
    {
        public string TemplateHTML { get; set; }

        public frmHTMLDisplayForm()
        {
            InitializeComponent();
        }

        private void frmHTMLDisplayForm_Load(object sender, EventArgs e)
        {
            webBrowserHTML.ScriptErrorsSuppressed = true;
            webBrowserHTML.ObjectForScripting = this;
            webBrowserHTML.DocumentText = TemplateHTML;
            TopMost = true;
        }

        public List< ScriptVariable> GetVariablesFromHTML(string tagSearch)
        {
            var varList = new List<ScriptVariable>();

            HtmlElementCollection collection = webBrowserHTML.Document.GetElementsByTagName(tagSearch);
            for (int i = 0; i < collection.Count; i++)
            {
                var variableName = collection[i].GetAttribute("v_OutputUserVariableName");

                if (!string.IsNullOrEmpty(variableName))
                {
                    var parentElement = collection[i];

                    if (tagSearch == "select")
                    {
                        foreach (HtmlElement item in parentElement.Children)
                        {
                            if (item.GetAttribute("selected") == "True")
                            {
                                varList.Add(new ScriptVariable() 
                                { 
                                    VariableName = variableName, 
                                    VariableType = typeof(string), 
                                    VariableValue = item.InnerText 
                                });
                            }
                        }
                    }
                    else
                    {
                        if (parentElement.GetAttribute("type") == "checkbox")
                        {
                            var inputValue = bool.Parse(collection[i].GetAttribute("checked"));
                            varList.Add(new ScriptVariable() 
                            { 
                                VariableName = variableName, 
                                VariableType = typeof(bool), 
                                VariableValue = inputValue 
                            });
                        }
                        else
                        {
                            var inputValue = collection[i].GetAttribute("value");
                            if(collection[i].GetAttribute("type").ToString().ToLower() == "password")
                            {
                                var secureValue = inputValue.ConvertStringToSecureString();
                                varList.Add(new ScriptVariable()
                                {
                                    VariableName = variableName,
                                    VariableType = typeof(SecureString),
                                    VariableValue = secureValue
                                });
                            }   
                            else
                                varList.Add(new ScriptVariable() 
                                { 
                                    VariableName = variableName, 
                                    VariableType = typeof(string), 
                                    VariableValue = inputValue 
                                });
                        }
                    }
                }
            }
            return varList;
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
