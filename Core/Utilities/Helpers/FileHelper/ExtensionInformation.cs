using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Helpers.FileHelper
{
    public class ExtensionInformation
    {
        public string FolderName { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }
        public double MaximumUploadSizeInByte { get; set; }
    }
}
