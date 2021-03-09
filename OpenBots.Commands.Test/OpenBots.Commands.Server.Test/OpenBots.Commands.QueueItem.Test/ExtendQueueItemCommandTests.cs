using OpenBots.Core.Server.API_Methods;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using Xunit;

namespace OpenBots.Commands.QueueItem.Test
{
    public class ExtendQueueItemCommandTests
    {
        private AutomationEngineInstance _engine;
        private AddQueueItemCommand _addQueueItem;
        private WorkQueueItemCommand _workQueueItem;
        private ExtendQueueItemCommand _extendQueueItem;

        [Fact]
        public void ExtendQueueItem()
        {
            _engine = new AutomationEngineInstance(null);
            _addQueueItem = new AddQueueItemCommand();
            _workQueueItem = new WorkQueueItemCommand();
            _extendQueueItem = new ExtendQueueItemCommand();

            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(Dictionary<,>));
            VariableMethods.CreateTestVariable(null, _engine, "vQueueItem", typeof(Dictionary<,>));

            _addQueueItem.v_QueueName = "UnitTestQueue";
            _addQueueItem.v_QueueItemName = "ExtendQueueItemTest";
            _addQueueItem.v_QueueItemType = "Text";
            _addQueueItem.v_JsonType = "Test Type";
            _addQueueItem.v_QueueItemTextValue = "Test Text";
            _addQueueItem.v_Priority = "10";

            _addQueueItem.RunCommand(_engine);

            _workQueueItem.v_QueueName = "UnitTestQueue";
            _workQueueItem.v_OutputUserVariableName = "{output}";
            _workQueueItem.v_SaveAttachments = "No";
            _workQueueItem.v_AttachmentDirectory = "";

            _workQueueItem.RunCommand(_engine);

            var queueItemDict = (Dictionary<string, object>)"{output}".ConvertUserVariableToObject(_engine, typeof(Dictionary<,>));
            var transactionKey = queueItemDict["LockTransactionKey"].ToString();
            var client = AuthMethods.GetAuthToken();
            var queueItem = QueueItemMethods.GetQueueItemByLockTransactionKey(client, transactionKey);

            _extendQueueItem.v_QueueItem = "{vQueueItem}";
            queueItemDict.StoreInUserVariable(_engine, _extendQueueItem.v_QueueItem, typeof(Dictionary<,>));

            _extendQueueItem.RunCommand(_engine);

            var extendedQueueItem = QueueItemMethods.GetQueueItemByLockTransactionKey(client, transactionKey);

            Assert.True(queueItem.LockedUntilUTC < extendedQueueItem.LockedUntilUTC);
        }

        [Fact]
        public void HandlesNonExistentTransactionKey()
        {
            _engine = new AutomationEngineInstance(null);
            _extendQueueItem = new ExtendQueueItemCommand();

            var queueItemDict = new Dictionary<string, object>()
            {
                {  "LockTransactionKey", null },
                { "Name", "ExtendQueueItemTest" },
                { "Source", null },
                { "Event", null },
                { "Type", "Text" },
                { "JsonType", "Test Type" },
                { "DataJson", "Test Text" },
                { "Priority", 10 },
                { "LockedUntilUTC", DateTime.UtcNow.AddHours(1) }
            };

            VariableMethods.CreateTestVariable(null, _engine, "vQueueItem", typeof(Dictionary<,>));
            _extendQueueItem.v_QueueItem = "{vQueueItem}";
            queueItemDict.StoreInUserVariable(_engine, _extendQueueItem.v_QueueItem, typeof(Dictionary<,>));

            Assert.Throws<NullReferenceException>(() => _extendQueueItem.RunCommand(_engine));
        }
    }
}
