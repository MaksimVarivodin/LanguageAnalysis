# TableExport Folder - Classes and Methods Documentation

This folder contains classes for exporting DataGridView contents to various file formats.

## Classes Overview

### **TableExporter** (TableExport)
Static utility class that provides methods to export DataGridView data to different file formats including CSV, Excel, and text files.

## Methods

### **SaveToCSV(DataGridView dgv)**
Exports DataGridView contents to a CSV file with semicolon separators.

**Parameters:**
- `dgv` - The DataGridView to export

**Features:**
- Only exports visible columns
- Prompts user to select save location via SaveFileDialog
- Properly escapes CSV values (quotes, commas, newlines)
- Uses UTF-8 encoding
- Shows success/error messages to user

### **SaveToExcel(DataGridView dgv)**
Exports DataGridView contents to an Excel (.xlsx) file using EPPlus library.

**Parameters:**
- `dgv` - The DataGridView to export

**Features:**
- Creates Excel workbook with "Sheet1" worksheet
- Exports all columns (visible and hidden)
- Includes column headers in first row
- Auto-fits column widths
- Skips new/empty rows
- Sets EPPlus license for non-commercial use
- Shows success message after export

### **SaveSelectedFile(DataGridView dataGridView)**
Saves DataGridView contents to a tab-separated text file.

**Parameters:**
- `dataGridView` - The DataGridView to export

**Features:**
- Uses tab character as delimiter
- Prompts user to select save location
- Handles null cell values gracefully
- Processes all rows except the last one in a loop, then handles the last row separately
- Shows success/error messages

## Private Helper Methods

### **EscapeCsv(string value)**
Internal helper method for properly escaping CSV values.

**Parameters:**
- `value` - String value to escape

**Returns:**
- Escaped CSV string wrapped in quotes if necessary

**Features:**
- Wraps values in quotes if they contain special characters (quotes, commas, newlines)
- Doubles internal quotes for proper CSV escaping
- Handles carriage returns and line feeds

## Dependencies

The class uses the following external libraries:
- **EPPlus** - For Excel file generation
- **System.Windows.Forms** - For SaveFileDialog and DataGridView
- **System.IO** - For file operations

## Usage Pattern

All methods follow a similar pattern:
1. Show SaveFileDialog to user
2. Process DataGridView data
3. Write to selected file format
4. Show success/error message

## File Format Support

- **CSV** - Semicolon-separated values with UTF-8 encoding
- **Excel** - .xlsx format with auto-fitted columns
- **Text** - Tab-separated values

## Error Handling

All export methods include try-catch blocks to handle exceptions gracefully and display user-friendly error messages.
