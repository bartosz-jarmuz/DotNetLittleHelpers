namespace DotNetLittleHelpers
{
    /// <summary>
    /// Class Filter.
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the comparison.
        /// </summary>
        /// <value>The comparison.</value>
        public ComparisonRule Comparison { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value { get; set; }
    }
}