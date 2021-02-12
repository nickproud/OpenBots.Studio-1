using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using Xunit;
using System.Data;

namespace OpenBots.Commands.Asset.Test
{
    public class AppendTextAssetCommandTests
    {
        private AutomationEngineInstance _engine;
        private AppendTextAssetCommand _appendTextAsset;
        private GetAssetCommand _getAsset;
        private UpdateAssetCommand _updateAsset;

        [Fact]
        public void AppendsTextAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _appendTextAsset = new AppendTextAssetCommand();
            _getAsset = new GetAssetCommand();

            string assetName = "testUpdateTextAsset";
            string toAppend = "textToAppend";

            assetName.CreateTestVariable(_engine, "assetName");
            toAppend.CreateTestVariable(_engine, "toAppend");
            "unassigned".CreateTestVariable(_engine, "initialText");
            "unassigned".CreateTestVariable(_engine, "updatedAsset");

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

            string initialText = "{initialText}".ConvertUserVariableToString(_engine);
            string updatedAsset = "{updatedAsset}".ConvertUserVariableToString(_engine);

            resetAsset(assetName, initialText, "Text");

            Assert.Equal(initialText + " " + toAppend, updatedAsset);
        }

        [Fact]
        public void HandlesNonexistentAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _appendTextAsset = new AppendTextAssetCommand();

            string assetName = "doesNotExist";
            string toAppend = "textToAppend";

            assetName.CreateTestVariable(_engine, "{assetName}");
            toAppend.CreateTestVariable(_engine, "{toAppend}");

            Assert.Throws<DataException>(() => _appendTextAsset.RunCommand(_engine));
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