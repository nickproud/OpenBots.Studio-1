using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Server.API_Methods;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Windows.Forms;

namespace OpenBots.Commands.Asset
{
    [Serializable]
    [Category("Asset Commands")]
    [Description("This command increments, decrements, adds, or subtracts an Asset in OpenBots Server.")]
    public class CalculateNumberAssetCommand : ScriptCommand
    {
        [Required]
        [DisplayName("Number Asset Name")]
        [Description("Enter the name of the Asset.")]
        [SampleUsage("Name || {vCredentialName}")]
        [Remarks("This command will throw an exception if an asset of the wrong type is used.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_AssetName { get; set; }

        [Required]
        [DisplayName("Asset Action Type")]
        [PropertyUISelectionOption("Increment")]
        [PropertyUISelectionOption("Decrement")]
        [PropertyUISelectionOption("Add")]
        [PropertyUISelectionOption("Subtract")]
        [Description("Specify the type of calculation of the Asset.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_AssetActionType { get; set; }

        [Required]
        [DisplayName("Asset Action Value")]
        [Description("Enter the new value you would like to add or subtract from the Asset.")]
        [SampleUsage("5 || {vAssetValue}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_AssetActionValue { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        private List<Control> _assetActionValueControls;

        [JsonIgnore]
        [Browsable(false)]
        private bool _hasRendered;

        public CalculateNumberAssetCommand()
        {
            CommandName = "CalculateNumberAssetCommand";
            SelectionName = "Calculate Number Asset";
            CommandEnabled = true;
            CommandIcon = Resources.command_asset;

            v_AssetActionType = "Increment";
            CommonMethods.InitializeDefaultWebProtocol();
        }

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            var vAssetName = v_AssetName.ConvertUserVariableToString(engine);
            var vAssetActionValue = v_AssetActionValue.ConvertUserVariableToString(engine);

            var client = AuthMethods.GetAuthToken();
            var asset = AssetMethods.GetAsset(client, vAssetName, "Number");

            if (asset == null)
                throw new DataException($"No Asset was found for '{vAssetName}' and type 'Number'");

            switch (v_AssetActionType)
            {
                case "Increment":
                    AssetMethods.IncrementAsset(client, asset.Id);
                    break;
                case "Decrement":
                    AssetMethods.DecrementAsset(client, asset.Id);
                    break;
                case "Add":
                    AssetMethods.AddAsset(client, asset.Id, vAssetActionValue);
                    break;
                case "Subtract":
                    AssetMethods.SubtractAsset(client, asset.Id, vAssetActionValue);
                    break;
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_AssetName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_AssetActionType", this, editor));
            ((ComboBox)RenderedControls[4]).SelectedIndexChanged += AssetActionTypeComboBox_SelectedIndexChanged;

            _assetActionValueControls = new List<Control>();
            _assetActionValueControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_AssetActionValue", this, editor));
            
            RenderedControls.AddRange(_assetActionValueControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            if (v_AssetActionType == "Increment")
                return base.GetDisplayValue() + $" [Increment '{v_AssetName}']";
            else if (v_AssetActionType == "Decrement")
                return base.GetDisplayValue() + $" [Decrement '{v_AssetName}']";
            else if (v_AssetActionType == "Add")
                return base.GetDisplayValue() + $" [Add {v_AssetActionValue} to '{v_AssetName}']";
            else
                return base.GetDisplayValue() + $" [Subtract {v_AssetActionValue} From '{v_AssetName}']";
        }

        public override void Shown()
        {
            base.Shown();
            _hasRendered = true;
            AssetActionTypeComboBox_SelectedIndexChanged(this, null);
        }

        private void AssetActionTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((((ComboBox)RenderedControls[4]).Text == "Add" || ((ComboBox)RenderedControls[4]).Text == "Subtract") && _hasRendered)
            {
                foreach (var ctrl in _assetActionValueControls)
                    ctrl.Visible = true;
            }
            else if(_hasRendered)
            {
                foreach (var ctrl in _assetActionValueControls)
                {
                    ctrl.Visible = false;
                    if (ctrl is TextBox)
                        ((TextBox)ctrl).Clear();
                }
            }
        }
    }
}
