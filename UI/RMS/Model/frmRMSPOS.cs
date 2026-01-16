using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.RMS.Class;
using YamyProject.RMS.UC;

namespace YamyProject.RMS.Model
{
    public partial class frmRMSPOS : Form
    {
        public frmRMSPOS()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }
        public int MainId = 0;
        public string Ordertype ="";
        public int driverID = 0;
        public string customerName = "";
        public string customerPhone = "";
        public string Username = "";
        int code;
        int sno = 0;
        int vid = 0;
        int proid = 0;
        string name = "";
        int qtyy = 0;
        double Price = 0;
        double totp = 0;

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();  
        }

        private void frmRMSPOS_Load(object sender, EventArgs e)
        {
            guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
            AddCategory();

            ProductPanel .Controls.Clear();
            loadProducts();
            label5.Text = "";

        }

        private void AddCategory()
        {
            string qry = "Select * from tbl_item_category";
            MySqlCommand cmd = new MySqlCommand(qry, RMSClass.conn());
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
             
            CategoryPanel.Controls.Clear();
            if (dt.Rows.Count > 0 )
            {
                foreach (DataRow row in dt.Rows)
                {

                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                    b.FillColor = Color.FromArgb(50, 55, 89);
                    b.Size = new Size(145, 45);
                    b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    b.Text = row["name"].ToString();
                    //event for click 
                    b.Click += new EventHandler(b_click);


                    CategoryPanel.Controls.Add(b);

                }

            }
        }
        //private void _click(object sender, EventArgs e)
        //{

        //    foreach (var item in ProductPanel.Controls)
        //    {
        //        var pro = (sender)item;
        //        pro.Visible = pro.Name.ToLower().Contains(txtSearch.Text.Trim().ToLower());

        //    }
        //}

        private void b_click(object sender, EventArgs e)

        {
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            if(b.Text =="All Categories")
            {
                txtSearch.Text = "1";
                txtSearch.Text = "";
                return; 
            }

            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PCategory.ToLower().Contains(b.Text.Trim().ToLower());

            }

        }

        private void AddItems(string id,string ProID,string name ,string cat,string price,Image pimage)
        {
            var w = new ucProduct()
            {
                Pname = name,
                Pprice = price,
                PCategory = cat,
                Pimage = pimage,
                id = Convert.ToInt32(ProID)

            };
            ProductPanel.Controls.Add(w);

            w.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;

                foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                {
                    //this will check it product already there then a one to quty and  update@
                    if (Convert.ToInt32(item.Cells["dgvproID"].Value) == wdg.id)
                    {
                        item.Cells["dgvQty"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1 ;
                        item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) *
                                                        double.Parse(item.Cells["dgvPrice"].Value.ToString());
                        gettotal();
        
                            return;
                    }
                



                }

                //this line  add new  product
                guna2DataGridView1.Rows.Add(new object[] {0, 0, wdg.id, wdg.Pname, 1, wdg.Pprice , wdg.Pprice });
                gettotal();
            };
        }
        //geting product from database
        private void loadProducts()
        {
            string qry = "SELECT tbl_items.*, C.name as cat FROM tbl_items INNER JOIN tbl_item_category C ON C.id = tbl_items.category_id and tbl_items.posItem = 1;";
            MySqlCommand cmd = new MySqlCommand(qry, RMSClass.conn());
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow item in dt.Rows)
            {
                byte[] imagearray = item["ItemImg"] as byte[];

                Image itemImage;

                if (imagearray != null && imagearray.Length > 0)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(imagearray))
                        {
                            itemImage = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        itemImage = Properties.Resources.Product;
                    }
                }
                else
                {
                    itemImage = Properties.Resources.Product;
                }

                AddItems("0",item["id"].ToString(), item["name"].ToString(), item["cat"].ToString(),
                    item["sales_price"].ToString(), itemImage);

            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in ProductPanel.Controls)
            {
               var pro = (ucProduct)item;
                pro.Visible = pro.Name.ToLower().Contains(txtSearch.Text.Trim().ToLower());

            }
        }

        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // for serial no
            int count = 0;
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {

                count++;
                row.Cells[0].Value = count;


            }
        }
        private void gettotal()
        {
            double tot = 0;
            lbtotal.Text = "";
            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                tot += double.Parse(item.Cells["dgvAmount"].Value.ToString());

            }
            lbtotal.Text = tot.ToString("N2");
        }



        private void btnNew_Click(object sender, EventArgs e)
        {
            lbtable.Text = "";
            lbwaiter.Text = "";
            lbtable.Visible = false;
            lbwaiter.Visible = false;   
            guna2DataGridView1.Rows.Clear();
            MainId = 0;
            lbtotal.Text = "0.00";

        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            lbtable.Text = "";
            lbwaiter.Text = "";
            lbtable.Visible = false;
            lbwaiter.Visible = false;
            Ordertype = "Delivery";

            frmRMSAddCustomer frm = new frmRMSAddCustomer();
            frm.mainID = MainId;
            frm.OrderType = Ordertype;
            RMSClass.blurbackground(frm);

            if (frm.txtname.Text != "")//as take away did not have Driver info
            {
                driverID = frm.driverID;
                LBIDriverName.Text = "Customer Name: " + frm.txtname.Text + " "+ "Phone: " + frm.txtPhone.Text + " " + "Driver:" + frm.cbDriver.Text;
                LBIDriverName.Visible = true;
                customerName = frm.txtname.Text;
                customerPhone = frm.txtPhone.Text;

            }

           
        }


        private void btnTake_Click(object sender, EventArgs e)
        {
           
            lbtable.Text = "";
            lbwaiter.Text = "";
            lbtable.Visible = false;
            lbwaiter.Visible = false;
            Ordertype = "Take Away";

            frmRMSAddCustomer frm = new frmRMSAddCustomer();
            frm.mainID = MainId;
            frm.OrderType = Ordertype;
            RMSClass.blurbackground(frm);

            if(frm.txtname.Text != "")//as take away did not have Driver info
            {
                driverID=frm.driverID;
                LBIDriverName.Text = "Customer Name: " + frm.txtname.Text + " " + "Phone: " + frm.txtPhone.Text ;
                LBIDriverName.Visible = true;
                customerName = frm.txtname.Text;
                customerPhone = frm.txtPhone.Text;

            }
        }

        private void btnDin_Click(object sender, EventArgs e)
        {
            {

                Ordertype = "Din In";
                LBIDriverName.Visible = false;
                // i will create firm for table selection and waiter selection
                frmRMSTableSelect frm =  new frmRMSTableSelect();
                RMSClass.blurbackground(frm);
                 if ( frm.Tablename !="")
                {
                    lbtable.Text = frm.Tablename;
                    lbtable.Visible =true;

                }
                else
                {
                    lbtable.Text = " ";
                    lbtable.Visible = false;
                }

                frmWaiterSelect frm2 = new frmWaiterSelect();
                RMSClass.blurbackground(frm2);
                if (frm2.waitrName != "")
                {
                    lbwaiter.Text = frm2.waitrName;
                    lbwaiter.Visible = true;
                }
                else
                {
                    lbwaiter.Text = " ";
                    lbwaiter.Visible = false;
                }




                //lbtable.Text = "";
                //lbwaiter.Text = "";
                //lbtable.Visible = false;
                //lbwaiter.Visible = false;
                //Ordertype = "Take Away";
            }
        }

        public string getmaxidOrder()
        {
            DateTime dtt;
            dtt = DateTime.Now.Date;
            using (var reader = DBClass.ExecuteReader("SELECT MAX(TdOrderNo)  FROM tbl_rmsmain where aDate = '" + dtt + "' "))
            {
                if (reader.Read() && reader["TdOrderNo"] != DBNull.Value)
                    code = int.Parse(reader["lastCode"].ToString()) + 1;
                else
                    code = 1;
            }
            return code.ToString();
        }

        private void btnKot_Click(object sender, EventArgs e)
        {
            //getmaxidOrder();
            if (guna2DataGridView1.Rows.Count == 0 || guna2DataGridView1.Rows.Count == 1 && guna2DataGridView1.Rows[0].IsNewRow)
            {

                MessageBox.Show("Can't Send Empty Order To Kitchen");
                // Optionally, you can stop the event if needed, but normally the event will end after the message.
                return;



            }
            string qry1 = ""; //main table
            string qry2 = ""; //Details table

            int detailID = 0; //Insert
            if (Ordertype == "")
            {
                MessageBox.Show("Please Select Order Type");
                return;
             }

            if (MainId == 0) 
            {
                qry1 = @"INSERT INTO tbl_rmsmain (aDate,time,tableName,
                         waiterName,status,orderType,Total,received,changetot,DriverID,CusTName,CustPhone,TdOrderNo,UserSale) VALUES (@aDate,@time,@tableName,
                         @waiterName,@status,@orderType,@Total,@received,@changetot,@DriverID,@CusTName,@CustPhone,@TdOrderNo,@UserSale); 
                       SELECT LAST_INSERT_ID();";

            }
            else //update
            {
                qry1 = @"update  tbl_rmsmain set status= @status,Total= @Total,received= @received,changetot= @changetot 
                         where MainId =@ID";
            }
            MySqlCommand cmd = new MySqlCommand(qry1, RMSClass.conn());
            cmd.Parameters.AddWithValue("@ID", MainId);
            cmd.Parameters.AddWithValue("@aDate",Convert.ToDateTime(DateTime.Now.Date));
            cmd.Parameters.AddWithValue("@time", DateTime.Now.ToShortTimeString());
            cmd.Parameters.AddWithValue("@tableName", lbtable.Text);
            cmd.Parameters.AddWithValue("@waiterName", lbwaiter.Text);
            cmd.Parameters.AddWithValue("@status", "Pending");
            cmd.Parameters.AddWithValue("@orderType", Ordertype);
            cmd.Parameters.AddWithValue("@Total", Convert.ToDouble(lbtotal.Text));
            cmd.Parameters.AddWithValue("@received", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@changetot", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@DriverID", driverID);
            cmd.Parameters.AddWithValue("@CusTName", customerName);
            cmd.Parameters.AddWithValue("@CustPhone", customerPhone);
            cmd.Parameters.AddWithValue("@TdOrderNo", code);
            cmd.Parameters.AddWithValue("@UserSale", Username);

            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            if (MainId == 0) {
                MainId=Convert.ToInt32( cmd.ExecuteScalar()); 
                Utilities.LogAudit(frmLogin.userId, "Add RMS Order", "RMS Order", MainId, "Added RMS Order: " + lbtable.Text + " " + lbwaiter.Text + " Total: " + lbtotal.Text);
            } else { 
                cmd.ExecuteNonQuery(); 
                Utilities.LogAudit(frmLogin.userId, "Update RMS Order", "RMS Order", MainId, "Updated RMS Order: " + lbtable.Text + " " + lbwaiter.Text + " Total: " + lbtotal.Text);
            }

            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();
            }

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                detailID = Convert.ToInt32(row.Cells["dgvid"].Value);

                if (detailID ==0)
                {
                    qry2 = @"INSERT INTO tbl_rmsdetails  (MainID,proID,qty,Price,amount) VALUES (@MainID,@proID,@qty,@Price,@amount)";
                }
                else
                {
                    qry2 = @"update  tbl_rmsdetails  set proID= @proID,qty= @qty,Price= @Price,amount= @amount where DetailID =@ID";
                }

                MySqlCommand cmd2 = new MySqlCommand(qry2, RMSClass.conn());
                cmd2.Parameters.AddWithValue("@ID", detailID);
                cmd2.Parameters.AddWithValue("@MainID", MainId);
                cmd2.Parameters.AddWithValue("@proID",Convert.ToInt32(row.Cells["dgvproID"].Value));
                cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmd2.Parameters.AddWithValue("@Price", Convert.ToDouble(row.Cells["dgvPrice"].Value));
                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(row.Cells["dgvAmount"].Value));


                if (cmd2.Connection.State != ConnectionState.Open)
                {
                    cmd2.Connection.Open();
                }
                cmd2.ExecuteNonQuery();
                Utilities.LogAudit(frmLogin.userId, "Add RMS Order Details", "RMS Order Details", detailID, "Added RMS Order Details: " + row.Cells["dgvproID"].Value + " Qty: " + row.Cells["dgvQty"].Value + " Price: " + row.Cells["dgvPrice"].Value + " Amount: " + row.Cells["dgvAmount"].Value);
                if (cmd2.Connection.State == ConnectionState.Open)
                {
                    cmd2.Connection.Close();
                }
                if (detailID == 0)
                {
                    object result = DBClass.ExecuteScalar("SELECT IFNULL(MAX(DetailID),0) FROM tbl_rmsdetails;");

                    if (result != null && result != DBNull.Value)
                    {
                        detailID = Convert.ToInt32(result);
                    }
                }

                    addTransactions(Convert.ToInt32(row.Cells["dgvproID"].Value), Convert.ToDecimal(row.Cells["dgvQty"].Value), Convert.ToDecimal(row.Cells["dgvPrice"].Value), detailID);
            }
            MainId = 0;
            detailID = 0;
            guna2DataGridView1.Rows.Clear();
            lbtable.Text = "";
            lbwaiter.Text = "";
            lbtable.Visible = false;
            lbwaiter.Visible = false;
            lbtotal.Text = "0.00";
            LBIDriverName.Text = "";

        }
        public int id = 0;
        private void addTransactions(int _itemId,decimal qty, decimal price, int invId)
        {
            using (MySqlDataReader readItem = DBClass.ExecuteReader(@"Select * from tbl_items where id = @id",DBClass.CreateParameter("id",_itemId)))
            {
                readItem.Read();
                string itemType = readItem["type"].ToString();
                string method = readItem["method"].ToString();

                if (itemType == "13 - Inventory Assembly")
                {
                    using (MySqlDataReader componentReader = DBClass.ExecuteReader(@"
                            SELECT item_id, qty,(select method FROM tbl_items WHERE tbl_items.id = tbl_item_assembly.item_id) as method 
                            FROM tbl_item_assembly 
                            WHERE assembly_id = @assemblyId",
                        DBClass.CreateParameter("assemblyId", _itemId)))
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

                            insertItemTransaction(componentId, price, componentQty, methodOfIngrediant,invId);
                            
                        }
                    }
                }
                else if (itemType == "11 - Inventory Part")
                {
                    using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT * FROM tbl_items WHERE id=@id",
                        DBClass.CreateParameter("id", _itemId)))
                    {
                        if (reader.Read())
                        {
                            decimal onHand = Convert.ToDecimal(reader["on_hand"]);
                            DBClass.ExecuteNonQuery("UPDATE tbl_items SET on_hand = @newQty WHERE id = @id",
                                DBClass.CreateParameter("newQty", onHand - qty),
                                DBClass.CreateParameter("id", _itemId));

                            insertItemTransaction(_itemId, price, qty, method, invId);
                        }
                    }
                }
            }
        }

        private void insertItemTransaction(int itemId,decimal price, decimal qty, string method, int invId)
        {
            bool AllowItemWithOutQty = false;
            decimal cost_price = 0, totalCost = 0;
            DateTime dated = DateTime.Now;
            var generalS = Utilities.GeneralSettingsState("ALLOW ITEM WITHOUT QTY");
            if (!string.IsNullOrEmpty(generalS) & int.Parse(generalS) > 0)
            {
                AllowItemWithOutQty = true;
            }
            else
            {
                AllowItemWithOutQty = false;
            }
            if (AllowItemWithOutQty)
            {
                object result = DBClass.ExecuteScalar(@"SELECT cost_price FROM tbl_item_transaction 
                                            WHERE item_id = @id AND date <= @date 
                                            ORDER BY date DESC LIMIT 1",
                                DBClass.CreateParameter("id", itemId), DBClass.CreateParameter("date", dated.Date));

                if (result != null && result != DBNull.Value)
                {
                    cost_price = Convert.ToDecimal(result);
                }

                totalCost = cost_price * qty;

                CommonInsert.InsertItemTransaction(dated.Date, "RMS Bill", invId.ToString(), itemId.ToString(),
                    cost_price.ToString(), "0", price.ToString(), qty.ToString(), "0",
                    "RMS Bill No. " + invId, "0");
            }
            else
            {
                if (method == "fifo" || method == "lifo")
                {
                    MySqlDataReader reader = null;
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

                        CommonInsert.InsertItemTransaction(dated.Date, "RMS Bill", invId.ToString(), itemId.ToString(),
                            cost_price.ToString(), "0", price.ToString(), qtyToUse.ToString(), "0",
                            "RMS Bill No. " + invId, "0");

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

                        CommonInsert.InsertItemTransaction(dated.Date, "RMS Bill", invId.ToString(), itemId.ToString(),
                            cost_price.ToString(), "0", price.ToString(), remainingQty.ToString(), "0",
                            "RMS Bill No. " + invId , "0");
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

                    CommonInsert.InsertItemTransaction(dated.Date, "RMS Bill", invId.ToString(), itemId.ToString(),
                        cost_price.ToString(), "0", price.ToString(), qty.ToString(), "0",
                        "RMS Bill No. " + invId, "0");

                    using (MySqlDataReader dr = DBClass.ExecuteReader(@"SELECT (balance / qty_balance) cost FROM tbl_item_card_details 
                            WHERE DATE <= @date AND trans_type = 'RMS Bill No' AND itemId = @itemId
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
            }
        }
        private void btnBill_Click(object sender, EventArgs e)
        {
            frmRMSBillList frm = new frmRMSBillList() ;
            RMSClass.blurbackground(frm);

            if (frm.MainID > 0)
            {
                id = frm.MainID;
                MainId = frm.MainID;
                loadEntries();

            }
            
        }

        private void loadEntries ()
        {
            string qry = @"Select d.DetailID,d.proID,p.name,p.id,d.qty,d.price,d.amount,m.tableName,m.waiterName,m.orderType from tbl_rmsmain m 
                                 inner join tbl_rmsdetails d on m.MainId = d.MainID
                                 inner join tbl_items p on p.id = d.proID 
                                 where m.MainId = " + id + " ";
            MySqlCommand cmd2 = new MySqlCommand(qry, RMSClass.conn());
            DataTable dt2 = new DataTable();
            MySqlDataAdapter DA2 = new MySqlDataAdapter(cmd2);
            DA2.Fill(dt2);

            if (dt2.Rows[0]["orderType"].ToString() == "Delivery")
            {

                btnDelivery.Checked = true;
                lbwaiter.Visible = false; 
                lbtable.Visible = false;
            }
            else if (dt2.Rows[0]["orderType"].ToString() == "Take Away")
            {
                btnTake.Checked = true;
                lbwaiter.Visible = false;
                lbtable.Visible = false;
            }
            else
            {
                btnDin.Checked = true;
                lbwaiter.Visible = true;
                lbtable.Visible = true;
            }





                guna2DataGridView1.Rows.Clear();
            foreach(DataRow item in dt2.Rows)
            {
                lbtable.Text = item["tableName"].ToString();
                lbwaiter.Text = item["waiterName"].ToString();
                string detailid = item["DetailID"].ToString();
                string ProName = item["name"].ToString();
                string proid = item["proID"].ToString();
                string qty = item["qty"].ToString();
                string Price = item["price"].ToString();
                string amount = item["amount"].ToString();
              
                object[] obj = { 0,detailid,proid, ProName,qty, Price,amount };
                guna2DataGridView1.Rows.Add(obj);

            }
            gettotal();
        }

        private void ProductPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btncheckout_Click(object sender, EventArgs e)
        {
            frmRMSCheckout frm = new frmRMSCheckout();
            frm.Mainid = id;
            frm.amt =Convert.ToDouble(lbtotal.Text);
            RMSClass.blurbackground(frm);

            MainId = 0;
            guna2DataGridView1.Rows.Clear();
            lbtable.Text = "";
            lbwaiter.Text = "";
            lbtable.Visible = false;
            lbwaiter.Visible = false;
            lbtotal.Text = "0.00";

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.Rows.Count == 0 || guna2DataGridView1.Rows.Count == 1 && guna2DataGridView1.Rows[0].IsNewRow)
            {

                MessageBox.Show("Can't Hold Empty Order");
                // Optionally, you can stop the event if needed, but normally the event will end after the message.
                return;



            }
            string qry1 = ""; //main table
            string qry2 = ""; //Details table

            int detailID = 0; //Insert
            if (Ordertype == "")
            {
                MessageBox.Show("Please Select Order Type");
                return;
            }
            if (MainId == 0)
               
            {
                qry1 = @"INSERT INTO tbl_rmsmain (aDate,time,tableName,
                         waiterName,status,orderType,Total,received,changetot,DriverID,CusTName,CustPhone) VALUES (@aDate,@time,@tableName,
                         @waiterName,@status,@orderType,@Total,@received,@changetot,@DriverID,@CusTName,@CustPhone); 
                       SELECT LAST_INSERT_ID();"; 
            }
            else //update
            {
                qry1 = @"update  tbl_rmsmain set status= @status,Total= @Total,received= @received,changetot= @changetot 
                         where MainId =@ID";
            }
            MySqlCommand cmd = new MySqlCommand(qry1, RMSClass.conn());
            cmd.Parameters.AddWithValue("@ID", MainId);
            cmd.Parameters.AddWithValue("@aDate", Convert.ToDateTime(DateTime.Now.Date));
            cmd.Parameters.AddWithValue("@time", DateTime.Now.ToShortTimeString());
            cmd.Parameters.AddWithValue("@tableName", lbtable.Text);
            cmd.Parameters.AddWithValue("@waiterName", lbwaiter.Text);
            cmd.Parameters.AddWithValue("@status", "Hold");
            cmd.Parameters.AddWithValue("@orderType", Ordertype);
            cmd.Parameters.AddWithValue("@Total", Convert.ToDouble(lbtotal.Text));
            cmd.Parameters.AddWithValue("@received", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@changetot", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@DriverID", driverID);
            cmd.Parameters.AddWithValue("@CusTName", customerName);
            cmd.Parameters.AddWithValue("@CustPhone", customerPhone);

            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            
            if (MainId == 0) { 
                MainId = Convert.ToInt32(cmd.ExecuteScalar());
                Utilities.LogAudit(frmLogin.userId, "Add RMS Order", "RMS Order", MainId, "Added RMS Order: " + lbtable.Text + " " + lbwaiter.Text + " Total: " + lbtotal.Text);
            } else { 
                cmd.ExecuteNonQuery();
                Utilities.LogAudit(frmLogin.userId, "Update RMS Order", "RMS Order", MainId, "Updated RMS Order: " + lbtable.Text + " " + lbwaiter.Text + " Total: " + lbtotal.Text);
            }
            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();
            }
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                detailID = Convert.ToInt32(row.Cells["dgvid"].Value);

                if (detailID == 0)
                {
                    qry2 = @"INSERT INTO tbl_rmsdetails  (MainID,proID,qty,Price,amount) VALUES (@MainID,@proID,@qty,@Price,@amount)";
                }
                else
                {
                    qry2 = @"update  tbl_rmsdetails  set proID= @proID,qty= @qty,Price= @Price,amount= @amount where DetailID =@ID";
                }
                MySqlCommand cmd2 = new MySqlCommand(qry2, RMSClass.conn());
                cmd2.Parameters.AddWithValue("@ID", detailID);
                cmd2.Parameters.AddWithValue("@MainID", MainId);
                cmd2.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["dgvproID"].Value));
                cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmd2.Parameters.AddWithValue("@Price", Convert.ToDouble(row.Cells["dgvPrice"].Value));
                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(row.Cells["dgvAmount"].Value));


                if (cmd2.Connection.State != ConnectionState.Open)
                {
                    cmd2.Connection.Open();
                }
                cmd2.ExecuteNonQuery();
                Utilities.LogAudit(frmLogin.userId, "Add RMS Order Details", "RMS Order Details", detailID, "Added RMS Order Details: " + row.Cells["dgvproID"].Value + " Qty: " + row.Cells["dgvQty"].Value + " Price: " + row.Cells["dgvPrice"].Value + " Amount: " + row.Cells["dgvAmount"].Value);
                if (cmd2.Connection.State == ConnectionState.Open)
                {
                    cmd2.Connection.Close();
                }


            }
            MainId = 0;
            detailID = 0;
            guna2DataGridView1.Rows.Clear();
            lbtable.Text = "";
            lbwaiter.Text = "";
            lbtable.Visible = false;
            lbwaiter.Visible = false;
            lbtotal.Text = "0.00";
            LBIDriverName.Text = "";
        }

        private void guna2TileButton17_Click(object sender, EventArgs e)
        {
            qtyy = Convert.ToInt32(guna2TextBox1.Text);
            guna2TextBox1.Text =Convert.ToString( qtyy + 1);
            qtyy = Convert.ToInt32(guna2TextBox1.Text);
        }

        private void guna2TileButton16_Click(object sender, EventArgs e)
        {
            qtyy = Convert.ToInt32(guna2TextBox1.Text);
            if (qtyy<= 1)
            {

                    MessageBox.Show("Can't Insert Quantity Down 1");
                    // Optionally, you can stop the event if needed, but normally the event will end after the message.
                    return;              
            }
            else
            { 
          
            guna2TextBox1.Text = Convert.ToString(qtyy - 1);
            qtyy = Convert.ToInt32(guna2TextBox1.Text);
            }
        }

        private void guna2TileButton1_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Text = guna2TextBox1.Text + 1;
        }

        private void guna2TileButton2_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Text = guna2TextBox1.Text + 2;
        }

        private void guna2TileButton3_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Text = guna2TextBox1.Text + 3;
        }

        private void guna2TileButton4_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Text = guna2TextBox1.Text + 4;
        }

        private void guna2TileButton5_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Text = guna2TextBox1.Text + 5;
        }

        private void guna2TileButton12_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Text = guna2TextBox1.Text + 6;
        }

        private void guna2TileButton7_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Text = guna2TextBox1.Text + 7;
        }

        private void guna2TileButton8_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Text = guna2TextBox1.Text + 8;
        }

        private void guna2TileButton9_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Text = guna2TextBox1.Text + 9;
        }

        private void guna2TileButton11_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Text = guna2TextBox1.Text + 0;
        }

        private void guna2TileButton10_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Text = "1";
            
        }

        private void RemoveSelectedRows()
        {
            // Loop through the selected rows in reverse order (to avoid index issues)
            foreach (DataGridViewRow row in guna2DataGridView1.SelectedRows)
            {
                // Remove the selected row
                guna2DataGridView1.Rows.Remove(row);
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            BindCustomerInfo();
            btndelete.Enabled = true;
            btnEnter.Enabled = true;
        }

        private void guna2DataGridView1_Click(object sender, EventArgs e)
        {
            BindCustomerInfo();
        }

        private void BindCustomerInfo()
        {
            var row = guna2DataGridView1.SelectedRows[0].Cells;
            label5.Text = string.IsNullOrWhiteSpace(row["dgvName"].Value?.ToString()) ? "-" : row["dgvName"].Value.ToString();
            qtyy = Convert.ToInt32(string.IsNullOrWhiteSpace(row["dgvQty"].Value?.ToString()) ? "-" : row["dgvQty"].Value.ToString());
            Price = Convert.ToDouble(string.IsNullOrWhiteSpace(row["dgvPrice"].Value?.ToString()) ? "-" : row["dgvPrice"].Value.ToString());
            totp = Convert.ToDouble(string.IsNullOrWhiteSpace(row["dgvAmount"].Value?.ToString()) ? "-" : row["dgvAmount"].Value.ToString());
            guna2TextBox1.Text = Convert.ToString(qtyy);
        }

        private void guna2TileButton6_Click(object sender, EventArgs e)
        {
         
            if (qtyy < 1)
            {
                MessageBox.Show("Please Choose An Item");
                return;
            }
            else
            {

            
     
                // Check if a row is selected
                if (guna2DataGridView1.SelectedRows.Count > 0)
                {
                    qtyy = Convert.ToInt32(guna2TextBox1.Text);
                    // Get the first selected row (assuming single selection)
                    DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];

                // Update values in the selected row's cells
                    selectedRow.Cells[4].Value = Convert.ToInt32(qtyy);
                    selectedRow.Cells[6].Value = Convert.ToInt32(qtyy) * Price;
                gettotal();
                label5.Text = "";
                qtyy = 0;
                guna2TextBox1.Text = "1";
                    // You can also update multiple columns
                    // selectedRow.Cells["ColumnName"].Value = "New Value"; // Use column names if needed
                }
           
                else
                {
                    // Display a message if no row is selected
                    MessageBox.Show("No row selected!");
                }
            }
        }
       

        private void guna2DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            guna2TextBox1.Text = "1";
            RemoveSelectedRows();
            gettotal();
            btndelete.Enabled = false;
            btnEnter.Enabled = false;


        }
    }
}
