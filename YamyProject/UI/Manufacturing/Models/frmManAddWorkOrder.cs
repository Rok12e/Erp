using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using YamyProject.RMS.Class;

namespace YamyProject.UI.Manufacturing.Models
{

    public partial class frmManAddWorkOrder : Form
    {
        public int id = 0;
        bool AllowItemWithOutQty = true;

        public frmManAddWorkOrder(int _id=0)
        {
            this.id = _id;
            InitializeComponent();
        }

        private DataTable dataTable;
        private void frmManAddWorkOrder_Load(object sender, EventArgs e)
        {
            dtInv.Value = DateTime.Now.Date;
            var generalS = Utilities.GeneralSettingsState("ALLOW ITEM WITHOUT QTY");
            if (!string.IsNullOrEmpty(generalS) & int.Parse(generalS) > 0)
            {
                AllowItemWithOutQty = true;
            }
            else
            {
                AllowItemWithOutQty = false;
            }
            lbtotal.Text = ("0.00").ToString();
            getdata();
            dgvitem2.Visible = false;
            LoadCombo();

            if (id > 0)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT product_id,warehouse_id,batchname,hours,amount,Description,date,fixedassetsID,Costamount,product_qty FROM tbl_manufacturer_batch WHERE id = @id", DBClass.CreateParameter("id", id)))
                {
                    if (reader.Read())
                    {
                        cmbProduct.SelectedValue = Convert.ToInt32(reader["product_id"]);
                        cmbWarehouse.SelectedValue = Convert.ToInt32(reader["warehouse_id"]);
                        txtname.Text = Convert.ToString(reader["batchname"]);
                        txtamount.Text = Convert.ToString(reader["amount"]);
                        txthourse.Text = Convert.ToString(reader["hours"]);
                        cbMachin.SelectedValue = Convert.ToString(reader["fixedassetsID"]);
                        txtdiscription.Text = Convert.ToString(reader["Description"]);
                        lbtotal.Text = Convert.ToString(reader["Costamount"]);
                        NoDropQty.Text = Convert.ToString(reader["product_qty"]);
                        GetDataforupdatedetails();
                    }
                }
            }
        }

        private void LoadCombo()
        {
            BindCombos.LoadMachincombox(cbMachin);
            BindCombos.PopulateItems(cmbProduct);
            cmbProduct.SelectedIndex = -1;
            BindCombos.PopulateWarehouse(cmbWarehouse);
        }


        private void getdata()
        {
            string qry1 = @"Select id,code,name,cost_price from  tbl_items order by name";
            DataTable DT = DBClass.ExecuteDataTable(qry1);
            dgvitem2.DataSource = DT;
            dgvitem2.Columns[0].Visible = false;
            dgvitem2.Columns[2].Width = 90;
            dgvitem2.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
        }
        private void getdatasearchname(string st)
        {

            string qry1 = @"Select id,code,name,cost_price from  tbl_items where name like '%" + st + "%'";

            DataTable DT = DBClass.ExecuteDataTable(qry1);
            dgvitem2.DataSource = DT;
            dgvitem2.Columns[0].Visible = false;
            dgvitem2.Columns[2].Width = 90;
            dgvitem2.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
        }

        private void grandtotal()
        {
            double gtot = 0;
            double tot = 0;

            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (!row.IsNewRow && row.Cells["DGVTOTATL"].Value != null)
                {
                    if (double.TryParse(Convert.ToString(row.Cells["DGVTOTATL"].Value), out tot))
                    {
                        gtot += tot;
                        lbtotal.Text = gtot.ToString();
                    }
                }
            }
        }

        private void dgvItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgvItems.EditMode = DataGridViewEditMode.EditOnEnter;

            int row = dgvItems.CurrentCell.RowIndex;
            double Price, qty = 0;
            double.TryParse(Convert.ToString(dgvItems.Rows[row].Cells[4].Value), out Price);
            double.TryParse(Convert.ToString(dgvItems.Rows[row].Cells[5].Value), out qty);
            dgvItems.Rows[row].Cells[6].Value = qty * Price;

        }

        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //
        }

        private void dgvitem2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.CurrentCell.ColumnIndex == 3)
            {
                int row = dgvItems.CurrentCell.RowIndex;

                dgvItems.Rows[row].Cells[1].Value = dgvitem2.CurrentRow.Cells[0].Value;
                dgvItems.Rows[row].Cells[2].Value = dgvitem2.CurrentRow.Cells[1].Value;
                dgvItems.Rows[row].Cells[4].Value = dgvitem2.CurrentRow.Cells[3].Value;
                dgvItems.Rows[row].Cells[5].Value = 1;
                dgvItems.CurrentCell.Value = dgvitem2.CurrentRow.Cells[2].Value;

                dgvitem2.Visible = false;

                if (Convert.ToString(dgvItems.Rows[row].Cells[1].Value) == string.Empty)
                {
                    dgvItems.Rows[row].Cells[1].Value = 0;

                }
                grandtotal();

            }



        }
        TextBox editingTextBox;

        private void dgvItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            if (editingTextBox != null)
                editingTextBox.TextChanged -= EditingTextBox_TextChanged;

            // Attach new handler
            editingTextBox = e.Control as TextBox;
            if (editingTextBox != null)
                editingTextBox.TextChanged += EditingTextBox_TextChanged;

            if (dgvItems.CurrentCell.ColumnIndex != 3)
            {
                dgvitem2.Visible = false;
            }

            if (dgvItems.CurrentCell.ColumnIndex == 3)
            {
                dgvitem2.Visible = true;
                if (e.Control is TextBox)
                {
                    TextBox txtbox = (TextBox)e.Control;
                    txtbox.TextChanged -= txtbox_TextChanged;
                    txtbox.TextChanged += txtbox_TextChanged;

                }
            }

            if (dgvItems.CurrentCell.ColumnIndex == 5)
            {
                int row = dgvItems.CurrentCell.RowIndex;
                double Price, qty = 0;
                double.TryParse(Convert.ToString(dgvItems.Rows[row].Cells[4].Value), out Price);
                double.TryParse(Convert.ToString(dgvItems.Rows[row].Cells[5].Value), out qty);
                dgvItems.Rows[row].Cells[6].Value = qty * Price;
                grandtotal();


            }
            grandtotal();

        }

        private void txtbox_TextChanged(object sender, EventArgs e)
        {
            {
                if (dgvItems.CurrentCell == null)
                    return;

                int colindex = dgvItems.CurrentCell.ColumnIndex;
                TextBox txtbox = (TextBox)sender;
                string content = txtbox.Text;
                if (colindex == 3)
                {
                    dgvitem2.Visible = true;

                }

                Rectangle cellRect = dgvItems.GetCellDisplayRectangle(dgvItems.CurrentCell.ColumnIndex,
                    dgvItems.CurrentCell.RowIndex, false);

                int centerX = cellRect.Left + 30;
                int centerY = cellRect.Top + 60;
                dgvitem2.Location = new Point(centerX, centerY);

                dgvitem2.CellClick -= dgvitem2_CellClick;
                dgvitem2.CellClick += dgvitem2_CellClick;
                grandtotal();
            }
        }

        private void dgvItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string serialNumber = (e.RowIndex + 1).ToString();
            var grid = sender as DataGridView;
            using (SolidBrush brush = new SolidBrush(grid.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString(serialNumber,
                                      grid.DefaultCellStyle.Font,
                                      brush,
                                      e.RowBounds.Location.X + 15,
                                      e.RowBounds.Location.Y + 4);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtname_TextChanged(object sender, EventArgs e)
        {
            getdatasearchname(txtname.Text);
        }

        private void EditingTextBox_TextChanged(object sender, EventArgs e)
        {
            if (dgvItems.CurrentCell != null && dgvItems.CurrentCell.ColumnIndex >= 3)
            {
                var liveText = ((TextBox)sender).Text;
                getdatasearchname(liveText);  // Your function call
            }
        }

        private void txtamount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar)) return;

            // Allow digits and decimal point
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Prevent more than one decimal point
            if (e.KeyChar == '.' && txtamount.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private bool EnsureValidItemSelected()
        {
            var selectedValue = cmbProduct.SelectedValue;

            bool itemNotFound = false;

            if (selectedValue == null)
            {
                itemNotFound = true;
            }
            else
            {
                object result = DBClass.ExecuteScalar(
                    @"SELECT COUNT(1) FROM tbl_items WHERE id = @id",
                    DBClass.CreateParameter("id", selectedValue)
                );

                int recordCount = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                if (recordCount <= 0)
                {
                    itemNotFound = true;
                }
            }

            if (itemNotFound)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Are you sure you want to create?",
                    "Item not found",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (dialogResult == DialogResult.Yes)
                {
                    new frmViewItem().ShowDialog();
                    LoadCombo();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true; // Item is valid and exists
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvItems.Rows.Count == 0 || dgvItems.Rows.Count == 1 && dgvItems.Rows[0].IsNewRow)
            {
                MessageBox.Show("Can't Hold Empty Order");
                return;
            }
            if (checkRequiredData())
            {
                checkQtyAvailable();
            }

            if (isSaved)
            {
                if(id>0)
                    MessageBox.Show("Work Order Updated");
                else
                    MessageBox.Show("Work Order Saved");
            }
        }
        private bool checkRequiredData()
        {
            if (cmbProduct.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Product", "Warning");
                cmbProduct.Focus();
                return false;
            }
            if (cmbWarehouse.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Warehouse", "Warning");
                cmbWarehouse.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtname.Text))
            {
                MessageBox.Show("Please Enter Batch Name", "Warning");
                txtname.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(NoDropQty.Text) || NoDropQty.Text == "0")
            {
                MessageBox.Show("Please Enter Qty", "Warning");
                NoDropQty.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txthourse.Text))
            {
                MessageBox.Show("Please Enter Hours", "Warning");
                txthourse.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtamount.Text) || txtamount.Text == "0")
            {
                MessageBox.Show("Please Enter Amount", "Warning");
                txtamount.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(lbtotal.Text) || lbtotal.Text == "0")
            {
                MessageBox.Show("Please Check Total Amount", "Warning");
                lbtotal.Focus();
                return false;
            }
            if (cbMachin.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Machine", "Warning");
                cbMachin.Focus();
                return false;
            }
            return true;
        }
        bool isSaved = false;
        private void checkQtyAvailable()
        {
            bool allItemsAvailable = true;

            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                int itemId = Convert.ToInt32(dgvItems.Rows[i].Cells["dgvid"].Value);
                decimal qty = dgvItems.Rows[i].Cells["Dgvqty"].Value == DBNull.Value ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["Dgvqty"].Value);

                if (!checkItemAvailability(itemId, qty))
                {
                    allItemsAvailable = false;
                    break;
                }
            }

            if (allItemsAvailable)
            {
                Save_Action();
                isSaved = true;
            }
            else
            {
                DialogResult result = MessageBox.Show("No Qty Available for some Item! Do you want to proceed?",
                                                      "Warning",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (AllowItemWithOutQty)
                    {
                        Save_Action();
                        isSaved = true;
                    }
                    else
                    {
                        isSaved = false;
                    }
                }
            }
        }
        bool checkItemAvailability(int itemId, decimal salesQty)
        {
            decimal totalQtyInGrid = 0;

            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT * FROM tbl_items WHERE id = @id",
                DBClass.CreateParameter("id", itemId)))
            {
                if (reader.Read())
                {
                    string itemType = reader["type"].ToString();

                    if (itemType == "12 - Service")
                        return true;

                    if (itemType == "11 - Inventory Part")
                    {
                        totalQtyInGrid = GetTotalQtyInGrid(itemId);

                        decimal onHand = decimal.Parse(reader["on_hand"].ToString());
                        if (totalQtyInGrid > onHand)
                        {
                            return false;
                        }
                    }
                    else if (itemType == "13 - Inventory Assembly")
                    {
                        using (MySqlDataReader assemblyReader = DBClass.ExecuteReader(@"
                            SELECT   a.qty, i.on_hand, i.name
                            FROM tbl_item_assembly a
                            JOIN tbl_items i ON a.item_id = i.id
                            WHERE a.assembly_id = @assemblyId",
                            DBClass.CreateParameter("@assemblyId", itemId)))
                        {
                            while (assemblyReader.Read())
                            {
                                string ComponentName = assemblyReader["name"].ToString();
                                decimal requiredPerUnit = Convert.ToDecimal(assemblyReader["qty"]);
                                decimal availableQty = Convert.ToDecimal(assemblyReader["on_hand"]);
                                decimal totalRequiredQty = GetTotalQtyInGrid(itemId) * requiredPerUnit;
                                if (totalRequiredQty > availableQty)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }
        decimal GetTotalQtyInGrid(int itemId)
        {
            decimal totalQty = 0;

            for (int i = 0; i < dgvItems.Rows.Count; i++)
                if (dgvItems.Rows[i].Cells["dgvid"].Value != null &&
                    dgvItems.Rows[i].Cells["dgvid"].Value.ToString() == itemId.ToString())
                    if (dgvItems.Rows[i].Cells["Dgvqty"].Value != null &&
                        dgvItems.Rows[i].Cells["Dgvqty"].Value.ToString() != "")
                        totalQty += decimal.Parse(dgvItems.Rows[i].Cells["Dgvqty"].Value.ToString());

            return totalQty;
        }

        private void Save_Action()
        {
            decimal totalAmount = Convert.ToDecimal(txtamount.Text.ToString());
            decimal totalCost = Convert.ToDecimal(lbtotal.Text.ToString());

            if (!EnsureValidItemSelected())
                return;

            string qry1 = ""; //main table

            if (id == 0)
            {
                qry1 = @"INSERT INTO tbl_manufacturer_batch (product_id,warehouse_id,product_qty,batchname,Costamount,amount,hours,userinsert,date,Description,fixedassetsID,fixedStatus) VALUES (@productId,@warehouseId,@productQty,@batchname,@Costamount,@amount,@hours,@userinsert,@date,@Description,@fixedassetsID,@fixedStatus); 
                       SELECT LAST_INSERT_ID();";
            }
            else //update
            {
                qry1 = @"update  tbl_manufacturer_batch set product_id =@productId , warehouse_id=@warehouseId, product_qty = @productQty, batchname= @batchname,Costamount= @Costamount,amount= @amount,hours= @hours,userinsert= @userinsert,date= @date,Description=@Description,fixedassetsID=@fixedassetsID,fixedStatus=@fixedStatus  where id =@ID";
            }
            if (id == 0)
            {
                id = Convert.ToInt32(DBClass.ExecuteScalar(qry1,
                DBClass.CreateParameter("@batchname", txtname.Text),
                DBClass.CreateParameter("@productId", cmbProduct.SelectedValue),
                DBClass.CreateParameter("@warehouseId", cmbWarehouse.SelectedValue),
                DBClass.CreateParameter("@Costamount", totalCost),
                DBClass.CreateParameter("@amount", totalAmount),
                DBClass.CreateParameter("@hours", txthourse.Text),
                DBClass.CreateParameter("@productQty", NoDropQty.Text),
                DBClass.CreateParameter("@userinsert", frmLogin.userId.ToString()),
                DBClass.CreateParameter("@date", dtInv.Value),
                DBClass.CreateParameter("@Description", txtdiscription.Text),
                DBClass.CreateParameter("@fixedassetsID", cbMachin.SelectedValue),
                DBClass.CreateParameter("@fixedStatus", "Draft")));

                Utilities.LogAudit(frmLogin.userId, "Add Manufacturing Batch", "Manufacturing", id, "Added Manufacturing Batch: " + txtname.Text);
            }
            else
            {
                DBClass.ExecuteNonQuery(qry1,
                DBClass.CreateParameter("@id", id),
                DBClass.CreateParameter("@batchname", txtname.Text),
                DBClass.CreateParameter("@productId", cmbProduct.SelectedValue),
                DBClass.CreateParameter("@warehouseId", cmbWarehouse.SelectedValue),
                DBClass.CreateParameter("@Costamount", totalCost),
                DBClass.CreateParameter("@amount", totalAmount),
                DBClass.CreateParameter("@hours", txthourse.Text),
                DBClass.CreateParameter("@productQty", NoDropQty.Text),
                DBClass.CreateParameter("@userinsert", frmLogin.userId.ToString()),
                DBClass.CreateParameter("@date", dtInv.Value),
                DBClass.CreateParameter("@Description", txtdiscription.Text),
                DBClass.CreateParameter("@fixedassetsID", cbMachin.SelectedValue),
                DBClass.CreateParameter("@fixedStatus", "Draft"));

                DBClass.ExecuteNonQuery("Delete from tbl_manufacturer_batchdetails where batchId = @id",
                    DBClass.CreateParameter("id", id));
                DBClass.ExecuteNonQuery(@"DELETE FROM tbl_item_transaction WHERE reference = @invId AND type = 'Manufacturing';DELETE FROM tbl_item_card_details 
                            WHERE trans_type = 'Manufacturing' and trans_no=@invId", DBClass.CreateParameter("invId", id));

                Utilities.LogAudit(frmLogin.userId, "Update Manufacturing Batch", "Manufacturing", id, "Updated Manufacturing Batch: " + txtname.Text);

            }

            decimal totalCostAllItems = 0;
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.IsNewRow)
                    continue;
                var cellValue = row.Cells[3].Value;
                if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                    continue;

                //using (MySqlDataReader itemReader000 = DBClass.ExecuteReader("select * from tbl_items where id = @id", DBClass.CreateParameter("id", Convert.ToInt32(row.Cells["dgvid"].Value))))
                //{
                    string qry2 = @"INSERT INTO  tbl_manufacturer_batchdetails  (batchID,itemid,cost,qty,Total) VALUES (@batchID,@itemid,@cost,@qty,@Total)";

                    DBClass.ExecuteNonQuery(qry2,
                    DBClass.CreateParameter("@batchID", id),
                    DBClass.CreateParameter("@itemid", Convert.ToInt32(row.Cells["dgvid"].Value)),
                    DBClass.CreateParameter("@cost", Convert.ToDouble(row.Cells["dgvCost"].Value)),
                    DBClass.CreateParameter("@qty", Convert.ToInt32(row.Cells["Dgvqty"].Value)),
                    DBClass.CreateParameter("@Total", Convert.ToDouble(row.Cells["DGVTOTATL"].Value)));

                    //if (itemReader.Read())
                    //{
                    //    string itemType = Convert.ToString(itemReader["type"]);
                    //    string method = Convert.ToString(itemReader["method"]);
                    //    decimal qty = Convert.ToDecimal(row.Cells["Dgvqty"].Value);
                    //    int itemId = Convert.ToInt32(row.Cells["dgvid"].Value);
                    //    decimal costAmount = 0;

                    //    if (itemType == "12 - Service") continue;

                    //    if (itemType == "13 - Inventory Assembly")
                    //    {
                    //        using (MySqlDataReader componentReader = DBClass.ExecuteReader(@"
                    //                SELECT itemid, qty,(select method FROM tbl_items WHERE tbl_items.id = tbl_manufacturer_batchdetails.itemid) as method 
                    //                FROM tbl_manufacturer_batchdetails WHERE batchid = @assemblyId",
                    //            DBClass.CreateParameter("assemblyId", itemId)))
                    //        {
                    //            while (componentReader.Read())
                    //            {
                    //                int componentId = Convert.ToInt32(componentReader["item_id"]);
                    //                decimal componentQty = Convert.ToDecimal(componentReader["qty"]) * qty;
                    //                string methodOfIngrediant = componentReader["method"].ToString().Trim();

                    //                DBClass.ExecuteNonQuery(@"
                    //                    UPDATE tbl_items 
                    //                    SET on_hand = on_hand - @qty 
                    //                    WHERE id = @componentId",
                    //                    DBClass.CreateParameter("qty", componentQty),
                    //                    DBClass.CreateParameter("componentId", componentId));

                    //                decimal costReturned = InsertItemTransaction(row, componentId, componentQty, methodOfIngrediant);
                    //                costAmount = costReturned;
                    //                totalCostAllItems += costReturned;
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT * FROM tbl_items WHERE id=@id",
                    //       DBClass.CreateParameter("id", itemId)))
                    //        {
                    //            if (reader.Read())
                    //            {
                    //                decimal onHand = Convert.ToDecimal(reader["on_hand"]);
                    //                DBClass.ExecuteNonQuery("UPDATE tbl_items SET on_hand = @newQty WHERE id = @id",
                    //                    DBClass.CreateParameter("newQty", onHand - qty),
                    //                    DBClass.CreateParameter("id", itemId));

                    //                decimal costReturned = InsertItemTransaction(row, itemId, qty, method);
                    //                costAmount = costReturned;
                    //                totalCostAllItems += costReturned;
                    //            }
                    //        }
                    //    }
                    //}
                //}
            }
        }
        
        private decimal InsertItemTransactionOldWay(DataGridViewRow row, int itemId, decimal qty, string method)
        {
            string invId = id.ToString();
            if (AllowItemWithOutQty)
            {
                // Allow selling item even without available stock (negative stock allowed)
                decimal cost_price = 0;
                decimal totalCost = 0;

                // Try to get a recent cost price (optional, to assign value for accounting)
                object result = DBClass.ExecuteScalar(@"SELECT cost_price FROM tbl_item_transaction 
                                            WHERE item_id = @id AND date <= @date 
                                            ORDER BY date DESC LIMIT 1",
                                DBClass.CreateParameter("id", itemId), DBClass.CreateParameter("date", dtInv.Value));

                if (result != null && result != DBNull.Value)
                {
                    cost_price = Convert.ToDecimal(result);
                }

                totalCost = cost_price * qty;

                CommonInsert.InsertItemTransaction(dtInv.Value, "Manufacturing", invId.ToString(), itemId.ToString(),
                    cost_price.ToString(), "0", row.Cells["dgvCost"].Value.ToString(), qty.ToString(), "0",
                    "Manufacturing (Negative Stock)", cmbWarehouse.SelectedValue.ToString());

                return totalCost;
            }
            else
            {
                if (itemId <= 0 || qty <= 0)
                {
                    MessageBox.Show("Invalid Item ID or Quantity.");
                    return 0;
                }

                decimal cost_price = 0;
                decimal totalCost = 0;
                MySqlDataReader reader = null;

                if (method == "fifo" || method == "lifo")
                {
                    string orderBy = method == "fifo" ? "ASC" : "DESC";
                    decimal remainingQty = qty;

                    reader = DBClass.ExecuteReader(@"
                        SELECT * FROM tbl_item_transaction 
                        WHERE date <= @date AND qty_inc > 0 AND item_id = @id 
                        ORDER BY id " + orderBy,
                        DBClass.CreateParameter("id", itemId), DBClass.CreateParameter("date", dtInv.Value));

                    while (reader.Read() && remainingQty > 0)
                    {
                        decimal availableQty = Convert.ToDecimal(reader["qty_inc"]);
                        cost_price = Convert.ToDecimal(reader["cost_price"]);
                        decimal qtyToUse = Math.Min(remainingQty, availableQty);

                        remainingQty -= qtyToUse;
                        totalCost += cost_price * qtyToUse;

                        CommonInsert.InsertItemTransaction(dtInv.Value, "Manufacturing", invId.ToString(), itemId.ToString(),
                            cost_price.ToString(), "0", row.Cells["dgvCost"].Value.ToString(), qtyToUse.ToString(), "0",
                            "Manufacturing . ", cmbWarehouse.SelectedValue.ToString());

                        DBClass.ExecuteNonQuery("UPDATE tbl_item_transaction SET qty_inc = qty_inc - @qty WHERE id = @id",
                            DBClass.CreateParameter("qty", qtyToUse),
                            DBClass.CreateParameter("id", reader["id"].ToString()));
                    }

                    // If some quantity remains unprocessed (not enough stock), allow negative stock handling
                    if (remainingQty > 0)
                    {
                        // Use last known or default cost
                        if (cost_price <= 0)
                        {
                            object fallbackCost = DBClass.ExecuteScalar(@"SELECT cost_price FROM tbl_item_transaction 
                                                              WHERE item_id = @id AND date <= @date 
                                                              ORDER BY date DESC LIMIT 1",
                                        DBClass.CreateParameter("id", itemId), DBClass.CreateParameter("date", dtInv.Value));
                            if (fallbackCost != null && fallbackCost != DBNull.Value)
                                cost_price = Convert.ToDecimal(fallbackCost);
                        }

                        totalCost += cost_price * remainingQty;

                        CommonInsert.InsertItemTransaction(dtInv.Value, "Manufacturing", invId.ToString(), itemId.ToString(),
                            cost_price.ToString(), "0", row.Cells["dgvCost"].Value.ToString(), remainingQty.ToString(), "0",
                            "Manufacturing No. " + " (Neg. Stock)", cmbWarehouse.SelectedValue.ToString());
                    }
                }
                else
                {
                    object result = DBClass.ExecuteScalar(@"SELECT 
                            CASE 
                                WHEN SUM(qty_in - qty_out) = 0 THEN 0
                                ELSE SUM((qty_in - qty_out) * cost_price) / SUM(qty_in - qty_out)
                            END AS cost_price 
                        FROM 
                            tbl_item_transaction 
                        WHERE item_id = @id AND date <= @date",
                                DBClass.CreateParameter("id", itemId), DBClass.CreateParameter("date", dtInv.Value));

                    decimal recordcost_price = (result != null && result != DBNull.Value) ? Convert.ToDecimal(result) : 0;

                    if (recordcost_price > 0)
                    {
                        cost_price = recordcost_price;
                    }

                    CommonInsert.InsertItemTransaction(dtInv.Value, "Manufacturing", invId.ToString(), itemId.ToString(),
                        cost_price.ToString(), "0", row.Cells["dgvCost"].Value.ToString(), qty.ToString(), "0",
                        "Manufacturing No. ", cmbWarehouse.SelectedValue.ToString());

                    using (MySqlDataReader dr = DBClass.ExecuteReader(@"SELECT (balance / qty_balance) cost FROM tbl_item_card_details 
                                WHERE DATE <= @date AND trans_type = 'Purchase Invoice' AND itemId = @itemId
                                ORDER BY trans_no DESC LIMIT 1",
                            DBClass.CreateParameter("date", dtInv.Value),
                            DBClass.CreateParameter("itemId", itemId.ToString())))
                    {
                        if (dr.Read())
                        {
                            totalCost = decimal.Parse(dr["cost"].ToString()) * qty;
                        }
                    }
                }

                return totalCost;
            }
        }

        private void addTransactions(int batchmainid)
        {
            using (MySqlDataReader readItem = DBClass.ExecuteReader(@"Select max(id) as id from  tbl_manufacturer_batch "))
            {
                readItem.Read();
                batchmainid = Convert.ToInt32(readItem["id"].ToString());
            }
        }

        public void GetDataforupdatedetails()
        {
            int sn = 0;
            sn = sn + 1;
            string qry = @" SELECT  B.itemId,C.code, C.name,B.cost,B.qty,B.Total
                           FROM `tbl_manufacturer_batchdetails` B
                           INNER JOIN tbl_items C ON C.id = B.itemid where   B.batchID = '" + id + "'";
            ListBox lb = new ListBox();
            lb.Items.Add(dgvid);
            lb.Items.Add(Procode);
            lb.Items.Add(dgvproducts);
            lb.Items.Add(dgvCost);
            lb.Items.Add(Dgvqty);
            lb.Items.Add(DGVTOTATL);
            RMSClass.loadData(qry, dgvItems, lb);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            GetDataforupdatedetails();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedIndex == -1) return;

            EnsureValidItemSelected();
        }

        private void cmbProduct_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cmbProduct.Text))
            {
                EnsureValidItemSelected();
            }
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}


