using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;

namespace OpenBots.Commands.Data.Test
{
    public class FormatDateCommandTests
    {
        private FormatDateCommand _formatDate;
        private AutomationEngineInstance _engine;

        [Fact]
        public void FormatsDate()
        {
            _formatDate = new FormatDateCommand();
            _engine = new AutomationEngineInstance(null);

            DateTime inputDate = DateTime.Now;
            string dateFormat = "MM/dd/yy, hh:mm:ss";

            VariableMethods.CreateTestVariable(inputDate, _engine, "inputDate", typeof(string));
            VariableMethods.CreateTestVariable(dateFormat, _engine, "dateFormat", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _formatDate.v_InputData = "{inputDate}";
            _formatDate.v_ToStringFormat = "{dateFormat}";
            _formatDate.v_OutputUserVariableName = "{output}";

            _formatDate.RunCommand(_engine);

            string formattedDate = inputDate.ToString(dateFormat);
            Assert.Equal(formattedDate, _formatDate.v_OutputUserVariableName.ConvertUserVariableToString(_engine));
        }

        [Fact]
        public void HandlesInvalidInput()
        {
            _formatDate = new FormatDateCommand();
            _engine = new AutomationEngineInstance(null);

            int inputDate = 1;
            string dateFormat = "MM/dd/yy, hh:mm:ss";

            VariableMethods.CreateTestVariable(inputDate, _engine, "inputDate", typeof(string));
            VariableMethods.CreateTestVariable(dateFormat, _engine, "dateFormat", typeof(string));

            _formatDate.v_InputData = "{inputDate}";
            _formatDate.v_ToStringFormat = "{dateFormat}";
            _formatDate.v_OutputUserVariableName = "{output}";

            Assert.Throws<FormatException>(() => _formatDate.RunCommand(_engine));
        }
    }
}
