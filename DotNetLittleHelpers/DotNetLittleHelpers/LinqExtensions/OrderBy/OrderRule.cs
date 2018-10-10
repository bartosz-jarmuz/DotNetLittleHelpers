namespace DotNetLittleHelpers
{
    /// <summary>
    /// Holds data for the orderby clause
    /// </summary>
    public class OrderRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRule"/> class.
        /// </summary>
        public OrderRule()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRule"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        public OrderRule(string propertyName, bool isDescending= false) : this()
        {
            this.Property = propertyName;
            this.Descending = isDescending;
        }

        /// <summary>
        /// Name of the property to sort by
        /// </summary>
        /// <value>The property.</value>
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="OrderRule"/> is descending sort.
        /// </summary>
        /// <value><c>true</c> if descending; otherwise, <c>false</c>.</value>
        public bool Descending { get; set; }
    }
}