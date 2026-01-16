using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using YamyProject.RMS.Class;
using YamyProject.UI.Manufacturing.Models;


namespace YamyProject.UI.Manufacturing.Viewform
{
    public partial class frmManScreen : Form
    {
        string status;
        public frmManScreen()
        {
            InitializeComponent();
        }

        public void GetOrdersDraft()
        {
            flowLayoutPanel1.Controls.Clear();

            string qry1 = "SELECT * FROM tbl_fixed_assets WHERE manufacture = 1 AND manufactureStatus = 'Draft'";
            MySqlCommand cmd1 = new MySqlCommand(qry1, RMSClass.conn());

            DataTable dt1 = new DataTable();
            new MySqlDataAdapter(cmd1).Fill(dt1);

            foreach (DataRow row in dt1.Rows)
            {
                FlowLayoutPanel P1 = new FlowLayoutPanel
                {
                    AutoSize = true,
                    Width = 230,
                    Height = 350,
                    FlowDirection = FlowDirection.TopDown,
                    Margin = new Padding(10),
                    BackColor = Color.LightGray,
                    BorderStyle = BorderStyle.None
                };

                P1.Paint += (s, e) =>
                {
                    int radius = 5;
                    var path = new System.Drawing.Drawing2D.GraphicsPath();
                    path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
                    path.AddArc(P1.Width - radius * 2 - 1, 0, radius * 2, radius * 2, 270, 90);
                    path.AddArc(P1.Width - radius * 2 - 1, P1.Height - radius * 2 - 1, radius * 2, radius * 2, 0, 90);
                    path.AddArc(0, P1.Height - radius * 2 - 1, radius * 2, radius * 2, 90, 90);
                    path.CloseAllFigures();

                    P1.Region = new Region(path);

                    using (var pen = new Pen(Color.LightGray, 1))
                    {
                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        e.Graphics.DrawPath(pen, path);
                    }
                };

                Panel picPanel = new Panel
                {
                    Width = P1.Width,
                    Height = 110
                };

                PictureBox pic = new PictureBox
                {
                    Image = YamyProject.Properties.Resources.GearsDraft,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(100, 100),
                    Location = new Point((picPanel.Width - 100) / 2, 5)
                };
                picPanel.Controls.Add(pic);
                P1.Controls.Add(picPanel);

                Label lb1 = new Label
                {
                    Text = "Machine Name: " + row["name"].ToString(),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83))))),
                    AutoSize = false,
                    Padding = new Padding(0, 3, 0, 3),
                    Margin = new Padding(0, 10, 0, 0),
                    Width = P1.Width,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                P1.Controls.Add(lb1);

                Panel btnPanel = new Panel
                {
                    Width = P1.Width,
                    Height = 50,
                    Margin = new Padding(0, 10, 0, 10)
                };

                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button
                {
                    AutoRoundedCorners = true,
                    Size = new Size(100, 35),
                    FillColor = Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83))))),
                    Text = "Start Production",
                    Tag = row["id"].ToString()
                };

                b.Location = new Point((btnPanel.Width - b.Width) / 2, (btnPanel.Height - b.Height) / 2);
                b.Click += (sender, e) =>
                {
                    frmManforManagetheBatchTask frm = new frmManforManagetheBatchTask(this);
                    frm.idmach = Convert.ToInt32(row["id"].ToString());
                    frm.nameMachin = (row["name"].ToString());
                    RMSClass.blurbackground3(frm);
                };
                btnPanel.Controls.Add(b);
                P1.Controls.Add(btnPanel);

                flowLayoutPanel1.Controls.Add(P1);
            }
        }

        public void GetOrdersProgress()
    {
        flowLayoutPanel2.Controls.Clear();

        string qry1 = "SELECT * FROM tbl_fixed_assets WHERE manufacture = 1 AND manufactureStatus = 'Progress'";
        MySqlCommand cmd1 = new MySqlCommand(qry1, RMSClass.conn());

        DataTable dt1 = new DataTable();
        new MySqlDataAdapter(cmd1).Fill(dt1);

        foreach (DataRow row in dt1.Rows)
        {
            int machid = Convert.ToInt32(row["id"]);

            FlowLayoutPanel P1 = new FlowLayoutPanel
            {
                AutoSize = true,
                Width = 230,
                Height = 450,
                FlowDirection = FlowDirection.TopDown,
                Margin = new Padding(10),
                BackColor = Color.LightGray,
                BorderStyle = BorderStyle.None
            };

            P1.Paint += (s, e) =>
            {
                int radius = 5;
                var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
                path.AddArc(P1.Width - radius * 2 - 1, 0, radius * 2, radius * 2, 270, 90);
                path.AddArc(P1.Width - radius * 2 - 1, P1.Height - radius * 2 - 1, radius * 2, radius * 2, 0, 90);
                path.AddArc(0, P1.Height - radius * 2 - 1, radius * 2, radius * 2, 90, 90);
                path.CloseAllFigures();

                P1.Region = new Region(path);

                using (var pen = new Pen(Color.LightGray, 1))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(pen, path);
                }
            };

            Panel picPanel = new Panel
            {
                Width = P1.Width,
                Height = 110
            };

            PictureBox pic = new PictureBox
            {
                Image = YamyProject.Properties.Resources._1493__2_,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(100, 100),
                Location = new Point((picPanel.Width - 100) / 2, 5)
            };
            picPanel.Controls.Add(pic);
            P1.Controls.Add(picPanel);

            string qry2 = $@"
            SELECT 
                t.id,
                t.StartTime,
                t.EndTime,
                b.batchname
            FROM 
                tbl_manufacturer_task t
            INNER JOIN 
                tbl_manufacturer_batch b 
            ON 
                t.BatchID = b.id
            WHERE 
                t.MachineID = {machid}
           and 
                t.Status ='Progress'
            LIMIT 1
        ";

            MySqlCommand cmd2 = new MySqlCommand(qry2, RMSClass.conn());
            DataTable dt2 = new DataTable();
            new MySqlDataAdapter(cmd2).Fill(dt2);

            string batchName = "";
            string startTimeStr = "";
            string endTimeStr = "";
                int batchid = 0;

            DateTime startTime = DateTime.MinValue;
            DateTime endTime = DateTime.MinValue;

            if (dt2.Rows.Count > 0)
            {
                var taskRow = dt2.Rows[0];
                batchName = taskRow["batchname"].ToString();
                startTimeStr = taskRow["StartTime"].ToString();
                endTimeStr = taskRow["EndTime"].ToString();
                    batchid = Convert.ToInt32(taskRow["id"].ToString());
                DateTime.TryParse(startTimeStr, out startTime);
                DateTime.TryParse(endTimeStr, out endTime);
            }

            string labelText = $"Machine Name: {row["name"]}";
            if (!string.IsNullOrEmpty(batchName))
            {
                labelText += $"\nBatch: {batchName}\nStart: {startTimeStr}\nEnd: {endTimeStr}";
            }

            Label lb1 = new Label
            {
                Text = labelText,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(252, 147, 69),
                AutoSize = false,
                Padding = new Padding(5),
                Margin = new Padding(0, 10, 0, 0),
                Width = P1.Width,
                Height = 100,
                TextAlign = ContentAlignment.MiddleLeft
            };
            P1.Controls.Add(lb1);

            Label lblCountdown = new Label
            {
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = false,
                Width = P1.Width,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 5, 0, 0)
            };
            P1.Controls.Add(lblCountdown);

            Timer timer = new Timer();
            timer.Interval = 1000; 

            timer.Tick += (s, e) =>
            {
                var now = DateTime.Now;
                if (endTime > now)
                {
                    TimeSpan remaining = endTime - now;
                    lblCountdown.Text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                        remaining.Hours + remaining.Days * 24, 
                        remaining.Minutes,
                        remaining.Seconds);
                }
                else
                {
                    lblCountdown.Text = "Finished";
                    timer.Stop();
                }
            };

            if (startTime != DateTime.MinValue && endTime != DateTime.MinValue)
            {
                timer.Start();
            }
            else
            {
                lblCountdown.Text = "No Timing Info";
            }

            Panel btnPanel = new Panel
            {
                Width = P1.Width,
                Height = 50,
                Margin = new Padding(0, 10, 0, 10)
            };

            Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button
            {
                AutoRoundedCorners = true,
                Size = new Size(100, 35),
                FillColor = Color.Green,
                Text = "Finish",
                Tag = row["id"].ToString(),
                Location = new Point((btnPanel.Width / 2) - 105, (btnPanel.Height - 35) / 2)
            };

            Guna.UI2.WinForms.Guna2Button b2 = new Guna.UI2.WinForms.Guna2Button
            {
                AutoRoundedCorners = true,
                Size = new Size(100, 35),
                FillColor = Color.Red,
                Text = "Cancel",
                Tag = row["id"].ToString(),
                Location = new Point((btnPanel.Width / 2) + 5, (btnPanel.Height - 35) / 2)
            };

                b.Click += (sender, e) =>
                {
                    if ((startTime - endTime).TotalSeconds > 0)
                    {
                        DialogResult result = MessageBox.Show(
                            "Time has not finished yet. Are you sure you want to finish this task?",
                            "Confirm Finish",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );

                        if (result == DialogResult.Yes)
                        {
                            UpdateFinshed(machid);
                            UpdateFinishTask(batchid);
                            GetOrdersDraft();
                            GetOrdersProgress();
                        }
                    }
                    else if ((startTime - endTime).TotalSeconds <= 0)
                    {
                        UpdateFinshed(machid);
                        UpdateFinishTask(batchid);
                        GetOrdersDraft();
                        GetOrdersProgress();
                    }

                };
                b2.Click += (sender, e) =>
                {
                    DialogResult result = MessageBox.Show(
                        "Are you sure you want to cancel this task?",
                        "Confirmation",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        UpdateFinshed(machid);
                        UpdateCancelTask(batchid);
                        GetOrdersDraft();
                        GetOrdersProgress();
                    }
                };


                btnPanel.Controls.Add(b);
            btnPanel.Controls.Add(b2);
            P1.Controls.Add(btnPanel);

            flowLayoutPanel2.Controls.Add(P1);
        }
    }



        private void UpdateFinshed(int id)
        {
            string qry1 = @"update  tbl_fixed_assets set manufactureStatus= @manufactureStatus where id =@ID";
            MySqlCommand cmd = new MySqlCommand(qry1, RMSClass.conn());
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@manufactureStatus", "Draft");
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.ExecuteNonQuery();
            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();

            }
            Utilities.LogAudit(frmLogin.userId, "Update Machine Status", "Machine", id, "Updated Machine Status to Draft");
        }

        private void UpdateCancelTask(int id)
        {
            string qry1 = @"update  tbl_manufacturer_task set Status= @Status where id =@ID";
            MySqlCommand cmd = new MySqlCommand(qry1, RMSClass.conn());
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@Status", "Cancel");
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.ExecuteNonQuery();
            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();

            }
            Utilities.LogAudit(frmLogin.userId, "Cancel Task", "Task", id, "Cancelled Task with ID: " + id);
            deleteItemActions(id);
        }
        private void UpdateFinishTask(int id)
        {
            string qry1 = @"update  tbl_manufacturer_task set Status= @Status where id =@ID";
            MySqlCommand cmd = new MySqlCommand(qry1, RMSClass.conn());
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@Status", "Done");
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.ExecuteNonQuery();
            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();

            }
            Utilities.LogAudit(frmLogin.userId, "Finish Task", "Task", id, "Finished Task with ID: " + id);
            ShowFinishedGoodsDialog(id);
        }
        private void ShowFinishedGoodsDialog(int id)
        {
            string query = @"SELECT b.id,c.name,b.product_id,b.warehouse_id,
                            (SELECT SUM(mbd.Total) FROM tbl_manufacturer_batchdetails mbd WHERE mbd.batchID=b.id) costPrice
                                FROM tbl_manufacturer_task a, tbl_manufacturer_batch b,tbl_items c WHERE a.BatchID = b.id AND a.MachineID = b.fixedassetsID AND b.product_id = c.id AND a.id = @id";
            using (var reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("id", id)))
            {
                if (reader.Read())
                {
                    string mId = Convert.ToString(reader["id"]);
                    string ItemName = Convert.ToString(reader["name"]);
                    string productId = Convert.ToString(reader["product_id"]);
                    string warehouseId = Convert.ToString(reader["warehouse_id"]);
                    decimal costPrice = Convert.ToDecimal(reader["costPrice"]);
                    using (var dialog = new FinishedGoodsDialog(ItemName))
                    {
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            decimal finishedQty = dialog.FinishedQty;
                            decimal sellingPrice = dialog.SellingPrice;
                            DateTime date = DateTime.Now.Date;

                            InsertFinishedGoodsItemTransaction(date, finishedQty, costPrice, sellingPrice, mId, productId, warehouseId);
                            InsertFinishedGoodsItemJournal(date, finishedQty, costPrice, sellingPrice, mId, productId, warehouseId);
                        }
                    }
                }
            }
        }
        private void InsertFinishedGoodsItemTransaction(DateTime date, decimal finishedQty, decimal costPrice, decimal sellingPrice, string mId, string productId, string warehouseId)
        {
            CommonInsert.InsertItemTransaction(date, "Manufacturing", mId.ToString(), productId, costPrice.ToString(),
                finishedQty.ToString(), sellingPrice.ToString(), "0", finishedQty.ToString(), "Manufacturing", warehouseId.ToString());
        }
        private void InsertFinishedGoodsItemJournal(DateTime date, decimal finishedQty, decimal costPrice, decimal sellingPrice, string mId, string productId, string warehouseId)
        {
            object result = DBClass.ExecuteScalar(@"SELECT id FROM tbl_coa_level_4 WHERE name =@name", DBClass.CreateParameter("name", "Raw Material Inventory"));
            int level4Id = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;

            CommonInsert.InsertTransactionEntry(date, BindCombos.SelectDefaultLevelAccount("Inventory").ToString(), (finishedQty * costPrice).ToString(), "0",
                mId.ToString(), mId.ToString(), "Manufacturing", "Manufacturing - Item Code - " + productId, frmLogin.userId, DateTime.Now.Date);
            CommonInsert.InsertTransactionEntry(date, level4Id.ToString(), "0", (finishedQty * costPrice).ToString(),
                    mId.ToString(), "0", "Manufacturing", "Manufacturing - Item Code - " + productId, frmLogin.userId, DateTime.Now.Date);
        }
        private void deleteItemActions(int id)
        {
            string query = @"SELECT b.id,c.name,b.product_id,b.warehouse_id,
                            (SELECT SUM(mbd.Total) FROM tbl_manufacturer_batchdetails mbd WHERE mbd.batchID=b.id) costPrice
                                FROM tbl_manufacturer_task a, tbl_manufacturer_batch b,tbl_items c WHERE a.BatchID = b.id AND a.MachineID = b.fixedassetsID AND b.product_id = c.id AND a.id = @id";
            using (var reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("id", id)))
            {
                if (reader.Read())
                {
                    string reference = Convert.ToString(reader["id"]);
                    string ItemName = Convert.ToString(reader["name"]);
                    string productId = Convert.ToString(reader["product_id"]);
                    string warehouseId = Convert.ToString(reader["warehouse_id"]);
                    decimal costPrice = Convert.ToDecimal(reader["costPrice"]);

                    DBClass.ExecuteNonQuery("DELETE FROM tbl_item_transaction WHERE `type` = 'Sales Invoice' AND `REFERENCE` = @id", DBClass.CreateParameter("id", reference));
                    DBClass.ExecuteNonQuery("DELETE FROM tbl_item_card_details WHERE `trans_type` = 'Sales Invoice' AND `trans_no` = @id", DBClass.CreateParameter("id", reference));
                    DBClass.ExecuteNonQuery(@"UPDATE tbl_items i
                                            INNER JOIN tbl_sales_details sd ON i.id = sd.item_id
                                            SET i.on_hand = i.on_hand + sd.qty
                                            WHERE sd.purchase_id = @id;", DBClass.CreateParameter("id", reference));
                }
            }
        }

        private void frmManScreen_Load(object sender, EventArgs e)
        {
            GetOrdersDraft();
            GetOrdersProgress();

        }
    }

}


public class FinishedGoodsDialog : Form
{
    private TextBox txtItemName;
    private TextBox txtFinishedQty;
    private TextBox txtSellingPrice;
    private Button btnSave;
    private Button btnClose;

    public string ItemName
    {
        get { return txtItemName.Text; }
        set { txtItemName.Text = value; }
    }

    public decimal FinishedQty
    {
        get
        {
            decimal qty;
            if (decimal.TryParse(txtFinishedQty.Text, out qty))
                return qty;
            else
                return 0;
        }
        set
        {
            txtFinishedQty.Text = value.ToString("F2");
        }
    }

    public decimal SellingPrice
    {
        get
        {
            decimal price;
            if (decimal.TryParse(txtSellingPrice.Text, out price))
                return price;
            else
                return 0m;
        }
        set
        {
            txtSellingPrice.Text = value.ToString("F2");
        }
    }

    public FinishedGoodsDialog(string itemName)
    {
        InitializeDynamicComponents();

        // Set item name and make it read-only
        txtItemName.Text = itemName;
        txtItemName.ReadOnly = true;
    }

    private void InitializeDynamicComponents()
    {
        // Form properties
        this.Text = "Finished Goods";
        this.ClientSize = new Size(360, 200);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // Create Labels
        Label lblItemName = new Label() { Text = "Item Name:", Location = new Point(20, 23), AutoSize = true };
        Label lblFinishedQty = new Label() { Text = "Finished Qty:", Location = new Point(20, 63), AutoSize = true };
        Label lblSellingPrice = new Label() { Text = "Selling Price:", Location = new Point(20, 103), AutoSize = true };

        // Create TextBoxes
        txtItemName = new TextBox() { Location = new Point(130, 20), Size = new Size(200, 22), ReadOnly = true };
        txtFinishedQty = new TextBox() { Location = new Point(130, 60), Size = new Size(200, 22) };
        txtSellingPrice = new TextBox() { Location = new Point(130, 100), Size = new Size(200, 22) };
        
        // Add KeyPress events to restrict to decimals only
        txtFinishedQty.KeyPress += NumericTextBox_KeyPress;
        txtSellingPrice.KeyPress += NumericTextBox_KeyPress;

        // Create Buttons
        btnSave = new Button() { Text = "Save", Location = new Point(130, 140), Size = new Size(90, 30) };
        btnClose = new Button() { Text = "Close", Location = new Point(240, 140), Size = new Size(90, 30) };

        // Add event handlers
        btnSave.Click += BtnSave_Click;
        btnClose.Click += BtnClose_Click;

        // Add controls to form
        this.Controls.Add(lblItemName);
        this.Controls.Add(txtItemName);
        this.Controls.Add(lblFinishedQty);
        this.Controls.Add(txtFinishedQty);
        this.Controls.Add(lblSellingPrice);
        this.Controls.Add(txtSellingPrice);
        this.Controls.Add(btnSave);
        this.Controls.Add(btnClose);
    }

    private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        // Allow digits, one decimal point, and control keys like backspace
        TextBox txt = sender as TextBox;

        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
        {
            e.Handled = true;
        }

        // Only allow one decimal point
        if (e.KeyChar == '.' && txt.Text.Contains("."))
        {
            e.Handled = true;
        }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (FinishedQty <= 0)
        {
            MessageBox.Show("Please enter a valid finished quantity (0 or more).", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtFinishedQty.Focus();
            return;
        }

        if (SellingPrice <= 0)
        {
            MessageBox.Show("Please enter a valid selling price (0 or more).", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtSellingPrice.Focus();
            return;
        }

        MessageBox.Show("Finished goods saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void BtnClose_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}