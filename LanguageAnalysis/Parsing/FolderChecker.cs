using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderWork
{
    static class FolderChecker
    {
        /// <summary>
        /// Checks if the provided folder name is valid.
        /// A valid folder name is not null, not whitespace, does not contain invalid path characters, and is not longer than 255 characters.
        /// </summary>
        /// <param name="folderName">The folder name to validate.</param>
        /// <returns>True if the folder name is valid; otherwise, false.</returns>
        private static bool FolderNameIsValid(string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
                return false;

            char[] invalidChars = System.IO.Path.GetInvalidPathChars();
            if (folderName.IndexOfAny(invalidChars) >= 0)
                return false;

            if (folderName.Length > 255)
                return false;

            return true;
        }

        /// <summary>
        /// Checks if the folder exists on the file system and has a valid name.
        /// </summary>
        /// <param name="folderName">The folder path to check.</param>
        /// <returns>True if the folder exists and the name is valid; otherwise, false.</returns>
        private static bool FolderExists(string folderName)
        {
            if (!FolderNameIsValid(folderName))
                return false;
            try
            {
                return System.IO.Directory.Exists(folderName);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the folder is readable (i.e., the application has permission to read its contents).
        /// </summary>
        /// <param name="folderName">The folder path to check.</param>
        /// <returns>True if the folder is readable; otherwise, false.</returns>
        private static bool FolderIsReadable(string folderName)
        {
            if (!FolderNameIsValid(folderName))
                return false;
            bool isReadable = false;
            try
            {
                // Trying to read files to check permissions
                Directory.GetFiles(folderName);
                isReadable = true;
            }
            catch (UnauthorizedAccessException) { }
            return isReadable;
        }

        /// <summary>
        /// Determines whether the specified folder exists, has a valid name, and is readable.
        /// </summary>
        /// <param name="folderName">The folder path to validate.</param>
        /// <returns>True if the folder exists, is valid, and is readable; otherwise, false.</returns>
        public static bool IsValidFolder(string folderName)
        {
            return FolderExists(folderName) && FolderIsReadable(folderName);
        }
    }
}
