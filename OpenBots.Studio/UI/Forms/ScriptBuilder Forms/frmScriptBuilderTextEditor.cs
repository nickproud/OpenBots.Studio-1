using OpenBots.Core.Enums;
using OpenBots.UI.CustomControls.CustomUIControls;
using ScintillaNET;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    {
        #region TextEditor Events
        private UIScintilla NewTextEditorActions(ProjectType projecttype, string title = "newTextEditorActions")
        {
            UIScintilla scintilla = new UIScintilla();
            scintilla.Dock = DockStyle.Fill;
            scintilla.KeyDown += newTextEditorActions_KeyDown;
            scintilla.TextChanged += newTextEditorActions_TextChanged;
            scintilla.Leave += newTextEditorActions_Leave;

            // Reset the styles
            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 11;
            scintilla.StyleClearAll();

            scintilla.SetProperty("tab.timmy.whinge.level", "1");
            scintilla.SetProperty("fold", "1");

            // Use margin 2 for fold markers
            scintilla.Margins[2].Type = MarginType.Symbol;
            scintilla.Margins[2].Mask = Marker.MaskFolders;
            scintilla.Margins[2].Sensitive = true;
            scintilla.Margins[2].Width = 20;

            // Reset folder markers
            for (int i = Marker.FolderEnd; i <= Marker.FolderOpen; i++)
            {
                scintilla.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                scintilla.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            // Style the folder markers
            scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            scintilla.Markers[Marker.Folder].SetBackColor(SystemColors.ControlText);
            scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            scintilla.Markers[Marker.FolderEnd].SetBackColor(SystemColors.ControlText);
            scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            scintilla.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

            switch (projecttype)
            {
                case ProjectType.Python:
                    scintilla.Lexer = Lexer.Python;
                    scintilla.Styles[Style.Python.Default].ForeColor = Color.FromArgb(0x80, 0x80, 0x80);
                    scintilla.Styles[Style.Python.CommentLine].ForeColor = Color.FromArgb(0x00, 0x7F, 0x00);
                    scintilla.Styles[Style.Python.CommentLine].Italic = true;
                    scintilla.Styles[Style.Python.Number].ForeColor = Color.FromArgb(0x00, 0x7F, 0x7F);
                    scintilla.Styles[Style.Python.String].ForeColor = Color.FromArgb(0x7F, 0x00, 0x7F);
                    scintilla.Styles[Style.Python.Character].ForeColor = Color.FromArgb(0x7F, 0x00, 0x7F);
                    scintilla.Styles[Style.Python.Word].ForeColor = Color.FromArgb(0x00, 0x00, 0x7F);
                    scintilla.Styles[Style.Python.Word].Bold = true;
                    scintilla.Styles[Style.Python.Triple].ForeColor = Color.FromArgb(0x7F, 0x00, 0x00);
                    scintilla.Styles[Style.Python.TripleDouble].ForeColor = Color.FromArgb(0x7F, 0x00, 0x00);
                    scintilla.Styles[Style.Python.ClassName].ForeColor = Color.FromArgb(0x00, 0x00, 0xFF);
                    scintilla.Styles[Style.Python.ClassName].Bold = true;
                    scintilla.Styles[Style.Python.DefName].ForeColor = Color.FromArgb(0x00, 0x7F, 0x7F);
                    scintilla.Styles[Style.Python.DefName].Bold = true;
                    scintilla.Styles[Style.Python.Operator].Bold = true;
                    scintilla.Styles[Style.Python.CommentBlock].ForeColor = Color.FromArgb(0x7F, 0x7F, 0x7F);
                    scintilla.Styles[Style.Python.CommentBlock].Italic = true;
                    scintilla.Styles[Style.Python.StringEol].ForeColor = Color.FromArgb(0x00, 0x00, 0x00);
                    scintilla.Styles[Style.Python.StringEol].BackColor = Color.FromArgb(0xE0, 0xC0, 0xE0);
                    scintilla.Styles[Style.Python.StringEol].FillLine = true;
                    scintilla.Styles[Style.Python.Word2].ForeColor = Color.FromArgb(0x40, 0x70, 0x90);
                    scintilla.Styles[Style.Python.Decorator].ForeColor = Color.FromArgb(0x80, 0x50, 0x00);
                    // Important for Python
                    scintilla.ViewWhitespace = WhitespaceMode.VisibleAlways;
                    break;
                case ProjectType.CSScript:
                case ProjectType.TagUI:
                    scintilla.Lexer = Lexer.Cpp;
                    scintilla.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
                    scintilla.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
                    scintilla.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
                    scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
                    scintilla.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
                    scintilla.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
                    scintilla.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
                    scintilla.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
                    scintilla.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
                    scintilla.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
                    scintilla.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
                    scintilla.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
                    scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
                    break;
                case ProjectType.PowerShell:
                    scintilla.Lexer = Lexer.PowerShell;
                    scintilla.Styles[Style.PowerShell.Default].ForeColor = Color.Black;
                    scintilla.Styles[Style.PowerShell.Comment].ForeColor = Color.Green;
                    scintilla.Styles[Style.PowerShell.CommentDocKeyword].ForeColor = Color.Green;
                    scintilla.Styles[Style.PowerShell.CommentStream].ForeColor = Color.Green;
                    scintilla.Styles[Style.PowerShell.Number].ForeColor = Color.Gray;
                    scintilla.Styles[Style.PowerShell.Cmdlet].ForeColor = Color.LimeGreen;
                    scintilla.Styles[Style.PowerShell.Variable].ForeColor = Color.Gray;
                    scintilla.Styles[Style.PowerShell.Operator].ForeColor = Color.Teal;
                    scintilla.Styles[Style.PowerShell.String].ForeColor = Color.LimeGreen;
                    scintilla.Styles[Style.PowerShell.Keyword].ForeColor = Color.ForestGreen;
                    scintilla.Styles[Style.PowerShell.User1].ForeColor = Color.Magenta;
                    scintilla.Styles[Style.PowerShell.Character].ForeColor = Color.LimeGreen;
                    scintilla.Styles[Style.PowerShell.Alias].ForeColor = Color.MediumBlue;               
                    scintilla.Styles[Style.PowerShell.Function].ForeColor = Color.MediumBlue;
                    scintilla.Styles[Style.PowerShell.HereCharacter].ForeColor = Color.LimeGreen;
                    scintilla.Styles[Style.PowerShell.HereString].ForeColor = Color.LimeGreen;
                    scintilla.Styles[Style.PowerShell.Identifier].ForeColor = Color.Black;            
                    break;

                    //Could be useful later if we decide to allow users to open/edit the config file
                    //case Json:
                    //    // Configure the JSON lexer styles
                    //    scintilla.Lexer = Lexer.Json;
                    //    scintilla.Styles[Style.Json.Default].ForeColor = Color.Silver;
                    //    scintilla.Styles[Style.Json.BlockComment].ForeColor = Color.FromArgb(0, 128, 0); // Green
                    //    scintilla.Styles[Style.Json.LineComment].ForeColor = Color.FromArgb(0, 128, 0); // Green
                    //    scintilla.Styles[Style.Json.Number].ForeColor = Color.Olive;
                    //    scintilla.Styles[Style.Json.PropertyName].ForeColor = Color.Blue;
                    //    scintilla.Styles[Style.Json.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
                    //    scintilla.Styles[Style.Json.StringEol].BackColor = Color.Pink;
                    //    scintilla.Styles[Style.Json.Operator].ForeColor = Color.Purple;
                    //    break;
            }

            return scintilla;
        }

        private void newTextEditorActions_Leave(object sender, EventArgs e)
        {
            if (ScriptProject.ProjectType == ProjectType.CSScript)
                _scriptContext.CodeInput_Leave(sender, e);
        }

        private void newTextEditorActions_TextChanged(object sender, EventArgs e)
        {
            if (ScriptProject.ProjectType == ProjectType.CSScript)
                _scriptContext.CodeTBXInput_TextChanged(sender, e);

            if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                uiScriptTabControl.SelectedTab.Text += " *";
        }

        #region TextEdtior Copy, Paste, Edit, Delete
        private void newTextEditorActions_KeyDown(object sender, KeyEventArgs e)
        {
            if (ScriptProject.ProjectType == ProjectType.CSScript)
                _scriptContext.CodeInput_KeyDown(sender, e);

            if (e.Control)
            {
                if (e.Shift)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.S:
                            SaveAllFiles();
                            e.SuppressKeyPress = true;
                            break;
                    }
                }
                else
                {
                    switch (e.KeyCode)
                    {
                        case Keys.S:
                            SaveToTextEditorFile(false);
                            e.SuppressKeyPress = true;
                            break;
                        case Keys.O:
                            aboutOpenBotsToolStripMenuItem_Click(null, null);
                            e.SuppressKeyPress = true;
                            break;
                    }
                }
            }
        }
        #endregion      
        #endregion
    }
}
