using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace YamyProject.UI.Manufacturing.Viewform
{
    public partial class frmManWorkOrderDetails : Form
    {
        bool AllowItemWithOutQty = true;
        int workOrderId = 0, warehouseId = 0;

        public frmManWorkOrderDetails(int _wId)
        {
            InitializeComponent();
            this.workOrderId = _wId;
        }

        string status = "";

        public void GetMaterialDetails(int id)
        {
            int sn = 0;
            sn = sn + 1;
            string qry = @" SELECT  B.id, C.id itemId,C.code, C.name,B.cost,B.qty,B.Total,B.RequestQty,B.ReceiveQty
                           FROM `tbl_manufacturer_batchdetails` B
                           INNER JOIN tbl_items C ON C.id = B.itemid where   B.batchID = '" + id + "'";
            using(MySqlDataReader reader = DBClass.ExecuteReader(qry))
            {
                dgvItems.Rows.Clear();
                while (reader.Read())
                {
                   int rowIndex = dgvItems.Rows.Add(sn, reader["id"], reader["itemId"], reader["code"], reader["name"],
                                    reader["qty"], reader["RequestQty"], reader["ReceiveQty"], reader["cost"], reader["Total"]);

                    sn++;
                }
            }
        }

        private void frmManWorkOrderDetails_Load(object sender, EventArgs e)
        {
            var generalS = Utilities.GeneralSettingsState("ALLOW ITEM WITHOUT QTY");
            if (!string.IsNullOrEmpty(generalS) & int.Parse(generalS) > 0)
            {
                AllowItemWithOutQty = true;
            }
            else
            {
                AllowItemWithOutQty = false;
            }


            dgvItems.Rows.Clear();
            if (workOrderId > 0)
            {
                string qry = @" SELECT a.id, a.batchname, a.hours, a.amount, a.Description, IFNULL(b.status,'Pending') status,a.warehouse_id FROM tbl_manufacturer_batch a LEFT JOIN (
                                SELECT t.batchId,t.status FROM tbl_manufacturer_task t WHERE t.id IN (SELECT MAX(id) FROM tbl_manufacturer_task GROUP BY BatchID)
                            ) b ON b.BatchID = a.id WHERE a.id = '" + workOrderId + "'";

                using (MySqlDataReader reader = DBClass.ExecuteReader(qry))
                {
                    if (reader.Read())
                    {
                        status = reader["status"].ToString();
                        lblBatchName.Text = reader["batchname"].ToString();
                        lblStatus.Text = status;
                        warehouseId = Convert.ToInt32(reader["warehouse_id"]);
                        GetMaterialDetails(workOrderId);
                    }
                }
            }
            
        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {

            if (status == "Done" || status == "Progress")
            {
                MessageBox.Show("Cant update this work already " + status);
            }
            else
            {
                bool isValid = true;
                decimal _receiveQty = 0, _requestQty = 0, _qty = 0;
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    if (row != null && row.Cells["dgvqty"].Value != DBNull.Value)
                    {
                        decimal.TryParse(row.Cells["dgvqty"].Value?.ToString(), out _qty);
                    }
                    if (row != null && row.Cells["dgvReceiveQty"].Value != DBNull.Value)
                    {
                        decimal.TryParse(row.Cells["dgvReceiveQty"].Value?.ToString(), out _receiveQty);
                    }
                    if (row != null && row.Cells["dgvRequestQty"].Value != DBNull.Value)
                    {
                        decimal.TryParse(row.Cells["dgvRequestQty"].Value?.ToString(), out _requestQty);
                    }
                    if (_receiveQty > _requestQty)
                    {
                        MessageBox.Show(
                            "Receive Qty is greater than Request Qty",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );

                        isValid = false;
                        break;
                    }

                    if (_receiveQty > _qty || _requestQty > _qty)
                    {
                        MessageBox.Show(
                            "Receive Qty or Request Qty is greater than Qty",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );

                        isValid = false;
                        break;
                    }
                }
                if (isValid)
                {
                    foreach (DataGridViewRow row in dgvItems.Rows)
                    {
                        int id = Convert.ToInt32(row.Cells["dgvid"].Value);
                        int itemId = Convert.ToInt32(row.Cells["dgvItemId"].Value);
                        _qty = Convert.ToInt32(row.Cells["Dgvqty"].Value);
                        _requestQty = Convert.ToInt32(row.Cells["dgvRequestQty"].Value);
                        _receiveQty = Convert.ToInt32(row.Cells["dgvReceiveQty"].Value);

                        DBClass.ExecuteNonQuery("UPDATE tbl_manufacturer_batchdetails set RequestQty = @requestQty,ReceiveQty = @receiveQty where id = @id",
                            DBClass.CreateParameter("id", id),
                            DBClass.CreateParameter("requestQty", _requestQty),
                            DBClass.CreateParameter("receiveQty", _receiveQty));
                        using (MySqlDataReader itemReader = DBClass.ExecuteReader("select * from tbl_items where id = @id", DBClass.CreateParameter("id", Convert.ToInt32(itemId))))
                        {
                            if (_receiveQty > 0)
                            {
                                if (itemReader.Read())
                                {
                                    string itemType = Convert.ToString(itemReader["type"]);
                                    string method = Convert.ToString(itemReader["method"]);
                                    string salesPrice = Convert.ToString(itemReader["sales_price"]);
                                    decimal qty = _receiveQty;

                                    decimal costAmount = 0;

                                    if (itemType == "12 - Service")
                                    {
                                        //
                                    }
                                    else if (itemType == "13 - Inventory Assembly")
                                    {
                                        using (MySqlDataReader componentReader = DBClass.ExecuteReader(@"
                                        SELECT itemid, qty,(select method FROM tbl_items WHERE tbl_items.id = tbl_manufacturer_batchdetails.itemid) as method 
                                        FROM tbl_manufacturer_batchdetails WHERE batchid = @assemblyId",
                                            DBClass.CreateParameter("assemblyId", itemId)))
                                        {
                                            while (componentReader.Read())
                                            {
                                                int componentId = Convert.ToInt32(componentReader["item_id"]);
                                                decimal componentQty = Convert.ToDecimal(componentReader["qty"]) * qty;
                                                string methodOfIngrediant = componentReader["method"].ToString().Trim();

                                                DBClass.ExecuteNonQuery(@"
                                            UPDATE tbl_items 
                                            SET on_hand = on_hand - @qty 
                                            WHERE id = @componentId",
                                                    DBClass.CreateParameter("qty", componentQty),
                                                    DBClass.CreateParameter("componentId", componentId));

                                                decimal costReturned = InsertItemTransaction(componentId, componentQty, methodOfIngrediant, salesPrice);
                                                costAmount = costReturned;
                                                //totalCostAllItems += costReturned;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT * FROM tbl_items WHERE id=@id",
                                       DBClass.CreateParameter("id", itemId)))
                                        {
                                            if (reader.Read())
                                            {
                                                decimal onHand = Convert.ToDecimal(reader["on_hand"]);
                                                DBClass.ExecuteNonQuery("UPDATE tbl_items SET on_hand = @newQty WHERE id = @id",
                                                    DBClass.CreateParameter("newQty", onHand - qty),
                                                    DBClass.CreateParameter("id", itemId));

                                                decimal costReturned = InsertItemTransaction(itemId, qty, method, salesPrice);
                                                costAmount = costReturned;
                                                //totalCostAllItems += costReturned;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Utilities.LogAudit(frmLogin.userId, "Update Work Order", "Work Order", workOrderId, "Updated Work Order: " + lblBatchName.Text);

                    MessageBox.Show(
                        "Update Successfully",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
        }

        private decimal InsertItemTransaction(int itemId, decimal qty, string method, string sales_price)
        {
            string invId = workOrderId.ToString();
            DateTime dated = DateTime.Now;
            if (AllowItemWithOutQty)
            {
                decimal cost_price = 0;
                decimal totalCost = 0;


                DeleteItemTransaction(invId.ToString(), itemId);

                object result = DBClass.ExecuteScalar(@"SELECT cost_price FROM tbl_item_transaction 
                                            WHERE item_id = @id AND date <= @date 
                                            ORDER BY date DESC LIMIT 1",
                                DBClass.CreateParameter("id", itemId), DBClass.CreateParameter("date", dated.Date));

                if (result != null && result != DBNull.Value)
                {
                    cost_price = Convert.ToDecimal(result);
                }

                totalCost = cost_price * qty;

                CommonInsert.InsertItemTransaction(dated.Date, "Manufacturing", invId.ToString(), itemId.ToString(),
                    cost_price.ToString(), "0", sales_price, qty.ToString(), "0",
                    "Manufacturing (Negative Stock)", warehouseId.ToString());

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
                DeleteItemTransaction(invId.ToString(), itemId);
                MySqlDataReader reader = null;

                if (method == "fifo" || method == "lifo")
                {
                    string orderBy = method == "fifo" ? "ASC" : "DESC";
                    decimal remainingQty = qty;

                    reader = DBClass.ExecuteReader(@"
                        SELECT * FROM tbl_item_transaction 
                        WHERE date <= @date AND qty_inc > 0 AND item_id = @id 
                        ORDER BY id " + orderBy,
                        DBClass.CreateParameter("id", itemId), DBClass.CreateParameter("date", dated.Date));

                    while (reader.Read() && remainingQty > 0)
                    {
                        decimal availableQty = Convert.ToDecimal(reader["qty_inc"]);
                        cost_price = Convert.ToDecimal(reader["cost_price"]);
                        decimal qtyToUse = Math.Min(remainingQty, availableQty);

                        remainingQty -= qtyToUse;
                        totalCost += cost_price * qtyToUse;

                        CommonInsert.InsertItemTransaction(dated.Date, "Manufacturing", invId.ToString(), itemId.ToString(),
                            cost_price.ToString(), "0", sales_price, qtyToUse.ToString(), "0",
                            "Manufacturing . ", warehouseId.ToString());

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
                                        DBClass.CreateParameter("id", itemId), DBClass.CreateParameter("date", dated.Date));
                            if (fallbackCost != null && fallbackCost != DBNull.Value)
                                cost_price = Convert.ToDecimal(fallbackCost);
                        }

                        totalCost += cost_price * remainingQty;

                        CommonInsert.InsertItemTransaction(dated.Date, "Manufacturing", invId.ToString(), itemId.ToString(),
                            cost_price.ToString(), "0", sales_price, remainingQty.ToString(), "0",
                            "Manufacturing No. " + " (Neg. Stock)", warehouseId.ToString());
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
                                DBClass.CreateParameter("id", itemId), DBClass.CreateParameter("date", dated.Date));

                    decimal recordcost_price = (result != null && result != DBNull.Value) ? Convert.ToDecimal(result) : 0;

                    if (recordcost_price > 0)
                    {
                        cost_price = recordcost_price;
                    }

                    CommonInsert.InsertItemTransaction(dated.Date, "Manufacturing", invId.ToString(), itemId.ToString(),
                        cost_price.ToString(), "0", sales_price.ToString(), qty.ToString(), "0",
                        "Manufacturing No. ", warehouseId.ToString());

                    using (MySqlDataReader dr = DBClass.ExecuteReader(@"SELECT (balance / qty_balance) cost FROM tbl_item_card_details 
                                WHERE DATE <= @date AND trans_type = 'Purchase Invoice' AND itemId = @itemId
                                ORDER BY trans_no DESC LIMIT 1",
                            DBClass.CreateParameter("date", dated.Date),
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMaximize_Click(object sender, EventArgs e)
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

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void DeleteItemTransaction(string reference, int itemId)
        {
            DBClass.ExecuteNonQuery("DELETE FROM tbl_item_transaction WHERE `type` = 'Manufacturing' and item_id = @itemId AND `REFERENCE` = @id", DBClass.CreateParameter("id", reference), DBClass.CreateParameter("itemId", itemId));
            DBClass.ExecuteNonQuery("DELETE FROM tbl_item_card_details WHERE `trans_type` = 'Manufacturing' and itemId = @itemId AND `trans_no` = @id", DBClass.CreateParameter("id", reference), DBClass.CreateParameter("itemId", itemId));
            CommonInsert.UpdateOnHandItem(itemId.ToString());
        }
    }

}
