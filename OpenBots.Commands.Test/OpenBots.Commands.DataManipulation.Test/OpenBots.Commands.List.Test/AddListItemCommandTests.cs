using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;
using Xunit.Abstractions;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.List.Test
{
    public class AddListItemCommandTests
    {
        private AutomationEngineInstance _engine;
        private AddListItemCommand _addListItem;
        private readonly ITestOutputHelper output;

        public AddListItemCommandTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void AddsStringListItem()
        {
            _engine = new AutomationEngineInstance(null);
            _addListItem = new AddListItemCommand();

            List<string> stringList = new List<string>();
            string itemToAdd = "item1";

            VariableMethods.CreateTestVariable(stringList, _engine, "list", typeof(List<>));
            VariableMethods.CreateTestVariable(itemToAdd, _engine, "itemToAdd", typeof(string));

            _addListItem.v_ListName = "{list}";
            _addListItem.v_ListItem = "{itemToAdd}";

            _addListItem.RunCommand(_engine);

            List<string> outputList = (List<string>)"{list}".ConvertUserVariableToObject(_engine, typeof(List<>));
            Assert.Equal(itemToAdd, outputList[0]);
        }

        [Fact]
        public void AddsDataTableListItem()
        {
            _engine = new AutomationEngineInstance(null);
            _addListItem = new AddListItemCommand();

            List<OBDataTable> stringList = new List<OBDataTable>();
            OBDataTable itemToAdd = new OBDataTable();
            itemToAdd.Columns.Add("first column");

            VariableMethods.CreateTestVariable(stringList, _engine, "list", typeof(List<>));
            VariableMethods.CreateTestVariable(itemToAdd, _engine, "itemToAdd", typeof(OBDataTable));

            _addListItem.v_ListName = "{list}";
            _addListItem.v_ListItem = "{itemToAdd}";

            _addListItem.RunCommand(_engine);

            List<OBDataTable> outputList = (List<OBDataTable>)"{list}".ConvertUserVariableToObject(_engine, typeof(List<>));
            Assert.Equal(itemToAdd, outputList[0]);
        }

        [Fact]
        public void HandlesInvalidListItem()
        {
            _engine = new AutomationEngineInstance(null);
            _addListItem = new AddListItemCommand();

            List<OBDataTable> stringList = new List<OBDataTable>();
            string itemToAdd = "newitem";

            VariableMethods.CreateTestVariable(stringList, _engine, "list", typeof(List<>));
            VariableMethods.CreateTestVariable(itemToAdd, _engine, "itemToAdd", typeof(string));

            _addListItem.v_ListName = "{list}";
            _addListItem.v_ListItem = "{itemToAdd}";

            Assert.Throws<Exception>(() => _addListItem.RunCommand(_engine));
        }
    }
}
