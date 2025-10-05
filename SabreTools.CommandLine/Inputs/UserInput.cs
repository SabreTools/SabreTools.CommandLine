using System;
using System.Collections.Generic;
using System.Text;

namespace SabreTools.CommandLine.Inputs
{
    /// <summary>
    /// Represents a single user input which may contain children
    /// </summary>
    public abstract class UserInput
    {
        #region Properties

        /// <summary>
        /// Display name for the feature
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Set of children associated with this input
        /// </summary>
        public readonly Dictionary<string, UserInput> Children = [];

        #endregion

        #region Fields

        /// <summary>
        /// Set of flags associated with the feature
        /// </summary>
        protected readonly List<string> Flags = [];

        /// <summary>
        /// Short description of the feature
        /// </summary>
        private readonly string _description;

        /// <summary>
        /// Optional long description of the feature
        /// </summary>
        private readonly string? _longDescription;

        #endregion

        #region Constructors

        internal UserInput(string name, string flag, string description, string? longDescription = null)
        {
            Name = name;
            Flags.Add(flag);
            _description = description;
            _longDescription = longDescription;
        }

        internal UserInput(string name, string[] flags, string description, string? longDescription = null)
        {
            Name = name;
            Flags.AddRange(flags);
            _description = description;
            _longDescription = longDescription;
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Directly address a given subfeature
        /// </summary>
        public UserInput? this[string name]
        {
            get { return Children.ContainsKey(name) ? Children[name] : null; }
        }

        /// <summary>
        /// Directly address a given subfeature
        /// </summary>
        public UserInput? this[UserInput subfeature]
        {
            get { return Children.ContainsKey(subfeature.Name) ? Children[subfeature.Name] : null; }
        }

        /// <summary>
        /// Add a new child input
        /// </summary>
        public void Add(UserInput input)
            => Children[input.Name] = input;

        /// <summary>
        /// Returns if a flag exists for the current feature
        /// </summary>
        /// <param name="name">Name of the flag to check</param>
        /// <returns>True if the flag was found, false otherwise</returns>
        public bool ContainsFlag(string name)
            => Flags.Exists(f => f == name || name.StartsWith($"{f}="));

        /// <summary>
        /// Returns if the feature contains a flag that starts with the given character
        /// </summary>
        /// <param name="c">Character to check against</param>
        /// <returns>True if the flag was found, false otherwise</returns>
        public bool StartsWith(char c)
            => Flags.Exists(f => f.TrimStart('-', '/', '\\').ToLowerInvariant()[0] == c);

        #endregion

        #region Children

        /// <summary>
        /// Get a boolean value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>The value if found, the default value otherwise</returns>
        public bool GetBoolean(string key, bool defaultValue = false)
        {
            if (TryGetBoolean(key, out bool value, defaultValue))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Get an Int8 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>The value if found, the default value otherwise</returns>
        public sbyte GetInt8(string key, sbyte defaultValue = sbyte.MinValue)
        {
            if (TryGetInt8(key, out sbyte value, defaultValue))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Get an Int16 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>The value if found, the default value otherwise</returns>
        public short GetInt16(string key, short defaultValue = short.MinValue)
        {
            if (TryGetInt16(key, out short value, defaultValue))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Get an Int32 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>The value if found, the default value otherwise</returns>
        public int GetInt32(string key, int defaultValue = int.MinValue)
        {
            if (TryGetInt32(key, out int value, defaultValue))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Get an Int64 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>The value if found, the default value otherwise</returns>
        public long GetInt64(string key, long defaultValue = long.MinValue)
        {
            if (TryGetInt64(key, out long value, defaultValue))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Get a string value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>The value if found, the default value otherwise</returns>
        public string? GetString(string key, string? defaultValue = null)
        {
            if (TryGetString(key, out string? value, defaultValue))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Get a string list value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>The value if found, the default value otherwise</returns>
        public List<string> GetStringList(string key)
        {
            if (TryGetStringList(key, out var value))
                return value;

            return [];
        }

        /// <summary>
        /// Get a UInt8 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>The value if found, the default value otherwise</returns>
        public byte GetUInt8(string key, byte defaultValue = byte.MinValue)
        {
            if (TryGetUInt8(key, out byte value, defaultValue))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Get a UInt16 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>The value if found, the default value otherwise</returns>
        public ushort GetUInt16(string key, ushort defaultValue = ushort.MinValue)
        {
            if (TryGetUInt16(key, out ushort value, defaultValue))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Get a UInt32 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>The value if found, the default value otherwise</returns>
        public uint GetUInt32(string key, uint defaultValue = uint.MinValue)
        {
            if (TryGetUInt32(key, out uint value, defaultValue))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Get a UInt64 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>The value if found, the default value otherwise</returns>
        public ulong GetUInt64(string key, ulong defaultValue = ulong.MinValue)
        {
            if (TryGetUInt64(key, out ulong value, defaultValue))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Get a boolean value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="value">Value that was found, default value otherwise</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetBoolean(string key, out bool value, bool defaultValue = false)
        {
            // Try to check immediate children
            if (Children.TryGetValue(key, out var input))
            {
                if (input is BooleanInput b)
                {
                    value = b.Value ?? defaultValue;
                    return true;
                }
                else if (input is FlagInput f)
                {
                    value = f.Value;
                    return true;
                }

                throw new ArgumentException("Feature is not a bool");
            }

            // Check all children recursively
            foreach (var child in Children.Values)
            {
                if (child.TryGetBoolean(key, out value, defaultValue))
                    return true;
            }

            value = defaultValue;
            return false;
        }

        /// <summary>
        /// Get an Int8 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="value">Value that was found, default value otherwise</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetInt8(string key, out sbyte value, sbyte defaultValue = sbyte.MinValue)
        {
            // Try to check immediate children
            if (Children.TryGetValue(key, out var input))
            {
                if (input is not Int8Input i)
                    throw new ArgumentException("Feature is not an sbyte");

                value = i.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in Children.Values)
            {
                if (child.TryGetInt8(key, out value, defaultValue))
                    return true;
            }

            value = defaultValue;
            return false;
        }

        /// <summary>
        /// Get an Int16 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="value">Value that was found, default value otherwise</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetInt16(string key, out short value, short defaultValue = short.MinValue)
        {
            // Try to check immediate children
            if (Children.TryGetValue(key, out var input))
            {
                if (input is not Int16Input i)
                    throw new ArgumentException("Feature is not a short");

                value = i.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in Children.Values)
            {
                if (child.TryGetInt16(key, out value, defaultValue))
                    return true;
            }

            value = defaultValue;
            return false;
        }

        /// <summary>
        /// Get an Int32 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="value">Value that was found, default value otherwise</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetInt32(string key, out int value, int defaultValue = int.MinValue)
        {
            // Try to check immediate children
            if (Children.TryGetValue(key, out var input))
            {
                if (input is not Int32Input i)
                    throw new ArgumentException("Feature is not an int");

                value = i.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in Children.Values)
            {
                if (child.TryGetInt32(key, out value, defaultValue))
                    return true;
            }

            value = defaultValue;
            return false;
        }

        /// <summary>
        /// Get an Int64 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="value">Value that was found, default value otherwise</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetInt64(string key, out long value, long defaultValue = long.MinValue)
        {
            // Try to check immediate children
            if (Children.TryGetValue(key, out var input))
            {
                if (input is not Int64Input l)
                    throw new ArgumentException("Feature is not a long");

                value = l.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in Children.Values)
            {
                if (child.TryGetInt64(key, out value, defaultValue))
                    return true;
            }

            value = defaultValue;
            return false;
        }

        /// <summary>
        /// Get a string value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="value">Value that was found, default value otherwise</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetString(string key, out string? value, string? defaultValue = null)
        {
            // Try to check immediate children
            if (Children.TryGetValue(key, out var input))
            {
                if (input is not StringInput s)
                    throw new ArgumentException("Feature is not a string");

                value = s.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in Children.Values)
            {
                if (child.TryGetString(key, out value, defaultValue))
                    return true;
            }

            value = defaultValue;
            return false;
        }

        /// <summary>
        /// Get a string list value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="value">Value that was found, default value otherwise</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetStringList(string key, out List<string> value)
        {
            // Try to check immediate children
            if (Children.TryGetValue(key, out var input))
            {
                if (input is not StringListInput l)
                    throw new ArgumentException("Feature is not a list");

                value = l.Value ?? [];
                return true;
            }

            // Check all children recursively
            foreach (var child in Children.Values)
            {
                if (child.TryGetStringList(key, out value))
                    return true;
            }

            value = [];
            return false;
        }

        /// <summary>
        /// Get a UInt8 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="value">Value that was found, default value otherwise</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetUInt8(string key, out byte value, byte defaultValue = byte.MinValue)
        {
            // Try to check immediate children
            if (Children.TryGetValue(key, out var input))
            {
                if (input is not UInt8Input i)
                    throw new ArgumentException("Feature is not an byte");

                value = i.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in Children.Values)
            {
                if (child.TryGetUInt8(key, out value, defaultValue))
                    return true;
            }

            value = defaultValue;
            return false;
        }

        /// <summary>
        /// Get a UInt16 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="value">Value that was found, default value otherwise</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetUInt16(string key, out ushort value, ushort defaultValue = ushort.MinValue)
        {
            // Try to check immediate children
            if (Children.TryGetValue(key, out var input))
            {
                if (input is not UInt16Input i)
                    throw new ArgumentException("Feature is not a ushort");

                value = i.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in Children.Values)
            {
                if (child.TryGetUInt16(key, out value, defaultValue))
                    return true;
            }

            value = defaultValue;
            return false;
        }

        /// <summary>
        /// Get a UInt32 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="value">Value that was found, default value otherwise</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetUInt32(string key, out uint value, uint defaultValue = uint.MinValue)
        {
            // Try to check immediate children
            if (Children.TryGetValue(key, out var input))
            {
                if (input is not UInt32Input i)
                    throw new ArgumentException("Feature is not an uint");

                value = i.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in Children.Values)
            {
                if (child.TryGetUInt32(key, out value, defaultValue))
                    return true;
            }

            value = defaultValue;
            return false;
        }

        /// <summary>
        /// Get a UInt64 value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="value">Value that was found, default value otherwise</param>
        /// <param name="defaultValue">Optional default value if not found</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetUInt64(string key, out ulong value, ulong defaultValue = ulong.MinValue)
        {
            // Try to check immediate children
            if (Children.TryGetValue(key, out var input))
            {
                if (input is not UInt64Input l)
                    throw new ArgumentException("Feature is not a ulong");

                value = l.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in Children.Values)
            {
                if (child.TryGetUInt64(key, out value, defaultValue))
                    return true;
            }

            value = defaultValue;
            return false;
        }

        #endregion

        #region Processing

        /// <summary>
        /// Validate whether a flag is valid for this feature or not
        /// </summary>
        /// <param name="args">Set of arguments to parse</param>
        /// <param name="index">Reference index into the argument set</param>
        /// <returns>True if the flag was valid, false otherwise</returns>
        public abstract bool ProcessInput(string[] args, ref int index);

        #endregion

        #region Help Output

        /// <summary>
        /// Create formatted help text
        /// </summary>
        /// <param name="detailed">True if the long description should be formatted and output, false otherwise</param>
        /// <returns>Help text formatted as a list of strings</returns>
        public List<string> Format(bool detailed = false)
            => Format(pre: 0, midpoint: 0, detailed);

        /// <summary>
        /// Create formatted help text
        /// </summary>
        /// <param name="pre">Positive number representing number of spaces to put in front of the feature</param>
        /// <param name="midpoint">Positive number representing the column where the description should start</param>
        /// <param name="detailed">True if the long description should be formatted and output, false otherwise</param>
        /// <returns>Help text formatted as a list of strings</returns>
        public List<string> Format(int pre, int midpoint, bool detailed = false)
        {
            // Create the output list
            List<string> outputList = [];

            // Add the standard line
            outputList.Add(FormatStandard(pre, midpoint));

            // Add the long description, if needed
            if (detailed)
                outputList.AddRange(FormatLongDescription(pre, midpoint));

            return outputList;
        }

        /// <summary>
        /// Create formatted help text including all children
        /// </summary>
        /// <param name="detailed">True if the long description should be formatted and output, false otherwise</param>
        /// <returns>Help text formatted as a list of strings</returns>
        public List<string> FormatRecursive(bool detailed = false)
            => FormatRecursive(tabLevel: 0, pre: 0, midpoint: 0, detailed);

        /// <summary>
        /// Create formatted help text including all children
        /// </summary>
        /// <param name="pre">Positive number representing number of spaces to put in front of the feature</param>
        /// <param name="midpoint">Positive number representing the column where the description should start</param>
        /// <param name="detailed">True if the long description should be formatted and output, false otherwise</param>
        /// <returns>Help text formatted as a list of strings</returns>
        public List<string> FormatRecursive(int pre, int midpoint, bool detailed = false)
            => FormatRecursive(tabLevel: 0, pre, midpoint, detailed);

        /// <summary>
        /// Pre-format the flags for output
        /// </summary>
        protected abstract string FormatFlags();

        /// <summary>
        /// Create a padding space based on the given length
        /// </summary>
        /// <param name="spaces">Number of padding spaces to add</param>
        /// <returns>String with requested number of blank spaces</returns>
        private static string CreatePadding(int spaces) => spaces > 0
            ? string.Empty.PadRight(spaces)
            : string.Empty;

        /// <summary>
        /// Format the standard help output line
        /// </summary>
        /// <param name="pre">Positive number representing number of spaces to put in front of the feature</param>
        /// <param name="midpoint">Positive number representing the column where the description should start</param>
        /// <returns>Formatted output string</returns>
        private string FormatStandard(int pre, int midpoint)
        {
            var output = new StringBuilder();

            // Determine the midpoint padding size
            int midpointPadding = midpoint > 0 && output.Length < midpoint
                ? midpoint - output.Length
                : 1;

            output.Append(CreatePadding(pre));
            output.Append(FormatFlags());
            output.Append(CreatePadding(midpointPadding));
            output.Append(_description);

            return output.ToString();
        }

        /// <summary>
        /// Format the long description help output lines
        /// </summary>
        /// <param name="pre">Positive number representing number of spaces to put in front of the feature</param>
        /// <param name="midpoint">Positive number representing the column where the description should start</param>
        /// <returns>Pre-split output lines</returns>
        private List<string> FormatLongDescription(int pre, int midpoint)
        {
            // If the long description is null or empty
            if (string.IsNullOrEmpty(_longDescription))
                return [];

            // Normalize the description for output
            string longDescription = _longDescription!.Replace("\r\n", "\n");

            // Get the width of the console for wrapping reference
            int width = (Console.WindowWidth == 0 ? 80 : Console.WindowWidth) - 1;

            // Prepare the outputs
            List<string> outputList = [];
            var output = new StringBuilder();
            output.Append(CreatePadding(pre + 4));

            // Now split the input description and start processing
            string[]? split = longDescription.Split(' ');
            for (int i = 0; i < split.Length; i++)
            {
                // Cache the current segment
                string segment = split[i];

                // If we have a newline character, reset the line and continue
                if (segment.Contains("\n"))
                {
                    string[] subsplit = segment.Split('\n');
                    for (int j = 0; j < subsplit.Length - 1; j++)
                    {
                        // Cache the current segment
                        string subSegment = subsplit[j];

                        // Add the next word only if the total length doesn't go above the width of the screen
                        if (output.Length + subSegment.Length < width)
                        {
                            output.Append((output.Length == pre + 4 ? string.Empty : " ") + subSegment);
                        }
                        // Otherwise, we want to cache the line to output and create a new blank string
                        else
                        {
                            outputList.Add(output.ToString());
#if NET20 || NET35
                            output = new();
#else
                            output.Clear();
#endif
                            output.Append(CreatePadding(pre + 4));
                            output.Append((output.Length == pre + 4 ? string.Empty : " ") + subSegment);
                        }

                        outputList.Add(output.ToString());
#if NET20 || NET35
                        output = new();
#else
                        output.Clear();
#endif
                        output.Append(CreatePadding(pre + 4));
                    }

                    output.Append(subsplit[subsplit.Length - 1]);
                    continue;
                }

                // Add the next word only if the total length doesn't go above the width of the screen
                if (output.Length + segment.Length < width)
                {
                    output.Append((output.Length == pre + 4 ? string.Empty : " ") + segment);
                }
                // Otherwise, we want to cache the line to output and create a new blank string
                else
                {
                    outputList.Add(output.ToString());
#if NET20 || NET35
                    output = new();
#else
                    output.Clear();
#endif
                    output.Append(CreatePadding(pre + 4));
                    output.Append((output.Length == pre + 4 ? string.Empty : " ") + segment);
                }
            }

            // Add the last created output and a blank line for clarity
            outputList.Add(output.ToString());
            outputList.Add(string.Empty);

            return outputList;
        }

        /// <summary>
        /// Create formatted help text including all children
        /// </summary>
        /// <param name="tabLevel">Level of indentation for this feature</param>
        /// <param name="pre">Positive number representing number of spaces to put in front of the feature</param>
        /// <param name="midpoint">Positive number representing the column where the description should start</param>
        /// <param name="detailed">True if the long description should be formatted and output, false otherwise</param>
        /// <returns>Help text formatted as a list of strings</returns>
        private List<string> FormatRecursive(int tabLevel, int pre = 0, int midpoint = 0, bool detailed = false)
        {
            // Create the output list
            List<string> outputList = [];

            // Normalize based on the tab level
            int preAdjusted = pre;
            int midpointAdjusted = midpoint;
            if (tabLevel > 0)
            {
                preAdjusted += 4 * tabLevel;
                midpointAdjusted += 4 * tabLevel;
            }

            // Add the standard line
            outputList.Add(FormatStandard(preAdjusted, midpointAdjusted));

            // Add the long description, if needed
            if (detailed)
                outputList.AddRange(FormatLongDescription(preAdjusted, midpointAdjusted));

            // Append all children recursively
            foreach (var feature in Children.Values)
            {
                outputList.AddRange(feature.FormatRecursive(tabLevel + 1, pre, midpoint, detailed));
            }

            return outputList;
        }

        #endregion
    }
}
