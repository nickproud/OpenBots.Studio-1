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
using System.Threading.Tasks;
using System.Windows.Forms;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.DataTable
{
    [Serializable]
	[Category("DataTable Commands")]
	[Description("This command filters specific rows from a DataTable into a new Datatable.")]
	public class FilterDataTableCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Input DataTable")]
		[Description("Enter the DataTable to filter through.")]
		[SampleUsage("vDataTable")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_DataTable { get; set; }

		[Required]
		[DisplayName("Filter Option")]
		[PropertyUISelectionOption("RowFilter")]
		[PropertyUISelectionOption("Filter Data")]
		[Description("Indicate whether this command should filter datatable based on a Tuple or RowFilter")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_FilterOption { get; set; }

		[Required]
		[DisplayName("RowFilter")]
		[Description("Enter a RowFilter")]
		[SampleUsage("\"[Employee Age] > 30\" || \"Name <> 'John'\" || vRowFilter")]
		[Remarks("DataRows must match all provided tuples to be included in the filtered DataTable. Column names containing spaces should be surrounded by [].")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_RowFilter { get; set; }

		[Required]
		[DisplayName("Filter Data")]
		[Description("Enter the column name and item you would like to filter by.")]
		[SampleUsage("[ \"First Name\" | \"John\" ] || [ vColumn | vData ]")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string), typeof(object) })]
		public OBDataTable v_DataRowDataTable { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Filtered DataTable Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(OBDataTable)})]
		public string v_OutputUserVariableName { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _rowFilterControls;

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _dataControls;

		[JsonIgnore]
		[Browsable(false)]
		private bool _hasRendered;

		public FilterDataTableCommand()
		{
			CommandName = "FilterDataTableCommand";
			SelectionName = "Filter DataTable";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

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
			OBDataTable Dt = (OBDataTable)await v_DataTable.EvaluateCode(engine);

            if (v_FilterOption == "RowFilter")
            {
				DataView dv = new DataView(Dt);
				dv.RowFilter = (string)await v_RowFilter.EvaluateCode(engine);
				dv.ToTable().SetVariableValue(engine, v_OutputUserVariableName);
			}
            else
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
							if (row[columnName] == data && !templist.Contains(row))
								templist.Add(row);
						}
					}
				}

				OBDataTable outputDT = new OBDataTable();
				int x = 0;

				foreach (DataColumn column in Dt.Columns)
				{
					outputDT.Columns.Add(Dt.Columns[x].ToString());
					x++;
				}

				foreach (DataRow item in templist)
					outputDT.Rows.Add(item.ItemArray);

				outputDT.SetVariableValue(engine, v_OutputUserVariableName);
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));
			
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_FilterOption", this, editor));
			((ComboBox)RenderedControls[4]).SelectedIndexChanged += OptionComboBox_SelectedIndexChanged;

			_rowFilterControls = commandControls.CreateDefaultInputGroupFor("v_RowFilter", this, editor);
			RenderedControls.AddRange(_rowFilterControls);

			_dataControls = commandControls.CreateDefaultDataGridViewGroupFor("v_DataRowDataTable", this, editor);
			RenderedControls.AddRange(_dataControls);

			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue()+ $" [Filter Rows From '{v_DataTable}' - Store Filtered DataTable in '{v_OutputUserVariableName}']";
		}

		public override void Shown()
		{
			base.Shown();
			_hasRendered = true;
			if (v_FilterOption == null)
			{
				v_FilterOption = "RowFilter";
				((ComboBox)RenderedControls[4]).Text = v_FilterOption;
			}
			OptionComboBox_SelectedIndexChanged(this, null);
		}

		private void OptionComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[4]).Text == "RowFilter" && _hasRendered)
			{
				foreach (var ctrl in _dataControls)
				{
					ctrl.Visible = false;
					if (ctrl is DataGridView)
						v_DataRowDataTable.Rows.Clear();
				}
				foreach (var ctrl in _rowFilterControls)
					ctrl.Visible = true;
			}
			else if (_hasRendered)
			{
				foreach (var ctrl in _dataControls)
					ctrl.Visible = true;
				foreach (var ctrl in _rowFilterControls)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
			}
		}
	}
}