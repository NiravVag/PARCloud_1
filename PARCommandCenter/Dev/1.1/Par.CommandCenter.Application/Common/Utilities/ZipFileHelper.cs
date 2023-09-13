using Par.CommandCenter.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Par.CommandCenter.Application.Common.Utilities
{
    public class ZipFileHelper
    {
        protected ZipFileHelper()
        {
        }

        public static CcFile ArchiveFileList(List<CcFile> byteArrayList)
        {
            using MemoryStream ms = new MemoryStream();

            using (ZipArchive archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                foreach (var file in byteArrayList)
                {
                    ZipArchiveEntry entry = archive.CreateEntry(file.FileName + "." + file.FileExtension, CompressionLevel.Fastest);

                    using Stream zipStream = entry.Open();
                    zipStream.Write(file.Content, 0, file.Content.Length);
                }
            }

            return new CcFile(ms.ToArray(), "Archive", "application/zip", "zip");
        }
    }   
}
