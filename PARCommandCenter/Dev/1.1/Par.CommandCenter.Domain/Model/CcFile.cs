using System;
using System.Collections.Generic;
using System.Text;

namespace Par.CommandCenter.Domain.Model
{
    public class CcFile
    {
        public string FileName { get; set; }

        public string MimeType { get; set; }

       

        public string FileExtension { get; set; }

        public byte[] Content { get; set; }

        public CcFile(byte[] content, string fileName, string mimeType, string fileExtension)
        {
            this.Content = content;
            this.FileName = fileName;
            this.MimeType = mimeType;
            this.FileExtension = fileExtension;
        }
    }
}
