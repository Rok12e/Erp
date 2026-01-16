using CrystalDecisions.Shared.Json;
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

namespace YamyProject
{
    public partial class frmUpdate : Form
    {
        public frmUpdate()
        {
            InitializeComponent();
            headerUC1.FormText = "Database Update";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"
                        CREATE TABLE IF NOT EXISTS `tbl_project_sites` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `name` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `location_id` int DEFAULT '0',
                          `plot_number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `address` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          PRIMARY KEY (`id`)
                        ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_unicode_ci;";

                DBClass.ExecuteNonQuery(query);

                query = @"
                CREATE TABLE IF NOT EXISTS `tbl_project_tender` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date NOT NULL,
                          `tender_name_id` int NOT NULL DEFAULT '0',
                          `account_id` int NOT NULL DEFAULT '0',
                          `project_id` int NOT NULL DEFAULT '0',
                          `fees` decimal(20, 4) NOT NULL DEFAULT '0.0000',
                          `submission_date` date NOT NULL,
                          `status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `tender_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `description` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `warehouse_id` int NOT NULL DEFAULT '0',
                          `created_by` int NOT NULL DEFAULT '0',
                          `created_date` date DEFAULT NULL,
                          `modified_by` int DEFAULT '0',
                          `modified_date` date DEFAULT NULL,
                          `amount` decimal(20, 4) NOT NULL DEFAULT '0.0000',
                          `bid_amount` decimal(20, 4) NOT NULL DEFAULT '0.0000',
                          `state` int NOT NULL DEFAULT '0',
                          `contractor_id` int NOT NULL DEFAULT '0',
	                      `estimate_status` INT(10) NULL DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_unicode_ci;";

                DBClass.ExecuteNonQuery(query);

                query = @"

                CREATE TABLE IF NOT EXISTS `tbl_project_tender_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `sr` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `tender_id` int NOT NULL DEFAULT '0',
                          `item_id` varchar(350) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `qty` decimal(20, 4) NOT NULL DEFAULT '0.0000',
                          `unit_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `rate` decimal(20, 4) NOT NULL DEFAULT '0.0000',
                          `amount` decimal(20, 4) NOT NULL DEFAULT '0.0000',
                          `margin_percentage` decimal(20, 4) NOT NULL DEFAULT '0.0000',
                          `margin_amount` decimal(20, 4) NOT NULL DEFAULT '0.0000',
                          `total` decimal(20, 4) NOT NULL DEFAULT '0.0000',
                          `progress` decimal(20, 4) NOT NULL DEFAULT '0.0000',
                          `assigned` int DEFAULT NULL,
                          `start_date` date DEFAULT NULL,
                          `end_date` date DEFAULT NULL,
	                      `length` DECIMAL(20, 4) NOT NULL DEFAULT '0.0000',
	                      `width` DECIMAL(20, 4) NOT NULL DEFAULT '0.0000',
	                      `thickness` VARCHAR(50) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	                      `note` VARCHAR(500) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
                          PRIMARY KEY (`id`)
                        ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_unicode_ci;";

                DBClass.ExecuteNonQuery(query);

                query = @"CREATE TABLE IF NOT EXISTS tbl_audit_log (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            user_id INT,
                            action_type VARCHAR(50),
                            module_name VARCHAR(100),
                            record_id INT,
                            details TEXT,
                            action_time DATETIME DEFAULT CURRENT_TIMESTAMP,
                            ip_address VARCHAR(45),
                            machine_name VARCHAR(100)
                        )
                        COLLATE='utf8mb4_general_ci'
                        ENGINE=InnoDB
                        ;";

                DBClass.ExecuteNonQuery(query);

                //query = @"
                //        CREATE TABLE IF NOT EXISTS `tbl_sub_contract` (
                //          `id` int NOT NULL AUTO_INCREMENT,
                //          `code` int NOT NULL DEFAULT '0',
                //          `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                //          `Cat_id` int DEFAULT NULL,
                //          `Balance` decimal(20,4) DEFAULT NULL,
                //          `date` date DEFAULT NULL,
                //          `main_phone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                //          `work_phone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                //          `mobile` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                //          `email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                //          `ccemail` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                //          `website` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                //          `country` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                //          `city` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                //          `region` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                //          `building_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                //          `account_id` int DEFAULT NULL,
                //          `trn` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                //          `facilty_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                //          `active` int DEFAULT NULL,
                //          `created_by` int DEFAULT NULL,
                //          `created_date` date DEFAULT NULL,
                //          `state` int DEFAULT NULL,
                //          PRIMARY KEY (`id`)
                //        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;";
                //DBClass.ExecuteNonQuery(query);
                //query = @"

                //        CREATE TABLE IF NOT EXISTS `tbl_sub_contract_category` (
                //          `id` int NOT NULL AUTO_INCREMENT,
                //          `name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                //          PRIMARY KEY (`id`) USING BTREE
                //        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;";
                //DBClass.ExecuteNonQuery(query);

                query = @"

                CREATE TABLE IF NOT EXISTS `tbl_tender_names` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT '0',
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_unicode_ci;";

                DBClass.ExecuteNonQuery(query);

                query = @"

                CREATE TABLE IF NOT EXISTS `tbl_contractor` (
	                        `id` INT(10) NOT NULL AUTO_INCREMENT,
	                        `name` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	                        `country_id` INT(10) NULL DEFAULT NULL,
	                        PRIMARY KEY (`id`) USING BTREE
                        ) COLLATE = 'utf8mb4_general_ci' ENGINE = InnoDB;";

                DBClass.ExecuteNonQuery(query);

                query = @"

                CREATE TABLE IF NOT EXISTS `tbl_project_material_requests` (
	                        `id` INT(10) NOT NULL AUTO_INCREMENT,
	                        `tender_id` INT(10) NOT NULL DEFAULT '0',
	                        `planning_id` INT(10) NOT NULL DEFAULT '0',
	                        `RequestedDate` DATE NULL DEFAULT NULL,
	                        `IssuedDate` DATE NULL DEFAULT NULL,
	                        `ReceivedDate` DATE NULL DEFAULT NULL,
	                        `itemId` INT(10) NOT NULL DEFAULT '0',
	                        `unit` VARCHAR(50) NOT NULL DEFAULT '0' COLLATE 'utf8mb4_general_ci',
	                        `RequestedQty` DECIMAL(10, 2) NOT NULL DEFAULT '0.00',
                            `IssuedQty` DECIMAL(10, 2) NOT NULL DEFAULT '0.00',
                            `ReceivedQty` DECIMAL(10, 2) NOT NULL DEFAULT '0.00',
	                        PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_unicode_ci;";

                DBClass.ExecuteNonQuery(query);

                query = @"

                CREATE TABLE IF NOT EXISTS tbl_project_resource (
                            `id` int NOT NULL AUTO_INCREMENT,
                            `code` VARCHAR(50) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
                            `date` date DEFAULT NULL,
                            `name` VARCHAR(200) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
                            `role` INT(10) NOT NULL DEFAULT '0',
                            `phone` VARCHAR(20) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
                            `type` VARCHAR(80) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	                        `price_unit` DECIMAL(20, 4) NOT NULL DEFAULT '0.0000',
	                        `unit_time` DECIMAL(20, 4) NOT NULL DEFAULT '0.0000',
	                        `max_unit_time` DECIMAL(20, 4) NOT NULL DEFAULT '0.0000',
							`employee_id` INT,
                  PRIMARY KEY(`id`)
                        );";

                DBClass.ExecuteNonQuery(query);

                query = @"

                CREATE TABLE IF NOT EXISTS tbl_project_role (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    code VARCHAR(50) NOT NULL UNIQUE COLLATE 'utf8mb4_general_ci',
                    name VARCHAR(100) NOT NULL COLLATE 'utf8mb4_general_ci'
                        );";

                DBClass.ExecuteNonQuery(query);

                query = @"

                CREATE TABLE IF NOT EXISTS tbl_project_activity (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    planning_id INT NOT NULL DEFAULT(0),
                    code VARCHAR(500) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
                    name VARCHAR(500) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
                    start_date DATE NOT NULL,
                    end_date DATE NOT NULL
                        );";

                DBClass.ExecuteNonQuery(query);

                query = @"

                CREATE TABLE IF NOT EXISTS `tbl_project_work_done` (
	                        `id` INT(10) NOT NULL AUTO_INCREMENT,
	                        `date` DATE NOT NULL,
	                        `planning_id` INT(10) NOT NULL,
	                        `account_id` INT(10) NOT NULL,
	                        `warehouse_id` INT(10) NOT NULL,
	                        `created_by` INT(10) NOT NULL,
	                        `created_date` DATE NOT NULL,
	                        `state` TINYINT(3) NULL DEFAULT '0',
	                        PRIMARY KEY (`id`) USING BTREE
                        )
                        COLLATE='utf8mb4_0900_ai_ci' ENGINE=InnoDB;";

                DBClass.ExecuteNonQuery(query);

                query = @"

                        CREATE TABLE IF NOT EXISTS `tbl_project_work_done_details` (
	                        `id` INT(10) NOT NULL AUTO_INCREMENT,
	                        `ref_id` INT(10) NOT NULL,
	                        `item_id` INT(10) NOT NULL,
	                        `main_id` INT(10) NOT NULL,
	                        `code` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_0900_ai_ci',
	                        `qty_total` DECIMAL(10,2) NULL DEFAULT '0.00',
	                        `unit` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_0900_ai_ci',
	                        `qty_used` DECIMAL(10,2) NULL DEFAULT '0.00',
	                        `created_at` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
	                        PRIMARY KEY (`id`) USING BTREE
                        )
                        COLLATE='utf8mb4_0900_ai_ci' ENGINE=InnoDB;";

                DBClass.ExecuteNonQuery(query);

                query = @"

                        CREATE TABLE IF NOT EXISTS tbl_project_activity_assignment (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            activity_id INT NOT NULL,
                            resource_id INT NOT NULL
                        ); ";

                DBClass.ExecuteNonQuery(query);

                query = @"

                        CREATE TABLE IF NOT EXISTS `tbl_manufacturer_batch` (
	                        `id` INT(10) NOT NULL AUTO_INCREMENT,
	                        `batchname` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_0900_ai_ci',
	                        `Costamount` DECIMAL(64,2) NULL DEFAULT NULL,
	                        `amount` DECIMAL(62,2) NULL DEFAULT NULL,
	                        `hours` DECIMAL(10,0) NULL DEFAULT NULL,
	                        `userinsert` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_0900_ai_ci',
	                        `date` DATE NULL DEFAULT NULL,
	                        `Description` VARCHAR(1000) NULL DEFAULT NULL COLLATE 'utf8mb4_0900_ai_ci',
	                        `fixedassetsID` INT(10) NULL DEFAULT NULL,
	                        `fixedStatus` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_0900_ai_ci',
                        	`product_id` INT(10) NULL DEFAULT '0',
	                        PRIMARY KEY (`id`) USING BTREE
                        )
                        COLLATE='utf8mb4_0900_ai_ci'
                        ENGINE=InnoDB;";

                DBClass.ExecuteNonQuery(query);

                query = @"

                        CREATE TABLE IF NOT EXISTS `tbl_manufacturer_batchdetails` (
	                        `id` INT(10) NOT NULL AUTO_INCREMENT,
	                        `batchID` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_0900_ai_ci',
	                        `itemid` INT(10) NULL DEFAULT NULL,
	                        `cost` DECIMAL(63,2) NULL DEFAULT NULL,
	                        `qty` INT(10) NULL DEFAULT NULL,
	                        `Total` DECIMAL(63,2) NULL DEFAULT NULL,
	                        PRIMARY KEY (`id`) USING BTREE
                        )
                        COLLATE='utf8mb4_0900_ai_ci'
                        ENGINE=InnoDB;";

                DBClass.ExecuteNonQuery(query);

                query = @"
                        CREATE TABLE IF NOT EXISTS `tbl_color` (
	                        `id` INT NOT NULL AUTO_INCREMENT,
	                        `headerColor` VARCHAR(50) NOT NULL DEFAULT 'RoyalBlue',
	                        `TextColor` VARCHAR(50) NOT NULL DEFAULT 'White',
	                        PRIMARY KEY (`id`)
                        )
                        COLLATE='utf8mb4_general_ci';
                        ";
                DBClass.ExecuteNonQuery(query);

                query = @"
                        CREATE TABLE IF NOT EXISTS tbl_deleted_records (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            table_name VARCHAR(100),
                            record_data TEXT,
                            deleted_by INT,
                            deleted_at DATETIME DEFAULT CURRENT_TIMESTAMP
                        );
                        ";
                DBClass.ExecuteNonQuery(query);

                query = @"
                        CREATE TABLE tbl_petty_cash(
                            id int NOT NULL AUTO_INCREMENT,
                            code VARCHAR(50) NOT NULL,
                            voucher_date DATE NOT NULL,
                            cash_account_id INT NOT NULL,
                            employee_id INT NOT NULL,
                            total decimal(20,6) DEFAULT NULL,
                            notes TEXT NULL,
                            created_by INT NULL,
                            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                            project_id INT NOT NULL DEFAULT '0',
                            status INT NOT NULL DEFAULT '0',
                            PRIMARY KEY (`id`)
                        );
                        ";
                DBClass.ExecuteNonQuery(query);

                query = @"
                        CREATE TABLE tbl_petty_cash_details(
                            id int NOT NULL AUTO_INCREMENT,
                            petty_cash_id INT NOT NULL,
                            entry_date DATE NOT NULL,
                            ref_id VARCHAR(100) NULL,
                            hum_id INT NOT NULL,
                            hum_name VARCHAR(300) NOT NULL DEFAULT '',
                            category INT(10) NOT NULL DEFAULT '0',
                            cost_center_id INT NOT NULL,
                            description TEXT NULL,
                            amount DECIMAL(18,2) NOT NULL DEFAULT 0,
                            project_id INT(10) NOT NULL DEFAULT '0',
                            note TEXT NULL,
                            PRIMARY KEY (`id`)
                        );
                        ";
                DBClass.ExecuteNonQuery(query);

                query = @"-- Modify column
                        ALTER TABLE tbl_items 
                        MODIFY COLUMN name VARCHAR(500) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci';
                        ";
                DBClass.ExecuteNonQuery(query);

                query = @"ALTER TABLE tbl_coa_config ADD UNIQUE KEY uq_category (category);";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_coa_level_1 ADD COLUMN category_code VARCHAR(50);";
                DBClass.ExecuteNonQuery(query);

                query = @"ALTER TABLE tbl_purchase_details CHANGE COLUMN sales_id purchase_id INT NOT NULL DEFAULT 0;";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_purchase_order_details CHANGE COLUMN sales_id purchase_id INT NOT NULL DEFAULT 0;";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_purchase_return_details CHANGE COLUMN sales_id purchase_id INT NOT NULL DEFAULT 0;";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_bank_card ADD COLUMN company_ac INT NOT NULL DEFAULT 0;";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_company ADD COLUMN country_id INT NOT NULL DEFAULT 0;";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_company ADD COLUMN `stampComp` longblob;";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE yamycompany.tbl_company ADD COLUMN `stampComp` longblob;";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_item_transaction ADD COLUMN warehouse_id INT NOT NULL DEFAULT 0;";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_item_transaction MODIFY description VARCHAR(250);";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_project_tender ADD COLUMN estimate_status INT NOT NULL DEFAULT 0;";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_project_activity CHANGE COLUMN activity_id id int NOT NULL AUTO_INCREMENT;";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_project_activity ADD COLUMN progress DECIMAL(20,3) NOT NULL DEFAULT 0;";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_project_activity ADD COLUMN status INT NOT NULL DEFAULT 0;";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_vendor ADD COLUMN `type` VARCHAR(200) NULL DEFAULT 'Vendor' COLLATE 'utf8mb4_general_ci';";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_Projects ADD COLUMN `category` VARCHAR(250) NULL DEFAULT '' COLLATE 'utf8mb4_general_ci';";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_projects ADD COLUMN `description` VARCHAR(500) NULL DEFAULT '' COLLATE 'utf8mb4_general_ci';";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_projects ADD COLUMN `start_date` date DEFAULT NULL;";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_projects ADD COLUMN `end_date` date DEFAULT NULL;";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_projects ADD COLUMN `country_id` int NOT NULL DEFAULT '0';";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_projects ADD COLUMN `city_id` int NOT NULL DEFAULT '0';";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_purchase ADD COLUMN `description` VARCHAR(300) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci';";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_purchase_order ADD COLUMN `description` VARCHAR(300) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci';";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_purchase_return ADD COLUMN `description` VARCHAR(300) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci';";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_sales ADD COLUMN `description` VARCHAR(300) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci';";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_sales_order ADD COLUMN `description` VARCHAR(300) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci';";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_sales_proforma ADD COLUMN `description` VARCHAR(300) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci';";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_sales_quotation ADD COLUMN `description` VARCHAR(300) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci';";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_sales_return ADD COLUMN `description` VARCHAR(300) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci';";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_petty_cash_details ADD COLUMN `hum_name` VARCHAR(300) NOT NULL DEFAULT '';";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_projects ADD COLUMN `status` VARCHAR(50) DEFAULT 'Planned' COLLATE 'utf8mb4_general_ci';";
                DBClass.ExecuteNonQuery(query);
                query = @"INSERT INTO tbl_general_settings (name, value, description, status)
                        SELECT 'PROJECT OPTION', 0, '', 0
                        WHERE NOT EXISTS (
                            SELECT 1 FROM tbl_general_settings WHERE name = 'PROJECT OPTION'
                        );";
                DBClass.ExecuteNonQuery(query);
                query = @"INSERT INTO tbl_general_settings (name, value, description, status)
                        SELECT 'ENABLE PETTYCASH APPROVAL', 0, '', 0
                        WHERE NOT EXISTS (
                            SELECT 1 FROM tbl_general_settings WHERE name = 'ENABLE PETTYCASH APPROVAL'
                        );";
                DBClass.ExecuteNonQuery(query);

                //string[] tables = {
                //    "tbl_advance_payment_voucher",
                //    "tbl_advance_payment_voucher_details",
                //    "tbl_transaction",
                //    "tbl_cost_center",
                //    "tbl_cost_center_transaction",
                //    "tbl_credit_note",
                //    "tbl_credit_note_details",
                //    "tbl_customer",
                //    "tbl_debit_note",
                //    "tbl_debit_note_details",
                //    "tbl_employee",
                //    "tbl_journal_voucher",
                //    "tbl_journal_voucher_details",
                //    "tbl_payment_voucher",
                //    "tbl_payment_voucher_details",
                //    "tbl_purchase",
                //    "tbl_purchase_details",
                //    "tbl_purchase_order",
                //    "tbl_purchase_order_details",
                //    "tbl_purchase_return",
                //    "tbl_purchase_return_details",
                //    "tbl_receipt_voucher",
                //    "tbl_receipt_voucher_details",
                //    "tbl_sales",
                //    "tbl_sales_details",
                //    "tbl_sales_order",
                //    "tbl_sales_order_details",
                //    "tbl_sales_proforma",
                //    "tbl_sales_proforma_details",
                //    "tbl_sales_quotation",
                //    "tbl_sales_quotation_details",
                //    "tbl_sales_return",
                //    "tbl_sales_return_details",
                //    "tbl_sub_cost_center",
                //    "tbl_item_transaction",
                //    "tbl_item_warehouse_transaction",
                //    "tbl_vendor"
                //};

                //foreach (var table in tables)
                //{
                //    string sql = $"ALTER TABLE {table} ADD COLUMN `project_id` INT NOT NULL DEFAULT 0;";
                //    try
                //    {
                //        DBClass.ExecuteNonQuery(sql);
                //    } catch(Exception ex)
                //    {
                //        ex.ToString();
                //    }
                //}

                query = @"ALTER TABLE tbl_vendor ADD COLUMN `project_site` VARCHAR(150) DEFAULT '' COLLATE 'utf8mb4_general_ci';";
                DBClass.ExecuteNonQuery(query);
                query = @"ALTER TABLE tbl_customer ADD COLUMN `project_site` VARCHAR(150) DEFAULT '' COLLATE 'utf8mb4_general_ci';";
                DBClass.ExecuteNonQuery(query);

                //query = @"
                //            ALTER TABLE tbl_advance_payment_voucher ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_advance_payment_voucher_details ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_transaction ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_cost_center ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_cost_center_transaction ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_credit_note ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_credit_note_details ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_customer ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_debit_note ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_debit_note_details ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_employee ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_journal_voucher ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_journal_voucher_details ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_payment_voucher ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_payment_voucher_details ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_purchase ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_purchase_details ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_purchase_order ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_purchase_order_details ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_purchase_return ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_purchase_return_details ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_receipt_voucher ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_receipt_voucher_details ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_sales ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_sales_details ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_sales_order ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_sales_order_details ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_sales_proforma ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_sales_proforma_details ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_sales_quotation ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_sales_quotation_details ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_sales_return ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_sales_return_details ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_sub_cost_center ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_transaction ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_item_transaction ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //            ALTER TABLE tbl_item_warehouse_transaction ADD COLUMN project_id INT NOT NULL DEFAULT 0;
                //        ";
                //DBClass.ExecuteNonQuery(query);

                query = @"-- Create table if not exists
                        CREATE TABLE IF NOT EXISTS tbl_user (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            code VARCHAR(50) NOT NULL,
                            name VARCHAR(255) NOT NULL,
                            active TINYINT DEFAULT 1
                        );

                        -- Add 'length' column if not exists
                        SET @col_exists = (
                            SELECT COUNT(*) 
                            FROM INFORMATION_SCHEMA.COLUMNS 
                            WHERE table_schema = DATABASE() 
                              AND table_name = 'tbl_items_boq' 
                              AND column_name = 'length'
                        );

                        SET @sql = IF(@col_exists = 0,
                                      'ALTER TABLE tbl_items_boq ADD COLUMN `length` DECIMAL(20,2) NULL DEFAULT NULL;',
                                      'SELECT ""length column already exists"";');
                        PREPARE stmt FROM @sql;
                        EXECUTE stmt;
                        DEALLOCATE PREPARE stmt;

                        -- Add 'width' column if not exists
                        SET @col_exists = (
                            SELECT COUNT(*) 
                            FROM INFORMATION_SCHEMA.COLUMNS 
                            WHERE table_schema = DATABASE() 
                              AND table_name = 'tbl_items_boq' 
                              AND column_name = 'width'
                        );

                        SET @sql = IF(@col_exists = 0,
                                      'ALTER TABLE tbl_items_boq ADD COLUMN `width` DECIMAL(10,2) NULL DEFAULT NULL;',
                                      'SELECT ""width column already exists"";');
                        PREPARE stmt FROM @sql;
                        EXECUTE stmt;
                        DEALLOCATE PREPARE stmt;

                        -- Add 'thickness' column if not exists
                        SET @col_exists = (
                            SELECT COUNT(*) 
                            FROM INFORMATION_SCHEMA.COLUMNS 
                            WHERE table_schema = DATABASE() 
                              AND table_name = 'tbl_items_boq' 
                              AND column_name = 'thickness'
                        );

                        SET @sql = IF(@col_exists = 0,
                                      'ALTER TABLE tbl_items_boq ADD COLUMN `thickness` VARCHAR(50) NULL DEFAULT NULL COLLATE utf8mb4_general_ci;',
                                      'SELECT ""thickness column already exists"";');
                        PREPARE stmt FROM @sql;
                        EXECUTE stmt;
                        DEALLOCATE PREPARE stmt;

                        -- Add 'note' column if not exists
                        SET @col_exists = (
                            SELECT COUNT(*) 
                            FROM INFORMATION_SCHEMA.COLUMNS 
                            WHERE table_schema = DATABASE() 
                              AND table_name = 'tbl_items_boq' 
                              AND column_name = 'note'
                        );

                        SET @sql = IF(@col_exists = 0,
                                      'ALTER TABLE tbl_items_boq ADD COLUMN `note` VARCHAR(500) NULL DEFAULT NULL COLLATE utf8mb4_general_ci;',
                                      'SELECT ""note column already exists"";');
                        PREPARE stmt FROM @sql;
                        EXECUTE stmt;
                        DEALLOCATE PREPARE stmt;
                        ";

                DBClass.ExecuteNonQuery(query);
                query = @"-- Add 'length' column if it does not exist
                        SET @exists := (SELECT COUNT(*) 
                                        FROM INFORMATION_SCHEMA.COLUMNS 
                                        WHERE TABLE_NAME = 'tbl_project_tender_details' 
                                        AND COLUMN_NAME = 'length'
                                        AND TABLE_SCHEMA = DATABASE());
                
                        SET @sql := IF(@exists = 0, 
                                       'ALTER TABLE tbl_project_tender_details ADD COLUMN `length` DECIMAL(20,2) NULL DEFAULT NULL;',
                                       'SELECT ""Column length already exists"";');
                        PREPARE stmt FROM @sql;
                        EXECUTE stmt;
                        DEALLOCATE PREPARE stmt;


                        -- Add 'width' column if it does not exist
                        SET @exists := (SELECT COUNT(*) 
                                        FROM INFORMATION_SCHEMA.COLUMNS 
                                        WHERE TABLE_NAME = 'tbl_project_tender_details' 
                                        AND COLUMN_NAME = 'width'
                                        AND TABLE_SCHEMA = DATABASE());
                
                        SET @sql := IF(@exists = 0, 
                                       'ALTER TABLE tbl_project_tender_details ADD COLUMN `width` DECIMAL(10,2) NULL DEFAULT NULL;',
                                       'SELECT ""Column width already exists"";');
                        PREPARE stmt FROM @sql;
                        EXECUTE stmt;
                        DEALLOCATE PREPARE stmt;


                        -- Add 'thickness' column if it does not exist
                        SET @exists := (SELECT COUNT(*) 
                                        FROM INFORMATION_SCHEMA.COLUMNS 
                                        WHERE TABLE_NAME = 'tbl_project_tender_details' 
                                        AND COLUMN_NAME = 'thickness'
                                        AND TABLE_SCHEMA = DATABASE());

                        SET @sql := IF(@exists = 0, 
                                       'ALTER TABLE tbl_project_tender_details ADD COLUMN `thickness` VARCHAR(50) NULL DEFAULT NULL COLLATE ''utf8mb4_general_ci'';',
                                       'SELECT ""Column thickness already exists"";');
                        PREPARE stmt FROM @sql;
                        EXECUTE stmt;
                        DEALLOCATE PREPARE stmt;


                        -- Add 'note' column if it does not exist
                        SET @exists := (SELECT COUNT(*) 
                                        FROM INFORMATION_SCHEMA.COLUMNS 
                                        WHERE TABLE_NAME = 'tbl_project_tender_details' 
                                        AND COLUMN_NAME = 'note'
                                        AND TABLE_SCHEMA = DATABASE());

                        SET @sql := IF(@exists = 0, 
                                       'ALTER TABLE tbl_project_tender_details ADD COLUMN `note` VARCHAR(500) NULL DEFAULT NULL COLLATE ''utf8mb4_general_ci'';',
                                       'SELECT ""Column note already exists"";');
                        PREPARE stmt FROM @sql;
                        EXECUTE stmt;
                        DEALLOCATE PREPARE stmt;
                        ";

                DBClass.ExecuteNonQuery(query);
                query = @"CREATE TABLE IF NOT EXISTS tbl_error_log (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            procedure_name VARCHAR(255),
                            error_message TEXT,
                            created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                            user_id INT
                        );";
                DBClass.ExecuteNonQuery(query);

                MessageBox.Show("Database Updated");
            }
            catch (Exception ex)
            {
                MessageBox.Show("error:" + ex.ToString());
            }
        }

        private void frmUpdate_Load(object sender, EventArgs e)
        {

        }
    }
}
