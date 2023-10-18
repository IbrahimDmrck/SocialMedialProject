using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Helpers.FileHelper
{
    public class FileCategory
    {
        public string FolderName { get; set; }
        public Dictionary<string, string> ExtensionMimeType { get; set; }
        public double MaximumUploadSizeInByte { get; set; }
    }
}
