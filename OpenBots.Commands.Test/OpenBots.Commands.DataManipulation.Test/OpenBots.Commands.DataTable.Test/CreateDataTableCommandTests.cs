using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Data;
using Xunit;
using OBData = System.Data;

namespace OpenBots.Commands.DataTable.Test
{
    public class CreateDataTableCommandTests
    {
        private CreateDataTableCommand _createDataTableCommand;
        private AutomationEngineInstance _engine;

        [Fact]
        public void CreatesDataTable()
        {
            _createDataTableCommand = new CreateDataTableCommand();
            _engine = new AutomationEngineInstance(null);

            OBData.DataTable columnNameDataTable = new OBData.DataTable
            {
                TableName = "ColumnNamesDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            };
            VariableMethods.CreateTestVariable("Col1", _engine, "Col1", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "outputTable", typeof(OBData.DataTable));
            columnNameDataTable.Columns.Add("{Col1}");

            _createDataTableCommand.v_ColumnNameDataTable = columnNameDataTable;
            _createDataTableCommand.v_OutputUserVariableName = "{outputTable}";

            _createDataTableCommand.RunCommand(_engine);

            OBData.DataTable expectedDt = new OBData.DataTable();
            foreach (DataRow rwColumnName in columnNameDataTable.Rows)
            {
                expectedDt.Columns.Add(rwColumnName.Field<string>("Column Name").ConvertUserVariableToString(_engine));
            }

            OBData.DataTable resultDataTable = (OBData.DataTable)_createDataTableCommand.v_OutputUserVariableName.ConvertUserVariableToObject(_engine, typeof(OBData.DataTable));

            for (int row = 0; row < expectedDt.Rows.Count; row++)
            {
                for (int col = 0; col < expectedDt.Columns.Count; col++)
                {
                    Assert.Equal(expectedDt.Rows[row][expectedDt.Columns[col]], resultDataTable.Rows[row][resultDataTable.Columns[col]]);
                }
            }
        }
    }
}
