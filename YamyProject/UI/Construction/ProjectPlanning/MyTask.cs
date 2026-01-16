using DlhSoft.ProjectDataControlLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YamyProject.UI.Construction.ProjectPlanning
{
    public class MyTask : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value == Name)
                    return;
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value == Description)
                    return;
                description = value;
                OnPropertyChanged("Description");
            }
        }

        private DateTime start = Schedule.GetStart(DateTime.Today.AddDays(1));
        public DateTime Start
        {
            get { return start; }
            set
            {
                if (value == Start)
                    return;
                start = Schedule.GetStart(value);
                OnPropertyChanged("Start");
                OnPropertyChanged("Finish");
                if (ParentTask != null)
                    ParentTask.OnPropertyChanged("Finish");
            }
        }

        private DateTime plannedStart = Schedule.GetStart(DateTime.Today.AddDays(1));
        public DateTime PlannedStart
        {
            get { return plannedStart; }
            set
            {
                if (value == PlannedStart)
                    return;
                plannedStart = Schedule.GetStart(value);
                OnPropertyChanged("PlannedStart");
            }
        }

        private double work = 8;
        public double Work
        {
            get { return work; }
            set
            {
                if (value == Work)
                    return;
                work = value;
                OnPropertyChanged("Work");
                OnPropertyChanged("IsMilestone");
                OnPropertyChanged("Finish");
                if (ParentTask != null)
                    ParentTask.OnPropertyChanged("Finish");
                OnPropertyChanged("PercentCompleted");
            }
        }

        private double plannedWork = 8;
        public double PlannedWork
        {
            get { return plannedWork; }
            set
            {
                if (value == PlannedWork)
                    return;
                plannedWork = value;
                OnPropertyChanged("PlannedWork");
            }
        }

        //Computed based on specified Schedule
        public DateTime Finish
        {
            get
            {
                DateTime finish = DateTime.MinValue;
                if (SubTasks.Count == 0)
                {
                    finish = Schedule.GetFinish(Start, Work, Interruptions);
                }
                else
                {
                    foreach (MyTask subTask in SubTasks)
                    {
                        DateTime subFinish = subTask.Finish;
                        if (subFinish > finish)
                            finish = subFinish;
                    }
                }
                return finish;
            }
            set
            {
                if (value == Finish)
                    return;
                Work = Schedule.GetWork(Start, value, Interruptions);
            }
        }

        private double completedWork;
        public double CompletedWork
        {
            get { return completedWork; }
            set
            {
                if (value == CompletedWork)
                    return;
                completedWork = value;
                OnPropertyChanged("CompletedWork");
                OnPropertyChanged("PercentCompleted");
            }
        }

        private double plannedCompletedWork;
        public double PlannedCompletedWork
        {
            get { return plannedCompletedWork; }
            set
            {
                if (value == PlannedCompletedWork)
                    return;
                plannedCompletedWork = value;
                OnPropertyChanged("PlannedCompletedWork");
            }
        }

        public double? PercentCompleted
        {
            get { return Work != 0 && !double.IsNaN(Work) ? (double?)(CompletedWork / Work) : null; }
        }

        public bool IsMilestone
        {
            get
            {
                return Work == 0;
            }
            set
            {
                if (value == IsMilestone)
                    return;
                Work = value ? 0 : 8;
            }
        }

        //Used to update Finish date for parent tasks
        private MyTask parentTask;
        public MyTask ParentTask
        {
            get { return parentTask; }
            set { parentTask = value; }
        }

        //Used to determine Finish date for parent tasks
        private List<MyTask> subTasks = new List<MyTask>();
        public List<MyTask> SubTasks
        {
            get { return subTasks; }
        }

        private string predecessors;
        public string Predecessors
        {
            get { return predecessors; }
            set
            {
                if (value == Predecessors)
                    return;
                predecessors = value;
                OnPropertyChanged("Predecessors");
            }
        }

        private string resources;
        public string Resources
        {
            get { return resources; }
            set
            {
                if (value == Resources)
                    return;
                resources = value;
                OnPropertyChanged("Resources");
            }
        }

        private int iconIndex = 0;
        public int IconIndex
        {
            get { return iconIndex; }
            set
            {
                if (value == IconIndex)
                    return;
                iconIndex = value;
                OnPropertyChanged("IconIndex");
            }
        }

        private int indentLevel;
        public int IndentLevel
        {
            get { return indentLevel; }
            set
            {
                if (value == IndentLevel)
                    return;
                indentLevel = value;
                OnPropertyChanged("IndentLevel");
            }
        }

        private InterruptionCollection interruptions;
        public InterruptionCollection Interruptions
        {
            get
            {
                if (interruptions == null)
                {
                    interruptions = new InterruptionCollection();
                    interruptions.ListChanged += interruptions_ListChanged;
                }
                return interruptions;
            }
            set
            {
                if (value == Interruptions)
                    return;
                interruptions = value;
                OnPropertyChanged("Interruptions");
            }
        }
        private void interruptions_ListChanged(object sender, ListChangedEventArgs e)
        {
            OnPropertyChanged("Interruptions");
        }

        private MarkerCollection markers;
        public MarkerCollection Markers
        {
            get
            {
                if (markers == null)
                {
                    markers = new MarkerCollection();
                    markers.ListChanged += markers_ListChanged;
                }
                return markers;
            }
            set
            {
                if (value == Markers)
                    return;
                markers = value;
                OnPropertyChanged("Markers");
            }
        }
        private void markers_ListChanged(object sender, ListChangedEventArgs e)
        {
            OnPropertyChanged("Markers");
        }

        private Brush brush;
        public Brush Brush
        {
            get { return brush; }
            set
            {
                if (value == Brush)
                    return;
                brush = value;
                OnPropertyChanged("Brush");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Schedule

        private static Schedule schedule = Schedule.Default;
        public static Schedule Schedule
        {
            get { return schedule; }
            set { schedule = value; }
        }

        #endregion
    }
}
