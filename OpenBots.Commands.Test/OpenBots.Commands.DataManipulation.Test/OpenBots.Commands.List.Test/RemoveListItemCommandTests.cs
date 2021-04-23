using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Collections.Generic;
using Xunit;
using System.Data;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.List.Test
{
    public class RemoveListItemCommandTests
    {
        private AutomationEngineInstance _engine;
        private RemoveListItemCommand _removeListItem;

        [Fact]
        public async void RemovesStringListItem()
        {
            _engine = new AutomationEngineInstance(null);
            _removeListItem = new RemoveListItemCommand();

            List<string> inputList = new List<string>();
            inputList.Add("item1");
            inputList.Add("item2");
            string index = "0";

            VariableMethods.CreateTestVariable(inputList, _engine, "inputList", typeof(List<>));
            VariableMethods.CreateTestVariable(index, _engine, "index", typeof(string));

            _removeListItem.v_ListName = "{inputList}";
            _removeListItem.v_ListIndex = "{index}";

            _removeListItem.RunCommand(_engine);
            List<string> outputList = (List<string>)await "{inputList}".EvaluateCode(_engine);
            Assert.Equal("item2", outputList[0]);
        }

        [Fact]
        public async void RemovesDataTableListItem()
        {
            _engine = new AutomationEngineInstance(null);
            _removeListItem = new RemoveListItemCommand();

            List<OBDataTable> inputList = new List<OBDataTable>();
            OBDataTable item1 = new OBDataTable();
            item1.Columns.Add("d1col");
            OBDataTable item2 = new OBDataTable();
            item2.Columns.Add("d2col");
            inputList.Add(item1);
            inputList.Add(item2);
            string index = "0";

            VariableMethods.CreateTestVariable(inputList, _engine, "inputList", typeof(List<>));
            VariableMethods.CreateTestVariable(index, _engine, "index", typeof(string));

            _removeListItem.v_ListName = "{inputList}";
            _removeListItem.v_ListIndex = "{index}";

            _removeListItem.RunCommand(_engine);
            List<OBDataTable> outputList = (List<OBDataTable>)await "{inputList}".EvaluateCode(_engine);
            Assert.Equal(item2, outputList[0]);
        }
    }
}
