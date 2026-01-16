using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmAddUnitsToItem : Form
    {
        private int id;
        private frmViewItem _item;
        private EventHandler itemUnitUpdatedHandler;

        public frmAddUnitsToItem(frmViewItem _item, int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            itemUnitUpdatedHandler = (sender, args) => BindCombos.PopulateItemUnit(cmbUnit);
            EventHub.ItemUnit += itemUnitUpdatedHandler;
            this._item = _item;
            this.id = id;
            headerUC1.FormText = id == 0 ? "Add Item UOM" : "Edit Item UOM";
        }

        private void frmAddUnitsToItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.ItemUnit -= itemUnitUpdatedHandler;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddUnitsToItem_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateItemUnit(cmbUnit);
            if (id != 0)
            {
                for (int i = 0; i < _item.unitIds.Count; i++)
                {
                    dgvLvl1.Rows.Add(_item.unitIds[i].ToString(), _item.unitNames[i].ToString(), _item.unitFactors[i].ToString());
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateItemUnitLists(_item.unitIds, _item.unitFactors, _item.unitNames);
            this.Close();
        }

        private void UpdateItemUnitLists(List<int> unitIds, List<decimal> unitFactors, List<string> unitNames)
        {
            foreach (DataGridViewRow row in dgvLvl1.Rows)
            {
                unitIds.Add(int.Parse(row.Cells["unitid"].Value.ToString()));
                unitFactors.Add(decimal.Parse(row.Cells["factor"].Value.ToString()));
                unitNames.Add(row.Cells["name"].Value.ToString());
            }
        }

        private void lnkNewUnit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewItemUnit().ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbUnit.Text == "")
            {
                MessageBox.Show("Choose Unit First");
                return;
            }
            decimal factor;
            if (!decimal.TryParse(txtFactor.Text, out  factor))
            {
                MessageBox.Show("Please enter a valid factor value.");
                return;
            }

            if (cmbUnit.Text == _item.cmbMainUnit.Text)
            {
                MessageBox.Show("Main Unit Can't Be Added As Sub Unit");
                return;
            }

            if (dgvLvl1.Rows.Cast<DataGridViewRow>().Any(row => row.Cells["name"].Value.ToString() == cmbUnit.Text))
            {
                MessageBox.Show("Unit Already Exists");
                return;
            }

            dgvLvl1.Rows.Add(cmbUnit.SelectedValue.ToString(), cmbUnit.Text, txtFactor.Text);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvLvl1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to remove.");
                return;
            }
            dgvLvl1.Rows.Remove(dgvLvl1.SelectedRows[0]);
        }

        private void LbHeader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal; // Restore down
            }
            else
            {
                this.WindowState = FormWindowState.Maximized; // Maximize
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
