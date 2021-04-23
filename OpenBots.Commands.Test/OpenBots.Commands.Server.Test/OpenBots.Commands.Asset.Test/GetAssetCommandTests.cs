﻿using Newtonsoft.Json.Linq;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Data;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace OpenBots.Commands.Asset.Test
{
    public class GetAssetCommandTests
    {
        private AutomationEngineInstance _engine;
        private GetAssetCommand _getAsset;
        private readonly ITestOutputHelper output;

        public GetAssetCommandTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async void GetsTextAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _getAsset = new GetAssetCommand();

            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _getAsset.v_AssetName = "testTextAsset";
            _getAsset.v_AssetType = "Text";
            _getAsset.v_OutputDirectoryPath = "";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            Assert.Equal("testText", (string)await "{output}".EvaluateCodeForTests(_engine));
        }

        [Fact]
        public async void GetsNumberAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _getAsset = new GetAssetCommand();

            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _getAsset.v_AssetName = "testNumberAsset";
            _getAsset.v_AssetType = "Number";
            _getAsset.v_OutputDirectoryPath = "";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            var asset = (string)await "{output}".EvaluateCodeForTests(_engine);

            Assert.Equal("42", asset);
        }

        [Fact]
        public async void GetsJSONAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _getAsset = new GetAssetCommand();

            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _getAsset.v_AssetName = "testJSONAsset";
            _getAsset.v_AssetType = "JSON";
            _getAsset.v_OutputDirectoryPath = "";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            string jsonString = (string)await "{output}".EvaluateCodeForTests(_engine);
            JObject jsonObject = JObject.Parse(jsonString);
            Assert.Equal("testText", jsonObject["text"]);
        }

        [Fact]
        public void GetsFileAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _getAsset = new GetAssetCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filepath = Path.Combine(projectDirectory, @"Resources\");

            _getAsset.v_AssetName = "testFileAsset";
            _getAsset.v_AssetType = "File";
            _getAsset.v_OutputDirectoryPath = filepath;
            _getAsset.v_OutputUserVariableName = "";

            _getAsset.RunCommand(_engine);

            Assert.True(File.Exists(filepath + "test.txt"));

            File.Delete(filepath + "test.txt");
        }

        [Fact]
        public async System.Threading.Tasks.Task HandlesNonexistentAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _getAsset = new GetAssetCommand();

            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _getAsset.v_AssetName = "noAsset";
            _getAsset.v_AssetType = "Text";
            _getAsset.v_OutputDirectoryPath = "";
            _getAsset.v_OutputUserVariableName = "{output}";

            await Assert.ThrowsAsync<DataException>(() => _getAsset.RunCommand(_engine));
        }
    }
}