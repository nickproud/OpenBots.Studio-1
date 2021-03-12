using Newtonsoft.Json;
using OpenBots.Core.Server.API_Methods;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Xunit;
using QueueItemModel = OpenBots.Core.Server.Models.QueueItem;

namespace OpenBots.Commands.QueueItem.Test
{
    public class WorkQueueItemCommandTests
    {
        private AutomationEngineInstance _engine;
        private AddQueueItemCommand _addQueueItem;
        private WorkQueueItemCommand _workQueueItem;

        [Fact]
        public void WorkQueueItemNoAttachments()
        {
            _engine = new AutomationEngineInstance(null);
            _addQueueItem = new AddQueueItemCommand();
            _workQueueItem = new WorkQueueItemCommand();
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(Dictionary<,>));

            _addQueueItem.v_QueueName = "UnitTestQueue";
            _addQueueItem.v_QueueItemName = "WorkQueueItemNoAttachmentTest";
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

            var queueItemObject = (Dictionary<string, object>)"{output}".ConvertUserVariableToObject(_engine, typeof(Dictionary<,>));
            var client = AuthMethods.GetAuthToken();
            var queueItem = QueueItemMethods.GetQueueItemByLockTransactionKey(client, queueItemObject["LockTransactionKey"].ToString());

            Assert.Equal("InProgress", queueItem.State);
        }

        [Fact]
        public void WorkQueueItemOneAttachment()
        {
            _engine = new AutomationEngineInstance(null);
            _addQueueItem = new AddQueueItemCommand();
            _workQueueItem = new WorkQueueItemCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");
            string fileName = "testFile.txt";
            string attachment = Path.Combine(filePath, @"Download\", fileName);
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(Dictionary<,>));

            _addQueueItem.v_QueueName = "UnitTestQueue";
            _addQueueItem.v_QueueItemName = "WorkQueueItemAttachmentTest";
            _addQueueItem.v_QueueItemType = "Text";
            _addQueueItem.v_JsonType = "Test Type";
            _addQueueItem.v_QueueItemTextValue = "Test Text";
            _addQueueItem.v_Priority = "10";
            _addQueueItem.v_Attachments = Path.Combine(filePath, @"Upload\", fileName);

            _addQueueItem.RunCommand(_engine);

            _workQueueItem.v_QueueName = "UnitTestQueue";
            _workQueueItem.v_OutputUserVariableName = "{output}";
            _workQueueItem.v_SaveAttachments = "Yes";
            _workQueueItem.v_AttachmentDirectory = filePath + @"Download\";

            _workQueueItem.RunCommand(_engine);

            var queueItemObject = "{output}".ConvertUserVariableToObject(_engine, typeof(Dictionary<,>));
            string queueItemString = JsonConvert.SerializeObject(queueItemObject);
            var vQueueItem = JsonConvert.DeserializeObject<QueueItemModel>(queueItemString);

            var client = AuthMethods.GetAuthToken();
            var queueItem = QueueItemMethods.GetQueueItemByLockTransactionKey(client, vQueueItem.LockTransactionKey.ToString());

            Assert.Equal("InProgress", queueItem.State);
            Assert.True(File.Exists(attachment));

            File.Delete(attachment);
        }

        [Fact]
        public void WorkQueueItemMultipleAttachments()
        {
            _engine = new AutomationEngineInstance(null);
            _addQueueItem = new AddQueueItemCommand();
            _workQueueItem = new WorkQueueItemCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources\");
            string fileName1 = "testFile.txt";
            string fileName2 = "testFile2.txt";
            string attachment1 = Path.Combine(filePath, @"Download\", fileName1);
            string attachment2 = Path.Combine(filePath, @"Download\", fileName2);
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(Dictionary<,>));

            _addQueueItem.v_QueueName = "UnitTestQueue";
            _addQueueItem.v_QueueItemName = "WorkQueueItemAttachmentsTest";
            _addQueueItem.v_QueueItemType = "Text";
            _addQueueItem.v_JsonType = "Test Type";
            _addQueueItem.v_QueueItemTextValue = "Test Text";
            _addQueueItem.v_Priority = "10";
            _addQueueItem.v_Attachments = filePath + @"Upload\" + fileName1
                + ";" + filePath + @"Upload\" + fileName2;

            _addQueueItem.RunCommand(_engine);

            _workQueueItem.v_QueueName = "UnitTestQueue";
            _workQueueItem.v_OutputUserVariableName = "{output}";
            _workQueueItem.v_SaveAttachments = "Yes";
            _workQueueItem.v_AttachmentDirectory = filePath + @"Download\";

            _workQueueItem.RunCommand(_engine);

            var queueItemObject = "{output}".ConvertUserVariableToObject(_engine, typeof(Dictionary<,>));
            string queueItemString = JsonConvert.SerializeObject(queueItemObject);
            var vQueueItem = JsonConvert.DeserializeObject<QueueItemModel>(queueItemString);

            var client = AuthMethods.GetAuthToken();
            var queueItem = QueueItemMethods.GetQueueItemByLockTransactionKey(client, vQueueItem.LockTransactionKey.ToString());

            Assert.Equal("InProgress", queueItem.State);
            Assert.True(File.Exists(attachment1));
            Assert.True(File.Exists(attachment2));

            File.Delete(attachment1);
            File.Delete(attachment2);
        }

        [Fact]
        public void HandlesNonExistentQueue()
        {
            _engine = new AutomationEngineInstance(null);
            _addQueueItem = new AddQueueItemCommand();
            _workQueueItem = new WorkQueueItemCommand();
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(Dictionary<,>));

            _addQueueItem.v_QueueName = "UnitTestQueue";
            _addQueueItem.v_QueueItemName = "WorkQueueItemJsonTest";
            _addQueueItem.v_QueueItemType = "Json";
            _addQueueItem.v_JsonType = "Test Type";
            _addQueueItem.v_QueueItemTextValue = "{'text':'testText'}";
            _addQueueItem.v_Priority = "10";

            _addQueueItem.RunCommand(_engine);

            _workQueueItem.v_QueueName = "NoQueue";
            _workQueueItem.v_OutputUserVariableName = "{output}";
            _workQueueItem.v_SaveAttachments = "No";
            _workQueueItem.v_AttachmentDirectory = "";

            Assert.Throws<DataException>(() => _workQueueItem.RunCommand(_engine));
        }
    }
}
