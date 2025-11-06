namespace QIS.Medinfras.Data.Service
{
    using System;
    using System.Globalization;

    /// <summary>
    /// A helper class used to format a person's name. 
    /// </summary>
    public static class PersonNameFormat
    {
        #region Const Values

        /// <summary>
        /// Index of the lastName in an array returned by the FormatNameArray method
        /// </summary>
        public const int lastNameIndex = 0;

        /// <summary>
        /// Index of the firstName in an array returned by the FormatNameArray method. 
        /// </summary>
        public const int firstNameIndex = 1;

        /// <summary>
        /// Index of the middleName in an array returned by the FormatNameArray method. 
        /// </summary>
        public const int middleNameIndex = 2;

        /// <summary>
        /// Index of the Title in an array returned by the FormatNameArray method.
        /// </summary>
        public const int TitleIndex = 3;

        /// <summary>
        /// An ellipsis.
        /// </summary>
        public const string Ellipsis = "...";

        /// <summary>
        /// The maximum number of characters allowed in lastName.
        /// </summary>
        private const int MaxLastNameChars = 25;

        /// <summary>
        /// The maximum number of characters allowed in firstName.
        /// </summary>
        private const int MaxFirstNameChars = 25;

        /// <summary>
        /// The maximum number of characters allowed in firstName.
        /// </summary>
        private const int MaxMiddleNameChars = 25;

        /// <summary>
        /// The maximum number of characters in Title.
        /// </summary>
        private const int MaxTitleChars = 35;

        private const string LastAndFirstNameSeparator = ", ";
        private const string FirstAndMiddleNameSeparator = " ";
        private const string TitleFormat =  " ({0})";

        #endregion

        #region Public Methods
        /// <summary>
        /// Formats the person's name. 
        /// </summary>
        /// <param name="lastName">The person's family or last name. (Last Name)</param>
        /// <param name="firstName">The person's given or first name. (First Name)</param>
        /// <param name="middleName">The person's middle name. (Middle Name)</param>/// 
        /// <param name="title">The person's title. </param>
        /// <returns>The person's name formatted as lastName, firstName middleName (Title), where lastName is capitalized.</returns>
        public static string Format(string lastName, string firstName, string middleName, string title)
        {
            string[] formattedNameParts = FormatNameArray(lastName, firstName, middleName, title);
            string formattedName = string.Concat(formattedNameParts);
            return formattedName;
        }

        /// <summary>
        /// Returns an array containing the formatted name elements. 
        /// </summary>
        /// <param name="lastName">The person's family name.</param>
        /// <param name="firstName">The person given name.</param>
        /// <param name="middleName">The person middle name.</param>/// 
        /// <param name="title">The person's title. </param>
        /// <returns>An array of three formatted strings for lastName, firstName, middleName and Title. </returns>
        public static string[] FormatNameArray(string lastName, string firstName, string middleName, string title)
        {
            string[] returnArray =
                                    {
                                        FormatLastName(lastName),
                                        FormatFirstName(firstName),
                                        FormatMiddleName(middleName),
                                        FormatTitle(title)
                                    };

            // if we have a first name and middle name separate them
            if (returnArray[firstNameIndex].Length > 0 && returnArray[middleNameIndex].Length > 0)
            {
                returnArray[firstNameIndex] += FirstAndMiddleNameSeparator;
            }

            // if we have a first name and last name separate them
            if (returnArray[lastNameIndex].Length > 0 && returnArray[firstNameIndex].Length > 0)
            {
                returnArray[lastNameIndex] += LastAndFirstNameSeparator;
            }            
            else if (returnArray[lastNameIndex].Length == 0 && returnArray[firstNameIndex].Length == 0)
            {
                // if no first or last name don't display title
                returnArray[TitleIndex] = string.Empty;
            }

            return returnArray;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Format the Title.
        /// </summary>
        /// <param name="title">person title.</param>
        /// <returns>Formatted Title.</returns>
        private static string FormatTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return string.Empty;
            }

            return string.Format(CultureInfo.CurrentCulture, TitleFormat, TruncateWithEllipsisIfNeeded(title.Trim(), MaxTitleChars));
        }

        /// <summary>
        /// Format the firstName.
        /// </summary>
        /// <param name="firstName">Person given name.</param>
        /// <returns>Formatted firstName.</returns>
        private static string FormatFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                return string.Empty;
            }

            return TruncateWithEllipsisIfNeeded(firstName.Trim(), MaxFirstNameChars);
        }

        /// <summary>
        /// Format the middleName.
        /// </summary>
        /// <param name="firstName">Person given name.</param>
        /// <returns>Formatted firstName.</returns>
        private static string FormatMiddleName(string middleName)
        {
            if (string.IsNullOrEmpty(middleName))
            {
                return string.Empty;
            }

            return TruncateWithEllipsisIfNeeded(middleName.Trim(), MaxMiddleNameChars);
        }

        /// <summary>
        /// Format the lastName.
        /// </summary>
        /// <param name="lastName">person family name.</param>
        /// <returns>Formatted lastName.</returns>
        private static string FormatLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                return string.Empty;
            }

            return TruncateWithEllipsisIfNeeded(lastName.Trim().ToUpper(CultureInfo.CurrentCulture), MaxLastNameChars);
        }

        /// <summary>
        /// Truncate the supplied value and append ellipsis so that the total
        /// length does not exceed the maximum specified.
        /// </summary>
        /// <param name="value">Value to truncate.</param>
        /// <param name="maxLength">Maximum length.</param>
        /// <returns>Truncated value.</returns>
        private static string TruncateWithEllipsisIfNeeded(string value, int maxLength)
        {
            if (value.Length > maxLength)
            {
                return value.Substring(0, maxLength - Ellipsis.Length) + Ellipsis;
            }

            return value;
        }

        #endregion
    }
}
