using Autofac;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Script;
using OpenBots.Core.UI.Controls;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenBots.Core.Interfaces
{
    public interface IfrmCommandEditor
    {
        List<AutomationCommand> CommandList { get; set; }
        IScriptContext ScriptContext { get; set; }
        string ProjectPath { get; set; }
        IContainer AContainer { get; set; }
        ScriptCommand SelectedCommand { get; set; }
        ScriptCommand OriginalCommand { get; set; }
        CreationMode CreationModeInstance { get; set; }
        string DefaultStartupCommand { get; set; }
        ScriptCommand EditingCommand { get; set; }
        List<ScriptCommand> ConfiguredCommands { get; set; }
        string HTMLElementRecorderURL { get; set; }
        TypeContext TypeContext { get; set; }
        FlowLayoutPanel flw_InputVariables { get; set; }

        void ShowMessage(string message, string title, DialogType dialogType, int closeAfter);
    }
}
