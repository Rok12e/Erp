# README: Enable Remote Access for MySQL `root` User

This guide helps you configure MySQL to allow the `root` user to connect remotely using the `mysql_native_password` plugin (recommended for compatibility with many apps).

## Prerequisites

- MySQL installed (version 5.7 or 8.0+)
- Access to MySQL server with admin privileges
- Command-line access or MySQL client

---

## Steps

Open Command Prompt or Terminal on the server (same machine MySQL is running).

Log in as root from localhost:

bash
Copy
Edit
mysql -u root -p
You'll be prompted for the root password (the one set during MySQL installation).

Once logged in, run:

sql
Copy
Edit
ALTER USER 'root'@'localhost' IDENTIFIED WITH mysql_native_password BY 'yamy';
GRANT ALL PRIVILEGES ON *.* TO 'root'@'localhost' WITH GRANT OPTION;
FLUSH PRIVILEGES;
✅ This will:

Enable root to connect from remote machines (localhost)

Use the correct password method

Allow full access


### 1. Log in to MySQL as root (locally on the server)

Open a terminal or command prompt **on the machine where MySQL is installed**, then run:

```bash
mysql -u root -p

2. Enable remote access for root
Run the following SQL commands:

sql
Copy
Edit
-- Set the password and enable mysql_native_password for remote root
ALTER USER 'root'@'%' IDENTIFIED WITH mysql_native_password BY 'yamy';

-- Grant full access to all databases from any host
GRANT ALL PRIVILEGES ON *.* TO 'root'@'%' WITH GRANT OPTION;

-- Apply the changes
FLUSH PRIVILEGES;
🔐 Replace 'yamy' with your actual password if needed.

3. Allow MySQL to listen on all IP addresses
Edit your MySQL config file (my.cnf or my.ini):

Linux: /etc/mysql/mysql.conf.d/mysqld.cnf

Windows: Usually C:\ProgramData\MySQL\MySQL Server X.Y\my.ini

Look for:

ini
Copy
Edit
bind-address = 127.0.0.1
Change it to:

ini
Copy
Edit
bind-address = 0.0.0.0
Then restart MySQL:

Linux:

bash
Copy
Edit
sudo systemctl restart mysql
Windows:
Open services.msc, find MySQL, right-click → Restart

4. (Optional) Open MySQL port in firewall
Make sure port 3306 is open:

Windows:

cmd
Copy
Edit
netsh advfirewall firewall add rule name="MySQL" dir=in action=allow protocol=TCP localport=3306
Linux (UFW):

bash
Copy
Edit
sudo ufw allow 3306
5. Test remote connection
From another machine:

bash
Copy
Edit
mysql -h <server-ip> -u root -p
Done!
You can now connect to your MySQL server remotely using:

csharp
Copy
Edit
Server=your-server-name;Database=your-db;Uid=root;Pwd=yamy;
vbnet
Copy
Edit

new columns 

tbl_items 
ALTER TABLE tbl_items MODIFY COLUMN name VARCHAR(500) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci'

table tbl_project_tender
`estimate_status` INT(10) NULL DEFAULT '0',

table tbl_project_tender_details
`length` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
`width` DECIMAL(20,4) NOT NULL DEFAULT '0.0000',
`thickness` VARCHAR(50) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
`note` VARCHAR(500) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',

table tbl_items_boq
`length` decimal(10,2) DEFAULT NULL,
`width` decimal(10,2) DEFAULT NULL,
`thickness` VARCHAR(20) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
`note` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,

new tables/****************/


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
                            `role` VARCHAR(80) NOT NULL DEFAULT '' COLLATE 'utf8mb4_general_ci',
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

                        'Assets', 'Liabilities', 'Equity'

'Income', 'Cost', 'General & Direct Expenses'


= "ASSET"; 
= "LIABILITY";
= "EQUITY";
 = "INCOME";
= "COST";
= "EXPENSE";

category_code