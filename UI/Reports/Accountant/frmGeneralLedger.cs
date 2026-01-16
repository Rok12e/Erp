using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;
using Excel = Microsoft.Office.Interop.Excel;
using Orientation = MigraDoc.DocumentObjectModel.Orientation;
namespace YamyProject
{
    public partial class frmGeneralLedger : Form
    {
        public frmGeneralLedger()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void frmGeneralLedger_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ▼";
            LoadData();
            DateTime dated = DateTime.Now;
            guna2HtmlLabel11.Text = dated.TimeOfDay.ToString();
            guna2HtmlLabel11.Text = dated.Date.ToShortDateString();
            loadCompany();
            LoadData();
        }
        private void loadCompany()
        {
            using (var reader = DBClass.ExecuteReader("SELECT name FROM tbl_company"))
            {
                if (reader.Read() && reader["name"] != DBNull.Value)
                {
                    guna2HtmlLabel8.Text = reader["name"].ToString();
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            contextMenuExport.Show(btnPrint, new Point(0, btnPrint.Height));
        }

        private void Report_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Exporting Report...");
        }

        //private DataTable ledgerTable;

        //private void InitializeLedgerTable()
        //{
        //    ledgerTable = new DataTable();
        //    ledgerTable.Columns.Add("id", typeof(int));
        //    ledgerTable.Columns.Add("Type", typeof(string));
        //    ledgerTable.Columns.Add("Date", typeof(string));
        //    ledgerTable.Columns.Add("Num", typeof(string));
        //    ledgerTable.Columns.Add("Description", typeof(string));
        //    ledgerTable.Columns.Add("Debit", typeof(decimal));
        //    ledgerTable.Columns.Add("Credit", typeof(decimal));
        //    ledgerTable.Columns.Add("Balance", typeof(decimal));
        //}

        //private void LoadData()
        //{
        //    dgvSales.DataSource = null;

        //    Task.Run(() =>
        //    {
        //        DataTable tmpTable = new DataTable();
        //        tmpTable.Columns.Add("id", typeof(int));
        //        tmpTable.Columns.Add("Type", typeof(string));
        //        tmpTable.Columns.Add("Date", typeof(string));
        //        tmpTable.Columns.Add("Num", typeof(string));
        //        tmpTable.Columns.Add("Description", typeof(string));
        //        tmpTable.Columns.Add("Debit", typeof(decimal));
        //        tmpTable.Columns.Add("Credit", typeof(decimal));
        //        tmpTable.Columns.Add("Balance", typeof(decimal));

        //        using (MySqlDataReader readerL = DBClass.ExecuteReader(
        //            "SELECT id, CONCAT(code,' - ',name) name FROM tbl_coa_level_4 WHERE ID IN (SELECT DISTINCT account_id FROM tbl_transaction);"))
        //        {
        //            while (readerL.Read())
        //            {
        //                int id = Convert.ToInt32(readerL["id"]);
        //                string accName = readerL["name"].ToString();

        //                // 🔹 Header row
        //                DataRow headerRow = tmpTable.NewRow();
        //                headerRow["id"] = id;
        //                headerRow["Type"] = accName;
        //                headerRow["Date"] = DBNull.Value;
        //                headerRow["Num"] = DBNull.Value;
        //                headerRow["Description"] = DBNull.Value;
        //                headerRow["Debit"] = DBNull.Value;
        //                headerRow["Credit"] = DBNull.Value;
        //                headerRow["Balance"] = DBNull.Value;
        //                tmpTable.Rows.Add(headerRow);

        //                // 🔹 Transaction rows
        //                string sql = @"SELECT 
        //                        DATE_FORMAT(t.date, '%M %d %Y') AS date,
        //                        t.transaction_id AS `Num`,
        //                        t.type AS `Type`,
        //                        t.description,
        //                        t.debit,
        //                        t.credit
        //                       FROM tbl_transaction t
        //                       WHERE t.account_id = @id
        //                       ORDER BY t.date;";

        //                List<MySqlParameter> parameters = new List<MySqlParameter>
        //        {
        //            DBClass.CreateParameter("id", id)
        //        };

        //                decimal runningDebit = 0, runningCredit = 0;

        //                using (MySqlDataReader reader = DBClass.ExecuteReader(sql, parameters.ToArray()))
        //                {
        //                    while (reader.Read())
        //                    {
        //                        decimal debit = Convert.ToDecimal(reader["debit"]);
        //                        decimal credit = Convert.ToDecimal(reader["credit"]);

        //                        runningDebit += debit;
        //                        runningCredit += credit;

        //                        DataRow row = tmpTable.NewRow();
        //                        row["id"] = id;
        //                        row["Type"] = reader["Type"].ToString();
        //                        row["Date"] = reader["date"].ToString();
        //                        row["Num"] = reader["Num"].ToString();
        //                        row["Description"] = reader["description"].ToString();
        //                        row["Debit"] = debit;
        //                        row["Credit"] = credit;
        //                        row["Balance"] = (runningDebit - runningCredit);
        //                        tmpTable.Rows.Add(row);
        //                    }
        //                }
        //            }
        //        }

        //        // 🔹 Bind only after table is fully built
        //        this.Invoke((MethodInvoker)delegate
        //        {
        //            ledgerTable = tmpTable; // swap reference
        //            dgvSales.DataSource = ledgerTable;
        //            FormatGrid();
        //        });
        //    });
        //}

        //private void LoadData()
        //{
        //    dgvSales.DataSource = null;
        //    InitializeLedgerTable(); // Reset table

        //    if (report_type.Text == "Default")
        //    {
        //        Task.Run(() =>
        //        {
        //            decimal totalAmount = 0;

        //            using (MySqlDataReader readerL = DBClass.ExecuteReader(
        //                "SELECT id, CONCAT(code,' - ',name) name FROM tbl_coa_level_4 WHERE ID IN (SELECT DISTINCT account_id FROM tbl_transaction);"))
        //            {
        //                while (readerL.Read())
        //                {
        //                    int id = Convert.ToInt32(readerL["id"]);
        //                    string accName = readerL["name"].ToString();

        //                    decimal lineTotalDebit = 0, lineTotalCredit = 0;
        //                    decimal subTotalDebit = 0, subTotalCredit = 0;

        //                    string sql = @"
        //                SELECT 
        //                    DATE_FORMAT(t.date, '%M %d %Y') AS date,
        //                    t.transaction_id AS `Num`,
        //                    t.type AS `Type`,
        //                    t.description,
        //                    t.debit,
        //                    t.credit
        //                FROM tbl_transaction t
        //                WHERE t.account_id = @id
        //            ";

        //                    List<MySqlParameter> parameters = new List<MySqlParameter>
        //            {
        //                DBClass.CreateParameter("id", id)
        //            };

        //                    // Date filter
        //                    Invoke(new MethodInvoker(() =>
        //                    {
        //                        if (!chkDate.Checked)
        //                        {
        //                            sql += " AND t.date >= @startDate AND t.date <= @endDate";
        //                            parameters.Add(DBClass.CreateParameter("startDate", dtFrom.Value.Date));
        //                            parameters.Add(DBClass.CreateParameter("endDate", dtTo.Value.Date));
        //                        }
        //                    }));

        //                    sql += " ORDER BY t.date;";

        //                    // Add header row safely
        //                    DataRow headerRow = ledgerTable.NewRow();
        //                    headerRow["id"] = id;
        //                    headerRow["Type"] = accName;
        //                    headerRow["Date"] = DBNull.Value;
        //                    headerRow["Num"] = DBNull.Value;
        //                    headerRow["Description"] = DBNull.Value;
        //                    headerRow["Debit"] = DBNull.Value;
        //                    headerRow["Credit"] = DBNull.Value;
        //                    headerRow["Balance"] = DBNull.Value;
        //                    ledgerTable.Rows.Add(headerRow);

        //                    using (MySqlDataReader reader = DBClass.ExecuteReader(sql, parameters.ToArray()))
        //                    {
        //                        while (reader.Read())
        //                        {
        //                            if (reader["Date"] == DBNull.Value)
        //                                continue;

        //                            decimal debit = Convert.ToDecimal(reader["debit"]);
        //                            decimal credit = Convert.ToDecimal(reader["credit"]);

        //                            subTotalDebit += debit;
        //                            subTotalCredit += credit;

        //                            DataRow row = ledgerTable.NewRow();
        //                            row["id"] = id;
        //                            row["Type"] = reader["Type"].ToString();
        //                            row["Date"] = reader["date"].ToString();
        //                            row["Num"] = reader["Num"].ToString();
        //                            row["Description"] = reader["description"].ToString();
        //                            row["Debit"] = debit;
        //                            row["Credit"] = credit;
        //                            row["Balance"] = (subTotalDebit - subTotalCredit);
        //                            ledgerTable.Rows.Add(row);
        //                        }
        //                    }

        //                    lineTotalDebit += subTotalDebit;
        //                    lineTotalCredit += subTotalCredit;
        //                    decimal lineBalance = lineTotalDebit - lineTotalCredit;

        //                    // Add total row for account
        //                    DataRow totalRow = ledgerTable.NewRow();
        //                    totalRow["Description"] = "TOTAL";
        //                    totalRow["Debit"] = lineTotalDebit;
        //                    totalRow["Credit"] = lineTotalCredit;
        //                    totalRow["Balance"] = lineBalance;
        //                    ledgerTable.Rows.Add(totalRow);

        //                    totalAmount += lineBalance;
        //                }
        //            }

        //            // Final total row
        //            DataRow finalRow = ledgerTable.NewRow();
        //            finalRow["Description"] = "TOTAL";
        //            finalRow["Balance"] = totalAmount;
        //            ledgerTable.Rows.Add(finalRow);

        //            // Bind to grid
        //            this.Invoke((MethodInvoker)delegate
        //            {
        //                dgvSales.DataSource = ledgerTable;
        //                FormatGrid();
        //            });
        //        });
        //    }
        //    else
        //    {
        //        Task.Run(() =>
        //        {
        //            using (MySqlDataReader readerL = DBClass.ExecuteReader(
        //                "SELECT id FROM tbl_coa_level_4 WHERE ID IN (SELECT DISTINCT account_id FROM tbl_transaction);"))
        //            {
        //                while (readerL.Read())
        //                {
        //                    int id = Convert.ToInt32(readerL["id"]);

        //                    string sql = @"
        //                SELECT 
        //                    DATE_FORMAT(t.date, '%M %d %Y') AS date,
        //                    t.transaction_id AS `Num`,
        //                    t.type AS `Type`,
        //                    t.description,
        //                    t.debit,
        //                    t.credit
        //                FROM tbl_transaction t
        //                WHERE t.account_id = @id
        //            ";

        //                    List<MySqlParameter> parameters = new List<MySqlParameter>
        //            {
        //                DBClass.CreateParameter("id", id)
        //            };

        //                    // Date filter
        //                    Invoke(new MethodInvoker(() =>
        //                    {
        //                        if (!chkDate.Checked)
        //                        {
        //                            sql += " AND t.date >= @startDate AND t.date <= @endDate";
        //                            parameters.Add(DBClass.CreateParameter("startDate", dtFrom.Value.Date));
        //                            parameters.Add(DBClass.CreateParameter("endDate", dtTo.Value.Date));
        //                        }
        //                    }));

        //                    sql += " ORDER BY t.date;";

        //                    decimal runningDebit = 0, runningCredit = 0;

        //                    using (MySqlDataReader reader = DBClass.ExecuteReader(sql, parameters.ToArray()))
        //                    {
        //                        while (reader.Read())
        //                        {
        //                            if (reader["Date"] == DBNull.Value)
        //                                continue;

        //                            decimal debit = Convert.ToDecimal(reader["debit"]);
        //                            decimal credit = Convert.ToDecimal(reader["credit"]);

        //                            runningDebit += debit;
        //                            runningCredit += credit;

        //                            DataRow row = ledgerTable.NewRow();
        //                            row["id"] = id;
        //                            row["Type"] = reader["Type"].ToString();
        //                            row["Date"] = reader["date"].ToString();
        //                            row["Num"] = reader["Num"].ToString();
        //                            row["Description"] = reader["description"].ToString();
        //                            row["Debit"] = debit;
        //                            row["Credit"] = credit;
        //                            row["Balance"] = (runningDebit - runningCredit);
        //                            ledgerTable.Rows.Add(row);
        //                        }
        //                    }
        //                }
        //            }

        //            // Bind to grid
        //            this.Invoke((MethodInvoker)delegate
        //            {
        //                dgvSales.DataSource = ledgerTable;
        //                FormatGrid();
        //            });
        //        });
        //    }
        //}

        //private void FormatGrid()
        //{
        //    dgvSales.Columns["id"].Visible = false;
        //    dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //    dgvSales.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);
        //    dgvSales.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
        //    dgvSales.EnableHeadersVisualStyles = false;

        //    dgvSales.Columns["Debit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //    dgvSales.Columns["Credit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //    dgvSales.Columns["Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        //    dgvSales.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
        //    dgvSales.DefaultCellStyle.BackColor = System.Drawing.Color.White;
        //    dgvSales.GridColor = System.Drawing.Color.LightGray;
        //    dgvSales.BorderStyle = System.Windows.Forms.BorderStyle.None;
        //    dgvSales.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        //    dgvSales.RowHeadersVisible = false;
        //}

        //private void FilterLedger(string filterType, string filterText)
        //{
        //    if (ledgerTable == null) return;

        //    DataView dv = ledgerTable.DefaultView;

        //    if (!string.IsNullOrEmpty(filterText))
        //        dv.RowFilter = $"{filterType} LIKE '%{filterText}%'";
        //    else
        //        dv.RowFilter = "";

        //    dgvSales.DataSource = dv;
        //}

        private void LoadData()
        {
            dgvSales.Rows.Clear();

            if (dgvSales.Columns.Count == 0)
            {
                dgvSales.Columns.Add("id", "");
                dgvSales.Columns.Add("Type", "Type");
                dgvSales.Columns.Add("Date", "Date");
                dgvSales.Columns.Add("Num", "Num");
                dgvSales.Columns.Add("Description", "Description");
                dgvSales.Columns.Add("Debit", "Debit");
                dgvSales.Columns.Add("Credit", "Credit");
                dgvSales.Columns.Add("Balance", "Balance");

                LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                dgvSales.Columns["Debit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Columns["Credit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Columns["Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgvSales.Columns["Debit"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);
                dgvSales.Columns["Credit"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);

                dgvSales.Columns["id"].Visible = false;
                dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvSales.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);
                dgvSales.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                dgvSales.EnableHeadersVisualStyles = false;
                dgvSales.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dgvSales.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                dgvSales.GridColor = System.Drawing.Color.LightGray;
                dgvSales.BorderStyle = System.Windows.Forms.BorderStyle.None;
                dgvSales.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dgvSales.RowHeadersVisible = false;
            }
            if (report_type.Text == "Default")
            {
                Task.Run(() =>
                {
                    decimal totalAmount = 0;

                    using (MySqlDataReader readerL = DBClass.ExecuteReader("SELECT id, CONCAT(code,' - ',name) name FROM tbl_coa_level_4 WHERE ID IN (SELECT DISTINCT account_id FROM tbl_transaction);"))
                    {
                        while (readerL.Read())
                        {
                            int id = Convert.ToInt32(readerL["id"]);
                            string accName = readerL["name"].ToString();

                            decimal lineTotalDebit = 0, lineTotalCredit = 0;
                            List<DataGridViewRow> rowsToAdd = new List<DataGridViewRow>();

                            string sql = @"
                                        SELECT 
                                            DATE_FORMAT(t.date, '%M %d %Y') AS date,
                                            t.transaction_id AS `Num`,
                                            t.type AS `Type`,
                                            t.t_type, 
                                            t.description,
                                            t.debit,
                                            t.credit,
                                            (t.debit - t.credit) AS amount
                                        FROM tbl_transaction t
                                        INNER JOIN tbl_coa_level_4 l4 ON t.account_id = l4.id
                                        WHERE t.account_id = @id
                                    ";

                            List<MySqlParameter> parameters = new List<MySqlParameter> { DBClass.CreateParameter("id", id) };

                            if (!chkDate.InvokeRequired)
                            {
                                if (!chkDate.Checked)
                                {
                                    sql += " AND t.date >= @startDate AND t.date <= @endDate";
                                    parameters.Add(DBClass.CreateParameter("startDate", dtFrom.Value.Date));
                                    parameters.Add(DBClass.CreateParameter("endDate", dtTo.Value.Date));
                                }
                            }
                            else
                            {
                                Invoke(new MethodInvoker(() =>
                                {
                                    if (!chkDate.Checked)
                                    {
                                        sql += " AND t.date >= @startDate AND t.date <= @endDate";
                                        parameters.Add(DBClass.CreateParameter("startDate", dtFrom.Value.Date));
                                        parameters.Add(DBClass.CreateParameter("endDate", dtTo.Value.Date));
                                    }
                                }));
                            }

                            sql += " ORDER BY t.date;";

                            using (MySqlDataReader reader = DBClass.ExecuteReader(sql, parameters.ToArray()))
                            {
                                decimal subTotalDebit = 0, subTotalCredit = 0;

                                while (reader.Read())
                                {
                                    if (reader["Date"] == DBNull.Value)
                                        continue;

                                    decimal debit = Convert.ToDecimal(reader["debit"]);
                                    decimal credit = Convert.ToDecimal(reader["credit"]);

                                    subTotalDebit += debit;
                                    subTotalCredit += credit;

                                    DataGridViewRow row = new DataGridViewRow();
                                    row.CreateCells(dgvSales,
                                        reader["Num"].ToString(),
                                        reader["Type"].ToString(),
                                        reader["date"].ToString(),
                                        reader["Num"].ToString(),
                                        reader["description"].ToString(),
                                        debit.ToString("N2"),
                                        credit.ToString("N2"),
                                        (subTotalDebit - subTotalCredit).ToString("N2")
                                    );
                                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                    row.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);

                                    rowsToAdd.Add(row);
                                }

                                lineTotalDebit += subTotalDebit;
                                lineTotalCredit += subTotalCredit;
                            }

                            decimal lineBalance = lineTotalDebit - lineTotalCredit;

                            DataGridViewRow headerRow = new DataGridViewRow();
                            headerRow.CreateCells(dgvSales, id, accName, "", "", "", "", "", "");
                            rowsToAdd.Insert(0, headerRow);

                            DataGridViewRow totalRow = new DataGridViewRow();
                            totalRow.CreateCells(dgvSales, null, null, null, null, "TOTAL",
                                lineTotalDebit.ToString("N2"),
                                lineTotalCredit.ToString("N2"),
                                lineBalance.ToString("N2"));
                            totalRow.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold | FontStyle.Underline);
                            totalRow.DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
                            totalRow.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                            totalRow.Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            totalRow.Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                            rowsToAdd.Add(totalRow);

                            totalAmount += lineBalance;

                            this.Invoke((MethodInvoker)delegate
                            {
                                foreach (var r in rowsToAdd)
                                    dgvSales.Rows.Add(r);
                            });
                        }

                        // Final total row
                        this.Invoke((MethodInvoker)delegate
                        {
                            int totalRow = dgvSales.Rows.Add(null, null, null, null, "", "", "TOTAL", totalAmount.ToString("N2"));
                            dgvSales.Rows[totalRow].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold | FontStyle.Underline);
                            dgvSales.Rows[totalRow].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
                            dgvSales.Rows[totalRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                            dgvSales.Rows[totalRow].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            dgvSales.Rows[totalRow].Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                        });
                    }
                });
            }
            else
            {
                Task.Run(() =>
                {
                    using (MySqlDataReader readerL = DBClass.ExecuteReader("SELECT id FROM tbl_coa_level_4 WHERE ID IN (SELECT DISTINCT account_id FROM tbl_transaction);"))
                    {
                        while (readerL.Read())
                        {
                            int id = Convert.ToInt32(readerL["id"]);

                            List<DataGridViewRow> rowsToAdd = new List<DataGridViewRow>();

                            string sql = @"SELECT 
                                                DATE_FORMAT(t.date, '%M %d %Y') AS date,
                                                t.transaction_id AS `Num`,
                                                t.type AS `Type`,
                                                t.description,
                                                t.debit,
                                                t.credit
                                            FROM tbl_transaction t
                                            WHERE t.account_id = @id";

                            List<MySqlParameter> parameters = new List<MySqlParameter> { DBClass.CreateParameter("id", id) };

                            // Apply date filter
                            if (!chkDate.InvokeRequired)
                            {
                                if (!chkDate.Checked)
                                {
                                    sql += " AND t.date >= @startDate AND t.date <= @endDate";
                                    parameters.Add(DBClass.CreateParameter("startDate", dtFrom.Value.Date));
                                    parameters.Add(DBClass.CreateParameter("endDate", dtTo.Value.Date));
                                }
                            }
                            else
                            {
                                Invoke(new MethodInvoker(() =>
                                {
                                    if (!chkDate.Checked)
                                    {
                                        sql += " AND t.date >= @startDate AND t.date <= @endDate";
                                        parameters.Add(DBClass.CreateParameter("startDate", dtFrom.Value.Date));
                                        parameters.Add(DBClass.CreateParameter("endDate", dtTo.Value.Date));
                                    }
                                }));
                            }

                            sql += " ORDER BY t.date;";

                            decimal runningDebit = 0, runningCredit = 0;

                            using (MySqlDataReader reader = DBClass.ExecuteReader(sql, parameters.ToArray()))
                            {
                                while (reader.Read())
                                {
                                    if (reader["Date"] == DBNull.Value)
                                        continue;

                                    decimal debit = Convert.ToDecimal(reader["debit"]);
                                    decimal credit = Convert.ToDecimal(reader["credit"]);

                                    runningDebit += debit;
                                    runningCredit += credit;

                                    DataGridViewRow row = new DataGridViewRow();
                                    row.CreateCells(dgvSales,
                                        reader["Num"].ToString(),
                                        reader["Type"].ToString(),
                                        reader["date"].ToString(),
                                        reader["Num"].ToString(),
                                        reader["description"].ToString(),
                                        debit.ToString("N2"),
                                        credit.ToString("N2"),
                                        (runningDebit - runningCredit).ToString("N2")
                                    );
                                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                    row.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);

                                    rowsToAdd.Add(row);
                                }
                            }

                            this.Invoke((MethodInvoker)delegate
                            {
                                foreach (var r in rowsToAdd)
                                    dgvSales.Rows.Add(r);
                            });
                        }
                    }
                });
            }
        }

        //private void LoadDataOld()
        //{
        //    dgvSales.Rows.Clear();

        //    if (dgvSales.Columns.Count == 0)
        //    {
        //        dgvSales.Columns.Add("id", "");
        //        dgvSales.Columns.Add("Type", "Type");
        //        dgvSales.Columns.Add("Date", "Date");
        //        dgvSales.Columns.Add("Num", "Num");
        //        dgvSales.Columns.Add("Description", "Description");
        //        dgvSales.Columns.Add("Debit", "Debit");
        //        dgvSales.Columns.Add("Credit", "Credit");
        //        dgvSales.Columns.Add("Balance", "Balance");
        //        LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

        //        //dgvSales.Columns["Account"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        dgvSales.Columns["Debit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //        dgvSales.Columns["Credit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //        dgvSales.Columns["Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //        dgvSales.Columns["Debit"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);
        //        dgvSales.Columns["Credit"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);
        //        //dgvSales.Columns["Account"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);


        //        dgvSales.Columns["id"].Visible = false;
        //        dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //        dgvSales.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);
        //        dgvSales.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
        //        dgvSales.EnableHeadersVisualStyles = false;
        //        dgvSales.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
        //        dgvSales.DefaultCellStyle.BackColor = System.Drawing.Color.White;
        //        dgvSales.GridColor = System.Drawing.Color.LightGray;
        //        dgvSales.BorderStyle = System.Windows.Forms.BorderStyle.None;
        //        dgvSales.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        //        dgvSales.RowHeadersVisible = false;
        //    }

        //    decimal totalAmount = 0, totalDebit = 0, totalCredit = 0;

        //    using (MySqlDataReader readerL = DBClass.ExecuteReader("select id,CONCAT(code,' - ',name) name from tbl_coa_level_4 WHERE ID IN(SELECT DISTINCT account_id FROM tbl_transaction);"))
        //        while (readerL.Read())
        //        {
        //            int id = int.Parse(readerL["id"].ToString());
        //            dgvSales.Rows.Add(id, readerL["name"].ToString(), "", "", "", "", "", "", "");
        //            decimal lineTotalDebit = 0, lineTotalCredit = 0;

        //            string sql = @"
        //                            SELECT 
        //                                DATE_FORMAT(t.date, '%M %d %Y') AS date,
        //                                t.transaction_id AS `Num`,
        //                                t.type AS `Type`,
        //                                t.t_type, 
        //                                t.description,
        //                                t.debit,
        //                                t.credit,
        //                                (t.debit - t.credit) AS amount
        //                            FROM tbl_transaction t
        //                            INNER JOIN tbl_coa_level_4 l4 ON t.account_id = l4.id
        //                            WHERE t.account_id = @id
        //                        ";

        //            List<MySqlParameter> parameters = new List<MySqlParameter>
        //            {
        //                DBClass.CreateParameter("id", id)
        //            };

        //            if (!chkDate.Checked)
        //            {
        //                sql += " AND t.date >= @startDate AND t.date <= @endDate";
        //                parameters.Add(DBClass.CreateParameter("startDate", dtFrom.Value.Date));
        //                parameters.Add(DBClass.CreateParameter("endDate", dtTo.Value.Date));
        //            }
        //            sql += " ORDER BY t.date;";
        //            using (MySqlDataReader reader = DBClass.ExecuteReader(sql, parameters.ToArray()))
        //            {
        //                decimal subTotalDebit = 0, subTotalCredit = 0;
        //                while (reader.Read())
        //                {
        //                    if (reader["Date"] == DBNull.Value)
        //                        continue;

        //                    decimal debit = Convert.ToDecimal(reader["debit"]);
        //                    decimal credit = Convert.ToDecimal(reader["credit"]);


        //                    subTotalDebit += debit;
        //                    subTotalCredit += credit;

        //                    int rowIndex = dgvSales.Rows.Add(reader["Num"].ToString(), reader["Type"].ToString(), reader["date"].ToString(), reader["Num"].ToString(), reader["description"].ToString(), debit.ToString("N2"), credit.ToString("N2"), (subTotalDebit - subTotalCredit).ToString("N2"));
        //                    dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
        //                    dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);

        //                }
        //                lineTotalDebit += subTotalDebit;
        //                lineTotalCredit += subTotalCredit;

        //            }

        //            // Add total row
        //            int totalRow0 = dgvSales.Rows.Add(null, null, null, null, "TOTAL", lineTotalDebit.ToString("N2"), lineTotalCredit.ToString("N2"), (lineTotalDebit - lineTotalCredit).ToString("N2"));
        //            dgvSales.Rows[totalRow0].DefaultCellStyle.Font = new System.Drawing.Font("TSegoe", 8F, FontStyle.Bold | FontStyle.Underline);
        //            dgvSales.Rows[totalRow0].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
        //            dgvSales.Rows[totalRow0].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
        //            dgvSales.Columns["id"].Visible = false;
        //            dgvSales.Rows[totalRow0].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
        //            dgvSales.Rows[totalRow0].Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleRight;

        //            totalAmount += lineTotalDebit - lineTotalCredit;
        //        }
        //    int totalRow = dgvSales.Rows.Add(null, null, null, null, "", "", "TOTAL", totalAmount.ToString("N2"), totalAmount.ToString("N2"));
        //    dgvSales.Rows[totalRow].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold | FontStyle.Underline);
        //    dgvSales.Rows[totalRow].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
        //    dgvSales.Rows[totalRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
        //    dgvSales.Columns["id"].Visible = false;
        //    dgvSales.Rows[totalRow].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
        //    dgvSales.Rows[totalRow].Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        //}

        private void dgvSales_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvSales.Rows[e.RowIndex].Cells["id"].Value != null)
                {
                    int _id = int.Parse(dgvSales.Rows[e.RowIndex].Cells["id"].Value.ToString());
                    var _type = dgvSales.Rows[e.RowIndex].Cells["Type"].Value.ToString();
                    //if (_type.Contains("Sales"))
                    //{
                    //    if(_type.StartsWith("SalesReturn"))//SalesReturn Invoice
                    //        frmLogin.frmMain.openChildForm(new frmSalesReturn(_id));
                    //    else if (_type == "Sales Invoice" || _type == "Sales Invoice Cash")
                    //        frmLogin.frmMain.openChildForm(new frmSales(_id, "", 0));
                    //}
                    //else if (_type.Contains("Purchase"))
                    //{
                    //    if (_type.StartsWith("Purchase Return"))//Purchase Return Invoice
                    //        frmLogin.frmMain.openChildForm(new frmPurchaseReturn(_id));
                    //    else if (_type == "Purchase Invoice" || _type == "Purchase Invoice Cash")
                    //        frmLogin.frmMain.openChildForm(new frmPurchase(_id, "", 0));
                    //}
                    //else if (_type.Contains("Receipt"))
                    //{
                    //    //Customer Receipt
                    //    frmLogin.frmMain.openChildForm(new frmViewReceiptVoucher(_id, 0));
                    //}
                    //else if (_type.Contains("Advance Payment")) //Customer Advance Payment
                    //{
                    //    frmLogin.frmMain.openChildForm(new frmViewPaymentVoucherAdvance(_id, 0));
                    //}
                    //else if (_type.Contains("Payment")|| _type == "Employee Salary")//Employee Salary
                    //{
                    //    //Vendor Payment //Employee Salary Payment //Employee Petty Cash Payment
                    //    frmLogin.frmMain.openChildForm(new frmViewPaymentVoucher(_id, 0));
                    //}
                    //else if(_type.Contains("Opening Balance"))
                    //{
                    //    if (_type == "Customer Opening Balance")
                    //        new frmViewCustomer(_id));
                    //    else if (_type == "Vendor Opening Balance")
                    //        new frmViewVendor(_id));
                    //    else if (_type == "General Ledger Opening Balance")
                    //        new frmEditLevel4(_id);
                    //}
                    //else if (_type == "Debit Note")
                    //{
                    //    frmLogin.frmMain.openChildForm(new frmDebitNote(_id));
                    //}
                    //else if (_type == "Credit Note")
                    //{
                    //    frmLogin.frmMain.openChildForm(new frmCreditNote(_id));
                    //}
                    //else if (_type == "Fixed Assets")
                    //{
                    //    frmLogin.frmMain.openChildForm(new frmViewFixedAssets(_id));
                    //}
                    //else if (_type == "Leave Salary")//Leave Salary
                    //{
                    //    frmLogin.frmMain.openChildForm(new frmLeaveSalary(_id));
                    //}
                    //else if (_type == "End Of Service")//End Of Service
                    //{
                    //    frmLogin.frmMain.openChildForm(new frmEndOfService(_id));
                    //}
                    //else if (_type == "Prepaid Expense")//Prepaid Expense
                    //{
                    //    frmLogin.frmMain.openChildForm(new frmViewPrepaidExpense(_id));
                    //}
                    //else if (_type == "PDC Receivable")//PDC Receivable
                    //{
                    //    frmLogin.frmMain.openChildForm(new MasterPDC());
                    //}
                    //else if (_type == "Petty Cash Submission")
                    //{
                    //    frmLogin.frmMain.openChildForm(new frmPettyCashSubmission(_id));
                    //}
                    //else if (_type == "Loan Request")
                    //{
                    //    frmLogin.frmMain.openChildForm(new frmViewLoan(_id));
                    //}
                    //else if (_type == "Petty Cash Submition Approval")
                    //{
                    //    frmLogin.frmMain.openChildForm(new MasterPettyCashApprovalSubmission(_id));
                    //}
                    //else if (_type == "JOURNAL VOUCHER")
                    //{
                    //    frmLogin.frmMain.openChildForm(new frmViewJournalVoucher(_id));
                    //}
                    //if (_type == "Opening Qty")
                    //{
                    //    frmLogin.frmMain.openChildForm(new frmOpeningQty());
                    //}
                    //Project Tender
                    //RMS Bill
                    //else
                    //    frmLogin.frmMain.openChildForm(new MasterTransactionJournal(dgvSales.Rows[e.RowIndex].Cells["Num"].Value.ToString(), _type));
                    frmLogin.frmMain.openChildForm(new frmGeneralLedgerDetails(_id, _type));
                }
            }
        }

        private void SavePDF_Click(object sender, EventArgs e)
        {
            string companyName = guna2HtmlLabel8.Text;

            Document doc = new Document();
            Section section = doc.AddSection();

            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);

            section.PageSetup.Orientation = Orientation.Landscape;

            Table headerTable = section.AddTable();
            headerTable.Borders.Width = 0;
            headerTable.AddColumn("5cm");
            headerTable.AddColumn("8cm");
            headerTable.AddColumn("5cm");
            Row headerRow = headerTable.AddRow();

            Paragraph left = headerRow.Cells[0].AddParagraph();
            left.Format.Font.Name = "Times New Roman";
            left.Format.Font.Size = 10;
            left.Format.Alignment = ParagraphAlignment.Left;
            headerRow.Cells[0].VerticalAlignment = VerticalAlignment.Top;
            left.AddText(DateTime.Now.ToString("hh:mm tt") + "\n" + DateTime.Now.ToString("dd/MM/yyyy"));

            Paragraph center = headerRow.Cells[1].AddParagraph();
            center.Format.Font.Name = "Times New Roman";
            center.Format.Alignment = ParagraphAlignment.Center;
            center.Format.SpaceAfter = 0;
            center.AddFormattedText(companyName.ToUpper() + "\n", TextFormat.Bold).Font.Size = 12;
            center.AddFormattedText("General Ledger\n", TextFormat.Bold).Font.Size = 12;
            center.AddFormattedText("All Transactions", TextFormat.NotBold).Font.Size = 9;
            headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;

            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            Table dataTable = section.AddTable();
            dataTable.Borders.Width = 0;

            foreach (DataGridViewColumn col in dgvSales.Columns)
            {
                dataTable.AddColumn(Unit.FromCentimeter(4));
            }

            Row headerDataRow = dataTable.AddRow();
            int colIndex = 0;
            foreach (DataGridViewColumn col in dgvSales.Columns)
            {
                headerDataRow.Cells[colIndex].AddParagraph(col.HeaderText).Format.Alignment = ParagraphAlignment.Center;
                headerDataRow.Cells[colIndex].Format.Font.Bold = true;
                headerDataRow.Cells[colIndex].Format.Font.Name = "Times New Roman";
                headerDataRow.Cells[colIndex].Format.Font.Size = 10;
                colIndex++;
            }

            decimal cumulativeBalance = 0;
            decimal totalDebit = 0;

            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;

                Row tRow = dataTable.AddRow();
                colIndex = 0;

                foreach (DataGridViewColumn col in dgvSales.Columns)
                {
                    string cellValue = row.Cells[col.Name].Value?.ToString() ?? "";

                    if (col.HeaderText.ToUpper() == "DEBIT")
                    {
                        decimal debit = 0;
                        decimal.TryParse(cellValue, out debit);
                        cumulativeBalance += debit;
                        totalDebit += debit;
                        tRow.Cells[colIndex].AddParagraph(debit.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
                    }
                    else if (col.HeaderText.ToUpper() == "BALANCE")
                    {
                        tRow.Cells[colIndex].AddParagraph(cumulativeBalance.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
                    }
                    else
                    {
                        tRow.Cells[colIndex].AddParagraph(cellValue);
                    }

                    tRow.Cells[colIndex].Format.Font.Name = "Times New Roman";
                    tRow.Cells[colIndex].Format.Font.Size = 9;

                    if (cellValue.ToUpper().Contains("TOTAL"))
                        tRow.Cells[colIndex].Format.Font.Bold = true;

                    colIndex++;
                }
            }

            Row totalRow = dataTable.AddRow();
            colIndex = 0;
            foreach (DataGridViewColumn col in dgvSales.Columns)
            {
                if (col.HeaderText.ToUpper() == "TYPE")
                    totalRow.Cells[colIndex].AddParagraph("TOTAL").Format.Font.Bold = true;
                else if (col.HeaderText.ToUpper() == "DEBIT")
                    totalRow.Cells[colIndex].AddParagraph(totalDebit.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
                else if (col.HeaderText.ToUpper() == "BALANCE")
                    totalRow.Cells[colIndex].AddParagraph(cumulativeBalance.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
                colIndex++;
            }

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "GeneralLedger.pdf");
            renderer.PdfDocument.Save(filePath);
            Process.Start("explorer.exe", filePath);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            CreateExcel();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            contextMenuExport.Show(btnPrint, new Point(0, btnPrint.Height));

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            contextMenuExport.Show(btnPrint, new Point(0, btnPrint.Height));

        }

        private void btnToWord_Click(object sender, EventArgs e)
        {

        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            CreateExcel();
        }

        private void CreateExcel()
        {
            SaveFileDialog saveDialog = new SaveFileDialog()
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Save Excel File",
                FileName = "GeneralLedger.xlsx"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = false;
                var workbook = excelApp.Workbooks.Add(Type.Missing);
                var worksheet = (Excel.Worksheet)workbook.Sheets[1];
                worksheet.Name = "General Ledger";

                Excel.Range headerRange = worksheet.Range["A1", "F1"];
                headerRange.Merge();
                headerRange.Value = DateTime.Now.ToString("MMM dd, yy");
                headerRange.Font.Bold = true;
                headerRange.Font.Name = "Times New Roman";
                headerRange.Font.Size = 10;
                headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                int colIndex = 1;
                foreach (DataGridViewColumn col in dgvSales.Columns)
                {
                    worksheet.Cells[2, colIndex] = col.HeaderText;
                    worksheet.Cells[2, colIndex].Font.Bold = true;
                    worksheet.Cells[2, colIndex].Font.Name = "Times New Roman";
                    worksheet.Cells[2, colIndex].Font.Size = 10;
                    worksheet.Cells[2, colIndex].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    colIndex++;
                }

                int rowIndex = 3;
                decimal cumulativeBalance = 0;

                foreach (DataGridViewRow row in dgvSales.Rows)
                {
                    if (row.IsNewRow) continue;

                    colIndex = 1;
                    foreach (DataGridViewColumn col in dgvSales.Columns)
                    {
                        string cellValue = row.Cells[col.Name].Value?.ToString();

                        if (col.HeaderText.ToUpper() == "DEBIT")
                        {
                            decimal debit = 0;
                            decimal.TryParse(cellValue, out debit);
                            cumulativeBalance += debit;
                            worksheet.Cells[rowIndex, colIndex] = debit.ToString("N2");
                        }
                        else if (col.HeaderText.ToUpper() == "BALANCE")
                        {
                            worksheet.Cells[rowIndex, colIndex] = cumulativeBalance.ToString("N2");
                        }
                        else
                        {
                            worksheet.Cells[rowIndex, colIndex] = cellValue;
                        }

                        var excelCell = worksheet.Cells[rowIndex, colIndex];
                        excelCell.Font.Name = "Times New Roman";
                        excelCell.Font.Size = 9;

                        if (!string.IsNullOrEmpty(cellValue) && cellValue.ToUpper().Contains("TOTAL"))
                        {
                            excelCell.Font.Bold = true;
                            excelCell.Font.Underline = Excel.XlUnderlineStyle.xlUnderlineStyleDouble;
                        }

                        if (col.HeaderText.ToUpper() == "DEBIT" || col.HeaderText.ToUpper() == "BALANCE")
                            excelCell.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                        colIndex++;
                    }
                    rowIndex++;
                }

                worksheet.Columns.AutoFit();

                workbook.SaveAs(saveDialog.FileName);
                workbook.Close();
                excelApp.Quit();

                MessageBox.Show("Excel exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtFrom.Enabled = dtTo.Enabled = !chkDate.Checked;
            LoadData();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void report_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            //if (ledgerTable !=null && ledgerTable.Rows.Count > 0)
            //{
            //    if (!string.IsNullOrWhiteSpace(TxtSearch.Text.Trim()))
            //    {
            //        string searchText = TxtSearch.Text.Trim().ToLower();
            //        string searchType = CmbFilterType.Text.Trim().ToLower();
            //        FilterLedger(searchType, searchText);
            //    } else
            //    {
            //        dgvSales.DataSource = null;
            //        dgvSales.DataSource = ledgerTable;
            //        FormatGrid();
            //    }
            //}
        }
    }
}
