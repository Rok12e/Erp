using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmAddItem : Form
    {

        public event EventHandler FormClosedWithResult;
        private int id;
        string projectCode;
        int WarehouseId, CategoryID, UnitId, COGSAccount, VendorAccount, IncomeAccount, AssetAccount,itemSl;
        decimal CostPrice = 0, SalesPrice=0;

        public frmAddItem( int id=0,string _projectCode = "0",int _WarehouseId=0,int _CategoryID=0,int _UnitId=0,int _COGSAccount=0,int _VendorAccount=0,int _IncomeAccount=0,int _AssetAccount=0,int _itemSl=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);

            this.id = id;
            this.projectCode = _projectCode;
            this.WarehouseId = _WarehouseId;
            this.CategoryID = _CategoryID;
            this.UnitId = _UnitId;
            this.COGSAccount = _COGSAccount;
            this.VendorAccount = _VendorAccount;
            this.IncomeAccount = _IncomeAccount;
            this.AssetAccount = _AssetAccount;
            this.itemSl = _itemSl==0?1:_itemSl+1;
            if (id == 0)
                this.Text = id == 0 ? "New Item" : "Edit Item";
            headerUC1.FormText = this.Text;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            //FormClosedWithResult?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Name First");
                return;
            }

            MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items where name = @name",
            DBClass.CreateParameter("name", txtName.Text));
            if (reader.Read())
            {
                if (id == 0 || (id != int.Parse(reader["id"].ToString())))
                {
                    MessageBox.Show(" Name Already In Use. Enter Another Name.");
                    return;
                }
            }

            if (id == 0)
            {
                //object result = DBClass.ExecuteScalar("SELECT ifnull(MAX(Id),0) FROM tbl_items;");
                //int nextId = (result == DBNull.Value || result == null) ? 1 : Convert.ToInt32(result) + 1;
                id = (int)decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO `tbl_items`(`code`, `warehouse_id`, `type`,category_id, `name`, `unit_id`, `barcode`, `cost_price`, 
                                        `cogs_account_id`, `vendor_id`, `sales_price`, `income_account_id`, `asset_account_id`, 
                                        `min_amount`, `max_amount`, `on_hand`,method, `total_value`, `date`, `img`, `active`, `state`, 
                                        `created_By`, `created_date`) VALUES (
                                        @code, @warehouse_id, @type,@category, @name, @unit_id, @barcode, @cost_price, 
                                        @cogs_account_id, @vendor_id, @sales_price, @income_account_id, @asset_account_id, 
                                        @min_amount, @max_amount, @on_hand,@method, @total_value, @date, @img, @active, @state, 
                                        @created_By, @created_date); SELECT LAST_INSERT_ID();",
                                 DBClass.CreateParameter("code", projectCode + FormatItemSl(itemSl)),
                                 DBClass.CreateParameter("warehouse_id", WarehouseId),
                                 DBClass.CreateParameter("type", "11 - Inventory Part"),
                                 DBClass.CreateParameter("category", CategoryID),
                                 DBClass.CreateParameter("name", txtName.Text),
                                 DBClass.CreateParameter("unit_id", UnitId),
                                 DBClass.CreateParameter("barcode", ""),
                                 DBClass.CreateParameter("cost_price", CostPrice),
                                 DBClass.CreateParameter("cogs_account_id", COGSAccount),
                                 DBClass.CreateParameter("vendor_id", VendorAccount),
                                 DBClass.CreateParameter("sales_price", SalesPrice),
                                 DBClass.CreateParameter("income_account_id", IncomeAccount),
                                 DBClass.CreateParameter("asset_account_id", AssetAccount),
                                 DBClass.CreateParameter("min_amount", 0),
                                 DBClass.CreateParameter("max_amount", 0),
                                 DBClass.CreateParameter("on_hand", 0),
                                 DBClass.CreateParameter("method", "fifo"),
                                 DBClass.CreateParameter("total_value", 0),
                                 DBClass.CreateParameter("date", DateTime.Now.Date),
                                 DBClass.CreateParameter("img", ""),
                                 DBClass.CreateParameter("active", 0),
                                 DBClass.CreateParameter("state", 0),
                                 DBClass.CreateParameter("created_By", frmLogin.userId),
                                 DBClass.CreateParameter("created_date", DateTime.Now.Date)).ToString());
                Utilities.LogAudit(frmLogin.userId, "Add Item", "Item", id, "Added Item: " + txtName.Text);
            }
            else
            {
                DBClass.ExecuteNonQuery(@"UPDATE tbl_items SET  name = @name WHERE id = @id;",
                        DBClass.CreateParameter("@name", txtName.Text),
                        DBClass.CreateParameter("@id", id));
                Utilities.LogAudit(frmLogin.userId, "Update Item", "Item", id, "Updated Item: " + txtName.Text);
            }
            //EventHub.RefreshItemCategory();
            FormClosedWithResult?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void frmViewTax_Load(object sender, EventArgs e)
        {
            if (id != 0)
            {
                MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items where id = @id",
                      DBClass.CreateParameter("id", id));
                reader.Read();
                txtName.Text = reader["name"].ToString();
            }
            txtName.Focus();
        }
        public static string FormatItemSl(int itemSl)
        {
            string itemSlStr = itemSl.ToString();

            if (itemSlStr.Length == 1)
            {
                return $"00{itemSlStr}";
            }
            else if (itemSlStr.Length == 2)
            {
                return $"01{itemSlStr[0]}";
            }
            else if (itemSlStr.Length == 3)
            {
                return $"{itemSlStr[0]}{itemSlStr[1]}";
            }
            else
            {
                return itemSlStr;
            }
        }
    }
}
