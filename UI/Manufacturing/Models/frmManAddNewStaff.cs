using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using YamyProject.RMS.Class;

namespace YamyProject.UI.Manufacturing.Models
{

    public partial class frmManAddNewStaff : Form
    {

        public int actid = 0;
        public int MainId = 0;
        public int id = 0;
        public int editID = 0;
        int mainindd = 0;
        int code;
        public frmManAddNewStaff()
        {
            InitializeComponent();
        }

        private DataTable dataTable;
        private void frmManAddNewStaff_Load(object sender, EventArgs e)
        {
            //Add Supplier 
            getdata();
           // lbtotal.Text = ("0.00").ToString();
           //dgvitem2.Visible = false;
            BindCombos.LoadMachincombox(cbMachin);
            txtcode.Text = GenerateNextEmployeeCode();
            BindCombos.LoadDepartManufacture(CbDpartment);
            BindCombos.DepartPositionEmployee(cbMachin, CbDpartment.SelectedValue == null ? 0 : (int)CbDpartment.SelectedValue);

        }


        private void getdata()
        {
            string qry1 = @"Select id,code,name,cost_price from  tbl_items order by name";
            DataTable DT = DBClass.ExecuteDataTable(qry1);
            //dgvitem2.DataSource = DT;
            //dgvitem2.Columns[0].Visible = false;
            //dgvitem2.Columns[2].Width = 90;
            //dgvitem2.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;


            if (editID > 0)
            {
                //string qry2 = @"Select id,code,name,cost_price from  tbl_items order by name";
            }
        }
        private void getdatasearchname(string st)
        {

            string qry1 = "Select id,code,name,cost_price from tbl_items where name like @name";
            DataTable DT = DBClass.ExecuteDataTable(qry1, DBClass.CreateParameter("name", "%" + st + "%"));
            //dgvitem2.DataSource = DT;
            //dgvitem2.Columns[0].Visible = false;
            //dgvitem2.Columns[2].Width = 90;
            //dgvitem2.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;


            if (editID > 0)
            {
                //string qry2 = @"Select id,code,name,cost_price from  tbl_items order by name";
            }

        }






        private void grandtotal()
        {
            double gtot = 0;
            double tot = 0;

            //foreach (DataGridViewRow row in dgvItems.Rows)
            //{
            //    // تجاهل الصف الجديد الفاضي تلقائيًا
            //    if (!row.IsNewRow && row.Cells["DGVTOTATL"].Value != null)
            //    {
            //        // نحاول نحول القيمة إلى double
            //        if (double.TryParse(Convert.ToString(row.Cells["DGVTOTATL"].Value), out tot))
            //        {
            //            gtot += tot;
            //            lbtotal.Text = gtot.ToString();
            //        }
            //    }
            //}
        }

        private void dgvItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //dgvItems.EditMode = DataGridViewEditMode.EditOnEnter;

            //int row = dgvItems.CurrentCell.RowIndex;
            //double Price, qty = 0;
            //double.TryParse(Convert.ToString(dgvItems.Rows[row].Cells[4].Value), out Price);
            //double.TryParse(Convert.ToString(dgvItems.Rows[row].Cells[5].Value), out qty);
            //dgvItems.Rows[row].Cells[6].Value = qty * Price;

        }

        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {




        }

        private void dgvitem2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvItems.CurrentCell.ColumnIndex != 3)
            //    {
            //    return;
            //    }
            //if (dgvItems.CurrentCell.ColumnIndex == 3)
            //{
            //    int row = dgvItems.CurrentCell.RowIndex;

            //    dgvItems.Rows[row].Cells[1].Value = dgvitem2.CurrentRow.Cells[0].Value;
            //    dgvItems.Rows[row].Cells[2].Value = dgvitem2.CurrentRow.Cells[1].Value;
            //    dgvItems.Rows[row].Cells[4].Value = dgvitem2.CurrentRow.Cells[3].Value;
            //    dgvItems.Rows[row].Cells[5].Value = 1;
            //    dgvItems.CurrentCell.Value = dgvitem2.CurrentRow.Cells[2].Value;

            //    dgvitem2.Visible = false;

            //    if (Convert.ToString(dgvItems.Rows[row].Cells[1].Value) == string.Empty)
            //    {
            //        dgvItems.Rows[row].Cells[1].Value = 0;

            //    }
                grandtotal();

            //}



        }
        TextBox editingTextBox;

        private void dgvItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            //if (editingTextBox != null)
            //    editingTextBox.TextChanged -= EditingTextBox_TextChanged;

            //// Attach new handler
            //editingTextBox = e.Control as TextBox;
            //if (editingTextBox != null)
            //    editingTextBox.TextChanged += EditingTextBox_TextChanged;

            //if (dgvItems.CurrentCell.ColumnIndex != 3)
            //{
            //    dgvitem2.Visible = false;
            //}

            //if (dgvItems.CurrentCell.ColumnIndex == 3)
            //{
            //    dgvitem2.Visible = true;
            //    if (e.Control is TextBox)
            //    {
            //        TextBox txtbox = (TextBox)e.Control;
            //        txtbox.TextChanged -= txtbox_TextChanged;
            //        txtbox.TextChanged += txtbox_TextChanged;

            //    }
            }

        //    if (dgvItems.CurrentCell.ColumnIndex == 5)
        //    {
        //        int row = dgvItems.CurrentCell.RowIndex;
        //        double Price, qty = 0;
        //        double.TryParse(Convert.ToString(dgvItems.Rows[row].Cells[4].Value), out Price);
        //        double.TryParse(Convert.ToString(dgvItems.Rows[row].Cells[5].Value), out qty);
        //        dgvItems.Rows[row].Cells[6].Value = qty * Price;
        //        grandtotal();


        //    }
        //    grandtotal();

        //}

        private void txtbox_TextChanged(object sender, EventArgs e)
        {
            {
                // تأكد أن الخلية الحالية غير null
                //if (dgvItems.CurrentCell == null)
                //    return;

                //int colindex = dgvItems.CurrentCell.ColumnIndex;
                //TextBox txtbox = (TextBox)sender;
                //string content = txtbox.Text;

                //// إظهار الجدول عند عمود محدد (مثلاً العمود الثالث)
                //if (colindex == 3)
                //{
                //    dgvitem2.Visible = true;

                //}

                // التحقق من أن الجدول موجود
                if (dataTable != null)
                {
                    //getdatasearchname(dgvItems.CurrentRow.Cells[3].Value.ToString());
                    MessageBox.Show("ASODIJAS");
                }
                //else if (dataTable == null)
                //{
                //    getdata();
                //}

                //else
                //{
                //    MessageBox.Show("الجدول غير محمّل. تأكد من تحميل البيانات قبل الفلترة.");
                //    return;
                //}

                // تعيين موقع dgvitem2

                //Rectangle cellRect = dgvItems.GetCellDisplayRectangle(dgvItems.CurrentCell.ColumnIndex,
                //    dgvItems.CurrentCell.RowIndex, false);

                //int centerX = cellRect.Left + 30;
                //int centerY = cellRect.Top + 230;
                //dgvitem2.Location = new Point(centerX, centerY);

                //// التأكد من عدم تكرار إضافة الحدث
                //dgvitem2.CellClick -= dgvitem2_CellClick;
                //dgvitem2.CellClick += dgvitem2_CellClick;
                grandtotal();
            }
        }

        private void dgvItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

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
            //if (dgvItems.CurrentCell != null && dgvItems.CurrentCell.ColumnIndex >= 3)
            //{
            //    var liveText = ((TextBox)sender).Text;
            //    getdatasearchname(liveText);  // Your function call
            //}
        }

        private void txtamount_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(txtname.Text))
            //{
            //    MessageBox.Show("Can't Saved Lead Without Customer ");
            //    return;
            //}
            //if (string.IsNullOrEmpty(txtamount.Text))
            //{
            //    MessageBox.Show("Can't Saved Lead Without Stage level  ");
            //    return;
            //}

            //if (string.IsNullOrEmpty(txtamount.Text))
            //{
            //    txtamount.Text = "0";
            //}
            ////GenerateNextEmployeeCode();
            //string qry1 = "";
            //string qry2 = "";

            //if (id == 0)
            //{
            //    qry1 = "INSERT INTO  tbl_manufacturer_batch ( batchname,Costamount,amount,hours,userinsert,date,Description) VALUES (@batchname,@Costamount,@amount,@hours,@userinsert,@date,@Description)";
            //}
            //else
            //{
            //    qry1 = "Update  tbl_manufacturer_batch set batchname=@batchname , amount = @amount , hours = @hours ,userinsert=@userinsert,date=@date,Description=@Description  where id =@id";
            //}

            //decimal totalValue = Convert.ToDecimal(txtamount.Text.ToString());
            //decimal totalValue2 = Convert.ToDecimal(lbtotal.Text.ToString());


            //DateTime formattedDate = DateTime.Now;
            ////Hashtable ht = new Hashtable();
            ////if (id != 0)
            ////{
            ////    ht.Add("@id", id);
            ////}
            ////ht.Add("@batchname", txtname.Text);
            ////ht.Add("@amount", totalValue.ToString("N2"));
            ////ht.Add("@hours", txthourse.Text);
            ////ht.Add("@userinsert", "0");
            ////ht.Add("@date", formattedDate.Date);
            ////ht.Add("@Description", txtdiscription.Text);


            ////if (RMSClass.SQl(qry1, ht) > 0)
            ////{
            ////    MessageBox.Show("save Successfully...");
            ////    id = 0;
            ////    txtname.Text = "";
            ////    txtamount.Text = "";
            ////    txtname.Focus();
            ////    txtdiscription.Text = "";
            ////}
            //int detailID = 0;
            //MySqlCommand cmd = new MySqlCommand(qry1, RMSClass.conn());
            //cmd.Parameters.AddWithValue("@id", id);
            //cmd.Parameters.AddWithValue("@batchname", txtname.Text);
            //cmd.Parameters.AddWithValue("@Costamount", totalValue2);
            //cmd.Parameters.AddWithValue("@amount", totalValue);
            //cmd.Parameters.AddWithValue("@hours", txthourse.Text);
            //cmd.Parameters.AddWithValue("@userinsert", "0");
            //cmd.Parameters.AddWithValue("@date", formattedDate.Date);
            //cmd.Parameters.AddWithValue("@Description", txtdiscription.Text);

            //if (cmd.Connection.State != ConnectionState.Open)
            //{
            //    cmd.Connection.Open();
            //}
            //if (id == 0) { id = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }
            //if (cmd.Connection.State == ConnectionState.Open)
            //{
            //    cmd.Connection.Close();
            //}


            //foreach (DataGridViewRow row in dgvItems.Rows)
            //{

            //    detailID = Convert.ToInt32(row.Cells["dgvid"].Value);


            //    if (detailID == 0)
            //    {
            //        addTransactions(mainindd);
            //        qry2 = @"INSERT INTO tbl_batchdetails  (batchID,itemcode,itemName,cost,qty,Total) VALUES (@batchID,@itemcode,@itemName,@cost,@qty,@Total)";
            //    }
            //    else
            //    {
            //        qry2 = @"update  tbl_batchdetails  set batchID = @batchID , itemcode = @itemcode , itemName = @itemName , cost = @cost,qty = @qty,Total=@Total where batchID =@batchID";
            //    }
            //    MySqlCommand cmd2 = new MySqlCommand(qry2, RMSClass.conn());
            //    cmd2.Parameters.AddWithValue("@ID", detailID);
            //    cmd2.Parameters.AddWithValue("@batchID", mainindd.ToString());
            //    cmd2.Parameters.AddWithValue("@itemcode", Convert.ToInt32(row.Cells["Procode"].Value));
            //    cmd2.Parameters.AddWithValue("@itemName", row.Cells["dgvproducts"].Value);
            //    cmd2.Parameters.AddWithValue("@cost", Convert.ToDouble(row.Cells["dgvCost"].Value));
            //    cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["Dgvqty"].Value));
            //    cmd2.Parameters.AddWithValue("@Total", Convert.ToDouble(row.Cells["DGVTOTATL"].Value));
            //    if (cmd2.Connection.State != ConnectionState.Open)
            //    {
            //        cmd2.Connection.Open();
            //    }
            //    cmd2.ExecuteNonQuery();
            //    if (cmd2.Connection.State == ConnectionState.Open)
            //    {
            //        cmd2.Connection.Close();
            //    }

            //}
            DateTime formattedDate = DateTime.Now;

            //decimal totalValue2 = Convert.ToDecimal(lbtotal.Text.ToString());
            //if (dgvItems.Rows.Count == 0 || dgvItems.Rows.Count == 1 && dgvItems.Rows[0].IsNewRow)
            //{
            //    MessageBox.Show("Can't Hold Empty Order");
            //    return;
            //}
            string qry1 = ""; //main table
            string qry2 = ""; //Details table

            int detailID = 0; //Insert           
            if (MainId == 0)

            {
                qry1 = @"INSERT INTO tbl_manufacturer_batch (batchname,Costamount,amount,hours,userinsert,date,Description,fixedassetsID,fixedStatus) VALUES (@batchname,@Costamount,@amount,@hours,@userinsert,@date,@Description,@fixedassetsID,@fixedStatus); 
                       SELECT LAST_INSERT_ID();";
            }
            else //update
            {
                qry1 = @"update  tbl_manufacturer_batch set batchname= @batchname,Costamount= @Costamount,amount= @amount,hours= @hours,userinsert= @userinsert,date= @date,Description=@Description,fixedassetsID=@fixedassetsID,fixedStatus=@fixedStatus  where id =@ID";
            }
            MySqlCommand cmd = new MySqlCommand(qry1, RMSClass.conn());
            cmd.Parameters.AddWithValue("@id", MainId);
            cmd.Parameters.AddWithValue("@batchname", txtname.Text);

            //cmd.Parameters.AddWithValue("@amount", totalValue2);

            cmd.Parameters.AddWithValue("@userinsert","0");
            cmd.Parameters.AddWithValue("@date", formattedDate.Date);

            cmd.Parameters.AddWithValue("@fixedassetsID", cbMachin.SelectedValue);
            cmd.Parameters.AddWithValue("@fixedStatus", "Draft");
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            if (MainId == 0) { MainId = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }
            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();
            }
            //foreach (DataGridViewRow row in dgvItems.Rows)

            //{
            //    if (row.IsNewRow)
            //        continue;
            //    var cellValue = row.Cells[3].Value;
            //    if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
            //        continue;

            //    detailID = MainId;

            //    if (editID == 0)
            //    {
            //        qry2 = @"INSERT INTO  tbl_manufacturer_batchdetails  (batchID,itemid,cost,qty,Total) VALUES (@batchID,@itemid,@cost,@qty,@Total)";
            //    }
            //    else
            //    {
            //        qry2 = @"update   tbl_manufacturer_batchdetails  set batchID= @batchID,itemid= @itemid,cost= @cost,qty= @qty,Total= @Total where id  =@ID";
            //    }
            //    MySqlCommand cmd2 = new MySqlCommand(qry2, RMSClass.conn());
            //    cmd2.Parameters.AddWithValue("@id", detailID);
            //    cmd2.Parameters.AddWithValue("@batchID", MainId);
            //    cmd2.Parameters.AddWithValue("@itemid", Convert.ToInt32(row.Cells["dgvid"].Value));
            //    cmd2.Parameters.AddWithValue("@cost", Convert.ToDouble(row.Cells["dgvCost"].Value));
            //    cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["Dgvqty"].Value));
            //    cmd2.Parameters.AddWithValue("@Total", Convert.ToDouble(row.Cells["DGVTOTATL"].Value));

            //    if (cmd2.Connection.State != ConnectionState.Open)
            //    {
            //        cmd2.Connection.Open();
            //    }
            //    cmd2.ExecuteNonQuery();
            //    if (cmd2.Connection.State == ConnectionState.Open)
            //    {
            //        cmd2.Connection.Close();
            //    }


            //}
            MainId = 0;
            detailID = 0;

            Utilities.LogAudit(frmLogin.userId, "Save Batch", "Batch", MainId, "Saved Batch: " + txtname.Text);
        }

        private void addTransactions(int batchmainid)
        {
            using (MySqlDataReader readItem = DBClass.ExecuteReader(@"Select max(id) as id from  tbl_manufacturer_batch "))
            {
                readItem.Read();
                batchmainid =Convert.ToInt32(readItem["id"].ToString());    
            }

        }

        public void GetDataforupdatedetails()
        {
            int sn = 0;
            sn =sn + 1;
            string qry = @" SELECT  B.id,C.code, C.name,B.cost,B.qty,B.Total
                           FROM `tbl_manufacturer_batchdetails` B
                           INNER JOIN tbl_items C ON C.id = B.itemid where   B.batchID = '21'";
            ListBox lb = new ListBox();
            //lb.Items.Add(dgvid);
            //lb.Items.Add(dgvid);
            //lb.Items.Add(Procode);
            //lb.Items.Add(dgvproducts);
            //lb.Items.Add(dgvCost);
            //lb.Items.Add(Dgvqty);
            //lb.Items.Add(DGVTOTATL);
            //RMSClass.loadData(qry, dgvItems, lb);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            GetDataforupdatedetails();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public string GenerateNextEmployeeCode()
        {

            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(code AS UNSIGNED)) AS lastCode FROM tbl_employee"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                    code = int.Parse(reader["lastCode"].ToString()) + 1;
                else
                    code = 30001;
            }
            return code.ToString("D5");
        }

        private void cbMachin_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindCombos.DepartPositionEmployee(cbMachin, CbDpartment.SelectedValue == null ? 0 : (int)CbDpartment.SelectedValue);

        }

        private void CbDpartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbMachin.ResetText();
            BindCombos.DepartPositionEmployee(cbMachin, CbDpartment.SelectedValue == null ? 0 : (int)CbDpartment.SelectedValue);
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            RMSClass.blurbackground3(new frmManAddNewDepartment());
            BindCombos.LoadDepartManufacture(CbDpartment);
            BindCombos.DepartPositionEmployee(cbMachin, CbDpartment.SelectedValue == null ? 0 : (int)CbDpartment.SelectedValue);
        }

        private void btnaddNewDepartment_Click(object sender, EventArgs e)
        {
            RMSClass.blurbackground3(new frmManAddPositionStaff());
            BindCombos.LoadDepartManufacture(CbDpartment);
            BindCombos.DepartPositionEmployee(cbMachin, CbDpartment.SelectedValue == null ? 0 : (int)CbDpartment.SelectedValue);
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch1.Checked == false)
            {
                actid = 1;
            }
            else
                actid = 0;
            GenerateNextEmployeeCode();
            string qry1 = "";

            if (id == 0)
            {
                qry1 = "INSERT INTO `tbl_employee`(code,name,phone,Email,address,sRole,Position_id,Department_id,active) VALUES (@code,@name,@phone,@Email,@address,@srole,@Position_id,@Department_id,@active);";
            }
            else
            {
                qry1 = "Update  tbl_employee set name = @name,phone = @phone,Email = @Email,address = @address,sRole = @srole,Position_id=@Position_id,Department_id=@Department_id ,active=@active where id=@id ";
            }

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@code", code);
            ht.Add("@Name", txtname.Text);
            ht.Add("@phone", guna2TextBox1.Text);
            ht.Add("@Email", guna2TextBox3.Text);
            ht.Add("@address", guna2TextBox6.Text);
            ht.Add("@srole", cbMachin.Text);
            ht.Add("@Position_id", cbMachin.SelectedValue);
            ht.Add("@Department_id", CbDpartment.SelectedValue);
            ht.Add("@active", actid);

            if (RMSClass.SQl(qry1, ht) > 0)
            {
                Utilities.LogAudit(frmLogin.userId, (id == 0 ? "Save Employee" : "Update Employee"), "Employee", id, (id == 0 ? "Saved Employee: " : "Updated Employee: ") + txtname.Text);
                MessageBox.Show("save Successfully...");
                id = 0;
                guna2TextBox1.Text = "";
                txtcode.Text = "";
                cbMachin.SelectedIndex = -1;
                txtname.Text = "";
                txtname.Focus();
            }
            txtcode.Text = GenerateNextEmployeeCode();
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}


