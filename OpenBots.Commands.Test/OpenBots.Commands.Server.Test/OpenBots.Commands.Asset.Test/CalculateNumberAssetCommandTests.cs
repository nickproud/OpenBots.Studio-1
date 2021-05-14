using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using Xunit;

namespace OpenBots.Commands.Asset.Test
{
    public class CalculateNumberAssetCommandTests
    {
        private AutomationEngineInstance _engine;
        private CalculateNumberAssetCommand _calculateAsset;
        private GetAssetCommand _getAsset;
        private UpdateAssetCommand _updateAsset;

        [Fact]
        public async void IncrementsNumberAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _calculateAsset = new CalculateNumberAssetCommand();
            _getAsset = new GetAssetCommand();

            string assetName = "testIncrementNumberAsset";
            string newAsset = "50";
            VariableMethods.CreateTestVariable(assetName, _engine, "assetName", typeof(string));
            VariableMethods.CreateTestVariable(newAsset, _engine, "newAsset", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _calculateAsset.v_AssetName = "{assetName}";
            _calculateAsset.v_AssetActionType = "Increment";
            _calculateAsset.v_AssetActionValue = "";

            _calculateAsset.RunCommand(_engine);

            _getAsset.v_AssetName = "{assetName}";
            _getAsset.v_AssetType = "Number";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            string outputAsset = (string)await "{output}".EvaluateCode(_engine);
            Assert.Equal(newAsset, outputAsset);

            resetAsset(assetName, "49", "Number");
        }

        [Fact]
        public async void DecrementsNumberAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _calculateAsset = new CalculateNumberAssetCommand();
            _getAsset = new GetAssetCommand();

            string assetName = "testIncrementNumberAsset";
            string newAsset = "48";
            VariableMethods.CreateTestVariable(assetName, _engine, "assetName", typeof(string));
            VariableMethods.CreateTestVariable(newAsset, _engine, "newAsset", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _calculateAsset.v_AssetName = "{assetName}";
            _calculateAsset.v_AssetActionType = "Decrement";
            _calculateAsset.v_AssetActionValue = "";

            _calculateAsset.RunCommand(_engine);

            _getAsset.v_AssetName = "{assetName}";
            _getAsset.v_AssetType = "Number";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            string outputAsset = (string)await "{output}".EvaluateCode(_engine);
            Assert.Equal(newAsset, outputAsset);

            resetAsset(assetName, "49", "Number");
        }

        [Fact]
        public async void AddsNumberAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _calculateAsset = new CalculateNumberAssetCommand();
            _getAsset = new GetAssetCommand();

            string assetName = "testIncrementNumberAsset";
            string newAsset = "54";
            VariableMethods.CreateTestVariable(assetName, _engine, "assetName", typeof(string));
            VariableMethods.CreateTestVariable(newAsset, _engine, "newAsset", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _calculateAsset.v_AssetName = "{assetName}";
            _calculateAsset.v_AssetActionType = "Add";
            _calculateAsset.v_AssetActionValue = "5";

            _calculateAsset.RunCommand(_engine);

            _getAsset.v_AssetName = "{assetName}";
            _getAsset.v_AssetType = "Number";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            string outputAsset = (string)await "{output}".EvaluateCode(_engine);
            Assert.Equal(newAsset, outputAsset);

            resetAsset(assetName, "49", "Number");
        }

        [Fact]
        public async void SubtractsNumberAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _calculateAsset = new CalculateNumberAssetCommand();
            _getAsset = new GetAssetCommand();

            string assetName = "testIncrementNumberAsset";
            string newAsset = "43";
            VariableMethods.CreateTestVariable(assetName, _engine, "assetName", typeof(string));
            VariableMethods.CreateTestVariable(newAsset, _engine, "newAsset", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _calculateAsset.v_AssetName = "{assetName}";
            _calculateAsset.v_AssetActionType = "Subtract";
            _calculateAsset.v_AssetActionValue = "6";

            _calculateAsset.RunCommand(_engine);

            _getAsset.v_AssetName = "{assetName}";
            _getAsset.v_AssetType = "Number";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            string outputAsset = (string)await "{output}".EvaluateCode(_engine);
            Assert.Equal(newAsset, outputAsset);

            resetAsset(assetName, "49", "Number");
        }

        [Fact]
        public async System.Threading.Tasks.Task HandlesNonexistentAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _calculateAsset = new CalculateNumberAssetCommand();
            _getAsset = new GetAssetCommand();

            string assetName = "noAsset";
            string newAsset = "50";
            VariableMethods.CreateTestVariable(assetName, _engine, "{assetName}", typeof(string));
            VariableMethods.CreateTestVariable(newAsset, _engine, "{assetName}", typeof(string));

            _calculateAsset.v_AssetName = "{assetName}";
            _calculateAsset.v_AssetActionType = "Increment";
            _calculateAsset.v_AssetActionValue = "";

            Assert.ThrowsAsync<ArgumentNullException>(() => _calculateAsset.RunCommand(_engine));
        }

        private void resetAsset(string assetName, string assetValue, string type)
        {
            _engine = new AutomationEngineInstance(null);
            _updateAsset = new UpdateAssetCommand();

            _updateAsset.v_AssetName = assetName;
            _updateAsset.v_AssetType = type;
            _updateAsset.v_AssetValue = assetValue;

            _updateAsset.RunCommand(_engine);
        }
    }
}