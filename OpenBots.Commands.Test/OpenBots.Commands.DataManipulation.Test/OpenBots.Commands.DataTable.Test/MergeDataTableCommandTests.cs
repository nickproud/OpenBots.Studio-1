using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Xunit;
using OBData = System.Data;

namespace OpenBots.Commands.DataTable.Test
{

    public class MergeDataTableCommandTests
    {
        private MergeDataTableCommand _mergeDataTable;
        private AutomationEngineInstance _engine;
        
        [Theory]
        [ClassData(typeof(NullTestData))]
        public void HandlesNullDataTables(OBData.DataTable dt1, OBData.DataTable dt2)
        {
            _engine = new AutomationEngineInstance(null);
            _mergeDataTable = new MergeDataTableCommand();

            VariableMethods.CreateTestVariable(dt1, _engine, "dt1", typeof(OBData.DataTable));
            VariableMethods.CreateTestVariable(dt2, _engine, "dt2", typeof(OBData.DataTable));

            _mergeDataTable.v_SourceDataTable = "{dt1}";
            _mergeDataTable.v_DestinationDataTable = "{dt2}";
            _mergeDataTable.v_MissingSchemaAction = "Add";

            Assert.Throws<ArgumentNullException>(() => _mergeDataTable.RunCommand(_engine));
        }

        [Theory]
        [ClassData(typeof(IncorrectTypeTestData))]
        public void HandlesIncorrectTypeInput(object dt1, object dt2)
        {
            _engine = new AutomationEngineInstance(null);
            _mergeDataTable = new MergeDataTableCommand();

            VariableMethods.CreateTestVariable(dt1, _engine, "dt1", typeof(OBData.DataTable));
            VariableMethods.CreateTestVariable(dt2, _engine, "dt2", typeof(int));

            _mergeDataTable.v_SourceDataTable = "{dt1}";
            _mergeDataTable.v_DestinationDataTable = "{dt2}";
            _mergeDataTable.v_MissingSchemaAction = "Add";

            Assert.Throws<ArgumentException>(() => _mergeDataTable.RunCommand(_engine));
        }

        [Theory]
        [ClassData(typeof(TableTestData))]
        public void TableDataIsEqual(OBData.DataTable dt1, OBData.DataTable dt2, string schema)
        {
            _engine = new AutomationEngineInstance(null);
            _mergeDataTable = new MergeDataTableCommand();

            VariableMethods.CreateTestVariable(dt1, _engine, "dt1", typeof(OBData.DataTable));
            VariableMethods.CreateTestVariable(dt2, _engine, "dt2", typeof(OBData.DataTable));

            switch (schema)
            {
                case "Add":
                    dt2.Merge(dt1, false, MissingSchemaAction.Add);
                    break;
                case "AddWithKey":
                    dt2.Merge(dt1, false, MissingSchemaAction.AddWithKey);
                    break;
                case "Error":
                    dt2.Merge(dt1, false, MissingSchemaAction.Error);
                    break;
                case "Ignore":
                    dt2.Merge(dt1, false, MissingSchemaAction.Ignore);
                    break;
                default:
                    throw new NotImplementedException("Test for schema '" + schema + "' not implemented");
            }

            _mergeDataTable.v_SourceDataTable = "{dt1}";
            _mergeDataTable.v_DestinationDataTable = "{dt2}";
            _mergeDataTable.v_MissingSchemaAction = schema;

            _mergeDataTable.RunCommand(_engine);

            OBData.DataTable resultDataTable = (OBData.DataTable)_mergeDataTable.v_DestinationDataTable.ConvertUserVariableToObject(_engine, typeof(OBData.DataTable));

            Assert.Equal(dt2.GetType(), resultDataTable.GetType());
            // Check each row / column pair and assert equivalence
            for(int row = 0; row < dt2.Rows.Count; row++)
            {
                for(int col = 0;col < dt2.Columns.Count; col++)
                {
                    Assert.Equal(dt2.Rows[row][dt2.Columns[col]], resultDataTable.Rows[row][resultDataTable.Columns[col]]);
                }
            }
        }
    }

    public class NullTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            OBData.DataTable dt1 = new OBData.DataTable();
            DataColumn column1 = new DataColumn();
            column1.ColumnName = "col1";
            DataColumn column2 = new DataColumn();
            column2.ColumnName = "col2";
            dt1.Columns.Add(column1);
            dt1.Columns.Add(column2);
            DataRow row = dt1.NewRow();
            row["col1"] = "c1r1";
            row["col2"] = "c2r1";
            dt1.Rows.Add(row);
            OBData.DataTable dt2 = null;
            yield return new object[] { dt1, dt2 };
            yield return new object[] { dt2, dt1 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class IncorrectTypeTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            OBData.DataTable dt1 = new OBData.DataTable();
            DataColumn column1 = new DataColumn();
            column1.ColumnName = "col1";
            DataColumn column2 = new DataColumn();
            column2.ColumnName = "col2";
            dt1.Columns.Add(column1);
            dt1.Columns.Add(column2);
            DataRow row = dt1.NewRow();
            row["col1"] = "c1r1";
            row["col2"] = "c2r1";
            dt1.Rows.Add(row);
            int dt2 = 1;
            yield return new object[] { dt1, dt2 };
            yield return new object[] { dt2, dt1 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public class TableTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            OBData.DataTable dt1 = new OBData.DataTable();
            DataColumn column1 = new DataColumn();
            column1.ColumnName = "col1";
            DataColumn column2 = new DataColumn();
            column2.ColumnName = "col2";
            dt1.Columns.Add(column1);
            dt1.Columns.Add(column2);
            DataRow row = dt1.NewRow();
            row["col1"] = "c1r1";
            row["col2"] = "c2r1";
            dt1.Rows.Add(row);
            OBData.DataTable dt2 = new OBData.DataTable();
            column1 = new DataColumn();
            column1.ColumnName = "col1";
            column2 = new DataColumn();
            column2.ColumnName = "col2";
            dt2.Columns.Add(column1);
            dt2.Columns.Add(column2);
            row = dt2.NewRow();
            row["col1"] = "c1r1";
            row["col2"] = "c2r1";
            dt2.Rows.Add(row);
            string schema = "Add";
            yield return new object[] { dt1, dt2, schema };
            yield return new object[] { dt2, dt1, schema };
            row = dt1.NewRow();
            row["col1"] = 1;
            row["col2"] = null;
            dt1.Rows.Add(row);
            yield return new object[] { dt1, dt2, schema };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
