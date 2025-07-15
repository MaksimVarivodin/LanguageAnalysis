# LanguageAnalysis

## Overview
LanguageAnalysis is a tool designed for analyzing text data, generating n-grams, and exporting results in various formats. It supports processing text in multiple languages, including Japanese and Chinese, and provides utilities for handling files and folders.

## Features
- **N-Gram Generation**: Create symbol, literal, and word-based n-grams.
- **Text Preprocessing**: Remove unnecessary spaces, handle special characters, and normalize text.
- **Multi-Language Support**: Analyze text in languages like Japanese and Chinese using tokenization.
- **Export Utilities**: Export data to CSV, Excel, or text files.
- **Folder Validation**: Check folder validity, existence, and readability.
- **Performance Optimization**: Utilize parallel processing for faster computations.

## Requirements
- .NET Framework or .NET Core
- EPPlus library for Excel export
- JiebaNet.Segmenter for Chinese tokenization
- NMeCab for Japanese tokenization
- Ude for character encoding detection

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/your-repo/LanguageAnalysis.git
   ```
2. Install the required NuGet packages:
   - EPPlus
   - JiebaNet.Segmenter
   - NMeCab
   - Ude

## Usage
### N-Gram Processing
1. Initialize the `NgrammProcessor` with a file path and a `ProgressReporter` instance.
2. Call the appropriate methods to preprocess text and generate n-grams:
   - `ProcessSymbolNGramms(int n)`
   - `ProcessLiteralNGramms(int n)`
   - `ProcessWordNGramms(int n)`

### Exporting Data
Use the `TableExporter` class to export `DataGridView` content to:
- CSV: `SaveToCSV(DataGridView dgv)`
- Excel: `SaveToExcel(DataGridView dgv)`
- Text: `SaveSelectedFile(DataGridView dgv)`

### Folder Validation
Use the `FolderChecker` class to validate folder paths:
- `IsValidFolder(string folderName)`

## Examples
### Generate Symbol N-Grams
```csharp
var processor = new NgrammProcessor("path/to/file.txt", new ProgressReporter());
await processor.Preprocess();
await processor.ProcessSymbolNGramms(3);
var ngrams = processor.GetSymbolNgrams();
```

### Export Data to CSV
```csharp
TableExporter.SaveToCSV(dataGridView);
```

### Validate Folder
```csharp
bool isValid = FolderChecker.IsValidFolder("path/to/folder");
```

## Contributing
Contributions are welcome! Please submit a pull request or open an issue for any bugs or feature requests.

## License
This project is licensed under the [MIT License](LICENSE.txt).
