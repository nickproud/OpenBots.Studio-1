using mshtml;
using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Utilities;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.User32;
using OpenBots.Core.Utilities.CommonUtilities;

using SHDocVw;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace OpenBots.Commands.IEBrowser
{
    [Serializable]
    [Category("IE Browser Commands")]
    [Description("This command provides a number of functionalities or actions to perform Automation on Web Elements through the IE Browser.")]
    public class IEElementActionCommand : ScriptCommand
    {
        [Required]
        [DisplayName("IE Browser Instance Name")]
        [Description("Enter the unique instance that was specified in the **IE Create Browser** command.")]
        [SampleUsage("MyIEBrowserInstance")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **IE Create Browser** command will cause an error.")]
        [CompatibleTypes(new Type[] { typeof(InternetExplorer) })]
        public string v_InstanceName { get; set; }

        [Required]
        [DisplayName("Element Search Parameters")]
        [Description("Select the element search parameters appropriately to target an element efficiently.")]
        [SampleUsage("")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public DataTable v_WebSearchParameter { get; set; }

        [Required]
        [DisplayName("IE Element Action")]
        [PropertyUISelectionOption("Invoke Click")]
        [PropertyUISelectionOption("Left Click")]
        [PropertyUISelectionOption("Middle Click")]
        [PropertyUISelectionOption("Right Click")]
        [PropertyUISelectionOption("Get Text")]
        [PropertyUISelectionOption("Set Text")]
        [PropertyUISelectionOption("Get Attribute")]
        [PropertyUISelectionOption("Set Attribute")]
        [PropertyUISelectionOption("Fire onmousedown event")]
        [PropertyUISelectionOption("Fire onmouseover event")]
        [Description("Select the appropriate action to perform on an element once it has been located.")]
        [SampleUsage("")]
        [Remarks("Selecting this field changes the parameters that will be required in the next step.")]
        public string v_WebAction { get; set; }

        [Required]
        [DisplayName("Action Parameters")]
        [Description("Enter the action parameter values based on the selection of an Element Action.")]
        [SampleUsage("vParameterValue")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public DataTable v_WebActionParameterTable { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        private ComboBox _elementActionDropdown;

        [JsonIgnore]
        [Browsable(false)]
        private List<Control> _elementParameterControls;

        [JsonIgnore]
        [Browsable(false)]
        private static IHTMLElementCollection _lastElementCollectionFound;

        [JsonIgnore]
        [Browsable(false)]
        private static HTMLDocument _lastDocFound;

        public IEElementActionCommand()
        {
            CommandName = "IEElementActionCommand";
            SelectionName = "IE Element Action";
            CommandEnabled = true;
            CommandIcon = Resources.command_web;

            v_InstanceName = "DefaultIEBrowser";

            v_WebSearchParameter = new DataTable();
            v_WebSearchParameter.TableName = DateTime.Now.ToString("WebSearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            v_WebSearchParameter.Columns.Add("Enabled", typeof(Boolean));
            v_WebSearchParameter.Columns.Add("Property Name");
            v_WebSearchParameter.Columns.Add("Property Value");

            v_WebActionParameterTable = new DataTable();
            v_WebActionParameterTable.TableName = DateTime.Now.ToString("WebActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            v_WebActionParameterTable.Columns.Add("Parameter Name");
            v_WebActionParameterTable.Columns.Add("Parameter Value");
        }

        [STAThread]
        public async override Task RunCommand(object sender)
        {
            object browserObject = null;

            var engine = (IAutomationEngineInstance)sender;

            browserObject = v_InstanceName.GetAppInstance(engine);
            var browserInstance = (InternetExplorer)browserObject;

            DataTable searchTable = CommonMethods.Clone(v_WebSearchParameter);

            DataColumn matchFoundColumn = new DataColumn();
            matchFoundColumn.ColumnName = "Match Found";
            matchFoundColumn.DefaultValue = false;
            searchTable.Columns.Add(matchFoundColumn);

            var elementSearchProperties = from rws in searchTable.AsEnumerable()
                                          where rws.Field<bool>("Enabled").ToString() == "True"
                                          select rws;

            bool qualifyingElementFound = false;

            HTMLDocument doc = browserInstance.Document;

            if (doc == _lastDocFound)
            {
                qualifyingElementFound = await InspectFrame(_lastElementCollectionFound, elementSearchProperties, sender, browserInstance);
            }
            if (!qualifyingElementFound)
            {
                qualifyingElementFound = await InspectFrame(doc.all, elementSearchProperties, sender, browserInstance);
            }
            if (qualifyingElementFound)
            {
                _lastDocFound = doc;
            }

            if (!qualifyingElementFound)
            {
                throw new Exception("Could not find the element!");
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDataGridViewGroupFor("v_WebSearchParameter", this, editor));


            _elementActionDropdown = commandControls.CreateDropdownFor("v_WebAction", this);
            RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_WebAction", this));
            RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_WebAction", this, new Control[] { _elementActionDropdown }, editor));
            _elementActionDropdown.SelectionChangeCommitted += ElementActionDropdown_SelectionChangeCommitted;
            RenderedControls.Add(_elementActionDropdown);

            _elementParameterControls = commandControls.CreateDefaultDataGridViewGroupFor("v_WebActionParameterTable", this, editor);
            ((DataGridView)_elementParameterControls[2]).AllowUserToAddRows = false;
            RenderedControls.AddRange(_elementParameterControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            string parameters = string.Empty;
            //foreach (DataRow oRow in v_WebActionParameterTable.Rows)
            foreach (DataRow oRow in v_WebSearchParameter.Rows)
            {
                parameters += ", " + oRow["Property Name"] + "=" + oRow["Property Value"];
            }
            if (parameters.Length > 0) parameters = parameters.Substring(1);
            return base.GetDisplayValue() + $" [Perform Action '{v_WebAction} {parameters}' - Instance Name '{v_InstanceName}']";
        }

        private async Task<bool> FindQualifyingElement(object sender, EnumerableRowCollection<DataRow> elementSearchProperties, IHTMLElement element)
        {
            var engine = (IAutomationEngineInstance)sender;

            foreach (DataRow seachCriteria in elementSearchProperties)
            {
                string searchPropertyName = seachCriteria.Field<string>("Property Name");
                dynamic searchPropertyValue = await seachCriteria.Field<string>("Property Value").EvaluateCode(engine);
                string searchPropertyFound = seachCriteria.Field<string>("Match Found");

                string innerHTML = element.innerHTML;
                string outerHTML = element.outerHTML;

                searchPropertyFound = "False";

                try
                {
                    //if (element.GetType().GetProperty(searchPropertyName) == null)
                    if ((outerHTML == null) ||
                        (element.getAttribute(searchPropertyName) == null) ||
                        (Convert.IsDBNull(element.getAttribute(searchPropertyName))))
                    {
                        return false;
                    }

                    if (searchPropertyValue is string && searchPropertyName.ToLower() == "href")
                    {
                        try
                        {
                            HTMLAnchorElement anchor = (HTMLAnchorElement)element;
                            if (anchor.href.Contains(searchPropertyValue))
                            {
                                seachCriteria.SetField("Match Found", "True");
                            }
                            else
                            {
                                seachCriteria.SetField("Match Found", "False");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        if (searchPropertyValue is int)
                        {
                            //int elementValue = (int)element.GetType().GetProperty(searchPropertyName).GetValue(element, null);
                            int elementValue = (int)element.getAttribute(searchPropertyName);
                            if (elementValue == searchPropertyValue)
                            {
                                seachCriteria.SetField("Match Found", "True");
                            }
                            else
                            {
                                seachCriteria.SetField("Match Found", "False");
                            }
                        }
                        else
                        {
                            //string elementValue = (string)element.GetType().GetProperty(searchPropertyName).GetValue(element, null);
                            string elementValue = (string)element.getAttribute(searchPropertyName);
                            if ((elementValue != null) && (elementValue == searchPropertyValue))
                            {
                                seachCriteria.SetField("Match Found", "True");
                            }
                            else
                            {
                                seachCriteria.SetField("Match Found", "False");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /*foreach (var seachCriteria in elementSearchProperties)
            {
                Console.WriteLine(seachCriteria.Field<string>("Property Value"));
            }*/

            return elementSearchProperties.Where(seachCriteria => seachCriteria.Field<string>("Match Found") == "True").Count() == elementSearchProperties.Count();
        }

        private void ElementActionDropdown_SelectionChangeCommitted(object sender, EventArgs e)
        {

            IEElementActionCommand cmd = this;
            DataTable actionParameters = cmd.v_WebActionParameterTable;

            if (sender != null)
            {
                actionParameters.Rows.Clear();
            }


            switch (_elementActionDropdown.SelectedItem)
            {
                case "Invoke Click":
                case "Fire onmousedown event":
                case "Fire onmouseover event":
                case "Clear Element":

                    foreach (var ctrl in _elementParameterControls)
                    {
                        ctrl.Hide();
                    }

                    break;

                case "Set Text":
                    foreach (var ctrl in _elementParameterControls)
                    {
                        ctrl.Show();
                    }
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Text To Set");
                    }

                    break;

                case "Get Text":
                case "Get Matching Elements":
                    foreach (var ctrl in _elementParameterControls)
                    {
                        ctrl.Show();
                    }
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Variable Name");
                    }
                    break;

                case "Get Attribute":
                    foreach (var ctrl in _elementParameterControls)
                    {
                        ctrl.Show();
                    }
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Attribute Name");
                        actionParameters.Rows.Add("Variable Name");
                    }
                    break;

                case "Set Attribute":
                    foreach (var ctrl in _elementParameterControls)
                    {
                        ctrl.Show();
                    }
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Attribute Name");
                        actionParameters.Rows.Add("Value To Set");
                    }

                    break;

                default:
                    break;
            }

            ((DataGridView)_elementParameterControls[2]).Columns[0].ReadOnly = true;
        }

        private async Task RunCommandActions(IHTMLElement element, object sender, InternetExplorer browserInstance)
        {
            var engine = (IAutomationEngineInstance)sender;
            switch (v_WebAction)
            {
                case "Fire onmousedown event":
                    ((IHTMLElement3)element).FireEvent("onmousedown");
                    break;
                case "Fire onmouseover event":
                    ((IHTMLElement3)element).FireEvent("onmouseover");
                    break;
                case "Invoke Click":
                    element.click();
                    IECreateBrowserCommand.WaitForReadyState(browserInstance);
                    break;

                case "Left Click":
                case "Middle Click":
                case "Right Click":
                    int elementXposition = FindElementXPosition(element);
                    int elementYposition = FindElementYPosition(element);

                    //inputs need to be validated

                    string userXAdjustString = (from rw in v_WebActionParameterTable.AsEnumerable()
                                                       where rw.Field<string>("Parameter Name") == "X Adjustment"
                                                       select rw.Field<string>("Parameter Value")).FirstOrDefault();
                    int userXAdjust = (int)await userXAdjustString.EvaluateCode(engine);

                    string userYAdjustString = (from rw in v_WebActionParameterTable.AsEnumerable()
                                                       where rw.Field<string>("Parameter Name") == "Y Adjustment"
                                                       select rw.Field<string>("Parameter Value")).FirstOrDefault();
                    int userYAdjust = (int)await userYAdjustString.EvaluateCode(engine);

                    var ieClientLocation = User32Functions.GetWindowPosition(new IntPtr(browserInstance.HWND));

                    var mouseX = (elementXposition + ieClientLocation.left + 10) + userXAdjust; // + 10 gives extra padding
                    var mouseY = (elementYposition + ieClientLocation.top + 90 + SystemInformation.CaptionHeight) + userYAdjust; // +90 accounts for title bar height
                    var mouseClick = v_WebAction;

                    User32Functions.SendMouseMove(mouseX, mouseY, v_WebAction);
                    break;
                case "Set Attribute":
                    string setAttributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                               where rw.Field<string>("Parameter Name") == "Attribute Name"
                                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    string valueToSet = (from rw in v_WebActionParameterTable.AsEnumerable()
                                         where rw.Field<string>("Parameter Name") == "Value To Set"
                                         select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    valueToSet = (string)await valueToSet.EvaluateCode(engine);

                    element.setAttribute(setAttributeName, valueToSet);
                    break;
                case "Set Text":
                    string setTextAttributeName = "value";

                    string textToSet = (from rw in v_WebActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Text To Set"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    textToSet = (string)await textToSet.EvaluateCode(engine);

                    element.setAttribute(setTextAttributeName, textToSet);
                    break;
                case "Get Attribute":
                    string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Attribute Name"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    string variableName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                           where rw.Field<string>("Parameter Name") == "Variable Name"
                                           select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    string convertedAttribute = Convert.ToString(element.getAttribute(attributeName));

                    convertedAttribute.SetVariableValue(engine, variableName);
                    break;
            }
        }

        private int FindElementXPosition(IHTMLElement obj)
        {
            int curleft = 0;
            if (obj.offsetParent != null)
            {
                while (obj.offsetParent != null)
                {
                    curleft += obj.offsetLeft;
                    obj = obj.offsetParent;
                }
            }

            return curleft;
        }

        private int FindElementYPosition(IHTMLElement obj)
        {
            int curtop = 0;
            if (obj.offsetParent != null)
            {
                while (obj.offsetParent != null)
                {
                    curtop += obj.offsetTop;
                    obj = obj.offsetParent;
                }
            }

            return curtop;
        }

        private async Task<bool> InspectFrame(IHTMLElementCollection elementCollection, EnumerableRowCollection<DataRow> elementSearchProperties, object sender, InternetExplorer browserInstance)
        {
            bool qualifyingElementFound = false;
            foreach (IHTMLElement element in elementCollection)
            {
                if (element.outerHTML != null)
                {
                    string outerHtml = element.outerHTML.ToLower().Trim();

                    if (!outerHtml.StartsWith("<html") &&
                        !outerHtml.StartsWith("<body") &&
                        !outerHtml.StartsWith("<head") &&
                        !outerHtml.StartsWith("<!doctype"))
                    {
                        qualifyingElementFound = await FindQualifyingElement(sender, elementSearchProperties, element);
                        if (qualifyingElementFound)
                        {
                            await RunCommandActions(element, sender, browserInstance);
                            _lastElementCollectionFound = elementCollection;
                            return (true);
                            //break;
                        }
                        if (element.outerHTML != null && element.outerHTML.ToLower().Trim().StartsWith("<frame "))
                        {
                            string frameId = element.getAttribute("id");
                            if (frameId == null)
                            {
                                frameId = element.getAttribute("name");
                            }
                            if (frameId != null)
                            {
                                qualifyingElementFound = await InspectFrame(browserInstance.Document.getElementById(frameId).contentDocument.all, elementSearchProperties, sender, browserInstance);
                            }
                        }
                        if (qualifyingElementFound)
                        {
                            break;
                        }
                    }
                }
            }
            return (qualifyingElementFound);
        }

    }

}