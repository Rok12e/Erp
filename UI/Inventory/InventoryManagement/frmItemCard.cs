using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject.UI.Inventory.InventoryManagement
{
    public partial class frmItemCard : Form
    {
        string itemCode = "", itemName = "";
        int itemsId = 0;
        public frmItemCard(string _code, string _name, int _itemsId = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            itemCode = _code;
            itemName = _name;
            itemsId = _itemsId;
            dtDate.Value = DateTime.Now;
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
        List<decimal> costList = new List<decimal>();
        List<decimal> qtyList = new List<decimal>();
        private void frmItemCard_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(itemCode))
            {
                lblCode.Text = itemCode;
            }
            if (!string.IsNullOrEmpty(itemName))
            {
                lblName.Text = itemName;
            }
            using (MySqlDataReader itemReader = DBClass.ExecuteReader(@"SELECT on_hand,method FROM tbl_items WHERE id = @itemId", DBClass.CreateParameter("itemId", itemsId)))
            {
                if (itemReader.Read())
                {
                    lblMethod.Text = itemReader["method"].ToString().Trim();
                    decimal currentCostPrice = 0;
                    lblQty.Text = itemReader["on_hand"].ToString();
                    using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT 
                            ROW_NUMBER() OVER (ORDER BY date, id) AS SN,id, itemId, date, wharehouse_id, inv_no, trans_no, trans_type, description,
                            price, qty_in, qty_out, qty_balance, debit, credit, balance, fifo_qty, fifo_cost
                            FROM tbl_item_card_details WHERE itemId = @itemId ORDER BY date, id",
                            DBClass.CreateParameter("itemId", itemsId)))
                    {
                        bool fifoSet = false;

                        List<decimal> fifoPrices = new List<decimal>();
                        List<decimal> fifoQtys = new List<decimal>();
                        decimal lastPrice = 0;
                        string type = lblMethod.Text.ToUpper();

                        while (reader.Read())
                        {
                            object netResult = DBClass.ExecuteScalar(
                                "SELECT name FROM tbl_warehouse WHERE id = @id",
                                DBClass.CreateParameter("id", reader["wharehouse_id"].ToString())
                            );
                            string wharehouseName = (netResult != null && netResult != DBNull.Value) ? netResult.ToString() : "";

                            decimal qtyIn = Convert.ToDecimal(reader["qty_in"]);
                            decimal price = Convert.ToDecimal(reader["price"]);

                            // Collect for AVG or LIFO
                            if (qtyIn > 0)
                            {
                                fifoPrices.Add(price);
                                fifoQtys.Add(qtyIn);
                                lastPrice = price;

                                if (type == "FIFO" && !fifoSet)
                                {
                                    currentCostPrice = price;
                                    fifoSet = true;
                                }
                            }

                            dgvItems.Rows.Add(
                                reader["SN"].ToString(),
                                Convert.ToDateTime(reader["date"]).ToString("dd-MM-yyyy"),
                                wharehouseName,
                                reader["inv_no"].ToString(),
                                reader["trans_no"].ToString(),
                                reader["trans_type"].ToString(),
                                reader["description"].ToString(),
                                Convert.ToDecimal(reader["price"]).ToString("N2"),
                                Convert.ToDecimal(reader["qty_in"]).ToString("N2"),
                                Convert.ToDecimal(reader["qty_out"]).ToString("N2"),
                                Convert.ToDecimal(reader["qty_balance"]).ToString("N2"),
                                " | ",
                                Convert.ToDecimal(reader["debit"]).ToString("N2"),
                                Convert.ToDecimal(reader["credit"]).ToString("N2"),
                                Convert.ToDecimal(reader["balance"]).ToString("N2"),
                                Convert.ToDecimal(reader["fifo_qty"]).ToString("N2"),
                                Convert.ToDecimal(reader["fifo_cost"]).ToString("N2")
                            );

                            qtyList.Add(Convert.ToDecimal(reader["qty_balance"]));
                            if (type == "AVG")
                            {
                                decimal qtyBalance = reader["qty_balance"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["qty_balance"]);
                                decimal balance = reader["balance"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["balance"]);

                                if (qtyBalance != 0)
                                {
                                    costList.Add(Convert.ToDecimal(reader["balance"]) / Convert.ToDecimal(reader["qty_balance"]));
                                }
                                else
                                {
                                    costList.Add(0);
                                }
                            }
                            else
                            {
                                costList.Add(Convert.ToDecimal(reader["price"]));
                            }
                        }

                        if (type == "LIFO" && fifoPrices.Count > 0)
                        {
                            currentCostPrice = lastPrice;
                        }
                        else if (type == "AVG" && fifoPrices.Count > 0)
                        {
                            decimal totalCost = 0;
                            decimal totalQty = 0;

                            for (int i = 0; i < fifoPrices.Count; i++)
                            {
                                totalCost += fifoPrices[i] * fifoQtys[i];
                                totalQty += fifoQtys[i];
                            }

                            currentCostPrice = totalQty > 0 ? totalCost / totalQty : 0;
                        }
                    }

                    if (currentCostPrice > 0)
                    {
                        lblCost.Text = currentCostPrice.ToString("N2");
                    }

                    dgvItems.Columns["price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItems.Columns["qty_in"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItems.Columns["qty_out"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItems.Columns["qty_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItems.Columns["debit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItems.Columns["credit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItems.Columns["balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItems.Columns["FifoQty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItems.Columns["fifoCost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    loadDataForView();
                }
            }
        }

        private void loadDataForView()
        {
            guna2DataGridView1.DataSource = CostListToHorizontalTable(costList);
            guna2DataGridView1.ColumnHeadersVisible = false;
            guna2DataGridView2.DataSource = CostListToHorizontalTable(qtyList);
            guna2DataGridView2.ColumnHeadersVisible = false;

            formattColumn(guna2DataGridView1.Columns);
            formattColumn(guna2DataGridView2.Columns);

            try
            {
                lblCost.Text = costList.Last().ToString("N2");
                lblQty.Text = qtyList.Last().ToString("N2");
            }catch(Exception ex)
            {
                ex.ToString();
            }
        }
        private void formattColumn(DataGridViewColumnCollection columns)
        {
            foreach (DataGridViewColumn col in columns)
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.DefaultCellStyle.Format = "N2";
            }
        }
        public DataTable CostListToHorizontalTable(List<decimal> itemList)
        {
            DataTable dt = new DataTable();
            DataRow row = dt.NewRow();

            for (int i = 0; i < itemList.Count; i++)
            {
                string columnName = "Col" + (i + 1);
                dt.Columns.Add(columnName);
                row[i] = itemList[i].ToString("N2");
            }

            dt.Rows.Add(row);
            return dt;
        }
    }
}
