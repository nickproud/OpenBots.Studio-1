using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

namespace OpenBots.Commands.Data
{
    [Serializable]
	[Category("Data Commands")]
	[Description("This command runs a number of queries on a JSON object and saves the results in the specified list variables.")]
	public class ParseJSONModelCommand : ScriptCommand
	{
		[Required]
		[DisplayName("JSON")]
		[Description("Provide a variable or JSON object value.")]
		[SampleUsage("@\"{animals: ['cat', 'dog']}\" || vJsonObject")]
		[Remarks("Providing data of a type other than a 'JSON Object' will result in an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_JsonObject { get; set; }

		[Required]
		[DisplayName("Parameters")]
		[Description("Specify JSON Selector(s) (JPath) and Output Variable(s).")]
		[SampleUsage("[\"$.animals.length\" | vOutputList] || [\"animals\" | vOutputList] || [vSelector | vOutputList]")]
		[Remarks("'$.animals.length' is a JSON Selector to query on an inputted JSON Object and store its results in vOutputList.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string), typeof(List<string>) })]
		public OBDataTable v_ParseObjects { get; set; }

		public ParseJSONModelCommand()
		{
			CommandName = "ParseJSONModelCommand";
			SelectionName = "Parse JSON Model";
			CommandEnabled = true;
			CommandIcon = Resources.command_parse;

			v_ParseObjects = new OBDataTable();
			v_ParseObjects.Columns.Add("Json Selector");
			v_ParseObjects.Columns.Add("Output Variable");
			v_ParseObjects.TableName = $"ParseJsonObjectsTable{DateTime.Now.ToString("MMddyyhhmmss")}";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var variableInput = (string)await v_JsonObject.EvaluateCode(engine);

			foreach (DataRow rw in v_ParseObjects.Rows)
			{
				var jsonSelector = (string)await rw.Field<string>("Json Selector").EvaluateCode(engine);
				var targetVariableName = rw.Field<string>("Output Variable");

				//create objects
				JObject o;
				IEnumerable<JToken> searchResults;
				List<string> resultList = new List<string>();

				//parse json
				try
				{
					o = JObject.Parse(variableInput);
				}
				catch (Exception ex)
				{
					throw new Exception("Error Occured Parsing Tokens: " + ex.ToString());
				}

				//select results
				try
				{
					searchResults = o.SelectTokens(jsonSelector);
				}
				catch (Exception ex)
				{
					throw new Exception("Error Occured Selecting Tokens: " + ex.ToString());
				}

				//add results to result list since list<string> is supported
				foreach (var result in searchResults)
					resultList.Add(result.ToString());

				resultList.SetVariableValue(engine, targetVariableName);               
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_JsonObject", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDataGridViewGroupFor("v_ParseObjects", this, editor));
			
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return $"{base.GetDisplayValue()} [Select {v_ParseObjects.Rows.Count} Item(s) From JSON]";
		}
	}
}