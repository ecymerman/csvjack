# CsvJack
A performant CSV reader in ~100 lines of C#.<br>
Supports quoted values, and embedded line breaks.<br>
Apache 2.0 license.

## Usage
The namespace is `CsvJack`.
### CsvReader
The  constructor takes a `TextReader`, and optionally a `CsvReaderOptions`. The latter has the following fields:
* `EscapeWithSlash` Escape quotes in a quotes value, using a backslash, instead of a double quote.
* `NullIfEmpty` Return a value as `null` if it's an empty string.

Use the following methods to read the data.
* `string[] ReadRow()` Returns a single row.
* `IEnumerable<string[]> ReadRows()` Returns an `IEnumerable` of the rows.

Rows are never returned twice, even across different method calls.

That's it!
