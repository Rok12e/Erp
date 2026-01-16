using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YamyProject.UI.Construction.ProjectPlanning
{
    class DataGridViewDateTimePickerColumn : DataGridViewColumn
    {
        public DataGridViewDateTimePickerColumn() : base(new DateTimePickerCell())
        { }
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                //Ensure that the cell used for the template is a DateTimePickerCell.
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DateTimePickerCell)))
                {
                    throw new InvalidCastException("Must be a DateTimePickerCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class DateTimePickerCell : DataGridViewTextBoxCell
    {
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            //Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            DateTimePickerEditingControl ctl = DataGridView.EditingControl as DateTimePickerEditingControl;
            DateTime value = this.Value is DateTime ? (DateTime)this.Value : DateTime.Now;
            ctl.Value = value >= ctl.MinDate && value <= ctl.MaxDate ? value : (value < ctl.MinDate ? ctl.MinDate : ctl.MaxDate);
        }

        public override Type EditType
        {
            get
            {
                //Return the type of the editing contol that CalendarCell uses.
                return typeof(DateTimePickerEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                //Return the type of the value that CalendarCell contains.
                return typeof(DateTime);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                //Use the tomorrow date as the default value.
                return DBNull.Value;
            }
        }
    }

    internal class DateTimePickerEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public DateTimePickerEditingControl()
        {
            this.Format = DateTimePickerFormat.Custom;
            this.CustomFormat = string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);
        }

        //Implements the IDataGridViewEditingControl.EditingControlFormattedValue property.
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Value.ToString("g");
            }
            set
            {
                if (value is string)
                {
                    this.Value = DateTime.Parse((string)value);
                }
            }
        }

        //Implements the IDataGridViewEditingControl.GetEditingControlFormattedValue method.
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        //Implements the IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }

        //Implements the IDataGridViewEditingControl.EditingControlRowIndex property.
        public int EditingControlRowIndex
        {
            get { return rowIndex; }
            set { rowIndex = value; }
        }

        //Implements the IDataGridViewEditingControl.EditingControlWantsInputKey method.
        public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
        {
            //Let the DateTimePicker handle the keys listed.
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return false;
            }
        }

        //Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit method.
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            //No preparation needs to be done.
        }

        //Implements the IDataGridViewEditingControl.RepositionEditingControlOnValueChange property.
        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        //Implements the IDataGridViewEditingControl.EditingControlDataGridView property.
        public DataGridView EditingControlDataGridView
        {
            get { return dataGridView; }
            set { dataGridView = value; }
        }

        //Implements the IDataGridViewEditingControl.EditingControlValueChanged property.
        public bool EditingControlValueChanged
        {
            get { return valueChanged; }
            set { valueChanged = value; }
        }

        //Implements the IDataGridViewEditingControl.EditingPanelCursor property.
        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }

        protected override void OnValueChanged(EventArgs e)
        {
            //Notify the DataGridView that the contents of the cell has changed.
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(e);
        }
    }
}
