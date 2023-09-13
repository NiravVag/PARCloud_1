namespace Par.CommandCenter.Application.Common.Utilities
{
    using JetBrains.Annotations;
    using System;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    ///     A utility to get property display name.
    /// </summary>
    public class AttributeHelper
    {
        /// <summary>
        /// Retrieve a property display name
        /// </summary>
        /// <param name="type">
        ///     A object's class.
        /// </param>
        /// <param name="propertyName">
        ///     A property name.
        /// </param>
        /// <returns>
        ///     A display name value
        /// </returns>
        public static string GetPropertyDisPlayName(
            [NotNull] Type type,
            [NotNull] string propertyName)
        {
            var prop = type.GetProperty(propertyName);
            var attribute = prop?.GetCustomAttributes(false).FirstOrDefault() as DisplayNameAttribute;
            var name = attribute?.DisplayName;

            return name ?? string.Empty;
        }
    }
}
