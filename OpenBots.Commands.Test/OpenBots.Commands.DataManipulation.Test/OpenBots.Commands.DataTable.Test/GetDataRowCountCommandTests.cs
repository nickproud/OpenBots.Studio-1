using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Data;
using Xunit;
using OBData = System.Data;

namespace OpenBots.Commands.DataTable.Test
{
    public class GetDataRowCountCommandTests
    {
        private GetDataRowCountCommand _getDataRowCount;
        private AutomationEngineInstance _engine;

        [Fact]
        public void getsDataRowCount()
        {
            _getDataRowCount = new GetDataRowCountCommand();
            _engine = new AutomationEngineInstance(null);

            // Set up existing data table for the command
            OBData.DataTable inputTable = new OBData.DataTable();
            inputTable.Columns.Add("Column1");
            DataRow row1 = inputTable.NewRow();
            row1["Column1"] = "data1";
            inputTable.Rows.Add(row1);
            VariableMethods.CreateTestVariable(inputTable, _engine, "inputTable", typeof(OBData.DataTable));
            VariableMethods.CreateTestVariable(null, _engine, "outputCount", typeof(int));

            _getDataRowCount.v_DataTable = "{inputTable}";
            _getDataRowCount.v_OutputUserVariableName = "{outputCount}";

            _getDataRowCount.RunCommand(_engine);

            Assert.Equal("1", (string)_getDataRowCount.v_OutputUserVariableName.ConvertUserVariableToString(_engine));
        }
    }
}
