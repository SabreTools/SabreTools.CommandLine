using System.Text;

namespace SabreTools.CommandLine.Inputs
{
    /// <summary>
    /// Represents a user input bounded to the range of <see cref="long"/> 
    /// </summary>
    public class Int64Input : UserInput<long?>
    {
        #region Constructors

        public Int64Input(string name, string flag, string description, string? detailed = null)
            : base(name, flag, description, detailed)
        {
            Value = null;
        }

        public Int64Input(string name, string[] flags, string description, string? detailed = null)
            : base(name, flags, description, detailed)
        {
            Value = null;
        }

        #endregion

        #region Instance Methods

        /// <inheritdoc/>
        public override bool ProcessInput(string[] args, ref int index)
        {
            // If the index is invalid
            if (index < 0 || index >= args.Length)
                return false;

            // Get the current part
            string part = args[index];

            // If the current flag doesn't match, check to see if any of the subfeatures are valid
            if (!ContainsFlag(part))
            {
                foreach (var kvp in Children)
                {
                    if (kvp.Value.ProcessInput(args, ref index))
                        return true;
                }

                return false;
            }

            // Check for equal separated
            if (part.Contains("="))
            {
                // Split the string, using the first equal sign as the separator
                string[] tempSplit = part.Split('=');
                string val = string.Join("=", tempSplit, 1, tempSplit.Length - 1);

                // Ensure the value exists
                if (string.IsNullOrEmpty(val))
                    return false;

                // If the next value is valid
                if (!long.TryParse(val, out long value))
                    return false;

                Value = value;
                return true;
            }

            // Check for space-separated
            else
            {
                // Ensure the value exists
                if (index + 1 >= args.Length)
                    return false;

                // If the next value is valid
                if (!long.TryParse(args[index + 1], out long value))
                    return false;

                index++;
                Value = value;
                return true;
            }
        }

        /// <inheritdoc/>
        protected override string FormatFlags()
        {
            var sb = new StringBuilder();
            Flags.ForEach(flag => sb.Append($"{flag}=, "));
            return sb.ToString().TrimEnd(' ', ',');
        }

        #endregion
    }
}
