using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Properties;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.Utilities.CommandUtilities;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Security;

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
		[CompatibleTypes(new Type[] { typeof(OleDbConnection) })]
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

			v_InstanceName = "DefaultDatabase";
			v_TestConnection = "Yes";
		}

		public async override Task RunCommand(object sender)
		{
			//get engine and preference
			var engine = (IAutomationEngineInstance)sender;

			//create connection
			var oleDBConnection = await CreateConnection(sender);

			//attempt to open and close connection
			if (v_TestConnection == "Yes")
			{
				oleDBConnection.Open();
				oleDBConnection.Close();
			}

			oleDBConnection.AddAppInstance(engine, v_InstanceName);
		}

		private async Task<OleDbConnection> CreateConnection(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var connection = (string)await v_ConnectionString.EvaluateCode(engine);
			var connectionSecurePass = (SecureString)await v_ConnectionStringPassword.EvaluateCode(engine);
			var connectionPass = "";
			if(connectionSecurePass != null)
				connectionPass = connectionSecurePass.ConvertSecureStringToString();

			connection = connection.Replace("#pwd", connectionPass);

			return new OleDbConnection(connection);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

			CommandItemControl helperControl = new CommandItemControl();
			helperControl.Padding = new Padding(10, 0, 0, 0);
			helperControl.ForeColor = Color.AliceBlue;
			helperControl.Font = new Font("Segoe UI Semilight", 10);
			helperControl.Name = "connection_helper";
			helperControl.CommandImage = Resources.command_database;
			helperControl.CommandDisplay = "Build Connection String";
			helperControl.Click += (sender, e) => Button_Click(sender, e);

			_connectionString = commandControls.CreateDefaultInputFor("v_ConnectionString", this);

			var connectionLabel = commandControls.CreateDefaultLabelFor("v_ConnectionString", this);
			var connectionHelpers = commandControls.CreateUIHelpersFor("v_ConnectionString", this, new[] { _connectionString }, editor);
			CommandItemControl testConnectionControl = new CommandItemControl();
			testConnectionControl.Padding = new Padding(10, 0, 0, 0);
			testConnectionControl.ForeColor = Color.AliceBlue;
			testConnectionControl.Font = new Font("Segoe UI Semilight", 10);
			testConnectionControl.Name = "connection_helper";
			testConnectionControl.CommandImage = Resources.command_database;
			testConnectionControl.CommandDisplay = "Test Connection";
			testConnectionControl.Click += (sender, e) => TestConnection(sender, e, editor, commandControls);

			RenderedControls.Add(connectionLabel);
			RenderedControls.AddRange(connectionHelpers);
			RenderedControls.Add(helperControl);
			RenderedControls.Add(testConnectionControl);
			RenderedControls.Add(_connectionString);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ConnectionStringPassword", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_TestConnection", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Instance Name '{v_InstanceName}']";
		}

		private async void TestConnection(object sender, EventArgs e, IfrmCommandEditor editor, ICommandControls commandControls)
		{
			
			try
			{
				var engine = (IAutomationEngineInstance)commandControls.CreateAutomationEngineInstance(editor.ScriptEngineContext);
				var oleDBConnection = await CreateConnection(engine);
				oleDBConnection.Open();
				oleDBConnection.Close();
				MessageBox.Show("Connection Successful", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Connection Failed: {ex}", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void Button_Click(object sender, EventArgs e)
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
