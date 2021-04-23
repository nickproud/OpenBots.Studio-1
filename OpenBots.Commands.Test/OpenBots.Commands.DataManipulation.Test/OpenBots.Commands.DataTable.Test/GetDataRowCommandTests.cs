using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Data;
using Xunit;
using OBData = System.Data;

namespace OpenBots.Commands.DataTable.Test
{
    public class GetDataRowCommandTests
    {

        private GetDataRowCommand _getDataRow;
        private AutomationEngineInstance _engine;

        [Fact]
        public async void getsDataRow()
        {
            _getDataRow = new GetDataRowCommand();
            _engine = new AutomationEngineInstance(null);

            OBData.DataTable inputTable = new OBData.DataTable();
            DataColumn column1 = new DataColumn();
            column1.ColumnName = "col1";
            DataColumn column2 = new DataColumn();
            column2.ColumnName = "col2";
            inputTable.Columns.Add(column1);
            inputTable.Columns.Add(column2);
            DataRow row = inputTable.NewRow();
            row["col1"] = "c1r1";
            row["col2"] = "c2r1";
            inputTable.Rows.Add(row);

            VariableMethods.CreateTestVariable(inputTable, _engine, "inputTable", typeof(OBData.DataTable));
            VariableMethods.CreateTestVariable(null, _engine, "outputRow", typeof(OBData.DataRow));
            
            _getDataRow.v_DataTable = "{inputTable}";
            _getDataRow.v_DataRowIndex = "0";
            _getDataRow.v_OutputUserVariableName = "{outputRow}";

            _getDataRow.RunCommand(_engine);

            Assert.Equal(inputTable.Rows[0], (DataRow)await _getDataRow.v_OutputUserVariableName.EvaluateCode(_engine));
        }
    }
}
