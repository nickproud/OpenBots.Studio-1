using Microsoft.VisualBasic;
using Newtonsoft.Json;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Project;
using OpenBots.Core.Script;
using OpenBots.Nuget;
using OpenBots.Studio.Utilities;
using OpenBots.UI.CustomControls.CustomUIControls;
using OpenBots.UI.Forms.Supplement_Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VBFileSystem = Microsoft.VisualBasic.FileIO.FileSystem;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    {

        #region Project Tool Strip, Buttons and Pane
        private void addProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddProject();
        }

        private void uiBtnProject_Click(object sender, EventArgs e)
        {
            addProjectToolStripMenuItem_Click(sender, e);
        }

        public DialogResult AddProject()
        {
            tvProject.Nodes.Clear();
            var projectBuilder = new frmProjectBuilder();
            projectBuilder.ShowDialog();

            //close OpenBots if add project form is closed at startup
            if (projectBuilder.DialogResult == DialogResult.Cancel && ScriptProject == null)
            {
                Application.Exit();
                return DialogResult.Abort;
            }

            //create new project
            else if (projectBuilder.Action == frmProjectBuilder.ProjectAction.CreateProject)
            {
                DialogResult result = CheckForUnsavedScripts();
                if (result == DialogResult.Cancel)
                    return DialogResult.Cancel;

                uiScriptTabControl.TabPages.Clear();
                ScriptProjectPath = projectBuilder.NewProjectPath;

                //create new project
                ScriptProject = new Project(projectBuilder.NewProjectName, projectBuilder.NewProjectType);
                string configPath = Path.Combine(ScriptProjectPath, "project.obconfig");

                //create config file
                File.WriteAllText(configPath, JsonConvert.SerializeObject(ScriptProject));

                NotifySync("Loading package assemblies...", Color.White);               

                var assemblyList = NugetPackageManager.LoadPackageAssemblies(configPath);
                _builder = AppDomainSetupManager.LoadBuilder(assemblyList, _typeContext.GroupedTypes);
                AContainer = _builder.Build();

                string mainScriptPath = Path.Combine(ScriptProjectPath, ScriptProjectPath, ScriptProject.Main);
                string mainScriptName = Path.GetFileNameWithoutExtension(mainScriptPath);

                switch (ScriptProject.ProjectType)
                {
                    case ProjectType.OpenBots:
                        CreateOpenBotsProject(mainScriptName, mainScriptPath);
                        break;
                    case ProjectType.Python:
                    case ProjectType.TagUI:
                    case ProjectType.CSScript:
                        CreateTextEditorProject(mainScriptName, mainScriptPath);
                        break;
                }

                //show success dialog
                Notify("Project has been created successfully!", Color.White);
            }

            //open existing OpenBots project
            else if (projectBuilder.Action == frmProjectBuilder.ProjectAction.OpenProject)
            {
                DialogResult result = CheckForUnsavedScripts();
                if (result == DialogResult.Cancel)
                    return DialogResult.Cancel;

                try
                {
                    //open project
                    Project project = Project.OpenProject(projectBuilder.ExistingConfigPath);
                    string mainFileName = project.Main;

                    string mainFilePath = Directory.GetFiles(projectBuilder.ExistingProjectPath, mainFileName, SearchOption.AllDirectories).FirstOrDefault();
                    if (mainFilePath == null)
                        throw new Exception("Main script not found");

                    NotifySync("Loading package assemblies...", Color.White);

                    var assemblyList = NugetPackageManager.LoadPackageAssemblies(projectBuilder.ExistingConfigPath);
                    _builder = AppDomainSetupManager.LoadBuilder(assemblyList, _typeContext.GroupedTypes);
                    AContainer = _builder.Build();

                    ScriptProject = project;
                    _mainFileName = mainFileName;                 
                    ScriptProjectPath = projectBuilder.ExistingProjectPath;
                    uiScriptTabControl.TabPages.Clear();

                    //open Main
                    switch (ScriptProject.ProjectType)
                    {
                        case ProjectType.OpenBots:
                            OpenOpenBotsFile(mainFilePath);
                            break;
                        case ProjectType.Python:
                        case ProjectType.TagUI:
                        case ProjectType.CSScript:
                            OpenTextEditorFile(mainFilePath, ScriptProject.ProjectType);
                            break;
                    }
                                  
                    //show success dialog
                    Notify("Project has been opened successfully!", Color.White);
                }
                catch (Exception ex)
                {
                    projectBuilder.Dispose();

                    //show fail dialog
                    Notify("An Error Occured: " + ex.Message, Color.Red);

                    //try adding project again
                    AddProject();                    
                    return DialogResult.None;
                }
            }

            projectBuilder.Dispose();

            DirectoryInfo projectDirectoryInfo = new DirectoryInfo(ScriptProjectPath);
            TreeNode projectNode = new TreeNode(projectDirectoryInfo.Name);
            projectNode.Text = projectDirectoryInfo.Name;
            projectNode.Tag = projectDirectoryInfo.FullName;
            projectNode.Nodes.Add("Empty");
            projectNode.ContextMenuStrip = cmsProjectMainFolderActions;          
            tvProject.Nodes.Add(projectNode);
            projectNode.Expand();
            LoadCommands(this);

            //save to recent projects 
            if (_appSettings.ClientSettings.RecentProjects == null)
                _appSettings.ClientSettings.RecentProjects = new List<string>();

            if (_appSettings.ClientSettings.RecentProjects.Contains(ScriptProjectPath))
                _appSettings.ClientSettings.RecentProjects.Remove(ScriptProjectPath);

            _appSettings.ClientSettings.RecentProjects.Insert(0, ScriptProjectPath);

            if (_appSettings.ClientSettings.RecentProjects.Count > 10)
                _appSettings.ClientSettings.RecentProjects.RemoveAt(10);

            _appSettings.Save(_appSettings);

            return DialogResult.OK;
        }

        public void CreateOpenBotsProject(string mainScriptName, string mainScriptPath)
        {
            //create OpenBots specific project
            UIListView mainScriptActions = NewLstScriptActions(mainScriptName);

            List<ScriptVariable> mainScriptVariables = new List<ScriptVariable>();
            List<ScriptArgument> mainScriptArguments = new List<ScriptArgument>();
            List<ScriptElement> mainScriptElements = new List<ScriptElement>();

            try
            {
                dynamic helloWorldCommand = TypeMethods.CreateTypeInstance(AContainer, "ShowMessageCommand");
                helloWorldCommand.v_Message = "Hello World";
                mainScriptActions.Items.Insert(0, CreateScriptCommandListViewItem(helloWorldCommand));
            }
            catch (Exception)
            {
                var brokenHelloWorldCommand = new BrokenCodeCommentCommand();
                brokenHelloWorldCommand.v_Comment = "Hello World";
                mainScriptActions.Items.Insert(0, CreateScriptCommandListViewItem(brokenHelloWorldCommand));
            }

            //begin saving as main.xml
            ClearSelectedListViewItems();

            try
            {
                //serialize main script
                EngineContext engineContext = new EngineContext
                {
                    Variables = mainScriptVariables,
                    Arguments = mainScriptArguments,
                    Elements = mainScriptElements,
                    FilePath = mainScriptPath,
                    Container = AContainer
                };

                var mainScript = Script.SerializeScript(mainScriptActions.Items, engineContext);

                _mainFileName = ScriptProject.Main;

                OpenOpenBotsFile(mainScriptPath);
                ScriptFilePath = mainScriptPath;               
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }

        public void CreateTextEditorProject(string mainScriptName, string mainScriptPath)
        {
            try
            {
                _mainFileName = ScriptProject.Main;
                switch (ScriptProject.ProjectType)
                {
                    case ProjectType.Python:
                        File.WriteAllText(mainScriptPath, _helloWorldTextPython);
                        File.Create(Path.Combine(new FileInfo(mainScriptPath).Directory.FullName, "requirements.txt"));
                        break;
                    case ProjectType.TagUI:
                        File.WriteAllText(mainScriptPath, _helloWorldTextTagUI);
                        break;
                    case ProjectType.CSScript:
                        File.WriteAllText(mainScriptPath, _helloWorldTextCSScript);
                        break;                       
                }
                
                OpenTextEditorFile(mainScriptPath, ScriptProject.ProjectType);
                ScriptFilePath = mainScriptPath;
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }

        private void OpenProject(string projectPath)
        {
            tvProject.Nodes.Clear();

            DialogResult result = CheckForUnsavedScripts();
            if (result == DialogResult.Cancel)
                return;

            try
            {
                string configPath = Path.Combine(projectPath, "project.obconfig");

                //open project
                Project project = Project.OpenProject(configPath);
                string mainFileName = project.Main;

                string mainFilePath = Directory.GetFiles(projectPath, mainFileName, SearchOption.AllDirectories).FirstOrDefault();
                if (mainFilePath == null)
                    throw new Exception("Main script not found");

                var assemblyList = NugetPackageManager.LoadPackageAssemblies(configPath);
                _builder = AppDomainSetupManager.LoadBuilder(assemblyList, _typeContext.GroupedTypes);
                AContainer = _builder.Build();

                _mainFileName = mainFileName;
                ScriptProject = project;
                ScriptProjectPath = projectPath;
                uiScriptTabControl.TabPages.Clear();

                //open Main
                OpenOpenBotsFile(mainFilePath);

                //show success dialog
                Notify("Project has been opened successfully!", Color.White);
            }
            catch (Exception ex)
            {
                //show fail dialog
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        

            DirectoryInfo projectDirectoryInfo = new DirectoryInfo(ScriptProjectPath);
            TreeNode projectNode = new TreeNode(projectDirectoryInfo.Name);
            projectNode.Text = projectDirectoryInfo.Name;
            projectNode.Tag = projectDirectoryInfo.FullName;
            projectNode.Nodes.Add("Empty");
            projectNode.ContextMenuStrip = cmsProjectMainFolderActions;          
            tvProject.Nodes.Add(projectNode);
            projectNode.Expand();
            LoadCommands(this);

            //save to recent projects 
            if (_appSettings.ClientSettings.RecentProjects == null)
                _appSettings.ClientSettings.RecentProjects = new List<string>();

            if (_appSettings.ClientSettings.RecentProjects.Contains(ScriptProjectPath))
                _appSettings.ClientSettings.RecentProjects.Remove(ScriptProjectPath);

            _appSettings.ClientSettings.RecentProjects.Insert(0, ScriptProjectPath);

            if (_appSettings.ClientSettings.RecentProjects.Count > 10)
                _appSettings.ClientSettings.RecentProjects.RemoveAt(10);

            _appSettings.Save(_appSettings);
        }

        private void LoadChildren(TreeNode parentNode, string directory)
        {
            DirectoryInfo parentDirectoryInfo = new DirectoryInfo(directory);
            try
            {
                foreach (DirectoryInfo childDirectoryInfo in parentDirectoryInfo.GetDirectories())
                {
                    if (childDirectoryInfo.Attributes != FileAttributes.Hidden)
                        NewNode(parentNode, childDirectoryInfo.FullName, "folder");
                }
                foreach (FileInfo fileInfo in parentDirectoryInfo.GetFiles())
                {
                    if (fileInfo.Attributes != FileAttributes.Hidden)
                        NewNode(parentNode, fileInfo.FullName, "file");
                }
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }

        private void NewNode(TreeNode parentNode, string childPath, string type)
        {
            if (type == "folder")
            {
                DirectoryInfo childDirectoryInfo = new DirectoryInfo(childPath);

                TreeNode innerFolderNode = new TreeNode(childDirectoryInfo.Name);
                innerFolderNode.Name = childDirectoryInfo.Name;
                innerFolderNode.Text = childDirectoryInfo.Name;
                innerFolderNode.Tag = childDirectoryInfo.FullName;
                innerFolderNode.Nodes.Add("Empty");
                innerFolderNode.ContextMenuStrip = cmsProjectFolderActions;
                innerFolderNode.ImageIndex = 0; //folder icon
                innerFolderNode.SelectedImageIndex = 0;
                parentNode.Nodes.Add(innerFolderNode);
            }
            else if (type == "file")
            {
                FileInfo childFileInfo = new FileInfo(childPath);

                TreeNode fileNode = new TreeNode(childFileInfo.Name);
                fileNode.Name = childFileInfo.Name;
                fileNode.Text = childFileInfo.Name;
                fileNode.Tag = childFileInfo.FullName;
                
                if (fileNode.Name != "project.obconfig")
                    fileNode.ContextMenuStrip = cmsProjectFileActions;

                string fileExtension = Path.GetExtension(fileNode.Tag.ToString()).ToLower();
                switch (fileExtension)
                {
                    case ".obscript":
                        fileNode.ImageIndex = 1; //script file icon
                        fileNode.SelectedImageIndex = 1;
                        break;
                    case ".xlsx":
                    case ".csv":
                        fileNode.ImageIndex = 3; //excel file icon
                        fileNode.SelectedImageIndex = 3;
                        break;
                    case ".docx":
                        fileNode.ImageIndex = 4; //word file icon
                        fileNode.SelectedImageIndex = 4;
                        break;
                    case ".pdf":
                        fileNode.ImageIndex = 5; //pdf file icon
                        fileNode.SelectedImageIndex = 5;
                        break;
                    case ".py":
                        fileNode.ImageIndex = 6; //python file icon
                        fileNode.SelectedImageIndex = 6;
                        break;
                    case ".tag":
                        fileNode.ImageIndex = 7; //tagUI file icon
                        fileNode.SelectedImageIndex = 7;
                        break;
                    case ".cs":
                        fileNode.ImageIndex = 8; //c-sharp file icon
                        fileNode.SelectedImageIndex = 8;
                        break;
                    default:
                        fileNode.ImageIndex = 2; //default file icon
                        fileNode.SelectedImageIndex = 2;
                        break;
                }               

                parentNode.Nodes.Add(fileNode);
            }
        }
        #endregion

        #region Project TreeView Events
        private void tvProject_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (Directory.Exists(e.Node.Tag.ToString()))
            {
                e.Node.Nodes.Clear();
                LoadChildren(e.Node, e.Node.Tag.ToString());
            }
            else
                e.Cancel = true;
        }

        private void tvProject_DoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (IsScriptRunning)
                return;

            if (e == null || e.Button == MouseButtons.Left)
            {
                try
                {
                    string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                    string currentOpenScriptFilePath = _scriptFilePath;

                    string fileExtension = Path.GetExtension(selectedNodePath).ToLower();
                    if (File.Exists(selectedNodePath))
                    {
                        switch (fileExtension)
                        {
                            case ".obscript":
                                OpenOpenBotsFile(selectedNodePath);
                                break;
                            case ".py":
                                OpenTextEditorFile(selectedNodePath, ProjectType.Python);
                                break;
                            case ".tag":
                                OpenTextEditorFile(selectedNodePath, ProjectType.TagUI);
                                break;
                            case ".cs":
                                OpenTextEditorFile(selectedNodePath, ProjectType.CSScript);
                                break;
                            default:
                                Process.Start(selectedNodePath);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Notify("An Error Occured: " + ex.Message, Color.Red);
                }
            }
        }

        private void tvProject_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tvProject.SelectedNode = e.Node;
        }

        private void tvProject_KeyDown(object sender, KeyEventArgs e)
        {
            string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
            bool isFolder;
            if (Directory.Exists(selectedNodePath))
                isFolder = true;
            else
                isFolder = false;
            if (e.KeyCode == Keys.Delete && isFolder)
                tsmiDeleteFolder_Click(sender, e);
            else if (e.KeyCode == Keys.Delete && !isFolder)
                tsmiDeleteFile_Click(sender, e);
            else if (e.KeyCode == Keys.Enter && !isFolder)
                tvProject_DoubleClick(sender, null);
            else if (e.Control)
            {
                if (e.KeyCode == Keys.C)
                    tsmiCopyFolder_Click(sender, e);
                if (e.KeyCode == Keys.V)
                    tsmiPasteFolder_Click(sender, e);
            }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
        #endregion

        #region Project Folder Context Menu Strip
        private void tsmiCopyFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                Clipboard.SetData(DataFormats.Text, selectedNodePath);
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }

        private void tsmiDeleteFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string selectedNodeName = tvProject.SelectedNode.Text.ToString();
                if (selectedNodeName != ScriptProject.ProjectName)
                {
                    DialogResult result = MessageBox.Show($"Are you sure you would like to delete {selectedNodeName}?",
                                                 $"Delete {selectedNodeName}", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        if (Directory.Exists(selectedNodePath))
                        {
                            Directory.Delete(selectedNodePath, true);
                            tvProject.Nodes.Remove(tvProject.SelectedNode);
                        }
                        else
                            throw new FileNotFoundException();
                    }
                }
                else
                {
                    throw new Exception($"Cannot delete {selectedNodeName}");
                }
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }

        private void tsmiNewFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string newName = "";
                var newNameForm = new frmInputBox("Enter the name of the new folder", "New Folder");
                newNameForm.ShowDialog();

                if (newNameForm.DialogResult == DialogResult.OK)
                {
                    newName = newNameForm.txtInput.Text;
                    newNameForm.Dispose();
                }
                else if (newNameForm.DialogResult == DialogResult.Cancel)
                {
                    newNameForm.Dispose();
                    return;
                }

                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string newFolderPath = Path.Combine(selectedNodePath, newName);

                if (!Directory.Exists(newFolderPath))
                {
                    Directory.CreateDirectory(newFolderPath);
                    DirectoryInfo newDirectoryInfo = new DirectoryInfo(newFolderPath);
                    NewNode(tvProject.SelectedNode, newFolderPath, "folder");
                }
                else
                {
                    int count = 1;
                    string newerFolderPath = newFolderPath;
                    while (Directory.Exists(newerFolderPath))
                    {
                        newerFolderPath = $"{newFolderPath} ({count})";
                        count += 1;
                    }
                    Directory.CreateDirectory(newerFolderPath);
                    DirectoryInfo newDirectoryInfo = new DirectoryInfo(newerFolderPath);

                    NewNode(tvProject.SelectedNode, newerFolderPath, "folder");
                }
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }        

        private void tsmiPasteFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string copiedNodePath = Clipboard.GetData(DataFormats.Text).ToString();

                if (Directory.Exists(copiedNodePath))
                {
                    DirectoryInfo copiedNodeDirectoryInfo = new DirectoryInfo(copiedNodePath);

                    if (Directory.Exists(Path.Combine(selectedNodePath, copiedNodeDirectoryInfo.Name)))
                        throw new Exception("A directory with this name already exists in this location");

                    else if (copiedNodePath == ScriptProjectPath)
                        throw new Exception("The project directory cannot be copied or moved");

                    else
                    {
                        VBFileSystem.CopyDirectory(copiedNodePath, Path.Combine(selectedNodePath, copiedNodeDirectoryInfo.Name));
                        NewNode(tvProject.SelectedNode, copiedNodePath, "folder");
                    }
                }
                else if (File.Exists(copiedNodePath))
                {
                    FileInfo copiedNodeFileInfo = new FileInfo(copiedNodePath);

                    if (File.Exists(Path.Combine(selectedNodePath, copiedNodeFileInfo.Name)))
                        throw new Exception("A file with this name already exists in this location");

                    else if (copiedNodeFileInfo.Name == "project.obconfig")
                        throw new Exception("This file cannot be copied or moved");

                    else
                    {
                        File.Copy(copiedNodePath, Path.Combine(selectedNodePath, copiedNodeFileInfo.Name));
                        NewNode(tvProject.SelectedNode, copiedNodePath, "file");
                    }
                }
                else
                    throw new Exception("Attempted to paste something that isn't a file or folder");

            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }

        private void tsmiRenameFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                
                DirectoryInfo selectedNodeDirectoryInfo = new DirectoryInfo(selectedNodePath);

                string newName = "";

                string prompt;
                string title;

                if (selectedNodePath == ScriptProjectPath)
                {
                    prompt = "Enter the new name of the project";
                    title = "Rename Project";
                }
                else
                {
                    prompt = "Enter the new name of the folder";
                    title = "Rename Folder";
                }

                var newNameForm = new frmInputBox(prompt, title);
                newNameForm.txtInput.Text = tvProject.SelectedNode.Name;
                newNameForm.ShowDialog();

                if (newNameForm.DialogResult == DialogResult.OK)
                {
                    newName = newNameForm.txtInput.Text;
                    newNameForm.Dispose();
                }
                else if (newNameForm.DialogResult == DialogResult.Cancel)
                {
                    newNameForm.Dispose();
                    return;
                }

                string newPath = Path.Combine(selectedNodeDirectoryInfo.Parent.FullName, newName);
                bool isInvalidProjectName = new[] { @"/", @"\" }.Any(c => newName.Contains(c));

                if (isInvalidProjectName)
                    throw new Exception("Illegal characters in path");

                if (Directory.Exists(newPath))
                    throw new Exception("A folder with this name already exists");

                if (CloseAllFiles())
                {
                    FileSystem.Rename(selectedNodePath, newPath);
                    tvProject.SelectedNode.Name = newName;
                    tvProject.SelectedNode.Text = newName;
                    tvProject.SelectedNode.Tag = newPath;
                    tvProject.SelectedNode.Collapse();
                    tvProject.SelectedNode.Expand();

                    if (selectedNodePath == ScriptProjectPath)
                    {
                        ScriptProject.ProjectName = newName;
                        ScriptProjectPath = newPath;
                        Project.RenameProject(ScriptProject, ScriptProjectPath);
                        _appSettings.ClientSettings.RecentProjects.RemoveAt(0);
                        _appSettings.ClientSettings.RecentProjects.Insert(0, ScriptProjectPath);
                        _appSettings.Save(_appSettings);
                    }

                    string mainFilePath = Path.Combine(ScriptProjectPath, ScriptProject.Main);
                    OpenOpenBotsFile(mainFilePath);
                }               
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }

        private void tsmiNewScriptFile_Click(object sender, EventArgs e)
        {
            try
            {               
                string newName = "";
                var newNameForm = new frmInputBox("Enter the name of the new file WITH extension", "New File");
                newNameForm.ShowDialog();

                if (newNameForm.DialogResult == DialogResult.OK)
                {
                    newName = newNameForm.txtInput.Text;
                    newNameForm.Dispose();
                }
                else if (newNameForm.DialogResult == DialogResult.Cancel)
                {
                    newNameForm.Dispose();
                    return;
                }

                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string newFilePath = Path.Combine(selectedNodePath, newName);
                string extension = Path.GetExtension(newFilePath);

                if (File.Exists(newFilePath))
                {
                    int count = 1;
                    string newerFilePath = newFilePath;
                    while (File.Exists(newerFilePath))
                    {
                        string newDirectoryPath = Path.GetDirectoryName(newFilePath);
                        string newFileNameWithoutExtension = Path.GetFileNameWithoutExtension(newFilePath);
                        newerFilePath = Path.Combine(newDirectoryPath, $"{newFileNameWithoutExtension} ({count}){extension}");
                        count += 1;
                    }

                    newFilePath = newerFilePath;
                }

                switch (extension.ToLower())
                {
                    case ".obscript":
                        UIListView newScriptActions = NewLstScriptActions();
                        List<ScriptVariable> newScriptVariables = new List<ScriptVariable>();
                        List<ScriptArgument> newScriptArguments = new List<ScriptArgument>();
                        List<ScriptElement> newScriptElements = new List<ScriptElement>();

                        try
                        {
                            dynamic helloWorldCommand = TypeMethods.CreateTypeInstance(AContainer, "ShowMessageCommand");
                            helloWorldCommand.v_Message = "Hello World";
                            newScriptActions.Items.Insert(0, CreateScriptCommandListViewItem(helloWorldCommand));
                        }
                        catch (Exception)
                        {
                            var brokenHelloWorldCommand = new BrokenCodeCommentCommand();
                            brokenHelloWorldCommand.v_Comment = "Hello World";
                            newScriptActions.Items.Insert(0, CreateScriptCommandListViewItem(brokenHelloWorldCommand));
                        }

                        EngineContext engineContext = new EngineContext
                        {
                            Variables = newScriptVariables,
                            Arguments = newScriptArguments,
                            Elements = newScriptElements,
                            FilePath = newFilePath,
                            Container = AContainer
                        };

                        Script.SerializeScript(newScriptActions.Items, engineContext);
                        NewNode(tvProject.SelectedNode, newFilePath, "file");
                        OpenOpenBotsFile(newFilePath);
                        break;
                    case ".py":
                        File.WriteAllText(newFilePath, _helloWorldTextPython);
                        NewNode(tvProject.SelectedNode, newFilePath, "file");
                        OpenTextEditorFile(newFilePath, ProjectType.Python);
                        break;
                    case ".tag":
                        File.WriteAllText(newFilePath, _helloWorldTextTagUI);
                        NewNode(tvProject.SelectedNode, newFilePath, "file");
                        OpenTextEditorFile(newFilePath, ProjectType.TagUI);
                        break;
                    case ".cs":
                        File.WriteAllText(newFilePath, _helloWorldTextCSScript);
                        NewNode(tvProject.SelectedNode, newFilePath, "file");
                        OpenTextEditorFile(newFilePath, ProjectType.CSScript);
                        break;
                    default:
                        File.Create(newFilePath);
                        NewNode(tvProject.SelectedNode, newFilePath, "file");
                        return;
                }
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }
        #endregion

        #region Project File Context Menu Strip
        private void tsmiCopyFile_Click(object sender, EventArgs e)
        {
            tsmiCopyFolder_Click(sender, e);
        }

        private void tsmiDeleteFile_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string selectedNodeName = tvProject.SelectedNode.Text.ToString();
                if (selectedNodeName != "project.obconfig")
                {
                    var result = MessageBox.Show($"Are you sure you would like to delete {selectedNodeName}?",
                                             $"Delete {selectedNodeName}", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        if (File.Exists(selectedNodePath))
                        {
                            string selectedFileName = Path.GetFileNameWithoutExtension(selectedNodePath);
                            File.Delete(selectedNodePath);
                            tvProject.Nodes.Remove(tvProject.SelectedNode);
                            var foundTab = uiScriptTabControl.TabPages.Cast<TabPage>()
                                                                      .Where(t => t.ToolTipText == selectedNodePath)
                                                                      .FirstOrDefault();
                            if (foundTab != null)
                                uiScriptTabControl.TabPages.Remove(foundTab);
                        }
                        else
                            throw new FileNotFoundException();
                    }
                }
                else
                    throw new Exception($"Cannot delete {selectedNodeName}");
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }

        private void tsmiRenameFile_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string selectedNodeName = tvProject.SelectedNode.Text.ToString();
                string selectedNodeNameWithoutExtension = Path.GetFileNameWithoutExtension(selectedNodeName);

                if (selectedNodeName != "project.obconfig")
                {
                    FileInfo selectedNodeDirectoryInfo = new FileInfo(selectedNodePath);

                    string newName = "";
                    var newNameForm = new frmInputBox("Enter the new name of the file WITH extension", "Rename File");
                    newNameForm.txtInput.Text = selectedNodeDirectoryInfo.Name;
                    newNameForm.ShowDialog();

                    if (newNameForm.DialogResult == DialogResult.OK)
                    {
                        newName = newNameForm.txtInput.Text;
                        newNameForm.Dispose();
                    }
                    else if (newNameForm.DialogResult == DialogResult.Cancel)
                    {
                        newNameForm.Dispose();
                        return;
                    }

                    string newPath = Path.Combine(selectedNodeDirectoryInfo.DirectoryName, newName);

                    bool isInvalidProjectName = new[] { @"/", @"\" }.Any(c => newName.Contains(c));
                    if (isInvalidProjectName)
                        throw new Exception("Illegal characters in path");

                    if (File.Exists(newPath))
                        throw new Exception("A file with this name already exists");

                    var foundTab = uiScriptTabControl.TabPages.Cast<TabPage>().Where(t => t.ToolTipText == selectedNodePath)
                                                                          .FirstOrDefault();

                    if (foundTab != null)
                    {
                        DialogResult result = CheckForUnsavedScript(foundTab);
                        if (result == DialogResult.Cancel)
                            return;

                        uiScriptTabControl.TabPages.Remove(foundTab);
                    }

                    FileSystem.Rename(selectedNodePath, newPath);

                    if (selectedNodeName == _mainFileName)
                    {
                        string newMainName = Path.GetFileName(newPath);
                        _mainFileName = newMainName;
                        ScriptProject.Main = newMainName;
                        ScriptProject.SaveProject(newPath);
                    }

                    tvProject.SelectedNode.Name = newName;
                    tvProject.SelectedNode.Text = newName;
                    tvProject.SelectedNode.Tag = newPath;                   
                }
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }
        #endregion

        #region Project Pane Buttons
        private void uiBtnRefresh_Click(object sender, EventArgs e)
        {
            tvProject.CollapseAll();
            tvProject.TopNode.Expand();
        }

        private void uiBtnExpand_Click(object sender, EventArgs e)
        {
            tvProject.ExpandAll();
        }

        private void uiBtnCollapse_Click(object sender, EventArgs e)
        {
            tvProject.CollapseAll();
        }

        private void uiBtnOpenDirectory_Click(object sender, EventArgs e)
        {
            Process.Start(ScriptProjectPath);
        }
        #endregion
    }
}
