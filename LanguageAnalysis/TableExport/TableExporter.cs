using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OfficeOpenXml;

namespace TableExport
{
    public static class TableExporter
    {
        public static void ExportToCSV(DataGridView dgv)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "Експорт таблиці в CSV";
                saveDialog.Filter = "CSV Files|*.csv";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();

                        // Only export visible columns
                        var visibleColumns = dgv.Columns.Cast<DataGridViewColumn>()
                            .Where(col => col.Visible)
                            .ToList();

                        // Заголовки
                        var headers = visibleColumns
                            .Select(col => EscapeCsv(col.HeaderText));
                        sb.AppendLine(string.Join(",", headers));

                        // Данные
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            // Skip new row for input
                            if (row.IsNewRow) continue;

                            var cells = visibleColumns
                                .Select(col =>
                                {
                                    var value = row.Cells[col.Index].Value;
                                    return EscapeCsv(value?.ToString() ?? string.Empty);
                                });
                            sb.AppendLine(string.Join(",", cells));
                        }

                        File.WriteAllText(saveDialog.FileName, sb.ToString(), Encoding.UTF8);
                        MessageBox.Show("Експорт завершено успішно.", "Експорт CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при експорті: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Escapes a value for CSV (wraps in quotes, doubles quotes inside)
        private static string EscapeCsv(string value)
        {
            if (value.Contains("\"") || value.Contains(",") || value.Contains("\n") || value.Contains("\r"))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }

        public static void ExportToExcel(DataGridView dgv)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files|*.xlsx";
                saveDialog.Title = "Зберегти Excel файл";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    // Настройка лицензии (для версий EPPlus 5+)
                    ExcelPackage.License.SetNonCommercialPersonal(Environment.UserName);

                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                        // Заголовки столбцов
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i + 1].Value = dgv.Columns[i].HeaderText;
                        }

                        // Данные
                        for (int row = 0; row < dgv.Rows.Count; row++)
                        {
                            // Пропустить пустую строку (если AllowUserToAddRows=true)
                            if (dgv.Rows[row].IsNewRow) continue;

                            for (int col = 0; col < dgv.Columns.Count; col++)
                            {
                                worksheet.Cells[row + 2, col + 1].Value =
                                    dgv.Rows[row].Cells[col].Value?.ToString();
                            }
                        }

                        // Авто-ширина столбцов
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        // Сохранение
                        FileInfo excelFile = new FileInfo(saveDialog.FileName);
                        excelPackage.SaveAs(excelFile);
                    }

                    MessageBox.Show("Експорт завершено успішно!");
                }
            }
        }
    }
}
