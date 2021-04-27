using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Collections.Generic;
using System.Data;
using Xunit;
using OBData = System.Data;

namespace OpenBots.Commands.Dictionary.Test
{
    public class AddDictionaryItemCommandTests
    {
        private AddDictionaryItemCommand _addDictionaryItem;
        private AutomationEngineInstance _engine;

        [Fact]
        public async void AddsDictionaryItem()
        {
            _addDictionaryItem = new AddDictionaryItemCommand();
            _engine = new AutomationEngineInstance(null);

            Dictionary<string, string> inputDict = new Dictionary<string, string>();
            VariableMethods.CreateTestVariable(inputDict, _engine, "inputDict", typeof(Dictionary<,>));

            OBData.DataTable inputTable = new OBData.DataTable();
            inputTable.Columns.Add("Keys");
            inputTable.Columns.Add("Values");
            DataRow row1 = inputTable.NewRow();
            row1["Keys"] = "key1";
            row1["Values"] = "val1";
            inputTable.Rows.Add(row1);
            VariableMethods.CreateTestVariable(inputTable, _engine, "inputTable", typeof(OBData.DataTable));

            _addDictionaryItem.v_DictionaryName = "{inputDict}";
            _addDictionaryItem.v_ColumnNameDataTable = (OBData.DataTable)await "{inputTable}".EvaluateCode(_engine);

            _addDictionaryItem.RunCommand(_engine);

            Assert.Equal("val1", inputDict["key1"]);

        }
    }
}
