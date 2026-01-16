         CREATE TABLE IF NOT EXISTS `tbl_advance_payment_voucher` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `pv_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `method` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `amount` decimal(20,4) DEFAULT NULL,
                          `debit_account_id` int DEFAULT NULL,
                          `debit_cost_center_id` int DEFAULT NULL,
                          `description` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `credit_account_id` int DEFAULT NULL,
                          `credit_cost_center_id` int DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_advance_payment_voucher_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `payment_id` int NOT NULL DEFAULT '0',
                          `name` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `bank_name` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `check_name` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `check_no` int DEFAULT NULL,
                          `check_date` date DEFAULT NULL,
                          `bank_account_name` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `book_no` int DEFAULT NULL,
                          `trans_date` date DEFAULT NULL,
                          `trans_name` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `trans_ref` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `description` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `amount` decimal(20,6) DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_attendancesheet` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `attendance_salary_id` int NOT NULL DEFAULT '0',
                          `code` int NOT NULL,
                          `WorkDate` date NOT NULL,
                          `TimeIn` time DEFAULT NULL,
                          `TimeOut` time DEFAULT NULL,
                          `DayOfWeek` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Status` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Reference` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Ref_Code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_attendance_salary` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `emp_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `absence_days` int DEFAULT NULL,
                          `total_absence` decimal(20,6) DEFAULT NULL,
                          `delay_minutes` decimal(20,6) DEFAULT NULL,
                          `total_delay` decimal(20,6) DEFAULT NULL,
                          `total_loan` decimal(20,6) DEFAULT NULL,
                          `net_salary` decimal(20,6) DEFAULT NULL,
                          `pay` decimal(20,6) DEFAULT NULL,
                          `change` decimal(20,6) DEFAULT NULL,
                          `ss_no` int DEFAULT '0',
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_bank` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `abb_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `ent_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `route_num` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `country_id` int DEFAULT NULL,
                          `state` int DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_bank_card` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `bank_id` int DEFAULT NULL,
                          `account_name` varchar(70) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `account_type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `account_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `swift` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `iban_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `branch_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `emirates` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `currency` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `account_manager` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `account_sign` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `account_mob` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `account_id` int DEFAULT NULL,
                          `state` int DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          `company_ac` int DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_bank_register` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `bank_id` int DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_check_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `check_id` int DEFAULT NULL,
                          `check_no` int DEFAULT NULL,
                          `check_date` date DEFAULT NULL,
                          `check_type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `pvc_no` int DEFAULT NULL,
                          `check_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `amount` decimal(20,6) DEFAULT NULL,
                          `pass_date` date DEFAULT NULL,
                          `return_date` date DEFAULT NULL,
                          `hold_date` date DEFAULT NULL,
                          `cancel_date` date DEFAULT NULL,
                          `state` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_cheque` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `bank_card_id` int DEFAULT NULL,
                          `chq_book_no` int DEFAULT NULL,
                          `chq_book_qty` int DEFAULT NULL,
                          `leaves_start_from` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `leaves_end_in` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_city` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `country_id` int DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        INSERT INTO `tbl_city` (`name`, `country_id`) VALUES
	                        ('Dubai', 49),
	                        ('Abu Dhabi', 49),
	                        ('Sharjah', 49),
	                        ('Ajman', 49),
	                        ('Umm Al-Quwain', 49),
	                        ('Ras Al Khaimah', 49),
	                        ('Fujairah', 49),
	                        ('Tirana', 1),
	                        ('Algiers', 2),
	                        ('Andorra la Vella', 3),
	                        ('Luanda', 4),
	                        ('Buenos Aires', 5),
	                        ('Yerevan', 6),
	                        ('Sydney', 7),
	                        ('Vienna', 8),
	                        ('Baku', 9),
	                        ('Nassau', 10),
	                        ('Manama', 11),
	                        ('Dhaka', 12),
	                        ('Bridgetown', 13),
	                        ('Minsk', 14),
	                        ('Brussels', 15),
	                        ('Belmopan', 16),
	                        ('Porto-Novo', 17),
	                        ('Thimphu', 18),
	                        ('Sucre', 19),
	                        ('Sarajevo', 20),
	                        ('Gaborone', 21),
	                        ('Brasília', 22),
	                        ('Bandar Seri Begawan', 23),
	                        ('Sofia', 24),
	                        ('Ouagadougou', 25),
	                        ('Bujumbura', 26),
	                        ('Phnom Penh', 27),
	                        ('Yaoundé', 28),
	                        ('Ottawa', 29),
	                        ('Praia', 30),
	                        ('Santiago', 31),
	                        ('Beijing', 32),
	                        ('Bogotá', 33),
	                        ('Havana', 34),
	                        ('Copenhagen', 35),
	                        ('Cairo', 36),
	                        ('Paris', 37),
	                        ('Berlin', 38),
	                        ('New Delhi', 39),
	                        ('Rome', 40),
	                        ('Tokyo', 41),
	                        ('Mexico City', 42),
	                        ('Abuja', 43),
	                        ('Moscow', 44),
	                        ('Riyadh', 45),
	                        ('Cape Town', 46),
	                        ('Madrid', 47),
	                        ('Ankara', 48),
	                        ('London', 50),
	                        ('Washington D.C.', 51),
	                        ('Hanoi', 52),
	                        ('Harare', 53);

                        CREATE TABLE IF NOT EXISTS `tbl_coa` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `company_id` int NOT NULL,
                          `account_code` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          `account_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          `parent_id` int DEFAULT NULL,
                          `account_type` enum('Asset','Liability','Equity','Revenue','Expense') CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          `level` int NOT NULL,
                          `account_category` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `is_group` tinyint(1) DEFAULT '1',
                          PRIMARY KEY (`id`),
                          UNIQUE KEY `account_code` (`account_code`),
                          KEY `parent_id` (`parent_id`),
                          CONSTRAINT `tbl_coa_ibfk_1` FOREIGN KEY (`parent_id`) REFERENCES `tbl_coa` (`id`) ON DELETE CASCADE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_coa_config` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `account_id` int DEFAULT NULL,
                          `category` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL UNIQUE,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                       CREATE TABLE IF NOT EXISTS `tbl_coa_level_1` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          `code` int DEFAULT NULL,
                          `category_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        INSERT INTO `tbl_coa_level_1` (`id`, `name`, `code`, `category_code`) VALUES
	                        (1, 'Assets', 1,'ASSET'),
	                        (2, 'Liabilities', 2,'LIABILITY'),
	                        (3, 'Equity', 3,'EQUITY'),
	                        (4, 'Income', 4,'INCOME'),
	                        (5, 'Cost', 5,'COST'),
	                        (6, 'General & Direct Expenses', 6,'EXPENSE');

                        CREATE TABLE IF NOT EXISTS `tbl_coa_level_2` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          `code` int NOT NULL,
                          `main_id` int NOT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        INSERT INTO `tbl_coa_level_2` (`id`, `name`, `code`, `main_id`) VALUES
	                        (1, 'Current Assets', 11, 1),
	                        (2, 'Long-Term Assets', 12, 1),
	                        (3, 'Current Liabilities', 21, 2),
	                        (4, 'Long-Term Liabilities', 22, 2),
	                        (5, 'Equity & Capital', 31, 3),
	                        (6, 'Direct Income', 41, 4),
	                        (7, 'Indirect Income', 42, 4),
	                        (8, 'Cost', 51, 5),
	                        (9, 'Direct Expenses', 61, 6),
	                        (10, 'Operating Expenses', 62, 6),
	                        (11, 'Inventory Adjustments', 63, 6),
	                        (12, 'Other Income', 465, 4);

                        CREATE TABLE IF NOT EXISTS `tbl_coa_level_3` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          `code` int NOT NULL,
                          `main_id` int NOT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        INSERT INTO `tbl_coa_level_3` (`id`, `name`, `code`, `main_id`) VALUES
	                        (1, 'Banks', 1101, 1),
	                        (2, 'Petty Cash', 1102, 1),
	                        (3, 'Accounts Receivable', 1103, 1),
	                        (4, 'Inventory', 1104, 1),
	                        (5, 'Prepayment', 1105, 1),
	                        (6, 'Other\'s Receivable', 1106, 1),
	                        (7, 'Related parties Receivable', 1107, 1),
	                        (8, 'Fixed Assets', 1201, 2),
	                        (9, 'Suppliers', 2101, 3),
	                        (10, 'Accrued Employee benefits Expense', 2102, 3),
	                        (11, 'Other Current Liabilities', 2103, 3),
	                        (12, 'Long-term employee benefits', 2201, 4),
	                        (13, 'Capital', 3101, 5),
	                        (14, 'Members Draw', 3102, 5),
	                        (15, 'Opening Balance Equity', 3201, 5),
	                        (16, 'Reserve', 3301, 5),
	                        (17, 'Retained earnings', 3401, 5),
	                        (18, 'Sales', 4101, 6),
	                        (19, 'Sales Return', 4102, 6),
	                        (20, 'Maintenance Income', 4201, 7),
	                        (21, 'Cost of Goods Sold', 5101, 8),
	                        (22, 'Freight Shipping Cost', 5102, 8),
	                        (23, 'Customs & Clearance', 5103, 8),
	                        (24, 'Other Cost', 5104, 8),
	                        (25, 'General Expenses', 6101, 9),
                            (26, 'Payroll Liabilities', 2104, 3),
	                        (27, 'Employee Receivable', 1108, 1),
	                        (28, 'Short Term Investment', 1109, 1),
	                        (29, 'Other Payable', 2105, 3),
	                        (30, 'Cash & Cash Equivalents', 1110, 1),
	                        (31, 'Losses', 6201, 16),
	                        (32, 'Stock Write-offs', 6387, 17),
	                        (33, 'Stock Adjustments', 46501, 18),
	                        (34, 'Accounts Payable', 1111, 1);

                        CREATE TABLE IF NOT EXISTS `tbl_coa_level_4` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          `code` int NOT NULL,
                          `main_id` int NOT NULL,
                          `debit` decimal(20,3) DEFAULT '0.000',
                          `credit` decimal(20,3) DEFAULT '0.000',
                          `date` datetime DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        INSERT INTO `tbl_coa_level_4` (`id`, `name`, `code`, `main_id`, `debit`, `credit`, `date`) VALUES
	                        (1, 'Bank Account', 1101001, 1, 0.000, 0.000, NULL),
	                        (2, 'Petty Cash', 1102001, 2, 0.000, 0.000, NULL),
	                        (3, 'Accounts Receivable', 1103001, 3, 0.000, 0.000, NULL),
	                        (4, 'Raw Material Inventory', 1104001, 4, 0.000, 0.000, NULL),
	                        (5, 'Finish Goods Inventory', 1104002, 4, 0.000, 0.000, NULL),
	                        (6, 'Prepayment Government', 1105001, 5, 0.000, 0.000, NULL),
	                        (7, 'Prepayment Rent', 1105002, 5, 0.000, 0.000, NULL),
	                        (8, 'Prepayment Other\'s', 1105003, 5, 0.000, 0.000, NULL),
	                        (9, 'Deposits', 1106001, 6, 0.000, 0.000, NULL),
	                        (10, 'Taxes Paid (Input)', 1106002, 6, 0.000, 0.000, NULL),
	                        (11, 'Advance payments to employees', 1106003, 6, 0.000, 0.000, NULL),
	                        (12, 'Fixed Assets Net', 1201001, 8, 0.000, 0.000, NULL),
	                        (13, 'Suppliers (Creditors)  Net', 2101001, 9, 0.000, 0.000, NULL),
	                        (14, 'Accrued Salaries', 2102001, 10, 0.000, 0.000, NULL),
	                        (15, 'Accrued Sales commission', 2102002, 10, 0.000, 0.000, NULL),
	                        (16, 'Accrued Leave Salary', 2102003, 10, 0.000, 0.000, NULL),
	                        (17, 'Sales Tax Payable', 2103001, 11, 0.000, 0.000, NULL),
	                        (18, 'PDC CHQ', 2103002, 11, 0.000, 0.000, NULL),
	                        (19, 'Advance from Customers', 2103004, 11, 0.000, 0.000, NULL),
	                        (20, 'Gratuity Provision', 2201001, 12, 0.000, 0.000, NULL),
	                        (21, 'Capital - Opening Balance', 3101001, 13, 0.000, 0.000, NULL),
	                        (22, 'Opening Balance Equity', 3201001, 15, 0.000, 0.000, NULL),
	                        (23, 'Sales - Local', 4101001, 18, 0.000, 0.000, NULL),
	                        (24, 'Sales - GCC', 4101002, 18, 0.000, 0.000, NULL),
	                        (25, 'Materials Purchased', 5101001, 21, 0.000, 0.000, NULL),
	                        (26, 'Employee Salary Advance', 1108001, 57, 0.000, 0.000, NULL),
	                        (27, 'Employee Loans', 1108002, 57, 0.000, 0.000, NULL),
	                        (28, 'Other Employee Receivables', 1101002, 1, 0.000, 0.000, NULL),
	                        (29, 'Accrued Salary', 2102004, 10, 0.000, 0.000, NULL),
	                        (30, 'Employee Payables', 2105001, 59, 0.000, 0.000, NULL),
	                        (31, 'Stock Settlement Gain', 46501001, 68, 0.000, 0.000, NULL),
	                        (32, 'Accumlated Dep Fur', 1201002, 8, 0.000, 0.000, NULL),
	                        (33, 'PDC Receivable', 1103002, 3, 0.000, 0.000, NULL),
	                        (34, 'PDC Receivable Return', 1103003, 3, 0.000, 0.000, NULL),
	                        (35, 'PDC Receivable Hold', 1103004, 3, 0.000, 0.000, NULL),
	                        (36, 'PDC Receivable Cancel', 1103005, 3, 0.000, 0.000, NULL),
	                        (37, 'PDC Payable', 1111001, 69, 0.000, 0.000, NULL),
	                        (38, 'PDC Payable Return', 1111002, 69, 0.000, 0.000, NULL),
	                        (39, 'PDC Payable Hold', 1111003, 69, 0.000, 0.000, NULL),
	                        (40, 'PDC Payable Cancel', 1111003, 69, 0.000, 0.000, NULL),
	                        (41, 'Sales Discount', 6101004, 25, 0.000, 0.000, NULL),
	                        (42, 'Cash On Hand', 1103006, 3, 0.000, 0.000, NULL),
	                        (43, 'Sales Return', 4101003, 19, 0.000, 0.000, NULL);

                        INSERT INTO `tbl_coa_config` (`id`, `account_id`, `category`) VALUES
	                        (1, 23, 'Sales'),
	                        (2, 25, 'COGS'),
	                        (3, 42, 'Default Account For Cash'),
	                        (4, 5, 'Inventory'),
                            (5, 4, 'Item Damage'),
	                        (6, 42, 'Invoice Payment Cash Method'),
                            (7, 13, 'Invoice Payment Credit Method'),
	                        (8, 17, 'Vat Output'),
	                        (9, 13, 'Vendor'),
	                        (10, 10, 'Vat Input'),
	                        (11, 42, 'Purchase Payment Cash Method'),
	                        (12, 13, 'Purchase Payment Credit Method'),
	                        (13, 3, 'Opening Balance'),
	                        (14, 22, 'Opening Balance Equity'),
	                        (15, 14, 'Accrued Salaries'),
	                        (16, 16, 'Acroal Leave Salary'),
	                        (17, 28, 'Employee Receivable'),
	                        (18, 8, 'Prepaid Expense Debit Account'),
	                        (19, 12, 'Fixed Asset Credit Account'),
	                        (20, 37, 'PDC Payable'),
	                        (21, 38, 'PDC Payable Return'),
	                        (22, 39, 'PDC Payable Hold'),
	                        (23, 40, 'PDC Payable Cancel'),
	                        (24, 33, 'PDC Receivable'),
	                        (25, 34, 'PDC Receivable Return'),
	                        (26, 35, 'PDC Receivable Hold'),
	                        (27, 36, 'PDC Receivable Cancel'),
	                        (28, 3, 'Customer'),
	                        (29, 2, 'Petty Cash Account'),
	                        (30, 14, 'End of Service Debit'),
	                        (31, 30, 'End of Service Credit'),
	                        (32, 30, 'Leave Salary Debit'),
	                        (33, 30, 'Leave Salary Credit'),
	                        (34, 13, 'Purchase'),
	                        (35, 13, 'PurchaseReturn'),
	                        (36, 43, 'SalesReturn'),
	                        (37, 1, 'Default Account For Bank'),
                            (38, 1, 'Salaries'),
                            (39, 31, 'Stock Settlement');

                        CREATE TABLE IF NOT EXISTS `tbl_company` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `descriptions` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `phone1` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `phone2` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `gmail` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `mobile_number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `website` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `address` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `trn_no` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `country_id` int NOT NULL DEFAULT(0),
                          `logoComp` longblob,
                          `stampComp` longblob,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_corporate_tax_configration` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `corporateTax_no` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `trn_issue_date` date DEFAULT NULL,
                          `corporatetax_start_date` date DEFAULT NULL,
                          `corporatetax_end_date` date DEFAULT NULL,
                          `corporatetax_due_date` date DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_cost_center` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` int DEFAULT NULL,
                          `name` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_country` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        INSERT INTO `tbl_country` (`id`, `name`) VALUES
	                        (1, 'Albania'),
	                        (2, 'Algeria'),
	                        (3, 'Andorra'),
	                        (4, 'Angola'),
	                        (5, 'Argentina'),
	                        (6, 'Armenia'),
	                        (7, 'Australia'),
	                        (8, 'Austria'),
	                        (9, 'Azerbaijan'),
	                        (10, 'The Bahamas'),
	                        (11, 'Bahrain'),
	                        (12, 'Bangladesh'),
	                        (13, 'Barbados'),
	                        (14, 'Belarus'),
	                        (15, 'Belgium'),
	                        (16, 'Belize'),
	                        (17, 'Benin'),
	                        (18, 'Bhutan'),
	                        (19, 'Bolivia'),
	                        (20, 'Bosnia & Herzegovina'),
	                        (21, 'Botswana'),
	                        (22, 'Brazil'),
	                        (23, 'Brunei'),
	                        (24, 'Bulgaria'),
	                        (25, 'Burkina Faso'),
	                        (26, 'Burundi'),
	                        (27, 'Cambodia'),
	                        (28, 'Cameroon'),
	                        (29, 'Canada'),
	                        (30, 'Cape Verde'),
	                        (31, 'Chile'),
	                        (32, 'China'),
	                        (33, 'Colombia'),
	                        (34, 'Cuba'),
	                        (35, 'Denmark'),
	                        (36, 'Egypt'),
	                        (37, 'France'),
	                        (38, 'Germany'),
	                        (39, 'India'),
	                        (40, 'Italy'),
	                        (41, 'Japan'),
	                        (42, 'Mexico'),
	                        (43, 'Nigeria'),
	                        (44, 'Russia'),
	                        (45, 'Saudi Arabia'),
	                        (46, 'South Africa'),
	                        (47, 'Spain'),
	                        (48, 'Turkey'),
	                        (49, 'United Arab Emirates'),
	                        (50, 'United Kingdom'),
	                        (51, 'United States'),
	                        (52, 'Vietnam'),
	                        (53, 'Zimbabwe');

                        CREATE TABLE IF NOT EXISTS `tbl_credit_note` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `credit_account` int DEFAULT '0',
                          `debit_account` int NOT NULL DEFAULT '0',
                          `type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `invoice_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `amount` decimal(20,2) DEFAULT '0.00',
                          `vat` decimal(20,2) DEFAULT '0.00',
                          `total` decimal(20,2) DEFAULT '0.00',
                          `description` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `created_date` date DEFAULT NULL,
                          `created_by` int NOT NULL DEFAULT '0',
                          `modified_by` int NOT NULL DEFAULT '0',
                          `modified_date` date DEFAULT NULL,
                          `state` int NOT NULL DEFAULT '0',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_credit_note_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `ref_id` int NOT NULL DEFAULT '0',
                          `invoice_id` int NOT NULL DEFAULT '0',
                          `inv_no` varchar(50) COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `invoice_date` date DEFAULT NULL,
                          `invoice_type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `total` decimal(20,2) NOT NULL DEFAULT '0.00',
                          `vat` decimal(20,2) NOT NULL DEFAULT '0.00',
                          `amount` decimal(20,2) NOT NULL DEFAULT '0.00',
                          `balance` decimal(20,2) NOT NULL DEFAULT '0.00',
                          `remaining` decimal(20,2) NOT NULL DEFAULT '0.00',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_customer` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` int NOT NULL DEFAULT '0',
                          `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Cat_id` int DEFAULT NULL,
                          `Balance` decimal(20,4) DEFAULT NULL,
                          `date` date DEFAULT NULL,
                          `main_phone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `work_phone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `mobile` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `ccemail` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `website` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `country` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `city` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `region` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `building_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `account_id` int DEFAULT NULL,
                          `trn` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `facilty_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `active` int DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          `state` int DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          `project_site` VARCHAR(150) DEFAULT '' COLLATE 'utf8mb4_general_ci',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_customer_category` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_damage` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date NOT NULL,
                          `warehouse_id` int NOT NULL DEFAULT '0',
                          `reference_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `reported_by` int NOT NULL DEFAULT '0',
                          `damage_reason` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `total` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `account_id` int NOT NULL DEFAULT '0',
                          `created_by` int NOT NULL DEFAULT '0',
                          `created_date` date NOT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_damage_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `damage_id` int NOT NULL,
                          `item_id` int NOT NULL,
                          `qty` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `cost_price` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `total` decimal(20,4) NOT NULL DEFAULT '0.000',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_debit_note` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `credit_account` int DEFAULT '0',
                          `debit_account` int NOT NULL DEFAULT '0',
                          `type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `invoice_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `amount` decimal(20,2) DEFAULT '0.00',
                          `vat` decimal(20,2) DEFAULT '0.00',
                          `total` decimal(20,2) DEFAULT '0.00',
                          `description` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `created_date` date DEFAULT NULL,
                          `created_by` int NOT NULL DEFAULT '0',
                          `modified_by` int NOT NULL DEFAULT '0',
                          `modified_date` date DEFAULT NULL,
                          `state` int NOT NULL DEFAULT '0',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_debit_note_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `ref_id` int NOT NULL DEFAULT '0',
                          `invoice_id` int NOT NULL DEFAULT '0',
                          `inv_no` varchar(50) COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `invoice_date` date DEFAULT NULL,
                          `invoice_type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `amount` decimal(20,2) NOT NULL DEFAULT '0.00',
                          `vat` decimal(20,2) NOT NULL DEFAULT '0.00',
                          `total` decimal(20,2) NOT NULL DEFAULT '0.00',
                          `balance` decimal(20,2) NOT NULL DEFAULT '0.00',
                          `remaining` decimal(20,2) NOT NULL DEFAULT '0.00',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_departments` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Department` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_employee` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` int NOT NULL DEFAULT '0',
                          `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `city_id` int DEFAULT NULL,
                          `address` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `phone` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `birth_day` date DEFAULT NULL,
                          `Social_Status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Social_Insurance_Number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Email` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `EmergencyName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `EmergencyAddress` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `EmergencyPhone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Relation` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `BasicSalary` decimal(10,2) DEFAULT NULL,
                          `HousingAllowance` decimal(10,2) DEFAULT NULL,
                          `TransportationAllowance` decimal(10,2) DEFAULT NULL,
                          `Other` decimal(10,2) DEFAULT NULL,
                          `bank_id` int DEFAULT NULL,
                          `Iban_Number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Bank_account_Number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `EmiratesIDFileNumber` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `EmiratesIDIssuingAuthority` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `EmiratesIDIssueDate` date DEFAULT NULL,
                          `EmiratesIDExpiryDate` date DEFAULT NULL,
                          `PassportNumber` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `CountryOfIssue` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `PassportIssueDate` date DEFAULT NULL,
                          `PassportExpiryDate` date DEFAULT NULL,
                          `WorkContractNumber` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `WorkContractType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Position_id` int DEFAULT NULL,
                          `Department_id` int DEFAULT NULL,
                          `WorkDays` int DEFAULT NULL,
                          `workinghours` int DEFAULT NULL,
                          `ContractIssueDate` date DEFAULT NULL,
                          `ContractExpiryDate` date DEFAULT NULL,
                          `ResidencyFileNumber` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `ResidencyIssuingAuthority` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `ResidencyIssueDate` date DEFAULT NULL,
                          `ResidencyExpiryDate` date DEFAULT NULL,
                          `account_id` int DEFAULT NULL,
                          `Accrued_Salaries_id` int DEFAULT NULL,
                          `Employee_Recivable_id` int DEFAULT NULL,
                          `Acroal_Leave_Salary_id` int DEFAULT NULL,
                          `Gratuit_id` int DEFAULT NULL,
                          `Petty_Cash_id` int DEFAULT NULL,
                          `active` int NOT NULL DEFAULT '0',
                          `state` int NOT NULL DEFAULT '0',
                          `sRole` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_end_of_service` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `code` int DEFAULT NULL,
                          `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Reference` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `description` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `debit` decimal(20,6) DEFAULT NULL,
                          `leave_days` decimal(20,6) DEFAULT NULL,
                          `credit` decimal(20,6) DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_final_settlement` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Date` date DEFAULT NULL,
                          `emp_id` int DEFAULT NULL,
                          `DateCommencement` date DEFAULT NULL,
                          `DateLastWork` date DEFAULT NULL,
                          `TotalSalary` decimal(20,2) DEFAULT NULL,
                          `OtherAdditions` decimal(20,2) DEFAULT NULL,
                          `TotalAdditions` decimal(20,2) DEFAULT NULL,
                          `Payments` decimal(20,2) DEFAULT NULL,
                          `OtherDeductions` decimal(20,2) DEFAULT NULL,
                          `TotalDeductions` decimal(20,2) DEFAULT NULL,
                          `NetAccruals` decimal(20,2) DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_fixed_assets` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `name` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `brand` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `category_id` int DEFAULT NULL,
                          `model` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `supplier` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `status` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `invoice_number` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `purchase_date` date DEFAULT NULL,
                          `end_date` date DEFAULT NULL,
                          `depreciation_life` int DEFAULT NULL,
                          `purchase_price` decimal(20,6) DEFAULT NULL,
                          `debit_account_id` int DEFAULT NULL,
                          `credit_account_id` int DEFAULT NULL,
                          `expence_account_id` int DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int DEFAULT '0',
                          `manufacture` int DEFAULT '0',
                          `manufactureStatus` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_fixed_assets_category` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `category_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
	                      `assets_account_id` INT(10) NOT NULL DEFAULT '0',
	                      `depreciation_account_id` INT(10) NOT NULL DEFAULT '0',
	                      `expence_account_id` INT(10) NOT NULL DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
                        
                       CREATE TABLE IF NOT EXISTS `tbl_general_settings` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `value` int NOT NULL DEFAULT '0',
                          `description` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `status` int DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        INSERT INTO `tbl_general_settings` (`id`, `name`, `value`, `description`, `status`) VALUES
	                        (1, 'ALL ITEMS IN SALES', 0, '', 0),
	                        (2, 'ALL ITEMS IN PURCHASE', 0, '', 0),
	                        (3, 'ALLOW ITEM WITHOUT QTY', 0, '', 0),
	                        (4, 'DEFAULT TAX PERCENTAGE', 1, '', 1),
                            (5, 'PRINT KITCHEN KOT', 0, '' ,1),
                            (6, 'PRINT BILL', 0, '' ,1),
                            (7, 'PROJECT OPTION', 0, '' ,0),
                            (8, 'ENABLE PETTYCASH APPROVAL', 0, '', 0);

                        CREATE TABLE `tbl_items` (
	                    `id` INT(10) NOT NULL AUTO_INCREMENT,
	                    `code` VARCHAR(50) NOT NULL DEFAULT '0' COLLATE 'utf8mb4_general_ci',
	                    `type` VARCHAR(50) NOT NULL DEFAULT '0' COLLATE 'utf8mb4_general_ci',
	                    `warehouse_id` INT(10) NOT NULL DEFAULT '0',
	                    `name` VARCHAR(500) NOT NULL DEFAULT '0' COLLATE 'utf8mb4_general_ci',
	                    `unit_id` INT(10) NOT NULL DEFAULT '0',
	                    `barcode` VARCHAR(70) NOT NULL DEFAULT '0' COLLATE 'utf8mb4_general_ci',
	                    `cost_price` DECIMAL(20,4) NOT NULL DEFAULT '0.000',
	                    `cogs_account_id` INT(10) NOT NULL DEFAULT '0',
	                    `vendor_id` INT(10) NOT NULL DEFAULT '0',
	                    `sales_price` DECIMAL(20,4) NOT NULL DEFAULT '0.000',
	                    `tax_code_id` INT(10) NOT NULL DEFAULT '0',
	                    `income_account_id` INT(10) NOT NULL DEFAULT '0',
	                    `asset_account_id` INT(10) NOT NULL DEFAULT '0',
	                    `min_amount` DECIMAL(20,4) NOT NULL DEFAULT '0.000',
	                    `max_amount` DECIMAL(20,4) NOT NULL DEFAULT '0.000',
	                    `on_hand` DECIMAL(20,4) NOT NULL DEFAULT '0.000',
	                    `total_value` DECIMAL(20,6) NOT NULL DEFAULT '0.000000',
	                    `date` DATE NOT NULL,
	                    `img` BLOB NULL DEFAULT NULL,
                        `ItemImg` LONGBLOB NULL DEFAULT NULL,
	                    `active` INT(10) NOT NULL DEFAULT '0',
	                    `method` VARCHAR(50) NOT NULL DEFAULT '0' COLLATE 'utf8mb4_general_ci',
	                    `state` INT(10) NOT NULL DEFAULT '0',
	                    `created_By` INT(10) NOT NULL DEFAULT '0',
	                    `created_date` DATE NOT NULL,
	                    `deleted_by` INT(10) NOT NULL DEFAULT '0',
	                    `category_id` INT(10) NULL DEFAULT NULL,
	                    `posItem` INT(10) NULL DEFAULT NULL,
                        `item_type` VARCHAR(50) NOT NULL DEFAULT '0' COLLATE 'utf8mb4_general_ci',
	                    PRIMARY KEY (`id`) USING BTREE
                    )
                    COLLATE='utf8mb4_general_ci' ENGINE=InnoDB ;

                        CREATE TABLE IF NOT EXISTS `tbl_items_unit` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `item_id` int NOT NULL DEFAULT '0',
                          `unit_id` int NOT NULL DEFAULT '0',
                          `factor` int NOT NULL DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_items_warehouse` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `warehouse_id` int DEFAULT NULL,
                          `item_id` int DEFAULT NULL,
                          `qty` decimal(20,6) DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_item_assembly` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `assembly_id` int DEFAULT NULL,
                          `item_id` int DEFAULT NULL,
                          `qty` decimal(20,4) DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_item_category` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '0',
                          `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_item_stock_settlement` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `date` date DEFAULT NULL,
                          `warehouse_id` int DEFAULT NULL,
                          `total_plus` decimal(20,6) DEFAULT NULL,
                          `total_minus` decimal(20,6) DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_item_stock_settlement_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `settle_id` int DEFAULT NULL,
                          `item_id` int DEFAULT NULL,
                          `on_hand` decimal(20,4) DEFAULT NULL,
                          `price` decimal(20,4) DEFAULT NULL,
                          `new_on_hand` decimal(20,4) DEFAULT NULL,
                          `qty` decimal(20,4) DEFAULT NULL,
                          `minusamount` decimal(20,4) DEFAULT NULL,
                          `plusamount` decimal(20,4) DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_item_transaction` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `reference` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `item_id` int DEFAULT NULL,
                          `cost_price` decimal(20,8) DEFAULT NULL,
                          `qty_in` decimal(20,4) DEFAULT NULL,
                          `sales_price` decimal(20,4) DEFAULT NULL,
                          `qty_out` decimal(20,4) DEFAULT NULL,
                          `qty_inc` decimal(20,4) DEFAULT NULL,
                          `description` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `warehouse_id` INT NOT NULL DEFAULT 0,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_item_warehouse_transaction` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `warehouse_from` int DEFAULT NULL,
                          `warehouse_to` int DEFAULT NULL,
                          `item_id` int DEFAULT NULL,
                          `qty` decimal(20,6) DEFAULT NULL,
                          `description` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_journal_voucher` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `debit` decimal(20,4) DEFAULT NULL,
                          `credit` decimal(20,4) DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_journal_voucher_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date NOT NULL,
                          `debit` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `credit` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `inv_id` int NOT NULL DEFAULT '0',
                          `description` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `partner` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `account_id` int NOT NULL DEFAULT '0',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_leave_salary` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `code` int DEFAULT NULL,
                          `name` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Reference` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `description` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `debit` decimal(20,6) DEFAULT '0.000000',
                          `leave_days` decimal(20,6) DEFAULT NULL,
                          `credit` decimal(20,6) DEFAULT '0.000000',
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_ledger` (
                          `ledger_id` int NOT NULL AUTO_INCREMENT,
                          `code` int NOT NULL DEFAULT '0',
                          `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `balance` decimal(20,4) DEFAULT '0.0000',
                          `phone` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `mobile` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `website` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `country` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '0',
                          `city` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '0',
                          `region` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `active` int NOT NULL DEFAULT '0',
                          `created_by` int DEFAULT '0',
                          `created_date` date DEFAULT NULL,
                          `state` int DEFAULT '0',
                          `entity_type` enum('vendor','bank','level4','customer','employee') CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          PRIMARY KEY (`ledger_id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_loan` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `LoanDate` date DEFAULT NULL,
                          `EmployeeID` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `EmployeeName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `RequestAmount` decimal(20,6) DEFAULT NULL,
                          `Installments` int DEFAULT NULL,
                          `StartDate` date DEFAULT NULL,
                          `EndDate` date DEFAULT NULL,
                          `loanDates` date DEFAULT NULL,
                          `Months` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Description` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Amount` decimal(20,6) DEFAULT NULL,
                          `debit_account_id` int DEFAULT NULL,
                          `credit_account_id` int DEFAULT NULL,
                          `pay` decimal(20,6) DEFAULT '0.000000',
                          `change` decimal(20,6) DEFAULT '0.000000',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_payment_voucher` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `method` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `amount` decimal(20,4) DEFAULT NULL,
                          `debit_account_id` int DEFAULT NULL,
                          `debit_cost_center_id` int DEFAULT NULL,
                          `description` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `credit_account_id` int DEFAULT NULL,
                          `credit_cost_center_id` int DEFAULT NULL,
                          `bank_id` int DEFAULT NULL,
                          `bank_account_id` int DEFAULT NULL,
                          `book_no` int DEFAULT NULL,
                          `check_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `check_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `check_date` date DEFAULT NULL,
                          `trans_date` date DEFAULT NULL,
                          `trans_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `trans_ref` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_payment_voucher_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `payment_id` int DEFAULT NULL,
                          `hum_id` int DEFAULT NULL,
                          `inv_id` int DEFAULT NULL,
                          `inv_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `total` decimal(20,6) DEFAULT NULL,
                          `payment` decimal(20,6) DEFAULT NULL,
                          `description` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `voucher_type` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `cost_center_id` int DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_petty_cash_card` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `name` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `account_id` int DEFAULT NULL,
                          `mobile` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `whatsapp_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `email` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_petty_cash_category` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_petty_cash_request` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `request_date` date DEFAULT NULL,
                          `request_ref` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Petty_cash_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `amount` decimal(20,6) DEFAULT NULL,
                          `description` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `debit_account_id` int DEFAULT NULL,
                          `credit_account_id` int DEFAULT NULL,
                          `approved_date` date DEFAULT NULL,
                          `state` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `pay` decimal(20,6) DEFAULT NULL,
                          `change` decimal(20,6) DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_petty_cash_submition` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `name` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `amount` decimal(20,6) DEFAULT NULL,
                          `total_before_vat` decimal(20,6) DEFAULT NULL,
                          `total_vat` decimal(20,6) DEFAULT NULL,
                          `net_amount` decimal(20,6) DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_petty_cash_submition_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `petty_id` int NOT NULL DEFAULT '0',
                          `date` date DEFAULT NULL,
                          `account_id` int DEFAULT NULL,
                          `category` int DEFAULT NULL,
                          `cost_center_id` int DEFAULT NULL,
                          `amount` decimal(20,6) DEFAULT NULL,
                          `vat` decimal(20,6) DEFAULT NULL,
                          `total` decimal(20,6) DEFAULT NULL,
                          `note` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `state` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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

                        CREATE TABLE IF NOT EXISTS `tbl_position` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `department_id` int DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_prepaid_expense` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `category_id` int DEFAULT NULL,
                          `debit_account_id` int DEFAULT NULL,
                          `credit_account_id` int DEFAULT NULL,
                          `start_date` date DEFAULT NULL,
                          `end_date` date DEFAULT NULL,
                          `amount` decimal(20,4) DEFAULT NULL,
                          `fee` decimal(20,4) DEFAULT NULL,
                          `total` decimal(20,4) DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_prepaid_expense_category` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_print_config` (
                          `table_border` int DEFAULT '0',
                          `company_name` int DEFAULT '0',
                          `portrait` int DEFAULT '0',
                          `landscape` int DEFAULT '0',
                          `orientation` int DEFAULT '1'
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_purchase` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date NOT NULL,
                          `vendor_id` int NOT NULL DEFAULT '0',
                          `invoice_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `warehouse_id` int NOT NULL DEFAULT '0',
                          `po_num` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `bill_to` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `city` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `sales_man` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `ship_date` date DEFAULT NULL,
                          `ship_via` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `ship_to` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_method` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `account_cash_id` int NOT NULL DEFAULT '0',
                          `payment_terms` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_date` date NOT NULL,
                          `total` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `vat` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `net` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `pay` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `change` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `created_by` int NOT NULL DEFAULT '0',
                          `created_date` date NOT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int NOT NULL DEFAULT '0',
                          `purchase_type` VARCHAR(50) NOT NULL DEFAULT '0' COLLATE 'utf8mb4_general_ci',
                          `fixed_asset_category_id` int NOT NULL DEFAULT '0',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_purchase_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `purchase_id` int DEFAULT NULL,
                          `item_id` int DEFAULT NULL,
                          `qty` decimal(20,4) DEFAULT NULL,
                          `cost_price` decimal(20,4) DEFAULT NULL,
                          `price` decimal(20,4) DEFAULT NULL,
                          `vatp` decimal(20,4) DEFAULT '0.000',
                          `vat` int DEFAULT NULL,
                          `total` decimal(20,4) DEFAULT NULL,
                          `discount` decimal(20,4) DEFAULT NULL,
                          `cost_center_id` int DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_purchase_order` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date NOT NULL,
                          `vendor_id` int NOT NULL DEFAULT '0',
                          `invoice_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `warehouse_id` int NOT NULL DEFAULT '0',
                          `po_num` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `bill_to` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `city` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `sales_man` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `ship_date` date DEFAULT NULL,
                          `ship_via` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `ship_to` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_method` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `account_cash_id` int NOT NULL DEFAULT '0',
                          `payment_terms` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_date` date NOT NULL,
                          `total` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `vat` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `net` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `pay` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `change` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `created_by` int NOT NULL DEFAULT '0',
                          `created_date` date NOT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int NOT NULL DEFAULT '0',
                          `tranfer_status` int NOT NULL DEFAULT '0',
                          `purchase_id` int NOT NULL DEFAULT '0',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_purchase_order_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `purchase_id` int DEFAULT NULL,
                          `item_id` int DEFAULT NULL,
                          `qty` decimal(20,4) DEFAULT NULL,
                          `cost_price` decimal(20,4) DEFAULT '0.000',
                          `price` decimal(20,4) DEFAULT '0.000',
                          `vatp` decimal(20,4) DEFAULT '0.000',
                          `vat` int DEFAULT NULL,
                          `total` decimal(20,4) DEFAULT NULL,
                          `discount` decimal(20,4) DEFAULT NULL,
                          `cost_center_id` int DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_purchase_return` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date NOT NULL,
                          `vendor_id` int NOT NULL DEFAULT '0',
                          `invoice_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `warehouse_id` int NOT NULL DEFAULT '0',
                          `po_num` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `bill_to` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `city` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `sales_man` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `ship_date` date DEFAULT NULL,
                          `ship_via` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `ship_to` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_method` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `account_cash_id` int NOT NULL DEFAULT '0',
                          `payment_terms` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_date` date NOT NULL,
                          `total` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `vat` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `net` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `pay` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `change` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `created_by` int NOT NULL DEFAULT '0',
                          `created_date` date NOT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int NOT NULL DEFAULT '0',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_purchase_return_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `purchase_id` int DEFAULT NULL,
                          `item_id` int DEFAULT NULL,
                          `qty` decimal(20,4) DEFAULT NULL,
                          `cost_price` decimal(20,4) DEFAULT NULL,
                          `price` decimal(20,4) DEFAULT NULL,
                          `vatp` decimal(20,4) DEFAULT '0.000',
                          `vat` int DEFAULT NULL,
                          `total` decimal(20,4) DEFAULT NULL,
                          `discount` decimal(20,4) DEFAULT NULL,
                          `cost_center_id` int DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_receipt_voucher` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `method` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `amount` decimal(20,6) DEFAULT NULL,
                          `hum_id` int DEFAULT NULL,
                          `credit_account_id` int DEFAULT NULL,
                          `credit_cost_center_id` int DEFAULT NULL,
                          `description` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `debit_account_id` int DEFAULT NULL,
                          `debit_cost_center_id` int DEFAULT NULL,
                          `bank_account_id` int DEFAULT NULL,
                          `book_no` int DEFAULT NULL,
                          `bank_id` int DEFAULT NULL,
                          `bank_account` int DEFAULT NULL,
                          `bank_code` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `check_name` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `check_no` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `check_date` date DEFAULT NULL,
                          `trans_date` date DEFAULT NULL,
                          `trans_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `trans_ref` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          `state` int DEFAULT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_receipt_voucher_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `payment_id` int DEFAULT NULL,
                          `inv_id` int DEFAULT NULL,
                          `hum_id` int DEFAULT NULL,
                          `inv_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `total` decimal(20,2) DEFAULT '0.00',
                          `payment` decimal(20,2) DEFAULT '0.00',
                          `description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `cost_center_id` int DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_rmsdetails` (
                          `DetailID` int NOT NULL AUTO_INCREMENT,
                          `MainID` int DEFAULT NULL,
                          `proID` int DEFAULT NULL,
                          `qty` decimal(65,2) DEFAULT NULL,
                          `Price` decimal(65,2) DEFAULT NULL,
                          `amount` decimal(65,2) DEFAULT NULL,
                          PRIMARY KEY (`DetailID`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_rmsmain` (
                          `MainId` int NOT NULL AUTO_INCREMENT,
                          `aDate` date DEFAULT NULL,
                          `time` varchar(50) DEFAULT NULL,
                          `tableName` varchar(50) DEFAULT NULL,
                          `waiterName` varchar(50) DEFAULT NULL,
                          `status` varchar(50) DEFAULT NULL,
                          `orderType` varchar(50) DEFAULT NULL,
                          `Total` decimal(65,2) DEFAULT NULL,
                          `received` decimal(65,2) DEFAULT NULL,
                          `changetot` decimal(65,2) DEFAULT NULL,
                          `DriverID` int DEFAULT NULL,
                          `CusTName` varchar(50) DEFAULT NULL,
                          `CustPhone` varchar(50) DEFAULT NULL,
                          `TdOrderNo` int DEFAULT NULL,
                          `UserSale` varchar(50) DEFAULT NULL,
                          `PaidSt` varchar(50) DEFAULT NULL,
                          PRIMARY KEY (`MainId`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_rmstables` (
                          `tid` int NOT NULL AUTO_INCREMENT,
                          `tname` varchar(50) NOT NULL,
                          PRIMARY KEY (`tid`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_salary` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `employee_name` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `date` date DEFAULT NULL,
                          `month` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `year` int DEFAULT NULL,
                          `salary` decimal(20,6) DEFAULT NULL,
                          `pay` decimal(20,6) DEFAULT NULL,
                          `change` decimal(20,6) DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_salary_adjustments` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `name` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `adjustment_type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `description` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `amount` decimal(10,6) DEFAULT NULL,
                          `date` date DEFAULT NULL,
                          `ref_id` int NOT NULL DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_sales` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date NOT NULL,
                          `customer_id` int NOT NULL DEFAULT '0',
                          `invoice_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `warehouse_id` int NOT NULL DEFAULT '0',
                          `po_num` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `bill_to` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `city` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `sales_man` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `ship_date` date DEFAULT NULL,
                          `ship_via` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `ship_to` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_method` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `account_cash_id` int NOT NULL DEFAULT '0',
                          `payment_terms` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_date` date NOT NULL,
                          `total` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `vat` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `net` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `pay` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `change` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `created_by` int NOT NULL DEFAULT '0',
                          `created_date` date NOT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int NOT NULL DEFAULT '0',
                          `discount` decimal(20,6) NOT NULL DEFAULT '0.000000',
                          `cost_center_id` decimal(20,6) NOT NULL DEFAULT '0.000000',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_sales_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `sales_id` int DEFAULT NULL,
                          `item_id` int DEFAULT NULL,
                          `qty` decimal(20,4) DEFAULT NULL,
                          `cost_price` decimal(20,4) DEFAULT NULL,
                          `price` decimal(20,4) DEFAULT NULL,
                          `vatp` decimal(20,4) DEFAULT '0.000',
                          `vat` int DEFAULT NULL,
                          `total` decimal(20,4) DEFAULT NULL,
                          `discount` decimal(20,4) DEFAULT NULL,
                          `cost_center_id` int DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_sales_order` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date NOT NULL,
                          `customer_id` int NOT NULL DEFAULT '0',
                          `invoice_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `warehouse_id` int NOT NULL DEFAULT '0',
                          `inv_id` int NOT NULL DEFAULT '0',
                          `po_num` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `bill_to` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `city` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `sales_man` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `ship_date` date DEFAULT NULL,
                          `ship_via` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `ship_to` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_method` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `account_cash_id` int NOT NULL DEFAULT '0',
                          `payment_terms` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_date` date NOT NULL,
                          `total` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `vat` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `net` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `pay` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `change` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `created_by` int NOT NULL DEFAULT '0',
                          `created_date` date NOT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int NOT NULL DEFAULT '0',
                          `tranfer_status` int NOT NULL DEFAULT '0',
                          `sales_id` int NOT NULL DEFAULT '0',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_sales_order_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `sales_id` int DEFAULT NULL,
                          `item_id` int DEFAULT NULL,
                          `qty` decimal(20,4) DEFAULT NULL,
                          `cost_price` decimal(20,4) DEFAULT NULL,
                          `price` decimal(20,4) DEFAULT NULL,
                          `discount` decimal(20,4) DEFAULT '0.000',
                          `vatp` decimal(20,4) DEFAULT '0.000',
                          `vat` int DEFAULT NULL,
                          `total` decimal(20,4) DEFAULT NULL,
                          `cost_center_id` int DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_sales_proforma` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date NOT NULL,
                          `customer_id` int NOT NULL DEFAULT '0',
                          `invoice_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `warehouse_id` int NOT NULL DEFAULT '0',
                          `po_num` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `bill_to` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `city` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `sales_man` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `ship_date` date DEFAULT NULL,
                          `ship_via` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `ship_to` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_method` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `account_cash_id` int NOT NULL DEFAULT '0',
                          `payment_terms` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_date` date NOT NULL,
                          `total` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `vat` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `net` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `pay` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `change` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `created_by` int NOT NULL DEFAULT '0',
                          `created_date` date NOT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int NOT NULL DEFAULT '0',
                          `tranfer_status` int DEFAULT '0',
                          `sales_id` int DEFAULT '0',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_sales_proforma_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `sales_id` int DEFAULT NULL,
                          `item_id` int DEFAULT NULL,
                          `qty` decimal(20,4) DEFAULT NULL,
                          `cost_price` decimal(20,4) DEFAULT NULL,
                          `price` decimal(20,4) DEFAULT NULL,
                          `discount` decimal(20,4) DEFAULT '0.000',
                          `vatp` decimal(20,4) DEFAULT '0.000',
                          `vat` int DEFAULT NULL,
                          `cost_center_id` int DEFAULT NULL,
                          `total` decimal(20,4) DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_sales_quotation` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date NOT NULL,
                          `customer_id` int NOT NULL DEFAULT '0',
                          `invoice_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `warehouse_id` int NOT NULL DEFAULT '0',
                          `po_num` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `bill_to` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `city` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `sales_man` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `ship_date` date DEFAULT NULL,
                          `ship_via` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `ship_to` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_method` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `account_cash_id` int NOT NULL DEFAULT '0',
                          `payment_terms` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_date` date NOT NULL,
                          `total` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `vat` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `net` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `pay` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `change` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `created_by` int NOT NULL DEFAULT '0',
                          `created_date` date NOT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int NOT NULL DEFAULT '0',
                          `tranfer_status` int DEFAULT '0',
                          `sales_id` int DEFAULT '0',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_sales_quotation_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `sales_id` int DEFAULT NULL,
                          `item_id` int DEFAULT NULL,
                          `qty` decimal(20,4) DEFAULT NULL,
                          `cost_price` decimal(20,4) DEFAULT NULL,
                          `price` decimal(20,4) DEFAULT NULL,
                          `discount` decimal(20,4) DEFAULT '0.000',
                          `vatp` decimal(20,4) DEFAULT '0.000',
                          `vat` int DEFAULT NULL,
                          `cost_center_id` int DEFAULT NULL,
                          `total` decimal(20,4) DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_sales_return` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date NOT NULL,
                          `customer_id` int NOT NULL DEFAULT '0',
                          `invoice_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `warehouse_id` int NOT NULL DEFAULT '0',
                          `po_num` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `bill_to` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `city` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `sales_man` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `ship_date` date DEFAULT NULL,
                          `ship_via` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `ship_to` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_method` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `account_cash_id` int NOT NULL DEFAULT '0',
                          `payment_terms` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `payment_date` date NOT NULL,
                          `total` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `vat` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `net` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `pay` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `change` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `created_by` int NOT NULL DEFAULT '0',
                          `created_date` date NOT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int NOT NULL DEFAULT '0',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_sales_return_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `sales_id` int DEFAULT NULL,
                          `item_id` int DEFAULT NULL,
                          `qty` decimal(20,4) DEFAULT NULL,
                          `cost_price` decimal(20,4) DEFAULT NULL,
                          `discount` decimal(20,4) DEFAULT NULL,
                          `price` decimal(20,4) DEFAULT NULL,
                          `vatp` decimal(20,4) DEFAULT '0.000',
                          `vat` int DEFAULT NULL,
                          `cost_center_id` int DEFAULT NULL,
                          `total` decimal(20,4) DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_sec_roles` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                       INSERT INTO `tbl_sec_roles` (`id`, `name`) VALUES
                            (1, 'Admin'),
                            (2, 'Manager'),
                            (3, 'Accountant'),
                            (4, 'Sales'),
                            (5, 'Purchaser'),
                            (6, 'Inventory Manager'),
                            (7, 'HR'),
                            (8, 'Cashier'),
                            (9, 'Auditor');

                        CREATE TABLE IF NOT EXISTS `tbl_sec_role_form` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `role_id` int DEFAULT NULL,
                          `form_id` int DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_sec_users` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `user_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          `PasswordHash` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          `salt` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          `emp_id` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          `first_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `last_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Role_Id` int NOT NULL,
                          `active` int NOT NULL,
                          `state` int NOT NULL,
                          `password_updated_by` int DEFAULT NULL,
                          `password_last_update` date DEFAULT NULL,
                          PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS tbl_deleted_records (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            table_name VARCHAR(100),
                            record_data TEXT,
                            deleted_by INT,
                            deleted_at DATETIME DEFAULT CURRENT_TIMESTAMP
                        );

                        CREATE TABLE IF NOT EXISTS `tbl_setting_attendance` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `day` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `timein` time DEFAULT NULL,
                          `timeout` time DEFAULT NULL,
                          `state` int DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_setting_deduction_config` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `latearrivaldeduction` decimal(20,2) DEFAULT NULL,
                          `delaytime` time DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_setting_default_account` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `level4_id` int NOT NULL DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_set_main_menu` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        INSERT INTO `tbl_set_main_menu` (`id`, `name`) VALUES
	                        (1, 'Accountant'),
	                        (2, 'Banking'),
	                        (3, 'Inventory'),
	                        (4, 'Customers && Sales'),
	                        (5, 'Vendors && Purchases'),
	                        (8, 'Setting'),
	                        (9, 'HR');

                        CREATE TABLE IF NOT EXISTS `tbl_set_menu_forms` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `menu_id` int DEFAULT NULL,
                          `form_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `form_text` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `params` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `seq` int DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        INSERT INTO `tbl_set_menu_forms` (`id`, `menu_id`, `form_name`, `form_text`, `params`, `seq`) VALUES
	                        (1, 1, 'MasterChartOfAccount', 'Chart of Account', 'MainForm', 1),
	                        (2, 9, 'MasterEmployee', 'Employee Center', 'MainForm', 2),
	                        (3, 4, 'MasterCustomer', 'Customer Center', 'MainForm', 1),
	                        (4, 5, 'MasterVendor', 'Vendor Center', 'MainForm', 1),
	                        (5, 4, 'MasterSales', 'Sales Center', 'MainForm', 1),
	                        (6, 8, 'MasterSetting', 'Setting', 'MainForm', 1),
	                        (7, 2, 'MasterBanking', 'Bank Center', 'MainForm', 1),
	                        (8, 5, 'MasterPurchases', 'Purchases Center', 'MainForm', 3),
	                        (9, 1, 'MasterPaymentVoucher', 'Payment Voucher', 'MainForm', 3),
	                        (10, 3, 'MasterInventory', 'Inventory', 'MainForm', 1),
	                        (11, 2, 'MasterBankCard', 'Bank Card Center', 'MainForm', 2),
	                        (13, 1, 'MasterReciptVoucher', 'Receipt Voucher', 'MainForm', 3),
	                        (14, 3, 'MasterInventoryReport', 'Inventory Report', 'MainForm', 3),
	                        (22, 9, 'MasterAttendanceSheet', 'Attendance Sheet Center', 'MainForm', 2),
	                        (23, 9, 'frmSalarySheet', 'Salary Sheet', 'MainForm', 2),
	                        (24, 9, 'frmEndOfService', 'End Of Services', 'MainForm', 2),
	                        (25, 9, 'frmLeaveSalary', 'Leave Salary', 'MainForm', 2),
	                        (26, 9, 'MasterFinalSettlement', 'Final Settlement', 'MainForm', 2),
	                        (27, 9, 'frmViewLoan', 'Loan', 'MainForm', 2),
	                        (28, 1, 'MasterCostCenter', 'Cost Center', 'MainForm', 1),
	                        (29, 1, 'MasterPrepaidExpense', 'Prepaid Expense', 'MainForm', 1),
	                        (30, 1, 'MasterFixedAssets', 'Fixed Assets', 'MainForm', 1),
	                        (31, 2, 'MasterCheque', 'Cheques', 'MainForm', 2),
	                        (32, 1, 'MasterPettyCash', 'Petty Cash', 'MainForm', 1);

                        CREATE TABLE IF NOT EXISTS `tbl_sub_cost_center` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` int DEFAULT NULL,
                          `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `main_id` int DEFAULT NULL,
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_tax` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `value` int DEFAULT NULL,
                          `description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `state` int DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        INSERT INTO `tbl_tax` (`id`, `name`, `value`, `description`, `state`) VALUES
	                        (1, 'TAX', 5, '', 0),
	                        (2, 'TAX', 0, '', 0);

                        CREATE TABLE IF NOT EXISTS `tbl_tools` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `tool_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `is_selected` int DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        INSERT INTO `tbl_tools` (`id`, `tool_name`, `is_selected`) VALUES
	                        (1, 'Chart Of Account', 0),
	                        (2, 'Cost Center', 0),
	                        (3, 'Fixed Assets', 0),
	                        (4, 'Payment Voucher', 0),
	                        (5, 'Receipt Voucher', 0),
	                        (6, 'Petty Cash', 0),
	                        (7, 'Prepaid Expense', 0),
	                        (8, 'Bank Card', 0),
	                        (9, 'Bank Register', 0),
	                        (10, 'Cheques', 0),
	                        (11, 'PDC', 0),
	                        (12, 'Customer Center', 0),
	                        (13, 'Sales Center', 0),
	                        (14, 'Vendor Center', 0),
	                        (15, 'Purchases Center', 0),
	                        (16, 'Employee Center', 0),
	                        (17, 'Attendance Sheet Center', 0),
	                        (18, 'Salary Sheet', 0);

                        CREATE TABLE IF NOT EXISTS `tbl_transaction` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `account_id` int DEFAULT NULL,
                          `debit` decimal(20,4) DEFAULT NULL,
                          `credit` decimal(20,4) DEFAULT NULL,
                          `transaction_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `hum_id` int DEFAULT '0',
                          `type` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `t_type` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `description` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          `modified_by` int DEFAULT NULL,
                          `modified_date` date DEFAULT NULL,
                          `state` int DEFAULT NULL,
                          `voucher_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_unit` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_vat_configration` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `registration_no` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `TRNIssue_date` date DEFAULT NULL,
                          `quarter_one_start_date` date DEFAULT NULL,
                          `quarter_one_end_date` date DEFAULT NULL,
                          `quarter_one_due_date` date DEFAULT NULL,
                          `quarter_two_start_date` date DEFAULT NULL,
                          `quarter_two_end_date` date DEFAULT NULL,
                          `quarter_two_due_date` date DEFAULT NULL,
                          `quarter_three_start_date` date DEFAULT NULL,
                          `quarter_three_end_date` date DEFAULT NULL,
                          `quarter_three_due_date` date DEFAULT NULL,
                          `quarter_four_start_date` date DEFAULT NULL,
                          `quarter_four_end_date` date DEFAULT NULL,
                          `quarter_four_due_date` date DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_vendor` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` int NOT NULL DEFAULT '0',
                          `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `Cat_id` int DEFAULT NULL,
                          `Balance` decimal(20,4) DEFAULT NULL,
                          `date` date DEFAULT NULL,
                          `main_phone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `work_phone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `mobile` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `ccemail` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `website` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `country` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `city` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `region` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `building_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `account_id` int DEFAULT NULL,
                          `trn` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `facilty_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `active` int DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          `state` int DEFAULT NULL,
                          `type` VARCHAR(150) NULL DEFAULT 'Vendor' COLLATE 'utf8mb4_general_ci',
                          `project_id` INT NOT NULL DEFAULT 0,
                          `project_site` VARCHAR(150) DEFAULT '' COLLATE 'utf8mb4_general_ci',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_vendor_category` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_warehouse` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `emp_id` int DEFAULT NULL,
                          `city` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `building_name` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `account_id` int DEFAULT NULL,
                          `state` int DEFAULT NULL,
                          `created_by` int DEFAULT NULL,
                          `created_date` date DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_cost_center_transaction` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `type` varchar(50) COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `date` datetime DEFAULT NULL,
                          `ref_id` int DEFAULT '0',
                          `debit` decimal(20,4) DEFAULT '0.000',
                          `credit` decimal(20,4) DEFAULT '0.000',
                          `description` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `cost_center_id` int DEFAULT '0',
                          `project_id` INT NOT NULL DEFAULT 0,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE `tbl_item_card_details` (
	                        `id` INT(10) NOT NULL AUTO_INCREMENT,
	                        `itemId` INT(10) NOT NULL DEFAULT '0',
	                        `date` DATE NULL DEFAULT NULL,
	                        `wharehouse_id` INT(10) NOT NULL DEFAULT '0',
	                        `inv_no` VARCHAR(50) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	                        `trans_no` INT(10) NOT NULL DEFAULT '0',
	                        `trans_type` VARCHAR(50) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	                        `description` VARCHAR(500) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	                        `price` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
	                        `qty_in` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
	                        `qty_out` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
	                        `qty_balance` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
	                        `debit` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
	                        `credit` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
	                        `balance` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
	                        `fifo_qty` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
	                        `fifo_cost` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
	                        PRIMARY KEY (`id`) USING BTREE
                        )
                        COLLATE='utf8mb4_general_ci'
                        ENGINE=InnoDB
                        ;

                        CREATE TABLE `tbl_crmcustomer` (
	                        `ID` INT(10) NOT NULL AUTO_INCREMENT,
	                        `LeadName` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	                        `custcode` INT(10) NULL DEFAULT NULL,
	                        `CustName` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	                        `openlvl` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	                        `Stage` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	                        `Date` DATETIME NULL DEFAULT NULL,
	                        `Amount` DECIMAL(15,2) NULL DEFAULT NULL,
	                        `Discription` VARCHAR(1000) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	                        `Assigendto` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	                        `CreateAt` DATETIME NULL DEFAULT NULL,
	                        PRIMARY KEY (`ID`) USING BTREE
                        ) COLLATE='utf8mb4_general_ci' ENGINE=InnoDB ;

                        CREATE TABLE IF NOT EXISTS `tbl_items_boq` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `sr` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `ref_id` int NOT NULL,
                          `type` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          `unit_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `qty` decimal(10,2) DEFAULT NULL,
                          `price` decimal(10,2) DEFAULT NULL,
                          `amount` decimal(10,2) DEFAULT NULL,
                          `length` decimal(10,2) DEFAULT NULL,
                          `width` decimal(10,2) DEFAULT NULL,
                          `thickness` VARCHAR(20) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
                          `note` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_items_boq_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` varchar(100) NOT NULL,
                          `warehouse_id` int NOT NULL,
                          `type` varchar(50) NOT NULL,
                          `category_id` int DEFAULT NULL,
                          `name` varchar(255) NOT NULL,
                          `unit_id` int NOT NULL,
                          `barcode` varchar(100) DEFAULT NULL,
                          `cost_price` decimal(10,2) DEFAULT NULL,
                          `cogs_account_id` int DEFAULT NULL,
                          `vendor_id` int DEFAULT NULL,
                          `sales_price` decimal(10,2) DEFAULT NULL,
                          `income_account_id` int DEFAULT NULL,
                          `asset_account_id` int DEFAULT NULL,
                          `min_amount` decimal(10,2) DEFAULT NULL,
                          `max_amount` decimal(10,2) DEFAULT NULL,
                          `on_hand` decimal(10,2) DEFAULT NULL,
                          `method` varchar(50) DEFAULT NULL,
                          `total_value` decimal(15,2) DEFAULT NULL,
                          `date` date DEFAULT NULL,
                          `img` varchar(255) DEFAULT NULL,
                          `active` tinyint(1) DEFAULT '1',
                          `state` int DEFAULT '0',
                          `created_by` int NOT NULL,
                          `created_date` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
                          `ref_id` int DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_item_assembly_bos` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `assembly_id` int NOT NULL,
                          `item_id` int NOT NULL,
                          `qty` decimal(10,2) NOT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_projects` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `name` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `category` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `description` varchar(350) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `start_date` date DEFAULT NULL,             
                          `end_date` date DEFAULT NULL,
                          `country_id` int NOT NULL DEFAULT '0',
                          `city_id` int NOT NULL DEFAULT '0',
                          `status` VARCHAR(50) DEFAULT 'Planned',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_project_estimate` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `project_id` int NOT NULL DEFAULT '0',
                          `fund_account_id` int NOT NULL DEFAULT '0',
                          `material_cost` decimal(20,4) NOT NULL DEFAULT '0.0000',
                          `labor_cost` decimal(20,4) NOT NULL DEFAULT '0.0000',
                          `equipment_cost` decimal(20,4) NOT NULL DEFAULT '0.0000',
                          `overhead_cost` decimal(20,4) NOT NULL DEFAULT '0.0000',
                          `total_estimate` decimal(20,4) NOT NULL DEFAULT '0.0000',
                          `description` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT '',
                          `state` int NOT NULL DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_project_management` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `project_planning_id` int NOT NULL DEFAULT '0',
                          `project_id` int NOT NULL DEFAULT '0',
                          `budget` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `actual_cost` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `remaining_budget` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `modified_date` date DEFAULT NULL,
                          `created_by` int NOT NULL DEFAULT '0',
                          `created_date` date DEFAULT NULL,
                          `modified_by` int NOT NULL DEFAULT '0',
                          `state` int NOT NULL DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_project_plan` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `project_id` int NOT NULL DEFAULT '0',
                          `location` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `site` int NOT NULL DEFAULT '0',
                          `plot_number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `start_date` date DEFAULT NULL,
                          `end_date` date DEFAULT NULL,
                          `status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `project_type` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `estimated_budget` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `fund_account_id` int NOT NULL DEFAULT '0',
                          `description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `fund_period` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `assigned_team` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `progress` float DEFAULT '0',
                          `tender_id` int DEFAULT '0',
                          `tender_name_id` int DEFAULT '0',
                          `created_by` int DEFAULT '0',
                          `created_date` date DEFAULT NULL,
                          `modified_by` int DEFAULT '0',
                          `modified_date` date DEFAULT NULL,
                          `state` int DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_project_planning` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date DEFAULT NULL,
                          `project_id` int NOT NULL DEFAULT '0',
                          `location` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `site` int NOT NULL DEFAULT '0',
                          `plot_number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
                          `start_date` date DEFAULT NULL,
                          `end_date` date DEFAULT NULL,
                          `status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `project_type` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `estimated_budget` decimal(20,4) NOT NULL DEFAULT '0.000',
                          `fund_account_id` int NOT NULL DEFAULT '0',
                          `description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `fund_period` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `assigned_team` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `progress` float DEFAULT '0',
                          `tender_id` int DEFAULT '0',
                          `tender_name_id` int DEFAULT '0',
                          `created_by` int DEFAULT '0',
                          `created_date` date DEFAULT NULL,
                          `modified_by` int DEFAULT '0',
                          `modified_date` date DEFAULT NULL,
                          `state` int DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_project_sites` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `name` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `location_id` int DEFAULT '0',
                          `plot_number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          `address` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_project_tender` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `date` date NOT NULL,
                          `tender_name_id` int NOT NULL DEFAULT '0',
                          `account_id` int NOT NULL DEFAULT '0',
                          `project_id` int NOT NULL DEFAULT '0',
                          `fees` decimal(20,4) NOT NULL DEFAULT '0.0000',
                          `submission_date` date NOT NULL,
                          `status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `tender_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `description` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `warehouse_id` int NOT NULL DEFAULT '0',
                          `created_by` int NOT NULL DEFAULT '0',
                          `created_date` date DEFAULT NULL,
                          `modified_by` int DEFAULT '0',
                          `modified_date` date DEFAULT NULL,
                          `amount` decimal(20,4) NOT NULL DEFAULT '0.0000',
                          `bid_amount` decimal(20,4) NOT NULL DEFAULT '0.0000',
                          `state` int NOT NULL DEFAULT '0',
                          `contractor_id` int NOT NULL DEFAULT '0',
	                      `estimate_status` INT(10) NULL DEFAULT '0',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_project_tender_details` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `sr` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `tender_id` int NOT NULL DEFAULT '0',
                          `item_id` varchar(350) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
                          `qty` decimal(20,4) NOT NULL DEFAULT '0.0000',
                          `unit_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0',
                          `rate` decimal(20,4) NOT NULL DEFAULT '0.0000',
                          `amount` decimal(20,4) NOT NULL DEFAULT '0.0000',
                          `margin_percentage` decimal(20,4) NOT NULL DEFAULT '0.0000',
                          `margin_amount` decimal(20,4) NOT NULL DEFAULT '0.0000',
                          `total` decimal(20,4) NOT NULL DEFAULT '0.0000',
                          `progress` decimal(20,4) NOT NULL DEFAULT '0.0000',
                          `assigned` int DEFAULT NULL,
                          `start_date` date DEFAULT NULL,
                          `end_date` date DEFAULT NULL,
	                      `length` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
	                      `width` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
	                      `thickness` VARCHAR(50) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	                      `note` VARCHAR(500) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        CREATE TABLE IF NOT EXISTS `tbl_tender_names` (
                          `id` int NOT NULL AUTO_INCREMENT,
                          `name` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT '0',
                          `code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        CREATE TABLE `tbl_contractor` (
	                        `id` INT(10) NOT NULL AUTO_INCREMENT,
	                        `name` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	                        `country_id` INT(10) NULL DEFAULT NULL,
	                        PRIMARY KEY (`id`) USING BTREE
                        ) COLLATE='utf8mb4_general_ci' ENGINE=InnoDB;
                        
                        CREATE TABLE `tbl_project_material_requests` (
	                        `id` INT(10) NOT NULL AUTO_INCREMENT,
	                        `tender_id` INT(10) NOT NULL DEFAULT '0',
	                        `planning_id` INT(10) NOT NULL DEFAULT '0',
	                        `RequestedDate` DATE NULL DEFAULT NULL,
	                        `IssuedDate` DATE NULL DEFAULT NULL,
	                        `ReceivedDate` DATE NULL DEFAULT NULL,
	                        `itemId` INT(10) NOT NULL DEFAULT '0',
	                        `unit` VARCHAR(50) NOT NULL DEFAULT '0' COLLATE 'utf8mb4_general_ci',
	                        `RequestedQty` DECIMAL(10,2) NOT NULL DEFAULT '0.00',
                            `IssuedQty` DECIMAL(10,2) NOT NULL DEFAULT '0.00',
                            `ReceivedQty` DECIMAL(10,2) NOT NULL DEFAULT '0.00',
	                        PRIMARY KEY (`id`) USING BTREE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        CREATE TABLE tbl_project_resource (
                            `id` int NOT NULL AUTO_INCREMENT,
                            `code` VARCHAR(50) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
                            `date` date DEFAULT NULL,
                            `name` VARCHAR(200) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
                            `role` INT(10) NOT NULL DEFAULT '0',
                            `phone` VARCHAR(20) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
                            `type` VARCHAR(80) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
	                        `price_unit` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
	                        `unit_time` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
	                        `max_unit_time` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
							`employee_id` INT,
                          PRIMARY KEY (`id`)
                        );

                        CREATE TABLE tbl_project_role (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            code VARCHAR(50) NOT NULL UNIQUE COLLATE 'utf8mb4_general_ci',
                            name VARCHAR(100) NOT NULL COLLATE 'utf8mb4_general_ci'
                        );

                        CREATE TABLE tbl_project_activity (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            planning_id INT NOT NULL DEFAULT(0),
                            code VARCHAR(500) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
                            name VARCHAR(500) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
                            start_date DATE NOT NULL,
                            end_date DATE NOT NULL,
                            progress DECIMAL(20,3) NOT NULL DEFAULT '0.000',
                            status INT NOT NULL DEFAULT 0
                        );

                        CREATE TABLE `tbl_project_work_done` (
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
                        COLLATE='utf8mb4_0900_ai_ci' ENGINE=InnoDB;

                        CREATE TABLE `tbl_project_work_done_details` (
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
                        COLLATE='utf8mb4_0900_ai_ci' ENGINE=InnoDB;

                        CREATE TABLE tbl_project_activity_assignment (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            activity_id INT NOT NULL,
                            resource_id INT NOT NULL
                        ); 

                        CREATE TABLE `tbl_manufacturer_batch` (
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
	                        `warehouse_id` INT(10) NULL DEFAULT '0',
                            `product_qty` DECIMAL(10,0) NULL DEFAULT '0',
	                        PRIMARY KEY (`id`) USING BTREE
                        )
                        COLLATE='utf8mb4_0900_ai_ci'
                        ENGINE=InnoDB;

                        CREATE TABLE `tbl_manufacturer_batchdetails` (
	                        `id` INT(10) NOT NULL AUTO_INCREMENT,
	                        `batchID` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_0900_ai_ci',
	                        `itemid` INT(10) NULL DEFAULT NULL,
	                        `cost` DECIMAL(63,2) NULL DEFAULT NULL,
	                        `qty` DECIMAL(20,2) NULL DEFAULT NULL,
	                        `Total` DECIMAL(63,2) NULL DEFAULT NULL,
	                        `RequestQty` DECIMAL(20,2) NULL DEFAULT '0.00',
	                        `ReceiveQty` DECIMAL(20,2) NULL DEFAULT '0.00',
	                        PRIMARY KEY (`id`) USING BTREE
                        )
                        COLLATE='utf8mb4_0900_ai_ci'
                        ENGINE=InnoDB;

                        CREATE TABLE `tbl_manufacturer_task` (
	                        `id` INT(10) NOT NULL AUTO_INCREMENT,
	                        `MachineID` INT(10) NULL DEFAULT NULL,
	                        `BatchID` INT(10) NULL DEFAULT NULL,
	                        `StartTime` DATETIME NULL DEFAULT NULL,
	                        `EndTime` DATETIME NULL DEFAULT NULL,
	                        `Status` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_0900_ai_ci',
	                        `userID` INT(10) NULL DEFAULT NULL,
	                        PRIMARY KEY (`id`) USING BTREE
                        )
                        COLLATE='utf8mb4_0900_ai_ci'
                        ENGINE=InnoDB;

                        CREATE TABLE tbl_manufacturer_task_details (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            TaskID INT NOT NULL,
                            DepartmentID INT NOT NULL,
                            EmployeeID INT NOT NULL,
                            StartTime DATETIME,
                            EndTime DATETIME,
                            Status VARCHAR(50), -- e.g., 'Pending', 'Progress', 'Done'
                            Remarks TEXT
                        );
                        
                        CREATE TABLE `tbl_printconfg` (
	                        `id` INT(10) NOT NULL AUTO_INCREMENT,
	                        `PrintName` VARCHAR(150) NULL DEFAULT NULL COLLATE 'utf8mb4_0900_ai_ci',
	                        PRIMARY KEY (`id`) USING BTREE
                        )
                        COLLATE='utf8mb4_0900_ai_ci'
                        ENGINE=InnoDB
                        ;

                        CREATE TABLE tbl_main_menus ( id INT PRIMARY KEY, name VARCHAR(100) NOT NULL );
                        CREATE TABLE tbl_sub_menus (
                            id INT PRIMARY KEY,
                            name VARCHAR(150) NOT NULL,
                            m_id INT,
                            FOREIGN KEY (m_id) REFERENCES tbl_main_menus(id) );
                                CREATE TABLE tbl_user_permissions (
                                    `id` INT AUTO_INCREMENT PRIMARY KEY,
                                    `user_id` INT NOT NULL,
                                    `sub_menu_id` INT NOT NULL,
                                    `can_view` BOOLEAN NOT NULL DEFAULT FALSE,
                                    `can_edit` BOOLEAN NOT NULL DEFAULT FALSE,
                                    `can_delete` BOOLEAN NOT NULL DEFAULT FALSE
                                );
                        ALTER TABLE tbl_user_permissions
                        ADD UNIQUE KEY uniq_user_submenu (user_id, sub_menu_id);

                        INSERT INTO tbl_main_menus (id, name) VALUES
                        (1, 'Accountant'),
                        (2, 'Inventory'),
                        (3, 'Customer'),
                        (4, 'Vendor'),
                        (5, 'HR'),
                        (6, 'Bank'),
                        (7, 'Report'),
                        (8, 'Setting'),
                        (9, 'File'),
                        (10, 'Construction');
                        
                        INSERT INTO tbl_sub_menus (id, name, m_id) VALUES
                            (1, 'Chart Of Account', 1),
                            (2, 'Cost Center', 1),
                            (3, 'Transactions Journal', 1),
                            (4, 'Fixed Assets', 1),
                            (5, 'Vouchers', 1),
                            (6, 'Prepaid Expense', 1),
                            (7, 'Petty Cash', 1),
                            (8, 'Inventory Items', 2),
                            (9, 'Stock Management', 2),
                            (10, 'Inventory Center', 2),
                            (11, 'Warehouse Center', 2),
                            (12, 'Customer Center', 3),
                            (13, 'Sales Center', 3),
                            (14, 'Create Invoice', 3),
                            (15, 'Receipt Voucher', 3),
                            (16, 'Credit Note', 3),
                            (17, 'Quotation', 3),
                            (18, 'Sales Order', 3),
                            (19, 'Sales Return', 3),
                            (20, 'Sales Proforma', 3),
                            (21, 'Vendor Center', 4),
                            (22, 'Purchases Center', 4),
                            (23, 'Create Purchases', 4),
                            (24, 'Payment Voucher', 4),
                            (25, 'Debit Note', 4),
                            (26, 'Purchase Order', 4),
                            (27, 'Purchase Return', 4),
                            (28, 'Human Resource Center', 5),
                            (29, 'Attendance Sheet', 5),
                            (30, 'Salary Sheet', 5),
                            (31, 'Leave Salary', 5),
                            (32, 'End Of Services', 5),
                            (33, 'Loans', 5),
                            (34, 'Final Settlement', 5),
                            (35, 'Payment Voucher', 5),
                            (36, 'Bank Center', 6),
                            (37, 'Open Bank Card', 6),
                            (38, 'Cheques', 6),
                            (39, 'PDC', 6),
                            (40, 'Company', 7),
                            (41, 'Customer & Receivable', 7),
                            (42, 'Sales', 7),
                            (43, 'Vendor & Payable', 7),
                            (44, 'Purchases', 7),
                            (45, 'Employees', 7),
                            (46, 'Accountant', 7),
                            (47, 'Inventory', 7),
                            (48, 'List', 7),
                            (49, 'Setting', 8),
                            (50, 'Change Current Password', 8),
                            (51, 'Clear Data', 8),
                            (52, 'Users', 8),
                            (53, 'Company List', 9),
                            (54, 'Back Up Company', 9),
                            (55, 'Restore Company', 9),
                            (56, 'Project DashBoard', 10),
                            (57, 'Project Tender', 10),
                            (58, 'Project Estimate', 10),
                            (59, 'Project Planning', 10);
        
                        CREATE TABLE tbl_audit_log (
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
                        ;

                        CREATE TABLE `tbl_color` (
	                        `id` INT NOT NULL AUTO_INCREMENT,
	                        `headerColor` VARCHAR(50) NOT NULL DEFAULT 'White',
	                        `TextColor` VARCHAR(50) NOT NULL DEFAULT 'Black',
	                        PRIMARY KEY (`id`)
                        )
                        COLLATE='utf8mb4_general_ci';
                        INSERT INTO `tbl_color` (id, headerColor, TextColor)
                        VALUES (1,'RoyalBlue','White');
                        
                        
                        CREATE PROCEDURE `sp_data_reset`()
                        BEGIN
                        TRUNCATE TABLE tbl_attendancesheet;
                        TRUNCATE TABLE tbl_attendance_salary;
                        TRUNCATE TABLE tbl_bank_card;
                        TRUNCATE TABLE tbl_bank_register;
                        TRUNCATE TABLE tbl_bank;
                        TRUNCATE TABLE tbl_check_details;
                        TRUNCATE TABLE tbl_cheque;
                        TRUNCATE TABLE tbl_corporate_tax_configration;
                        TRUNCATE TABLE tbl_cost_center;
                        TRUNCATE TABLE tbl_credit_note;
                        TRUNCATE TABLE tbl_credit_note_details;
                        TRUNCATE TABLE tbl_customer;
                        TRUNCATE TABLE tbl_customer_category;
                        TRUNCATE TABLE tbl_damage;
                        TRUNCATE TABLE tbl_damage_details;
                        TRUNCATE TABLE tbl_debit_note;
                        TRUNCATE TABLE tbl_debit_note_details;
                        TRUNCATE TABLE tbl_departments;
                        TRUNCATE TABLE tbl_employee;
                        TRUNCATE TABLE tbl_end_of_service;
                        TRUNCATE TABLE tbl_final_settlement;
                        TRUNCATE TABLE tbl_fixed_assets;
                        TRUNCATE TABLE tbl_fixed_assets_category;
                        TRUNCATE TABLE tbl_general_config;
                        TRUNCATE TABLE tbl_items;
                        TRUNCATE TABLE tbl_items_unit;
                        TRUNCATE TABLE tbl_items_warehouse;
                        TRUNCATE TABLE tbl_item_assembly;
                        TRUNCATE TABLE tbl_item_category;
                        TRUNCATE TABLE tbl_item_stock_settlement;
                        TRUNCATE TABLE tbl_item_stock_settlement_details;
                        TRUNCATE TABLE tbl_item_transaction;
                        TRUNCATE TABLE tbl_item_warehouse_transaction;
                        TRUNCATE TABLE tbl_journal_voucher;
                        TRUNCATE TABLE tbl_journal_voucher_details;
                        TRUNCATE TABLE tbl_leave_salary;
                        TRUNCATE TABLE tbl_loan;
                        TRUNCATE TABLE tbl_payment_voucher;
                        TRUNCATE TABLE tbl_payment_voucher_details;
                        TRUNCATE TABLE tbl_petty_cash_card;
                        TRUNCATE TABLE tbl_petty_cash_category;
                        TRUNCATE TABLE tbl_petty_cash_request;
                        TRUNCATE TABLE tbl_petty_cash_submition;
                        TRUNCATE TABLE tbl_petty_cash_submition_details;
                        TRUNCATE TABLE tbl_position;
                        TRUNCATE TABLE tbl_prepaid_expense;
                        TRUNCATE TABLE tbl_prepaid_expense_category;
                        TRUNCATE TABLE tbl_purchase;
                        TRUNCATE TABLE tbl_purchase_details;
                        TRUNCATE TABLE tbl_purchase_order;
                        TRUNCATE TABLE tbl_purchase_order_details;
                        TRUNCATE TABLE tbl_purchase_return;
                        TRUNCATE TABLE tbl_purchase_return_details;
                        TRUNCATE TABLE tbl_receipt_voucher;
                        TRUNCATE TABLE tbl_receipt_voucher_details;
                        TRUNCATE TABLE tbl_salary;
                        TRUNCATE TABLE tbl_sales;
                        TRUNCATE TABLE tbl_sales_details;
                        TRUNCATE TABLE tbl_sales_order;
                        TRUNCATE TABLE tbl_sales_order_details;
                        TRUNCATE TABLE tbl_sales_quotation;
                        TRUNCATE TABLE tbl_sales_quotation_details;
                        TRUNCATE TABLE tbl_sales_return;
                        TRUNCATE TABLE tbl_sales_return_details;
                        TRUNCATE TABLE tbl_transaction;
                        TRUNCATE TABLE tbl_sub_cost_center;
                        TRUNCATE TABLE tbl_transaction;
                        TRUNCATE TABLE tbl_unit;
                        TRUNCATE TABLE tbl_vat;
                        TRUNCATE TABLE tbl_vendor;
                        TRUNCATE TABLE tbl_vendor_category;
                        TRUNCATE TABLE tbl_warehouse;
                        TRUNCATE TABLE tbl_salary_adjustments;
                        TRUNCATE TABLE tbl_advance_payment_voucher;
                        TRUNCATE TABLE tbl_advance_payment_voucher_details;

                        end

                