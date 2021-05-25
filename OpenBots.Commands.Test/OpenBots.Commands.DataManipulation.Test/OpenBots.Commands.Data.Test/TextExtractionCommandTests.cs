using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System.Data;
using Xunit;
using OBData = System.Data;

namespace OpenBots.Commands.Data.Test
{
    public class TextExtractionCommandTests
    {
        private AutomationEngineInstance _engine;
        private TextExtractionCommand _textExtraction;

        [Fact]
        public async void ExtractsAllAfterText()
        {
            _engine = new AutomationEngineInstance(null);
            _textExtraction = new TextExtractionCommand();

            string input = "This is an example sentence";
            OBData.DataTable extractParams = new OBData.DataTable();
            extractParams.Columns.Add("Parameter Name");
            extractParams.Columns.Add("Parameter Value");
            DataRow row1 = extractParams.NewRow();
            row1["Parameter Name"] = "Leading Text";
            row1["Parameter Value"] = "{leadingText}";
            extractParams.Rows.Add(row1);
            DataRow row2 = extractParams.NewRow();
            row2["Parameter Name"] = "Skip Past Occurences";
            row2["Parameter Value"] = "0";
            extractParams.Rows.Add(row2);

            VariableMethods.CreateTestVariable("This is an ", _engine, "leadingText", typeof(string));
            VariableMethods.CreateTestVariable(input, _engine, "input", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _textExtraction.v_InputText = "{input}";
            _textExtraction.v_TextExtractionType = "Extract All After Text";
            _textExtraction.v_TextExtractionTable = extractParams;
            _textExtraction.v_OutputUserVariableName = "{output}";

            _textExtraction.RunCommand(_engine);

            Assert.Equal("example sentence", (string)await "{output}".EvaluateCode(_engine));
        }

        [Fact]
        public async void ExtractsAllBeforeText()
        {
            _engine = new AutomationEngineInstance(null);
            _textExtraction = new TextExtractionCommand();

            string input = "This is an example sentence";
            OBData.DataTable extractParams = new OBData.DataTable();
            extractParams.Columns.Add("Parameter Name");
            extractParams.Columns.Add("Parameter Value");
            DataRow row1 = extractParams.NewRow();
            row1["Parameter Name"] = "Trailing Text";
            row1["Parameter Value"] = "{trailingText}";
            extractParams.Rows.Add(row1);
            DataRow row2 = extractParams.NewRow();
            row2["Parameter Name"] = "Skip Past Occurences";
            row2["Parameter Value"] = "0";
            extractParams.Rows.Add(row2);

            VariableMethods.CreateTestVariable(" an example sentence", _engine, "trailingText", typeof(string));
            VariableMethods.CreateTestVariable(input, _engine, "input", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _textExtraction.v_InputText = "{input}";
            _textExtraction.v_TextExtractionType = "Extract All Before Text";
            _textExtraction.v_TextExtractionTable = extractParams;
            _textExtraction.v_OutputUserVariableName = "{output}";

            _textExtraction.RunCommand(_engine);

            Assert.Equal("This is", (string)await "{output}".EvaluateCode(_engine));
        }

        [Fact]
        public async void ExtractsAllBetweenText()
        {
            _engine = new AutomationEngineInstance(null);
            _textExtraction = new TextExtractionCommand();

            string input = "This is an example sentence";
            OBData.DataTable extractParams = new OBData.DataTable();
            extractParams.Columns.Add("Parameter Name");
            extractParams.Columns.Add("Parameter Value");
            DataRow row1 = extractParams.NewRow();
            row1["Parameter Name"] = "Leading Text";
            row1["Parameter Value"] = "{leadingText}";
            extractParams.Rows.Add(row1);
            DataRow row2 = extractParams.NewRow();
            row2["Parameter Name"] = "Trailing Text";
            row2["Parameter Value"] = "{trailingText}";
            extractParams.Rows.Add(row2);
            DataRow row3 = extractParams.NewRow();
            row3["Parameter Name"] = "Skip Past Occurences";
            row3["Parameter Value"] = "0";
            extractParams.Rows.Add(row3);

            VariableMethods.CreateTestVariable("This is an ", _engine, "leadingText", typeof(string));
            VariableMethods.CreateTestVariable(" sentence", _engine, "trailingText", typeof(string));
            VariableMethods.CreateTestVariable(input, _engine, "input", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "output", typeof(string));

            _textExtraction.v_InputText = "{input}";
            _textExtraction.v_TextExtractionType = "Extract All Between Text";
            _textExtraction.v_TextExtractionTable = extractParams;
            _textExtraction.v_OutputUserVariableName = "{output}";

            _textExtraction.RunCommand(_engine);

            Assert.Equal("example", (string)await "{output}".EvaluateCode(_engine));
        }
    }
}
