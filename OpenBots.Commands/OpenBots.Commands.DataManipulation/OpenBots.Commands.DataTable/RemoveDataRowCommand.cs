﻿using OpenBots.Core.Attributes.PropertyAttributes;
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
		[SampleUsage("{vDataTable}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_DataTable { get; set; }

		[Required]
		[DisplayName("Remove Option")]
		[PropertyUISelectionOption("Tuple")]
		[PropertyUISelectionOption("Index")]
		[Description("Indicate whether DataRow(s) will be deleted by index or tuple.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_RemoveOption { get; set; }

		[Required]
		[DisplayName("Tuple/Index")]
		[Description("Enter a tuple containing the column name and item you would like to remove or enter the index of DataRow you want to remove.")]
		[SampleUsage("(ColumnName1,Item1),(ColumnName2,Item2) || ({vColumn1},{vItem1}),({vCloumn2},{vItem2}) || {vTuple} || 0 || {vIndex}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string), typeof(int) })]
		public string v_SearchItem { get; set; }

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
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_OutputUserVariableName { get; set; }

		public RemoveDataRowCommand()
		{
			CommandName = "RemoveDataRowCommand";
			SelectionName = "Remove DataRow";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			v_AndOr = "And";
			v_RemoveOption = "Tuple";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			dynamic vSearchItem = await v_SearchItem.EvaluateCode(engine);
			OBDataTable Dt = (OBDataTable)await v_DataTable.EvaluateCode(engine, nameof(v_DataTable), this);

			if(v_RemoveOption == "Index")
                Dt.Rows[(int)vSearchItem].Delete();
            else
            {
				var t = new List<Tuple<string, string>>();
				var listPairs = ((string)vSearchItem).Split(')');
				int i = 0;

				listPairs = listPairs.Take(listPairs.Count() - 1).ToArray();

				foreach (string item in listPairs)
				{
					string temp;
					temp = item.Trim('(', '"');
					var tempList = temp.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

					for (int z = 0; z < tempList.Count; z++)
						tempList[z] = tempList[z].Trim('(');

					t.Insert(i, Tuple.Create(tempList[0], tempList[1]));
					i++;
				}

				List<DataRow> listrows = Dt.AsEnumerable().ToList();
				if (v_AndOr == "Or")
				{
					List<DataRow> templist = new List<DataRow>();
					//for each filter
					foreach (Tuple<string, string> tuple in t)
					{
						//for each datarow
						foreach (DataRow item in listrows)
						{
							if (item[tuple.Item1] != null)
							{
								if (item[tuple.Item1].ToString() == tuple.Item2.ToString())
								{
									//add to list if filter matches
									if (!templist.Contains(item))
										templist.Add(item);
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
					List<DataRow> templist = new List<DataRow>(listrows);

					//for each tuple
					foreach (Tuple<string, string> tuple in t)
					{
						//for each datarow
						foreach (DataRow drow in Dt.AsEnumerable())
						{
							if (drow[tuple.Item1].ToString() != tuple.Item2)
							{
								//remove from list if filter matches
								templist.Remove(drow);
							}
						}
					}

					foreach (DataRow item in templist)
						Dt.Rows.Remove(item);
				}
			}

			Dt.AcceptChanges();
			Dt.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_RemoveOption", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SearchItem", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_AndOr", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Remove Rows With '{v_SearchItem}' From '{v_DataTable}' - Store DataTable in '{v_OutputUserVariableName}']";
		}       
	}
}
