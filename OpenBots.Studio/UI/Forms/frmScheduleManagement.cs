//Copyright (c) 2019 Jason Bayldon
//Modifications - Copyright (c) 2020 OpenBots Inc.
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using Microsoft.Win32.TaskScheduler;
using OpenBots.Core.Project;
using OpenBots.Core.UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace OpenBots.UI.Forms
{
    public partial class frmScheduleManagement : UIForm
    {
        public frmScheduleManagement()
        {
            InitializeComponent();
        }

        #region Form and Control Events

        private void frmScheduleManagement_Load(object sender, EventArgs e)
        {
            //get path to executing assembly
            txtAppPath.Text = Assembly.GetEntryAssembly().Location;

            //set autosize mode
            colTaskName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //call bgw to pull schedule info
            RefreshTasks();
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {          
            if (string.IsNullOrEmpty(txtProjectPath.Text))
            {
                MessageBox.Show("Please select a project!");
                return;
            }

            string selectedFile = Path.Combine(txtProjectPath.Text, "project.config");
            if (!File.Exists(selectedFile))
            {
                MessageBox.Show("'project.config' file not found!");
                return;
            }

            if (string.IsNullOrEmpty(txtRecurCount.Text))
            {
                MessageBox.Show("Please indicate a recurrence value!");
                return;
            }

            if (string.IsNullOrEmpty(cboRecurType.Text))
            {
                MessageBox.Show("Please select a recurrence frequency!");
                return;
            }

            Project scriptProject = Project.OpenProject(Path.Combine(txtProjectPath.Text, "project.config"));          

            using (TaskService ts = new TaskService())
            {
                try
                {
                    // Create a new task definition and assign properties
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = "Scheduled task from OpenBots Studio - " + scriptProject.ProjectName;
                    var trigger = new TimeTrigger();
                    ////   // Add a trigger that will fire the task at this time every other day
                    //DailyTrigger dt = (DailyTrigger)td.Triggers.Add(new DailyTrigger(2));
                    //dt.Repetition.Duration = TimeSpan.FromHours(4);
                    //dt.Repetition.Interval = TimeSpan.FromHours()
                    // Create a trigger that will execute very 2 minutes.

                    trigger.StartBoundary = dtStartTime.Value;
                    if (rdoEndByDate.Checked)
                    {
                        trigger.EndBoundary = dtEndTime.Value;
                    }

                    double recurParsed;

                    if (!double.TryParse(txtRecurCount.Text, out recurParsed))
                    {
                        MessageBox.Show("Recur value must be a number type (double)!");
                        return;
                    }

                    switch (cboRecurType.Text)
                    {
                        case "Minute(s)":
                            trigger.Repetition.Interval = TimeSpan.FromMinutes(recurParsed);
                            break;

                        case "Hour(s)":
                            trigger.Repetition.Interval = TimeSpan.FromHours(recurParsed);
                            break;

                        case "Day(s)":
                            trigger.Repetition.Interval = TimeSpan.FromDays(recurParsed);
                            break;

                        default:
                            break;
                    }               

                    if (trigger.Repetition.Interval < new TimeSpan(0, 1, 0) || trigger.Repetition.Interval > new TimeSpan(31,0,0,0))
                    {
                        MessageBox.Show("Recurrence interval must be between 1 minute and 31 days");
                        return;
                    }

                    td.Triggers.Add(trigger);

                    td.Actions.Add(new ExecAction(@"" + txtAppPath.Text + "", "\"" + selectedFile + "\"", null));
                    ts.RootFolder.RegisterTaskDefinition(@"OpenBots-" + scriptProject.ProjectName, td);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void uiBtnShowScheduleManager_Click(object sender, EventArgs e)
        {
            using (TaskService ts = new TaskService())
                ts.StartSystemTaskSchedulerManager();
        }

        private void dgvScheduledTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvScheduledTasks.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
                {
                    var buttonColumn = (DataGridViewButtonColumn)dgvScheduledTasks.Columns[e.ColumnIndex];
                    if (buttonColumn.HeaderText == "Update")
                    {
                        int row = dgvScheduledTasks.CurrentCell.RowIndex;
                        using (TaskService ts = new TaskService())
                        {
                            string taskName = (string)dgvScheduledTasks.Rows[row].Cells["colTaskName"].Value;
                            var updateTask = ts.FindTask(taskName);
                            updateTask.Enabled = !updateTask.Enabled;
                        }
                    }
                    else if (buttonColumn.HeaderText == "Delete")
                    {
                        int row = dgvScheduledTasks.CurrentCell.RowIndex;
                        using (TaskService ts = new TaskService())
                        {
                            string taskName = (string)dgvScheduledTasks.Rows[row].Cells["colTaskName"].Value;
                            ts.RootFolder.DeleteTask(taskName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnFolderManagerProject_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtProjectPath.Text = fbd.SelectedPath;
                txtProjectPath.Focus();
            }
        }

        private void btnFileManagerStudio_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = new DirectoryInfo(Assembly.GetEntryAssembly().Location).Parent.FullName;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtAppPath.Text = ofd.FileName;
                txtAppPath.Focus();
            }
        }
        #endregion Form and Control Events

        #region BackgroundWorker, Timer

        //events for background worker and associated methods
        private void RefreshTasks()
        {
            bgwGetSchedulingInfo.RunWorkerAsync();
        }

        private void tmrGetSchedulingInfo_Tick(object sender, EventArgs e)
        {
            if (!bgwGetSchedulingInfo.IsBusy)
                bgwGetSchedulingInfo.RunWorkerAsync();
        }

        private void bgwGetSchedulingInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<object[]> scheduledTaskList = new List<object[]>();

                using (TaskService ts = new TaskService())
                {
                    foreach (Task task in ts.RootFolder.Tasks)
                    {
                        if (task.Name.Contains("OpenBots"))
                        {
                            string currentState = "enable";
                            if (task.Enabled)
                                currentState = "disable";

                            var scheduleItem = new object[]
                            {
                            task.Name,
                            task.LastRunTime,
                            task.LastTaskResult,
                            task.NextRunTime,
                            task.IsActive,
                            currentState,
                            "delete"
                            };
                            scheduledTaskList.Add(scheduleItem);
                        }
                    }
                }

                e.Result = scheduledTaskList;
            }           
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void bgwGetSchedulingInfo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                dgvScheduledTasks.Rows.Clear();
                List<object[]> datagridRows = (List<object[]>)e.Result;
                datagridRows.ForEach(itm => dgvScheduledTasks.Rows.Add(itm[0], itm[1], itm[2], itm[3], itm[4], itm[5], itm[6]));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        #endregion BackgroundWorker, Timer       
    }
}