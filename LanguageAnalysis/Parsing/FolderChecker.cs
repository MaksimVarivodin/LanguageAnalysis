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
        public static bool IsValidFolder(string folderName)
        {
            return FolderExists(folderName) && FolderIsReadable(folderName);
        }
    }
}
