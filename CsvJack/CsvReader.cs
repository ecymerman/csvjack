using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvJack
{

    public class CsvReader : IDisposable
    {

        TextReader textReader;
        CsvReaderOptions options;

        public CsvReader(TextReader textReader, CsvReaderOptions options = null)
        {
            this.textReader = textReader;
            this.options = options ?? new CsvReaderOptions();
        }

        public IEnumerable<string[]> ReadRows()
        {
            string[] row;
            while ((row = ReadRow()) != null)
            {
                yield return row;
            }
        }

        public string[] ReadRow()
        {
            var sb = new StringBuilder();
            var parts = new List<string>();
            bool inQuote = false;
            while (true)
            {
                int c = textReader.Read();

                if (options.EscapeWithSlash && inQuote && c == '\\' && textReader.Peek() >= 0)
                {
                    sb.Append((char)textReader.Read());
                    continue;
                }

                if (inQuote && c >= 0)
                {
                    if (c == '"')
                    {
                        if (textReader.Peek() == '"')
                        {
                            textReader.Read();
                            sb.Append('"');
                            continue;
                        }
                        inQuote = false;
                        continue;
                    }
                    sb.Append((char)c);
                    continue;
                }
                if (c == '"')
                {
                    if (sb.Length == 0)
                    {
                        inQuote = true;
                        continue;
                    }
                    sb.Append('"');
                    continue;
                }
                if (c == ',')
                {
                    addPart();
                    continue;
                }
                if (c == '\r' || c == '\n' || c < 0)
                {
                    if (c < 0 && parts.Count == 0 && sb.Length == 0)
                        return null;
                    addPart();
                    if (c == '\r' && textReader.Peek() == '\n')
                        textReader.Read();
                    return parts.ToArray();
                }
                sb.Append((char)c);
            }

            void addPart()
            {
                if (sb.Length == 0 && options.NullIfEmpty)
                    parts.Add(null);
                else
                    parts.Add(sb.ToString());
                sb.Clear();
            }
        }

        public void Dispose()
        {
            textReader.Dispose();
        }

    }

    public class CsvReaderOptions
    {
        public bool EscapeWithSlash;
        public bool NullIfEmpty;
    }

}
