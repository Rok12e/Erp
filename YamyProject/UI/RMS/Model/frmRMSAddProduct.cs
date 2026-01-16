using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using YamyProject.RMS.Class;
using YamyProject.RMS.View;

namespace YamyProject.RMS.Model
{
    public partial class frmRMSAddProduct : frmRMSAddSample
    {
        public frmRMSAddProduct()
        {
            InitializeComponent();
        }

        public int id = 0;
        public int CID =0 ;
        public int code = 0;
        public string DAAT = DateTime.Now.ToString("yyyy-MM-dd");
        string getNextCode()
        {
            string typeCategory = "13 - Inventory Assembly".Split('-')[0].Trim();
            object resultD = DBClass.ExecuteScalar(@"SELECT code FROM tbl_item_category WHERE id = @id", DBClass.CreateParameter("id", Convert.ToInt32(cbCat.SelectedValue)));

            string recordCount = "";
            if (resultD != null && resultD != DBNull.Value)
                recordCount = resultD.ToString();

            if (!string.IsNullOrEmpty(recordCount))
            {
                typeCategory = typeCategory + recordCount;
            }
            string query = "SELECT RIGHT(code, 4) FROM tbl_items WHERE LEFT(code, 5) ORDER BY id DESC LIMIT 1";
            object result = DBClass.ExecuteScalar(query, DBClass.CreateParameter("@typeCategory", typeCategory));
            int nextSerial = (result == DBNull.Value || result == null) ? 1 : Convert.ToInt32(result) + 1;
            string formattedSerial = nextSerial.ToString().PadLeft(4, '0');
            string newItemCode = typeCategory + formattedSerial;
            return newItemCode;
        }

        int cogsAccountId = 0, vendorId = 0, incomeAccountId = 0, assetAccountId = 0;
        private void frmRMSAddProduct_Load(object sender, EventArgs e)
        {
            incomeAccountId = BindCombos.SelectDefaultLevelAccount("Sales");
            assetAccountId = BindCombos.SelectDefaultLevelAccount("Inventory");
            cogsAccountId = BindCombos.SelectDefaultLevelAccount("COGS");

            txtcode.Text = getNextCode() ;
            string qry = "Select id  , name  from tbl_item_category ";
            RMSClass.CBFILL(qry, cbCat);
            bindItemAssembly();

            if (CID > 0)
            {
                cbCat.SelectedValue = CID ;
            }
            if (id > 0)
            {
                ForUpdateLoadData();
            }

        }
        private void bindItemAssembly()
        {
            //dgvItems.Rows.Clear();
            DataTable dt = DBClass.ExecuteDataTable("select code,name from tbl_items where  state = 0 and active = 0");
            DataGridViewComboBoxColumn name = (DataGridViewComboBoxColumn)dgvItems.Columns["name"];
            name.DataSource = dt;
            name.DisplayMember = "name";
            name.ValueMember = "code";
        }
        string filepath;
        Byte[] ImagebyteArray;
        private void btnBrows_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images(.jpg, .png)|* .png; *.jpg";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                filepath = ofd.FileName;
                txtimage.Image = new Bitmap(filepath);


            }
        }

        public override void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtprice.Text))
            {
                // Show a message box if the TextBox is empty
                MessageBox.Show("Can't Saved Product Without Price");
                // Optionally, you can stop the event if needed, but normally the event will end after the message.
                return;
            }
            if (string.IsNullOrEmpty(txtname.Text))
            {
                // Show a message box if the TextBox is empty
                MessageBox.Show("Can't Saved Product Without Name");
                // Optionally, you can stop the event if needed, but normally the event will end after the message.
                return;
            }
            if (string.IsNullOrEmpty(cbCat.Text))
            {
                // Show a message box if the TextBox is empty
                MessageBox.Show("The Item Need Category");
                // Optionally, you can stop the event if needed, but normally the event will end after the message.
                return;
            }
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                if (dgvItems.Rows[i].Cells["qty"].Value == null || dgvItems.Rows[i].Cells["name"].Value == null || dgvItems.Rows[i].Cells["qty"].Value.ToString() == "" || dgvItems.Rows[i].Cells["qty"].Value.ToString() == "0")
                {
                    MessageBox.Show("Item Assembly Can't be 0 or empty");
                    return;
                }
            }

            code = int.Parse(getNextCode());


            string qry1 = "";

            if (id == 0)
            {
                qry1 = "INSERT INTO `tbl_items`(code,type,category_id,name,sales_price,ItemImg,posItem,date,created_date,item_type,cost_price,cogs_account_id,vendor_id,income_account_id,asset_account_id,total_value,method) VALUES (@code,'13 - Inventory Assembly',@catid,@name,@psales,@ItemImg,1,'" + DAAT + "','" + DAAT + "','Inventory',@costPrice,@cogsAccountId,@vendorId,@incomeAccountId,@assetAccountId,@totalValue,'avg');";
            }
            else
            {
                qry1 = "Update  tbl_items set category_id = @catid , name = @name ,sales_price = @psales ,ItemImg = @ItemImg, cost_price = @costPrice,total_value=@totalValue where id=@id ";
            }
            Image Temp = new Bitmap(txtimage.Image);
            MemoryStream ms = new MemoryStream();
            Temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ImagebyteArray = ms.ToArray();

            decimal totalValue = totalCost;

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@code", code);
            ht.Add("@catid",Convert.ToInt32(cbCat.SelectedValue));
            ht.Add("@name", txtname.Text);
            ht.Add("@psales", txtprice.Text);
            ht.Add("@costPrice", totalCost.ToString("N2"));
            ht.Add("@ItemImg", ImagebyteArray);
            ht.Add("@cogsAccountId", cogsAccountId);
            ht.Add("@vendorId", vendorId);
            ht.Add("@incomeAccountId", incomeAccountId);
            ht.Add("@assetAccountId", assetAccountId);
            ht.Add("@totalValue", totalValue.ToString("N2"));

            int resultId = RMSClass.SQl(qry1, ht);
            if (resultId > 0)
            {
                object result = DBClass.ExecuteScalar(
                 @"SELECT id FROM tbl_items WHERE name = @name AND posItem = 1 AND category_id = @catId Order by id desc limit 1",
                 DBClass.CreateParameter("name", txtname.Text.Trim()),DBClass.CreateParameter("catId", Convert.ToInt32(cbCat.SelectedValue)));

                int recordCount = 0;
                if (result != null && result != DBNull.Value)
                    recordCount = Convert.ToInt32(result);

                if (recordCount > 0)
                {
                    insertItemsAssembly(recordCount);
                }
                Utilities.LogAudit(frmLogin.userId, (id == 0 ? "Add Product" : "Update Product"), "Product", recordCount, (id == 0 ? "Added Product: " : "Updated Product: ") + txtname.Text);
                MessageBox.Show("save Successfully...");
                id = 0;
                CID = 0;
                txtprice.Text = "";
                txtcode.Text = "";
                cbCat.SelectedIndex = -1;
                txtname.Text = "";
                txtname.Focus();
                txtimage.Image = Properties.Resources.Phot2o_Gallery;

            }
        }
        private void insertItemsAssembly(int id)
        {
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                DBClass.ExecuteNonQuery("insert into tbl_item_assembly (assembly_id,item_id,qty) values (@assembly_id,@item_id,@qty)",
                    DBClass.CreateParameter("assembly_id", id),
                    DBClass.CreateParameter("item_id", dgvItems.Rows[i].Cells["itemId"].Value.ToString()),
                    DBClass.CreateParameter("qty", dgvItems.Rows[i].Cells["qty"].Value.ToString()));
            }
        }
        private void ForUpdateLoadData()
        {
            string qry = @"Select * from tbl_items where id = " + id + "";
            MySqlCommand cmd = new MySqlCommand(qry, RMSClass.conn());
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtname.Text = dt.Rows[0]["name"].ToString();
                txtprice.Text = dt.Rows[0]["sales_price"].ToString();

                byte[] imagearray = dt.Rows[0]["ItemImg"] as byte[];

                Image itemImage;

                if (imagearray != null && imagearray.Length > 0)
                {
                    itemImage = Image.FromStream(new MemoryStream(imagearray));
                    txtimage.Image = itemImage;
                }
                else
                {
                    itemImage = Properties.Resources.Product;
                }
                using (MySqlDataReader reader = DBClass.ExecuteReader("select ti.code,ti.id,ta.qty,ti.cost_price from tbl_item_assembly ta inner join tbl_items ti on ta.item_id = ti.id where ta.assembly_id=@id", DBClass.CreateParameter("id", id)))
                {
                    while (reader.Read())
                    {
                        dgvItems.Rows.Add(reader["id"].ToString(), "", reader["code"].ToString(), reader["code"].ToString(), reader["cost_price"].ToString(), reader["qty"].ToString());
                    }
                }
            }
        }
        

        private void txtprice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar)) return;

            // Allow digits and decimal point
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Prevent more than one decimal point
            if (e.KeyChar == '.' && txtprice.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvItems.Rows.Count > 1 && dgvItems.CurrentRow.Cells["delete"].ColumnIndex == e.ColumnIndex && !dgvItems.CurrentRow.IsNewRow)
                    dgvItems.Rows.Remove(dgvItems.CurrentRow);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows.Count > 1)
            {
                var row = dgvItems.Rows[e.RowIndex];

                //if (e.ColumnIndex == dgvItems.Columns["Code"].Index)
                //{
                //    string codeValue = row.Cells["Code"].Value?.ToString();
                //    DataGridViewComboBoxCell comboCell = row.Cells["name"] as DataGridViewComboBoxCell;
                //    if (comboCell != null)
                //        insertItemThroughCodeOrCombo("code", comboCell, null);
                //}
                calculateCost();
            }
        }
        private void insertItemThroughCodeOrCombo(string type, DataGridViewComboBoxCell comboCell, ComboBox comboBox)
        {
            MySqlDataReader reader = null;
            if (type == "code")
            {
                reader = DBClass.ExecuteReader(@"SELECT *
                  FROM tbl_items 
                  WHERE code = @code ",
                    DBClass.CreateParameter("code", dgvItems.CurrentRow.Cells["code"].Value.ToString()));
            }
            else if (type == "combo" && comboBox.SelectedValue != null && !comboBox.SelectedValue.ToString().ToLower().Contains("rowview"))
            {
                string selectedItemCode = comboBox.SelectedValue.ToString();
                reader = DBClass.ExecuteReader(@"SELECT tbl_items.id,method,type, code,  sales_price,  cost_price 
                  FROM tbl_items 
                  WHERE  code = @code",
                    DBClass.CreateParameter("code", selectedItemCode));
            }
            if (reader != null && reader.Read())
            {
                dgvItems.CurrentRow.Cells["qty"].Value = "1";
                //dgvItems.CurrentRow.Cells["code"].Value = reader["code"].ToString();
                dgvItems.CurrentRow.Cells["price"].Value = reader["cost_price"].ToString();
                dgvItems.CurrentRow.Cells["itemid"].Value = reader["id"].ToString();
                if (type == "code" && comboCell != null)
                    comboCell.Value = dgvItems.CurrentRow.Cells["code"].Value.ToString();
            }
            else
            {
                if (!dgvItems.CurrentRow.IsNewRow)
                    dgvItems.Rows.Remove(dgvItems.CurrentRow);
            }
        }
        decimal totalCost = 0;
        private void calculateCost()
        {
            for (int i = 0; i < dgvItems.Rows.Count; i++)
            {
                if (dgvItems.Rows[i].Cells["price"].Value == null || dgvItems.Rows[i].Cells["qty"].Value == null || dgvItems.Rows[i].Cells["price"].Value.ToString() == "" || dgvItems.Rows[i].Cells["qty"].Value.ToString() == "")
                    continue;
                totalCost += decimal.Parse(dgvItems.Rows[i].Cells["price"].Value.ToString()) * decimal.Parse(dgvItems.Rows[i].Cells["qty"].Value.ToString());
            }
            //txtCostPrice.Text = totalCost.ToString();
            totalCost = 0;
        }
        private void ComboBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedValue != null)
            {
                insertItemThroughCodeOrCombo("combo", null, comboBox);
            }
        }

        private void dgvItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["name"].Index)
            {
                ComboBox comboBox = e.Control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.SelectedIndexChanged -= new EventHandler(ComboBoxName_SelectedIndexChanged);
                    comboBox.SelectedIndexChanged += new EventHandler(ComboBoxName_SelectedIndexChanged);
                }
            }
        }

        private void lbLinkAddIngredients_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewItem().ShowDialog();
            bindItemAssembly();
        }
    }
}
