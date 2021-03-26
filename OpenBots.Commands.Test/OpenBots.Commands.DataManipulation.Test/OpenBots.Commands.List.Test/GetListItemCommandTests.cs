using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Collections.Generic;
using Xunit;
using System.Data;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.List.Test
{
    public class GetListItemCommandTests
    {
        private AutomationEngineInstance _engine;
        private GetListItemCommand _getListItem;

        [Fact]
        public void GetsStringListItem()
        {
            _engine = new AutomationEngineInstance(null);
            _getListItem = new GetListItemCommand();

            List<string> list = new List<string>();
            list.Add("item1");
            list.Add("item2");
            VariableMethods.CreateTestVariable(list, _engine, "inputList", typeof(List<>));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _getListItem.v_ListName = "{inputList}";
            _getListItem.v_ItemIndex = "1";
            _getListItem.v_OutputUserVariableName = "{output}";

            _getListItem.RunCommand(_engine);

            Assert.Equal("item2", "{output}".ConvertUserVariableToString(_engine));
        }

        [Fact]
        public void GetsDataTableListItem()
        {
            _engine = new AutomationEngineInstance(null);
            _getListItem = new GetListItemCommand();

            List<OBDataTable> list = new List<OBDataTable>();
            OBDataTable item1 = new OBDataTable();
            item1.Columns.Add("d1col");
            OBDataTable item2 = new OBDataTable();
            item2.Columns.Add("d2col");
            list.Add(item1);
            list.Add(item2);
            VariableMethods.CreateTestVariable(list, _engine, "inputList", typeof(List<>));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(OBDataTable));

            _getListItem.v_ListName = "{inputList}";
            _getListItem.v_ItemIndex = "1";
            _getListItem.v_OutputUserVariableName = "{output}";

            _getListItem.RunCommand(_engine);

            Assert.Equal(item2, (OBDataTable)"{output}".ConvertUserVariableToObject(_engine, typeof(OBDataTable)));
        }
    }
}
