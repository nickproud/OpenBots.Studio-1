using Newtonsoft.Json.Linq;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;

namespace OpenBots.Commands.Asset.Test
{
    public class UpdateAssetCommandTests
    {
        private AutomationEngineInstance _engine;
        private UpdateAssetCommand _updateAsset;
        private GetAssetCommand _getAsset;

        [Fact]
        public async void UpdatesTextAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _updateAsset = new UpdateAssetCommand();
            _getAsset = new GetAssetCommand();

            string assetName = "testTextAsset";
            string newAsset = "newText";
            VariableMethods.CreateTestVariable(assetName, _engine, "assetName", typeof(string));
            VariableMethods.CreateTestVariable(newAsset, _engine, "newAsset", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _updateAsset.v_AssetName = "{assetName}";
            _updateAsset.v_AssetType = "Text";
            _updateAsset.v_AssetFilePath = "";
            _updateAsset.v_AssetValue = "{newAsset}";

            _updateAsset.RunCommand(_engine);

            _getAsset.v_AssetName = "{assetName}";
            _getAsset.v_AssetType = "Text";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            string outputAsset = (string)await "{output}".EvaluateCode(_engine);
            Assert.Equal("newText", outputAsset);

            resetAsset(assetName, "testText", "Text");
        }

        [Fact]
        public async void UpdatesNumberAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _updateAsset = new UpdateAssetCommand();
            _getAsset = new GetAssetCommand();

            string assetName = "testNumberAsset";
            string newAsset = "70";
            VariableMethods.CreateTestVariable(assetName, _engine, "assetName", typeof(string));
            VariableMethods.CreateTestVariable(newAsset, _engine, "newAsset", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _updateAsset.v_AssetName = "{assetName}";
            _updateAsset.v_AssetType = "Number";
            _updateAsset.v_AssetFilePath = "";
            _updateAsset.v_AssetValue = "{newAsset}";

            _updateAsset.RunCommand(_engine);

            _getAsset.v_AssetName = "{assetName}";
            _getAsset.v_AssetType = "Number";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            string outputAsset = (string)await "{output}".EvaluateCode(_engine);
            Assert.Equal("70", outputAsset);

            resetAsset(assetName, "42", "Number");
        }

        [Fact]
        public async void UpdatesJsonAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _updateAsset = new UpdateAssetCommand();
            _getAsset = new GetAssetCommand();

            string assetName = "testJsonAsset";
            string newAsset = "{ \"text\": \"newText\" }";
            VariableMethods.CreateTestVariable(assetName, _engine, "assetName", typeof(string));
            VariableMethods.CreateTestVariable(newAsset, _engine, "newAsset", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _updateAsset.v_AssetName = "{assetName}";
            _updateAsset.v_AssetType = "Json";
            _updateAsset.v_AssetFilePath = "";
            _updateAsset.v_AssetValue = "{newAsset}";

            _updateAsset.RunCommand(_engine);

            _getAsset.v_AssetName = "{assetName}";
            _getAsset.v_AssetType = "Json";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            JObject outputAsset = (JObject)await "{output}".EvaluateCode(_engine);
            Assert.Equal("newText", outputAsset["text"]);

            resetAsset(assetName, "{ \"text\": \"testText\" }", "Json");
        }

        [Fact]
        public void UpdatesFileAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _updateAsset = new UpdateAssetCommand();
            _getAsset = new GetAssetCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filepath = Path.Combine(projectDirectory, @"Resources\");
            string assetName = "testUpdateFileAsset";
            string newAsset = filepath + @"Upload\newtest.txt";
            VariableMethods.CreateTestVariable(assetName, _engine, "assetName", typeof(string));
            VariableMethods.CreateTestVariable(newAsset, _engine, "newAsset", typeof(string));

            _updateAsset.v_AssetName = "{assetName}";
            _updateAsset.v_AssetType = "File";
            _updateAsset.v_AssetFilePath = "{newAsset}";

            _updateAsset.RunCommand(_engine);

            _getAsset.v_AssetName = "{assetName}";
            _getAsset.v_AssetType = "File";
            _getAsset.v_OutputDirectoryPath = filepath + @"Download\";

            if (!Directory.Exists(_getAsset.v_OutputDirectoryPath))
                Directory.CreateDirectory(_getAsset.v_OutputDirectoryPath);

            _getAsset.RunCommand(_engine);

            Assert.True(File.Exists(filepath + @"Download\newtest.txt"));

            File.Delete(filepath + @"Download\newtest.txt");
            resetAsset(assetName, filepath + @"Upload\oldtest.txt", "File");
        }

        [Fact]
        public async System.Threading.Tasks.Task HandlesNonexistentAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _updateAsset = new UpdateAssetCommand();
            _getAsset = new GetAssetCommand();

            string assetName = "noAsset";
            string newAsset = "newText";
            VariableMethods.CreateTestVariable(assetName, _engine, "{assetName}", typeof(string));
            VariableMethods.CreateTestVariable(newAsset, _engine, "{newAsset}", typeof(string));

            _updateAsset.v_AssetName = "{assetName}";
            _updateAsset.v_AssetType = "Text";
            _updateAsset.v_AssetFilePath = "";
            _updateAsset.v_AssetValue = "{newAsset}";

            Assert.ThrowsAsync<ArgumentNullException>(() => _updateAsset.RunCommand(_engine));
        }

        private void resetAsset(string assetName, string assetVal, string type)
        {
            _engine = new AutomationEngineInstance(null);
            _updateAsset = new UpdateAssetCommand();

            VariableMethods.CreateTestVariable(assetName, _engine, "assetName", typeof(string));
            VariableMethods.CreateTestVariable(assetVal, _engine, "assetVal", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _updateAsset.v_AssetName = "{assetName}";
            _updateAsset.v_AssetType = type;
            _updateAsset.v_AssetValue = "{assetVal}";

            if (type == "File")
            {
                _updateAsset.v_AssetFilePath = assetVal;
            }

            _updateAsset.RunCommand(_engine);
        }
    }
}