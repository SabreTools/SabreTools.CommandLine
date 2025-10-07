using System;
using System.Collections.Generic;
using SabreTools.CommandLine.Features;
using SabreTools.CommandLine.Inputs;

namespace SabreTools.CommandLine
{
    /// <summary>
    /// Represents a logically-grouped set of user inputs
    /// </summary>
    /// <remarks>
    /// It is recommended to use this class as the primary
    /// way to address user inputs from the application. It
    /// is also recommended that all directly-included
    /// inputs are <see cref="Feature"/> unless the implementing
    /// program only has a single utility.
    /// </remarks>
    public class CommandSet
    {
        #region Private variables

        /// <summary>
        /// Preamble used when printing help text to console
        /// </summary>
        private readonly List<string> _header = [];

        /// <summary>
        /// Trailing lines used when printing help text to console
        /// </summary>
        private readonly List<string> _footer = [];

        /// <summary>
        /// Set of all user inputs in this grouping
        /// </summary>
        /// <remarks>
        /// Only the top level inputs need to be defined. All
        /// children will be included by default.
        /// </remarks>
        private readonly Dictionary<string, UserInput> _inputs = [];

        #endregion

        #region Properties

        /// <summary>
        /// Feature that represents the default functionality
        /// for a command set
        /// </summary>
        /// <remarks>
        /// It is recommended to use this in applications that
        /// do not need multiple distinct functional modes.
        /// 
        /// When printing help text, the flags and description
        /// of this feature will be omitted, instead printing
        /// the children of the feature directly at the
        /// top level.
        /// 
        /// If the default feature is included as a normal
        /// top-level input for the command set, then the flags
        /// will be printed twice - once under the feature
        /// itself and once at the top level.
        /// </remarks>
        public Feature? DefaultFeature { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new CommandSet with no printable header
        /// </summary>
        public CommandSet() { }

        /// <summary>
        /// Create a new CommandSet with a printable header
        /// </summary>
        /// <param name="header">Custom commandline header to be printed when outputting help</param>
        public CommandSet(string header)
        {
            _header.Add(header);
        }

        /// <summary>
        /// Create a new CommandSet with a printable header
        /// </summary>
        /// <param name="header">Custom commandline header to be printed when outputting help</param>
        public CommandSet(List<string> header)
        {
            _header.AddRange(header);
        }

        /// <summary>
        /// Create a new CommandSet with a printable header
        /// </summary>
        /// <param name="header">Custom commandline header to be printed when outputting help</param>
        /// <param name="footer">Custom commandline footer to be printed when outputting help</param>
        public CommandSet(string header, string footer)
        {
            _header.Add(header);
            _footer.Add(footer);
        }

        /// <summary>
        /// Create a new CommandSet with a printable header
        /// </summary>
        /// <param name="header">Custom commandline header to be printed when outputting help</param>
        /// <param name="footer">Custom commandline footer to be printed when outputting help</param>
        public CommandSet(List<string> header, List<string> footer)
        {
            _header.AddRange(header);
            _footer.AddRange(footer);
        }

        #endregion

        #region Accessors

        public UserInput? this[string name]
        {
            get
            {
                if (!_inputs.TryGetValue(name, out var input))
                    return null;

                return input;
            }
        }

        public UserInput? this[UserInput subfeature]
        {
            get
            {
                if (!_inputs.TryGetValue(subfeature.Name, out var input))
                    return null;

                return input;
            }
        }

        /// <summary>
        /// Add a new input to the set
        /// </summary>
        /// <param name="input">UserInput object to map to</param>
        public void Add(UserInput input)
            => _inputs.Add(input.Name, input);

        /// <summary>
        /// Add all children from an input to the set
        /// </summary>
        /// <param name="input">UserInput object to retrieve children from</param>
        /// <remarks>
        /// This should only be used in situations where an input is defined
        /// but not used within the context of the command set directly.
        /// 
        /// This is helpful for when there are applications with default functionality
        /// that need to be able to expose both defined features as well as
        /// the default functionality in help text.
        /// 
        /// If there is any overlap between existing names and the names from
        /// any of the children, this operation will overwrite them.
        /// </reamrks>
        public void AddFrom(UserInput input)
        {
            foreach (var kvp in input.Children)
            {
                _inputs.Add(kvp.Key, kvp.Value);
            }
        }

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
        /// Get a Feature value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <returns>The value if found, null otherwise</returns>
        public Feature? GetFeature(string key)
        {
            if (TryGetFeature(key, out Feature? value))
                return value;

            return null;
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
            if (_inputs.TryGetValue(key, out var input))
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

                throw new ArgumentException("Input is not a bool");
            }

            // Check all children recursively
            foreach (var child in _inputs.Values)
            {
                if (child.TryGetBoolean(key, out value, defaultValue))
                    return true;
            }

            value = defaultValue;
            return false;
        }

        /// <summary>
        /// Get a Feature value from a named input
        /// </summary>
        /// <param name="key">Input name to retrieve, if possible</param>
        /// <param name="value">Value that was found, default value otherwise</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetFeature(string key, out Feature? value)
        {
            // Try to check immediate children
            if (_inputs.TryGetValue(key, out var input))
            {
                if (input is not Feature i)
                    throw new ArgumentException("Input is not a Feature");

                value = i;
                return true;
            }

            // Check all children recursively
            foreach (var child in _inputs.Values)
            {
                if (child.TryGetFeature(key, out value))
                    return true;
            }

            value = null;
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
            if (_inputs.TryGetValue(key, out var input))
            {
                if (input is not Int8Input i)
                    throw new ArgumentException("Input is not an sbyte");

                value = i.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in _inputs.Values)
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
            if (_inputs.TryGetValue(key, out var input))
            {
                if (input is not Int16Input i)
                    throw new ArgumentException("Input is not a short");

                value = i.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in _inputs.Values)
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
            if (_inputs.TryGetValue(key, out var input))
            {
                if (input is not Int32Input i)
                    throw new ArgumentException("Input is not an int");

                value = i.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in _inputs.Values)
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
            if (_inputs.TryGetValue(key, out var input))
            {
                if (input is not Int64Input l)
                    throw new ArgumentException("Input is not a long");

                value = l.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in _inputs.Values)
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
            if (_inputs.TryGetValue(key, out var input))
            {
                if (input is not StringInput s)
                    throw new ArgumentException("Input is not a string");

                value = s.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in _inputs.Values)
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
            if (_inputs.TryGetValue(key, out var input))
            {
                if (input is not StringListInput l)
                    throw new ArgumentException("Input is not a list");

                value = l.Value ?? [];
                return true;
            }

            // Check all children recursively
            foreach (var child in _inputs.Values)
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
            if (_inputs.TryGetValue(key, out var input))
            {
                if (input is not UInt8Input i)
                    throw new ArgumentException("Input is not an byte");

                value = i.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in _inputs.Values)
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
            if (_inputs.TryGetValue(key, out var input))
            {
                if (input is not UInt16Input i)
                    throw new ArgumentException("Input is not a ushort");

                value = i.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in _inputs.Values)
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
            if (_inputs.TryGetValue(key, out var input))
            {
                if (input is not UInt32Input i)
                    throw new ArgumentException("Input is not an uint");

                value = i.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in _inputs.Values)
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
            if (_inputs.TryGetValue(key, out var input))
            {
                if (input is not UInt64Input l)
                    throw new ArgumentException("Input is not a ulong");

                value = l.Value ?? defaultValue;
                return true;
            }

            // Check all children recursively
            foreach (var child in _inputs.Values)
            {
                if (child.TryGetUInt64(key, out value, defaultValue))
                    return true;
            }

            value = defaultValue;
            return false;
        }

        #endregion

        #region Inputs

        /// <summary>
        /// Get the input name for a given flag or short name
        /// </summary>
        /// <param name="value">Name or flag value to match</param>
        /// <returns>User input name on success, empty string on error</returns>
        public string GetInputName(string value)
        {
            // Pre-split the input for efficiency
            string[] splitInput = value.Split('=');

            foreach (var key in _inputs.Keys)
            {
                // Validate the name matches
                var feature = _inputs[key];
                if (feature.Name == splitInput[0])
                    return key;

                // Validate the flag is contained
                if (feature.ContainsFlag(splitInput[0]))
                    return key;
            }

            // No feature could be found
            return string.Empty;
        }

        /// <summary>
        /// Gets the top-level user input associated with a given name or flag
        /// </summary>
        /// <param name="value">Name or flag value to match</param>
        /// <returns>User input associated with the name or flag, if possible</returns>
        public UserInput? GetTopLevel(string value)
        {
            // Validate the value matches something
            string inputName = GetInputName(value);
            if (inputName.Length == 0)
                return null;

            // Try to get the child based on the name
            if (!_inputs.TryGetValue(inputName, out var input))
                return null;

            return input;
        }

        /// <summary>
        /// Check if a value is a direct child input
        /// </summary>
        /// <param name="value">Name or flag value to match</param>
        /// <returns>True if the feature was found, false otherwise</returns>
        public bool IsTopLevel(string value)
            => GetInputName(value).Length > 0;

        #endregion

        #region Help Output

        /// <summary>
        /// Output top-level features only
        /// </summary>
        /// <param name="detailed">True if the detailed descriptions should be formatted and output, false otherwise</param>
        public void OutputGenericHelp(bool detailed = false)
        {
            // Start building the output list
            List<string> output = [];

            // Append the header, if needed
            if (_header.Count > 0)
                output.AddRange(_header);

            // Now append all available top-level flags
            output.Add("Available options:");
            foreach (var input in _inputs.Values)
            {
                var outputs = input.Format(pre: 2, midpoint: 30, detailed);
                if (outputs != null)
                    output.AddRange(outputs);
            }

            // If there is a default feature
            if (DefaultFeature != null)
            {
                foreach (var input in DefaultFeature.Children)
                {
                    var outputs = input.Value.Format(pre: 2, midpoint: 30, detailed);
                    if (outputs != null)
                        output.AddRange(outputs);
                }
            }

            // Append the footer, if needed
            if (_footer.Count > 0)
                output.AddRange(_footer);

            // Now write out everything in a staged manner
            WriteOutWithPauses(output);
        }

        /// <summary>
        /// Output all features recursively
        /// </summary>
        /// <param name="detailed">True if the detailed descriptions should be formatted and output, false otherwise</param>
        public void OutputAllHelp(bool detailed = false)
        {
            // Start building the output list
            List<string> output = [];

            // Append the header first, if needed
            if (_header.Count > 0)
                output.AddRange(_header);

            // Now append all available flags recursively
            output.Add("Available options:");
            foreach (var input in _inputs.Values)
            {
                var outputs = input.FormatRecursive(pre: 2, midpoint: 30, detailed);
                if (outputs != null)
                    output.AddRange(outputs);
            }

            // If there is a default feature
            if (DefaultFeature != null)
            {
                foreach (var input in DefaultFeature.Children)
                {
                    var outputs = input.Value.Format(pre: 2, midpoint: 30, detailed);
                    if (outputs != null)
                        output.AddRange(outputs);
                }
            }

            // Append the footer, if needed
            if (_footer.Count > 0)
                output.AddRange(_footer);

            // Now write out everything in a staged manner
            WriteOutWithPauses(output);
        }

        /// <summary>
        /// Output a single feature recursively
        /// </summary>
        /// <param name="featureName">Name of the feature to output information for, if possible</param>
        /// <param name="detailed">True if the detailed description should be formatted and output, false otherwise</param>
        public void OutputFeatureHelp(string? featureName, bool detailed = false)
        {
            // If the feature name is null, empty, or just consisting of leading characters
            string trimmedName = featureName?.TrimStart('-', '/', '\\') ?? string.Empty;
            if (trimmedName.Length == 0)
            {
                OutputGenericHelp();
                return;
            }

            // If a top-level input is found
            if (IsTopLevel(featureName!))
            {
                // Retrieve the input
                featureName = GetInputName(featureName!);
                var input = _inputs[featureName];

                // Append the formatted text
                List<string> output = [];
                output.Add($"Available options for {featureName}:");
                output.AddRange(input.FormatRecursive(pre: 2, midpoint: 30, detailed));

                // Now write out everything in a staged manner
                WriteOutWithPauses(output);
            }

            // Find all partial matches
            List<string> startsWith = [];
            foreach (var kvp in _inputs)
            {
                if (kvp.Value.StartsWith(trimmedName[0]))
                    startsWith.Add(kvp.Key);
            }

            // If there are possible matches, append them
            if (startsWith.Count > 0)
            {
                List<string> output = [];
                output.Add($"\"{featureName}\" not found. Did you mean:");
                foreach (string possible in startsWith)
                {
                    output.AddRange(_inputs[possible].Format(pre: 2, midpoint: 30, detailed));
                }

                // Now write out everything in a staged manner
                WriteOutWithPauses(output);
            }
        }

        /// <summary>
        /// Pause on console output
        /// </summary>
        private static void Pause()
        {
#if NET452_OR_GREATER || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
            if (!Console.IsOutputRedirected)
#endif
            {
                Console.WriteLine();
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Write out the help text with pauses, if needed
        /// </summary>
        private static void WriteOutWithPauses(List<string> helpText)
        {
            // Now output based on the size of the screen
            int i = 0;
            for (int line = 0; line < helpText.Count; line++)
            {
                string help = helpText[line];

                Console.WriteLine(help);
                i++;

                // If we're not being redirected and we reached the size of the screen, pause
                if (i == Console.WindowHeight - 3 && line != helpText.Count - 1)
                {
                    i = 0;
                    Pause();
                }
            }

            Pause();
        }

        #endregion

        #region Processing

        /// <summary>
        /// Process args list with default handling
        /// </summary>
        /// <param name="args">Set of arguments to process</param>
        /// <returns>True if all arguments were processed correctly, false otherwise</returns>
        /// <remarks>
        /// This default processing implementation assumes a few key points:
        /// - Top-level items are all <see cref="Feature"/>
        /// - There is only top-level item allowed at a time
        /// - The first argument is always the <see cref="Feature"/> flag
        /// </remarks>
        public bool ProcessArgs(string[] args)
        {
            // If there's no arguments, show help
            if (args.Length == 0)
            {
                OutputAllHelp();
                return true;
            }

            // Get the first argument as a feature flag
            string featureName = args[0];

            // Get the associated feature
            var topLevel = GetTopLevel(featureName);
            if (topLevel == null || topLevel is not Feature feature)
            {
                Console.WriteLine($"'{featureName}' is not valid feature flag");
                OutputFeatureHelp(featureName);
                return false;
            }

            // Handle default help functionality
            if (topLevel is Help helpFeature)
            {
                helpFeature.ProcessArgs(args, 0, this);
                return true;
            }
            else if (topLevel is HelpExtended helpExtFeature)
            {
                helpExtFeature.ProcessArgs(args, 0, this);
                return true;
            }

            // Now verify that all other flags are valid
            if (!feature.ProcessArgs(args, 1))
            {
                OutputFeatureHelp(topLevel.Name);
                return false;
            }

            // If inputs are required
            if (feature.RequiresInputs && !feature.VerifyInputs())
            {
                OutputFeatureHelp(topLevel.Name);
                return false;
            }

            // Now execute the current feature
            return feature.Execute();
        }

        #endregion
    }
}
