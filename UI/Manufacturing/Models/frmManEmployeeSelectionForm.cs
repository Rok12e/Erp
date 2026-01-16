using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace YamyProject.UI.Manufacturing.Models
{
    public partial class frmManEmployeeSelectionForm : Form
    {
        public List<EmployeeModel> SelectedEmployees { get; private set; } = new List<EmployeeModel>();

        public frmManEmployeeSelectionForm(List<EmployeeModel> employeeList)
        {
            InitializeComponent();
            foreach (var emp in employeeList)
            {
                checkedListBoxEmployees.Items.Add(emp);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (EmployeeModel item in checkedListBoxEmployees.CheckedItems)
            {
                SelectedEmployees.Add(item);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}