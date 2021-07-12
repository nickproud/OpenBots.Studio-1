using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Model.ApplicationModel;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Database
{
    [Serializable]
	[Category("Database Commands")]
	[Description("This command performs a OleDb database query.")]
	public class ExecuteDatabaseStoredProcedureCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Database Instance Name")]
		[Description("Enter the unique instance that was specified in the **Define Database Connection** command.")]
		[SampleUsage("MyBrowserInstance")]
		[Remarks("Failure to enter the correct instance name or failure to first call the **Define Database Connection** command will cause an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBAppInstance) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Stored Procedure")]
		[Description("Define the OleDb stored procedure to execute.")]
		[SampleUsage("\"storedProcedureName\" || vStoredProcedure")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_StoredProcedure { get; set; }

		[Required]
		[DisplayName("Timeout (Seconds)")]
		[Description("Specify how many seconds to wait before throwing an exception.")]
		[SampleUsage("30 || vSeconds")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_ProcedureTimeout { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Dataset Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(DataTable), typeof(int) })]
		public string v_OutputUserVariableName { get; set; }

		public ExecuteDatabaseStoredProcedureCommand()
		{
			CommandName = "ExecuteDatabaseStoredProcedureCommand";
			SelectionName = "Execute Database Stored Procedure";
			CommandEnabled = true;
			CommandIcon = Resources.command_database;

			v_ProcedureTimeout = "30";
		}

		public async override Task RunCommand(object sender)
		{
			//create engine, instance, query
			var engine = (IAutomationEngineInstance)sender;
			var storedProcedure = (string)await v_StoredProcedure.EvaluateCode(engine);
			var vQueryTimeout = (int)await v_ProcedureTimeout.EvaluateCode(engine);

			//define connection
			var databaseConnection = (OleDbConnection)((OBAppInstance)await v_InstanceName.EvaluateCode(engine)).Value;

			//define command
			var oleCommand = new OleDbCommand(storedProcedure, databaseConnection);
			oleCommand.CommandTimeout = vQueryTimeout;

			oleCommand.CommandType = CommandType.StoredProcedure;
			databaseConnection.Open();
			var result = oleCommand.ExecuteNonQuery();
			databaseConnection.Close();
			result.SetVariableValue(engine, v_OutputUserVariableName);	
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_StoredProcedure", this, editor));

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ProcedureTimeout", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Execute Stored Procedure {v_StoredProcedure} - Store result in '{v_OutputUserVariableName}' - Instance Name '{v_InstanceName}']";
		}
	}
}
