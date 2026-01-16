using System;
using System.Collections.Generic;
using System.Data;

namespace YamyProject
{
    public static class BindDataTable
    {
        public static DataTable tableCountries, tableCities, tableGeneralSettings, tableModules;
        internal static DataTable tableLevel4, tableCoaConfig;
        public static Dictionary<string, int> coaConfigDict;

        public static void FetchData()
        {
            LoadTable(ref tableGeneralSettings, "SELECT id, NAME, VALUE, description,status FROM tbl_general_settings");
            LoadTable(ref tableCountries, "SELECT * FROM tbl_country");
            LoadTable(ref tableCities, "SELECT * FROM tbl_city");
            LoadTable(ref tableLevel4, "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_coa_level_4 ORDER BY code");
            LoadTable(ref tableModules, "select id, name from tbl_set_main_menu id");
        }

        //public static DataTable tableBank, tableBankCard, tableBankCheque, tableCompany, tableCostCenter, tableCustomers, tableCustomerCategory, tableDepartments, tableEmployee, tableFixedAssetsCategory, tableItemCategory, tableProducts, tableProductUnits, tableProductWarehouses, tablePettyCashCategory, tablePosition, tablePrepaidExpenseCategory,
        //    tableSecRoles, tableSecUsers, tableSubCostCenter, tableProductsUnitList, tableVendor, tableVendorCategory, tableAccount1, tableAccount2, tableAccount3, tableAccount4, tableWarehouses;

        //public static void LoadAllData()
        //{
        //    LoadTable(ref tableBank, "SELECT * FROM tbl_bank");
        //    LoadTable(ref tableBankCard, "SELECT * FROM tbl_bank_card");
        //    LoadTable(ref tableBankCard, "SELECT * FROM tbl_bank_register");
        //    LoadTable(ref tableBankCheque, "SELECT * FROM tbl_cheque");
        //    LoadTable(ref tableCompany, "SELECT * FROM tbl_company");
        //    LoadTable(ref tableCostCenter, "SELECT * FROM tbl_cost_center");
        //    LoadTable(ref tableAccount1, "SELECT * FROM tbl_coa_level_1");
        //    LoadTable(ref tableAccount2, "SELECT * FROM tbl_coa_level_2");
        //    LoadTable(ref tableAccount3, "SELECT * FROM tbl_coa_level_3");
        //    LoadTable(ref tableAccount4, "SELECT * FROM tbl_coa_level_4");
        //    LoadTable(ref tableCustomers, "SELECT * FROM tbl_customer");
        //    LoadTable(ref tableCustomerCategory, "SELECT * FROM tbl_customer_category");
        //    LoadTable(ref tableDepartments, "SELECT * FROM tbl_departments");
        //    LoadTable(ref tableEmployee, "SELECT * FROM tbl_employee");
        //    LoadTable(ref tableFixedAssetsCategory, "SELECT * FROM tbl_fixed_assets_category");
        //    LoadTable(ref tableItemCategory, "SELECT * FROM tbl_item_category");
        //    LoadTable(ref tableProducts, "SELECT * FROM tbl_items");
        //    LoadTable(ref tableProductsUnitList, "SELECT * FROM tbl_items_unit");
        //    LoadTable(ref tableProductWarehouses, "SELECT * FROM tbl_items_warehouse");
        //    LoadTable(ref tablePettyCashCategory, "SELECT * FROM tbl_petty_cash_category");
        //    LoadTable(ref tablePosition, "SELECT * FROM tbl_position");
        //    LoadTable(ref tablePrepaidExpenseCategory, "SELECT * FROM tbl_prepaid_expense_category");
        //    LoadTable(ref tableSecRoles, "SELECT * FROM tbl_sec_roles");
        //    LoadTable(ref tableSecUsers, "SELECT * FROM tbl_sec_users");
        //    LoadTable(ref tableSubCostCenter, "SELECT * FROM tbl_sub_cost_center");
        //    LoadTable(ref tableProductUnits, "SELECT * FROM tbl_unit");
        //    LoadTable(ref tableVendor, "SELECT * FROM tbl_vendor");
        //    LoadTable(ref tableVendorCategory, "SELECT * FROM tbl_vendor_category");
        //    LoadTable(ref tableWarehouses, "SELECT * FROM tbl_warehouse");
        //}

        //public static void ReloadItems()
        //{
        //    LoadTable(ref tableProducts, "SELECT * FROM tbl_items");
        //}


        // Generic loader
        private static void LoadTable(ref DataTable table, string query)
        {
            table = DBClass.ExecuteDataTable(query);
        }

        internal static void LoadConfigData()
        {
            string configQuery = "SELECT category, account_id FROM tbl_coa_config";
            BindDataTable.tableCoaConfig = DBClass.ExecuteDataTable(configQuery);
        }

        //private static DataTable GetCustomers()
        //{
        //    var cached = CacheManager<DataTable>.Get("customers");
        //    if (cached != null) return cached;

        //    string query = "SELECT id, code, name FROM tbl_customer";
        //    DataTable dt = DBClass.ExecuteDataTable(query);

        //    CacheManager<DataTable>.Set("customers", dt, TimeSpan.FromMinutes(10));
        //    return dt;
        //}
    }

}

//public class CacheManager<T>
//{
//    private static Dictionary<string, (T Value, DateTime Expiry)> _cache = new();

//    public static void Set(string key, T value, TimeSpan duration)
//    {
//        _cache[key] = (value, DateTime.Now.Add(duration));
//    }

//    public static T? Get(string key)
//    {
//        if (_cache.TryGetValue(key, out var entry) && DateTime.Now < entry.Expiry)
//            return entry.Value;
//        return default;
//    }

//    public static void Clear(string key) => _cache.Remove(key);
//}


//namespace YamyProject
//{
//    public static class BindDataTable
//    {
//        public static DataTable tablePopulateCountries, tablePopulateCities, tableGeneralSettings, tablePopulateCustomers, tablePopulateVendor, tablePopulateBanks, tablePopulateAccountNames;
//        public static void FetchData()
//        {
//            PopulateGeneralSettings();
//            PopulateCountries();
//            PopulateCities();
//        }
//        public static void PopulateCustomers()
//        {
//            tablePopulateCustomers = DBClass.ExecuteDataTable("SELECT * FROM tbl_customer");
//        }
//        public static void PopulateBanks()
//        {
//            tablePopulateBanks = DBClass.ExecuteDataTable("select * from tbl_bank");
//        }
//        public static void PopulateVendors()
//        {
//            tablePopulateVendor = DBClass.ExecuteDataTable("SELECT * FROM tbl_vendor");
//        }
//        public static void PopulateGeneralSettings()
//        {
//            tableGeneralSettings = DBClass.ExecuteDataTable("SELECT id, NAME, VALUE, description,status FROM tbl_general_settings");
//        }
//        public static void PopulateCountries()
//        {
//            tablePopulateCountries = DBClass.ExecuteDataTable("select * from tbl_country");
//        }
//        public static void PopulateCities()
//        {
//            tablePopulateCities = DBClass.ExecuteDataTable("select * from tbl_city");
//        }
//    }
//}
