namespace SabreTools.CommandLine.Inputs
{
    /// <summary>
    /// Represents a single user input which may contain children
    /// </summary>
    public abstract class UserInput<T> : UserInput
    {
        /// <summary>
        /// Typed value provided by the user
        /// </summary>
        public T? Value { get; protected set; }

        #region Constructors

        public UserInput(string name, string flag, string description, string? detailedDescription = null)
            : base(name, flag, description, detailedDescription)
        {
        }

        public UserInput(string name, string[] flags, string description, string? detailedDescription = null)
            : base(name, flags, description, detailedDescription)
        {
        }

        #endregion

        #region Instance Methods

        /// <inheritdoc/>
        public override abstract bool ProcessInput(string[] args, ref int index);

        /// <inheritdoc/>
        protected override abstract string FormatFlags();

        #endregion
    }
}
