using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Collections.Generic;
using System.Data;
using Xunit;
using OBData = System.Data;

namespace OpenBots.Commands.Dictionary.Test
{
    public class CreateDictionaryCommandTests
    {
        private CreateDictionaryCommand _createDictionary;
        private AutomationEngineInstance _engine;

        [Fact]
        public void CreatesDictionary()
        {
            _createDictionary = new CreateDictionaryCommand();
            _engine = new AutomationEngineInstance(null);
            OBData.DataTable inputDt = new OBData.DataTable();
            inputDt.Columns.Add("Keys");
            inputDt.Columns.Add("Values");
            DataRow row1 = inputDt.NewRow();
            row1["Keys"] = "key1";
            row1["Values"] = "val1";
            inputDt.Rows.Add(row1);
            VariableMethods.CreateTestVariable(inputDt, _engine, "inputDt", typeof(OBData.DataTable));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(Dictionary<,>));

            _createDictionary.v_ColumnNameDataTable = (OBData.DataTable)"{inputDt}".ConvertUserVariableToObject(_engine, typeof(OBData.DataTable));
            _createDictionary.v_OutputUserVariableName = "{output}";

            _createDictionary.RunCommand(_engine);

            Dictionary<string, string> outDict = (Dictionary<string, string>)"{output}".ConvertUserVariableToObject(_engine, typeof(Dictionary<,>));

            Assert.True(outDict.ContainsKey("key1"));
            Assert.Equal("val1", outDict["key1"]);
        }
    }
}
