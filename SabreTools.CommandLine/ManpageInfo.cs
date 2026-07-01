namespace SabreTools.CommandLine
{
    /// <summary>
    /// Document-level metadata used when generating a man page
    /// </summary>
    /// <remarks>
    /// These values populate the parts of a man page that cannot be
    /// derived from the command-line model itself, such as the
    /// <c>.TH</c> title line and the <c>NAME</c> section.
    /// </remarks>
    public class ManpageInfo
    {
        /// <summary>
        /// Program name as invoked on the command line
        /// </summary>
        /// <remarks>
        /// Used for the <c>.TH</c> title line, the <c>NAME</c> section,
        /// and the <c>SYNOPSIS</c> section.
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// Manual section the page belongs to
        /// </summary>
        /// <remarks>Defaults to section 1 (user commands)</remarks>
        public string Section { get; set; } = "1";

        /// <summary>
        /// Date used for the <c>.TH</c> title line
        /// </summary>
        /// <remarks>
        /// When left null or empty, the field is emitted blank so that a
        /// downstream packager can stamp it at build time.
        /// </remarks>
        public string? Date { get; set; }

        /// <summary>
        /// Version string used for the <c>.TH</c> title line
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// Manual title used for the <c>.TH</c> title line
        /// </summary>
        /// <remarks>Typically a value such as "User Commands"</remarks>
        public string? Title { get; set; }

        /// <summary>
        /// Short description used for the <c>NAME</c> section
        /// </summary>
        /// <remarks>Should be a single printable line</remarks>
        public string? Description { get; set; }

        /// <summary>
        /// Section heading used for the list of inputs
        /// </summary>
        /// <remarks>Defaults to "OPTIONS"</remarks>
        public string OptionsHeading { get; set; } = "OPTIONS";

        /// <summary>
        /// Create a new <see cref="ManpageInfo"/> for the given program name
        /// </summary>
        /// <param name="name">Program name as invoked on the command line</param>
        public ManpageInfo(string name)
        {
            Name = name;
        }
    }
}
