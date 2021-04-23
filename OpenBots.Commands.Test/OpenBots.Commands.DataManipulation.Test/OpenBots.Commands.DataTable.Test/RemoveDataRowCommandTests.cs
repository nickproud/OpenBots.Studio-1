using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Data;
using Xunit;
using Xunit.Abstractions;
using OBData = System.Data;

namespace OpenBots.Commands.DataTable.Test
{
    public class RemoveDataRowCommandTests
    {
        private RemoveDataRowCommand _removeDataRow;
        private AutomationEngineInstance _engine;
        private readonly ITestOutputHelper output;

        public RemoveDataRowCommandTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("(firstname,john)","And", 0, "jane")]
        [InlineData("(lastname,smith)","Or",0,"jane")]
        public async void removesDataRow(string search, string andOr, int expectedIndex, string expectedName)
        {
            _removeDataRow = new RemoveDataRowCommand();
            _engine = new AutomationEngineInstance(null);

            OBData.DataTable inputTable = new OBData.DataTable();
            inputTable.Columns.Add("firstname");
            inputTable.Columns.Add("lastname");
            DataRow row1 = inputTable.NewRow();
            row1["firstname"] = "john";
            row1["lastname"] = "smith";
            inputTable.Rows.Add(row1);
            DataRow row2 = inputTable.NewRow();
            row2["firstname"] = "jane";
            row2["lastname"] = "smith";
            inputTable.Rows.Add(row2);
            DataRow row3 = inputTable.NewRow();
            row3["firstname"] = "jane";
            row3["lastname"] = "doe";
            inputTable.Rows.Add(row3);

            VariableMethods.CreateTestVariable(inputTable, _engine, "inputTable", typeof(OBData.DataTable));

            _removeDataRow.v_DataTable = "{inputTable}";
            //_removeDataRow.v_SearchItem = search;
            _removeDataRow.v_AndOr = andOr;

            _removeDataRow.RunCommand(_engine);
            
            OBData.DataTable outputTable = (OBData.DataTable)await _removeDataRow.v_DataTable.EvaluateCode(_engine);
            Assert.True(outputTable.Rows[expectedIndex][0].Equals(expectedName));
        }
    }
}
