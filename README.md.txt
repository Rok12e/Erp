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
ALTER USER 'root'@'%' IDENTIFIED WITH mysql_native_password BY 'yamy';
GRANT ALL PRIVILEGES ON *.* TO 'root'@'%' WITH GRANT OPTION;
FLUSH PRIVILEGES;
✅ This will:

Enable root to connect from remote machines (%)

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
