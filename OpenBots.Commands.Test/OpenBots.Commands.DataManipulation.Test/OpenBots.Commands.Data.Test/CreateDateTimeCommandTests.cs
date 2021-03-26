using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using Xunit;

namespace OpenBots.Commands.Data.Test
{
    public class CreateDateTimeCommandTests
    {
        private CreateDateTimeCommand _createDateTime;
        private AutomationEngineInstance _engine;

        [Fact]
        public void CreatesDateTime()
        {
            _createDateTime = new CreateDateTimeCommand();
            _engine = new AutomationEngineInstance(null);

            string year = "2020";
            string month = "jan";
            string day = "1";
            string time = "1:10";
            string AMorPM = "PM";

            VariableMethods.CreateTestVariable(year, _engine, "year", typeof(string));
            VariableMethods.CreateTestVariable(month, _engine, "month", typeof(string));
            VariableMethods.CreateTestVariable(day, _engine, "day", typeof(string));
            VariableMethods.CreateTestVariable(time, _engine, "time", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "outputVar", typeof(DateTime));

            _createDateTime.v_Year = "{year}";
            _createDateTime.v_Month = "{month}";
            _createDateTime.v_Day = "{day}";
            _createDateTime.v_Time = "{time}";
            _createDateTime.v_AMorPM = AMorPM;
            _createDateTime.v_OutputUserVariableName = "{outputVar}";

            _createDateTime.RunCommand(_engine);

            Object output = _createDateTime.v_OutputUserVariableName.ConvertUserVariableToObject(_engine, typeof(DateTime));
            Assert.IsType<DateTime>(output);
            DateTime dateOutput = (DateTime)output;
            Assert.Equal(Int32.Parse(year), dateOutput.Year);
            Assert.Equal(1,dateOutput.Month);
            Assert.Equal(1, dateOutput.Day);
            Assert.Equal(13, dateOutput.Hour);
            Assert.Equal(10, dateOutput.Minute);
        }
    }
}
