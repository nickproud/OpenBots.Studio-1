using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Text;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.UI.CustomControls.CustomUIControls;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.Control;
using Folders = OpenBots.Core.IO.Folders;
using OBScriptVariable = OpenBots.Core.Script.ScriptVariable;

namespace OpenBots.Utilities
{
    public class ScriptContext : IScriptContext
    {
        public List<OBScriptVariable> Variables { get; set; }
        public List<ScriptArgument> Arguments { get; set; }
        public List<ScriptElement> Elements { get; set; }
        public Dictionary<string, List<AssemblyReference>> ImportedNamespaces { get; set; }
        public List<Assembly> AssembliesList { get; set; }
        public List<string> NamespacesList { get; set; }
        public CSharpCompilationOptions DefaultCompilationOptions { get; set; }
        public List<MetadataReference> DefaultReferences { get; set; }
        public string GuidPlaceholder { get; set; }
        public string CSharpPath { get; set; }
        public string ScriptFileExtension { get; set; }
        public bool IsMainScript { get; set; }

        //Intellisense properties
        private AdhocWorkspace _workspace;
        private ProjectInfo _scriptProjectInfo;
        public UIIntellisenseListBox IntellisenseListBox { get; set; }
        public UIDataGridView ActiveGridView { get; set; }
        public Control ActiveTextBox { get; set; } 
        public bool ScriptLoading { get; set; }
        public bool KeepHidden { get; set; }
        public bool WordMatched { get; set; }
        private List<int> _charsToRemoveList;
        private List<string> _completionList;
        private int _carotLocation;

        public ScriptContext()
        {
            Variables = new List<OBScriptVariable>();
            Arguments = new List<ScriptArgument>();
            Elements = new List<ScriptElement>();
            ImportedNamespaces = new Dictionary<string, List<AssemblyReference>>(ScriptDefaultNamespaces.DefaultNamespaces);
            CSharpPath = Path.Combine(Folders.GetFolder(FolderType.StudioFolder), "CSharp");
            GenerateGuidPlaceHolder();
            IntellisenseListBox = CreateIntellisenseListBox();
        }

        public void LoadCompilerObjects()
        {
            AssembliesList = NamespaceMethods.GetAssemblies(ImportedNamespaces);
            NamespacesList = NamespaceMethods.GetNamespacesList(ImportedNamespaces);

            DefaultCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithOverflowChecks(true)
                                                                                                         .WithOptimizationLevel(OptimizationLevel.Release)
                                                                                                         .WithUsings(NamespacesList);

            DefaultReferences = AssembliesList.Select(x => (MetadataReference)MetadataReference.CreateFromFile(x.Location)).ToList();

            //Intellisense
            AssembliesList.AddRange(MefHostServices.DefaultAssemblies);
            _workspace = new AdhocWorkspace(MefHostServices.Create(MefHostServices.DefaultAssemblies));

            _scriptProjectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "Script", "Script", LanguageNames.CSharp, isSubmission: true)
               .WithMetadataReferences(DefaultReferences).WithCompilationOptions(DefaultCompilationOptions);

            ActiveTextBox = null;
            LoadIntellisenseScript();
        }

        public void GenerateGuidPlaceHolder()
        {
            GuidPlaceholder = $"v{Guid.NewGuid()}".Replace("-", "");
        }

        public EmitResult EvaluateVariable(string varName, Type varType, string code)
        {
            if (string.IsNullOrEmpty(code))
                code = "null";

            string script = $"{varType.GetRealTypeFullName()}? {varName} = {code};";

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(SourceText.From(script, Encoding.UTF8), new CSharpParseOptions(languageVersion: LanguageVersion.CSharp8, kind: SourceCodeKind.Script), "");
            var compilation = CSharpCompilation.Create("CSharp", new SyntaxTree[] { parsedSyntaxTree }, DefaultReferences, DefaultCompilationOptions);
            var result = compilation.Emit(CSharpPath);

            return result;
        }

        public EmitResult EvaluateInput(Type varType, string code)
        {
            if (string.IsNullOrEmpty(code))
                code = "null";

            var script = "";
            Variables.ForEach(v => script += $"{v.VariableType.GetRealTypeFullName()}? {v.VariableName} = " +
                $"{(v.VariableValue == null || (v.VariableValue is string && string.IsNullOrEmpty(v.VariableValue.ToString())) ? "null" : v.VariableValue)};");
            Arguments.ForEach(a => script += $"{a.ArgumentType.GetRealTypeFullName()}? {a.ArgumentName} = " +
                $"{(a.ArgumentValue == null || (a.ArgumentValue is string && string.IsNullOrEmpty(a.ArgumentValue.ToString())) ? "null" : a.ArgumentValue)};");

            GenerateGuidPlaceHolder();

            string line;
            if (varType.IsGenericType && varType.GetRealTypeFullName() == "System.Collections.Generic.List<>")
                line = $"List<object> {GuidPlaceholder} = {code}.Cast<object>().ToList();";
            else if (varType.IsGenericType && varType.GetRealTypeFullName() == "System.Collections.Generic.Dictionary<,>")
                line = $"Dictionary<object, object> {GuidPlaceholder} = {code}.ToDictionary(k => (object)k.Key, v => (object)v.Value);";
            else if (varType.IsGenericType && varType.GetRealTypeFullName() == "System.Collections.Generic.KeyValuePair<,>")
                line = $"KeyValuePair<object, object> {GuidPlaceholder} = new KeyValuePair<object, object>({code}.Value.Key, {code}.Value.Value);";
            else
                line = $"{varType.GetRealTypeFullName()}? {GuidPlaceholder} = {code};";

            script += line;

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(SourceText.From(script, Encoding.UTF8), new CSharpParseOptions(languageVersion: LanguageVersion.CSharp8, kind: SourceCodeKind.Script), "");
            var compilation = CSharpCompilation.Create("CSharp", new SyntaxTree[] { parsedSyntaxTree }, DefaultReferences, DefaultCompilationOptions);
            var result = compilation.Emit(CSharpPath);

            return result;
        }

        public EmitResult EvaluateSnippet(string code)
        {
            if (string.IsNullOrEmpty(code))
                code = "null";

            var script = "";
            Variables.ForEach(v => script += $"{v.VariableType.GetRealTypeFullName()}? {v.VariableName} = " +
               $"{(v.VariableValue == null || (v.VariableValue is string && string.IsNullOrEmpty(v.VariableValue.ToString())) ? "null" : v.VariableValue)};");
            Arguments.ForEach(a => script += $"{a.ArgumentType.GetRealTypeFullName()}? {a.ArgumentName} = " +
                $"{(a.ArgumentValue == null || (a.ArgumentValue is string && string.IsNullOrEmpty(a.ArgumentValue.ToString())) ? "null" : a.ArgumentValue)};");

            script += $"{code};";

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(SourceText.From(script, Encoding.UTF8), new CSharpParseOptions(languageVersion: LanguageVersion.CSharp8, kind: SourceCodeKind.Script), "");
            var compilation = CSharpCompilation.Create("CSharp", new SyntaxTree[] { parsedSyntaxTree }, DefaultReferences, DefaultCompilationOptions);
            var result = compilation.Emit(CSharpPath);

            return result;
        }

        #region Intellisense
        public CompletionList LoadIntellisenseScript(string script = "", string scriptCode = "")
        {
            var scriptProject = _workspace.AddProject(_scriptProjectInfo);
            var scriptDocumentInfo = DocumentInfo.Create(
                DocumentId.CreateNewId(scriptProject.Id), "Script",
                sourceCodeKind: SourceCodeKind.Script,
                loader: TextLoader.From(TextAndVersion.Create(SourceText.From(scriptCode), VersionStamp.Create())));
            var scriptDocument = _workspace.AddDocument(scriptDocumentInfo);

            // cursor position is at the end
            var position = script.Length;
            if (ActiveTextBox != null && ActiveTextBox is TextBox)
                position += ((TextBox)ActiveTextBox).SelectionStart;
            else if (ActiveTextBox != null && ActiveTextBox is UIScintilla)
                position += ((UIScintilla)ActiveTextBox).SelectionStart+1;

            var completionService = CompletionService.GetService(scriptDocument);
            var results = completionService.GetCompletionsAsync(scriptDocument, position).GetAwaiter().GetResult();

            return results;
        }

        public UIIntellisenseListBox CreateIntellisenseListBox()
        {
            UIIntellisenseListBox listbox = new UIIntellisenseListBox();
            listbox.BorderStyle = BorderStyle.FixedSingle;
            listbox.DrawMode = DrawMode.OwnerDrawFixed;
            listbox.Location = new Point(0, 0);
            listbox.Size = new Size(250, 150);
            listbox.TabIndex = 3;
            listbox.Visible = false;
            listbox.Font = new Font("Segoe UI SemiBold", 8);
            listbox.SelectedIndexChanged += new EventHandler(IntellisenseListBoxAutoComplete_SelectedIndexChanged);
            listbox.DoubleClick += new EventHandler(IntellisenseListBoxAutoComplete_DoubleClick);
            listbox.KeyDown += new KeyEventHandler(IntellisenseListBoxAutoComplete_KeyDown);
            listbox.Leave += new EventHandler(IntellisenseListBoxAutoComplete_Leave);

            return listbox;        
        }

        private void IntellisenseListBoxAutoComplete_Leave(object sender, EventArgs e)
        {
            if (!ActiveTextBox.Focused)
            {
                IntellisenseListBox.Visible = false;
                KeepHidden = true;
            }
        }

        private void IntellisenseListBoxAutoComplete_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure when an item is selected, control is returned back to the richtext
            IntellisenseListBox = (UIIntellisenseListBox)sender;

            if (ActiveTextBox == null)
                return;

            ActiveTextBox.Focus();
        }

        private void IntellisenseListBoxAutoComplete_DoubleClick(object sender, EventArgs e)
        {
            if (ActiveTextBox == null)
                return;

            IntellisenseListBox = (UIIntellisenseListBox)sender;
            // Item double clicked, select it
            if (IntellisenseListBox.SelectedItems.Count == 1)
            {
                WordMatched = true;
                SelectIntellisenseListBoxItem();
                IntellisenseListBox.Hide();

                if (ActiveGridView != null)
                    ActiveGridView.ListBoxShown = false;

                if (ActiveTextBox is UITextBox)
                    ((UITextBox)ActiveTextBox).ListBoxShown = false;
                else if (ActiveTextBox is UIScintilla)
                    ((UIScintilla)ActiveTextBox).ListBoxShown = false;

                ActiveTextBox.Focus();
                WordMatched = false;               
            }
        }

        private void IntellisenseListBoxAutoComplete_KeyDown(object sender, KeyEventArgs e)
        {
            // Ignore any keys being pressed on the listview
            if (ActiveTextBox == null)
                return;

            ActiveTextBox.Focus();
        }

        public void SelectIntellisenseListBoxItem()
        {
            if (ActiveTextBox == null)
                return;

            if (WordMatched)
            {
                // insert selected item
                if (ActiveTextBox is TextBox)
                {
                    TextBox activeTextBox = (TextBox)ActiveTextBox;

                    activeTextBox.SelectionStart = _carotLocation;
                    activeTextBox.SelectionLength = 0;
                    int selectedIndex = IntellisenseListBox.SelectedIndex;
                    string intellisenseSelection = _completionList[selectedIndex];

                    string precursorText = activeTextBox.Text.Substring(0, activeTextBox.SelectionStart);
                    string postcursorText = activeTextBox.Text.Substring(activeTextBox.SelectionStart);
                    string initialText = precursorText.Substring(0, precursorText.Length - _charsToRemoveList[selectedIndex]);
                    string postInsertionText = initialText.Insert(precursorText.Length - _charsToRemoveList[selectedIndex], intellisenseSelection);

                    activeTextBox.Text = postInsertionText + postcursorText;
                    if (ActiveGridView != null && ActiveGridView.CurrentCell != null)
                        ActiveGridView.CurrentCell.Value = activeTextBox.Text;

                    activeTextBox.SelectionStart = initialText.Length + intellisenseSelection.Length;
                }
                else if (ActiveTextBox is UIScintilla)
                {
                    UIScintilla activeScintilla = (UIScintilla)ActiveTextBox;

                    activeScintilla.SelectionStart = _carotLocation+1;
                    //activeScintilla.SelectionLength = 0;
                    int selectedIndex = IntellisenseListBox.SelectedIndex;
                    string intellisenseSelection = _completionList[selectedIndex];

                    string precursorText = activeScintilla.Text.Substring(0, activeScintilla.SelectionStart);
                    string postcursorText = activeScintilla.Text.Substring(activeScintilla.SelectionStart);
                    string initialText = precursorText.Substring(0, precursorText.Length - _charsToRemoveList[selectedIndex]);
                    string postInsertionText = initialText.Insert(precursorText.Length - _charsToRemoveList[selectedIndex], intellisenseSelection);

                    activeScintilla.Text = postInsertionText + postcursorText;
                    if (ActiveGridView != null && ActiveGridView.CurrentCell != null)
                        ActiveGridView.CurrentCell.Value = activeScintilla.Text;

                    activeScintilla.SelectionStart = initialText.Length + intellisenseSelection.Length;
                }               
            }
        }

        public bool PopulateIntellisenseListBox()
        {
            IntellisenseListBox.Items.Clear();
            _workspace.ClearSolution();
            _completionList = new List<string>();
            _charsToRemoveList = new List<int>();

            string script = "";

            if (ActiveTextBox is TextBox)
            {
                Variables.ForEach(v => script += $"{v.VariableType.GetRealTypeFullName()}? {v.VariableName} = " +
                $"{(v.VariableValue == null || (v.VariableValue is string && string.IsNullOrEmpty(v.VariableValue.ToString())) ? "null" : v.VariableValue.ToString().Trim())};");
                Arguments.ForEach(a => script += $"{a.ArgumentType.GetRealTypeFullName()}? {a.ArgumentName} = " +
                    $"{(a.ArgumentValue == null || (a.ArgumentValue is string && string.IsNullOrEmpty(a.ArgumentValue.ToString())) ? "null" : a.ArgumentValue.ToString().Trim())};");

                TextBox activeTextBox = (TextBox)ActiveTextBox;

                var scriptCode = script + activeTextBox.Text;

                var results = LoadIntellisenseScript(script, scriptCode);

                if (results == null)
                    return false;

                foreach (var i in results.Items)
                {
                    string recString = activeTextBox.Text.Substring(i.Span.Start - script.Length, i.Span.Length);
                    if (scriptCode[script.Length + activeTextBox.SelectionStart - 1] != '.')
                    {
                        if (recString != "" && i.DisplayText.Length >= recString.Length && i.DisplayText.Substring(0, recString.Length).ToLower().Equals(recString.ToLower()))
                        {
                            if (recString != "")
                            {
                                _completionList.Add(i.DisplayText);
                                _charsToRemoveList.Add(recString.Length);
                            }
                            else
                            {
                                _completionList.Add(i.DisplayText);
                                _charsToRemoveList.Add(0);
                            }
                            IntellisenseListBox.Items.Add(new UIIntellisenseListBoxItem(i.DisplayText, GetIntellisenseIcon(i)));
                        }
                    }
                    else
                    {
                        _completionList.Add(i.DisplayText);
                        _charsToRemoveList.Add(0);
                        IntellisenseListBox.Items.Add(new UIIntellisenseListBoxItem(i.DisplayText, GetIntellisenseIcon(i)));
                    }
                }

            }
            else if (ActiveTextBox is UIScintilla)
            {
                UIScintilla activeScintilla = (UIScintilla)ActiveTextBox;

                var scriptCode = script + activeScintilla.Text;

                if (activeScintilla.SelectionStart >= scriptCode.Length)
                    activeScintilla.SelectionStart = scriptCode.Length - 1;
                var results = LoadIntellisenseScript(script, scriptCode);

                if (results == null)
                    return false;
                try
                {
                    foreach (var i in results.Items)
                    {
                        int trueLength = i.Span.Length;
                        if (i.Span.Length == 0)
                            trueLength = 1;
                        string recString = activeScintilla.Text.Substring(i.Span.Start - script.Length, trueLength);
                        if (scriptCode[script.Length + activeScintilla.SelectionStart] != '.')
                        {
                            if (recString != "" && i.DisplayText.Length >= recString.Length && i.DisplayText.Substring(0, recString.Length).ToLower().Equals(recString.ToLower()))
                            {
                                if (recString != "")
                                {
                                    _completionList.Add(i.DisplayText);
                                    _charsToRemoveList.Add(recString.Length);
                                }
                                else
                                {
                                    _completionList.Add(i.DisplayText);
                                    _charsToRemoveList.Add(0);
                                }
                                IntellisenseListBox.Items.Add(new UIIntellisenseListBoxItem(i.DisplayText, GetIntellisenseIcon(i)));
                            }
                        }
                        else
                        {
                            _completionList.Add(i.DisplayText);
                            _charsToRemoveList.Add(0);
                            IntellisenseListBox.Items.Add(new UIIntellisenseListBoxItem(i.DisplayText, GetIntellisenseIcon(i)));
                        }
                    }
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex);
                }
            }

            if (IntellisenseListBox.Items.Count <= 0)
                return false;

            return true;
        }

        private int GetIntellisenseIcon(CompletionItem i)
        {
            int image = 15;
            if (i.Tags.Contains("Method"))
                image = 0;
            else if (i.Tags.Contains("ExtensionMethod"))
                image = 1;
            else if (i.Tags.Contains("Property"))
                image = 2;
            else if (i.Tags.Contains("Class"))
                image = 3;
            else if (i.Tags.Contains("Enum"))
                image = 4;
            else if (i.Tags.Contains("Event"))
                image = 5;
            else if (i.Tags.Contains("Interface"))
                image = 6;
            else if (i.Tags.Contains("Namespace"))
                image = 7;
            else if (i.Tags.Contains("Structure"))
                image = 8;
            else if (i.Tags.Contains("Field"))
                image = 9;
            else if (i.Tags.Contains("Delegate"))
                image = 10;
            else if (i.Tags.Contains("Keyword"))
                image = 11;
            else if (i.Tags.Contains("TypeParameter"))
                image = 12;
            else if (i.Tags.Contains("Parameter"))
                image = 13;
            else if (i.Tags.Contains("Constant"))
                image = 14;

            return image;
        }

        public void CodeDGV_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ActiveGridView = (UIDataGridView)sender;

            var txtBox = e.Control as TextBox;
            if (txtBox != null)
            {
                //removes an existing event handlers, if present, to avoid adding multiple handlers when the editing control is reused
                txtBox.KeyDown -= new KeyEventHandler(CodeInput_KeyDown);
                txtBox.TextChanged -= new EventHandler(CodeDGVInput_TextChanged);
                txtBox.Leave -= new EventHandler(CodeInput_Leave);

                if ((ScriptFileExtension == ".obscript" || ScriptFileExtension == ".cs") &&
                    ActiveGridView.CurrentCell.OwningColumn.Name != "variableName" && 
                    ActiveGridView.CurrentCell.OwningColumn.Name != "argumentName")
                { 
                    //adds the event handlers
                    txtBox.KeyDown += new KeyEventHandler(CodeInput_KeyDown);
                    txtBox.TextChanged += new EventHandler(CodeDGVInput_TextChanged);
                    txtBox.Leave += new EventHandler(CodeInput_Leave);
                }
            }
        }

        public void CodeInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox)
                ActiveTextBox = (TextBox)sender;
            else if (sender is UIScintilla)
                ActiveTextBox = (UIScintilla)sender;

            KeepHidden = false;

            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Tab || e.KeyCode == Keys.Space)
            {
                // Autocomplete
                SelectIntellisenseListBoxItem();

                //this.typed = "";
                WordMatched = false;
                if (ActiveTextBox is TextBox)
                    e.Handled = true;          
                IntellisenseListBox.Hide();
                if (ActiveGridView != null)
                    ActiveGridView.ListBoxShown = false;

                if (ActiveTextBox is UITextBox)
                    ((UITextBox)ActiveTextBox).ListBoxShown = false;
                else if (ActiveTextBox is UIScintilla)
                    ((UIScintilla)ActiveTextBox).ListBoxShown = false;

                KeepHidden = true;
            }
            else if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Delete)
            {
                IntellisenseListBox.Hide();
                if (ActiveGridView != null)
                    ActiveGridView.ListBoxShown = false;

                if (ActiveTextBox is UITextBox)
                    ((UITextBox)ActiveTextBox).ListBoxShown = false;
                else if (ActiveTextBox is UIScintilla)
                    ((UIScintilla)ActiveTextBox).ListBoxShown = false;

                KeepHidden = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                // The up key moves up our member list, if the list is visible
                if (IntellisenseListBox.Visible)
                {
                    WordMatched = true;
                    if (IntellisenseListBox.SelectedIndex > 0)
                        IntellisenseListBox.SelectedIndex--;

                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                // The up key moves down our member list, if the list is visible
                if (IntellisenseListBox.Visible)
                {
                    WordMatched = true;
                    if (IntellisenseListBox.SelectedIndex < IntellisenseListBox.Items.Count - 1)
                        IntellisenseListBox.SelectedIndex++;

                    e.Handled = true;
                }
            }
        }

        public void CodeDGVInput_TextChanged(object sender, EventArgs e)
        {
            ActiveTextBox = (TextBox)sender;
            _carotLocation = ((TextBox)ActiveTextBox).SelectionStart;

            if (ActiveTextBox.Parent != null)
                ActiveGridView = (UIDataGridView)ActiveTextBox.Parent.Parent;

            if (PopulateIntellisenseListBox() && !KeepHidden)
            {
                // Find the position of the caret
                Point point = ActiveGridView.Location;
                var cellRowMultiplier = ActiveGridView.CurrentCellAddress.Y + 1;

                List<int> colWidths = new List<int>();
                foreach (DataGridViewColumn col in ActiveGridView.Columns)
                    colWidths.Add(col.Width);

                Point currentCellAddress = ActiveGridView.CurrentCellAddress;
                int widthToAdd = 0;
                for (int i = 0; i < currentCellAddress.X; i++)
                    widthToAdd += colWidths[i];

                point.Y += ActiveTextBox.Location.Y + (int)Math.Ceiling(ActiveTextBox.Font.GetHeight() * 1.4) + (ActiveGridView.RowTemplate.Height * cellRowMultiplier);
                point.X += ActiveTextBox.Location.X + TextRenderer.MeasureText(ActiveTextBox.Text.Substring(0, _carotLocation), ActiveTextBox.Font).Width + ActiveGridView.RowHeadersWidth + widthToAdd;

                Control currenControl = ActiveGridView;
                while (currenControl.Parent != null && !(currenControl.Parent is Form))
                {
                    point.Y += currenControl.Parent.Location.Y;
                    point.X += currenControl.Parent.Location.X;
                    currenControl = currenControl.Parent;
                }

                IntellisenseListBox.Location = point;
                IntellisenseListBox.BringToFront();               
                ActiveGridView.ListBoxShown = true;
                IntellisenseListBox.Show();
            }
            else
            {
                IntellisenseListBox.Hide();
                ActiveGridView.ListBoxShown = false;
            }
        }

        public void CodeTBXInput_TextChanged(object sender, EventArgs e)
        {          
            if (sender is UITextBox)
            {
                ActiveTextBox = (UITextBox)sender;

                if (ActiveTextBox.Text == "")
                    return;
                _carotLocation = ((UITextBox)ActiveTextBox).SelectionStart;
            }
            else if (sender is UIScintilla)
            {
                ActiveTextBox = (UIScintilla)sender;

                if (ActiveTextBox.Text == "" || ScriptLoading)
                {
                    ScriptLoading = false;
                    return;
                }

                _carotLocation = ((UIScintilla)ActiveTextBox).SelectionStart;
            }

            if (PopulateIntellisenseListBox() && !KeepHidden && ActiveTextBox is UITextBox)
            {
                // Find the position of the caret
                Point point = new Point(0, 0);

                string[] lines = ActiveTextBox.Text.Split(new char[] { '\r', '\n' });
                List<string> filteredLines = new List<string>();
                int strippedChars = lines.Length - 1;
                foreach (string line in lines)
                {
                    if (line != "")
                        filteredLines.Add(line);
                }
                int lengthSum = 0;
                int currentLine = 1;
                int carotLocationOnLine = _carotLocation;
                for (int i = 0; i < filteredLines.Count; i++)
                {
                    if (_carotLocation < lengthSum + strippedChars)
                        continue;
                    currentLine = i + 1;
                    carotLocationOnLine = _carotLocation - (lengthSum + i * 2);
                    lengthSum += filteredLines[i].Length;
                }

                int lineLength = TextRenderer.MeasureText(filteredLines[currentLine - 1].Substring(0, carotLocationOnLine), ActiveTextBox.Font).Width;

                point.Y += ActiveTextBox.Location.Y + (int)Math.Ceiling(ActiveTextBox.Font.GetHeight() * currentLine) + 2;
                point.X += ActiveTextBox.Location.X + lineLength;

                Control currentControl = ActiveTextBox;
                while (currentControl.Parent != null && !(currentControl.Parent is Form))
                {
                    point.Y += currentControl.Parent.Location.Y;
                    point.X += currentControl.Parent.Location.X;
                    currentControl = currentControl.Parent;
                }

                IntellisenseListBox.Location = point;
                IntellisenseListBox.BringToFront();

                if (ActiveTextBox is UITextBox)
                    ((UITextBox)ActiveTextBox).ListBoxShown = true;
                else if (ActiveTextBox is UIScintilla)
                    ((UIScintilla)ActiveTextBox).ListBoxShown = true;

                IntellisenseListBox.Show();
            }
            else if (PopulateIntellisenseListBox() && !KeepHidden && ActiveTextBox is UIScintilla)
            {
                // Find the position of the caret
                Point point = new Point(0, 0);

                string precursorText = ActiveTextBox.Text.Substring(0, ((UIScintilla)ActiveTextBox).SelectionStart);
                string[] splitPretext = precursorText.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
                string lastLine = splitPretext[splitPretext.Length - 1];

                string spacesInTab = "";
                for(int i=0; i < ((UIScintilla)ActiveTextBox).TabSize; i++)
                {
                    spacesInTab += " ";
                }
                int marginWidths = 0;
                foreach(Margin margin in ((UIScintilla)ActiveTextBox).Margins)
                {
                    marginWidths += margin.Width;
                }

                var scintillaStyle = ((UIScintilla)ActiveTextBox).Styles[0];
                Font fontUsed = new Font(scintillaStyle.Font, scintillaStyle.Size, FontStyle.Regular);

                int lineLength = TextRenderer.MeasureText(lastLine.Replace("\t", spacesInTab), fontUsed).Width;
                
                point.Y += ActiveTextBox.Location.Y + (int)Math.Ceiling(ActiveTextBox.Font.GetHeight() * (splitPretext.Length)) + 2;
                point.X += ActiveTextBox.Location.X + lineLength + marginWidths + 5;
                

                Control currentControl = ActiveTextBox;
                while (currentControl.Parent != null && !(currentControl.Parent is Form))
                {
                    point.Y += currentControl.Parent.Location.Y;
                    point.X += currentControl.Parent.Location.X;
                    currentControl = currentControl.Parent;
                }

                IntellisenseListBox.Location = point;
                IntellisenseListBox.BringToFront();

                if (ActiveTextBox is UITextBox)
                    ((UITextBox)ActiveTextBox).ListBoxShown = true;
                else if (ActiveTextBox is UIScintilla)
                    ((UIScintilla)ActiveTextBox).ListBoxShown = true;

                IntellisenseListBox.Show();
            }
            else
            {
                IntellisenseListBox.Hide();
                if (ActiveTextBox is UITextBox)
                    ((UITextBox)ActiveTextBox).ListBoxShown = false;
                else if (ActiveTextBox is UIScintilla)
                    ((UIScintilla)ActiveTextBox).ListBoxShown = false;
            }              
        }

        public void CodeInput_Leave(object sender, EventArgs e)
        {
            if (!IntellisenseListBox.Focused) {
                IntellisenseListBox.Visible = false;
                KeepHidden = true;
                ActiveGridView = null;
                ActiveTextBox = null;
            }
        }
        public void AddIntellisenseControls(ControlCollection controls)
        {
            controls.Add(IntellisenseListBox);
            ActiveTextBox = null;
            ActiveGridView = null;
            _charsToRemoveList = null;
            _completionList = null;
            _carotLocation = -1;
            KeepHidden = false;
            WordMatched = false;           
        }

        public void RemoveIntellisenseControls(ControlCollection controls)
        {
            IntellisenseListBox.Hide();
            controls.Remove(IntellisenseListBox);
            ActiveTextBox = null;
            ActiveGridView = null;
        }
        #endregion
    }
}
