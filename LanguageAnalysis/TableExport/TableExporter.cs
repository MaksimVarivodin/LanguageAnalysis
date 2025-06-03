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
    /// <summary>
    /// Provides methods to export the contents of a DataGridView to various file formats.
    /// </summary>
    public static class TableExporter
    {
        /// <summary>
        /// Exports the contents of a DataGridView to a CSV file.
        /// Only visible columns are exported. Prompts the user to select the file location.
        /// </summary>
        /// <param name="dgv">The DataGridView to export.</param>
        public static void SaveToCSV(DataGridView dgv)
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
                        sb.AppendLine(string.Join(";", headers));

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
                            sb.AppendLine(string.Join(";", cells));
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

        /// <summary>
        /// Escapes a value for CSV output by wrapping in quotes and doubling any internal quotes.
        /// </summary>
        /// <param name="value">The string value to escape.</param>
        /// <returns>The escaped CSV string.</returns>
        private static string EscapeCsv(string value)
        {
            if (value.Contains("\"") || value.Contains(",") || value.Contains("\n") || value.Contains("\r"))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }

        /// <summary>
        /// Exports the contents of a DataGridView to an Excel (.xlsx) file using EPPlus.
        /// Prompts the user to select the file location.
        /// </summary>
        /// <param name="dgv">The DataGridView to export.</param>
        public static void SaveToExcel(DataGridView dgv)
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

        /// <summary>
        /// Saves the contents of a DataGridView to a text file (tab-separated values).
        /// Prompts the user to select the file location.
        /// </summary>
        /// <param name="dataGridView">The DataGridView to export.</param>
        public static void SaveSelectedFile(DataGridView dataGridView)
        {
            // Initialize SaveFileDialog
            SaveFileDialog saveFile = new SaveFileDialog
            {
                DefaultExt = "*.txt",
                Filter = "TXT Files|*.txt"
            };

            // Show the dialog and check if the user selected a file
            if (saveFile.ShowDialog() != DialogResult.OK || saveFile.FileName.Length <= 0)
                return;

            try
            {
                // Create a StreamWriter to write to the selected file
                using (var sw = new StreamWriter(saveFile.FileName))
                {
                    // Iterate through DataGridView rows and columns
                    for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
                    {
                        var forValues = new List<string>();
                        for (int j = 0; j < dataGridView.Columns.Count; j++)
                        {
                            if (dataGridView.Rows[i].Cells[j].Value != null)
                            {
                                forValues.Add(dataGridView.Rows[i].Cells[j].Value.ToString());
                            }
                        }
                        sw.WriteLine(string.Join("\t", forValues));
                    }
                    var cellValues = new List<string>();
                    for (int j = 0; j < dataGridView.Columns.Count; j++)
                    {
                        if (dataGridView.Rows[dataGridView.Rows.Count - 1].Cells[j].Value != null)
                        {
                            cellValues.Add(dataGridView.Rows[dataGridView.Rows.Count - 1].Cells[j].Value.ToString());
                        }
                    }
                    sw.Write(string.Join("\t", cellValues));

                }
                // Inform the user that the data was saved successfully
                MessageBox.Show("Дані збережено");
            }
            catch (Exception ex)
            {
                // Show an error message if something went wrong
                MessageBox.Show($"Помилка при збереженні файлу: {ex.Message}");
            }
        }
    }
}
