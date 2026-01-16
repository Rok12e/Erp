using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace YamyProject
{
    class Utilities
    {
        public static bool AreDefaultAccountsSet(List<string> accountCategories)
        {
            string categories = string.Join(",", accountCategories.ConvertAll(c => $"'{c}'"));

            string query = $"SELECT COUNT(*) FROM tbl_coa_config WHERE category IN ({categories})";
            using (MySqlDataReader reader = DBClass.ExecuteReader(query))
                if (reader.Read() && Convert.ToInt32(reader[0]) == accountCategories.Count)
                    return true;

            return false;
        }

        public static string FormatDecimal(object value)
        {
            if (value != DBNull.Value && value != null)
            {
                return Convert.ToDecimal(value).ToString(DecimalFormatDigit());
            }
            return "0.00";
        }

        public static string DecimalFormatDigit()
        {
            return DecimalPlace == 1 ? "F1" : DecimalPlace == 2 ? "F2" : DecimalPlace == 3 ? "F3" : DecimalPlace == 4 ? "F4" : "F2";
        }
        public static int DecimalPlace = 2;

        public static string GeneralSettings(string name)
        {
            DataRow[] rows = BindDataTable.tableGeneralSettings.Select($"name = '" + name + "'");
            if (rows.Length > 0)
            {
                return rows[0]["value"].ToString();
            }
            return "";
        }
        public static string GeneralSettingsState(string name)
        {
            DataRow[] rows = BindDataTable.tableGeneralSettings.Select($"name = '" + name + "'");
            if (rows.Length > 0)
            {
                return rows[0]["status"].ToString();
            }
            return "";
        }
        public static bool GetModules(string name)
        {
            DataRow[] dataRow = BindDataTable.tableModules.Select($"name = '" + name + "'");
            return dataRow.Length > 0;
        }
        public static string GeneralTaxType(DataTable dt, string name)
        {
            //concat(name, '-', value, '%')
            DataRow[] rows = BindDataTable.tableGeneralSettings.Select($"name = '" + name + "'");
            if (rows.Length > 0)
            {
                DataRow[] rows1 = dt.Select($"id = '" + rows[0]["value"] + "'");
                if (rows1.Length > 0)
                {
                    return rows1[0]["name"].ToString();
                }
                else
                {
                    return "VAT" + rows[0]["value"].ToString();
                }
            }
            return "";
        }

        public static int? GetVoucherIdFromCode(string code)
        {
            var match = Regex.Match(code, @"(\d+)$");
            if (match.Success && int.TryParse(match.Value, out int id))
                return id;
            return null;
        }

        public void Form_KeyDown(Form form, object sender, KeyEventArgs e)
        {
            // Check if the pressed key is Escape
            if (e.KeyCode == Keys.Escape)
            {
                // Close the form
                form.Close();
            }

        }

        public static decimal ParseDecimalValue(object cellValue)
        {
            if (cellValue == null)
                return 0;

            string rawValue = cellValue.ToString().Trim();
            string cleanedValue = Regex.Replace(rawValue, @"[^\d.]", "");
            int dotIndex = cleanedValue.IndexOf('.');
            if (dotIndex >= 0)
            {
                cleanedValue = cleanedValue.Substring(0, dotIndex + 1) +
                               cleanedValue.Substring(dotIndex + 1).Replace(".", "");
            }
            decimal costPrice = 0;
            decimal.TryParse(cleanedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out costPrice);

            return Math.Round(costPrice, 2);
        }

        public static void LogAudit(int userId, string actionType, string module, int recordId, string details)
        {
            try
            {
                string query = @"INSERT INTO tbl_audit_log (user_id, action_type, module_name, record_id, details, ip_address, machine_name)
                     VALUES (@userId, @actionType, @module, @recordId, @details, @ip, @machine)";

                DBClass.ExecuteNonQuery(query,
                    DBClass.CreateParameter("userId", userId),
                    DBClass.CreateParameter("actionType", actionType),
                    DBClass.CreateParameter("module", module),
                    DBClass.CreateParameter("recordId", recordId),
                    DBClass.CreateParameter("details", details),
                    DBClass.CreateParameter("ip", GetLocalIPAddress()),
                    DBClass.CreateParameter("machine", Environment.MachineName)
                );
            }
            catch (Exception ex)
            {
                ex.ToString(); // Log or handle the exception as needed
            }
        }

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return "127.0.0.1";
        }

        internal static void LogError(string v, object message, int userId)
        {
            try
            {
                string query = @"INSERT INTO tbl_error_log (procedure_name, error_message, user_id)
                     VALUES (@proName, @message, @userId)";

                DBClass.ExecuteNonQueryLog(query,
                    DBClass.CreateParameter("userId", userId),
                    DBClass.CreateParameter("proName", v),
                    DBClass.CreateParameter("message", message)
                );
            }
            catch (Exception ex)
            {
                ex.ToString(); // Log or handle the exception as needed
            }
        }

        public static decimal ToDecimal(object input)
        {
            if (input == null || input == DBNull.Value)
                return 0;

            decimal result;
            if (decimal.TryParse(input.ToString(), out result))
                return result;

            return 0;
        }

        public static bool CheckVendorType(object selectedValue)
        {
            object ob = DBClass.ExecuteScalar("select type from tbl_vendor where id=@id", DBClass.CreateParameter("id", selectedValue));
            if (ob != null)
            {
                if(!string.IsNullOrWhiteSpace(ob.ToString()))
                    return ob.ToString() == "Subcontractor";
            }
            return false;
        }
    }
}

class CostItem
{
    public int Level4Id { get; set; }
    public decimal Amount { get; set; }
}

public static class AccountCategory
{
    public static readonly string[] Codes = new string[]
    {
            ASSET, LIABILITY, EQUITY, INCOME, COST, EXPENSE
    };

    public const string ASSET = "ASSET";
    public const string LIABILITY = "LIABILITY";
    public const string EQUITY = "EQUITY";
    public const string INCOME = "INCOME";
    public const string COST = "COST";
    public const string EXPENSE = "EXPENSE";
}

public static class DateRangeHelper
{
    public static void SetDateRange(string option, DateTimePicker dtpFrom, DateTimePicker dtpTo)
    {
        DateTime today = DateTime.Today;
        DateTime fromDate = today;
        DateTime toDate = today;

        switch (option)
        {
            case "All":
                fromDate = dtpFrom.MinDate;
                toDate = dtpTo.MaxDate;
                break;

            case "Today":
                fromDate = today;
                toDate = today;
                break;

            case "Yesterday":
                fromDate = today.AddDays(-1);
                toDate = today.AddDays(-1);
                break;

            case "This Week":
                int diff = (7 + (today.DayOfWeek - DayOfWeek.Sunday)) % 7;
                fromDate = today.AddDays(-diff);
                toDate = fromDate.AddDays(6);
                break;

            case "This Week-To-Date":
                diff = (7 + (today.DayOfWeek - DayOfWeek.Sunday)) % 7;
                fromDate = today.AddDays(-diff);
                toDate = today;
                break;

            case "This Month":
                fromDate = new DateTime(today.Year, today.Month, 1);
                toDate = fromDate.AddMonths(1).AddDays(-1);
                break;

            case "This Month-to-Date":
                fromDate = new DateTime(today.Year, today.Month, 1);
                toDate = today;
                break;

            case "This Fiscal Quarter":
                GetFiscalQuarter(today, out fromDate, out toDate);
                break;

            case "This Fiscal Quarter-To-Date":
                GetFiscalQuarter(today, out fromDate, out toDate);
                toDate = today;
                break;

            case "This Fiscal Quarter Year":
                GetFiscalYear(today, out fromDate, out toDate);
                break;

            case "This Fiscal Quarter Year-To-Last Month":
                GetFiscalYear(today, out fromDate, out toDate);
                toDate = new DateTime(today.Year, today.Month, 1).AddDays(-1);
                break;

            case "This Fiscal Quarter Year-To-Date":
                GetFiscalYear(today, out fromDate, out toDate);
                toDate = today;
                break;
        }

        dtpFrom.Value = fromDate;
        dtpTo.Value = toDate;
    }

    private static void GetFiscalQuarter(DateTime today, out DateTime start, out DateTime end)
    {
        int quarterNumber = (today.Month - 1) / 3 + 1;
        start = new DateTime(today.Year, (quarterNumber - 1) * 3 + 1, 1);
        end = start.AddMonths(3).AddDays(-1);
    }

    private static void GetFiscalYear(DateTime today, out DateTime start, out DateTime end)
    {
        // Example: Fiscal year = Jan 1 – Dec 31
        start = new DateTime(today.Year, 1, 1);
        end = new DateTime(today.Year, 12, 31);
    }
}

public static class HardwareInfo
{
    public static string GetHDDSerialX()
    {
        try
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive");
            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                return wmi_HD["SerialNumber"].ToString().Trim();
            }
        }
        catch { }
        return "UNKNOWN";
    }

    public static string GetMotherboardSerial()
    {
        try
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");
            foreach (ManagementObject board in searcher.Get())
            {
                return board["SerialNumber"].ToString().Trim();
            }
        }
        catch { }
        return "UNKNOWN";
    }

    public static class TrialManager
    {
        private static int trialDays = 30; // 30-day trial

        private static string GetLicenseFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "erp_license.dat");
        }

        public static void CreateTrial(int trialDays)
        {
            string hddKey = HardwareInfo.GetMotherboardSerial();
            string startDate = DateTime.Now.ToString("yyyy-MM-dd");
            string expireDate = DateTime.Now.AddDays(trialDays).ToString("yyyy-MM-dd");

            string data = $"{hddKey}|{startDate}|{expireDate}";
            File.WriteAllText(GetLicenseFilePath(), Encrypt(data));
        }

        public static bool IsTrialValid()
        {
            string licenseFile = GetLicenseFilePath();

            if (!File.Exists(licenseFile))
            {
                // First run → create trial
                CreateTrial(trialDays);
                return true;
            }

            try
            {
                string encrypted = File.ReadAllText(licenseFile);
                string decrypted = Decrypt(encrypted);

                var parts = decrypted.Split('|');
                if (parts.Length != 3) return false;

                string hddKey = parts[0];
                string secret = parts[1];
                DateTime expireDate = DateTime.Parse(parts[2]);

                // Check motherboard binding
                if (hddKey != HardwareInfo.GetMotherboardSerial())
                    return false;

                // Prevent backdate cheating
                if (DateTime.Now > expireDate)
                    return false;

                return true;
            }
            catch
            {
                return false; // corrupted license file
            }
        }

        // Simple AES Encrypt/Decrypt
        private static string Encrypt(string plainText)
        {
            byte[] key = Encoding.UTF8.GetBytes("MySecretKey12345"); // 16 chars
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = new byte[16];
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private static string Decrypt(string cipherText)
        {
            byte[] key = Encoding.UTF8.GetBytes("MySecretKey12345");
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = new byte[16];
                ICryptoTransform descriptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, descriptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static int GetRemainingTrialDays()
        {
            string decrypted = Decrypt(File.ReadAllText(GetLicenseFilePath()));
            var parts = decrypted.Split('|');
            DateTime expireDate = DateTime.Parse(parts[2]);
            int days = (expireDate - DateTime.Now).Days;
            return Math.Max(days, 0);
        }
    }

    public static class ActivationManager
    {
        private static string secret = "ERP2025"; // your secret key

        // Generate the valid key for this motherboard
        public static string GenerateActivationKey()
        {
            string mbKey = HardwareInfo.GetMotherboardSerial();
            string raw = mbKey + "|" + secret;

            using (var md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(raw));
                StringBuilder sb = new StringBuilder();

                foreach (byte b in hash)
                {
                    int val = b % 36;
                    if (val < 10)
                        sb.Append(val);   // digits 0–9
                    else
                        sb.Append((char)('A' + (val - 10))); // A–Z
                }

                // 4-4-4-2 block style
                string block1 = sb.ToString().Substring(0, 4);
                string block2 = sb.ToString().Substring(4, 4);
                string block3 = sb.ToString().Substring(8, 4);
                string block4 = sb.ToString().Substring(12, 2);

                return $"{block1}-{block2}-{block3}-{block4}";
            }
        }

        public static bool ValidateLicense(string enteredKey)
        {
            string validKey = GenerateActivationKey();
            if (enteredKey.Trim().Equals(validKey, StringComparison.OrdinalIgnoreCase))
            {
                SaveLicense(enteredKey); // store license locally
                return true;
            }
            return false;
        }

        private static void SaveLicense(string key)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "erp_license.dat");
            File.WriteAllText(path, key);
        }

        public static bool IsActivated()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "erp_license.dat");
            if (!File.Exists(path)) return false;

            string savedKey = File.ReadAllText(path);
            return ValidateLicense(savedKey);
        }
    }

}