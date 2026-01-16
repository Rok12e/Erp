namespace YamyProject.UI.Construction.ProjectPlanning
{
    partial class frmProjectTimeLine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProjectTimeLine));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.iconsList = new System.Windows.Forms.ImageList(this.components);
            this.markersList = new System.Windows.Forms.ImageList(this.components);
            this.timerCurrentCellChanged = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.resourceLoadChartView = new DlhSoft.ProjectDataControlLibrary.ResourceLoad.ResourceLoadChartView();
            this.resourceLoadChartViewResourceLoadScaleHeader = new DlhSoft.ProjectDataControlLibrary.ResourceLoad.ResourceLoadScaleHeader();
            this.resourceLoadChartViewResourceLoadScaleArea = new DlhSoft.ProjectDataControlLibrary.ResourceLoad.ResourceLoadScaleArea();
            this.resourceLoadChartViewResourceLoadChartHeader = new DlhSoft.ProjectDataControlLibrary.ResourceLoad.ResourceLoadChartHeader();
            this.resourceLoadChartViewResourceLoadChartArea = new DlhSoft.ProjectDataControlLibrary.ResourceLoad.ResourceLoadChartArea();
            this.resourceLoadChartViewResourceLoadChartCurrentDateScrollBar = new DlhSoft.ProjectDataControlLibrary.ResourceLoad.ResourceLoadChartCurrentDateScrollBar();
            this.ganttChartView = new DlhSoft.ProjectDataControlLibrary.GanttChartView();
            this.ganttChartViewSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ganttChartViewTasksTreeGrid = new DlhSoft.ProjectDataControlLibrary.TasksTreeGrid();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startDataGridViewDateTimePickerColumn = new YamyProject.UI.Construction.ProjectPlanning.DataGridViewDateTimePickerColumn();
            this.finishDataGridViewDateTimePickerColumn = new YamyProject.UI.Construction.ProjectPlanning.DataGridViewDateTimePickerColumn();
            this.workDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.completedWorkDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.percentCompletedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isMilestoneDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.predecessorsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resourcesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plannedStartDataGridViewDateTimePickerColumn = new YamyProject.UI.Construction.ProjectPlanning.DataGridViewDateTimePickerColumn();
            this.plannedWorkDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plannedCompletedWorkDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.indentLevelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iconIndexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ganttChartViewGanttChartPanel = new DlhSoft.ProjectDataControlLibrary.GanttChartPanel();
            this.ganttChartViewGanttChartHeader = new DlhSoft.ProjectDataControlLibrary.GanttChartHeader();
            this.ganttChartViewGanttChartArea = new DlhSoft.ProjectDataControlLibrary.GanttChartArea();
            this.ganttChartViewGanttChartVerticalScrollBar = new DlhSoft.ProjectDataControlLibrary.GanttChartVerticalScrollBar();
            this.ganttChartViewGanttChartCurrentDateScrollBar = new DlhSoft.ProjectDataControlLibrary.GanttChartCurrentDateScrollBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.closeButton = new System.Windows.Forms.Button();
            this.tabControlModes = new System.Windows.Forms.TabControl();
            this.standardTabPage = new System.Windows.Forms.TabPage();
            this.standardDisabledLabel = new System.Windows.Forms.Label();
            this.standardRowGroupBox = new System.Windows.Forms.GroupBox();
            this.standardExpandButton = new System.Windows.Forms.Button();
            this.standardIncreaseIndentButton = new System.Windows.Forms.Button();
            this.standardSetIconLabel = new System.Windows.Forms.Label();
            this.standardDecreaseIndentButton = new System.Windows.Forms.Button();
            this.standardSetIconComboBox = new System.Windows.Forms.ComboBox();
            this.standardMainGroupBox = new System.Windows.Forms.GroupBox();
            this.standardPrintPreviewButton = new System.Windows.Forms.Button();
            this.standardPageSetupButton = new System.Windows.Forms.Button();
            this.standardGanttChartGroupBox = new System.Windows.Forms.GroupBox();
            this.standardScaleComboBox = new System.Windows.Forms.ComboBox();
            this.standardGanttChartShowGroupBox = new System.Windows.Forms.GroupBox();
            this.standardShowBaselineCheckBox = new System.Windows.Forms.CheckBox();
            this.standardHighlightCriticalTasksCheckBox = new System.Windows.Forms.CheckBox();
            this.standardShowDependenciesCheckBox = new System.Windows.Forms.CheckBox();
            this.standardShowExtraDaytimeCheckBox = new System.Windows.Forms.CheckBox();
            this.standardShowExtraDaysCheckBox = new System.Windows.Forms.CheckBox();
            this.standardShowResourcesCheckBox = new System.Windows.Forms.CheckBox();
            this.standardColorComboBox = new System.Windows.Forms.ComboBox();
            this.standardCalendarComboBox = new System.Windows.Forms.ComboBox();
            this.standardUpdateScaleComboBox = new System.Windows.Forms.ComboBox();
            this.standardTasksTreeGridGroupBox = new System.Windows.Forms.GroupBox();
            this.standardFixedGridCheckBox = new System.Windows.Forms.CheckBox();
            this.standardShowGridCheckBox = new System.Windows.Forms.CheckBox();
            this.standardReinitializeButton = new System.Windows.Forms.Button();
            this.standardClearButton = new System.Windows.Forms.Button();
            this.dataTableTabPage = new System.Windows.Forms.TabPage();
            this.dataTableDisabledLabel = new System.Windows.Forms.Label();
            this.dataTableRowGroupBox = new System.Windows.Forms.GroupBox();
            this.dataTableMarkersGroupBox = new System.Windows.Forms.GroupBox();
            this.dataTableMarkerAtLabel = new System.Windows.Forms.Label();
            this.dataTableMarkerAtNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.dataTableMarkersClearButton = new System.Windows.Forms.Button();
            this.dataTableMarkerAddButton = new System.Windows.Forms.Button();
            this.dataTableMarkerTypeComboBox = new System.Windows.Forms.ComboBox();
            this.dataTableInterruptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.dataTableInterruptionForLabel = new System.Windows.Forms.Label();
            this.dataTableInterruptionAtLabel = new System.Windows.Forms.Label();
            this.dataTableInterruptionForNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.dataTableInterruptionAtNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.dataTableInterruptionsClearButton = new System.Windows.Forms.Button();
            this.dataTableInterruptionAddButton = new System.Windows.Forms.Button();
            this.dataTableSetColorLabel = new System.Windows.Forms.Label();
            this.dataTableSetColorComboBox = new System.Windows.Forms.ComboBox();
            this.dataTableSetIconLabel = new System.Windows.Forms.Label();
            this.dataTableSetIconComboBox = new System.Windows.Forms.ComboBox();
            this.dataTableIncreaseIndentButton = new System.Windows.Forms.Button();
            this.dataTableDecreaseIndentButton = new System.Windows.Forms.Button();
            this.dataTableExpandButton = new System.Windows.Forms.Button();
            this.dataTableMainGroupBox = new System.Windows.Forms.GroupBox();
            this.dataTablePrintPreviewButton = new System.Windows.Forms.Button();
            this.dataTablePageSetupButton = new System.Windows.Forms.Button();
            this.dataTableGanttChartGroupBox = new System.Windows.Forms.GroupBox();
            this.dataTableScaleComboBox = new System.Windows.Forms.ComboBox();
            this.dataTableGanttChartShowGroupBox = new System.Windows.Forms.GroupBox();
            this.dataTableShowBaselineCheckBox = new System.Windows.Forms.CheckBox();
            this.dataTableHighlightCriticalTasksCheckBox = new System.Windows.Forms.CheckBox();
            this.dataTableShowExtraDaytimeCheckBox = new System.Windows.Forms.CheckBox();
            this.dataTableShowExtraDaysCheckBox = new System.Windows.Forms.CheckBox();
            this.dataTableShowToolTipsCheckBox = new System.Windows.Forms.CheckBox();
            this.dataTableShowDependenciesCheckBox = new System.Windows.Forms.CheckBox();
            this.dataTableShowResourcesCheckBox = new System.Windows.Forms.CheckBox();
            this.dataTableShowMarkersCheckBox = new System.Windows.Forms.CheckBox();
            this.dataTableShowIconsCheckBox = new System.Windows.Forms.CheckBox();
            this.dataTableColorComboBox = new System.Windows.Forms.ComboBox();
            this.dataTableCalendarComboBox = new System.Windows.Forms.ComboBox();
            this.dataTableUpdateScaleComboBox = new System.Windows.Forms.ComboBox();
            this.dataTableTasksTreeGridGroupBox = new System.Windows.Forms.GroupBox();
            this.dataTableFixedGridCheckBox = new System.Windows.Forms.CheckBox();
            this.dataTableShowGridCheckBox = new System.Windows.Forms.CheckBox();
            this.dataTableReinitializeButton = new System.Windows.Forms.Button();
            this.dataTableClearButton = new System.Windows.Forms.Button();
            this.objectsTabPage = new System.Windows.Forms.TabPage();
            this.objectsDisabledLabel = new System.Windows.Forms.Label();
            this.objectsRowGroupBox = new System.Windows.Forms.GroupBox();
            this.objectsMarkersGroupBox = new System.Windows.Forms.GroupBox();
            this.objectsMarkerAtLabel = new System.Windows.Forms.Label();
            this.objectsMarkerAtNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.objectsMarkersClearButton = new System.Windows.Forms.Button();
            this.objectsMarkerAddButton = new System.Windows.Forms.Button();
            this.objectsMarkerTypeComboBox = new System.Windows.Forms.ComboBox();
            this.objectsInterruptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.objectsInterruptionForLabel = new System.Windows.Forms.Label();
            this.objectsInterruptionAtLabel = new System.Windows.Forms.Label();
            this.objectsInterruptionForNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.objectsInterruptionAtNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.objectsInterruptionsClear = new System.Windows.Forms.Button();
            this.objectsInterruptionAddButton = new System.Windows.Forms.Button();
            this.objectsSetColorLabel = new System.Windows.Forms.Label();
            this.objectsSetIconLabel = new System.Windows.Forms.Label();
            this.objectsSetColorComboBox = new System.Windows.Forms.ComboBox();
            this.objectsSetIconComboBox = new System.Windows.Forms.ComboBox();
            this.objectsIncreaseIndentButton = new System.Windows.Forms.Button();
            this.objectsDecreaseIndentButton = new System.Windows.Forms.Button();
            this.objectsExpandButton = new System.Windows.Forms.Button();
            this.objectsMainGroupBox = new System.Windows.Forms.GroupBox();
            this.objectsPrintPreviewButton = new System.Windows.Forms.Button();
            this.objectsPageSetupButton = new System.Windows.Forms.Button();
            this.objectsGanttChartGroupBox = new System.Windows.Forms.GroupBox();
            this.objectsScaleComboBox = new System.Windows.Forms.ComboBox();
            this.objectsGanttChartShowGroupBox = new System.Windows.Forms.GroupBox();
            this.objectsShowBaselineCheckBox = new System.Windows.Forms.CheckBox();
            this.objectsHighlightCriticalTasksCheckBox = new System.Windows.Forms.CheckBox();
            this.objectsShowExtraDaytimeCheckBox = new System.Windows.Forms.CheckBox();
            this.objectsShowExtraDaysCheckBox = new System.Windows.Forms.CheckBox();
            this.objectsShowToolTipsCheckBox = new System.Windows.Forms.CheckBox();
            this.objectsShowDependenciesCheckBox = new System.Windows.Forms.CheckBox();
            this.objectsShowResourcesCheckBox = new System.Windows.Forms.CheckBox();
            this.objectsShowMarkersCheckBox = new System.Windows.Forms.CheckBox();
            this.objectsShowIconsCheckBox = new System.Windows.Forms.CheckBox();
            this.objectsColorComboBox = new System.Windows.Forms.ComboBox();
            this.objectsCalendarComboBox = new System.Windows.Forms.ComboBox();
            this.objectsUpdateScaleComboBox = new System.Windows.Forms.ComboBox();
            this.objectsTasksTreeGridGroupBox = new System.Windows.Forms.GroupBox();
            this.objectsFixedGridCheckBox = new System.Windows.Forms.CheckBox();
            this.objectsShowGridCheckBox = new System.Windows.Forms.CheckBox();
            this.objectsReinitializeButton = new System.Windows.Forms.Button();
            this.objectsClearButton = new System.Windows.Forms.Button();
            this.propertiesButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.resourceLoadChartView.SuspendLayout();
            this.ganttChartView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ganttChartViewSplitContainer)).BeginInit();
            this.ganttChartViewSplitContainer.Panel1.SuspendLayout();
            this.ganttChartViewSplitContainer.Panel2.SuspendLayout();
            this.ganttChartViewSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ganttChartViewTasksTreeGrid)).BeginInit();
            this.ganttChartViewGanttChartPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControlModes.SuspendLayout();
            this.standardTabPage.SuspendLayout();
            this.standardRowGroupBox.SuspendLayout();
            this.standardMainGroupBox.SuspendLayout();
            this.standardGanttChartGroupBox.SuspendLayout();
            this.standardGanttChartShowGroupBox.SuspendLayout();
            this.standardTasksTreeGridGroupBox.SuspendLayout();
            this.dataTableTabPage.SuspendLayout();
            this.dataTableRowGroupBox.SuspendLayout();
            this.dataTableMarkersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableMarkerAtNumericUpDown)).BeginInit();
            this.dataTableInterruptionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableInterruptionForNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableInterruptionAtNumericUpDown)).BeginInit();
            this.dataTableMainGroupBox.SuspendLayout();
            this.dataTableGanttChartGroupBox.SuspendLayout();
            this.dataTableGanttChartShowGroupBox.SuspendLayout();
            this.dataTableTasksTreeGridGroupBox.SuspendLayout();
            this.objectsTabPage.SuspendLayout();
            this.objectsRowGroupBox.SuspendLayout();
            this.objectsMarkersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectsMarkerAtNumericUpDown)).BeginInit();
            this.objectsInterruptionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectsInterruptionForNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectsInterruptionAtNumericUpDown)).BeginInit();
            this.objectsMainGroupBox.SuspendLayout();
            this.objectsGanttChartGroupBox.SuspendLayout();
            this.objectsGanttChartShowGroupBox.SuspendLayout();
            this.objectsTasksTreeGridGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // iconsList
            // 
            this.iconsList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconsList.ImageStream")));
            this.iconsList.TransparentColor = System.Drawing.Color.Transparent;
            this.iconsList.Images.SetKeyName(0, "Task");
            this.iconsList.Images.SetKeyName(1, "Folder");
            this.iconsList.Images.SetKeyName(2, "Note");
            // 
            // markersList
            // 
            this.markersList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("markersList.ImageStream")));
            this.markersList.TransparentColor = System.Drawing.Color.Transparent;
            this.markersList.Images.SetKeyName(0, "");
            this.markersList.Images.SetKeyName(1, "");
            // 
            // timerCurrentCellChanged
            // 
            this.timerCurrentCellChanged.Interval = 1;
            this.timerCurrentCellChanged.Tick += new System.EventHandler(this.timerCurrentCellChanged_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.resourceLoadChartView);
            this.panel1.Controls.Add(this.ganttChartView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1380, 674);
            this.panel1.TabIndex = 11;
            // 
            // resourceLoadChartView
            // 
            this.resourceLoadChartView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resourceLoadChartView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resourceLoadChartView.CompletedWorkMember = "CompletedWork";
            this.resourceLoadChartView.Controls.Add(this.resourceLoadChartViewResourceLoadScaleHeader);
            this.resourceLoadChartView.Controls.Add(this.resourceLoadChartViewResourceLoadScaleArea);
            this.resourceLoadChartView.Controls.Add(this.resourceLoadChartViewResourceLoadChartHeader);
            this.resourceLoadChartView.Controls.Add(this.resourceLoadChartViewResourceLoadChartArea);
            this.resourceLoadChartView.Controls.Add(this.resourceLoadChartViewResourceLoadChartCurrentDateScrollBar);
            this.resourceLoadChartView.IndentLevelMember = "IndentLevel";
            this.resourceLoadChartView.InterruptionsMember = "Interruptions";
            this.resourceLoadChartView.Location = new System.Drawing.Point(215, 514);
            this.resourceLoadChartView.Name = "resourceLoadChartView";
            this.resourceLoadChartView.ResourceLoadScaleWidth = 304;
            this.resourceLoadChartView.ResourcesMember = "Resources";
            this.resourceLoadChartView.Size = new System.Drawing.Size(949, 111);
            this.resourceLoadChartView.StartMember = "Start";
            this.resourceLoadChartView.TabIndex = 9;
            this.resourceLoadChartView.WorkMember = "Work";
            this.resourceLoadChartView.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(this.resourceLoadChartView_PropertyChanged);
            // 
            // resourceLoadChartViewResourceLoadScaleHeader
            // 
            this.resourceLoadChartViewResourceLoadScaleHeader.Location = new System.Drawing.Point(0, 0);
            this.resourceLoadChartViewResourceLoadScaleHeader.Name = "resourceLoadChartViewResourceLoadScaleHeader";
            this.resourceLoadChartViewResourceLoadScaleHeader.Size = new System.Drawing.Size(304, 40);
            this.resourceLoadChartViewResourceLoadScaleHeader.TabIndex = 0;
            // 
            // resourceLoadChartViewResourceLoadScaleArea
            // 
            this.resourceLoadChartViewResourceLoadScaleArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.resourceLoadChartViewResourceLoadScaleArea.BackColor = System.Drawing.SystemColors.Window;
            this.resourceLoadChartViewResourceLoadScaleArea.Location = new System.Drawing.Point(0, 40);
            this.resourceLoadChartViewResourceLoadScaleArea.Name = "resourceLoadChartViewResourceLoadScaleArea";
            this.resourceLoadChartViewResourceLoadScaleArea.Size = new System.Drawing.Size(304, 52);
            this.resourceLoadChartViewResourceLoadScaleArea.TabIndex = 1;
            // 
            // resourceLoadChartViewResourceLoadChartHeader
            // 
            this.resourceLoadChartViewResourceLoadChartHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resourceLoadChartViewResourceLoadChartHeader.Location = new System.Drawing.Point(304, 0);
            this.resourceLoadChartViewResourceLoadChartHeader.Name = "resourceLoadChartViewResourceLoadChartHeader";
            this.resourceLoadChartViewResourceLoadChartHeader.Size = new System.Drawing.Size(643, 40);
            this.resourceLoadChartViewResourceLoadChartHeader.TabIndex = 2;
            // 
            // resourceLoadChartViewResourceLoadChartArea
            // 
            this.resourceLoadChartViewResourceLoadChartArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resourceLoadChartViewResourceLoadChartArea.BackColor = System.Drawing.SystemColors.Window;
            this.resourceLoadChartViewResourceLoadChartArea.CompletedWorkMember = "CompletedWork";
            this.resourceLoadChartViewResourceLoadChartArea.IndentLevelMember = "IndentLevel";
            this.resourceLoadChartViewResourceLoadChartArea.InterruptionsMember = "Interruptions";
            this.resourceLoadChartViewResourceLoadChartArea.Location = new System.Drawing.Point(304, 40);
            this.resourceLoadChartViewResourceLoadChartArea.Name = "resourceLoadChartViewResourceLoadChartArea";
            this.resourceLoadChartViewResourceLoadChartArea.ResourcesMember = "Resources";
            this.resourceLoadChartViewResourceLoadChartArea.Size = new System.Drawing.Size(643, 52);
            this.resourceLoadChartViewResourceLoadChartArea.StartMember = "Start";
            this.resourceLoadChartViewResourceLoadChartArea.TabIndex = 3;
            this.resourceLoadChartViewResourceLoadChartArea.WorkMember = "Work";
            // 
            // resourceLoadChartViewResourceLoadChartCurrentDateScrollBar
            // 
            this.resourceLoadChartViewResourceLoadChartCurrentDateScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resourceLoadChartViewResourceLoadChartCurrentDateScrollBar.LargeChange = 7;
            this.resourceLoadChartViewResourceLoadChartCurrentDateScrollBar.Location = new System.Drawing.Point(307, 92);
            this.resourceLoadChartViewResourceLoadChartCurrentDateScrollBar.Maximum = 730;
            this.resourceLoadChartViewResourceLoadChartCurrentDateScrollBar.Name = "resourceLoadChartViewResourceLoadChartCurrentDateScrollBar";
            this.resourceLoadChartViewResourceLoadChartCurrentDateScrollBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.resourceLoadChartViewResourceLoadChartCurrentDateScrollBar.Size = new System.Drawing.Size(956, 17);
            this.resourceLoadChartViewResourceLoadChartCurrentDateScrollBar.TabIndex = 4;
            // 
            // ganttChartView
            // 
            this.ganttChartView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ganttChartView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ganttChartView.CompletedWorkMember = "CompletedWork";
            this.ganttChartView.Controls.Add(this.ganttChartViewSplitContainer);
            this.ganttChartView.DefaultAssignmentsBrush = null;
            this.ganttChartView.DefaultAssignmentsFont = null;
            this.ganttChartView.DefaultBarTextsBrush = null;
            this.ganttChartView.DefaultBarTextsFont = null;
            this.ganttChartView.DefaultBottomTextsBrush = null;
            this.ganttChartView.DefaultBottomTextsFont = null;
            this.ganttChartView.DefaultCriticalRemainingsTextBrush = null;
            this.ganttChartView.DefaultCriticalRemainingsTextFont = null;
            this.ganttChartView.DefaultLeftTextsBrush = null;
            this.ganttChartView.DefaultLeftTextsFont = null;
            this.ganttChartView.DefaultTopTextsBrush = null;
            this.ganttChartView.DefaultTopTextsFont = null;
            this.ganttChartView.Enabled = false;
            this.ganttChartView.GanttImageIndexMember = "IconIndex";
            this.ganttChartView.GanttImageList = this.iconsList;
            this.ganttChartView.IndentLevelMember = "IndentLevel";
            this.ganttChartView.InterruptionsMember = "Interruptions";
            this.ganttChartView.Location = new System.Drawing.Point(215, 48);
            this.ganttChartView.MarkersImageList = this.markersList;
            this.ganttChartView.MarkersMember = "Markers";
            this.ganttChartView.Name = "ganttChartView";
            this.ganttChartView.PlannedCompletedWorkMember = "PlannedCompletedWork";
            this.ganttChartView.PlannedStartMember = "PlannedStart";
            this.ganttChartView.PlannedWorkMember = "PlannedWork";
            this.ganttChartView.PredecessorsMember = "Predecessors";
            this.ganttChartView.ResourcesMember = "Resources";
            this.ganttChartView.Size = new System.Drawing.Size(950, 463);
            this.ganttChartView.StartMember = "Start";
            this.ganttChartView.TabIndex = 8;
            this.ganttChartView.ToolTipMember = "Description";
            this.ganttChartView.TreeImageIndexMember = "IconIndex";
            this.ganttChartView.TreeImageList = this.iconsList;
            this.ganttChartView.WorkBrushMember = "Brush";
            this.ganttChartView.WorkMember = "Work";
            this.ganttChartView.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(this.ganttChartView_PropertyChanged);
            // 
            // ganttChartViewSplitContainer
            // 
            this.ganttChartViewSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ganttChartViewSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.ganttChartViewSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.ganttChartViewSplitContainer.Name = "ganttChartViewSplitContainer";
            // 
            // ganttChartViewSplitContainer.Panel1
            // 
            this.ganttChartViewSplitContainer.Panel1.Controls.Add(this.ganttChartViewTasksTreeGrid);
            // 
            // ganttChartViewSplitContainer.Panel2
            // 
            this.ganttChartViewSplitContainer.Panel2.Controls.Add(this.ganttChartViewGanttChartPanel);
            this.ganttChartViewSplitContainer.Panel2.Controls.Add(this.ganttChartViewGanttChartCurrentDateScrollBar);
            this.ganttChartViewSplitContainer.Size = new System.Drawing.Size(948, 461);
            this.ganttChartViewSplitContainer.SplitterDistance = 300;
            this.ganttChartViewSplitContainer.TabIndex = 0;
            this.ganttChartViewSplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.ganttChartViewSplitContainer_SplitterMoved);
            // 
            // ganttChartViewTasksTreeGrid
            // 
            this.ganttChartViewTasksTreeGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ganttChartViewTasksTreeGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ganttChartViewTasksTreeGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ganttChartViewTasksTreeGrid.ColumnHeadersHeight = 40;
            this.ganttChartViewTasksTreeGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.startDataGridViewDateTimePickerColumn,
            this.finishDataGridViewDateTimePickerColumn,
            this.workDataGridViewTextBoxColumn,
            this.completedWorkDataGridViewTextBoxColumn,
            this.percentCompletedDataGridViewTextBoxColumn,
            this.isMilestoneDataGridViewCheckBoxColumn,
            this.predecessorsDataGridViewTextBoxColumn,
            this.resourcesDataGridViewTextBoxColumn,
            this.plannedStartDataGridViewDateTimePickerColumn,
            this.plannedWorkDataGridViewTextBoxColumn,
            this.plannedCompletedWorkDataGridViewTextBoxColumn,
            this.indentLevelDataGridViewTextBoxColumn,
            this.iconIndexDataGridViewTextBoxColumn});
            this.ganttChartViewTasksTreeGrid.CompletedWorkMember = "CompletedWork";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ganttChartViewTasksTreeGrid.DefaultCellStyle = dataGridViewCellStyle13;
            this.ganttChartViewTasksTreeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ganttChartViewTasksTreeGrid.EnableHeadersVisualStyles = false;
            this.ganttChartViewTasksTreeGrid.ImageIndexMember = "IconIndex";
            this.ganttChartViewTasksTreeGrid.ImageList = this.iconsList;
            this.ganttChartViewTasksTreeGrid.IndentLevelMember = "IndentLevel";
            this.ganttChartViewTasksTreeGrid.Location = new System.Drawing.Point(0, 0);
            this.ganttChartViewTasksTreeGrid.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ganttChartViewTasksTreeGrid.Name = "ganttChartViewTasksTreeGrid";
            this.ganttChartViewTasksTreeGrid.PredecessorsMember = "Predecessors";
            this.ganttChartViewTasksTreeGrid.ResourcesMember = "Resources";
            this.ganttChartViewTasksTreeGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ganttChartViewTasksTreeGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.ganttChartViewTasksTreeGrid.Size = new System.Drawing.Size(300, 461);
            this.ganttChartViewTasksTreeGrid.StartMember = "Start";
            this.ganttChartViewTasksTreeGrid.TabIndex = 0;
            this.ganttChartViewTasksTreeGrid.TreeImageList = this.iconsList;
            this.ganttChartViewTasksTreeGrid.TreeMinusIcon = ((System.Drawing.Icon)(resources.GetObject("ganttChartViewTasksTreeGrid.TreeMinusIcon")));
            this.ganttChartViewTasksTreeGrid.TreePlusIcon = ((System.Drawing.Icon)(resources.GetObject("ganttChartViewTasksTreeGrid.TreePlusIcon")));
            this.ganttChartViewTasksTreeGrid.WorkMember = "Work";
            this.ganttChartViewTasksTreeGrid.RowIndentLevelChanged += new System.Windows.Forms.DataGridViewRowEventHandler(this.ganttChartViewTasksTreeGrid_RowIndentLevelChanged);
            this.ganttChartViewTasksTreeGrid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.ganttChartViewTasksTreeGrid_CellBeginEdit);
            this.ganttChartViewTasksTreeGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ganttChartViewTasksTreeGridView_CellEndEdit);
            this.ganttChartViewTasksTreeGrid.CurrentCellChanged += new System.EventHandler(this.ganttChartViewTasksTreeGridView_CurrentCellChanged);
            this.ganttChartViewTasksTreeGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.ganttChartViewTasksTreeGrid_DataError);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.nameDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.nameDataGridViewTextBoxColumn.Frozen = true;
            this.nameDataGridViewTextBoxColumn.HeaderText = "Task";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.nameDataGridViewTextBoxColumn.Width = 123;
            // 
            // startDataGridViewDateTimePickerColumn
            // 
            this.startDataGridViewDateTimePickerColumn.DataPropertyName = "Start";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.startDataGridViewDateTimePickerColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.startDataGridViewDateTimePickerColumn.HeaderText = "Start";
            this.startDataGridViewDateTimePickerColumn.Name = "startDataGridViewDateTimePickerColumn";
            this.startDataGridViewDateTimePickerColumn.Width = 135;
            // 
            // finishDataGridViewDateTimePickerColumn
            // 
            this.finishDataGridViewDateTimePickerColumn.DataPropertyName = "Finish";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.finishDataGridViewDateTimePickerColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.finishDataGridViewDateTimePickerColumn.HeaderText = "Finish";
            this.finishDataGridViewDateTimePickerColumn.Name = "finishDataGridViewDateTimePickerColumn";
            this.finishDataGridViewDateTimePickerColumn.Width = 135;
            // 
            // workDataGridViewTextBoxColumn
            // 
            this.workDataGridViewTextBoxColumn.DataPropertyName = "Work";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "0.00h";
            dataGridViewCellStyle5.NullValue = null;
            this.workDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.workDataGridViewTextBoxColumn.HeaderText = "Work";
            this.workDataGridViewTextBoxColumn.Name = "workDataGridViewTextBoxColumn";
            this.workDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.workDataGridViewTextBoxColumn.Width = 80;
            // 
            // completedWorkDataGridViewTextBoxColumn
            // 
            this.completedWorkDataGridViewTextBoxColumn.DataPropertyName = "CompletedWork";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "0.00h";
            dataGridViewCellStyle6.NullValue = null;
            this.completedWorkDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.completedWorkDataGridViewTextBoxColumn.HeaderText = "Completed Work";
            this.completedWorkDataGridViewTextBoxColumn.Name = "completedWorkDataGridViewTextBoxColumn";
            this.completedWorkDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.completedWorkDataGridViewTextBoxColumn.Width = 80;
            // 
            // percentCompletedDataGridViewTextBoxColumn
            // 
            this.percentCompletedDataGridViewTextBoxColumn.DataPropertyName = "PercentCompleted";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "0.00%";
            dataGridViewCellStyle7.NullValue = null;
            this.percentCompletedDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.percentCompletedDataGridViewTextBoxColumn.HeaderText = "Percent Completed";
            this.percentCompletedDataGridViewTextBoxColumn.Name = "percentCompletedDataGridViewTextBoxColumn";
            this.percentCompletedDataGridViewTextBoxColumn.ReadOnly = true;
            this.percentCompletedDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.percentCompletedDataGridViewTextBoxColumn.Width = 80;
            // 
            // isMilestoneDataGridViewCheckBoxColumn
            // 
            this.isMilestoneDataGridViewCheckBoxColumn.DataPropertyName = "IsMilestone";
            this.isMilestoneDataGridViewCheckBoxColumn.HeaderText = "Is Milestone";
            this.isMilestoneDataGridViewCheckBoxColumn.Name = "isMilestoneDataGridViewCheckBoxColumn";
            this.isMilestoneDataGridViewCheckBoxColumn.Width = 80;
            // 
            // predecessorsDataGridViewTextBoxColumn
            // 
            this.predecessorsDataGridViewTextBoxColumn.DataPropertyName = "Predecessors";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.predecessorsDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle8;
            this.predecessorsDataGridViewTextBoxColumn.HeaderText = "Predecessors";
            this.predecessorsDataGridViewTextBoxColumn.Name = "predecessorsDataGridViewTextBoxColumn";
            this.predecessorsDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.predecessorsDataGridViewTextBoxColumn.Width = 80;
            // 
            // resourcesDataGridViewTextBoxColumn
            // 
            this.resourcesDataGridViewTextBoxColumn.DataPropertyName = "Resources";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.resourcesDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle9;
            this.resourcesDataGridViewTextBoxColumn.HeaderText = "Resources";
            this.resourcesDataGridViewTextBoxColumn.Name = "resourcesDataGridViewTextBoxColumn";
            this.resourcesDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.resourcesDataGridViewTextBoxColumn.Width = 80;
            // 
            // plannedStartDataGridViewDateTimePickerColumn
            // 
            this.plannedStartDataGridViewDateTimePickerColumn.DataPropertyName = "PlannedStart";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.plannedStartDataGridViewDateTimePickerColumn.DefaultCellStyle = dataGridViewCellStyle10;
            this.plannedStartDataGridViewDateTimePickerColumn.HeaderText = "Baseline Start";
            this.plannedStartDataGridViewDateTimePickerColumn.Name = "plannedStartDataGridViewDateTimePickerColumn";
            this.plannedStartDataGridViewDateTimePickerColumn.Width = 135;
            // 
            // plannedWorkDataGridViewTextBoxColumn
            // 
            this.plannedWorkDataGridViewTextBoxColumn.DataPropertyName = "PlannedWork";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "0.00h";
            this.plannedWorkDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle11;
            this.plannedWorkDataGridViewTextBoxColumn.HeaderText = "Baseline Work";
            this.plannedWorkDataGridViewTextBoxColumn.Name = "plannedWorkDataGridViewTextBoxColumn";
            this.plannedWorkDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.plannedWorkDataGridViewTextBoxColumn.Width = 80;
            // 
            // plannedCompletedWorkDataGridViewTextBoxColumn
            // 
            this.plannedCompletedWorkDataGridViewTextBoxColumn.DataPropertyName = "PlannedCompletedWork";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.Format = "0.00h";
            this.plannedCompletedWorkDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle12;
            this.plannedCompletedWorkDataGridViewTextBoxColumn.HeaderText = "Baseline Compl. Work";
            this.plannedCompletedWorkDataGridViewTextBoxColumn.Name = "plannedCompletedWorkDataGridViewTextBoxColumn";
            this.plannedCompletedWorkDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.plannedCompletedWorkDataGridViewTextBoxColumn.Width = 80;
            // 
            // indentLevelDataGridViewTextBoxColumn
            // 
            this.indentLevelDataGridViewTextBoxColumn.DataPropertyName = "IndentLevel";
            this.indentLevelDataGridViewTextBoxColumn.HeaderText = "Indent Level";
            this.indentLevelDataGridViewTextBoxColumn.Name = "indentLevelDataGridViewTextBoxColumn";
            this.indentLevelDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.indentLevelDataGridViewTextBoxColumn.Width = 80;
            // 
            // iconIndexDataGridViewTextBoxColumn
            // 
            this.iconIndexDataGridViewTextBoxColumn.DataPropertyName = "IconIndex";
            this.iconIndexDataGridViewTextBoxColumn.HeaderText = "Icon Index";
            this.iconIndexDataGridViewTextBoxColumn.Name = "iconIndexDataGridViewTextBoxColumn";
            this.iconIndexDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.iconIndexDataGridViewTextBoxColumn.Width = 80;
            // 
            // ganttChartViewGanttChartPanel
            // 
            this.ganttChartViewGanttChartPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ganttChartViewGanttChartPanel.BackColor = System.Drawing.SystemColors.Window;
            this.ganttChartViewGanttChartPanel.Controls.Add(this.ganttChartViewGanttChartHeader);
            this.ganttChartViewGanttChartPanel.Controls.Add(this.ganttChartViewGanttChartArea);
            this.ganttChartViewGanttChartPanel.Controls.Add(this.ganttChartViewGanttChartVerticalScrollBar);
            this.ganttChartViewGanttChartPanel.Location = new System.Drawing.Point(0, 0);
            this.ganttChartViewGanttChartPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ganttChartViewGanttChartPanel.Name = "ganttChartViewGanttChartPanel";
            this.ganttChartViewGanttChartPanel.Size = new System.Drawing.Size(644, 444);
            this.ganttChartViewGanttChartPanel.TabIndex = 0;
            // 
            // ganttChartViewGanttChartHeader
            // 
            this.ganttChartViewGanttChartHeader.BackColor = System.Drawing.SystemColors.Control;
            this.ganttChartViewGanttChartHeader.Location = new System.Drawing.Point(0, 0);
            this.ganttChartViewGanttChartHeader.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ganttChartViewGanttChartHeader.Name = "ganttChartViewGanttChartHeader";
            this.ganttChartViewGanttChartHeader.Size = new System.Drawing.Size(644, 40);
            this.ganttChartViewGanttChartHeader.TabIndex = 0;
            this.ganttChartViewGanttChartHeader.Click += new System.EventHandler(this.ganttChartViewGanttChartHeader_Click);
            // 
            // ganttChartViewGanttChartArea
            // 
            this.ganttChartViewGanttChartArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ganttChartViewGanttChartArea.CompletedWorkMember = "CompletedWork";
            this.ganttChartViewGanttChartArea.DefaultAssignmentsBrush = null;
            this.ganttChartViewGanttChartArea.DefaultAssignmentsFont = null;
            this.ganttChartViewGanttChartArea.DefaultBarTextsBrush = null;
            this.ganttChartViewGanttChartArea.DefaultBarTextsFont = null;
            this.ganttChartViewGanttChartArea.DefaultBottomTextsBrush = null;
            this.ganttChartViewGanttChartArea.DefaultBottomTextsFont = null;
            this.ganttChartViewGanttChartArea.DefaultCriticalRemainingsTextBrush = null;
            this.ganttChartViewGanttChartArea.DefaultCriticalRemainingsTextFont = null;
            this.ganttChartViewGanttChartArea.DefaultLeftTextsBrush = null;
            this.ganttChartViewGanttChartArea.DefaultLeftTextsFont = null;
            this.ganttChartViewGanttChartArea.DefaultTopTextsBrush = null;
            this.ganttChartViewGanttChartArea.DefaultTopTextsFont = null;
            this.ganttChartViewGanttChartArea.ImageIndexMember = "IconIndex";
            this.ganttChartViewGanttChartArea.ImageList = this.iconsList;
            this.ganttChartViewGanttChartArea.InterruptionsMember = "Interruptions";
            this.ganttChartViewGanttChartArea.Location = new System.Drawing.Point(0, 40);
            this.ganttChartViewGanttChartArea.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ganttChartViewGanttChartArea.MarkersImageList = this.markersList;
            this.ganttChartViewGanttChartArea.MarkersMember = "Markers";
            this.ganttChartViewGanttChartArea.Name = "ganttChartViewGanttChartArea";
            this.ganttChartViewGanttChartArea.PlannedCompletedWorkMember = "PlannedCompletedWork";
            this.ganttChartViewGanttChartArea.PlannedStartMember = "PlannedStart";
            this.ganttChartViewGanttChartArea.PlannedWorkMember = "PlannedWork";
            this.ganttChartViewGanttChartArea.PredecessorsMember = "Predecessors";
            this.ganttChartViewGanttChartArea.ResourcesMember = "Resources";
            this.ganttChartViewGanttChartArea.Size = new System.Drawing.Size(644, 404);
            this.ganttChartViewGanttChartArea.StartMember = "Start";
            this.ganttChartViewGanttChartArea.TabIndex = 1;
            this.ganttChartViewGanttChartArea.ToolTipMember = "Description";
            this.ganttChartViewGanttChartArea.WorkBrushMember = "Brush";
            this.ganttChartViewGanttChartArea.WorkMember = "Work";
            this.ganttChartViewGanttChartArea.PostPaint += new System.Windows.Forms.PaintEventHandler(this.ganttChartViewGanttChartArea_PostPaint);
            this.ganttChartViewGanttChartArea.Paint += new System.Windows.Forms.PaintEventHandler(this.ganttChartViewGanttChartArea_Paint);
            // 
            // ganttChartViewGanttChartVerticalScrollBar
            // 
            this.ganttChartViewGanttChartVerticalScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ganttChartViewGanttChartVerticalScrollBar.LargeChange = 22;
            this.ganttChartViewGanttChartVerticalScrollBar.Location = new System.Drawing.Point(644, 0);
            this.ganttChartViewGanttChartVerticalScrollBar.Maximum = 22;
            this.ganttChartViewGanttChartVerticalScrollBar.Name = "ganttChartViewGanttChartVerticalScrollBar";
            this.ganttChartViewGanttChartVerticalScrollBar.Size = new System.Drawing.Size(17, 444);
            this.ganttChartViewGanttChartVerticalScrollBar.SmallChange = 22;
            this.ganttChartViewGanttChartVerticalScrollBar.TabIndex = 2;
            // 
            // ganttChartViewGanttChartCurrentDateScrollBar
            // 
            this.ganttChartViewGanttChartCurrentDateScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ganttChartViewGanttChartCurrentDateScrollBar.LargeChange = 7;
            this.ganttChartViewGanttChartCurrentDateScrollBar.Location = new System.Drawing.Point(0, 444);
            this.ganttChartViewGanttChartCurrentDateScrollBar.Maximum = 730;
            this.ganttChartViewGanttChartCurrentDateScrollBar.Name = "ganttChartViewGanttChartCurrentDateScrollBar";
            this.ganttChartViewGanttChartCurrentDateScrollBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ganttChartViewGanttChartCurrentDateScrollBar.Size = new System.Drawing.Size(644, 17);
            this.ganttChartViewGanttChartCurrentDateScrollBar.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.closeButton);
            this.panel2.Controls.Add(this.tabControlModes);
            this.panel2.Controls.Add(this.propertiesButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1163, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(217, 674);
            this.panel2.TabIndex = 12;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(675, 548);
            this.closeButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(104, 19);
            this.closeButton.TabIndex = 13;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // tabControlModes
            // 
            this.tabControlModes.Controls.Add(this.standardTabPage);
            this.tabControlModes.Controls.Add(this.dataTableTabPage);
            this.tabControlModes.Controls.Add(this.objectsTabPage);
            this.tabControlModes.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabControlModes.Location = new System.Drawing.Point(15, 0);
            this.tabControlModes.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabControlModes.Name = "tabControlModes";
            this.tabControlModes.SelectedIndex = 0;
            this.tabControlModes.Size = new System.Drawing.Size(202, 674);
            this.tabControlModes.TabIndex = 12;
            // 
            // standardTabPage
            // 
            this.standardTabPage.Controls.Add(this.standardDisabledLabel);
            this.standardTabPage.Controls.Add(this.standardRowGroupBox);
            this.standardTabPage.Controls.Add(this.standardMainGroupBox);
            this.standardTabPage.Location = new System.Drawing.Point(4, 22);
            this.standardTabPage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardTabPage.Name = "standardTabPage";
            this.standardTabPage.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardTabPage.Size = new System.Drawing.Size(194, 648);
            this.standardTabPage.TabIndex = 0;
            this.standardTabPage.Text = "No binding";
            this.standardTabPage.UseVisualStyleBackColor = true;
            // 
            // standardDisabledLabel
            // 
            this.standardDisabledLabel.Location = new System.Drawing.Point(4, 319);
            this.standardDisabledLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.standardDisabledLabel.Name = "standardDisabledLabel";
            this.standardDisabledLabel.Size = new System.Drawing.Size(172, 56);
            this.standardDisabledLabel.TabIndex = 2;
            this.standardDisabledLabel.Text = resources.GetString("standardDisabledLabel.Text");
            this.standardDisabledLabel.Visible = false;
            // 
            // standardRowGroupBox
            // 
            this.standardRowGroupBox.Controls.Add(this.standardExpandButton);
            this.standardRowGroupBox.Controls.Add(this.standardIncreaseIndentButton);
            this.standardRowGroupBox.Controls.Add(this.standardSetIconLabel);
            this.standardRowGroupBox.Controls.Add(this.standardDecreaseIndentButton);
            this.standardRowGroupBox.Controls.Add(this.standardSetIconComboBox);
            this.standardRowGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardRowGroupBox.Location = new System.Drawing.Point(4, 254);
            this.standardRowGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardRowGroupBox.Name = "standardRowGroupBox";
            this.standardRowGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardRowGroupBox.Size = new System.Drawing.Size(172, 63);
            this.standardRowGroupBox.TabIndex = 1;
            this.standardRowGroupBox.TabStop = false;
            this.standardRowGroupBox.Text = "Current Task";
            // 
            // standardExpandButton
            // 
            this.standardExpandButton.Enabled = false;
            this.standardExpandButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardExpandButton.Location = new System.Drawing.Point(4, 15);
            this.standardExpandButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardExpandButton.Name = "standardExpandButton";
            this.standardExpandButton.Size = new System.Drawing.Size(80, 19);
            this.standardExpandButton.TabIndex = 0;
            this.standardExpandButton.Text = "Expand/Close";
            this.standardExpandButton.UseVisualStyleBackColor = true;
            this.standardExpandButton.Click += new System.EventHandler(this.standardExpandButton_Click);
            // 
            // standardIncreaseIndentButton
            // 
            this.standardIncreaseIndentButton.Enabled = false;
            this.standardIncreaseIndentButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardIncreaseIndentButton.Location = new System.Drawing.Point(88, 39);
            this.standardIncreaseIndentButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardIncreaseIndentButton.Name = "standardIncreaseIndentButton";
            this.standardIncreaseIndentButton.Size = new System.Drawing.Size(80, 19);
            this.standardIncreaseIndentButton.TabIndex = 4;
            this.standardIncreaseIndentButton.Text = "Increase Indent >";
            this.standardIncreaseIndentButton.UseVisualStyleBackColor = true;
            this.standardIncreaseIndentButton.Click += new System.EventHandler(this.standardIncreaseIndentButton_Click);
            // 
            // standardSetIconLabel
            // 
            this.standardSetIconLabel.AutoSize = true;
            this.standardSetIconLabel.Enabled = false;
            this.standardSetIconLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardSetIconLabel.Location = new System.Drawing.Point(88, 20);
            this.standardSetIconLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.standardSetIconLabel.Name = "standardSetIconLabel";
            this.standardSetIconLabel.Size = new System.Drawing.Size(50, 13);
            this.standardSetIconLabel.TabIndex = 1;
            this.standardSetIconLabel.Text = "Set Icon:";
            // 
            // standardDecreaseIndentButton
            // 
            this.standardDecreaseIndentButton.Enabled = false;
            this.standardDecreaseIndentButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardDecreaseIndentButton.Location = new System.Drawing.Point(4, 39);
            this.standardDecreaseIndentButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardDecreaseIndentButton.Name = "standardDecreaseIndentButton";
            this.standardDecreaseIndentButton.Size = new System.Drawing.Size(80, 19);
            this.standardDecreaseIndentButton.TabIndex = 3;
            this.standardDecreaseIndentButton.Text = "< Decrease Indent";
            this.standardDecreaseIndentButton.UseVisualStyleBackColor = true;
            this.standardDecreaseIndentButton.Click += new System.EventHandler(this.standardDecreaseIndentButton_Click);
            // 
            // standardSetIconComboBox
            // 
            this.standardSetIconComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.standardSetIconComboBox.Enabled = false;
            this.standardSetIconComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardSetIconComboBox.FormattingEnabled = true;
            this.standardSetIconComboBox.Items.AddRange(new object[] {
            "Task",
            "Folder",
            "Note"});
            this.standardSetIconComboBox.Location = new System.Drawing.Point(126, 15);
            this.standardSetIconComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardSetIconComboBox.Name = "standardSetIconComboBox";
            this.standardSetIconComboBox.Size = new System.Drawing.Size(43, 21);
            this.standardSetIconComboBox.TabIndex = 2;
            this.standardSetIconComboBox.SelectedIndexChanged += new System.EventHandler(this.standardSetIconComboBox_SelectedIndexChanged);
            // 
            // standardMainGroupBox
            // 
            this.standardMainGroupBox.Controls.Add(this.standardPrintPreviewButton);
            this.standardMainGroupBox.Controls.Add(this.standardPageSetupButton);
            this.standardMainGroupBox.Controls.Add(this.standardGanttChartGroupBox);
            this.standardMainGroupBox.Controls.Add(this.standardTasksTreeGridGroupBox);
            this.standardMainGroupBox.Controls.Add(this.standardReinitializeButton);
            this.standardMainGroupBox.Controls.Add(this.standardClearButton);
            this.standardMainGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardMainGroupBox.Location = new System.Drawing.Point(4, 5);
            this.standardMainGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardMainGroupBox.Name = "standardMainGroupBox";
            this.standardMainGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardMainGroupBox.Size = new System.Drawing.Size(172, 245);
            this.standardMainGroupBox.TabIndex = 0;
            this.standardMainGroupBox.TabStop = false;
            this.standardMainGroupBox.Text = "Data";
            // 
            // standardPrintPreviewButton
            // 
            this.standardPrintPreviewButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardPrintPreviewButton.Location = new System.Drawing.Point(88, 218);
            this.standardPrintPreviewButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardPrintPreviewButton.Name = "standardPrintPreviewButton";
            this.standardPrintPreviewButton.Size = new System.Drawing.Size(80, 19);
            this.standardPrintPreviewButton.TabIndex = 7;
            this.standardPrintPreviewButton.Text = "Print Preview";
            this.standardPrintPreviewButton.UseVisualStyleBackColor = true;
            // 
            // standardPageSetupButton
            // 
            this.standardPageSetupButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardPageSetupButton.Location = new System.Drawing.Point(4, 218);
            this.standardPageSetupButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardPageSetupButton.Name = "standardPageSetupButton";
            this.standardPageSetupButton.Size = new System.Drawing.Size(80, 19);
            this.standardPageSetupButton.TabIndex = 6;
            this.standardPageSetupButton.Text = "Page Setup";
            this.standardPageSetupButton.UseVisualStyleBackColor = true;
            // 
            // standardGanttChartGroupBox
            // 
            this.standardGanttChartGroupBox.Controls.Add(this.standardScaleComboBox);
            this.standardGanttChartGroupBox.Controls.Add(this.standardGanttChartShowGroupBox);
            this.standardGanttChartGroupBox.Controls.Add(this.standardColorComboBox);
            this.standardGanttChartGroupBox.Controls.Add(this.standardCalendarComboBox);
            this.standardGanttChartGroupBox.Controls.Add(this.standardUpdateScaleComboBox);
            this.standardGanttChartGroupBox.Enabled = false;
            this.standardGanttChartGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardGanttChartGroupBox.Location = new System.Drawing.Point(4, 78);
            this.standardGanttChartGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardGanttChartGroupBox.Name = "standardGanttChartGroupBox";
            this.standardGanttChartGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardGanttChartGroupBox.Size = new System.Drawing.Size(164, 135);
            this.standardGanttChartGroupBox.TabIndex = 7;
            this.standardGanttChartGroupBox.TabStop = false;
            this.standardGanttChartGroupBox.Text = "Gantt Chart";
            // 
            // standardScaleComboBox
            // 
            this.standardScaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.standardScaleComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardScaleComboBox.FormattingEnabled = true;
            this.standardScaleComboBox.Items.AddRange(new object[] {
            "Days/Hours",
            "Weeks/Days",
            "Months/Weeks",
            "Quarters/Months",
            "Years/Quarters"});
            this.standardScaleComboBox.Location = new System.Drawing.Point(7, 15);
            this.standardScaleComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardScaleComboBox.Name = "standardScaleComboBox";
            this.standardScaleComboBox.Size = new System.Drawing.Size(72, 21);
            this.standardScaleComboBox.TabIndex = 3;
            // 
            // standardGanttChartShowGroupBox
            // 
            this.standardGanttChartShowGroupBox.Controls.Add(this.standardShowBaselineCheckBox);
            this.standardGanttChartShowGroupBox.Controls.Add(this.standardHighlightCriticalTasksCheckBox);
            this.standardGanttChartShowGroupBox.Controls.Add(this.standardShowDependenciesCheckBox);
            this.standardGanttChartShowGroupBox.Controls.Add(this.standardShowExtraDaytimeCheckBox);
            this.standardGanttChartShowGroupBox.Controls.Add(this.standardShowExtraDaysCheckBox);
            this.standardGanttChartShowGroupBox.Controls.Add(this.standardShowResourcesCheckBox);
            this.standardGanttChartShowGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardGanttChartShowGroupBox.Location = new System.Drawing.Point(7, 59);
            this.standardGanttChartShowGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardGanttChartShowGroupBox.Name = "standardGanttChartShowGroupBox";
            this.standardGanttChartShowGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardGanttChartShowGroupBox.Size = new System.Drawing.Size(152, 71);
            this.standardGanttChartShowGroupBox.TabIndex = 5;
            this.standardGanttChartShowGroupBox.TabStop = false;
            this.standardGanttChartShowGroupBox.Text = "Show";
            // 
            // standardShowBaselineCheckBox
            // 
            this.standardShowBaselineCheckBox.AutoSize = true;
            this.standardShowBaselineCheckBox.Location = new System.Drawing.Point(76, 52);
            this.standardShowBaselineCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardShowBaselineCheckBox.Name = "standardShowBaselineCheckBox";
            this.standardShowBaselineCheckBox.Size = new System.Drawing.Size(66, 17);
            this.standardShowBaselineCheckBox.TabIndex = 6;
            this.standardShowBaselineCheckBox.Text = "Baseline";
            this.standardShowBaselineCheckBox.UseVisualStyleBackColor = true;
            this.standardShowBaselineCheckBox.CheckedChanged += new System.EventHandler(this.standardShowBaselineCheckBox_CheckedChanged);
            // 
            // standardHighlightCriticalTasksCheckBox
            // 
            this.standardHighlightCriticalTasksCheckBox.AutoSize = true;
            this.standardHighlightCriticalTasksCheckBox.Location = new System.Drawing.Point(4, 53);
            this.standardHighlightCriticalTasksCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardHighlightCriticalTasksCheckBox.Name = "standardHighlightCriticalTasksCheckBox";
            this.standardHighlightCriticalTasksCheckBox.Size = new System.Drawing.Size(89, 17);
            this.standardHighlightCriticalTasksCheckBox.TabIndex = 5;
            this.standardHighlightCriticalTasksCheckBox.Text = "Critical Tasks";
            this.standardHighlightCriticalTasksCheckBox.UseVisualStyleBackColor = true;
            // 
            // standardShowDependenciesCheckBox
            // 
            this.standardShowDependenciesCheckBox.AutoSize = true;
            this.standardShowDependenciesCheckBox.Location = new System.Drawing.Point(76, 15);
            this.standardShowDependenciesCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardShowDependenciesCheckBox.Name = "standardShowDependenciesCheckBox";
            this.standardShowDependenciesCheckBox.Size = new System.Drawing.Size(95, 17);
            this.standardShowDependenciesCheckBox.TabIndex = 4;
            this.standardShowDependenciesCheckBox.Text = "Dependencies";
            this.standardShowDependenciesCheckBox.UseVisualStyleBackColor = true;
            this.standardShowDependenciesCheckBox.CheckedChanged += new System.EventHandler(this.standardShowDependenciesCheckBox_CheckedChanged);
            // 
            // standardShowExtraDaytimeCheckBox
            // 
            this.standardShowExtraDaytimeCheckBox.AutoSize = true;
            this.standardShowExtraDaytimeCheckBox.Location = new System.Drawing.Point(76, 33);
            this.standardShowExtraDaytimeCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardShowExtraDaytimeCheckBox.Name = "standardShowExtraDaytimeCheckBox";
            this.standardShowExtraDaytimeCheckBox.Size = new System.Drawing.Size(91, 17);
            this.standardShowExtraDaytimeCheckBox.TabIndex = 4;
            this.standardShowExtraDaytimeCheckBox.Text = "Extra Daytime";
            this.standardShowExtraDaytimeCheckBox.UseVisualStyleBackColor = true;
            this.standardShowExtraDaytimeCheckBox.CheckedChanged += new System.EventHandler(this.dataTableShowExtraDaytimeCheckBox_CheckedChanged);
            // 
            // standardShowExtraDaysCheckBox
            // 
            this.standardShowExtraDaysCheckBox.AutoSize = true;
            this.standardShowExtraDaysCheckBox.Location = new System.Drawing.Point(4, 34);
            this.standardShowExtraDaysCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardShowExtraDaysCheckBox.Name = "standardShowExtraDaysCheckBox";
            this.standardShowExtraDaysCheckBox.Size = new System.Drawing.Size(77, 17);
            this.standardShowExtraDaysCheckBox.TabIndex = 4;
            this.standardShowExtraDaysCheckBox.Text = "Extra Days";
            this.standardShowExtraDaysCheckBox.UseVisualStyleBackColor = true;
            this.standardShowExtraDaysCheckBox.CheckedChanged += new System.EventHandler(this.standardShowExtraDaysCheckBox_CheckedChanged);
            // 
            // standardShowResourcesCheckBox
            // 
            this.standardShowResourcesCheckBox.AutoSize = true;
            this.standardShowResourcesCheckBox.Location = new System.Drawing.Point(4, 15);
            this.standardShowResourcesCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardShowResourcesCheckBox.Name = "standardShowResourcesCheckBox";
            this.standardShowResourcesCheckBox.Size = new System.Drawing.Size(77, 17);
            this.standardShowResourcesCheckBox.TabIndex = 4;
            this.standardShowResourcesCheckBox.Text = "Resources";
            this.standardShowResourcesCheckBox.UseVisualStyleBackColor = true;
            // 
            // standardColorComboBox
            // 
            this.standardColorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.standardColorComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardColorComboBox.FormattingEnabled = true;
            this.standardColorComboBox.Items.AddRange(new object[] {
            "Blue (Gradient)",
            "Blue (Standard)",
            "Green (Standard)",
            "Blue (Diagonal)",
            "Green (Gradient)"});
            this.standardColorComboBox.Location = new System.Drawing.Point(82, 37);
            this.standardColorComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardColorComboBox.Name = "standardColorComboBox";
            this.standardColorComboBox.Size = new System.Drawing.Size(72, 21);
            this.standardColorComboBox.TabIndex = 3;
            // 
            // standardCalendarComboBox
            // 
            this.standardCalendarComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.standardCalendarComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardCalendarComboBox.FormattingEnabled = true;
            this.standardCalendarComboBox.Items.AddRange(new object[] {
            "Mon-Fri 8h",
            "Mon-Fri 6h, Sat 3h"});
            this.standardCalendarComboBox.Location = new System.Drawing.Point(82, 15);
            this.standardCalendarComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardCalendarComboBox.Name = "standardCalendarComboBox";
            this.standardCalendarComboBox.Size = new System.Drawing.Size(72, 21);
            this.standardCalendarComboBox.TabIndex = 3;
            this.standardCalendarComboBox.SelectedIndexChanged += new System.EventHandler(this.standardCalendarComboBox_SelectedIndexChanged);
            // 
            // standardUpdateScaleComboBox
            // 
            this.standardUpdateScaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.standardUpdateScaleComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardUpdateScaleComboBox.FormattingEnabled = true;
            this.standardUpdateScaleComboBox.Items.AddRange(new object[] {
            "Hours Update",
            "Days Update",
            "Scaled Update",
            "Free Update"});
            this.standardUpdateScaleComboBox.Location = new System.Drawing.Point(7, 37);
            this.standardUpdateScaleComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardUpdateScaleComboBox.Name = "standardUpdateScaleComboBox";
            this.standardUpdateScaleComboBox.Size = new System.Drawing.Size(72, 21);
            this.standardUpdateScaleComboBox.TabIndex = 3;
            // 
            // standardTasksTreeGridGroupBox
            // 
            this.standardTasksTreeGridGroupBox.Controls.Add(this.standardFixedGridCheckBox);
            this.standardTasksTreeGridGroupBox.Controls.Add(this.standardShowGridCheckBox);
            this.standardTasksTreeGridGroupBox.Enabled = false;
            this.standardTasksTreeGridGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardTasksTreeGridGroupBox.Location = new System.Drawing.Point(4, 39);
            this.standardTasksTreeGridGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardTasksTreeGridGroupBox.Name = "standardTasksTreeGridGroupBox";
            this.standardTasksTreeGridGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardTasksTreeGridGroupBox.Size = new System.Drawing.Size(164, 34);
            this.standardTasksTreeGridGroupBox.TabIndex = 6;
            this.standardTasksTreeGridGroupBox.TabStop = false;
            this.standardTasksTreeGridGroupBox.Text = "Tasks Tree-Grid";
            // 
            // standardFixedGridCheckBox
            // 
            this.standardFixedGridCheckBox.AutoSize = true;
            this.standardFixedGridCheckBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardFixedGridCheckBox.Location = new System.Drawing.Point(67, 15);
            this.standardFixedGridCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardFixedGridCheckBox.Name = "standardFixedGridCheckBox";
            this.standardFixedGridCheckBox.Size = new System.Drawing.Size(129, 17);
            this.standardFixedGridCheckBox.TabIndex = 4;
            this.standardFixedGridCheckBox.Text = "Fixed Grid (on Resize)";
            this.standardFixedGridCheckBox.UseVisualStyleBackColor = true;
            // 
            // standardShowGridCheckBox
            // 
            this.standardShowGridCheckBox.AutoSize = true;
            this.standardShowGridCheckBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardShowGridCheckBox.Location = new System.Drawing.Point(6, 15);
            this.standardShowGridCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardShowGridCheckBox.Name = "standardShowGridCheckBox";
            this.standardShowGridCheckBox.Size = new System.Drawing.Size(75, 17);
            this.standardShowGridCheckBox.TabIndex = 4;
            this.standardShowGridCheckBox.Text = "Show Grid";
            this.standardShowGridCheckBox.UseVisualStyleBackColor = true;
            this.standardShowGridCheckBox.CheckedChanged += new System.EventHandler(this.standardShowGridCheckBox_CheckedChanged);
            // 
            // standardReinitializeButton
            // 
            this.standardReinitializeButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardReinitializeButton.Location = new System.Drawing.Point(4, 15);
            this.standardReinitializeButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardReinitializeButton.Name = "standardReinitializeButton";
            this.standardReinitializeButton.Size = new System.Drawing.Size(80, 19);
            this.standardReinitializeButton.TabIndex = 0;
            this.standardReinitializeButton.Text = "Initialize";
            this.standardReinitializeButton.UseVisualStyleBackColor = true;
            this.standardReinitializeButton.Click += new System.EventHandler(this.standardReinitializeButton_Click);
            // 
            // standardClearButton
            // 
            this.standardClearButton.Enabled = false;
            this.standardClearButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardClearButton.Location = new System.Drawing.Point(88, 15);
            this.standardClearButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.standardClearButton.Name = "standardClearButton";
            this.standardClearButton.Size = new System.Drawing.Size(80, 19);
            this.standardClearButton.TabIndex = 1;
            this.standardClearButton.Text = "Clear";
            this.standardClearButton.UseVisualStyleBackColor = true;
            this.standardClearButton.Click += new System.EventHandler(this.standardClearButton_Click);
            // 
            // dataTableTabPage
            // 
            this.dataTableTabPage.Controls.Add(this.dataTableDisabledLabel);
            this.dataTableTabPage.Controls.Add(this.dataTableRowGroupBox);
            this.dataTableTabPage.Controls.Add(this.dataTableMainGroupBox);
            this.dataTableTabPage.Location = new System.Drawing.Point(4, 22);
            this.dataTableTabPage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableTabPage.Name = "dataTableTabPage";
            this.dataTableTabPage.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableTabPage.Size = new System.Drawing.Size(194, 648);
            this.dataTableTabPage.TabIndex = 1;
            this.dataTableTabPage.Text = "DataTable binding";
            this.dataTableTabPage.UseVisualStyleBackColor = true;
            // 
            // dataTableDisabledLabel
            // 
            this.dataTableDisabledLabel.Location = new System.Drawing.Point(4, 443);
            this.dataTableDisabledLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dataTableDisabledLabel.Name = "dataTableDisabledLabel";
            this.dataTableDisabledLabel.Size = new System.Drawing.Size(172, 56);
            this.dataTableDisabledLabel.TabIndex = 2;
            this.dataTableDisabledLabel.Text = resources.GetString("dataTableDisabledLabel.Text");
            this.dataTableDisabledLabel.Visible = false;
            // 
            // dataTableRowGroupBox
            // 
            this.dataTableRowGroupBox.Controls.Add(this.dataTableMarkersGroupBox);
            this.dataTableRowGroupBox.Controls.Add(this.dataTableInterruptionsGroupBox);
            this.dataTableRowGroupBox.Controls.Add(this.dataTableSetColorLabel);
            this.dataTableRowGroupBox.Controls.Add(this.dataTableSetColorComboBox);
            this.dataTableRowGroupBox.Controls.Add(this.dataTableSetIconLabel);
            this.dataTableRowGroupBox.Controls.Add(this.dataTableSetIconComboBox);
            this.dataTableRowGroupBox.Controls.Add(this.dataTableIncreaseIndentButton);
            this.dataTableRowGroupBox.Controls.Add(this.dataTableDecreaseIndentButton);
            this.dataTableRowGroupBox.Controls.Add(this.dataTableExpandButton);
            this.dataTableRowGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableRowGroupBox.Location = new System.Drawing.Point(4, 272);
            this.dataTableRowGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableRowGroupBox.Name = "dataTableRowGroupBox";
            this.dataTableRowGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableRowGroupBox.Size = new System.Drawing.Size(172, 168);
            this.dataTableRowGroupBox.TabIndex = 1;
            this.dataTableRowGroupBox.TabStop = false;
            this.dataTableRowGroupBox.Text = "Current Task";
            // 
            // dataTableMarkersGroupBox
            // 
            this.dataTableMarkersGroupBox.Controls.Add(this.dataTableMarkerAtLabel);
            this.dataTableMarkersGroupBox.Controls.Add(this.dataTableMarkerAtNumericUpDown);
            this.dataTableMarkersGroupBox.Controls.Add(this.dataTableMarkersClearButton);
            this.dataTableMarkersGroupBox.Controls.Add(this.dataTableMarkerAddButton);
            this.dataTableMarkersGroupBox.Controls.Add(this.dataTableMarkerTypeComboBox);
            this.dataTableMarkersGroupBox.Enabled = false;
            this.dataTableMarkersGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableMarkersGroupBox.Location = new System.Drawing.Point(4, 127);
            this.dataTableMarkersGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableMarkersGroupBox.Name = "dataTableMarkersGroupBox";
            this.dataTableMarkersGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableMarkersGroupBox.Size = new System.Drawing.Size(164, 37);
            this.dataTableMarkersGroupBox.TabIndex = 8;
            this.dataTableMarkersGroupBox.TabStop = false;
            this.dataTableMarkersGroupBox.Text = "Markers";
            // 
            // dataTableMarkerAtLabel
            // 
            this.dataTableMarkerAtLabel.AutoSize = true;
            this.dataTableMarkerAtLabel.Location = new System.Drawing.Point(4, 17);
            this.dataTableMarkerAtLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dataTableMarkerAtLabel.Name = "dataTableMarkerAtLabel";
            this.dataTableMarkerAtLabel.Size = new System.Drawing.Size(17, 13);
            this.dataTableMarkerAtLabel.TabIndex = 2;
            this.dataTableMarkerAtLabel.Text = "At";
            // 
            // dataTableMarkerAtNumericUpDown
            // 
            this.dataTableMarkerAtNumericUpDown.Location = new System.Drawing.Point(16, 15);
            this.dataTableMarkerAtNumericUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableMarkerAtNumericUpDown.Maximum = new decimal(new int[] {
            96,
            0,
            0,
            0});
            this.dataTableMarkerAtNumericUpDown.Name = "dataTableMarkerAtNumericUpDown";
            this.dataTableMarkerAtNumericUpDown.Size = new System.Drawing.Size(27, 20);
            this.dataTableMarkerAtNumericUpDown.TabIndex = 1;
            this.dataTableMarkerAtNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dataTableMarkerAtNumericUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // dataTableMarkersClearButton
            // 
            this.dataTableMarkersClearButton.Location = new System.Drawing.Point(126, 13);
            this.dataTableMarkersClearButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableMarkersClearButton.Name = "dataTableMarkersClearButton";
            this.dataTableMarkersClearButton.Size = new System.Drawing.Size(33, 19);
            this.dataTableMarkersClearButton.TabIndex = 0;
            this.dataTableMarkersClearButton.Text = "Clear";
            this.dataTableMarkersClearButton.UseVisualStyleBackColor = true;
            this.dataTableMarkersClearButton.Click += new System.EventHandler(this.dataTableMarkersClearButton_Click);
            // 
            // dataTableMarkerAddButton
            // 
            this.dataTableMarkerAddButton.Location = new System.Drawing.Point(94, 13);
            this.dataTableMarkerAddButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableMarkerAddButton.Name = "dataTableMarkerAddButton";
            this.dataTableMarkerAddButton.Size = new System.Drawing.Size(28, 19);
            this.dataTableMarkerAddButton.TabIndex = 0;
            this.dataTableMarkerAddButton.Text = "Set";
            this.dataTableMarkerAddButton.UseVisualStyleBackColor = true;
            this.dataTableMarkerAddButton.Click += new System.EventHandler(this.dataTableMarkerAddButton_Click);
            // 
            // dataTableMarkerTypeComboBox
            // 
            this.dataTableMarkerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataTableMarkerTypeComboBox.FormattingEnabled = true;
            this.dataTableMarkerTypeComboBox.Items.AddRange(new object[] {
            "Light",
            "Dark"});
            this.dataTableMarkerTypeComboBox.Location = new System.Drawing.Point(46, 15);
            this.dataTableMarkerTypeComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableMarkerTypeComboBox.Name = "dataTableMarkerTypeComboBox";
            this.dataTableMarkerTypeComboBox.Size = new System.Drawing.Size(45, 21);
            this.dataTableMarkerTypeComboBox.TabIndex = 2;
            // 
            // dataTableInterruptionsGroupBox
            // 
            this.dataTableInterruptionsGroupBox.Controls.Add(this.dataTableInterruptionForLabel);
            this.dataTableInterruptionsGroupBox.Controls.Add(this.dataTableInterruptionAtLabel);
            this.dataTableInterruptionsGroupBox.Controls.Add(this.dataTableInterruptionForNumericUpDown);
            this.dataTableInterruptionsGroupBox.Controls.Add(this.dataTableInterruptionAtNumericUpDown);
            this.dataTableInterruptionsGroupBox.Controls.Add(this.dataTableInterruptionsClearButton);
            this.dataTableInterruptionsGroupBox.Controls.Add(this.dataTableInterruptionAddButton);
            this.dataTableInterruptionsGroupBox.Enabled = false;
            this.dataTableInterruptionsGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableInterruptionsGroupBox.Location = new System.Drawing.Point(4, 84);
            this.dataTableInterruptionsGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableInterruptionsGroupBox.Name = "dataTableInterruptionsGroupBox";
            this.dataTableInterruptionsGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableInterruptionsGroupBox.Size = new System.Drawing.Size(164, 37);
            this.dataTableInterruptionsGroupBox.TabIndex = 9;
            this.dataTableInterruptionsGroupBox.TabStop = false;
            this.dataTableInterruptionsGroupBox.Text = "Interruptions";
            // 
            // dataTableInterruptionForLabel
            // 
            this.dataTableInterruptionForLabel.AutoSize = true;
            this.dataTableInterruptionForLabel.Location = new System.Drawing.Point(46, 17);
            this.dataTableInterruptionForLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dataTableInterruptionForLabel.Name = "dataTableInterruptionForLabel";
            this.dataTableInterruptionForLabel.Size = new System.Drawing.Size(19, 13);
            this.dataTableInterruptionForLabel.TabIndex = 2;
            this.dataTableInterruptionForLabel.Text = "for";
            // 
            // dataTableInterruptionAtLabel
            // 
            this.dataTableInterruptionAtLabel.AutoSize = true;
            this.dataTableInterruptionAtLabel.Location = new System.Drawing.Point(4, 17);
            this.dataTableInterruptionAtLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dataTableInterruptionAtLabel.Name = "dataTableInterruptionAtLabel";
            this.dataTableInterruptionAtLabel.Size = new System.Drawing.Size(17, 13);
            this.dataTableInterruptionAtLabel.TabIndex = 2;
            this.dataTableInterruptionAtLabel.Text = "At";
            // 
            // dataTableInterruptionForNumericUpDown
            // 
            this.dataTableInterruptionForNumericUpDown.Location = new System.Drawing.Point(64, 15);
            this.dataTableInterruptionForNumericUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableInterruptionForNumericUpDown.Maximum = new decimal(new int[] {
            96,
            0,
            0,
            0});
            this.dataTableInterruptionForNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dataTableInterruptionForNumericUpDown.Name = "dataTableInterruptionForNumericUpDown";
            this.dataTableInterruptionForNumericUpDown.Size = new System.Drawing.Size(27, 20);
            this.dataTableInterruptionForNumericUpDown.TabIndex = 1;
            this.dataTableInterruptionForNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dataTableInterruptionForNumericUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // dataTableInterruptionAtNumericUpDown
            // 
            this.dataTableInterruptionAtNumericUpDown.Location = new System.Drawing.Point(16, 15);
            this.dataTableInterruptionAtNumericUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableInterruptionAtNumericUpDown.Maximum = new decimal(new int[] {
            96,
            0,
            0,
            0});
            this.dataTableInterruptionAtNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dataTableInterruptionAtNumericUpDown.Name = "dataTableInterruptionAtNumericUpDown";
            this.dataTableInterruptionAtNumericUpDown.Size = new System.Drawing.Size(27, 20);
            this.dataTableInterruptionAtNumericUpDown.TabIndex = 1;
            this.dataTableInterruptionAtNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dataTableInterruptionAtNumericUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // dataTableInterruptionsClearButton
            // 
            this.dataTableInterruptionsClearButton.Location = new System.Drawing.Point(126, 13);
            this.dataTableInterruptionsClearButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableInterruptionsClearButton.Name = "dataTableInterruptionsClearButton";
            this.dataTableInterruptionsClearButton.Size = new System.Drawing.Size(33, 19);
            this.dataTableInterruptionsClearButton.TabIndex = 0;
            this.dataTableInterruptionsClearButton.Text = "Clear";
            this.dataTableInterruptionsClearButton.UseVisualStyleBackColor = true;
            this.dataTableInterruptionsClearButton.Click += new System.EventHandler(this.dataTableInterruptionsClearButton_Click);
            // 
            // dataTableInterruptionAddButton
            // 
            this.dataTableInterruptionAddButton.Location = new System.Drawing.Point(94, 13);
            this.dataTableInterruptionAddButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableInterruptionAddButton.Name = "dataTableInterruptionAddButton";
            this.dataTableInterruptionAddButton.Size = new System.Drawing.Size(28, 19);
            this.dataTableInterruptionAddButton.TabIndex = 0;
            this.dataTableInterruptionAddButton.Text = "Set";
            this.dataTableInterruptionAddButton.UseVisualStyleBackColor = true;
            this.dataTableInterruptionAddButton.Click += new System.EventHandler(this.dataTableInterruptionAddButton_Click);
            // 
            // dataTableSetColorLabel
            // 
            this.dataTableSetColorLabel.AutoSize = true;
            this.dataTableSetColorLabel.Enabled = false;
            this.dataTableSetColorLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableSetColorLabel.Location = new System.Drawing.Point(4, 65);
            this.dataTableSetColorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dataTableSetColorLabel.Name = "dataTableSetColorLabel";
            this.dataTableSetColorLabel.Size = new System.Drawing.Size(53, 13);
            this.dataTableSetColorLabel.TabIndex = 6;
            this.dataTableSetColorLabel.Text = "Set Color:";
            // 
            // dataTableSetColorComboBox
            // 
            this.dataTableSetColorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataTableSetColorComboBox.Enabled = false;
            this.dataTableSetColorComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableSetColorComboBox.FormattingEnabled = true;
            this.dataTableSetColorComboBox.Items.AddRange(new object[] {
            "Blue (Gradient)",
            "Blue (Standard)",
            "Green (Standard)",
            "Blue (Diagonal)",
            "Green (Gradient)"});
            this.dataTableSetColorComboBox.Location = new System.Drawing.Point(49, 63);
            this.dataTableSetColorComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableSetColorComboBox.Name = "dataTableSetColorComboBox";
            this.dataTableSetColorComboBox.Size = new System.Drawing.Size(120, 21);
            this.dataTableSetColorComboBox.TabIndex = 7;
            this.dataTableSetColorComboBox.SelectedIndexChanged += new System.EventHandler(this.dataTableSetColorComboBox_SelectedIndexChanged);
            // 
            // dataTableSetIconLabel
            // 
            this.dataTableSetIconLabel.AutoSize = true;
            this.dataTableSetIconLabel.Enabled = false;
            this.dataTableSetIconLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableSetIconLabel.Location = new System.Drawing.Point(88, 20);
            this.dataTableSetIconLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dataTableSetIconLabel.Name = "dataTableSetIconLabel";
            this.dataTableSetIconLabel.Size = new System.Drawing.Size(50, 13);
            this.dataTableSetIconLabel.TabIndex = 1;
            this.dataTableSetIconLabel.Text = "Set Icon:";
            // 
            // dataTableSetIconComboBox
            // 
            this.dataTableSetIconComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataTableSetIconComboBox.Enabled = false;
            this.dataTableSetIconComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableSetIconComboBox.FormattingEnabled = true;
            this.dataTableSetIconComboBox.Items.AddRange(new object[] {
            "Task",
            "Folder",
            "Note"});
            this.dataTableSetIconComboBox.Location = new System.Drawing.Point(126, 15);
            this.dataTableSetIconComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableSetIconComboBox.Name = "dataTableSetIconComboBox";
            this.dataTableSetIconComboBox.Size = new System.Drawing.Size(43, 21);
            this.dataTableSetIconComboBox.TabIndex = 2;
            // 
            // dataTableIncreaseIndentButton
            // 
            this.dataTableIncreaseIndentButton.Enabled = false;
            this.dataTableIncreaseIndentButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableIncreaseIndentButton.Location = new System.Drawing.Point(88, 39);
            this.dataTableIncreaseIndentButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableIncreaseIndentButton.Name = "dataTableIncreaseIndentButton";
            this.dataTableIncreaseIndentButton.Size = new System.Drawing.Size(80, 19);
            this.dataTableIncreaseIndentButton.TabIndex = 4;
            this.dataTableIncreaseIndentButton.Text = "Increase Indent >";
            this.dataTableIncreaseIndentButton.UseVisualStyleBackColor = true;
            // 
            // dataTableDecreaseIndentButton
            // 
            this.dataTableDecreaseIndentButton.Enabled = false;
            this.dataTableDecreaseIndentButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableDecreaseIndentButton.Location = new System.Drawing.Point(4, 39);
            this.dataTableDecreaseIndentButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableDecreaseIndentButton.Name = "dataTableDecreaseIndentButton";
            this.dataTableDecreaseIndentButton.Size = new System.Drawing.Size(80, 19);
            this.dataTableDecreaseIndentButton.TabIndex = 3;
            this.dataTableDecreaseIndentButton.Text = "< Decrease Indent";
            this.dataTableDecreaseIndentButton.UseVisualStyleBackColor = true;
            // 
            // dataTableExpandButton
            // 
            this.dataTableExpandButton.Enabled = false;
            this.dataTableExpandButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableExpandButton.Location = new System.Drawing.Point(4, 15);
            this.dataTableExpandButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableExpandButton.Name = "dataTableExpandButton";
            this.dataTableExpandButton.Size = new System.Drawing.Size(80, 19);
            this.dataTableExpandButton.TabIndex = 0;
            this.dataTableExpandButton.Text = "Expand/Close";
            this.dataTableExpandButton.UseVisualStyleBackColor = true;
            // 
            // dataTableMainGroupBox
            // 
            this.dataTableMainGroupBox.Controls.Add(this.dataTablePrintPreviewButton);
            this.dataTableMainGroupBox.Controls.Add(this.dataTablePageSetupButton);
            this.dataTableMainGroupBox.Controls.Add(this.dataTableGanttChartGroupBox);
            this.dataTableMainGroupBox.Controls.Add(this.dataTableTasksTreeGridGroupBox);
            this.dataTableMainGroupBox.Controls.Add(this.dataTableReinitializeButton);
            this.dataTableMainGroupBox.Controls.Add(this.dataTableClearButton);
            this.dataTableMainGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableMainGroupBox.Location = new System.Drawing.Point(4, 5);
            this.dataTableMainGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableMainGroupBox.Name = "dataTableMainGroupBox";
            this.dataTableMainGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableMainGroupBox.Size = new System.Drawing.Size(172, 262);
            this.dataTableMainGroupBox.TabIndex = 0;
            this.dataTableMainGroupBox.TabStop = false;
            this.dataTableMainGroupBox.Text = "Data";
            // 
            // dataTablePrintPreviewButton
            // 
            this.dataTablePrintPreviewButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTablePrintPreviewButton.Location = new System.Drawing.Point(88, 236);
            this.dataTablePrintPreviewButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTablePrintPreviewButton.Name = "dataTablePrintPreviewButton";
            this.dataTablePrintPreviewButton.Size = new System.Drawing.Size(80, 19);
            this.dataTablePrintPreviewButton.TabIndex = 7;
            this.dataTablePrintPreviewButton.Text = "Print Preview";
            this.dataTablePrintPreviewButton.UseVisualStyleBackColor = true;
            // 
            // dataTablePageSetupButton
            // 
            this.dataTablePageSetupButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTablePageSetupButton.Location = new System.Drawing.Point(4, 236);
            this.dataTablePageSetupButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTablePageSetupButton.Name = "dataTablePageSetupButton";
            this.dataTablePageSetupButton.Size = new System.Drawing.Size(80, 19);
            this.dataTablePageSetupButton.TabIndex = 6;
            this.dataTablePageSetupButton.Text = "Page Setup";
            this.dataTablePageSetupButton.UseVisualStyleBackColor = true;
            this.dataTablePageSetupButton.Click += new System.EventHandler(this.dataTablePageSetupButton_Click);
            // 
            // dataTableGanttChartGroupBox
            // 
            this.dataTableGanttChartGroupBox.Controls.Add(this.dataTableScaleComboBox);
            this.dataTableGanttChartGroupBox.Controls.Add(this.dataTableGanttChartShowGroupBox);
            this.dataTableGanttChartGroupBox.Controls.Add(this.dataTableColorComboBox);
            this.dataTableGanttChartGroupBox.Controls.Add(this.dataTableCalendarComboBox);
            this.dataTableGanttChartGroupBox.Controls.Add(this.dataTableUpdateScaleComboBox);
            this.dataTableGanttChartGroupBox.Enabled = false;
            this.dataTableGanttChartGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableGanttChartGroupBox.Location = new System.Drawing.Point(4, 78);
            this.dataTableGanttChartGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableGanttChartGroupBox.Name = "dataTableGanttChartGroupBox";
            this.dataTableGanttChartGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableGanttChartGroupBox.Size = new System.Drawing.Size(164, 153);
            this.dataTableGanttChartGroupBox.TabIndex = 7;
            this.dataTableGanttChartGroupBox.TabStop = false;
            this.dataTableGanttChartGroupBox.Text = "Gantt Chart";
            // 
            // dataTableScaleComboBox
            // 
            this.dataTableScaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataTableScaleComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableScaleComboBox.FormattingEnabled = true;
            this.dataTableScaleComboBox.Items.AddRange(new object[] {
            "Days/Hours",
            "Weeks/Days",
            "Months/Weeks",
            "Quarters/Months",
            "Years/Quarters"});
            this.dataTableScaleComboBox.Location = new System.Drawing.Point(7, 15);
            this.dataTableScaleComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableScaleComboBox.Name = "dataTableScaleComboBox";
            this.dataTableScaleComboBox.Size = new System.Drawing.Size(72, 21);
            this.dataTableScaleComboBox.TabIndex = 3;
            // 
            // dataTableGanttChartShowGroupBox
            // 
            this.dataTableGanttChartShowGroupBox.Controls.Add(this.dataTableShowBaselineCheckBox);
            this.dataTableGanttChartShowGroupBox.Controls.Add(this.dataTableHighlightCriticalTasksCheckBox);
            this.dataTableGanttChartShowGroupBox.Controls.Add(this.dataTableShowExtraDaytimeCheckBox);
            this.dataTableGanttChartShowGroupBox.Controls.Add(this.dataTableShowExtraDaysCheckBox);
            this.dataTableGanttChartShowGroupBox.Controls.Add(this.dataTableShowToolTipsCheckBox);
            this.dataTableGanttChartShowGroupBox.Controls.Add(this.dataTableShowDependenciesCheckBox);
            this.dataTableGanttChartShowGroupBox.Controls.Add(this.dataTableShowResourcesCheckBox);
            this.dataTableGanttChartShowGroupBox.Controls.Add(this.dataTableShowMarkersCheckBox);
            this.dataTableGanttChartShowGroupBox.Controls.Add(this.dataTableShowIconsCheckBox);
            this.dataTableGanttChartShowGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableGanttChartShowGroupBox.Location = new System.Drawing.Point(7, 59);
            this.dataTableGanttChartShowGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableGanttChartShowGroupBox.Name = "dataTableGanttChartShowGroupBox";
            this.dataTableGanttChartShowGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableGanttChartShowGroupBox.Size = new System.Drawing.Size(152, 89);
            this.dataTableGanttChartShowGroupBox.TabIndex = 5;
            this.dataTableGanttChartShowGroupBox.TabStop = false;
            this.dataTableGanttChartShowGroupBox.Text = "Show";
            // 
            // dataTableShowBaselineCheckBox
            // 
            this.dataTableShowBaselineCheckBox.AutoSize = true;
            this.dataTableShowBaselineCheckBox.Location = new System.Drawing.Point(76, 72);
            this.dataTableShowBaselineCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableShowBaselineCheckBox.Name = "dataTableShowBaselineCheckBox";
            this.dataTableShowBaselineCheckBox.Size = new System.Drawing.Size(66, 17);
            this.dataTableShowBaselineCheckBox.TabIndex = 8;
            this.dataTableShowBaselineCheckBox.Text = "Baseline";
            this.dataTableShowBaselineCheckBox.UseVisualStyleBackColor = true;
            // 
            // dataTableHighlightCriticalTasksCheckBox
            // 
            this.dataTableHighlightCriticalTasksCheckBox.AutoSize = true;
            this.dataTableHighlightCriticalTasksCheckBox.Location = new System.Drawing.Point(4, 72);
            this.dataTableHighlightCriticalTasksCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableHighlightCriticalTasksCheckBox.Name = "dataTableHighlightCriticalTasksCheckBox";
            this.dataTableHighlightCriticalTasksCheckBox.Size = new System.Drawing.Size(89, 17);
            this.dataTableHighlightCriticalTasksCheckBox.TabIndex = 7;
            this.dataTableHighlightCriticalTasksCheckBox.Text = "Critical Tasks";
            this.dataTableHighlightCriticalTasksCheckBox.UseVisualStyleBackColor = true;
            // 
            // dataTableShowExtraDaytimeCheckBox
            // 
            this.dataTableShowExtraDaytimeCheckBox.AutoSize = true;
            this.dataTableShowExtraDaytimeCheckBox.Location = new System.Drawing.Point(76, 53);
            this.dataTableShowExtraDaytimeCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableShowExtraDaytimeCheckBox.Name = "dataTableShowExtraDaytimeCheckBox";
            this.dataTableShowExtraDaytimeCheckBox.Size = new System.Drawing.Size(91, 17);
            this.dataTableShowExtraDaytimeCheckBox.TabIndex = 6;
            this.dataTableShowExtraDaytimeCheckBox.Text = "Extra Daytime";
            this.dataTableShowExtraDaytimeCheckBox.UseVisualStyleBackColor = true;
            // 
            // dataTableShowExtraDaysCheckBox
            // 
            this.dataTableShowExtraDaysCheckBox.AutoSize = true;
            this.dataTableShowExtraDaysCheckBox.Location = new System.Drawing.Point(4, 53);
            this.dataTableShowExtraDaysCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableShowExtraDaysCheckBox.Name = "dataTableShowExtraDaysCheckBox";
            this.dataTableShowExtraDaysCheckBox.Size = new System.Drawing.Size(77, 17);
            this.dataTableShowExtraDaysCheckBox.TabIndex = 5;
            this.dataTableShowExtraDaysCheckBox.Text = "Extra Days";
            this.dataTableShowExtraDaysCheckBox.UseVisualStyleBackColor = true;
            // 
            // dataTableShowToolTipsCheckBox
            // 
            this.dataTableShowToolTipsCheckBox.AutoSize = true;
            this.dataTableShowToolTipsCheckBox.Location = new System.Drawing.Point(100, 15);
            this.dataTableShowToolTipsCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableShowToolTipsCheckBox.Name = "dataTableShowToolTipsCheckBox";
            this.dataTableShowToolTipsCheckBox.Size = new System.Drawing.Size(63, 17);
            this.dataTableShowToolTipsCheckBox.TabIndex = 4;
            this.dataTableShowToolTipsCheckBox.Text = "Tooltips";
            this.dataTableShowToolTipsCheckBox.UseVisualStyleBackColor = true;
            // 
            // dataTableShowDependenciesCheckBox
            // 
            this.dataTableShowDependenciesCheckBox.AutoSize = true;
            this.dataTableShowDependenciesCheckBox.Location = new System.Drawing.Point(76, 34);
            this.dataTableShowDependenciesCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableShowDependenciesCheckBox.Name = "dataTableShowDependenciesCheckBox";
            this.dataTableShowDependenciesCheckBox.Size = new System.Drawing.Size(95, 17);
            this.dataTableShowDependenciesCheckBox.TabIndex = 4;
            this.dataTableShowDependenciesCheckBox.Text = "Dependencies";
            this.dataTableShowDependenciesCheckBox.UseVisualStyleBackColor = true;
            // 
            // dataTableShowResourcesCheckBox
            // 
            this.dataTableShowResourcesCheckBox.AutoSize = true;
            this.dataTableShowResourcesCheckBox.Location = new System.Drawing.Point(4, 34);
            this.dataTableShowResourcesCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableShowResourcesCheckBox.Name = "dataTableShowResourcesCheckBox";
            this.dataTableShowResourcesCheckBox.Size = new System.Drawing.Size(77, 17);
            this.dataTableShowResourcesCheckBox.TabIndex = 4;
            this.dataTableShowResourcesCheckBox.Text = "Resources";
            this.dataTableShowResourcesCheckBox.UseVisualStyleBackColor = true;
            // 
            // dataTableShowMarkersCheckBox
            // 
            this.dataTableShowMarkersCheckBox.AutoSize = true;
            this.dataTableShowMarkersCheckBox.Location = new System.Drawing.Point(48, 15);
            this.dataTableShowMarkersCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableShowMarkersCheckBox.Name = "dataTableShowMarkersCheckBox";
            this.dataTableShowMarkersCheckBox.Size = new System.Drawing.Size(64, 17);
            this.dataTableShowMarkersCheckBox.TabIndex = 4;
            this.dataTableShowMarkersCheckBox.Text = "Markers";
            this.dataTableShowMarkersCheckBox.UseVisualStyleBackColor = true;
            this.dataTableShowMarkersCheckBox.CheckedChanged += new System.EventHandler(this.dataTableShowMarkersCheckBox_CheckedChanged);
            // 
            // dataTableShowIconsCheckBox
            // 
            this.dataTableShowIconsCheckBox.AutoSize = true;
            this.dataTableShowIconsCheckBox.Location = new System.Drawing.Point(4, 15);
            this.dataTableShowIconsCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableShowIconsCheckBox.Name = "dataTableShowIconsCheckBox";
            this.dataTableShowIconsCheckBox.Size = new System.Drawing.Size(52, 17);
            this.dataTableShowIconsCheckBox.TabIndex = 4;
            this.dataTableShowIconsCheckBox.Text = "Icons";
            this.dataTableShowIconsCheckBox.UseVisualStyleBackColor = true;
            // 
            // dataTableColorComboBox
            // 
            this.dataTableColorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataTableColorComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableColorComboBox.FormattingEnabled = true;
            this.dataTableColorComboBox.Items.AddRange(new object[] {
            "Blue (Gradient)",
            "Blue (Standard)",
            "Green (Standard)",
            "Blue (Diagonal)",
            "Green (Gradient)"});
            this.dataTableColorComboBox.Location = new System.Drawing.Point(82, 37);
            this.dataTableColorComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableColorComboBox.Name = "dataTableColorComboBox";
            this.dataTableColorComboBox.Size = new System.Drawing.Size(72, 21);
            this.dataTableColorComboBox.TabIndex = 3;
            this.dataTableColorComboBox.SelectedIndexChanged += new System.EventHandler(this.dataTableColorComboBox_SelectedIndexChanged);
            // 
            // dataTableCalendarComboBox
            // 
            this.dataTableCalendarComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataTableCalendarComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableCalendarComboBox.FormattingEnabled = true;
            this.dataTableCalendarComboBox.Items.AddRange(new object[] {
            "Mon-Fri 8h",
            "Mon-Fri 6h, Sat 3h"});
            this.dataTableCalendarComboBox.Location = new System.Drawing.Point(82, 15);
            this.dataTableCalendarComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableCalendarComboBox.Name = "dataTableCalendarComboBox";
            this.dataTableCalendarComboBox.Size = new System.Drawing.Size(72, 21);
            this.dataTableCalendarComboBox.TabIndex = 3;
            // 
            // dataTableUpdateScaleComboBox
            // 
            this.dataTableUpdateScaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataTableUpdateScaleComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableUpdateScaleComboBox.FormattingEnabled = true;
            this.dataTableUpdateScaleComboBox.Items.AddRange(new object[] {
            "Hours Update",
            "Days Update",
            "Scaled Update",
            "Free Update"});
            this.dataTableUpdateScaleComboBox.Location = new System.Drawing.Point(7, 37);
            this.dataTableUpdateScaleComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableUpdateScaleComboBox.Name = "dataTableUpdateScaleComboBox";
            this.dataTableUpdateScaleComboBox.Size = new System.Drawing.Size(72, 21);
            this.dataTableUpdateScaleComboBox.TabIndex = 3;
            // 
            // dataTableTasksTreeGridGroupBox
            // 
            this.dataTableTasksTreeGridGroupBox.Controls.Add(this.dataTableFixedGridCheckBox);
            this.dataTableTasksTreeGridGroupBox.Controls.Add(this.dataTableShowGridCheckBox);
            this.dataTableTasksTreeGridGroupBox.Enabled = false;
            this.dataTableTasksTreeGridGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableTasksTreeGridGroupBox.Location = new System.Drawing.Point(4, 39);
            this.dataTableTasksTreeGridGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableTasksTreeGridGroupBox.Name = "dataTableTasksTreeGridGroupBox";
            this.dataTableTasksTreeGridGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableTasksTreeGridGroupBox.Size = new System.Drawing.Size(164, 34);
            this.dataTableTasksTreeGridGroupBox.TabIndex = 6;
            this.dataTableTasksTreeGridGroupBox.TabStop = false;
            this.dataTableTasksTreeGridGroupBox.Text = "Tasks Tree-Grid";
            // 
            // dataTableFixedGridCheckBox
            // 
            this.dataTableFixedGridCheckBox.AutoSize = true;
            this.dataTableFixedGridCheckBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableFixedGridCheckBox.Location = new System.Drawing.Point(67, 15);
            this.dataTableFixedGridCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableFixedGridCheckBox.Name = "dataTableFixedGridCheckBox";
            this.dataTableFixedGridCheckBox.Size = new System.Drawing.Size(129, 17);
            this.dataTableFixedGridCheckBox.TabIndex = 4;
            this.dataTableFixedGridCheckBox.Text = "Fixed Grid (on Resize)";
            this.dataTableFixedGridCheckBox.UseVisualStyleBackColor = true;
            this.dataTableFixedGridCheckBox.CheckedChanged += new System.EventHandler(this.dataTableFixedGridCheckBox_CheckedChanged);
            // 
            // dataTableShowGridCheckBox
            // 
            this.dataTableShowGridCheckBox.AutoSize = true;
            this.dataTableShowGridCheckBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableShowGridCheckBox.Location = new System.Drawing.Point(6, 15);
            this.dataTableShowGridCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableShowGridCheckBox.Name = "dataTableShowGridCheckBox";
            this.dataTableShowGridCheckBox.Size = new System.Drawing.Size(75, 17);
            this.dataTableShowGridCheckBox.TabIndex = 4;
            this.dataTableShowGridCheckBox.Text = "Show Grid";
            this.dataTableShowGridCheckBox.UseVisualStyleBackColor = true;
            // 
            // dataTableReinitializeButton
            // 
            this.dataTableReinitializeButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableReinitializeButton.Location = new System.Drawing.Point(4, 15);
            this.dataTableReinitializeButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableReinitializeButton.Name = "dataTableReinitializeButton";
            this.dataTableReinitializeButton.Size = new System.Drawing.Size(80, 19);
            this.dataTableReinitializeButton.TabIndex = 0;
            this.dataTableReinitializeButton.Text = "Initialize";
            this.dataTableReinitializeButton.UseVisualStyleBackColor = true;
            this.dataTableReinitializeButton.Click += new System.EventHandler(this.dataTableReinitializeButton_Click);
            // 
            // dataTableClearButton
            // 
            this.dataTableClearButton.Enabled = false;
            this.dataTableClearButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTableClearButton.Location = new System.Drawing.Point(88, 15);
            this.dataTableClearButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataTableClearButton.Name = "dataTableClearButton";
            this.dataTableClearButton.Size = new System.Drawing.Size(80, 19);
            this.dataTableClearButton.TabIndex = 1;
            this.dataTableClearButton.Text = "Clear";
            this.dataTableClearButton.UseVisualStyleBackColor = true;
            // 
            // objectsTabPage
            // 
            this.objectsTabPage.Controls.Add(this.objectsDisabledLabel);
            this.objectsTabPage.Controls.Add(this.objectsRowGroupBox);
            this.objectsTabPage.Controls.Add(this.objectsMainGroupBox);
            this.objectsTabPage.Location = new System.Drawing.Point(4, 22);
            this.objectsTabPage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsTabPage.Name = "objectsTabPage";
            this.objectsTabPage.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsTabPage.Size = new System.Drawing.Size(194, 648);
            this.objectsTabPage.TabIndex = 2;
            this.objectsTabPage.Text = "Object binding";
            this.objectsTabPage.UseVisualStyleBackColor = true;
            // 
            // objectsDisabledLabel
            // 
            this.objectsDisabledLabel.Location = new System.Drawing.Point(4, 443);
            this.objectsDisabledLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.objectsDisabledLabel.Name = "objectsDisabledLabel";
            this.objectsDisabledLabel.Size = new System.Drawing.Size(172, 56);
            this.objectsDisabledLabel.TabIndex = 2;
            this.objectsDisabledLabel.Text = resources.GetString("objectsDisabledLabel.Text");
            this.objectsDisabledLabel.Visible = false;
            // 
            // objectsRowGroupBox
            // 
            this.objectsRowGroupBox.Controls.Add(this.objectsMarkersGroupBox);
            this.objectsRowGroupBox.Controls.Add(this.objectsInterruptionsGroupBox);
            this.objectsRowGroupBox.Controls.Add(this.objectsSetColorLabel);
            this.objectsRowGroupBox.Controls.Add(this.objectsSetIconLabel);
            this.objectsRowGroupBox.Controls.Add(this.objectsSetColorComboBox);
            this.objectsRowGroupBox.Controls.Add(this.objectsSetIconComboBox);
            this.objectsRowGroupBox.Controls.Add(this.objectsIncreaseIndentButton);
            this.objectsRowGroupBox.Controls.Add(this.objectsDecreaseIndentButton);
            this.objectsRowGroupBox.Controls.Add(this.objectsExpandButton);
            this.objectsRowGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsRowGroupBox.Location = new System.Drawing.Point(4, 272);
            this.objectsRowGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsRowGroupBox.Name = "objectsRowGroupBox";
            this.objectsRowGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsRowGroupBox.Size = new System.Drawing.Size(172, 168);
            this.objectsRowGroupBox.TabIndex = 1;
            this.objectsRowGroupBox.TabStop = false;
            this.objectsRowGroupBox.Text = "Current Task";
            // 
            // objectsMarkersGroupBox
            // 
            this.objectsMarkersGroupBox.Controls.Add(this.objectsMarkerAtLabel);
            this.objectsMarkersGroupBox.Controls.Add(this.objectsMarkerAtNumericUpDown);
            this.objectsMarkersGroupBox.Controls.Add(this.objectsMarkersClearButton);
            this.objectsMarkersGroupBox.Controls.Add(this.objectsMarkerAddButton);
            this.objectsMarkersGroupBox.Controls.Add(this.objectsMarkerTypeComboBox);
            this.objectsMarkersGroupBox.Enabled = false;
            this.objectsMarkersGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsMarkersGroupBox.Location = new System.Drawing.Point(4, 127);
            this.objectsMarkersGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsMarkersGroupBox.Name = "objectsMarkersGroupBox";
            this.objectsMarkersGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsMarkersGroupBox.Size = new System.Drawing.Size(164, 37);
            this.objectsMarkersGroupBox.TabIndex = 5;
            this.objectsMarkersGroupBox.TabStop = false;
            this.objectsMarkersGroupBox.Text = "Markers";
            // 
            // objectsMarkerAtLabel
            // 
            this.objectsMarkerAtLabel.AutoSize = true;
            this.objectsMarkerAtLabel.Location = new System.Drawing.Point(4, 17);
            this.objectsMarkerAtLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.objectsMarkerAtLabel.Name = "objectsMarkerAtLabel";
            this.objectsMarkerAtLabel.Size = new System.Drawing.Size(17, 13);
            this.objectsMarkerAtLabel.TabIndex = 2;
            this.objectsMarkerAtLabel.Text = "At";
            // 
            // objectsMarkerAtNumericUpDown
            // 
            this.objectsMarkerAtNumericUpDown.Location = new System.Drawing.Point(16, 15);
            this.objectsMarkerAtNumericUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsMarkerAtNumericUpDown.Maximum = new decimal(new int[] {
            96,
            0,
            0,
            0});
            this.objectsMarkerAtNumericUpDown.Minimum = new decimal(new int[] {
            24,
            0,
            0,
            -2147483648});
            this.objectsMarkerAtNumericUpDown.Name = "objectsMarkerAtNumericUpDown";
            this.objectsMarkerAtNumericUpDown.Size = new System.Drawing.Size(27, 20);
            this.objectsMarkerAtNumericUpDown.TabIndex = 1;
            this.objectsMarkerAtNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // objectsMarkersClearButton
            // 
            this.objectsMarkersClearButton.Location = new System.Drawing.Point(126, 13);
            this.objectsMarkersClearButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsMarkersClearButton.Name = "objectsMarkersClearButton";
            this.objectsMarkersClearButton.Size = new System.Drawing.Size(33, 19);
            this.objectsMarkersClearButton.TabIndex = 0;
            this.objectsMarkersClearButton.Text = "Clear";
            this.objectsMarkersClearButton.UseVisualStyleBackColor = true;
            this.objectsMarkersClearButton.Click += new System.EventHandler(this.objectsMarkersClearButton_Click);
            // 
            // objectsMarkerAddButton
            // 
            this.objectsMarkerAddButton.Location = new System.Drawing.Point(94, 13);
            this.objectsMarkerAddButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsMarkerAddButton.Name = "objectsMarkerAddButton";
            this.objectsMarkerAddButton.Size = new System.Drawing.Size(28, 19);
            this.objectsMarkerAddButton.TabIndex = 0;
            this.objectsMarkerAddButton.Text = "Set";
            this.objectsMarkerAddButton.UseVisualStyleBackColor = true;
            this.objectsMarkerAddButton.Click += new System.EventHandler(this.objectsMarkerAddButton_Click);
            // 
            // objectsMarkerTypeComboBox
            // 
            this.objectsMarkerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.objectsMarkerTypeComboBox.FormattingEnabled = true;
            this.objectsMarkerTypeComboBox.Items.AddRange(new object[] {
            "Light",
            "Dark"});
            this.objectsMarkerTypeComboBox.Location = new System.Drawing.Point(46, 15);
            this.objectsMarkerTypeComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsMarkerTypeComboBox.Name = "objectsMarkerTypeComboBox";
            this.objectsMarkerTypeComboBox.Size = new System.Drawing.Size(45, 21);
            this.objectsMarkerTypeComboBox.TabIndex = 2;
            // 
            // objectsInterruptionsGroupBox
            // 
            this.objectsInterruptionsGroupBox.Controls.Add(this.objectsInterruptionForLabel);
            this.objectsInterruptionsGroupBox.Controls.Add(this.objectsInterruptionAtLabel);
            this.objectsInterruptionsGroupBox.Controls.Add(this.objectsInterruptionForNumericUpDown);
            this.objectsInterruptionsGroupBox.Controls.Add(this.objectsInterruptionAtNumericUpDown);
            this.objectsInterruptionsGroupBox.Controls.Add(this.objectsInterruptionsClear);
            this.objectsInterruptionsGroupBox.Controls.Add(this.objectsInterruptionAddButton);
            this.objectsInterruptionsGroupBox.Enabled = false;
            this.objectsInterruptionsGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsInterruptionsGroupBox.Location = new System.Drawing.Point(4, 84);
            this.objectsInterruptionsGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsInterruptionsGroupBox.Name = "objectsInterruptionsGroupBox";
            this.objectsInterruptionsGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsInterruptionsGroupBox.Size = new System.Drawing.Size(164, 37);
            this.objectsInterruptionsGroupBox.TabIndex = 5;
            this.objectsInterruptionsGroupBox.TabStop = false;
            this.objectsInterruptionsGroupBox.Text = "Interruptions";
            // 
            // objectsInterruptionForLabel
            // 
            this.objectsInterruptionForLabel.AutoSize = true;
            this.objectsInterruptionForLabel.Location = new System.Drawing.Point(46, 17);
            this.objectsInterruptionForLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.objectsInterruptionForLabel.Name = "objectsInterruptionForLabel";
            this.objectsInterruptionForLabel.Size = new System.Drawing.Size(19, 13);
            this.objectsInterruptionForLabel.TabIndex = 2;
            this.objectsInterruptionForLabel.Text = "for";
            // 
            // objectsInterruptionAtLabel
            // 
            this.objectsInterruptionAtLabel.AutoSize = true;
            this.objectsInterruptionAtLabel.Location = new System.Drawing.Point(4, 17);
            this.objectsInterruptionAtLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.objectsInterruptionAtLabel.Name = "objectsInterruptionAtLabel";
            this.objectsInterruptionAtLabel.Size = new System.Drawing.Size(17, 13);
            this.objectsInterruptionAtLabel.TabIndex = 2;
            this.objectsInterruptionAtLabel.Text = "At";
            // 
            // objectsInterruptionForNumericUpDown
            // 
            this.objectsInterruptionForNumericUpDown.Location = new System.Drawing.Point(64, 15);
            this.objectsInterruptionForNumericUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsInterruptionForNumericUpDown.Maximum = new decimal(new int[] {
            96,
            0,
            0,
            0});
            this.objectsInterruptionForNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.objectsInterruptionForNumericUpDown.Name = "objectsInterruptionForNumericUpDown";
            this.objectsInterruptionForNumericUpDown.Size = new System.Drawing.Size(27, 20);
            this.objectsInterruptionForNumericUpDown.TabIndex = 1;
            this.objectsInterruptionForNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.objectsInterruptionForNumericUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // objectsInterruptionAtNumericUpDown
            // 
            this.objectsInterruptionAtNumericUpDown.Location = new System.Drawing.Point(16, 15);
            this.objectsInterruptionAtNumericUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsInterruptionAtNumericUpDown.Maximum = new decimal(new int[] {
            96,
            0,
            0,
            0});
            this.objectsInterruptionAtNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.objectsInterruptionAtNumericUpDown.Name = "objectsInterruptionAtNumericUpDown";
            this.objectsInterruptionAtNumericUpDown.Size = new System.Drawing.Size(27, 20);
            this.objectsInterruptionAtNumericUpDown.TabIndex = 1;
            this.objectsInterruptionAtNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.objectsInterruptionAtNumericUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // objectsInterruptionsClear
            // 
            this.objectsInterruptionsClear.Location = new System.Drawing.Point(126, 13);
            this.objectsInterruptionsClear.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsInterruptionsClear.Name = "objectsInterruptionsClear";
            this.objectsInterruptionsClear.Size = new System.Drawing.Size(32, 19);
            this.objectsInterruptionsClear.TabIndex = 0;
            this.objectsInterruptionsClear.Text = "Clear";
            this.objectsInterruptionsClear.UseVisualStyleBackColor = true;
            this.objectsInterruptionsClear.Click += new System.EventHandler(this.objectsInterruptionsClear_Click);
            // 
            // objectsInterruptionAddButton
            // 
            this.objectsInterruptionAddButton.Location = new System.Drawing.Point(94, 13);
            this.objectsInterruptionAddButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsInterruptionAddButton.Name = "objectsInterruptionAddButton";
            this.objectsInterruptionAddButton.Size = new System.Drawing.Size(28, 19);
            this.objectsInterruptionAddButton.TabIndex = 0;
            this.objectsInterruptionAddButton.Text = "Set";
            this.objectsInterruptionAddButton.UseVisualStyleBackColor = true;
            this.objectsInterruptionAddButton.Click += new System.EventHandler(this.objectsInterruptionAddButton_Click);
            // 
            // objectsSetColorLabel
            // 
            this.objectsSetColorLabel.AutoSize = true;
            this.objectsSetColorLabel.Enabled = false;
            this.objectsSetColorLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsSetColorLabel.Location = new System.Drawing.Point(4, 65);
            this.objectsSetColorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.objectsSetColorLabel.Name = "objectsSetColorLabel";
            this.objectsSetColorLabel.Size = new System.Drawing.Size(53, 13);
            this.objectsSetColorLabel.TabIndex = 1;
            this.objectsSetColorLabel.Text = "Set Color:";
            // 
            // objectsSetIconLabel
            // 
            this.objectsSetIconLabel.AutoSize = true;
            this.objectsSetIconLabel.Enabled = false;
            this.objectsSetIconLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsSetIconLabel.Location = new System.Drawing.Point(88, 20);
            this.objectsSetIconLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.objectsSetIconLabel.Name = "objectsSetIconLabel";
            this.objectsSetIconLabel.Size = new System.Drawing.Size(50, 13);
            this.objectsSetIconLabel.TabIndex = 1;
            this.objectsSetIconLabel.Text = "Set Icon:";
            // 
            // objectsSetColorComboBox
            // 
            this.objectsSetColorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.objectsSetColorComboBox.Enabled = false;
            this.objectsSetColorComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsSetColorComboBox.FormattingEnabled = true;
            this.objectsSetColorComboBox.Items.AddRange(new object[] {
            "Blue (Gradient)",
            "Blue (Standard)",
            "Green (Standard)",
            "Blue (Diagonal)",
            "Green (Gradient)"});
            this.objectsSetColorComboBox.Location = new System.Drawing.Point(49, 63);
            this.objectsSetColorComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsSetColorComboBox.Name = "objectsSetColorComboBox";
            this.objectsSetColorComboBox.Size = new System.Drawing.Size(120, 21);
            this.objectsSetColorComboBox.TabIndex = 2;
            this.objectsSetColorComboBox.SelectedIndexChanged += new System.EventHandler(this.objectsSetColorComboBox_SelectedIndexChanged);
            // 
            // objectsSetIconComboBox
            // 
            this.objectsSetIconComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.objectsSetIconComboBox.Enabled = false;
            this.objectsSetIconComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsSetIconComboBox.FormattingEnabled = true;
            this.objectsSetIconComboBox.Items.AddRange(new object[] {
            "Task",
            "Folder",
            "Note"});
            this.objectsSetIconComboBox.Location = new System.Drawing.Point(126, 15);
            this.objectsSetIconComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsSetIconComboBox.Name = "objectsSetIconComboBox";
            this.objectsSetIconComboBox.Size = new System.Drawing.Size(43, 21);
            this.objectsSetIconComboBox.TabIndex = 2;
            // 
            // objectsIncreaseIndentButton
            // 
            this.objectsIncreaseIndentButton.Enabled = false;
            this.objectsIncreaseIndentButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsIncreaseIndentButton.Location = new System.Drawing.Point(88, 39);
            this.objectsIncreaseIndentButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsIncreaseIndentButton.Name = "objectsIncreaseIndentButton";
            this.objectsIncreaseIndentButton.Size = new System.Drawing.Size(80, 19);
            this.objectsIncreaseIndentButton.TabIndex = 4;
            this.objectsIncreaseIndentButton.Text = "Increase Indent >";
            this.objectsIncreaseIndentButton.UseVisualStyleBackColor = true;
            // 
            // objectsDecreaseIndentButton
            // 
            this.objectsDecreaseIndentButton.Enabled = false;
            this.objectsDecreaseIndentButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsDecreaseIndentButton.Location = new System.Drawing.Point(4, 39);
            this.objectsDecreaseIndentButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsDecreaseIndentButton.Name = "objectsDecreaseIndentButton";
            this.objectsDecreaseIndentButton.Size = new System.Drawing.Size(80, 19);
            this.objectsDecreaseIndentButton.TabIndex = 3;
            this.objectsDecreaseIndentButton.Text = "< Decrease Indent";
            this.objectsDecreaseIndentButton.UseVisualStyleBackColor = true;
            // 
            // objectsExpandButton
            // 
            this.objectsExpandButton.Enabled = false;
            this.objectsExpandButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsExpandButton.Location = new System.Drawing.Point(4, 15);
            this.objectsExpandButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsExpandButton.Name = "objectsExpandButton";
            this.objectsExpandButton.Size = new System.Drawing.Size(80, 19);
            this.objectsExpandButton.TabIndex = 0;
            this.objectsExpandButton.Text = "Expand/Close";
            this.objectsExpandButton.UseVisualStyleBackColor = true;
            // 
            // objectsMainGroupBox
            // 
            this.objectsMainGroupBox.Controls.Add(this.objectsPrintPreviewButton);
            this.objectsMainGroupBox.Controls.Add(this.objectsPageSetupButton);
            this.objectsMainGroupBox.Controls.Add(this.objectsGanttChartGroupBox);
            this.objectsMainGroupBox.Controls.Add(this.objectsTasksTreeGridGroupBox);
            this.objectsMainGroupBox.Controls.Add(this.objectsReinitializeButton);
            this.objectsMainGroupBox.Controls.Add(this.objectsClearButton);
            this.objectsMainGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsMainGroupBox.Location = new System.Drawing.Point(4, 5);
            this.objectsMainGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsMainGroupBox.Name = "objectsMainGroupBox";
            this.objectsMainGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsMainGroupBox.Size = new System.Drawing.Size(172, 262);
            this.objectsMainGroupBox.TabIndex = 0;
            this.objectsMainGroupBox.TabStop = false;
            this.objectsMainGroupBox.Text = "Data";
            // 
            // objectsPrintPreviewButton
            // 
            this.objectsPrintPreviewButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsPrintPreviewButton.Location = new System.Drawing.Point(88, 236);
            this.objectsPrintPreviewButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsPrintPreviewButton.Name = "objectsPrintPreviewButton";
            this.objectsPrintPreviewButton.Size = new System.Drawing.Size(80, 19);
            this.objectsPrintPreviewButton.TabIndex = 7;
            this.objectsPrintPreviewButton.Text = "Print Preview";
            this.objectsPrintPreviewButton.UseVisualStyleBackColor = true;
            this.objectsPrintPreviewButton.Click += new System.EventHandler(this.objectsPrintPreviewButton_Click);
            // 
            // objectsPageSetupButton
            // 
            this.objectsPageSetupButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsPageSetupButton.Location = new System.Drawing.Point(4, 236);
            this.objectsPageSetupButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsPageSetupButton.Name = "objectsPageSetupButton";
            this.objectsPageSetupButton.Size = new System.Drawing.Size(80, 19);
            this.objectsPageSetupButton.TabIndex = 6;
            this.objectsPageSetupButton.Text = "Page Setup";
            this.objectsPageSetupButton.UseVisualStyleBackColor = true;
            // 
            // objectsGanttChartGroupBox
            // 
            this.objectsGanttChartGroupBox.Controls.Add(this.objectsScaleComboBox);
            this.objectsGanttChartGroupBox.Controls.Add(this.objectsGanttChartShowGroupBox);
            this.objectsGanttChartGroupBox.Controls.Add(this.objectsColorComboBox);
            this.objectsGanttChartGroupBox.Controls.Add(this.objectsCalendarComboBox);
            this.objectsGanttChartGroupBox.Controls.Add(this.objectsUpdateScaleComboBox);
            this.objectsGanttChartGroupBox.Enabled = false;
            this.objectsGanttChartGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsGanttChartGroupBox.Location = new System.Drawing.Point(4, 78);
            this.objectsGanttChartGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsGanttChartGroupBox.Name = "objectsGanttChartGroupBox";
            this.objectsGanttChartGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsGanttChartGroupBox.Size = new System.Drawing.Size(164, 153);
            this.objectsGanttChartGroupBox.TabIndex = 5;
            this.objectsGanttChartGroupBox.TabStop = false;
            this.objectsGanttChartGroupBox.Text = "Gantt Chart";
            // 
            // objectsScaleComboBox
            // 
            this.objectsScaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.objectsScaleComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsScaleComboBox.FormattingEnabled = true;
            this.objectsScaleComboBox.Items.AddRange(new object[] {
            "Days/Hours",
            "Weeks/Days",
            "Months/Weeks",
            "Quarters/Months",
            "Years/Quarters"});
            this.objectsScaleComboBox.Location = new System.Drawing.Point(7, 15);
            this.objectsScaleComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsScaleComboBox.Name = "objectsScaleComboBox";
            this.objectsScaleComboBox.Size = new System.Drawing.Size(72, 21);
            this.objectsScaleComboBox.TabIndex = 3;
            this.objectsScaleComboBox.SelectedIndexChanged += new System.EventHandler(this.objectsScaleComboBox_SelectedIndexChanged);
            // 
            // objectsGanttChartShowGroupBox
            // 
            this.objectsGanttChartShowGroupBox.Controls.Add(this.objectsShowBaselineCheckBox);
            this.objectsGanttChartShowGroupBox.Controls.Add(this.objectsHighlightCriticalTasksCheckBox);
            this.objectsGanttChartShowGroupBox.Controls.Add(this.objectsShowExtraDaytimeCheckBox);
            this.objectsGanttChartShowGroupBox.Controls.Add(this.objectsShowExtraDaysCheckBox);
            this.objectsGanttChartShowGroupBox.Controls.Add(this.objectsShowToolTipsCheckBox);
            this.objectsGanttChartShowGroupBox.Controls.Add(this.objectsShowDependenciesCheckBox);
            this.objectsGanttChartShowGroupBox.Controls.Add(this.objectsShowResourcesCheckBox);
            this.objectsGanttChartShowGroupBox.Controls.Add(this.objectsShowMarkersCheckBox);
            this.objectsGanttChartShowGroupBox.Controls.Add(this.objectsShowIconsCheckBox);
            this.objectsGanttChartShowGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsGanttChartShowGroupBox.Location = new System.Drawing.Point(7, 59);
            this.objectsGanttChartShowGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsGanttChartShowGroupBox.Name = "objectsGanttChartShowGroupBox";
            this.objectsGanttChartShowGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsGanttChartShowGroupBox.Size = new System.Drawing.Size(152, 89);
            this.objectsGanttChartShowGroupBox.TabIndex = 5;
            this.objectsGanttChartShowGroupBox.TabStop = false;
            this.objectsGanttChartShowGroupBox.Text = "Show";
            // 
            // objectsShowBaselineCheckBox
            // 
            this.objectsShowBaselineCheckBox.AutoSize = true;
            this.objectsShowBaselineCheckBox.Location = new System.Drawing.Point(76, 72);
            this.objectsShowBaselineCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsShowBaselineCheckBox.Name = "objectsShowBaselineCheckBox";
            this.objectsShowBaselineCheckBox.Size = new System.Drawing.Size(66, 17);
            this.objectsShowBaselineCheckBox.TabIndex = 8;
            this.objectsShowBaselineCheckBox.Text = "Baseline";
            this.objectsShowBaselineCheckBox.UseVisualStyleBackColor = true;
            // 
            // objectsHighlightCriticalTasksCheckBox
            // 
            this.objectsHighlightCriticalTasksCheckBox.AutoSize = true;
            this.objectsHighlightCriticalTasksCheckBox.Location = new System.Drawing.Point(4, 72);
            this.objectsHighlightCriticalTasksCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsHighlightCriticalTasksCheckBox.Name = "objectsHighlightCriticalTasksCheckBox";
            this.objectsHighlightCriticalTasksCheckBox.Size = new System.Drawing.Size(89, 17);
            this.objectsHighlightCriticalTasksCheckBox.TabIndex = 7;
            this.objectsHighlightCriticalTasksCheckBox.Text = "Critical Tasks";
            this.objectsHighlightCriticalTasksCheckBox.UseVisualStyleBackColor = true;
            this.objectsHighlightCriticalTasksCheckBox.CheckedChanged += new System.EventHandler(this.objectsHighlightCriticalTasksCheckBox_CheckedChanged);
            // 
            // objectsShowExtraDaytimeCheckBox
            // 
            this.objectsShowExtraDaytimeCheckBox.AutoSize = true;
            this.objectsShowExtraDaytimeCheckBox.Location = new System.Drawing.Point(76, 53);
            this.objectsShowExtraDaytimeCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsShowExtraDaytimeCheckBox.Name = "objectsShowExtraDaytimeCheckBox";
            this.objectsShowExtraDaytimeCheckBox.Size = new System.Drawing.Size(91, 17);
            this.objectsShowExtraDaytimeCheckBox.TabIndex = 6;
            this.objectsShowExtraDaytimeCheckBox.Text = "Extra Daytime";
            this.objectsShowExtraDaytimeCheckBox.UseVisualStyleBackColor = true;
            // 
            // objectsShowExtraDaysCheckBox
            // 
            this.objectsShowExtraDaysCheckBox.AutoSize = true;
            this.objectsShowExtraDaysCheckBox.Location = new System.Drawing.Point(4, 53);
            this.objectsShowExtraDaysCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsShowExtraDaysCheckBox.Name = "objectsShowExtraDaysCheckBox";
            this.objectsShowExtraDaysCheckBox.Size = new System.Drawing.Size(77, 17);
            this.objectsShowExtraDaysCheckBox.TabIndex = 5;
            this.objectsShowExtraDaysCheckBox.Text = "Extra Days";
            this.objectsShowExtraDaysCheckBox.UseVisualStyleBackColor = true;
            // 
            // objectsShowToolTipsCheckBox
            // 
            this.objectsShowToolTipsCheckBox.AutoSize = true;
            this.objectsShowToolTipsCheckBox.Location = new System.Drawing.Point(100, 15);
            this.objectsShowToolTipsCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsShowToolTipsCheckBox.Name = "objectsShowToolTipsCheckBox";
            this.objectsShowToolTipsCheckBox.Size = new System.Drawing.Size(63, 17);
            this.objectsShowToolTipsCheckBox.TabIndex = 4;
            this.objectsShowToolTipsCheckBox.Text = "Tooltips";
            this.objectsShowToolTipsCheckBox.UseVisualStyleBackColor = true;
            this.objectsShowToolTipsCheckBox.CheckedChanged += new System.EventHandler(this.objectsShowToolTipsCheckBox_CheckedChanged);
            // 
            // objectsShowDependenciesCheckBox
            // 
            this.objectsShowDependenciesCheckBox.AutoSize = true;
            this.objectsShowDependenciesCheckBox.Location = new System.Drawing.Point(76, 34);
            this.objectsShowDependenciesCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsShowDependenciesCheckBox.Name = "objectsShowDependenciesCheckBox";
            this.objectsShowDependenciesCheckBox.Size = new System.Drawing.Size(95, 17);
            this.objectsShowDependenciesCheckBox.TabIndex = 4;
            this.objectsShowDependenciesCheckBox.Text = "Dependencies";
            this.objectsShowDependenciesCheckBox.UseVisualStyleBackColor = true;
            // 
            // objectsShowResourcesCheckBox
            // 
            this.objectsShowResourcesCheckBox.AutoSize = true;
            this.objectsShowResourcesCheckBox.Location = new System.Drawing.Point(4, 34);
            this.objectsShowResourcesCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsShowResourcesCheckBox.Name = "objectsShowResourcesCheckBox";
            this.objectsShowResourcesCheckBox.Size = new System.Drawing.Size(77, 17);
            this.objectsShowResourcesCheckBox.TabIndex = 4;
            this.objectsShowResourcesCheckBox.Text = "Resources";
            this.objectsShowResourcesCheckBox.UseVisualStyleBackColor = true;
            this.objectsShowResourcesCheckBox.CheckedChanged += new System.EventHandler(this.objectsShowResourcesCheckBox_CheckedChanged);
            // 
            // objectsShowMarkersCheckBox
            // 
            this.objectsShowMarkersCheckBox.AutoSize = true;
            this.objectsShowMarkersCheckBox.Location = new System.Drawing.Point(48, 15);
            this.objectsShowMarkersCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsShowMarkersCheckBox.Name = "objectsShowMarkersCheckBox";
            this.objectsShowMarkersCheckBox.Size = new System.Drawing.Size(64, 17);
            this.objectsShowMarkersCheckBox.TabIndex = 4;
            this.objectsShowMarkersCheckBox.Text = "Markers";
            this.objectsShowMarkersCheckBox.UseVisualStyleBackColor = true;
            // 
            // objectsShowIconsCheckBox
            // 
            this.objectsShowIconsCheckBox.AutoSize = true;
            this.objectsShowIconsCheckBox.Location = new System.Drawing.Point(4, 15);
            this.objectsShowIconsCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsShowIconsCheckBox.Name = "objectsShowIconsCheckBox";
            this.objectsShowIconsCheckBox.Size = new System.Drawing.Size(52, 17);
            this.objectsShowIconsCheckBox.TabIndex = 4;
            this.objectsShowIconsCheckBox.Text = "Icons";
            this.objectsShowIconsCheckBox.UseVisualStyleBackColor = true;
            this.objectsShowIconsCheckBox.CheckedChanged += new System.EventHandler(this.objectsShowIconsCheckBox_CheckedChanged);
            // 
            // objectsColorComboBox
            // 
            this.objectsColorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.objectsColorComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsColorComboBox.FormattingEnabled = true;
            this.objectsColorComboBox.Items.AddRange(new object[] {
            "Blue (Gradient)",
            "Blue (Standard)",
            "Green (Standard)",
            "Blue (Diagonal)",
            "Green (Gradient)"});
            this.objectsColorComboBox.Location = new System.Drawing.Point(82, 37);
            this.objectsColorComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsColorComboBox.Name = "objectsColorComboBox";
            this.objectsColorComboBox.Size = new System.Drawing.Size(72, 21);
            this.objectsColorComboBox.TabIndex = 3;
            // 
            // objectsCalendarComboBox
            // 
            this.objectsCalendarComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.objectsCalendarComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsCalendarComboBox.FormattingEnabled = true;
            this.objectsCalendarComboBox.Items.AddRange(new object[] {
            "Mon-Fri 8h",
            "Mon-Fri 6h, Sat 3h"});
            this.objectsCalendarComboBox.Location = new System.Drawing.Point(82, 15);
            this.objectsCalendarComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsCalendarComboBox.Name = "objectsCalendarComboBox";
            this.objectsCalendarComboBox.Size = new System.Drawing.Size(72, 21);
            this.objectsCalendarComboBox.TabIndex = 3;
            // 
            // objectsUpdateScaleComboBox
            // 
            this.objectsUpdateScaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.objectsUpdateScaleComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsUpdateScaleComboBox.FormattingEnabled = true;
            this.objectsUpdateScaleComboBox.Items.AddRange(new object[] {
            "Hours Update",
            "Days Update",
            "Scaled Update",
            "Free Update"});
            this.objectsUpdateScaleComboBox.Location = new System.Drawing.Point(7, 37);
            this.objectsUpdateScaleComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsUpdateScaleComboBox.Name = "objectsUpdateScaleComboBox";
            this.objectsUpdateScaleComboBox.Size = new System.Drawing.Size(72, 21);
            this.objectsUpdateScaleComboBox.TabIndex = 3;
            this.objectsUpdateScaleComboBox.SelectedIndexChanged += new System.EventHandler(this.objectsUpdateScaleComboBox_SelectedIndexChanged);
            // 
            // objectsTasksTreeGridGroupBox
            // 
            this.objectsTasksTreeGridGroupBox.Controls.Add(this.objectsFixedGridCheckBox);
            this.objectsTasksTreeGridGroupBox.Controls.Add(this.objectsShowGridCheckBox);
            this.objectsTasksTreeGridGroupBox.Enabled = false;
            this.objectsTasksTreeGridGroupBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsTasksTreeGridGroupBox.Location = new System.Drawing.Point(4, 39);
            this.objectsTasksTreeGridGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsTasksTreeGridGroupBox.Name = "objectsTasksTreeGridGroupBox";
            this.objectsTasksTreeGridGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsTasksTreeGridGroupBox.Size = new System.Drawing.Size(164, 34);
            this.objectsTasksTreeGridGroupBox.TabIndex = 5;
            this.objectsTasksTreeGridGroupBox.TabStop = false;
            this.objectsTasksTreeGridGroupBox.Text = "Tasks Tree-Grid";
            // 
            // objectsFixedGridCheckBox
            // 
            this.objectsFixedGridCheckBox.AutoSize = true;
            this.objectsFixedGridCheckBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsFixedGridCheckBox.Location = new System.Drawing.Point(67, 15);
            this.objectsFixedGridCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsFixedGridCheckBox.Name = "objectsFixedGridCheckBox";
            this.objectsFixedGridCheckBox.Size = new System.Drawing.Size(129, 17);
            this.objectsFixedGridCheckBox.TabIndex = 4;
            this.objectsFixedGridCheckBox.Text = "Fixed Grid (on Resize)";
            this.objectsFixedGridCheckBox.UseVisualStyleBackColor = true;
            // 
            // objectsShowGridCheckBox
            // 
            this.objectsShowGridCheckBox.AutoSize = true;
            this.objectsShowGridCheckBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsShowGridCheckBox.Location = new System.Drawing.Point(6, 15);
            this.objectsShowGridCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsShowGridCheckBox.Name = "objectsShowGridCheckBox";
            this.objectsShowGridCheckBox.Size = new System.Drawing.Size(75, 17);
            this.objectsShowGridCheckBox.TabIndex = 4;
            this.objectsShowGridCheckBox.Text = "Show Grid";
            this.objectsShowGridCheckBox.UseVisualStyleBackColor = true;
            // 
            // objectsReinitializeButton
            // 
            this.objectsReinitializeButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsReinitializeButton.Location = new System.Drawing.Point(4, 15);
            this.objectsReinitializeButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsReinitializeButton.Name = "objectsReinitializeButton";
            this.objectsReinitializeButton.Size = new System.Drawing.Size(80, 19);
            this.objectsReinitializeButton.TabIndex = 0;
            this.objectsReinitializeButton.Text = "Initialize";
            this.objectsReinitializeButton.UseVisualStyleBackColor = true;
            this.objectsReinitializeButton.Click += new System.EventHandler(this.objectsReinitializeButton_Click);
            // 
            // objectsClearButton
            // 
            this.objectsClearButton.Enabled = false;
            this.objectsClearButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsClearButton.Location = new System.Drawing.Point(88, 15);
            this.objectsClearButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.objectsClearButton.Name = "objectsClearButton";
            this.objectsClearButton.Size = new System.Drawing.Size(80, 19);
            this.objectsClearButton.TabIndex = 1;
            this.objectsClearButton.Text = "Clear";
            this.objectsClearButton.UseVisualStyleBackColor = true;
            // 
            // propertiesButton
            // 
            this.propertiesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.propertiesButton.Location = new System.Drawing.Point(578, 548);
            this.propertiesButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.propertiesButton.Name = "propertiesButton";
            this.propertiesButton.Size = new System.Drawing.Size(104, 19);
            this.propertiesButton.TabIndex = 11;
            this.propertiesButton.Text = "&Properties";
            this.propertiesButton.UseVisualStyleBackColor = true;
            this.propertiesButton.Click += new System.EventHandler(this.propertiesButton_Click);
            // 
            // frmProjectTimeLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1380, 674);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "frmProjectTimeLine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Project Time Line";
            this.Load += new System.EventHandler(this.frmProjectTimeLine_Load);
            this.panel1.ResumeLayout(false);
            this.resourceLoadChartView.ResumeLayout(false);
            this.ganttChartView.ResumeLayout(false);
            this.ganttChartViewSplitContainer.Panel1.ResumeLayout(false);
            this.ganttChartViewSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ganttChartViewSplitContainer)).EndInit();
            this.ganttChartViewSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ganttChartViewTasksTreeGrid)).EndInit();
            this.ganttChartViewGanttChartPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControlModes.ResumeLayout(false);
            this.standardTabPage.ResumeLayout(false);
            this.standardRowGroupBox.ResumeLayout(false);
            this.standardRowGroupBox.PerformLayout();
            this.standardMainGroupBox.ResumeLayout(false);
            this.standardGanttChartGroupBox.ResumeLayout(false);
            this.standardGanttChartShowGroupBox.ResumeLayout(false);
            this.standardGanttChartShowGroupBox.PerformLayout();
            this.standardTasksTreeGridGroupBox.ResumeLayout(false);
            this.standardTasksTreeGridGroupBox.PerformLayout();
            this.dataTableTabPage.ResumeLayout(false);
            this.dataTableRowGroupBox.ResumeLayout(false);
            this.dataTableRowGroupBox.PerformLayout();
            this.dataTableMarkersGroupBox.ResumeLayout(false);
            this.dataTableMarkersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableMarkerAtNumericUpDown)).EndInit();
            this.dataTableInterruptionsGroupBox.ResumeLayout(false);
            this.dataTableInterruptionsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableInterruptionForNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableInterruptionAtNumericUpDown)).EndInit();
            this.dataTableMainGroupBox.ResumeLayout(false);
            this.dataTableGanttChartGroupBox.ResumeLayout(false);
            this.dataTableGanttChartShowGroupBox.ResumeLayout(false);
            this.dataTableGanttChartShowGroupBox.PerformLayout();
            this.dataTableTasksTreeGridGroupBox.ResumeLayout(false);
            this.dataTableTasksTreeGridGroupBox.PerformLayout();
            this.objectsTabPage.ResumeLayout(false);
            this.objectsRowGroupBox.ResumeLayout(false);
            this.objectsRowGroupBox.PerformLayout();
            this.objectsMarkersGroupBox.ResumeLayout(false);
            this.objectsMarkersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectsMarkerAtNumericUpDown)).EndInit();
            this.objectsInterruptionsGroupBox.ResumeLayout(false);
            this.objectsInterruptionsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectsInterruptionForNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectsInterruptionAtNumericUpDown)).EndInit();
            this.objectsMainGroupBox.ResumeLayout(false);
            this.objectsGanttChartGroupBox.ResumeLayout(false);
            this.objectsGanttChartShowGroupBox.ResumeLayout(false);
            this.objectsGanttChartShowGroupBox.PerformLayout();
            this.objectsTasksTreeGridGroupBox.ResumeLayout(false);
            this.objectsTasksTreeGridGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList iconsList;
        private System.Windows.Forms.ImageList markersList;
        private System.Windows.Forms.Timer timerCurrentCellChanged;
        private System.Windows.Forms.Panel panel1;
        private DlhSoft.ProjectDataControlLibrary.ResourceLoad.ResourceLoadChartView resourceLoadChartView;
        private DlhSoft.ProjectDataControlLibrary.ResourceLoad.ResourceLoadScaleHeader resourceLoadChartViewResourceLoadScaleHeader;
        private DlhSoft.ProjectDataControlLibrary.ResourceLoad.ResourceLoadScaleArea resourceLoadChartViewResourceLoadScaleArea;
        private DlhSoft.ProjectDataControlLibrary.ResourceLoad.ResourceLoadChartHeader resourceLoadChartViewResourceLoadChartHeader;
        private DlhSoft.ProjectDataControlLibrary.ResourceLoad.ResourceLoadChartArea resourceLoadChartViewResourceLoadChartArea;
        private DlhSoft.ProjectDataControlLibrary.ResourceLoad.ResourceLoadChartCurrentDateScrollBar resourceLoadChartViewResourceLoadChartCurrentDateScrollBar;
        private DlhSoft.ProjectDataControlLibrary.GanttChartView ganttChartView;
        private System.Windows.Forms.SplitContainer ganttChartViewSplitContainer;
        private DlhSoft.ProjectDataControlLibrary.TasksTreeGrid ganttChartViewTasksTreeGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private DataGridViewDateTimePickerColumn startDataGridViewDateTimePickerColumn;
        private DataGridViewDateTimePickerColumn finishDataGridViewDateTimePickerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn workDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn completedWorkDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn percentCompletedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isMilestoneDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn predecessorsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resourcesDataGridViewTextBoxColumn;
        private DataGridViewDateTimePickerColumn plannedStartDataGridViewDateTimePickerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn plannedWorkDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn plannedCompletedWorkDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn indentLevelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn iconIndexDataGridViewTextBoxColumn;
        private DlhSoft.ProjectDataControlLibrary.GanttChartPanel ganttChartViewGanttChartPanel;
        private DlhSoft.ProjectDataControlLibrary.GanttChartHeader ganttChartViewGanttChartHeader;
        private DlhSoft.ProjectDataControlLibrary.GanttChartArea ganttChartViewGanttChartArea;
        private DlhSoft.ProjectDataControlLibrary.GanttChartVerticalScrollBar ganttChartViewGanttChartVerticalScrollBar;
        private DlhSoft.ProjectDataControlLibrary.GanttChartCurrentDateScrollBar ganttChartViewGanttChartCurrentDateScrollBar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.TabControl tabControlModes;
        private System.Windows.Forms.TabPage standardTabPage;
        private System.Windows.Forms.Label standardDisabledLabel;
        private System.Windows.Forms.GroupBox standardRowGroupBox;
        private System.Windows.Forms.Button standardExpandButton;
        private System.Windows.Forms.Button standardIncreaseIndentButton;
        private System.Windows.Forms.Label standardSetIconLabel;
        private System.Windows.Forms.Button standardDecreaseIndentButton;
        private System.Windows.Forms.ComboBox standardSetIconComboBox;
        private System.Windows.Forms.GroupBox standardMainGroupBox;
        private System.Windows.Forms.Button standardPrintPreviewButton;
        private System.Windows.Forms.Button standardPageSetupButton;
        private System.Windows.Forms.GroupBox standardGanttChartGroupBox;
        private System.Windows.Forms.ComboBox standardScaleComboBox;
        private System.Windows.Forms.GroupBox standardGanttChartShowGroupBox;
        private System.Windows.Forms.CheckBox standardShowBaselineCheckBox;
        private System.Windows.Forms.CheckBox standardHighlightCriticalTasksCheckBox;
        private System.Windows.Forms.CheckBox standardShowDependenciesCheckBox;
        private System.Windows.Forms.CheckBox standardShowExtraDaytimeCheckBox;
        private System.Windows.Forms.CheckBox standardShowExtraDaysCheckBox;
        private System.Windows.Forms.CheckBox standardShowResourcesCheckBox;
        private System.Windows.Forms.ComboBox standardColorComboBox;
        private System.Windows.Forms.ComboBox standardCalendarComboBox;
        private System.Windows.Forms.ComboBox standardUpdateScaleComboBox;
        private System.Windows.Forms.GroupBox standardTasksTreeGridGroupBox;
        private System.Windows.Forms.CheckBox standardFixedGridCheckBox;
        private System.Windows.Forms.CheckBox standardShowGridCheckBox;
        private System.Windows.Forms.Button standardReinitializeButton;
        private System.Windows.Forms.Button standardClearButton;
        private System.Windows.Forms.TabPage dataTableTabPage;
        private System.Windows.Forms.Label dataTableDisabledLabel;
        private System.Windows.Forms.GroupBox dataTableRowGroupBox;
        private System.Windows.Forms.GroupBox dataTableMarkersGroupBox;
        private System.Windows.Forms.Label dataTableMarkerAtLabel;
        private System.Windows.Forms.NumericUpDown dataTableMarkerAtNumericUpDown;
        private System.Windows.Forms.Button dataTableMarkersClearButton;
        private System.Windows.Forms.Button dataTableMarkerAddButton;
        private System.Windows.Forms.ComboBox dataTableMarkerTypeComboBox;
        private System.Windows.Forms.GroupBox dataTableInterruptionsGroupBox;
        private System.Windows.Forms.Label dataTableInterruptionForLabel;
        private System.Windows.Forms.Label dataTableInterruptionAtLabel;
        private System.Windows.Forms.NumericUpDown dataTableInterruptionForNumericUpDown;
        private System.Windows.Forms.NumericUpDown dataTableInterruptionAtNumericUpDown;
        private System.Windows.Forms.Button dataTableInterruptionsClearButton;
        private System.Windows.Forms.Button dataTableInterruptionAddButton;
        private System.Windows.Forms.Label dataTableSetColorLabel;
        private System.Windows.Forms.ComboBox dataTableSetColorComboBox;
        private System.Windows.Forms.Label dataTableSetIconLabel;
        private System.Windows.Forms.ComboBox dataTableSetIconComboBox;
        private System.Windows.Forms.Button dataTableIncreaseIndentButton;
        private System.Windows.Forms.Button dataTableDecreaseIndentButton;
        private System.Windows.Forms.Button dataTableExpandButton;
        private System.Windows.Forms.GroupBox dataTableMainGroupBox;
        private System.Windows.Forms.Button dataTablePrintPreviewButton;
        private System.Windows.Forms.Button dataTablePageSetupButton;
        private System.Windows.Forms.GroupBox dataTableGanttChartGroupBox;
        private System.Windows.Forms.ComboBox dataTableScaleComboBox;
        private System.Windows.Forms.GroupBox dataTableGanttChartShowGroupBox;
        private System.Windows.Forms.CheckBox dataTableShowBaselineCheckBox;
        private System.Windows.Forms.CheckBox dataTableHighlightCriticalTasksCheckBox;
        private System.Windows.Forms.CheckBox dataTableShowExtraDaytimeCheckBox;
        private System.Windows.Forms.CheckBox dataTableShowExtraDaysCheckBox;
        private System.Windows.Forms.CheckBox dataTableShowToolTipsCheckBox;
        private System.Windows.Forms.CheckBox dataTableShowDependenciesCheckBox;
        private System.Windows.Forms.CheckBox dataTableShowResourcesCheckBox;
        private System.Windows.Forms.CheckBox dataTableShowMarkersCheckBox;
        private System.Windows.Forms.CheckBox dataTableShowIconsCheckBox;
        private System.Windows.Forms.ComboBox dataTableColorComboBox;
        private System.Windows.Forms.ComboBox dataTableCalendarComboBox;
        private System.Windows.Forms.ComboBox dataTableUpdateScaleComboBox;
        private System.Windows.Forms.GroupBox dataTableTasksTreeGridGroupBox;
        private System.Windows.Forms.CheckBox dataTableFixedGridCheckBox;
        private System.Windows.Forms.CheckBox dataTableShowGridCheckBox;
        private System.Windows.Forms.Button dataTableReinitializeButton;
        private System.Windows.Forms.Button dataTableClearButton;
        private System.Windows.Forms.TabPage objectsTabPage;
        private System.Windows.Forms.Label objectsDisabledLabel;
        private System.Windows.Forms.GroupBox objectsRowGroupBox;
        private System.Windows.Forms.GroupBox objectsMarkersGroupBox;
        private System.Windows.Forms.Label objectsMarkerAtLabel;
        private System.Windows.Forms.NumericUpDown objectsMarkerAtNumericUpDown;
        private System.Windows.Forms.Button objectsMarkersClearButton;
        private System.Windows.Forms.Button objectsMarkerAddButton;
        private System.Windows.Forms.ComboBox objectsMarkerTypeComboBox;
        private System.Windows.Forms.GroupBox objectsInterruptionsGroupBox;
        private System.Windows.Forms.Label objectsInterruptionForLabel;
        private System.Windows.Forms.Label objectsInterruptionAtLabel;
        private System.Windows.Forms.NumericUpDown objectsInterruptionForNumericUpDown;
        private System.Windows.Forms.NumericUpDown objectsInterruptionAtNumericUpDown;
        private System.Windows.Forms.Button objectsInterruptionsClear;
        private System.Windows.Forms.Button objectsInterruptionAddButton;
        private System.Windows.Forms.Label objectsSetColorLabel;
        private System.Windows.Forms.Label objectsSetIconLabel;
        private System.Windows.Forms.ComboBox objectsSetColorComboBox;
        private System.Windows.Forms.ComboBox objectsSetIconComboBox;
        private System.Windows.Forms.Button objectsIncreaseIndentButton;
        private System.Windows.Forms.Button objectsDecreaseIndentButton;
        private System.Windows.Forms.Button objectsExpandButton;
        private System.Windows.Forms.GroupBox objectsMainGroupBox;
        private System.Windows.Forms.Button objectsPrintPreviewButton;
        private System.Windows.Forms.Button objectsPageSetupButton;
        private System.Windows.Forms.GroupBox objectsGanttChartGroupBox;
        private System.Windows.Forms.ComboBox objectsScaleComboBox;
        private System.Windows.Forms.GroupBox objectsGanttChartShowGroupBox;
        private System.Windows.Forms.CheckBox objectsShowBaselineCheckBox;
        private System.Windows.Forms.CheckBox objectsHighlightCriticalTasksCheckBox;
        private System.Windows.Forms.CheckBox objectsShowExtraDaytimeCheckBox;
        private System.Windows.Forms.CheckBox objectsShowExtraDaysCheckBox;
        private System.Windows.Forms.CheckBox objectsShowToolTipsCheckBox;
        private System.Windows.Forms.CheckBox objectsShowDependenciesCheckBox;
        private System.Windows.Forms.CheckBox objectsShowResourcesCheckBox;
        private System.Windows.Forms.CheckBox objectsShowMarkersCheckBox;
        private System.Windows.Forms.CheckBox objectsShowIconsCheckBox;
        private System.Windows.Forms.ComboBox objectsColorComboBox;
        private System.Windows.Forms.ComboBox objectsCalendarComboBox;
        private System.Windows.Forms.ComboBox objectsUpdateScaleComboBox;
        private System.Windows.Forms.GroupBox objectsTasksTreeGridGroupBox;
        private System.Windows.Forms.CheckBox objectsFixedGridCheckBox;
        private System.Windows.Forms.CheckBox objectsShowGridCheckBox;
        private System.Windows.Forms.Button objectsReinitializeButton;
        private System.Windows.Forms.Button objectsClearButton;
        private System.Windows.Forms.Button propertiesButton;
    }
}