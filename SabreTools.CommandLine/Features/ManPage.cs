using System;

namespace SabreTools.CommandLine.Features
{
    /// <summary>
    /// Reference feature that emits a man page for the enclosing command set
    /// </summary>
    /// <remarks>
    /// This mirrors the built-in <see cref="Help"/> feature: it must be given
    /// the enclosing <see cref="CommandSet"/> so that it can render the page
    /// from the same model used to format help text. The roff output is
    /// written to standard output so that it can be redirected to a file at
    /// packaging time, for example <c>tool help-man &gt; tool.1</c>.
    /// </remarks>
    public class ManPage : Feature
    {
        public const string DisplayName = "Man Page";

        private static readonly string[] _defaultFlags = ["man"];

        private const string _description = "Generate a man page";

        private const string _detailedDescription = "Write a roff-formatted man page for this program to standard output.";

        /// <summary>
        /// Document-level metadata for the generated man page
        /// </summary>
        private readonly ManPageInfo _info;

        /// <summary>
        /// Indicates if detailed descriptions should be included
        /// </summary>
        private readonly bool _includeVerbose;

        public ManPage(ManPageInfo info, bool includeVerbose = true)
            : base(DisplayName, _defaultFlags, _description, _detailedDescription)
        {
            _info = info;
            _includeVerbose = includeVerbose;
            RequiresInputs = false;
        }

        public ManPage(ManPageInfo info, string[] flags, bool includeVerbose = true)
            : base(DisplayName, flags, _description, _detailedDescription)
        {
            _info = info;
            _includeVerbose = includeVerbose;
            RequiresInputs = false;
        }

        /// <inheritdoc/>
        public override bool ProcessArgs(string[] args, int index)
            => ProcessArgs(args, index, null);

        /// <inheritdoc cref="ProcessArgs(string[], int)"/>
        /// <param name="parentSet">Reference to the enclosing parent set</param>
        public bool ProcessArgs(string[] args, int index, CommandSet? parentSet)
        {
            // The page can only be rendered with the enclosing set
            if (parentSet is not null)
                Console.Write(parentSet.OutputManPage(_info, _includeVerbose));

            return true;
        }

        /// <inheritdoc/>
        public override bool VerifyInputs() => true;

        /// <inheritdoc/>
        public override bool Execute() => true;
    }
}
