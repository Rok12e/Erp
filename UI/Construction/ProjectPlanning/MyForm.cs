//  To use Project Data Control Library controls in your projects follow these steps:
//
//  1. Create or open a Windows Forms Application project in Visual Studio 2005/2008.
//
//  2. Add reference to Project Data Control Library assembly. To do this, right 
//     click the Windows Forms Application project in Visual Studio's Solution Explorer, and 
//     select Add Reference from the context menu. Select the Browse tab from the Add 
//     Reference dialog to find and select Project Data Control Library assembly, 
//     usually installed as C:\Program Files\DlhSoft\Gantt Chart Library 2.0.0\
//     DlhSoft.ProjectDataControlLibrary.dll file. The reference is added to your project.
//
//  3. If you want to use the controls at design time, add the Project Data 
//     Control Library control icons to the Visual Studio form designer toolbar. To do 
//     this, open a Windows Form within Visual Studio in design mode, right click a 
//     Toolbox tab and select Choose Items command from the context menu. In the 
//     Choose Toolbox Items dialog use the Browse button to find and select 
//     Project Data Control Library assembly, usually installed as 
//     C:\Program Files\DlhSoft\Project Data Control Library 2.2\
//     DlhSoft.ProjectDataControlLibrary.dll file. Make sure that all the 
//     controls you want are selected, and click OK. Find the control icons within the 
//     toolbox. The initially selected control is GanttChartView.
//
//  4. Use the Project Data Control Library controls according to the technical 
//     documentation available from Start Menu, DlhSoft Project Data Control 
//     Library 2.2, Documentation.

using DlhSoft.ProjectDataControlLibrary;
using DlhSoft.ProjectDataControlLibrary.ResourceLoad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject.UI.Construction.ProjectPlanning
{
    public partial class MyForm : Form
    {
        #region Constructors

        public MyForm()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);

            //By default, open the objects binding mode and initialize data
            tabControlModes.SelectedTab = objectsTabPage;
            objectsReinitializeButton_Click(this, EventArgs.Empty);
        }

        #endregion

        #region Data Fields

        BindingList<MyTask> tasks; //stores the data for object binding mode
        DataTable dataTable; //stores the data for DataTable binding mode

        #endregion

        #region Data Initialization

        private bool duringInitialization;

        //No binding
        private void standardReinitializeButton_Click(object sender, EventArgs e)
        {
            CurrentDataBindingMode = DataBindingMode.Standard;

            duringInitialization = true;

            ganttChartView.TasksTreeGrid.Rows.Clear();

            ganttChartView.TasksTreeGrid.Rows.Add(
                "Parent Task", //name
                DateTime.Today.AddDays(1), //start is not used as this will be a parent task
                DateTime.MinValue, //finish is not used in standard (no binding) mode
                8, //work is not used as this will be a parent task
                0, //completed work is not used as this will be a parent task
                0, //percent completed is not used in standard (no binding) mode
                false, //is milestone is not used in standard (no binding) mode
                string.Empty, //predecessors
                string.Empty, //resources
                DateTime.Today, //baseline (planned) start
                8, //baseline (planned) work
                0 //baseline (planned) completed work
                );

            ganttChartView.TasksTreeGrid.Rows.Add(
                "Task 2", //name
                DateTime.Today, //start
                DateTime.MinValue, //finish is not used in standard (no binding) mode
                10, //work
                0, //completed work
                0, //percent completed is not used in standard (no binding) mode
                false, //is milestone is not used in standard (no binding) mode
                string.Empty, //predecessors
                string.Empty, //resources
                DateTime.Today, //baseline (planned) start
                8, //baseline (planned) work
                0 //baseline (planned) completed work
                );

            ganttChartView.TasksTreeGrid.Rows.Add(
                "Task 3", //name
                DateTime.Today.AddDays(3).AddHours(16), //start: three days later, at 4 PM
                DateTime.MinValue, //finish is not used in standard (no binding) mode
                20, //work
                0, //completed work
                0, //percent completed is not used in standard (no binding) mode
                false, //is milestone is not used in standard (no binding) mode
                "2", //predecessors
                "John", //resources
                DateTime.Today.AddDays(2), //baseline (planned) start
                24, //baseline (planned) work
                2 //baseline (planned) completed work
                );

            ganttChartView.TasksTreeGrid.Rows.Add(
                "Task 4", //name
                DateTime.Today.AddDays(4), //start: four days later
                DateTime.MinValue, //finish is not used in standard (no binding) mode
                24, //work
                0, //completed work
                0, //percent completed is not used in standard (no binding) mode
                false, //is milestone is not used in standard (no binding) mode
                string.Empty, //predecessors
                "Diane", //resources
                DateTime.Today.AddDays(5), //baseline (planned) start
                24, //baseline (planned) work
                0 //baseline (planned) completed work
                );

            for (int i = 5; i <= 20; i++)
            {
                ganttChartView.TasksTreeGrid.Rows.Add(
                    "Task " + i, //name
                    DateTime.Today.AddDays(i), //start
                    DateTime.MinValue, //finish is not used in standard (no binding) mode
                    8 + (i % 20), //work
                    i % 20, //completed work
                    0, //percent completed is not used in standard (no binding) mode
                    false, //is milestone is not used in standard (no binding) mode
                    string.Empty, //predecessors
                    string.Empty, //resources
                    DateTime.Today.AddDays(i), //baseline (planned) start
                    8 + (i % 20), //baseline (planned) work
                    0 //baseline (planned) completed work
                    );
            }

            ganttChartView.TasksTreeGrid.IncreaseIndent(1); //Task 2
            ganttChartView.TasksTreeGrid.IncreaseIndent(2); //Task 3
            ganttChartView.TasksTreeGrid.IncreaseIndent(3); //Task 4

            ganttChartView.TasksTreeGrid.SetImageIndex(0, 1); //Task 1

            duringInitialization = false;

            ensureEnableItemUI();
        }

        //DataTable binding
        private void dataTableReinitializeButton_Click(object sender, EventArgs e)
        {
            CurrentDataBindingMode = DataBindingMode.DataTable;

            duringInitialization = true;

            dataTable = new DataTable("MyDataTable");
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("IndentLevel", typeof(int));
            dataTable.Columns.Add("IconIndex", typeof(int));
            dataTable.Columns.Add("Start", typeof(DateTime));
            dataTable.Columns.Add("Work", typeof(decimal));
            dataTable.Columns.Add("CompletedWork", typeof(decimal));
            dataTable.Columns.Add("Predecessors", typeof(string));
            dataTable.Columns.Add("Resources", typeof(string));
            dataTable.Columns.Add("Interruptions", typeof(Dictionary<double, double>));
            dataTable.Columns.Add("PlannedStart", typeof(DateTime));
            dataTable.Columns.Add("PlannedWork", typeof(decimal));
            dataTable.Columns.Add("PlannedCompletedWork", typeof(decimal));
            dataTable.Columns.Add("Markers", typeof(Dictionary<double, int>));
            dataTable.Columns.Add("Brush", typeof(Brush));

            dataTable.Columns["IndentLevel"].DefaultValue = 0;
            dataTable.Columns["IconIndex"].DefaultValue = 0;
            dataTable.Columns["Start"].DefaultValue = DateTime.Today.AddDays(1);
            dataTable.Columns["Work"].DefaultValue = 8;
            dataTable.Columns["Predecessors"].DefaultValue = string.Empty;
            dataTable.Columns["CompletedWork"].DefaultValue = 0;
            dataTable.Columns["PlannedStart"].DefaultValue = DateTime.Today.AddDays(1);
            dataTable.Columns["PlannedWork"].DefaultValue = 8;
            dataTable.Columns["PlannedCompletedWork"].DefaultValue = 0;

            dataTable.TableNewRow += 
                delegate(object sender0, DataTableNewRowEventArgs e0)
                { 
                    e0.Row["Interruptions"] = new Dictionary<double, double>();
                    e0.Row["Markers"] = new Dictionary<double, int>();
                };

            DataRow t1 = dataTable.NewRow();
            t1["Name"] = "Parent Task";
            t1["Description"] = "This is the first parent task";
            t1["IconIndex"] = 1; //set second icon from iconsList
            dataTable.Rows.Add(t1);

            DataRow t2 = dataTable.NewRow();
            t2["Name"] = "Task 2";
            t2["Description"] = "This is the first sub-task of the parent task";
            t2["Start"] = DateTime.Today;
            t2["Work"] = 10;
            t2["IconIndex"] = 0; //set first icon from iconsList
            Dictionary<double, int> t2Markers = new Dictionary<double, int>();
            t2Markers.Add(6, 0);
            t2["Markers"] = t2Markers; //add a marker on this task after 6h
            t2["IndentLevel"] = 1;
            t2["PlannedStart"] = DateTime.Today;
            t2["PlannedWork"] = 8;
            t2["PlannedCompletedWork"] = 0;
            dataTable.Rows.Add(t2);

            DataRow t3 = dataTable.NewRow();
            t3["Name"] = "Task 3";
            t3["Description"] = "This is the second sub-task of the parent task\nAnother line of text shown in the tool-tip";
            t3["Start"] = DateTime.Today.AddDays(3).AddHours(16); //three days later, at 4 PM
            t3["Work"] = 20;
            t3["IconIndex"] = 0; //set first icon from iconsList
            Dictionary<double, double> t3Interruptions = new Dictionary<double, double>();
            t3Interruptions.Add(10, 4);
            t3["Interruptions"] = t3Interruptions; //add a marker on this task after 6h
            t3["Predecessors"] = "2";
            t3["Resources"] = "John";
            t3["IndentLevel"] = 1;
            t3["PlannedStart"] = DateTime.Today.AddDays(2);
            t3["PlannedWork"] = 24;
            t3["PlannedCompletedWork"] = 2;
            dataTable.Rows.Add(t3);

            DataRow t4 = dataTable.NewRow();
            t4["Name"] = "Task 4";
            t4["Description"] = "Task tooltip";
            t4["Start"] = DateTime.Today.AddDays(4); //four days later
            t4["Work"] = 24;
            t4["Resources"] = "Diane";
            t4["IconIndex"] = 0; //set first icon from iconsList
            t4["IndentLevel"] = 1;
            t4["PlannedStart"] = DateTime.Today.AddDays(5);
            t4["PlannedWork"] = 24;
            t4["PlannedCompletedWork"] = 0;
            t4["Brush"] = new LinearGradientBrush(new Point(0, 0), new Point(0, 16), Color.FromArgb(128, Color.LightGreen), Color.FromArgb(224, Color.Green));
            dataTable.Rows.Add(t4);

            for (int i = 5; i <= 20; i++)
            {
                DataRow t = dataTable.NewRow();
                t["Name"] = "Task " + i;
                t["Description"] = "Task tooltip";
                t["Start"] = DateTime.Today.AddDays(i);
                t["Work"] = 8 + (i % 20);
                t["CompletedWork"] = i % 20;
                t["IconIndex"] = 0;
                t["PlannedStart"] = DateTime.Today.AddDays(i);
                t["PlannedWork"] = 8 + (i % 20);
                t["PlannedCompletedWork"] = 0;
                dataTable.Rows.Add(t);
            }

            ganttChartView.DataSource = dataTable;

            duringInitialization = false;

            ensureEnableItemUI();
        }

        //Objects binding
        private void objectsReinitializeButton_Click(object sender, EventArgs e)
        {
            CurrentDataBindingMode = DataBindingMode.Objects;

            duringInitialization = true;

            tasks = new BindingList<MyTask>();

            MyTask t1 = new MyTask();
            t1.Name = "Parent Task";
            t1.Description = "This is the first parent task";
            t1.IconIndex = 1; //set second icon from iconsList
            tasks.Add(t1);
            
            MyTask t2 = new MyTask();
            t2.Name = "Task 2";
            t2.Description = "This is the first sub-task of the parent task";
            t2.Start = DateTime.Today;
            t2.Work = 10;
            t2.IconIndex = 0; //set first icon from iconsList
            t2.Markers.Add(6, 0); //add a marker on this task after 6h
            t2.IndentLevel = 1;
            t2.PlannedStart = DateTime.Today;
            t2.PlannedWork = 8;
            t2.PlannedCompletedWork = 0;
            tasks.Add(t2);

            MyTask t3 = new MyTask();
            t3.Name = "Task 3";
            t3.Description = "This is the second sub-task of the parent task\nAnother line of text shown in the tool-tip";
            t3.Start = DateTime.Today.AddDays(3).AddHours(16); //three days later, at 4 PM
            t3.Work = 20;
            t3.IconIndex = 0; //set first icon from iconsList
            t3.Interruptions.Add(10, 4); //add an interruption to this task, after 10 hours of work, for 4 hours of scheduled interrupted work
            t3.Predecessors = "2";
            t3.Resources = "John0";
            t3.IndentLevel = 1;
            t3.PlannedStart = DateTime.Today.AddDays(2);
            t3.PlannedWork = 24;
            t3.PlannedCompletedWork = 2;
            tasks.Add(t3);

            MyTask t4 = new MyTask();
            t4.Name = "Task 4";
            t4.Description = "Task tooltip";
            t4.Start = DateTime.Today.AddDays(4); //four days later
            t4.Work = 24;
            t4.Resources = "Diane";
            t4.IconIndex = 0; //set first icon from iconsList
            t4.Brush = new LinearGradientBrush(new Point(0, 0), new Point(0, 16), Color.FromArgb(128, Color.LightGreen), Color.FromArgb(224, Color.Green));
            t4.IndentLevel = 1;
            t4.PlannedStart = DateTime.Today.AddDays(5);
            t4.PlannedWork = 24;
            t4.PlannedCompletedWork = 0;
            tasks.Add(t4);

            for (int i = 5; i <= 20; i++)
            {
                MyTask t = new MyTask();
                t.Name = "Task " + i;
                t.Description = "Task tooltip";
                t.Start = DateTime.Today.AddDays(i);
                t.Work = 8 + (i % 20);
                t.CompletedWork = i % 20;
                t.IconIndex = 0;
                t.PlannedStart = DateTime.Today.AddDays(i);
                t.PlannedWork = 8 + (i % 20);
                t.PlannedCompletedWork = 0;
                tasks.Add(t);
            }

            ganttChartView.DataSource = tasks;

            ResetTaskHierarchyLists();

            duringInitialization = false;

            ensureEnableItemUI();
        }

        private void ResetTaskHierarchyLists()
        {
            if (tasks == null)
                return;
            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].ParentTask = null;
                List<MyTask> subTasks = tasks[i].SubTasks;
                subTasks.Clear();
                if (ganttChartView.AutoManageParentTasks && i < ganttChartView.TasksTreeGrid.RowCount)
                {
                    foreach (int j in ganttChartView.TasksTreeGrid.GetAllLevelsChildRowsIndexes(i))
                    {
                        if (!ganttChartView.TasksTreeGrid.HasChildRows(j) && j < tasks.Count)
                            subTasks.Add(tasks[j]);
                    }
                    int k = ganttChartView.TasksTreeGrid.GetParentRowIndex(i);
                    if (k >= 0)
                        tasks[i].ParentTask = tasks[k];
                }
            }
            ganttChartView.TasksTreeGrid.InvalidateColumn(startDataGridViewDateTimePickerColumn.Index);
            ganttChartView.TasksTreeGrid.InvalidateColumn(finishDataGridViewDateTimePickerColumn.Index);
        }

        #endregion

        #region Binding Mode

        //The sample can run in three modes, selected from the tab control: 
        //Standard (no binding), DataTable (DataTable binding), and Objects (object binding)
        //When the user hasn't initialized data in any of the available modes, the NotSelected mode is used.
        private enum DataBindingMode
        {
            NotSelected,
            Standard,
            DataTable,
            Objects
        }

        private DataBindingMode currentDataBindingMode;

        //When the current data binding mode changes, we make sure to enable the appropriate user interface controls
        private DataBindingMode CurrentDataBindingMode
        {
            get { return currentDataBindingMode; }
            set
            {
                currentDataBindingMode = value;

                ensureEnableDataUI();
            }
        }

        //Make sure to enable the appropriate user interface controls based on the current data binding mode
        private void ensureEnableDataUI()
        {
            //Only the Initialize button of the current databinding mode is available;
            //When the user hasn't yet initialized data in a specific mode, all Initialize buttons are available.
            standardReinitializeButton.Enabled = CurrentDataBindingMode == DataBindingMode.Standard || CurrentDataBindingMode == DataBindingMode.NotSelected;
            dataTableReinitializeButton.Enabled = CurrentDataBindingMode == DataBindingMode.DataTable || CurrentDataBindingMode == DataBindingMode.NotSelected;
            objectsReinitializeButton.Enabled = CurrentDataBindingMode == DataBindingMode.Objects || CurrentDataBindingMode == DataBindingMode.NotSelected;

            //Only the Clear button of the current databinding mode is available.
            standardClearButton.Enabled = CurrentDataBindingMode == DataBindingMode.Standard;
            dataTableClearButton.Enabled = CurrentDataBindingMode == DataBindingMode.DataTable;
            objectsClearButton.Enabled = CurrentDataBindingMode == DataBindingMode.Objects;

            //Only the Data buttons of the current databinding mode are available
            standardTasksTreeGridGroupBox.Enabled = CurrentDataBindingMode == DataBindingMode.Standard;
            standardGanttChartGroupBox.Enabled = CurrentDataBindingMode == DataBindingMode.Standard;
            dataTableTasksTreeGridGroupBox.Enabled = CurrentDataBindingMode == DataBindingMode.DataTable;
            dataTableGanttChartGroupBox.Enabled = CurrentDataBindingMode == DataBindingMode.DataTable;
            objectsTasksTreeGridGroupBox.Enabled = CurrentDataBindingMode == DataBindingMode.Objects;
            objectsGanttChartGroupBox.Enabled = CurrentDataBindingMode == DataBindingMode.Objects;

            //Only the Print buttons of the current databinding mode are available.
            standardPrintPreviewButton.Enabled = standardPageSetupButton.Enabled = CurrentDataBindingMode == DataBindingMode.Standard;
            dataTablePrintPreviewButton.Enabled = dataTablePageSetupButton.Enabled = CurrentDataBindingMode == DataBindingMode.DataTable;
            objectsPrintPreviewButton.Enabled = objectsPageSetupButton.Enabled = CurrentDataBindingMode == DataBindingMode.Objects;

            //The Gantt Chart view is enabled only after initializing data.
            ganttChartView.Enabled = CurrentDataBindingMode != DataBindingMode.NotSelected;

            //Special data binding grid columns are visible only in data bound modes (DataTable or Objects).
            indentLevelDataGridViewTextBoxColumn.Visible = CurrentDataBindingMode > DataBindingMode.Standard;
            iconIndexDataGridViewTextBoxColumn.Visible = CurrentDataBindingMode > DataBindingMode.Standard;

            //Some grid columns are not visible in standard (no binding) mode
            finishDataGridViewDateTimePickerColumn.Visible = CurrentDataBindingMode == DataBindingMode.Objects;
            percentCompletedDataGridViewTextBoxColumn.Visible = CurrentDataBindingMode == DataBindingMode.Objects;
            isMilestoneDataGridViewCheckBoxColumn.Visible = CurrentDataBindingMode == DataBindingMode.Objects;

            //When we use the standard (no binding) mode, we need to set up the column names as the data members
            if (CurrentDataBindingMode == DataBindingMode.Standard)
            {
                ganttChartView.StartMember = startDataGridViewDateTimePickerColumn.Name;
                ganttChartView.WorkMember = workDataGridViewTextBoxColumn.Name;
                ganttChartView.CompletedWorkMember = completedWorkDataGridViewTextBoxColumn.Name;
                ganttChartView.PredecessorsMember = predecessorsDataGridViewTextBoxColumn.Name;
                ganttChartView.ResourcesMember = resourcesDataGridViewTextBoxColumn.Name;
                ganttChartView.GanttImageIndexMember = string.Empty; //show tree images as Gantt images
                ganttChartView.PlannedStartMember = plannedStartDataGridViewDateTimePickerColumn.Name;
                ganttChartView.PlannedWorkMember = plannedWorkDataGridViewTextBoxColumn.Name;
                ganttChartView.PlannedCompletedWorkMember = plannedCompletedWorkDataGridViewTextBoxColumn.Name;
            }
            else //otherwise set the default data members (as established in the designer)
            {
                ganttChartView.StartMember = "Start";
                ganttChartView.WorkMember = "Work";
                ganttChartView.CompletedWorkMember = "CompletedWork";
                ganttChartView.PredecessorsMember = "Predecessors";
                ganttChartView.ResourcesMember = "Resources";
                ganttChartView.GanttImageIndexMember = "IconIndex";
                ganttChartView.PlannedStartMember = "PlannedStart";
                ganttChartView.PlannedWorkMember = "PlannedWork";
                ganttChartView.PlannedCompletedWorkMember = "PlannedCompletedWork";
            }

            ganttChartView.CurrentDate = ganttChartView.InitialDate;

            ganttChartView.ShowTasksTreeGrid = true;
            standardShowGridCheckBox.Checked = true;
            dataTableShowGridCheckBox.Checked = true;
            objectsShowGridCheckBox.Checked = true;

            ganttChartViewSplitContainer.FixedPanel = FixedPanel.None;
            standardFixedGridCheckBox.Checked = false;
            dataTableFixedGridCheckBox.Checked = false;
            objectsFixedGridCheckBox.Checked = false;

            ganttChartView.ScaleType = ScaleType.Weeks;
            standardScaleComboBox.SelectedIndex = (int)ScaleType.Weeks;
            dataTableScaleComboBox.SelectedIndex = (int)ScaleType.Weeks;
            objectsScaleComboBox.SelectedIndex = (int)ScaleType.Weeks;

            ganttChartView.UpdateScaleType = UpdateScaleType.Hour;
            standardUpdateScaleComboBox.SelectedIndex = (int)UpdateScaleType.Hour;
            dataTableUpdateScaleComboBox.SelectedIndex = (int)UpdateScaleType.Hour;
            objectsUpdateScaleComboBox.SelectedIndex = (int)UpdateScaleType.Hour;

            ganttChartView.Schedule = MyTask.Schedule = Schedule.Default;
            if (finishDataGridViewDateTimePickerColumn.Visible)
                ganttChartView.TasksTreeGrid.InvalidateColumn(finishDataGridViewDateTimePickerColumn.Index);
            standardCalendarComboBox.SelectedIndex = 0;
            dataTableCalendarComboBox.SelectedIndex = 0;
            objectsCalendarComboBox.SelectedIndex = 0;

            ganttChartView.DefaultWorkBrush = new LinearGradientBrush(new Point(0, 0), new Point(0, 16), Color.FromArgb(128, Color.LightBlue), Color.FromArgb(224, Color.Blue));
            standardColorComboBox.SelectedIndex = 0;
            dataTableColorComboBox.SelectedIndex = 0;
            objectsColorComboBox.SelectedIndex = 0;

            ganttChartView.ShowGanttIconImages = false;
            dataTableShowIconsCheckBox.Checked = false;
            objectsShowIconsCheckBox.Checked = false;

            ganttChartView.ShowMarkers = true;
            dataTableShowMarkersCheckBox.Checked = true;
            objectsShowMarkersCheckBox.Checked = true;

            ganttChartView.ShowToolTips = true;
            dataTableShowToolTipsCheckBox.Checked = true;
            objectsShowToolTipsCheckBox.Checked = true;

            ganttChartView.ShowAssignments = true;
            standardShowResourcesCheckBox.Checked = true;
            dataTableShowResourcesCheckBox.Checked = true;
            objectsShowResourcesCheckBox.Checked = true;

            ganttChartView.ShowDependencies = true;
            ganttChartView.AllowGanttUpdateDependencies = true;
            standardShowDependenciesCheckBox.Checked = true;
            dataTableShowDependenciesCheckBox.Checked = true;
            objectsShowDependenciesCheckBox.Checked = true;

            ganttChartView.ShowNonworkingDays = true;
            standardShowExtraDaysCheckBox.Checked = true;
            dataTableShowExtraDaysCheckBox.Checked = true;
            objectsShowExtraDaysCheckBox.Checked = true;

            ganttChartView.ShowNonworkingDaytime = false;
            standardShowExtraDaytimeCheckBox.Checked = false;
            dataTableShowExtraDaytimeCheckBox.Checked = false;
            objectsShowExtraDaytimeCheckBox.Checked = false;

            ganttChartView.HighlightCriticalTasks = false;
            standardHighlightCriticalTasksCheckBox.Checked = false;
            dataTableHighlightCriticalTasksCheckBox.Checked = false;
            objectsHighlightCriticalTasksCheckBox.Checked = false;

            ganttChartView.ShowPlannedValues = false;
            standardShowBaselineCheckBox.Checked = false;
            dataTableShowBaselineCheckBox.Checked = false;
            objectsShowBaselineCheckBox.Checked = false;

            dataTableMarkerTypeComboBox.SelectedIndex = 0;
            objectsMarkerTypeComboBox.SelectedIndex = 0;

            //Establish the markers image list only in data bound modes (DataTable or Objects).
            ganttChartView.MarkersImageList = null;
            if (CurrentDataBindingMode == DataBindingMode.DataTable || CurrentDataBindingMode == DataBindingMode.Objects)
                ganttChartView.MarkersImageList = markersList;

            //Establish available interruption intervals only in data bound modes (DataTable or Objects).
            ganttChartView.AvailableInterruptionTypes.Clear();
            if (CurrentDataBindingMode == DataBindingMode.DataTable || CurrentDataBindingMode == DataBindingMode.Objects)
            {
                ganttChartView.AvailableInterruptionTypes.Add(1);
                ganttChartView.AvailableInterruptionTypes.Add(2);
                ganttChartView.AvailableInterruptionTypes.Add(4);
                ganttChartView.AvailableInterruptionTypes.Add(8);
            }

            //Establish available resources.
            ganttChartView.AvailableResources.Clear();
            ganttChartView.AvailableResources.Add("John1");
            ganttChartView.AvailableResources.Add("Diane");

            //Disabled modes notification
            standardDisabledLabel.Visible = CurrentDataBindingMode != DataBindingMode.Standard && CurrentDataBindingMode != DataBindingMode.NotSelected;
            dataTableDisabledLabel.Visible = CurrentDataBindingMode != DataBindingMode.DataTable && CurrentDataBindingMode != DataBindingMode.NotSelected;
            objectsDisabledLabel.Visible = CurrentDataBindingMode != DataBindingMode.Objects && CurrentDataBindingMode != DataBindingMode.NotSelected;
        }

        #endregion

        #region Item Navigation

        //When the current cell changes (the user navigates in the grid), we need to make sure the appropriate user interface controls are enabled
        private void ganttChartViewTasksTreeGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            if (duringInitialization)
                return;
            //We delegate the functionality on a timer, to make sure it doesn't occur multiple times
            //when the CurrentCellChanged occurs multiple times in a short time
            timerCurrentCellChanged.Stop(); //reset timer interval if already started
            timerCurrentCellChanged.Start();
        }

        private void timerCurrentCellChanged_Tick(object sender, EventArgs e)
        {
            timerCurrentCellChanged.Stop(); //stop timer
            ensureEnableItemUI();
        }

        //After cell is edited changes, we need to make sure the appropriate user interface controls are enabled,
        //because the user might have just changed a property such as Is Expanded or IndentLevel.
        private void ganttChartViewTasksTreeGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ensureEnableItemUI(e.RowIndex);
        }

        //Make sure to enable the appropriate user interface controls based on the currently selected row index
        private void ensureEnableItemUI()
        {
            int rowIndex = ganttChartViewTasksTreeGrid.CurrentRow != null ? ganttChartViewTasksTreeGrid.CurrentRow.Index : -1;
            ensureEnableItemUI(rowIndex);
        }

        //Make sure to enable the appropriate user interface controls based on the specified selected row index
        private void ensureEnableItemUI(int rowIndex)
        {
            switch (CurrentDataBindingMode)
            {
                case DataBindingMode.Standard:
                    standardExpandButton.Enabled = rowIndex >= 0 && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible && (ganttChartViewTasksTreeGrid.CanExpand(rowIndex) || ganttChartViewTasksTreeGrid.CanClose(rowIndex));
                    standardIncreaseIndentButton.Enabled = rowIndex >= 0 && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible && ganttChartViewTasksTreeGrid.CanIncreaseIndent(rowIndex);
                    standardDecreaseIndentButton.Enabled = rowIndex >= 0 && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible && ganttChartViewTasksTreeGrid.CanDecreaseIndent(rowIndex);
                    standardSetIconLabel.Enabled = standardSetIconComboBox.Enabled = rowIndex >= 0 && rowIndex != ganttChartViewTasksTreeGrid.NewRowIndex && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible;
                    standardSetIconComboBox.SelectedIndex = -1;
                    break;

                case DataBindingMode.DataTable:
                    dataTableExpandButton.Enabled = rowIndex >= 0 && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible && (ganttChartViewTasksTreeGrid.CanExpand(rowIndex) || ganttChartViewTasksTreeGrid.CanClose(rowIndex));
                    dataTableIncreaseIndentButton.Enabled = rowIndex >= 0 && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible && ganttChartViewTasksTreeGrid.CanIncreaseIndent(rowIndex);
                    dataTableDecreaseIndentButton.Enabled = rowIndex >= 0 && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible && ganttChartViewTasksTreeGrid.CanDecreaseIndent(rowIndex);
                    dataTableSetIconLabel.Enabled = dataTableSetIconComboBox.Enabled = rowIndex >= 0 && rowIndex != ganttChartViewTasksTreeGrid.NewRowIndex && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible;
                    dataTableSetIconComboBox.SelectedIndex = -1;
                    dataTableSetColorLabel.Enabled = dataTableSetColorComboBox.Enabled = rowIndex >= 0 && rowIndex != ganttChartViewTasksTreeGrid.NewRowIndex && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible;
                    dataTableSetColorComboBox.SelectedIndex = -1;
                    dataTableInterruptionsGroupBox.Enabled = rowIndex >= 0 && rowIndex != ganttChartViewTasksTreeGrid.NewRowIndex && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible;
                    dataTableMarkersGroupBox.Enabled = rowIndex >= 0 && rowIndex != ganttChartViewTasksTreeGrid.NewRowIndex && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible;
                    break;

                case DataBindingMode.Objects:
                    objectsExpandButton.Enabled = rowIndex >= 0 && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible && (ganttChartViewTasksTreeGrid.CanExpand(rowIndex) || ganttChartViewTasksTreeGrid.CanClose(rowIndex));
                    objectsIncreaseIndentButton.Enabled = rowIndex >= 0 && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible && ganttChartViewTasksTreeGrid.CanIncreaseIndent(rowIndex);
                    objectsDecreaseIndentButton.Enabled = rowIndex >= 0 && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible && ganttChartViewTasksTreeGrid.CanDecreaseIndent(rowIndex);
                    objectsSetIconLabel.Enabled = objectsSetIconComboBox.Enabled = rowIndex >= 0 && rowIndex != ganttChartViewTasksTreeGrid.NewRowIndex && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible;
                    objectsSetIconComboBox.SelectedIndex = -1;
                    objectsSetColorLabel.Enabled = objectsSetColorComboBox.Enabled = rowIndex >= 0 && rowIndex != ganttChartViewTasksTreeGrid.NewRowIndex && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible;
                    objectsSetColorComboBox.SelectedIndex = -1;
                    objectsInterruptionsGroupBox.Enabled = rowIndex >= 0 && rowIndex != ganttChartViewTasksTreeGrid.NewRowIndex && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible;
                    objectsMarkersGroupBox.Enabled = rowIndex >= 0 && rowIndex != ganttChartViewTasksTreeGrid.NewRowIndex && ganttChartViewTasksTreeGrid.Rows[rowIndex].Visible;
                    break;
            }
        }

        #endregion

        #region Main Actions

        private void showGridCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            ganttChartView.ShowTasksTreeGrid = checkBox.Checked;

            CheckBox fixedGridCheckBox = GetNextControl(checkBox, false) as CheckBox;
            fixedGridCheckBox.Enabled = checkBox.Checked;
        }

        private void fixedGridCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            ganttChartViewSplitContainer.FixedPanel = checkBox.Checked ? FixedPanel.Panel1 : FixedPanel.None;
        }

        private void scaleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ganttChartView.ScaleType = (ScaleType)comboBox.SelectedIndex;
        }

        private void updateScaleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ganttChartView.UpdateScaleType = (UpdateScaleType)comboBox.SelectedIndex;
        }

        private void calendarComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ganttChartView.Schedule = MyTask.Schedule = comboBox.SelectedIndex == 0 ? Schedule.Default : GetCustomSchedule();
            if (finishDataGridViewDateTimePickerColumn.Visible)
                ganttChartView.TasksTreeGrid.InvalidateColumn(finishDataGridViewDateTimePickerColumn.Index);
        }

        private Schedule GetCustomSchedule()
        {
            DaytimeInterval[] defaultScheduleIntervals = new DaytimeInterval[] { new DaytimeInterval(8, 6) }; //starting from 8 AM, 6 hours
            Dictionary<DayOfWeek, DaytimeInterval[]> specialDayOfWeekScheduleIntervals = new Dictionary<DayOfWeek, DaytimeInterval[]>();
            specialDayOfWeekScheduleIntervals[DayOfWeek.Saturday] = new DaytimeInterval[] { new DaytimeInterval(8, 3) }; //3 hours only on Saturday
            specialDayOfWeekScheduleIntervals[DayOfWeek.Sunday] = DaytimeInterval.NonWorkingDay; //no work on Sunday
            return new Schedule(defaultScheduleIntervals, specialDayOfWeekScheduleIntervals, DaytimeInterval.DefaultHolidays);
        }

        private void colorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ganttChartView.DefaultWorkBrush = GetCustomBrush(comboBox.SelectedIndex);
        }

        private Brush GetCustomBrush(int index)
        {
            switch (index)
            {
                default:
                    return new LinearGradientBrush(new Point(0, 0), new Point(0, 16), Color.FromArgb(128, Color.LightBlue), Color.FromArgb(224, Color.Blue));
                case 1:
                    return new HatchBrush(HatchStyle.Percent50, Color.Blue, Color.Transparent);
                case 2:
                    return new HatchBrush(HatchStyle.Percent50, Color.Green, Color.Transparent);
                case 3:
                    return new HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.Blue, Color.Transparent);
                case 4:
                    return new LinearGradientBrush(new Point(0, 0), new Point(0, 16), Color.FromArgb(128, Color.LightGreen), Color.FromArgb(224, Color.Green));
            }
        }

        private void showIconsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            ganttChartView.ShowGanttIconImages = checkBox.Checked;
        }

        private void showMarkersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            ganttChartView.ShowMarkers = checkBox.Checked;

            dataTableMarkersGroupBox.Enabled = CurrentDataBindingMode == DataBindingMode.DataTable && checkBox.Checked;
            objectsMarkersGroupBox.Enabled = CurrentDataBindingMode == DataBindingMode.Objects && checkBox.Checked;
        }

        private void showToolTipsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            ganttChartView.ShowToolTips = checkBox.Checked;
        }

        private void showResourcesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            ganttChartView.ShowAssignments = checkBox.Checked;
        }

        private void showDependenciesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            ganttChartView.ShowDependencies = checkBox.Checked;
            ganttChartView.AllowGanttUpdateDependencies = checkBox.Checked;
        }

        private void showExtraDaysCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            ganttChartView.ShowNonworkingDays = checkBox.Checked;
        }

        private void showExtraDaytimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            ganttChartView.ShowNonworkingDaytime = checkBox.Checked;
        }

        private void highlightCriticalTasksCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            ganttChartView.HighlightCriticalTasks = checkBox.Checked;
        }

        private void showBaselineCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            ganttChartView.ShowPlannedValues = checkBox.Checked;
        }

        private void pageSetupButton_Click(object sender, EventArgs e)
        {
            ganttChartView.PageSetup();
        }

        private void printPreviewButton_Click(object sender, EventArgs e)
        {
            ganttChartView.PrintPreview();
        }

        #endregion

        #region Current Item Actions

        private void expandButton_Click(object sender, EventArgs e)
        {
            int rowIndex = ganttChartView.TasksTreeGrid.CurrentRow.Index;
            if (ganttChartView.TasksTreeGrid.IsClosed(rowIndex))
                ganttChartView.TasksTreeGrid.Expand(rowIndex);
            else
                ganttChartView.TasksTreeGrid.Close(rowIndex);
            ensureEnableItemUI(rowIndex);
        }

        private void increaseIndentButton_Click(object sender, EventArgs e)
        {
            int rowIndex = ganttChartView.TasksTreeGrid.CurrentRow.Index;
            ganttChartView.TasksTreeGrid.IncreaseIndent(rowIndex);
            ensureEnableItemUI(rowIndex);
        }

        private void decreaseIndentButton_Click(object sender, EventArgs e)
        {
            int rowIndex = ganttChartView.TasksTreeGrid.CurrentRow.Index;
            ganttChartView.TasksTreeGrid.DecreaseIndent(rowIndex);
            ensureEnableItemUI(rowIndex);
        }

        private void setIconComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedIndex >= 0)
            {
                int rowIndex = ganttChartView.TasksTreeGrid.CurrentRow.Index;
                ganttChartView.TasksTreeGrid.SetImageIndex(rowIndex, comboBox.SelectedIndex);
            }
        }

        private void dataTableSetColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataTableSetColorComboBox.SelectedIndex >= 0)
            {
                int rowIndex = ganttChartView.TasksTreeGrid.CurrentRow.Index;
                dataTable.DefaultView[rowIndex]["Brush"] = GetCustomBrush(dataTableSetColorComboBox.SelectedIndex);
            }
        }

        private void dataTableInterruptionAddButton_Click(object sender, EventArgs e)
        {
            int rowIndex = ganttChartView.TasksTreeGrid.CurrentRow.Index;
            Dictionary<double, double> interruptions = new Dictionary<double, double>(dataTable.DefaultView[rowIndex]["Interruptions"] as Dictionary<double, double>);
            double start = (double)dataTableInterruptionAtNumericUpDown.Value;
            double duration = (double)dataTableInterruptionForNumericUpDown.Value;
            if (!interruptions.ContainsKey(start))
                interruptions.Add(start, duration);
            else
                interruptions[start] = duration;
            dataTable.DefaultView[rowIndex]["Interruptions"] = interruptions;
        }

        private void dataTableInterruptionsClearButton_Click(object sender, EventArgs e)
        {
            int rowIndex = ganttChartView.TasksTreeGrid.CurrentRow.Index;
            dataTable.DefaultView[rowIndex]["Interruptions"] = new Dictionary<double, double>();
        }

        private void dataTableMarkerAddButton_Click(object sender, EventArgs e)
        {
            if (dataTableMarkerTypeComboBox.SelectedIndex >= 0)
            {
                int rowIndex = ganttChartView.TasksTreeGrid.CurrentRow.Index;
                Dictionary<double, int> markers = new Dictionary<double, int>(dataTable.DefaultView[rowIndex]["Markers"] as Dictionary<double, int>);
                double start = (double)dataTableMarkerAtNumericUpDown.Value;
                int type = dataTableMarkerTypeComboBox.SelectedIndex;
                if (!markers.ContainsKey(start))
                    markers.Add(start, type);
                else
                    markers[start] = type;
                dataTable.DefaultView[rowIndex]["Markers"] = markers;
            }
        }

        private void dataTableMarkersClearButton_Click(object sender, EventArgs e)
        {
            int rowIndex = ganttChartView.TasksTreeGrid.CurrentRow.Index;
            Dictionary<double, int> markers = dataTable.DefaultView[rowIndex]["Markers"] as Dictionary<double, int>;
            markers.Clear();
            ganttChartView.Rebind();
        }

        private void objectsSetColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (objectsSetColorComboBox.SelectedIndex >= 0)
            {
                int rowIndex = ganttChartView.TasksTreeGrid.CurrentRow.Index;
                tasks[rowIndex].Brush = GetCustomBrush(objectsSetColorComboBox.SelectedIndex);
            }
        }

        private void objectsInterruptionAddButton_Click(object sender, EventArgs e)
        {
            int rowIndex = ganttChartView.TasksTreeGrid.CurrentRow.Index;
            InterruptionCollection interruptions = tasks[rowIndex].Interruptions;
            double start = (double)objectsInterruptionAtNumericUpDown.Value;
            double duration = (double)objectsInterruptionForNumericUpDown.Value;
            if (interruptions.Contains(start))
                interruptions.Remove(start);
            interruptions.Add(start, duration);
        }

        private void objectsInterruptionsClear_Click(object sender, EventArgs e)
        {
            int rowIndex = ganttChartView.TasksTreeGrid.CurrentRow.Index;
            tasks[rowIndex].Interruptions.Clear();
        }

        private void objectsMarkerAddButton_Click(object sender, EventArgs e)
        {
            if (objectsMarkerTypeComboBox.SelectedIndex >= 0)
            {
                int rowIndex = ganttChartView.TasksTreeGrid.CurrentRow.Index;
                MarkerCollection markers = tasks[rowIndex].Markers;
                double start = (double)objectsMarkerAtNumericUpDown.Value;
                int type = objectsMarkerTypeComboBox.SelectedIndex;
                if (markers.Contains(start))
                    markers.Remove(start);
                markers.Add(start, type);
            }
        }

        private void objectsMarkersClearButton_Click(object sender, EventArgs e)
        {
            int rowIndex = ganttChartView.TasksTreeGrid.CurrentRow.Index;
            tasks[rowIndex].Markers.Clear();
        }

        #endregion

        #region Special Columns Management

        private void ganttChartViewTasksTreeGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //Do not allow editing the custom milestone and finish columns for parent rows, beside the columns already treated the same way by the tree-grid control
            string propertyName = ganttChartViewTasksTreeGrid.Columns[e.ColumnIndex].DataPropertyName;
            if (!ganttChartView.AllowUpdateParentTasks && ganttChartViewTasksTreeGrid.HasChildRows(e.RowIndex) && !string.IsNullOrEmpty(propertyName) &&
                (propertyName == "IsMilestone" || propertyName == "Finish"))
            {
                e.Cancel = true;
                return;
            }
        }

        #endregion

        #region Internal Event Handlers

        private void ganttChartViewTasksTreeGrid_RowIndentLevelChanged(object sender, DataGridViewRowEventArgs e)
        {
            //Update the sub-tasks lists
            ResetTaskHierarchyLists();
        }

        private void ganttChartView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AutoManageParentTasks")
                ResetTaskHierarchyLists();

            //Synchronize properties with Resource Load Chart
            if (e.PropertyName == "DataSource")
                resourceLoadChartView.DataSource = ganttChartView.DataSource;
            if (e.PropertyName == "DataMember")
                resourceLoadChartView.DataMember = ganttChartView.DataMember;
            if (e.PropertyName == "Schedule")
                resourceLoadChartView.Schedule = ganttChartView.Schedule;
            if (e.PropertyName == "CurrentDate")
                resourceLoadChartView.CurrentDate = ganttChartView.CurrentDate;
            if (e.PropertyName == "ScaleType")
                resourceLoadChartView.ScaleType = ganttChartView.ScaleType;
            if (e.PropertyName == "HourScale")
                resourceLoadChartView.HourScale = ganttChartView.HourScale;
            if (e.PropertyName == "ShowNonworkingDaytime")
                resourceLoadChartView.ShowNonworkingDaytime = ganttChartView.ShowNonworkingDaytime;
            if (e.PropertyName == "ShowNonworkingDays")
                resourceLoadChartView.ShowNonworkingDays = ganttChartView.ShowNonworkingDays;
        }

        private void ganttChartViewSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            resourceLoadChartViewResourceLoadChartCurrentDateScrollBar.Left = resourceLoadChartView.ResourceLoadScaleWidth = ganttChartViewSplitContainer.SplitterDistance + ganttChartViewSplitContainer.SplitterWidth;
            resourceLoadChartViewResourceLoadChartCurrentDateScrollBar.Width = resourceLoadChartViewResourceLoadChartArea.Width;
        }

        private void resourceLoadChartView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentDate")
                ganttChartView.CurrentDate = resourceLoadChartView.CurrentDate;
        }

        private void ganttChartViewTasksTreeGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            ganttChartViewTasksTreeGrid.CancelEdit();
        }

        private void ganttChartViewGanttChartHeader_Click(object sender, EventArgs e)
        {
            //Uncomment these to allow changing the Gantt Chart scale type by clicking on the Gantt Chart header
            //ScaleType nextScaleType = ganttChartViewGanttChartHeader.ScaleType + 1;
            //if (nextScaleType > ScaleType.Years)
            //    nextScaleType = ScaleType.Days;
            //ganttChartViewGanttChartHeader.ScaleType = nextScaleType;
        }

        private void ganttChartViewGanttChartArea_Paint(object sender, PaintEventArgs e)
        {
            //Uncomment this to provide custom drawings below the Gantt Chart
            //e.Graphics.FillRectangle(new HatchBrush(HatchStyle.Percent20, Color.Yellow, Color.Transparent), e.ClipRectangle);
        }

        private void ganttChartViewGanttChartArea_PostPaint(object sender, PaintEventArgs e)
        {
            //Uncomment this to provide custom drawings over the Gantt Chart
            //e.Graphics.DrawRectangle(new Pen(new HatchBrush(HatchStyle.Percent20, Color.Brown, Color.Transparent)), e.ClipRectangle);
        }

        #endregion

        #region Data Clear

        private void clearButton_Click(object sender, EventArgs e)
        {
            duringInitialization = true;

            ganttChartView.DataMember = string.Empty;
            ganttChartView.DataSource = null;
            ganttChartView.TasksTreeGrid.Rows.Clear();

            duringInitialization = false;

            ensureEnableItemUI();

            CurrentDataBindingMode = DataBindingMode.NotSelected;
        }

        #endregion

        #region Properties and Close

        private void propertiesButton_Click(object sender, EventArgs e)
        {
            propertiesButton.Enabled = false;

            PropertiesForm propertiesForm = new PropertiesForm();
            propertiesForm.Text = string.Format(propertiesForm.Text, ganttChartView.GetType().Name);
            propertiesForm.Location = new Point(this.Left + this.Width, this.Top);
            if (propertiesForm.Left + propertiesForm.Width > Screen.GetWorkingArea(propertiesForm).Width)
                propertiesForm.Left = Screen.GetWorkingArea(propertiesForm).Width - propertiesForm.Width;
            propertiesForm.Height = this.Height;
            propertiesForm.Control = this.ganttChartView;
            propertiesForm.FormClosed += new FormClosedEventHandler(propertiesForm_FormClosed);
            propertiesForm.Show(this);
        }

        private void propertiesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            propertiesButton.Enabled = true;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        private void MyForm_Load(object sender, EventArgs e)
        {

        }
    }
}