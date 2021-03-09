using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using Xunit;

namespace OpenBots.Commands.Engine.Test
{
    public class ShowEngineContextCommandTests
    {
        private AutomationEngineInstance _engine;
        private ShowEngineContextCommand _showEngineContextCommand;
        
        [Fact]
        public void ShowsEngineContext()
        {
            _engine = new AutomationEngineInstance(null);
            _showEngineContextCommand = new ShowEngineContextCommand();


            string closeAfterSec = "2";
            VariableMethods.CreateTestVariable(closeAfterSec, _engine, "closeAfterSec", typeof(string));

            _showEngineContextCommand.v_AutoCloseAfter = "{closeAfterSec}";

            _showEngineContextCommand.RunCommand(_engine);
        }
    }
}
