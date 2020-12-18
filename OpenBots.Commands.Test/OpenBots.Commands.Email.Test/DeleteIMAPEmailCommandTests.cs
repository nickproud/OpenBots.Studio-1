using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OpenBots.Engine;
using OpenBots.Core.Utilities.CommonUtilities;

namespace OpenBots.Commands.Email.Test
{
    public class DeleteIMAPEmailCommandTests
    {
        private AutomationEngineInstance _engine;
        private DeleteIMAPEmailCommand _deleteIMAPEmailCommand;

        [Fact]
        public void DeletesIMAPEmail()
        {
            _engine = new AutomationEngineInstance(null);
            _deleteIMAPEmailCommand = new DeleteIMAPEmailCommand();
        }
    }
}
