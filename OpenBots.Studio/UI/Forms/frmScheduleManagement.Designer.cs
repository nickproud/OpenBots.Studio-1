namespace OpenBots.UI.Forms
{
    partial class frmScheduleManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScheduleManagement));
            this.txtAppPath = new System.Windows.Forms.TextBox();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.lblScheduledScripts = new System.Windows.Forms.Label();
            this.lblAppPath = new System.Windows.Forms.Label();
            this.lblScriptName = new System.Windows.Forms.Label();
            this.dgvScheduledTasks = new System.Windows.Forms.DataGridView();
            this.colTaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskLastRun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskLastResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskNextRunTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChangeTaskStatus = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDeleteTask = new System.Windows.Forms.DataGridViewButtonColumn();
            this.bgwGetSchedulingInfo = new System.ComponentModel.BackgroundWorker();
            this.tmrGetSchedulingInfo = new System.Windows.Forms.Timer(this.components);
            this.uiBtnShowScheduleManager = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnOk = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboRecurType = new System.Windows.Forms.ComboBox();
            this.lblRecurrence = new System.Windows.Forms.Label();
            this.txtRecurCount = new System.Windows.Forms.TextBox();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.rdoDoNotEnd = new System.Windows.Forms.RadioButton();
            this.rdoEndByDate = new System.Windows.Forms.RadioButton();
            this.dtEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtStartTime = new System.Windows.Forms.DateTimePicker();
            this.lblAddNewSchedule = new System.Windows.Forms.Label();
            this.btnFolderManagerProject = new System.Windows.Forms.Button();
            this.txtProjectPath = new System.Windows.Forms.TextBox();
            this.btnFileManagerStudio = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScheduledTasks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnShowScheduleManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtAppPath
            // 
            this.txtAppPath.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAppPath.Location = new System.Drawing.Point(14, 369);
            this.txtAppPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAppPath.Name = "txtAppPath";
            this.txtAppPath.Size = new System.Drawing.Size(870, 31);
            this.txtAppPath.TabIndex = 0;
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(11, 6);
            this.lblMainLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(225, 55);
            this.lblMainLogo.TabIndex = 4;
            this.lblMainLogo.Text = "schedule";
            // 
            // lblScheduledScripts
            // 
            this.lblScheduledScripts.AutoSize = true;
            this.lblScheduledScripts.BackColor = System.Drawing.Color.Transparent;
            this.lblScheduledScripts.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScheduledScripts.ForeColor = System.Drawing.Color.AliceBlue;
            this.lblScheduledScripts.Location = new System.Drawing.Point(11, 55);
            this.lblScheduledScripts.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScheduledScripts.Name = "lblScheduledScripts";
            this.lblScheduledScripts.Size = new System.Drawing.Size(345, 32);
            this.lblScheduledScripts.TabIndex = 6;
            this.lblScheduledScripts.Text = "Currently Scheduled Projects";
            // 
            // lblAppPath
            // 
            this.lblAppPath.AutoSize = true;
            this.lblAppPath.BackColor = System.Drawing.Color.Transparent;
            this.lblAppPath.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppPath.ForeColor = System.Drawing.Color.AliceBlue;
            this.lblAppPath.Location = new System.Drawing.Point(11, 338);
            this.lblAppPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAppPath.Name = "lblAppPath";
            this.lblAppPath.Size = new System.Drawing.Size(454, 28);
            this.lblAppPath.TabIndex = 8;
            this.lblAppPath.Text = "OpenBots.Studio.exe location (executing assembly)";
            // 
            // lblScriptName
            // 
            this.lblScriptName.AutoSize = true;
            this.lblScriptName.BackColor = System.Drawing.Color.Transparent;
            this.lblScriptName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScriptName.ForeColor = System.Drawing.Color.White;
            this.lblScriptName.Location = new System.Drawing.Point(11, 412);
            this.lblScriptName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScriptName.Name = "lblScriptName";
            this.lblScriptName.Size = new System.Drawing.Size(149, 28);
            this.lblScriptName.TabIndex = 9;
            this.lblScriptName.Text = "Project location";
            // 
            // dgvScheduledTasks
            // 
            this.dgvScheduledTasks.AllowUserToAddRows = false;
            this.dgvScheduledTasks.AllowUserToDeleteRows = false;
            this.dgvScheduledTasks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvScheduledTasks.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvScheduledTasks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvScheduledTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScheduledTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTaskName,
            this.colTaskLastRun,
            this.colTaskLastResult,
            this.colTaskNextRunTime,
            this.colTaskState,
            this.colChangeTaskStatus,
            this.colDeleteTask});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvScheduledTasks.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvScheduledTasks.Location = new System.Drawing.Point(11, 94);
            this.dgvScheduledTasks.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvScheduledTasks.Name = "dgvScheduledTasks";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvScheduledTasks.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvScheduledTasks.RowHeadersWidth = 51;
            this.dgvScheduledTasks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvScheduledTasks.Size = new System.Drawing.Size(918, 204);
            this.dgvScheduledTasks.TabIndex = 11;
            this.dgvScheduledTasks.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvScheduledTasks_CellContentClick);
            // 
            // colTaskName
            // 
            this.colTaskName.HeaderText = "Task Name";
            this.colTaskName.MinimumWidth = 6;
            this.colTaskName.Name = "colTaskName";
            this.colTaskName.ReadOnly = true;
            this.colTaskName.Width = 138;
            // 
            // colTaskLastRun
            // 
            this.colTaskLastRun.HeaderText = "Last Run";
            this.colTaskLastRun.MinimumWidth = 6;
            this.colTaskLastRun.Name = "colTaskLastRun";
            this.colTaskLastRun.ReadOnly = true;
            this.colTaskLastRun.Width = 118;
            // 
            // colTaskLastResult
            // 
            this.colTaskLastResult.HeaderText = "Last Result";
            this.colTaskLastResult.MinimumWidth = 6;
            this.colTaskLastResult.Name = "colTaskLastResult";
            this.colTaskLastResult.ReadOnly = true;
            this.colTaskLastResult.Width = 136;
            // 
            // colTaskNextRunTime
            // 
            this.colTaskNextRunTime.HeaderText = "Next Run";
            this.colTaskNextRunTime.MinimumWidth = 6;
            this.colTaskNextRunTime.Name = "colTaskNextRunTime";
            this.colTaskNextRunTime.ReadOnly = true;
            this.colTaskNextRunTime.Width = 121;
            // 
            // colTaskState
            // 
            this.colTaskState.HeaderText = "Active";
            this.colTaskState.MinimumWidth = 6;
            this.colTaskState.Name = "colTaskState";
            this.colTaskState.ReadOnly = true;
            this.colTaskState.Width = 95;
            // 
            // colChangeTaskStatus
            // 
            this.colChangeTaskStatus.HeaderText = "Update";
            this.colChangeTaskStatus.MinimumWidth = 6;
            this.colChangeTaskStatus.Name = "colChangeTaskStatus";
            this.colChangeTaskStatus.ReadOnly = true;
            this.colChangeTaskStatus.Width = 74;
            // 
            // colDeleteTask
            // 
            this.colDeleteTask.HeaderText = "Delete";
            this.colDeleteTask.MinimumWidth = 6;
            this.colDeleteTask.Name = "colDeleteTask";
            this.colDeleteTask.ReadOnly = true;
            this.colDeleteTask.Width = 68;
            // 
            // bgwGetSchedulingInfo
            // 
            this.bgwGetSchedulingInfo.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwGetSchedulingInfo_DoWork);
            this.bgwGetSchedulingInfo.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwGetSchedulingInfo_RunWorkerCompleted);
            // 
            // tmrGetSchedulingInfo
            // 
            this.tmrGetSchedulingInfo.Enabled = true;
            this.tmrGetSchedulingInfo.Interval = 1000;
            this.tmrGetSchedulingInfo.Tick += new System.EventHandler(this.tmrGetSchedulingInfo_Tick);
            // 
            // uiBtnShowScheduleManager
            // 
            this.uiBtnShowScheduleManager.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnShowScheduleManager.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnShowScheduleManager.DisplayText = "Scheduler";
            this.uiBtnShowScheduleManager.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnShowScheduleManager.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.uiBtnShowScheduleManager.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnShowScheduleManager.Image")));
            this.uiBtnShowScheduleManager.IsMouseOver = false;
            this.uiBtnShowScheduleManager.Location = new System.Drawing.Point(850, 15);
            this.uiBtnShowScheduleManager.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiBtnShowScheduleManager.Name = "uiBtnShowScheduleManager";
            this.uiBtnShowScheduleManager.Size = new System.Drawing.Size(81, 75);
            this.uiBtnShowScheduleManager.TabIndex = 14;
            this.uiBtnShowScheduleManager.TabStop = false;
            this.uiBtnShowScheduleManager.Text = "Scheduler";
            this.uiBtnShowScheduleManager.Click += new System.EventHandler(this.uiBtnShowScheduleManager_Click);
            // 
            // uiBtnOk
            // 
            this.uiBtnOk.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOk.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOk.DisplayText = "Add";
            this.uiBtnOk.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.uiBtnOk.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOk.Image")));
            this.uiBtnOk.IsMouseOver = false;
            this.uiBtnOk.Location = new System.Drawing.Point(11, 658);
            this.uiBtnOk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiBtnOk.Name = "uiBtnOk";
            this.uiBtnOk.Size = new System.Drawing.Size(68, 75);
            this.uiBtnOk.TabIndex = 1;
            this.uiBtnOk.TabStop = false;
            this.uiBtnOk.Text = "Add";
            this.uiBtnOk.Click += new System.EventHandler(this.uiBtnOk_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.cboRecurType);
            this.panel1.Controls.Add(this.lblRecurrence);
            this.panel1.Controls.Add(this.txtRecurCount);
            this.panel1.Controls.Add(this.lblStartDate);
            this.panel1.Controls.Add(this.rdoDoNotEnd);
            this.panel1.Controls.Add(this.rdoEndByDate);
            this.panel1.Controls.Add(this.dtEndTime);
            this.panel1.Controls.Add(this.dtStartTime);
            this.panel1.Location = new System.Drawing.Point(4, 489);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(655, 162);
            this.panel1.TabIndex = 15;
            // 
            // cboRecurType
            // 
            this.cboRecurType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRecurType.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboRecurType.FormattingEnabled = true;
            this.cboRecurType.Items.AddRange(new object[] {
            "Minute(s)",
            "Hour(s)",
            "Day(s)"});
            this.cboRecurType.Location = new System.Drawing.Point(89, 108);
            this.cboRecurType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboRecurType.Name = "cboRecurType";
            this.cboRecurType.Size = new System.Drawing.Size(235, 33);
            this.cboRecurType.TabIndex = 7;
            // 
            // lblRecurrence
            // 
            this.lblRecurrence.AutoSize = true;
            this.lblRecurrence.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecurrence.ForeColor = System.Drawing.Color.White;
            this.lblRecurrence.Location = new System.Drawing.Point(6, 75);
            this.lblRecurrence.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRecurrence.Name = "lblRecurrence";
            this.lblRecurrence.Size = new System.Drawing.Size(107, 28);
            this.lblRecurrence.TabIndex = 6;
            this.lblRecurrence.Text = "Recurrence";
            // 
            // txtRecurCount
            // 
            this.txtRecurCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtRecurCount.Location = new System.Drawing.Point(10, 109);
            this.txtRecurCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtRecurCount.Name = "txtRecurCount";
            this.txtRecurCount.Size = new System.Drawing.Size(72, 31);
            this.txtRecurCount.TabIndex = 5;
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartDate.ForeColor = System.Drawing.Color.White;
            this.lblStartDate.Location = new System.Drawing.Point(9, 2);
            this.lblStartDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(99, 28);
            this.lblStartDate.TabIndex = 4;
            this.lblStartDate.Text = "Start Date";
            // 
            // rdoDoNotEnd
            // 
            this.rdoDoNotEnd.AutoSize = true;
            this.rdoDoNotEnd.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.rdoDoNotEnd.ForeColor = System.Drawing.Color.White;
            this.rdoDoNotEnd.Location = new System.Drawing.Point(501, 2);
            this.rdoDoNotEnd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdoDoNotEnd.Name = "rdoDoNotEnd";
            this.rdoDoNotEnd.Size = new System.Drawing.Size(140, 32);
            this.rdoDoNotEnd.TabIndex = 3;
            this.rdoDoNotEnd.Text = "Do Not End";
            this.rdoDoNotEnd.UseVisualStyleBackColor = true;
            // 
            // rdoEndByDate
            // 
            this.rdoEndByDate.AutoSize = true;
            this.rdoEndByDate.Checked = true;
            this.rdoEndByDate.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.rdoEndByDate.ForeColor = System.Drawing.Color.White;
            this.rdoEndByDate.Location = new System.Drawing.Point(256, 2);
            this.rdoEndByDate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdoEndByDate.Name = "rdoEndByDate";
            this.rdoEndByDate.Size = new System.Drawing.Size(142, 32);
            this.rdoEndByDate.TabIndex = 2;
            this.rdoEndByDate.TabStop = true;
            this.rdoEndByDate.Text = "End By Date";
            this.rdoEndByDate.UseVisualStyleBackColor = true;
            // 
            // dtEndTime
            // 
            this.dtEndTime.CalendarFont = new System.Drawing.Font("Segoe UI", 9F);
            this.dtEndTime.CustomFormat = "MM/dd/yyyy hh:mm:ss";
            this.dtEndTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtEndTime.Location = new System.Drawing.Point(256, 36);
            this.dtEndTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtEndTime.Name = "dtEndTime";
            this.dtEndTime.Size = new System.Drawing.Size(230, 31);
            this.dtEndTime.TabIndex = 1;
            // 
            // dtStartTime
            // 
            this.dtStartTime.CalendarFont = new System.Drawing.Font("Segoe UI", 9F);
            this.dtStartTime.CustomFormat = "MM/dd/yyyy hh:mm:ss";
            this.dtStartTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStartTime.Location = new System.Drawing.Point(9, 36);
            this.dtStartTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtStartTime.Name = "dtStartTime";
            this.dtStartTime.Size = new System.Drawing.Size(230, 31);
            this.dtStartTime.TabIndex = 0;
            // 
            // lblAddNewSchedule
            // 
            this.lblAddNewSchedule.AutoSize = true;
            this.lblAddNewSchedule.BackColor = System.Drawing.Color.Transparent;
            this.lblAddNewSchedule.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddNewSchedule.ForeColor = System.Drawing.Color.AliceBlue;
            this.lblAddNewSchedule.Location = new System.Drawing.Point(11, 302);
            this.lblAddNewSchedule.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddNewSchedule.Name = "lblAddNewSchedule";
            this.lblAddNewSchedule.Size = new System.Drawing.Size(229, 32);
            this.lblAddNewSchedule.TabIndex = 16;
            this.lblAddNewSchedule.Text = "Add New Schedule";
            // 
            // btnFolderManagerProject
            // 
            this.btnFolderManagerProject.Location = new System.Drawing.Point(892, 439);
            this.btnFolderManagerProject.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFolderManagerProject.Name = "btnFolderManagerProject";
            this.btnFolderManagerProject.Size = new System.Drawing.Size(36, 40);
            this.btnFolderManagerProject.TabIndex = 27;
            this.btnFolderManagerProject.Text = "...";
            this.btnFolderManagerProject.UseVisualStyleBackColor = true;
            this.btnFolderManagerProject.Click += new System.EventHandler(this.btnFolderManagerProject_Click);
            // 
            // txtProjectPath
            // 
            this.txtProjectPath.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtProjectPath.Location = new System.Drawing.Point(14, 444);
            this.txtProjectPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtProjectPath.Name = "txtProjectPath";
            this.txtProjectPath.Size = new System.Drawing.Size(870, 31);
            this.txtProjectPath.TabIndex = 28;
            // 
            // btnFileManagerStudio
            // 
            this.btnFileManagerStudio.Location = new System.Drawing.Point(892, 366);
            this.btnFileManagerStudio.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFileManagerStudio.Name = "btnFileManagerStudio";
            this.btnFileManagerStudio.Size = new System.Drawing.Size(36, 40);
            this.btnFileManagerStudio.TabIndex = 29;
            this.btnFileManagerStudio.Text = "...";
            this.btnFileManagerStudio.UseVisualStyleBackColor = true;
            this.btnFileManagerStudio.Click += new System.EventHandler(this.btnFileManagerStudio_Click);
            // 
            // frmScheduleManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 745);
            this.Controls.Add(this.btnFileManagerStudio);
            this.Controls.Add(this.txtProjectPath);
            this.Controls.Add(this.btnFolderManagerProject);
            this.Controls.Add(this.lblAddNewSchedule);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.uiBtnShowScheduleManager);
            this.Controls.Add(this.dgvScheduledTasks);
            this.Controls.Add(this.lblScriptName);
            this.Controls.Add(this.lblAppPath);
            this.Controls.Add(this.lblScheduledScripts);
            this.Controls.Add(this.lblMainLogo);
            this.Controls.Add(this.uiBtnOk);
            this.Controls.Add(this.txtAppPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmScheduleManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Schedule";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmScheduleManagement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvScheduledTasks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnShowScheduleManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAppPath;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnOk;
        private System.Windows.Forms.Label lblMainLogo;
        private System.Windows.Forms.Label lblScheduledScripts;
        private System.Windows.Forms.Label lblAppPath;
        private System.Windows.Forms.Label lblScriptName;
        private System.Windows.Forms.DataGridView dgvScheduledTasks;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskLastRun;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskLastResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskNextRunTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskState;
        private System.Windows.Forms.DataGridViewButtonColumn colChangeTaskStatus;
        private System.Windows.Forms.DataGridViewButtonColumn colDeleteTask;
        private System.ComponentModel.BackgroundWorker bgwGetSchedulingInfo;
        private System.Windows.Forms.Timer tmrGetSchedulingInfo;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnShowScheduleManager;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtStartTime;
        private System.Windows.Forms.RadioButton rdoEndByDate;
        private System.Windows.Forms.DateTimePicker dtEndTime;
        private System.Windows.Forms.RadioButton rdoDoNotEnd;
        private System.Windows.Forms.ComboBox cboRecurType;
        private System.Windows.Forms.Label lblRecurrence;
        private System.Windows.Forms.TextBox txtRecurCount;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Label lblAddNewSchedule;
        private System.Windows.Forms.Button btnFolderManagerProject;
        private System.Windows.Forms.TextBox txtProjectPath;
        private System.Windows.Forms.Button btnFileManagerStudio;
    }
}