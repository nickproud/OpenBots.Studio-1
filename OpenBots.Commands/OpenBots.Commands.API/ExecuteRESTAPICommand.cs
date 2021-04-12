using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenBots.Commands.API
{
    [Serializable]
	[Category("API Commands")]
	[Description("This command calls a REST API with a specific HTTP method.")]

	public class ExecuteRESTAPICommand : ScriptCommand
	{
		[Required]
		[DisplayName("Base URL")]
		[Description("Provide the base URL of the API.")]
		[SampleUsage("https://example.com || {vMyUrl}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_BaseURL { get; set; }

		[Required]
		[DisplayName("Endpoint")]
		[Description("Define any API endpoint which contains the full URL.")]
		[SampleUsage("/v2/getUser/1 || {vMyUrl}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_APIEndPoint { get; set; }

		[Required]
		[DisplayName("Method Type")]
		[PropertyUISelectionOption("GET")]
		[PropertyUISelectionOption("POST")]
		[Description("Select the necessary method type.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_APIMethodType { get; set; }

		[Required]
		[DisplayName("Request Format")]
		[PropertyUISelectionOption("Json")]
		[PropertyUISelectionOption("Xml")]
		[PropertyUISelectionOption("None")]
		[Description("Select the necessary request format.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_RequestFormat { get; set; }

		[DisplayName("Basic REST Parameters (Optional)")]
		[Description("Specify default search parameters.")]
		[SampleUsage("")]
		[Remarks("Once you have clicked on a valid window the search parameters will be populated.\n" +
				 "Enable only the ones required to be a match at runtime.")]
		[CompatibleTypes(null, true)]
		public DataTable v_RESTParameters { get; set; }

		[DisplayName("Advanced REST Parameters (Optional)")]
		[Description("Specify a list of advanced parameters.")]
		[SampleUsage("")]
		[Remarks("")]
		[CompatibleTypes(null, true)]
		public DataTable v_AdvancedParameters { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Response Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private DataGridView _RESTParametersGridViewHelper;

		[JsonIgnore]
		[Browsable(false)]
		private DataGridView _advancedRESTParametersGridViewHelper;

		public ExecuteRESTAPICommand()
		{
			CommandName = "ExecuteRESTAPICommand";
			SelectionName = "Execute REST API";
			CommandEnabled = true;
			CommandIcon = Resources.command_run_code;

			v_RequestFormat = "Json";
			v_RESTParameters = new DataTable();
			v_RESTParameters.Columns.Add("Parameter Type");
			v_RESTParameters.Columns.Add("Parameter Name");
			v_RESTParameters.Columns.Add("Parameter Value");
			v_RESTParameters.TableName = DateTime.Now.ToString("RESTParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

			//advanced parameters
			v_AdvancedParameters = new DataTable();
			v_AdvancedParameters.Columns.Add("Parameter Name");
			v_AdvancedParameters.Columns.Add("Parameter Value");
			v_AdvancedParameters.Columns.Add("Content Type");
			v_AdvancedParameters.Columns.Add("Parameter Type");
			v_AdvancedParameters.TableName = DateTime.Now.ToString("AdvRESTParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
		}

		public async override void RunCommand(object sender)
		{
			try
			{
				//make REST Request and store into variable
				string restContent;

				//get engine instance
				var engine = (IAutomationEngineInstance)sender;

				//get parameters
				var targetURL = (string)await v_BaseURL.EvaluateCode(engine);
				var targetEndpoint = (string)await v_APIEndPoint.EvaluateCode(engine);
				var targetMethod = (string)await v_APIMethodType.EvaluateCode(engine);

				//client
				var client = new RestClient(targetURL);

				//methods
				Method method = (Method)Enum.Parse(typeof(Method), targetMethod);

				//rest request
				var request = new RestRequest(targetEndpoint, method);

				//get parameters
				var apiParameters = v_RESTParameters.AsEnumerable().Where(rw => rw.Field<string>("Parameter Type") == "PARAMETER");

				//get headers
				var apiHeaders = v_RESTParameters.AsEnumerable().Where(rw => rw.Field<string>("Parameter Type") == "HEADER");

				//for each api parameter
				foreach (var param in apiParameters)
				{
					var paramName = (string)await ((string)param["Parameter Name"]).EvaluateCode(engine);
					var paramValue = (string)await ((string)param["Parameter Value"]).EvaluateCode(engine);

					request.AddParameter(paramName, paramValue);
				}

				//for each header
				foreach (var header in apiHeaders)
				{
					var paramName = (string)await ((string)header["Parameter Name"]).EvaluateCode(engine);
					var paramValue = (string)await ((string)header["Parameter Value"]).EvaluateCode(engine);

					request.AddHeader(paramName, paramValue);
				}

				//get json body
				var jsonBody = v_RESTParameters.AsEnumerable().Where(rw => rw.Field<string>("Parameter Type") == "JSON BODY")
											   .Select(rw => rw.Field<string>("Parameter Value")).FirstOrDefault();

				//add json body
				if (jsonBody != null)
				{
					var json = (string)await jsonBody.EvaluateCode(engine);
					request.AddJsonBody(jsonBody);
				}

				//get json body
				var file = v_RESTParameters.AsEnumerable().Where(rw => rw.Field<string>("Parameter Type") == "FILE").FirstOrDefault();

				//get file
				if (file != null)
				{
					var paramName = (string)await ((string)file["Parameter Name"]).EvaluateCode(engine);
					var paramValue = (string)await ((string)file["Parameter Value"]).EvaluateCode(engine);
					var fileData = (string)await paramValue.EvaluateCode(engine);
					request.AddFile(paramName, fileData);

				}

				//add advanced parameters
				foreach (DataRow rw in v_AdvancedParameters.Rows)
				{
					var paramName = (string)await rw.Field<string>("Parameter Name").EvaluateCode(engine);
					var paramValue = (string)await rw.Field<string>("Parameter Value").EvaluateCode(engine);
					var paramType = (string)await rw.Field<string>("Parameter Type").EvaluateCode(engine);
					var contentType = (string)await rw.Field<string>("Content Type").EvaluateCode(engine);

					request.AddParameter(paramName, paramValue, contentType, (ParameterType)Enum.Parse(typeof(ParameterType), paramType));
				}

				var requestFormat = (string)await v_RequestFormat.EvaluateCode(engine);
				if (string.IsNullOrEmpty(requestFormat))
				{
					requestFormat = "Xml";
				}
				request.RequestFormat = (DataFormat)Enum.Parse(typeof(DataFormat), requestFormat);


				//execute client request
				IRestResponse response = client.Execute(request);
				var content = response.Content;

				//add service response for tracking
				engine.ServiceResponses.Add(response);

				// return response.Content;
				try
				{
					//try to parse and return json content
					var result = JsonConvert.DeserializeObject(content);
					restContent = result.ToString();
				}
				catch (Exception)
				{
					//content failed to parse simply return it
					restContent = content;
				}

				restContent.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_BaseURL", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_APIEndPoint", this, editor));

			var apiMethodLabel = commandControls.CreateDefaultLabelFor("v_APIMethodType", this);
			var apiMethodDropdown = (ComboBox)commandControls.CreateDropdownFor("v_APIMethodType", this);
			foreach (Method method in (Method[])Enum.GetValues(typeof(Method)))
			{
				apiMethodDropdown.Items.Add(method.ToString());
			}
			RenderedControls.Add(apiMethodLabel);
			RenderedControls.Add(apiMethodDropdown);

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_RequestFormat", this, editor));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_RESTParameters", this));

			_RESTParametersGridViewHelper = commandControls.CreateDefaultDataGridViewFor("v_RESTParameters", this);
			_RESTParametersGridViewHelper.AutoGenerateColumns = false;

			var selectColumn = new DataGridViewComboBoxColumn();
			selectColumn.HeaderText = "Type";
			selectColumn.DataPropertyName = "Parameter Type";
			selectColumn.DataSource = new string[] { "HEADER", "PARAMETER", "JSON BODY", "FILE" };
			_RESTParametersGridViewHelper.Columns.Add(selectColumn);

			var paramNameColumn = new DataGridViewTextBoxColumn();
			paramNameColumn.HeaderText = "Name";
			paramNameColumn.DataPropertyName = "Parameter Name";
			_RESTParametersGridViewHelper.Columns.Add(paramNameColumn);

			var paramValueColumn = new DataGridViewTextBoxColumn();
			paramValueColumn.HeaderText = "Value";
			paramValueColumn.DataPropertyName = "Parameter Value";
			_RESTParametersGridViewHelper.Columns.Add(paramValueColumn);

			RenderedControls.Add(_RESTParametersGridViewHelper);

			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_AdvancedParameters", this));

			//advanced parameters
			_advancedRESTParametersGridViewHelper = commandControls.CreateDefaultDataGridViewFor("v_AdvancedParameters", this);
			_advancedRESTParametersGridViewHelper.AutoGenerateColumns = false;

			var advParamNameColumn = new DataGridViewTextBoxColumn();
			advParamNameColumn.HeaderText = "Name";
			advParamNameColumn.DataPropertyName = "Parameter Name";
			_advancedRESTParametersGridViewHelper.Columns.Add(advParamNameColumn);

			var advParamValueColumns = new DataGridViewTextBoxColumn();
			advParamValueColumns.HeaderText = "Value";
			advParamValueColumns.DataPropertyName = "Parameter Value";
			_advancedRESTParametersGridViewHelper.Columns.Add(advParamValueColumns);

			var advParamContentType = new DataGridViewTextBoxColumn();
			advParamContentType.HeaderText = "Content Type";
			advParamContentType.DataPropertyName = "Content Type";
			_advancedRESTParametersGridViewHelper.Columns.Add(advParamContentType);

			var advParamType = new DataGridViewComboBoxColumn();
			advParamType.HeaderText = "Parameter Type";
			advParamType.DataPropertyName = "Parameter Type";
			advParamType.DataSource = new string[] { "Cookie", "GetOrPost", "HttpHeader", "QueryString", "RequestBody", "URLSegment", "QueryStringWithoutEncode" };
			_advancedRESTParametersGridViewHelper.Columns.Add(advParamType);			
			RenderedControls.Add(_advancedRESTParametersGridViewHelper);

			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Target URL '{v_BaseURL}{v_APIEndPoint}' - Store Response in '{v_OutputUserVariableName}']";
		}
	}
}

