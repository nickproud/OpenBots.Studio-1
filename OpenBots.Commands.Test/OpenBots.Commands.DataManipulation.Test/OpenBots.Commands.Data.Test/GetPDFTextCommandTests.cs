using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.IO;
using Xunit;

namespace OpenBots.Commands.Data.Test
{
    public class GetPDFTextCommandTests
    {
        private GetPDFTextCommand _getPDFText;
        private AutomationEngineInstance _engine;

        [Theory]
        [InlineData("File Path")]
        [InlineData("File URL")]
        public async void GetsPDFText(string filePathOrUrl)
        {
            _getPDFText = new GetPDFTextCommand();
            _engine = new AutomationEngineInstance(null);
            string filepath = "";
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            if (filePathOrUrl.Equals("File Path"))
            {
                filepath = Path.Combine(projectDirectory, @"Resources\dummy.pdf");
            }
            else
            {
                filepath = "https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf";
            }
            VariableMethods.CreateTestVariable(filepath, _engine, "filepath", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "outputText", typeof(string));

            _getPDFText.v_FileSourceType = filePathOrUrl;
            _getPDFText.v_FilePath = "{filepath}";
            _getPDFText.v_OutputUserVariableName = "{outputText}";

            _getPDFText.RunCommand(_engine);

            Assert.Equal("Dummy PDF file", (string)await "{outputText}".EvaluateCode(_engine));
        }

        [Fact]
        public async System.Threading.Tasks.Task HandlesInvalidFilepath()
        {
            _getPDFText = new GetPDFTextCommand();
            _engine = new AutomationEngineInstance(null);
            string filepath = "";

            VariableMethods.CreateTestVariable(filepath, _engine, "filepath", typeof(string));

            _getPDFText.v_FileSourceType = "File Path";
            _getPDFText.v_FilePath = "{filepath}";
            _getPDFText.v_OutputUserVariableName = "{outputText}";

            await Assert.ThrowsAsync<FileNotFoundException>(() => _getPDFText.RunCommand(_engine));
        }
    }
}
