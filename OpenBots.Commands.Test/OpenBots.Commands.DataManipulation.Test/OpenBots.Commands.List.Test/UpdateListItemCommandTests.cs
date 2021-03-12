using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.List.Test
{
    public class UpdateListItemCommandTests
    {
        private AutomationEngineInstance _engine;
        private UpdateListItemCommand _updateListItem;

        [Fact]
        public void UpdatesStringListItem()
        {
            _engine = new AutomationEngineInstance(null);
            _updateListItem = new UpdateListItemCommand();

            List<string> inputList = new List<string>();
            inputList.Add("item1");
            inputList.Add("item2");
            string index = "0";
            string item = "item3";

            VariableMethods.CreateTestVariable(inputList, _engine, "inputList", typeof(List<>));
            VariableMethods.CreateTestVariable(index, _engine, "index", typeof(string));
            VariableMethods.CreateTestVariable(item, _engine, "item", typeof(string));

            _updateListItem.v_ListName = "{inputList}";
            _updateListItem.v_ListIndex = "{index}";
            _updateListItem.v_ListItem = "{item}";

            _updateListItem.RunCommand(_engine);

            List<string> outputList = (List<string>)"{inputList}".ConvertUserVariableToObject(_engine, typeof(List<>));
            Assert.Equal("item3", outputList[0]);
        }

        [Fact]
        public void UpdatesDataTableListItem()
        {
            _engine = new AutomationEngineInstance(null);
            _updateListItem = new UpdateListItemCommand();

            List<OBDataTable> inputList = new List<OBDataTable>();
            OBDataTable item1 = new OBDataTable();
            item1.Columns.Add("d1col");
            OBDataTable item2 = new OBDataTable();
            item2.Columns.Add("d2col");
            inputList.Add(item1);
            inputList.Add(item2);
            string index = "0";
            OBDataTable newitem = new OBDataTable();
            newitem.Columns.Add("d3col");

            VariableMethods.CreateTestVariable(inputList, _engine, "inputList", typeof(List<>));
            VariableMethods.CreateTestVariable(index, _engine, "index", typeof(string));
            VariableMethods.CreateTestVariable(newitem, _engine, "newitem", typeof(OBDataTable));

            _updateListItem.v_ListName = "{inputList}";
            _updateListItem.v_ListIndex = "{index}";
            _updateListItem.v_ListItem = "{newitem}";

            _updateListItem.RunCommand(_engine);

            List<OBDataTable> outputList = (List<OBDataTable>)"{inputList}".ConvertUserVariableToObject(_engine, typeof(List<>));
            Assert.Equal(newitem, outputList[0]);
        }

        [Fact]
        public void HandlesInvalidListItem()
        {
            _engine = new AutomationEngineInstance(null);
            _updateListItem = new UpdateListItemCommand();

            List<OBDataTable> inputList = new List<OBDataTable>();
            OBDataTable item1 = new OBDataTable();
            string newItem = "item2";
            inputList.Add(item1);
            string index = "0";

            VariableMethods.CreateTestVariable(inputList, _engine, "inputList", typeof(List<>));
            VariableMethods.CreateTestVariable(index, _engine, "index", typeof(string));
            VariableMethods.CreateTestVariable(newItem, _engine, "item", typeof(string));

            _updateListItem.v_ListName = "{inputList}";
            _updateListItem.v_ListIndex = "{index}";
            _updateListItem.v_ListItem = "{item}";

            Assert.Throws<Exception>(() => _updateListItem.RunCommand(_engine));
        }
    }
}
