using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using Xunit;
using System;

namespace OpenBots.Commands.Asset.Test
{
    public class AppendTextAssetCommandTests
    {
        private AutomationEngineInstance _engine;
        private AppendTextAssetCommand _appendTextAsset;
        private GetAssetCommand _getAsset;
        private UpdateAssetCommand _updateAsset;

        [Fact]
        public async void AppendsTextAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _appendTextAsset = new AppendTextAssetCommand();
            _getAsset = new GetAssetCommand();

            string assetName = "testUpdateTextAsset";
            string toAppend = "textToAppend";

            VariableMethods.CreateTestVariable(assetName, _engine, "assetName", typeof(string));
            VariableMethods.CreateTestVariable(toAppend, _engine, "toAppend", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "initialText", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "updatedAsset", typeof(string));

            _getAsset.v_AssetName = assetName;
            _getAsset.v_AssetType = "Text";
            _getAsset.v_OutputUserVariableName = "{initialText}";

            _getAsset.RunCommand(_engine);

            _appendTextAsset.v_AssetName = "{assetName}";
            _appendTextAsset.v_AppendText = "{toAppend}";

            _appendTextAsset.RunCommand(_engine);

            _getAsset.v_AssetName = assetName;
            _getAsset.v_AssetType = "Text";
            _getAsset.v_OutputUserVariableName = "{updatedAsset}";

            _getAsset.RunCommand(_engine);

            string initialText = (string)await "{initialText}".EvaluateCode(_engine);
            string updatedAsset = (string)await "{updatedAsset}".EvaluateCode(_engine);

            resetAsset(assetName, initialText, "Text");

            Assert.Equal(initialText + " " + toAppend, updatedAsset);
        }

        [Fact]
        public async System.Threading.Tasks.Task HandlesNonexistentAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _appendTextAsset = new AppendTextAssetCommand();

            string assetName = "doesNotExist";
            string toAppend = "textToAppend";

            VariableMethods.CreateTestVariable(assetName, _engine, "{assetName}", typeof(string));
            VariableMethods.CreateTestVariable(toAppend, _engine, "{toAppend}", typeof(string));

            Assert.ThrowsAsync<InvalidOperationException>(() => _appendTextAsset.RunCommand(_engine));
        }

        private void resetAsset(string assetName, string assetVal, string type)
        {
            _engine = new AutomationEngineInstance(null);
            _updateAsset = new UpdateAssetCommand();

            _updateAsset.v_AssetName = assetName;
            _updateAsset.v_AssetType = type;
            _updateAsset.v_AssetValue = assetVal;

            if (type == "File")
            {
                _updateAsset.v_AssetFilePath = assetVal;
            }

            _updateAsset.RunCommand(_engine);
        }
    }
}