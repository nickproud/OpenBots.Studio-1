using OpenBots.Core.Command;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.UI.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenBots.Core.Infrastructure
{
    public interface ICommandControls
    {
        List<Control> CreateDefaultInputGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor, int height = 30, int width = 300, bool shouldValidate = true, bool isEvaluateSnippet = false);
        List<Control> CreateDefaultPasswordInputGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor);
        List<Control> CreateDefaultOutputGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor);
        List<Control> CreateDefaultDropdownGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor);
        List<Control> CreateDefaultDataGridViewGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor);
        List<Control> CreateDefaultWebElementDataGridViewGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor, Control[] additionalHelpers);
        DataGridView CreateDefaultDataGridViewFor(string parameterName, ScriptCommand parent);
        PictureBox CreateDefaultPictureBoxFor(string parameterName, ScriptCommand parent);
        List<Control> CreateDefaultWindowControlGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor);
        Label CreateDefaultLabelFor(string parameterName, ScriptCommand parent);
        void CreateDefaultToolTipFor(string parameterName, ScriptCommand parent, Control label);
        TextBox CreateDefaultInputFor(string parameterName, ScriptCommand parent, int height = 30, int width = 300, bool isEvaluateSnippet = false);
        CheckBox CreateCheckBoxFor(string parameterName, ScriptCommand parent);
        ComboBox CreateDropdownFor(string parameterName, ScriptCommand parent);
        ComboBox CreateStandardComboboxFor(string parameterName, ScriptCommand parent);
        List<Control> CreateUIHelpersFor(string parameterName, ScriptCommand parent, Control[] targetControls, IfrmCommandEditor editor);
        Tuple<string, string> ShowConditionElementRecorder(object sender, EventArgs e, IfrmCommandEditor editor);
        IfrmScriptEngine CreateScriptEngineForm(EngineContext engineContext, bool blnCloseWhenDone, bool isDebugMode);
        IAutomationEngineInstance CreateAutomationEngineInstance(EngineContext engineContext);
        IfrmWebElementRecorder CreateWebElementRecorderForm(string startURL);
        IfrmAdvancedUIElementRecorder CreateAdvancedUIElementRecorderForm();
        IfrmCommandEditor CreateCommandEditorForm(List<AutomationCommand> commands, List<ScriptCommand> existingCommands);
        ScriptCommand CreateBeginIfCommand(string commandData = null);
        Type GetCommandType(string commandName);
    }
}
