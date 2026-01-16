using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmBoqImport : Form
    {

        public string SelectedSnColumn { get; private set; }
        public string SelectedNameColumn { get; private set; }
        public DataTable dataTable;


        public frmBoqImport()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            //this.id = id;
            //this.master = _master;
            //if (id == 0)
            this.Text = "BOQ Import";
            headerUC1.FormText = this.Text;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private List<Dictionary<string, string>> excelRows = new List<Dictionary<string, string>>(); // List of row dictionaries
        private Dictionary<string, string> columnMap = new Dictionary<string, string>();       // ColumnLetter -> Header

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboSnColumn.SelectedItem != null && comboNameColumn.SelectedItem != null && comboAmountColumn.SelectedItem != null && comboQtyColumn.SelectedItem != null && comboRateColumn.SelectedItem != null && comboUnitColumn.SelectedItem != null && comboLengthColumn.SelectedItem != null && comboWidthColumn.SelectedItem != null && comboThicknessColumn.SelectedItem != null && comboNoteColumn.SelectedItem != null)
                {
                    // Get selected column letters from ComboBoxes
                    string snCol = ((KeyValuePair<string, string>)comboSnColumn.SelectedItem).Key;
                    string nameCol = ((KeyValuePair<string, string>)comboNameColumn.SelectedItem).Key;
                    string amountCol = ((KeyValuePair<string, string>)comboAmountColumn.SelectedItem).Key;
                    string qtyCol = ((KeyValuePair<string, string>)comboQtyColumn.SelectedItem).Key;
                    string rateCol = ((KeyValuePair<string, string>)comboRateColumn.SelectedItem).Key;
                    string unitCol = ((KeyValuePair<string, string>)comboUnitColumn.SelectedItem).Key;
                    string lengthCol = ((KeyValuePair<string, string>)comboLengthColumn.SelectedItem).Key;
                    string widthCol = ((KeyValuePair<string, string>)comboWidthColumn.SelectedItem).Key;
                    string thicknessCol = ((KeyValuePair<string, string>)comboThicknessColumn.SelectedItem).Key;
                    string comboNoteCol = ((KeyValuePair<string, string>)comboNoteColumn.SelectedItem).Key;

                    // Create DataTable
                    DataTable table = new DataTable();
                    table.Columns.Add("Sr.");
                    table.Columns.Add("Description of work");
                    table.Columns.Add("Qty");
                    table.Columns.Add("Unit");
                    table.Columns.Add("Length");
                    table.Columns.Add("Width");
                    table.Columns.Add("Thick");
                    table.Columns.Add("Rate");
                    table.Columns.Add("Amount");
                    table.Columns.Add("Note");

                    // Fill DataTable from cached Excel data
                    foreach (var row in excelRows)
                    {
                        string sn = row.ContainsKey(snCol) ? row[snCol] : "";
                        string name = row.ContainsKey(nameCol) ? row[nameCol] : "";
                        string amount = row.ContainsKey(amountCol) ? row[amountCol].Replace(",", "") : "";
                        string qty = row.ContainsKey(qtyCol) ? row[qtyCol].Replace(",", "") : "";
                        string rate = row.ContainsKey(rateCol) ? row[rateCol].Replace(",", "") : "";
                        string unit = row.ContainsKey(unitCol) ? row[unitCol] : "";
                        string length = row.ContainsKey(lengthCol) ? row[lengthCol] : "";
                        string width = row.ContainsKey(widthCol) ? row[widthCol].Replace(",", "") : "";
                        string thickness = row.ContainsKey(thicknessCol) ? row[thicknessCol].Replace(",", "") : "";
                        string note = row.ContainsKey(comboNoteCol) ? row[comboNoteCol] : "";

                        if (string.IsNullOrWhiteSpace(name))
                            continue;

                        table.Rows.Add(sn, name, qty, unit, length, width, thickness, rate, amount, note);
                    }

                    // Store result
                    this.dataTable = table;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("All value must selected");
                }
            }
            catch
            {
                //
            }
        }

        private void frmBoqImport_Load(object sender, EventArgs e)
        {
            //
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    openFileDialog.Title = "Select Excel File";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        LoadExcelSheets(filePath);
                    }
                }
            }
            catch { }
        }

        private void LoadExcelSheets(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("The selected file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int selectedIndex = 0, lastIndex = 0;
            if (!string.IsNullOrEmpty(txtStartingIndex.Text))
            {
                selectedIndex = int.Parse(txtStartingIndex.Text.ToString());
            }
            if (!string.IsNullOrEmpty(txtEndingIndex.Text))
            {
                lastIndex = int.Parse(txtEndingIndex.Text.ToString());
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // temp

                //var headers = new Dictionary<string, string>();

                //for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                //{
                //    string headerText = worksheet.Cells[selectedIndex, col].Text?.Trim();
                //    if (!string.IsNullOrEmpty(headerText))
                //    {
                //        string colLetter = ExcelHelper.GetColumnLetter(col);
                //        headers[colLetter] = headerText;
                //    }
                //}

                // Cache worksheet data
                int dataStartRow = selectedIndex;
                for (int row = dataStartRow; row <= lastIndex; row++)
                {
                    var rowData = new Dictionary<string, string>();

                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        string colLetter = ExcelHelper.GetColumnLetter(col);
                        string cellValue = worksheet.Cells[row, col].Text?.Trim();

                        rowData[colLetter] = cellValue;
                    }

                    // Only add non-empty rows
                    if (rowData.Values.Any(v => !string.IsNullOrEmpty(v)))
                    {
                        excelRows.Add(rowData);
                    }
                }

                //columnMap = headers;

                //foreach (var kvp in headers)
                //{
                //    string display = $"{kvp.Key}: {kvp.Value}";
                //    comboSnColumn.Items.Add(new KeyValuePair<string, string>(kvp.Key, display));
                //    comboNameColumn.Items.Add(new KeyValuePair<string, string>(kvp.Key, display));
                //    comboAmountColumn.Items.Add(new KeyValuePair<string, string>(kvp.Key, display));
                //    comboQtyColumn.Items.Add(new KeyValuePair<string, string>(kvp.Key, display));
                //    comboRateColumn.Items.Add(new KeyValuePair<string, string>(kvp.Key, display));
                //    comboUnitColumn.Items.Add(new KeyValuePair<string, string>(kvp.Key, display));
                //}
                for (int col = 1; col <= 26; col++) // A to Z
                {
                    string colLetter = ExcelHelper.GetColumnLetter(col); // Or your own logic: ((char)(64 + col)).ToString();
                    string display = $"{colLetter}";

                    var item = new KeyValuePair<string, string>(colLetter, display);

                    comboSnColumn.Items.Add(item);
                    comboNameColumn.Items.Add(item);
                    comboAmountColumn.Items.Add(item);
                    comboQtyColumn.Items.Add(item);
                    comboRateColumn.Items.Add(item);
                    comboUnitColumn.Items.Add(item);
                    comboLengthColumn.Items.Add(item);
                    comboWidthColumn.Items.Add(item);
                    comboThicknessColumn.Items.Add(item);
                    comboNoteColumn.Items.Add(item);
                }

                comboSnColumn.DisplayMember = "Value";
                comboNameColumn.DisplayMember = "Value";
                comboAmountColumn.DisplayMember = "Value";
                comboQtyColumn.DisplayMember = "Value";
                comboRateColumn.DisplayMember = "Value";
                comboUnitColumn.DisplayMember = "Value";
                comboLengthColumn.DisplayMember = "Value";
                comboWidthColumn.DisplayMember = "Value";
                comboThicknessColumn.DisplayMember = "Value";
                comboNoteColumn.DisplayMember = "Value";
            }
        }
        

        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Block the character
            }
        }
    }
}

public static class ExcelHelper
{
    public static string GetColumnLetter(int columnNumber)
    {
        string columnString = "";
        while (columnNumber > 0)
        {
            int currentLetterNumber = (columnNumber - 1) % 26;
            columnString = (char)(currentLetterNumber + 65) + columnString;
            columnNumber = (columnNumber - currentLetterNumber - 1) / 26;
        }
        return columnString;
    }

    public static int GetColumnIndex(string columnLetter)
    {
        int sum = 0;
        for (int i = 0; i < columnLetter.Length; i++)
        {
            sum *= 26;
            sum += (columnLetter[i] - 'A' + 1);
        }
        return sum;
    }

}