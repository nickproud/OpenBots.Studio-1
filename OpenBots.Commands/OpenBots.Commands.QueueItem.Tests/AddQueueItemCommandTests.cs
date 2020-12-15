using Newtonsoft.Json.Linq;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using Xunit;

namespace OpenBots.Commands.QueueItem.Tests
{
    public class AddQueueItemCommandTests
    {
        private AutomationEngineInstance _engine;
        private AddQueueItemCommand _addQueueItem;
        private WorkQueueItemCommand _workQueueItem;

        [Fact]
        public void AddQueueItemTextNoAttachments()
        {
            _engine = new AutomationEngineInstance(null);
            _addQueueItem = new AddQueueItemCommand();
            _workQueueItem = new WorkQueueItemCommand();

            //Add queue item
            _addQueueItem.v_QueueName = "UnitTestQueue";
            _addQueueItem.v_QueueItemName = "Queue Item Test";
            _addQueueItem.v_QueueItemType = "Text";
            _addQueueItem.v_JsonType = "Test Type";
            _addQueueItem.v_QueueItemTextValue = "Test Text";
            _addQueueItem.v_Priority = "10";

            _addQueueItem.RunCommand(_engine);

            //Get queue item (dequeue)
            _workQueueItem.v_QueueName = "UnitTestQueue";
            _workQueueItem.v_OutputUserVariableName = "{output}";
            _workQueueItem.v_SaveAttachments = "No";
            _workQueueItem.v_AttachmentDirectory = "";

            _workQueueItem.RunCommand(_engine);

            var queueItem = "{output}".ConvertUserVariableToString(_engine);
            JObject jsonObject = JObject.Parse(queueItem);

            //Check if values are the same (dataJson)
            Assert.Equal("Test Text", jsonObject["DataJson"]);
        }

        //AddQueueItemTextOneAttachment


        //AddQueueItemTextMultipleAttachments


        //AddQueueItemJsonNoAttachments


        //??AddQueueItemJsonOneAttachment

        
        //??AddQueueItemJsonMultipleAttachments


        //HandlesNonExistentQueue

    }
}
