using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Server.API_Methods;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using OpenBots.Core.Properties;

namespace OpenBots.Commands.Asset
{
	[Serializable]
	[Category("Asset Commands")]
	[Description("This command appends text to the end of a text Asset in OpenBots Server.")]
	public class AppendTextAssetCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Text Asset Name")]
		[Description("Enter the name of the Asset.")]
		[SampleUsage("Name || {vAssetName}")]
		[Remarks("This command will throw an exception if an asset of the wrong type is used.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_AssetName { get; set; }

		[Required]
		[DisplayName("Append Text")]
		[Description("Enter the text value to append.")]
		[SampleUsage("Smith || {vAssetValue}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_AppendText { get; set; }


		public AppendTextAssetCommand()
		{
			CommandName = "AppendTextAssetCommand";
			SelectionName = "Append Text Asset";
			CommandEnabled = true;
			CommandIcon = Resources.command_asset;

			CommonMethods.InitializeDefaultWebProtocol();
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var vAssetName = v_AssetName.ConvertUserVariableToString(engine);
			var vAppendText = v_AppendText.ConvertUserVariableToString(engine);

			var client = AuthMethods.GetAuthToken();
			var asset = AssetMethods.GetAsset(client, vAssetName, "Text");

			if (asset == null)
				throw new DataException($"No Asset was found for '{vAssetName}' with type 'Text'");

			AssetMethods.AppendAsset(client, asset.Id, vAppendText);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_AssetName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_AppendText", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
				return base.GetDisplayValue() + $" ['{v_AssetName} With Value '{v_AppendText}']";
		}
	}

}
