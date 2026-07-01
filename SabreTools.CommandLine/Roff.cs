using System;
using System.Collections.Generic;
using System.Text;

namespace SabreTools.CommandLine
{
    /// <summary>
    /// Helpers for emitting roff-formatted man page text
    /// </summary>
    /// <remarks>
    /// Output is restricted to the portable man(7) macro set so that the
    /// same page renders without warnings under both groff and mandoc.
    /// </remarks>
    internal static class Roff
    {
        /// <summary>
        /// Maximum input line length, in characters, before wrapping
        /// </summary>
        private const int LineWidth = 78;

        /// <summary>
        /// Escape a value for safe inclusion in roff output
        /// </summary>
        /// <param name="value">Value to escape, if any</param>
        /// <returns>The escaped value, or an empty string if null or empty</returns>
        public static string Escape(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            // Backslashes are escaped first so that escapes introduced by
            // later replacements are not themselves re-escaped
            string escaped = value!.Replace("\\", "\\e");

            // Render hyphens as literal ASCII minus signs instead of letting
            // groff translate them to Unicode hyphens in UTF-8 output
            escaped = escaped.Replace("-", "\\-");

            return escaped;
        }

        /// <summary>
        /// Escape a value for inclusion in a quoted <c>.TH</c> title field
        /// </summary>
        /// <param name="value">Value to escape, if any</param>
        /// <returns>The escaped value, or an empty string if null or empty</returns>
        /// <remarks>
        /// Unlike <see cref="Escape"/>, hyphens are left intact so that the
        /// date field remains parseable by mandoc and version strings render
        /// verbatim. Embedded quotes are escaped so they cannot terminate the
        /// surrounding field.
        /// </remarks>
        public static string EscapeField(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value!.Replace("\\", "\\e").Replace("\"", "\\(dq");
        }

        /// <summary>
        /// Format a block of text into wrapped, escaped roff output lines
        /// </summary>
        /// <param name="text">Text block to format, which may contain newlines</param>
        /// <returns>Roff-safe output lines for the text block</returns>
        public static List<string> FormatText(string? text)
        {
            List<string> output = [];
            if (string.IsNullOrEmpty(text))
                return output;

            // Normalize line endings and split into paragraphs on blank lines
            string normalized = text!.Replace("\r\n", "\n").Replace("\r", "\n");
            string[] paragraphs = normalized.Split(new string[] { "\n\n" }, StringSplitOptions.None);

            for (int i = 0; i < paragraphs.Length; i++)
            {
                // Collapse single newlines within a paragraph into spaces
                string paragraph = paragraphs[i].Replace("\n", " ").Trim();
                if (paragraph.Length == 0)
                    continue;

                // Separate consecutive paragraphs with a blank line
                if (output.Count > 0)
                    output.Add(".sp");

                output.AddRange(WrapParagraph(paragraph));
            }

            return output;
        }

        /// <summary>
        /// Wrap a single paragraph into escaped roff output lines
        /// </summary>
        /// <param name="paragraph">Single-line paragraph text to wrap</param>
        /// <returns>Escaped output lines no longer than the configured width</returns>
        private static List<string> WrapParagraph(string paragraph)
        {
            List<string> lines = [];
            string[] words = paragraph.Split(' ');

            var current = new StringBuilder();
            for (int i = 0; i < words.Length; i++)
            {
                string word = Escape(words[i]);
                if (word.Length == 0)
                    continue;

                if (current.Length == 0)
                {
                    current.Append(word);
                }
                else if (current.Length + 1 + word.Length <= LineWidth)
                {
                    current.Append(' ');
                    current.Append(word);
                }
                else
                {
                    lines.Add(Protect(current.ToString()));
                    current = new StringBuilder();
                    current.Append(word);
                }
            }

            if (current.Length > 0)
                lines.Add(Protect(current.ToString()));

            return lines;
        }

        /// <summary>
        /// Protect a text line from being parsed as a roff control line
        /// </summary>
        /// <param name="line">Line to protect</param>
        /// <returns>The line, prefixed with a zero-width escape when needed</returns>
        private static string Protect(string line)
        {
            // A line beginning with a control character would be treated as a
            // request; a leading zero-width escape forces it to be text
            if (line.Length > 0 && (line[0] == '.' || line[0] == '\''))
                return "\\&" + line;

            return line;
        }
    }
}
