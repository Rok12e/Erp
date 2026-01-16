using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace YamyProject
{
    public static class BindCombos
    {
        private static Dictionary<string, DataTable> _caches = new Dictionary<string, DataTable>();

        public static void ClearCache(string key="")
        {
            if (key != "")
            {
                _caches.Remove(key);
                var matchingKeys = _caches.Keys
                    .Where(k => k.Contains(key))
                    .ToList();

                if (matchingKeys.Count > 0)
                {
                    foreach (var k in matchingKeys)
                    {
                        _caches.Remove(k);
                    }
                }
            } else
            {
                _caches.Clear();
            }
        }

        private static void BindComboBox<T>(T cmb, string query, string valueMember, string displayMember,
            bool isAddOption = false, string addOptionValue = "0", string addOptionText = "<< Add >>",
            bool forceRefresh = false, params MySqlParameter[] parameters)
            where T : ListControl
        {
            DataTable dt;

            // Build a cache key that includes parameter values
            //string cacheKey = query + string.Join(";", parameters.Select(p => $"{p.ParameterName}={p.Value}"));

            //if (!forceRefresh && _cache.ContainsKey(cacheKey))
            //{
            //    dt = _cache[cacheKey].Copy();
            //}
            //else
            //{
                dt = DBClass.ExecuteDataTable(query, parameters);
                //_cache[cacheKey] = dt.Copy();
            //}

            if (isAddOption)
            {
                DataRow newRow = dt.NewRow();
                newRow[valueMember] = addOptionValue;
                newRow[displayMember] = addOptionText;
                dt.Rows.InsertAt(newRow, 0);
            }

            cmb.ValueMember = valueMember;
            cmb.DisplayMember = displayMember;
            cmb.DataSource = dt;
        }

        private static void BindCheckedListBox(
            CheckedListBox lst, string query, string valueMember,string displayMember,bool isAddOption = false, string addOptionValue = "0", string addOptionText = "<< Add >>", bool forceRefresh = false)
        {
            if (lst == null)
                throw new ArgumentNullException(nameof(lst)); // make sure not null

            DataTable dt;

            //if (!forceRefresh && _cache.ContainsKey(query))
            //{
            //    dt = _cache[query].Copy();
            //}
            //else
            //{
                dt = DBClass.ExecuteDataTable(query);
            //    _cache[query] = dt.Copy();
            //}

            if (isAddOption)
            {
                DataRow newRow = dt.NewRow();
                newRow[valueMember] = addOptionValue;
                newRow[displayMember] = addOptionText;
                dt.Rows.InsertAt(newRow, 0);
            }

            // clear old items
            lst.Items.Clear();

            // add rows manually
            foreach (DataRow row in dt.Rows)
            {
                lst.Items.Add(
                    new
                    {
                        Text = row[displayMember].ToString(),
                        Value = row[valueMember].ToString()
                    },
                    false // not checked initially
                );
            }

            // make sure list shows text instead of object
            lst.DisplayMember = "Text";
            lst.ValueMember = "Value";
        }


        private static void BindDataGridViewComboBoxColumn(DataGridViewComboBoxColumn cmbColumn, string query, string valueMember, string displayMember,
            bool isAddOption = false, string addOptionValue = "0", string addOptionText = "<< Add >>",
            bool forceRefresh = false, params MySqlParameter[] parameters)
        {
            DataTable dt;

            //string cacheKey = query + string.Join(";", parameters.Select(p => $"{p.ParameterName}={p.Value}"));

            //if (!forceRefresh && _cache.ContainsKey(cacheKey))
            //{
            //    dt = _cache[cacheKey].Copy();
            //}
            //else
            //{
                dt = DBClass.ExecuteDataTable(query, parameters);
            //    _cache[cacheKey] = dt.Copy();
            //}

            if (isAddOption)
            {
                DataRow newRow = dt.NewRow();
                newRow[valueMember] = addOptionValue;
                newRow[displayMember] = addOptionText;
                dt.Rows.InsertAt(newRow, 0);
            }

            cmbColumn.ValueMember = valueMember;
            cmbColumn.DisplayMember = displayMember;
            cmbColumn.DataSource = dt;
        }

        public static Guna2ComboBox PopulateCustomers(Guna2ComboBox cmb, bool isAddOption=false, bool forceRefresh = false)
        {
            string query = "SELECT CONCAT(CODE ,' - ' , NAME) as name , id FROM tbl_customer";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("SELECT CONCAT(CODE ,' - ' , NAME) as NAME ,id FROM tbl_customer");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static ComboBox PopulateCustomers(ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "SELECT id,code,name FROM tbl_customer";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
        }
        public static Guna2ComboBox PopulateFixedAssetsCategories(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select * from tbl_fixed_assets_category";
            BindComboBox(cmb, query, "id", "category_name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_fixed_assets_category");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["category_name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "category_name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static CheckedListBox PopulateListCustomer(CheckedListBox lst, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "SELECT CONCAT(CODE ,' - ' , name) as NAME ,id FROM tbl_customer";
            BindCheckedListBox(lst, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return lst;
            //DataTable dt = DBClass.ExecuteDataTable("SELECT CONCAT(CODE ,' - ' , name) as NAME ,id FROM tbl_customer");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //lst.DisplayMember = "Name";
            //lst.ValueMember = "id";
            //lst.DataSource = dt;
            //return lst;
        }

        public static CheckedListBox PopulateListProject(CheckedListBox lst, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "SELECT CONCAT(CODE ,' - ' , name) as name ,id FROM tbl_projects";
            BindCheckedListBox(lst, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return lst;
            //DataTable dt = DBClass.ExecuteDataTable("SELECT CONCAT(CODE ,' - ' , name) as NAME ,id FROM tbl_customer");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //lst.DisplayMember = "Name";
            //lst.ValueMember = "id";
            //lst.DataSource = dt;
            //return lst;
        }

        internal static void PopulateItems()
        {
            throw new NotImplementedException();
        }

        public static Guna2ComboBox PopulateRegisterBanks(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_bank where state = 0";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_bank where state = 0");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static ComboBox PopulateRegisterBanks(ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_bank where state = 0";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select name, id from tbl_bank where state = 0");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static ComboBox PopulateRegisterBanksAll(ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_bank";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select name, id from tbl_bank where state = 0");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateAccountNames(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select account_name, id from tbl_bank_card";
            BindComboBox(cmb, query, "id", "account_name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select account_name from tbl_bank_card");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["account_name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "account_name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateBanks(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            BindComboBox(cmb, "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_bank", "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_bank");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateNonRegisterBanks(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            BindComboBox(cmb, "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_bank where state = -1", "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_bank where state = -1");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static ComboBox PopulateNonRegisterBanks(ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            BindComboBox(cmb, "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_bank where state = -1", "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_bank where state = -1");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateDepartments(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select id, name from tbl_departments";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_departments");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }

        public static ComboBox LoadMachincombox(ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select * from tbl_fixed_assets where manufacture = 1";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_fixed_assets where manufacture = 1");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static ComboBox PopulateItems(ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select id,name from tbl_items where state = 0 and active = 0";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select id,name from tbl_items where state = 0 and active = 0");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulatePositions(Guna2ComboBox cmb, int departId)
        {
            string query = "select * from tbl_position where department_id=@id";
            BindComboBox(cmb, query, "id", "name", false, null, null, false,
                DBClass.CreateParameter("id", departId));
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_position where department_id=@id ",
            //    DBClass.CreateParameter("id", departId));
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static int SelectDefaultLevelAccount(string type)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT category, account_id FROM tbl_coa_config WHERE category =@type",
                    DBClass.CreateParameter("type", type)))
                if (reader.Read())
                    return int.Parse(reader["account_id"].ToString());
                else return 0;
        }

        public static Dictionary<string, int> LoadDefaultAccounts()
        {
            Dictionary<string, int> defaultAccounts = new Dictionary<string, int>();

            try
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader(@"
            SELECT category, account_id FROM tbl_coa_config"))
                {
                    while (reader.Read())
                    {
                        string category = reader["category"].ToString();
                        int accountId = reader["account_id"] != DBNull.Value ? Convert.ToInt32(reader["account_id"]) : 0;
                        defaultAccounts[category] = accountId;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading default accounts: " + ex.Message);
            }

            return defaultAccounts;
        }

        public static Guna2ComboBox PopulateVendors(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false, string type="")
        {
            string query = "SELECT CONCAT(CODE ,' - ' , NAME) as name , id FROM tbl_vendor";
            if (!string.IsNullOrEmpty(type))
                query += " WHERE type = '"+type+"'";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;

            //BindComboBox(cmb, "SELECT CONCAT(CODE ,' - ' , NAME) as NAME ,id FROM tbl_vendor", "id", "name", isAddOption);
            //return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("SELECT CONCAT(CODE ,' - ' , NAME) as NAME ,id FROM tbl_vendor");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }

        public static ComboBox PopulateVendors(ComboBox cmb, bool isAddOption = false, bool forceRefresh = false, string type = "")
        {
            string query = "SELECT id, code, name FROM tbl_vendor";
            if (!string.IsNullOrEmpty(type))
                query += " WHERE type = '" + type + "'";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
        }

        public static CheckedListBox PopulateListEmployees(CheckedListBox lst, bool isAddOption = false)
        {
            //string query = "SELECT CONCAT(CODE ,' - ' , Name) as name ,id FROM tbl_employee";
            //BindCheckedListBox(lst, query, "id", "name", isAddOption, "0", "<< Add >>", false);
            //return lst;
            DataTable dt = DBClass.ExecuteDataTable("SELECT CONCAT(CODE ,' - ' , Name) as NAME ,id FROM tbl_employee");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            lst.DisplayMember = "name";
            lst.ValueMember = "id";
            if(dt != null && dt.Rows.Count > 0)
                lst.DataSource = dt;

            return lst;
        }
        public static Guna2ComboBox PopulateEmployees(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "SELECT CONCAT(CODE ,' - ' , Name) as NAME ,id FROM tbl_employee";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("SELECT CONCAT(CODE ,' - ' , Name) as NAME ,id FROM tbl_employee");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static ComboBox PopulateEmployees(ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "SELECT CONCAT(CODE ,' - ' , Name) as NAME ,id FROM tbl_employee";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("SELECT CONCAT(CODE ,' - ' , Name) as NAME ,id FROM tbl_employee");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateEmployeesForLoan(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "SELECT NAME ,id FROM tbl_employee";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateCountries(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select id,name from tbl_country";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select id,name from tbl_country");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulatePrepaidExpenseCategories(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select id,name from tbl_prepaid_expense_category";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select id,name from tbl_prepaid_expense_category");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateCities(Guna2ComboBox cmb,int countryId)
        {
            string query = "select * from tbl_city where country_id=@id";
            BindComboBox(cmb, query, "id", "name", false, null, null, false,
                DBClass.CreateParameter("id", countryId));
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_city where country_id=@id",
            //    DBClass.CreateParameter("id", countryId));
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateAllLevel4Account(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_coa_level_4 ORDER BY code";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_coa_level_4 ORDER BY code");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }

        public static ComboBox LoadAllLevel4Account(Guna2ComboBox cmb)
        {
            if (BindDataTable.tableLevel4 != null && BindDataTable.tableLevel4.Rows.Count != 0)
            {
                cmb.DataSource = BindDataTable.tableLevel4;
                cmb.ValueMember = "id";
                cmb.DisplayMember = "name";
                return cmb;
            }
            else
            {
                string query = "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_coa_level_4 ORDER BY code";
                BindDataTable.tableLevel4 = DBClass.ExecuteDataTable(query);

                cmb.DataSource = BindDataTable.tableLevel4;
                cmb.ValueMember = "id";
                cmb.DisplayMember = "name";
                return cmb;
            }
        }

        public static ComboBox PopulateAllLevel4Account(ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select id, code, name from tbl_coa_level_4 ORDER BY name";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
        }

        public static Guna2ComboBox Populatelevel3Account(Guna2ComboBox cmb, int level3Id, bool isAddOption = false)
        {
            string query = "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_coa_level_3 where main_id=@id ORDER BY code";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", false,
                DBClass.CreateParameter("id", level3Id));
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_coa_level_3 where main_id=@id ORDER BY code",
            //    DBClass.CreateParameter("id", level3Id));
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox Populatelevel2Account(Guna2ComboBox cmb, int level2Id, bool isAddOption = false)
        {
            string query = "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_coa_level_2 where main_id=@id ORDER BY code";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", false,
                DBClass.CreateParameter("id", level2Id));
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_coa_level_2 where main_id=@id ORDER BY code",
            //    DBClass.CreateParameter("id", level2Id));
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox Populatelevel1Account(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_coa_level_1";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_coa_level_1");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateVendorCategory(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select * from tbl_vendor_category";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_vendor_category");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }

        public static Guna2ComboBox PopulateSubContractCategory(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select * from tbl_vendor_category";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_vendor_category");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }

        public static Guna2ComboBox PopulateCustomerCategory(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select * from tbl_customer_category";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_customer_category");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateWarehouse(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select concat(code,' - ', name) as name,id from tbl_warehouse";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select concat(code,' - ', name) as name,id from tbl_warehouse");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateItemsCategory(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select concat(code,' - ', name) as name,id from tbl_item_category";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select concat(code,' - ', name) as name,id from tbl_item_category");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateItemUnit(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select * from tbl_unit";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_unit");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateUserRoles(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select * from tbl_sec_roles";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_sec_roles");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateItemTax(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select * from tbl_tax";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_tax");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateCostCenter(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_cost_center";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_cost_center");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static ComboBox PopulateCostCenter(ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_cost_center";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
        }
        public static Guna2ComboBox PopulateEmployeesToPettyCard(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = @"
                SELECT 
                    CONCAT(code, ' - ', name) AS name, 
                    id 
                FROM tbl_employee 
                WHERE id NOT IN (SELECT CAST(name AS UNSIGNED) FROM tbl_petty_cash_card)";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //string query = @"SELECT CONCAT(code, ' - ', name) AS name, id FROM tbl_employee WHERE id NOT IN (SELECT CAST(name AS UNSIGNED) FROM tbl_petty_cash_card)";
            //DataTable dt = DBClass.ExecuteDataTable(query);
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;

            //return cmb;
        }
        public static Guna2ComboBox PopulateEmployeesToPettyCardAllEmp(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = @"
                SELECT 
                    CONCAT(code, ' - ', name) AS name, 
                    id 
                FROM tbl_employee";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //string query = @"SELECT CONCAT(code, ' - ', name) AS name, id FROM tbl_employee WHERE id NOT IN (SELECT CAST(name AS UNSIGNED) FROM tbl_petty_cash_card)";
            //DataTable dt = DBClass.ExecuteDataTable(query);
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;

            //return cmb;
        }

        public static Guna2ComboBox PopulatePettyCash(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = @"
                SELECT 
                    CONCAT(pcc.code, ' - ', emp.name) AS name, 
                    CAST(emp.id AS CHAR) AS value
                FROM tbl_petty_cash_card pcc 
                JOIN tbl_employee emp ON CAST(pcc.name AS UNSIGNED) = emp.id";
            BindComboBox(cmb, query, "value", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable(@"
            //        SELECT 
            //            CONCAT(pcc.code, ' - ', emp.name) AS name, 
            //            CAST(emp.id AS CHAR) AS value
            //        FROM tbl_petty_cash_card pcc 
            //        JOIN tbl_employee emp ON CAST(pcc.name AS UNSIGNED) = emp.id;
            //    ");

            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["value"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.DisplayMember = "name";
            //cmb.ValueMember = "value";
            //cmb.DataSource = dt;

            //return cmb;
        }

        public static ListView PopulateAllLevel4AccountList(ListView lst)
        {
            lst.Items.Clear();
            lst.View = View.Details; 
            lst.FullRowSelect = true;
            lst.Columns.Clear();

            // Add columns
            lst.Columns.Add("Code - Name", 300);
            lst.Columns.Add("ID", 0);

            DataTable dt = DBClass.ExecuteDataTable("SELECT CONCAT(CODE, ' - ', NAME) AS name, id FROM tbl_coa_level_4 ORDER BY code");
            ListViewItem item = new ListViewItem("Add New");
            item.SubItems.Add("0");
            lst.Items.Add(item);
            foreach (DataRow row in dt.Rows)
            {
                ListViewItem itema = new ListViewItem(row["name"].ToString());
                itema.SubItems.Add(row["id"].ToString());
                lst.Items.Add(itema);
            }

            return lst;
        }
        public static Guna2ComboBox resturentcombox(Guna2ComboBox cmb, int departId)
        {
            string query = "select * from tbl_position where department_id=@id";
            BindComboBox(cmb, query, "id", "name", false, null, null, false,
                DBClass.CreateParameter("id", departId));
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_position where department_id=@id ",
            //    DBClass.CreateParameter("id", departId));
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox resturantdepartment(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select * from tbl_departments where name ='Restaurant'";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_departments where name ='Restaurant'");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static void PopulateDGVRegisterBanks(DataGridViewComboBoxColumn cmbColumn)
        {
            string query = "SELECT CONCAT(CODE , ' - ' , NAME) AS name, id FROM tbl_bank WHERE state = 0";
            BindDataGridViewComboBoxColumn(cmbColumn, query, "id", "name", false, null, null, false);
            //cmbColumn.ValueMember = "id";
            //DataTable dt = DBClass.ExecuteDataTable("select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_bank where state = 0");
            //cmbColumn.ValueMember = "id";
            //cmbColumn.DisplayMember = "name";
            //cmbColumn.DataSource = dt;
        }

        public static void PopulateLevel4Account(DataGridViewComboBoxColumn cmbColumn, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "SELECT CAST(code as CHAR) code,name FROM tbl_coa_level_4;";
            BindDataGridViewComboBoxColumn(cmbColumn, query, "code", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            //cmbColumn.DataSource = dt;
            //cmbColumn.DisplayMember = "name";
            //cmbColumn.ValueMember = "code";
        }

        public static Guna2ComboBox PopulatePettyCashCard(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = @"
                                            SELECT 
                    e.id, 
                    e.name, 
                    SUM(r.pay) AS total_amount
                FROM tbl_petty_cash_request r
                INNER JOIN tbl_employee e ON r.petty_cash_name = e.id
                WHERE r.state = 'Approved'
                GROUP BY e.id, e.name;
                                ";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable(query);
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.DisplayMember = "name";
            //cmb.ValueMember = "id";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateProjects(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "SELECT CONCAT(CODE ,' - ' , name) as name ,id FROM tbl_projects";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("SELECT CONCAT(CODE ,' - ' , name) as name ,id FROM tbl_projects");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateTenderedProjects(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = @"
                SELECT 
                    CONCAT(p.CODE, ' - ', p.name) AS name, 
                    p.id 
                FROM tbl_projects p
                WHERE p.id IN (SELECT project_id FROM tbl_project_tender WHERE amount > 0)";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("SELECT CONCAT(CODE ,' - ' , name) as name ,id FROM tbl_projects where id IN (select project_id from tbl_project_tender WHERE amount>0)");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateTender(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "SELECT tender_name as name ,id FROM tbl_project_tender";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("SELECT tender_name as name ,id FROM tbl_project_tender");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateTenderNames(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "SELECT CONCAT(CODE ,' - ' , name) as name ,id FROM tbl_tender_names";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("SELECT CONCAT(CODE ,' - ' , name) as name ,id FROM tbl_tender_names");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox populateSites(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "SELECT CONCAT(CODE ,' - ' , name) as name ,id FROM tbl_project_sites";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("SELECT CONCAT(CODE ,' - ' , name) as name ,id FROM tbl_project_sites");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static ComboBox PopulateCityAllNormalComboBox(ComboBox cmb)
        {
            string query = "select * from tbl_city";
            BindComboBox(cmb, query, "id", "name", false, null, null, false);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_city");
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }

        public static Guna2ComboBox monthcheckcmb(Guna2ComboBox cmb, int id1)
        {
            string query = "SELECT MONTHNAME(WorkDate) AS MonthName FROM tbl_attendancesheet WHERE code =@id GROUP BY MONTHNAME(WorkDate)";
            BindComboBox(cmb, query, "MonthName", "MonthName", false, null, null, false,
                DBClass.CreateParameter("id", id1));
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("SELECT MONTHNAME(WorkDate) AS MonthName FROM tbl_attendancesheet WHERE code =@id GROUP BY MONTHNAME(WorkDate) ",
            //    DBClass.CreateParameter("id", id1));
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "MonthName";
            //cmb.DataSource = dt;
            //return cmb;
        }
        public static Guna2ComboBox PopulateProjectResource(Guna2ComboBox cmb, bool isAddOption = false, bool forceRefresh = false )
        {
            string query = "SELECT CONCAT(CODE ,' - ' , name) as name ,id FROM tbl_project_role";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("SELECT CONCAT(CODE ,' - ' , name) as name ,id FROM tbl_project_role");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }

        public static ComboBox LoadDepartManufacture(ComboBox cmb, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select * from tbl_departments";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh);
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_departments");
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }

        public static ComboBox DepartPositionEmployee(ComboBox cmb, int departId, bool isAddOption = false, bool forceRefresh = false)
        {
            string query = "select * from tbl_position where department_id=@id";
            BindComboBox(cmb, query, "id", "name", isAddOption, "0", "<< Add >>", forceRefresh,
                DBClass.CreateParameter("id", departId));
            return cmb;
            //DataTable dt = DBClass.ExecuteDataTable("select * from tbl_position where department_id=@id ",
            //DBClass.CreateParameter("id", departId));
            //if (isAddOption)
            //{
            //    // Add the "<< Add >>" row at the top
            //    DataRow newRow = dt.NewRow();
            //    newRow["id"] = 0;
            //    newRow["name"] = "<< Add >>";
            //    dt.Rows.InsertAt(newRow, 0);
            //}
            //cmb.ValueMember = "id";
            //cmb.DisplayMember = "name";
            //cmb.DataSource = dt;
            //return cmb;
        }
    }

}
