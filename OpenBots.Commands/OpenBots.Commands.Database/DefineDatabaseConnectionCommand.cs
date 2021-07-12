using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Model.ApplicationModel;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Properties;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.OleDb;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Database
{
    [Serializable]
	[Category("Database Commands")]
	[Description("This command connects to an OleDb database.")]
	public class DefineDatabaseConnectionCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Database Instance Name")]
		[Description("Enter a unique name that will represent the application instance.")]
		[SampleUsage("MyDatabaseInstance")]
		[Remarks("This unique name allows you to refer to the instance by name in future commands, " +
				 "ensuring that the commands you specify run against the correct application.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBAppInstance) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Connection String")]
		[Description("Define the string to use when connecting to the OleDb database.")]
		[SampleUsage("\"Provider=sqloledb;Data Source=myServerAddress;Initial Catalog=myDataBase;Integrated Security=SSPI;\" || vConnectionString")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_ConnectionString { get; set; }

		[DisplayName("Connection String Password (Optional)")]
		[Description("Optional field to define the password to use when connecting to the OleDb database.")]
		[SampleUsage("vPassword")]
		[Remarks("If storing the password in the textbox below, please ensure the connection string " +
				"above contains a database-specific placeholder with #pwd to be replaced at runtime. (;Password=#pwd)")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(SecureString) })]
		public string v_ConnectionStringPassword { get; set; }

		[Required]
		[DisplayName("Test Connection Before Proceeding")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Select the appropriate option.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_TestConnection { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private TextBox _connectionString;

		public DefineDatabaseConnectionCommand()
		{
			CommandName = "DefineDatabaseConnectionCommand";
			SelectionName = "Define Database Connection";
			CommandEnabled = true;
			CommandIcon = Resources.command_database;

			v_TestConnection = "Yes";
		}

		public async override Task RunCommand(object sender)
		{
			//get engine and preference
			var engine = (IAutomationEngineInstance)sender;

			//create connection
			var oleDBConnection = await CreateConnection(engine);

			//attempt to open and close connection
			if (v_TestConnection == "Yes")
			{
				oleDBConnection.Open();
				oleDBConnection.Close();
			}

			new OBAppInstance(v_InstanceName, oleDBConnection).SetVariableValue(engine, v_InstanceName);
		}

		private async Task<OleDbConnection> CreateConnection(IAutomationEngineInstance engine)
		{
			var connection = (string)await v_ConnectionString.EvaluateCode(engine);
			var connectionSecurePass = (SecureString)await v_ConnectionStringPassword.EvaluateCode(engine);
			var connectionPass = "";
			if(connectionSecurePass != null)
				connectionPass = connectionSecurePass.ConvertSecureStringToString();

			connection = connection.Replace("#pwd", connectionPass);

			return new OleDbConnection(connection);
		}

		private async Task<OleDbConnection> CreateTestConnection(IfrmCommandEditor editor)
		{
			var engineContext = new EngineContext(editor.ScriptContext, editor.ProjectPath);

			foreach (var var in engineContext.Variables)
				await VariableMethods.InstantiateVariable(var.VariableName, (string)var.VariableValue, var.VariableType, engineContext);

			foreach (var arg in engineContext.Arguments)
				await VariableMethods.InstantiateVariable(arg.ArgumentName, (string)arg.ArgumentValue, arg.ArgumentType, engineContext);

			var connection = (string)await VariableMethods.EvaluateCode(v_ConnectionString, engineContext);
			var connectionPass = (SecureString)await VariableMethods.EvaluateCode(v_ConnectionStringPassword, engineContext);

			connection = connection.Replace("#pwd", connectionPass.ConvertSecureStringToString());

			return new OleDbConnection(connection);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

			CommandItemControl buildConnectionStringControl = new CommandItemControl("connectionString", Resources.command_database, "Build Connection String");
			buildConnectionStringControl.Click += (sender, e) => BuildConnectionString(sender, e);

			CommandItemControl testConnectionControl = new CommandItemControl("connectionTest", Resources.command_database, "Test Connection");
			testConnectionControl.Click += async (sender, e) => await TestConnection(sender, e, editor);

			_connectionString = commandControls.CreateDefaultInputFor("v_ConnectionString", this);
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_ConnectionString", this));
			RenderedControls.Add(buildConnectionStringControl);
			RenderedControls.Add(testConnectionControl);
			RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_ConnectionString", this, new[] { _connectionString }, editor));	
			RenderedControls.Add(_connectionString);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ConnectionStringPassword", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_TestConnection", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Instance Name '{v_InstanceName}']";
		}

		private async Task TestConnection(object sender, EventArgs e, IfrmCommandEditor editor)
		{
			try
			{
				OleDbConnection oleDBConnection = await CreateTestConnection(editor);
				oleDBConnection.Open();
				oleDBConnection.Close();
				MessageBox.Show("Connection Successful", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception)
			{
				MessageBox.Show($"Connection Failed: Could not establish connection", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void BuildConnectionString(object sender, EventArgs e)
		{
			ShowConnectionBuilder();
		}

		public void ShowConnectionBuilder()
		{
			var MSDASCObj = new MSDASC.DataLinks();
			var connection = new ADODB.Connection();
			MSDASCObj.PromptEdit(connection);

			if (!string.IsNullOrEmpty(connection.ConnectionString))
				_connectionString.Text = "@\"" + connection.ConnectionString + "\"";
		}
	}
}
