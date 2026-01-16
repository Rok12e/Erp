using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace YamyProject
{
    public class ReportPrinter
    {
        private DataGridView dgv;
        private string headerText;
        private int currentRow;
        private int leftMargin = 50;
        private int rightMargin = 50;
        bool includeTableBorder;


      

        public ReportPrinter(DataGridView dataGridView, string header, bool includeTableBorder)
        {
            dgv = dataGridView;
            headerText = header;
            this.includeTableBorder = includeTableBorder;
            currentRow = 0;
        }

        public void Print()
        {
            currentRow = 0; // reset
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDocument
            };
            previewDialog.ShowDialog();
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            int yPosition = 10;
            int lineHeight = 20;
            int pageWidth = e.PageBounds.Width;
            int availableWidth = pageWidth - leftMargin - rightMargin;

            System.Drawing.Font headerFont = new System.Drawing.Font("Segoe UI", 16, FontStyle.Bold);
            StringFormat headerFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            string[] headerLines = headerText.Split('\n');
            int headerLineHeight = (int)headerFont.GetHeight(e.Graphics);

            foreach (string line in headerLines)
            {
                SizeF headerSize = e.Graphics.MeasureString(line, headerFont);
                float headerX = (pageWidth - headerSize.Width) / 2;

                e.Graphics.DrawString(line, headerFont, Brushes.Black, new PointF(headerX, yPosition));

                yPosition += headerLineHeight + 5; 
            }

            yPosition += 20;

            string dateText = "Printed on: " + DateTime.Now.ToString("MM/dd/yyyy");
            e.Graphics.DrawString(dateText, new System.Drawing.Font("Segoe UI", 10), Brushes.Black, new PointF(leftMargin, yPosition));

            yPosition += 20;
            e.Graphics.DrawLine(Pens.Black, leftMargin, yPosition, pageWidth - rightMargin, yPosition);
            yPosition += 10;

            Dictionary<int, int> columnWidths = new Dictionary<int, int>();
            int totalColumns = dgv.Columns.Cast<DataGridViewColumn>().Count(c => c.Visible);
            MessageBox.Show("Visible Columns: " + totalColumns);

            if (totalColumns > 0)
            {
                int equalColumnWidth = availableWidth / totalColumns;

                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    if (column.Visible)
                    {
                        columnWidths[column.Index] = equalColumnWidth;
                    }
                }
            }

            int totalColumnWidth = columnWidths.Values.Sum();
            int centerMargin = (availableWidth - totalColumnWidth) / 2;
            int currentLeftMargin = leftMargin + centerMargin;

            System.Drawing.Font headerUnderlineFont = new System.Drawing.Font("Segoe UI", 10, FontStyle.Underline);
            int headerXPosition = currentLeftMargin;

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Visible)
                {
                    e.Graphics.DrawString(column.HeaderText, headerUnderlineFont, Brushes.Black, new PointF(headerXPosition, yPosition));
                    if(includeTableBorder)
                    e.Graphics.DrawRectangle(Pens.Black, headerXPosition, yPosition, columnWidths[column.Index], lineHeight); 
                    headerXPosition += columnWidths[column.Index];
                }
            }

            yPosition += lineHeight;

            StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near,
                Trimming = StringTrimming.Word
            };

            while (currentRow < dgv.Rows.Count)
            {
                int rowXPosition = currentLeftMargin;
                int maxRowHeight = lineHeight; 

                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    if (dgv.Rows[currentRow].IsNewRow)
                    {
                        currentRow++;
                        continue;
                    }
                    if (column.Visible)
                    {
                        string cellValue = dgv.Rows[currentRow].Cells[column.Index].Value?.ToString() ?? "";
                        RectangleF cellBounds = new RectangleF(rowXPosition, yPosition, columnWidths[column.Index], lineHeight * 3);

                        SizeF textSize = e.Graphics.MeasureString(cellValue, dgv.Font, columnWidths[column.Index]);
                        int requiredHeight = (int)Math.Ceiling(textSize.Height) + 5;
                        maxRowHeight = Math.Max(maxRowHeight, requiredHeight);
                    }
                }

                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    if (column.Visible)
                    {
                        string cellValue = dgv.Rows[currentRow].Cells[column.Index].Value?.ToString() ?? "";
                        RectangleF cellBounds = new RectangleF(rowXPosition+3, yPosition+5, columnWidths[column.Index], maxRowHeight);
                        e.Graphics.DrawString(cellValue, dgv.Font, Brushes.Black, cellBounds, stringFormat);
                        if (includeTableBorder)
                            e.Graphics.DrawRectangle(Pens.Black, rowXPosition, yPosition, columnWidths[column.Index], maxRowHeight); 
                        rowXPosition += columnWidths[column.Index];
                    }
                }

                yPosition += maxRowHeight; 
                currentRow++;

                if (yPosition + maxRowHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            e.HasMorePages = false;
        }
    }
    public class ExcelExporter
    {
        private DataGridView dgv;

        public ExcelExporter(DataGridView dataGridView)
        {
            dgv = dataGridView;
        }

        public void ExportToExcel(string filePath)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                for (int col = 0; col < dgv.Columns.Count; col++)
                {
                    if (dgv.Columns[col].Visible)
                    {
                        worksheet.Cells[1, col + 1].Value = dgv.Columns[col].HeaderText;
                    }
                }

                for (int row = 0; row < dgv.Rows.Count; row++)
                {
                    for (int col = 0; col < dgv.Columns.Count; col++)
                    {
                        if (dgv.Columns[col].Visible && dgv.Rows[row].Cells[col].Value != null)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = dgv.Rows[row].Cells[col].Value.ToString();
                        }
                    }
                }
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }

            MessageBox.Show("Export Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public class OpenXmlWordExporter
    {
        public void ExportToWord(DataGridView dgv, string filePath)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document(new Body());
                Body body = mainPart.Document.Body;
                Paragraph titleParagraph = new Paragraph(new Run(new Text("DataGridView Export")));
                titleParagraph.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center });
                body.Append(titleParagraph);

                Table table = new Table();
                TableProperties tblProperties = new TableProperties(
                    new TableWidth() { Type = TableWidthUnitValues.Pct, Width = "5000" },
                    new TableBorders(
                        new TopBorder() { Val = BorderValues.Single, Size = 4 },
                        new BottomBorder() { Val = BorderValues.Single, Size = 4 },
                        new LeftBorder() { Val = BorderValues.Single, Size = 4 },
                        new RightBorder() { Val = BorderValues.Single, Size = 4 },
                        new InsideHorizontalBorder() { Val = BorderValues.Single, Size = 4 },
                        new InsideVerticalBorder() { Val = BorderValues.Single, Size = 4 }
                    )
                );
                table.AppendChild(tblProperties);
                TableRow headerRow = new TableRow();
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    if (column.Visible)
                    {
                        TableCell cell = new TableCell(new Paragraph(new Run(new Text(column.HeaderText))));
                        headerRow.AppendChild(cell);
                    }
                }
                table.AppendChild(headerRow);
                foreach (DataGridViewRow dataRow in dgv.Rows)
                {
                    if (dataRow.IsNewRow) continue;

                    TableRow row = new TableRow();
                    foreach (DataGridViewCell cell in dataRow.Cells)
                    {
                        if (cell.Visible)
                        {
                            TableCell tableCell = new TableCell(new Paragraph(new Run(new Text(cell.Value?.ToString() ?? ""))));
                            row.AppendChild(tableCell);
                        }
                    }
                    table.AppendChild(row);
                }
                body.Append(table);
            }
            MessageBox.Show("Data exported to Word successfully!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

