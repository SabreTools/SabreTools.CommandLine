using System.Text;

namespace SabreTools.CommandLine.Inputs
{
    /// <summary>
    /// Represents a string input with a single instance allowed
    /// </summary>
    public class StringInput : UserInput<string?>
    {
        #region Constructors

        public StringInput(string name, string flag, string description, string? detailedDescription = null)
            : base(name, flag, description, detailedDescription)
        {
            Value = null;
        }

        public StringInput(string name, string[] flags, string description, string? detailedDescription = null)
            : base(name, flags, description, detailedDescription)
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

            /// Get the current part
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

                Value = val;
                return true;
            }

            // Check for space-separated
            else
            {
                // Ensure the value exists
                if (index + 1 >= args.Length)
                    return false;

                index++;
                Value = args[index];
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
