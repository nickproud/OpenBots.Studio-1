using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.DataTable
{
	[Serializable]
	[Category("DataTable Commands")]
	[Description("This command removes specific DataRows from a DataTable.")]
	public class RemoveDataRowCommand : ScriptCommand
	{
		[Required]
		[DisplayName("DataTable")]
		[Description("Enter an existing DataTable.")]
		[SampleUsage("vDataTable")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_DataTable { get; set; }

		[Required]
		[DisplayName("Remove Option")]
		[PropertyUISelectionOption("Index")]
		[PropertyUISelectionOption("Data")]
		[Description("Indicate whether DataRow(s) will be deleted by index or tuple.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_RemoveOption { get; set; }

		[Required]
		[DisplayName("Index")]
		[Description("Enter the index of the DataRow to remove.")]
		[SampleUsage("0 || vIndex")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_Index { get; set; }

		[Required]
		[DisplayName("Data")]
		[Description("Enter the column name and item of the row to remove.")]
		[SampleUsage("[ \"First Name\" | \"John\" ] || [ vColumn | vData ]")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string), typeof(object) })]
		public OBDataTable v_DataRowDataTable { get; set; }

		[Required]
		[DisplayName("Overwrite Option")]
		[PropertyUISelectionOption("And")]
		[PropertyUISelectionOption("Or")]
		[Description("Indicate whether this command should remove rows with all the constraints or remove those with 1 or more constraints.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_AndOr { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output DataTable Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_OutputUserVariableName { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _indexControls;

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _dataControls;

		[JsonIgnore]
		[Browsable(false)]
		private bool _hasRendered;

		public RemoveDataRowCommand()
		{
			CommandName = "RemoveDataRowCommand";
			SelectionName = "Remove DataRow";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			v_AndOr = "And";

			//initialize data table
			v_DataRowDataTable = new OBDataTable
			{
				TableName = "AddDataDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
			};

			v_DataRowDataTable.Columns.Add("Column Name");
			v_DataRowDataTable.Columns.Add("Data");
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			dynamic vSearchItem = await v_Index.EvaluateCode(engine);
			OBDataTable Dt = (OBDataTable)await v_DataTable.EvaluateCode(engine);

			if(v_RemoveOption == "Index")
                Dt.Rows[(int)vSearchItem].Delete();
            else
            {
				if (v_AndOr == "Or")
				{
					List<DataRow> templist = new List<DataRow>();
		
					foreach (DataRow rw in v_DataRowDataTable.Rows)
					{
						var columnName = (string)await rw.Field<string>("Column Name").EvaluateCode(engine);
						var data = await rw.Field<dynamic>("Data").EvaluateCode(engine);

						foreach (DataRow row in Dt.Rows)
						{
							if (row[columnName] != null)
							{
								if (row[columnName] == data)
                                {
									//add to list if filter matches
									if (!templist.Contains(row))
										templist.Add(row);
								}
							}
						}
					}

					foreach (DataRow item in templist)
						Dt.Rows.Remove(item);
				}

				//If And operation is checked
				if (v_AndOr == "And")
				{
					List<DataRow> listrows = Dt.AsEnumerable().ToList();
					List<DataRow> templist = new List<DataRow>(listrows);

					foreach (DataRow rw in v_DataRowDataTable.Rows)
					{
						var columnName = (string)await rw.Field<string>("Column Name").EvaluateCode(engine);
						var data = await rw.Field<dynamic>("Data").EvaluateCode(engine);

						foreach (DataRow row in Dt.Rows)
						{
							if (row[columnName] == data)
								templist.Remove(row);
						}
					}

					foreach (DataRow item in templist)
						Dt.Rows.Remove(item);
				}
			}

			Dt.AcceptChanges();
			Dt.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_RemoveOption", this, editor));
			((ComboBox)RenderedControls[4]).SelectedIndexChanged += OptionComboBox_SelectedIndexChanged;

			_indexControls = commandControls.CreateDefaultInputGroupFor("v_Index", this, editor);
			RenderedControls.AddRange(_indexControls);

			_dataControls = commandControls.CreateDefaultDataGridViewGroupFor("v_DataRowDataTable", this, editor);
			RenderedControls.AddRange(_dataControls);

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_AndOr", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Remove Rows From '{v_DataTable}' - Store DataTable in '{v_OutputUserVariableName}']";
		}

		public override void Shown()
		{
			base.Shown();
			_hasRendered = true;
			if (v_RemoveOption == null)
			{
				v_RemoveOption = "Index";
				((ComboBox)RenderedControls[4]).Text = v_RemoveOption;
			}
			OptionComboBox_SelectedIndexChanged(this, null);
		}

		private void OptionComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[4]).Text == "Index" && _hasRendered)
			{
				foreach (var ctrl in _dataControls)
				{
					ctrl.Visible = false;
					if (ctrl is DataGridView)
						v_DataRowDataTable.Rows.Clear();
				}
				foreach (var ctrl in _indexControls)
					ctrl.Visible = true;
			}
			else if (_hasRendered)
			{
				foreach (var ctrl in _dataControls)
					ctrl.Visible = true;
				foreach (var ctrl in _indexControls)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
			}
		}
	}
}
