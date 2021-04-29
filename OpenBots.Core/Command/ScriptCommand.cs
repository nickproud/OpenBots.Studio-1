using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Script;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Core.Command
{
	public abstract class ScriptCommand
	{
		[Browsable(false)]
		public string CommandID { get; set; }

		[Browsable(false)]
		public string CommandName { get; set; }

		[Browsable(false)]
		public string SelectionName { get; set; }

		[Browsable(false)]
		public int LineNumber { get; set; }

		[Browsable(false)]
		public bool IsCommented { get; set; }

		[Browsable(false)]
		public bool PauseBeforeExecution { get; set; }

		[Browsable(false)]
		public bool CommandEnabled { get; set; }

		[Browsable(false)]
		public bool ScopeStartCommand { get; set; }

		[DisplayName("Private (Optional)")]
		[Description("Optional field to mark the command as private (data sensitive) in order to avoid its logging.")]
		[SampleUsage("")]
		[Remarks("")]
		public bool v_IsPrivate { get; set; }

		[Required]
		[DisplayName("Error Handling")]
		[Description("Optional field for how to handle errors encountered.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_ErrorHandling { get; set; }

		[DisplayName("Comment Field (Optional)")]
		[Description("Optional field to enter a custom comment which could potentially describe this command or the need for this command, if required.")]
		[SampleUsage("\"I am using this command to ...\"")]
		[Remarks("Optional")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_Comment { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		public Image CommandIcon { get; set; } = Resources.command_function;

		[JsonIgnore]
		[Browsable(false)]
		public List<Control> RenderedControls;

		[JsonIgnore]
		[Browsable(false)]
		public bool IsSteppedInto { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		public IfrmScriptBuilder CurrentScriptBuilder { get; set; }

		public ScriptCommand()
		{
			CommandEnabled = false;
			IsCommented = false;
			v_ErrorHandling = "None";
			GenerateID();
		}

		public void GenerateID()
		{
			var id = Guid.NewGuid();
			CommandID = id.ToString();
		}

		public async virtual Task RunCommand(object sender)
		{
		}

		public async virtual Task RunCommand(object sender, ScriptAction command)
		{
		}

		public virtual List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			RenderedControls = new List<Control>();
			return RenderedControls;
		}

		public virtual string GetDisplayValue()
		{
			if (string.IsNullOrEmpty(v_Comment))
				return SelectionName;
			else
				return $"{v_Comment} - " + SelectionName;
		}

		public virtual void Shown()
        {
        }
	}
}