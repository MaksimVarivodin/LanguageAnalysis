# Parsing Folder - Classes and Methods Documentation

This folder contains classes for text processing and analysis, N-gram creation, and utility helpers.

## Classes Overview

### 1. **CategorizedTokens** (NGramm.Models)
Simple model for representing tokens with categories.

**Properties:**
- `Type` - token type (string)
- `Value` - token value (string)

### 2. **DoubleParsers** (Parsing)
Static class for safe parsing of strings to double values.

**Methods:**
- `doubleParseFromStringInvariant(string)` - parses string to double using invariant culture
- `doubleParseFromStringCurrent(string)` - parses string to double using current culture
- `doubleParseFromString(string)` - parses string trying current culture first, then invariant
- `doubleParseFromStrings(string)` - parses tab-separated string into tuple of two doubles
- `CompareDouble(double, double, double)` - compares two doubles with specified precision

### 3. **FolderChecker** (FolderWork)
Static class for checking folder validity and accessibility.

**Methods:**
- `FolderNameIsValid(string)` - validates folder name (private)
- `FolderExists(string)` - checks if folder exists (private)
- `FolderIsReadable(string)` - checks if folder is readable (private)
- `IsValidFolder(string)` - main folder validation method (public)

### 4. **NGramm** (NGramm)
Class for representing a single N-gram.

**Properties:**
- `text` - N-gram text
- `type` - N-gram type
- `count` - occurrence count
- `f` - frequency (double)

### 5. **NGrammContainer** (NGramm)
Container for storing and processing collections of N-grams.

**Properties:**
- `n` - N-gram size
- `ngrams` - dictionary of N-grams
- `ngram_reps` - grouping by occurrence count
- `ngram_reps_i` - index for grouping
- `count` - total count
- `absCount` - absolute count
- `source` - data source
- `source_unsigned` - source without punctuation

**Methods:**
- `NGrammContainer(int)` - constructor for creating container of size n
- `NGrammContainer(List<NGrammContainer>, int)` - constructor for merging containers
- `Add(string)` - adds N-gram
- `Add(string, string)` - adds N-gram with type
- `Add(string, int)` - adds N-gram with count
- `Process()` - processes and sorts N-grams
- `GetNgrams(int)` - gets N-grams with count filter

### 6. **NgrammProcessor** (NGramm)
Main class for text processing and creating different types of N-grams.

**Properties:**
- `RawTextOrg` - original text
- `UnsignedTextorg` - text without punctuation
- `EndsignedTextorg` - text with ending signs
- `CountDesiredVariables` - count of variables to process
- `Filename` - file name
- `progressReporter` - progress reporting object

**Methods:**
- `NgrammProcessor(string, ProgressReporter)` - constructor
- `GetFileContent()` - reads file content with encoding auto-detection
- `Preprocess()` - text preprocessing
- `InitializeJapaneseProcessing(string)` - initializes Japanese text processing
- `ProcessSymbolNGramms(int)` - processes character N-grams
- `ProcessLiteralNGramms(int)` - processes letter N-grams
- `ProcessWordNGramms(int)` - processes word N-grams
- `GetSymbolsCount(bool)` - counts symbols
- `GetLiteralCount(bool)` - counts letters
- `GetWordsCount()` - counts words
- `Words(string)` - splits text into words
- `TrySplitWords(string)` - attempts to split words with CJK support
- `GetLiteralNgrams()` - gets letter N-grams
- `GetSymbolNgrams()` - gets symbol N-grams
- `GetWordsNgrams()` - gets word N-grams

**Private Methods:**
- `ProcessSymbolNgrmmToContainer()` - creates symbol N-gram container
- `ProcessLiteralNgrmmToContainer()` - creates letter N-gram container
- `ProcessWordNgrmmToContainer()` - creates word N-gram container
- `AnalyzeText()` - analyzes text for CJK characters
- `TokenizeJapanese()` - tokenizes Japanese text
- `TokenizeChinese()` - tokenizes Chinese text
- `RemoveConsequtiveSpaces()` - removes consecutive spaces
- `NonRenderingCategories()` - checks for non-rendering characters
- `RemoveEndSigns()` - removes ending signs
- `IsEndSign()` - checks if character is ending sign

### 7. **PerformanceSettings** (NGramm)
Static class for configuring parallel processing performance.

**Properties:**
- `MaxCores` - maximum number of cores
- `Cores` - number of cores to use
- `MinNGrammCount` - minimum N-gram count
- `ParallelOpt` - parallel processing options

### 8. **ProgressReporter** (NGramm)
Class for reporting operation progress.

**Events:**
- `OperationNameChanged` - operation name changed
- `ProgressChanged` - progress changed
- `TimerStopRequest` - timer stop request
- `TimerStartRequest` - timer start request

**Methods:**
- `StartNewOperation(string)` - start new operation
- `MoveProgress(int)` - increment progress
- `Reset()` - reset progress
- `Finish()` - finish operation
- `StopTimer()` - stop timer
- `StartTimer()` - start timer

## Usage Overview

The classes in this folder form a system for:
1. **Text preprocessing** - cleaning, normalization, encoding detection
2. **N-gram creation** - character, letter, and word N-grams
3. **Text analysis** - support for various languages including CJK
4. **Performance management** - parallel processing settings
5. **Progress reporting** - operation progress tracking

Main workflow: `NgrammProcessor` → `Preprocess()` → `ProcessXXXNGramms()` → `NGrammContainer` → results.
