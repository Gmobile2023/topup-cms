using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AspNetZeroCore.Net;
using Abp.Dependency;
using Aspose.Cells;
using HLS.Topup.Dto;
using HLS.Topup.Storage;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Hosting;

namespace HLS.Topup.Files
{
    public class FileCommonAppService : ITransientDependency, IFileCommonAppService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public FileCommonAppService(IWebHostEnvironment hostingEnvironment,
            ITempFileCacheManager tempFileCacheManager)
        {
            _hostingEnvironment = hostingEnvironment;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public FileDto GetFileExcel<T>(List<T> data, string fileSourceName, string sourceName, string outputFileName)
        {
            try
            {
                var lic = new License();
                lic.SetLicense("Aspose_total_20220516.lic");
                var designer = new WorkbookDesigner();
                var path = Path.Combine(_hostingEnvironment.WebRootPath, fileSourceName);
                designer.Workbook = new Workbook(path);
                //var i = 1;
                designer.SetDataSource(sourceName, data.ToList());
                var workbook = designer.Workbook;
                designer.Process(false);
                var file = new FileDto(outputFileName,
                    MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
                using (var memoryStream = new MemoryStream())
                {
                    workbook.Save(memoryStream, SaveFormat.Xlsx);
                    _tempFileCacheManager.SetFile(file.FileToken, memoryStream.ToArray());
                }

                return file;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public byte[] CompressFile(FileDto file)
        {
            using (var outputZipFileStream = new MemoryStream())
            {
                using (var zipStream = new ZipArchive(outputZipFileStream, ZipArchiveMode.Create))
                {
                    var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
                    var entry = zipStream.CreateEntry(file.FileName);

                    using (var originalFileStream = new MemoryStream(fileBytes))
                    using (var zipEntryStream = entry.Open())
                    {
                        originalFileStream.CopyTo(zipEntryStream);
                    } 
                }

                return outputZipFileStream.ToArray();
            }
        }

        public byte[] CompressFiles(List<FileDto> files)
        {
            using (var outputZipFileStream = new MemoryStream())
            {
                using (var zipStream = new ZipArchive(outputZipFileStream, ZipArchiveMode.Create))
                {
                    foreach (var file in files)
                    {
                        var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
                        var entry = zipStream.CreateEntry(file.FileName);

                        using (var originalFileStream = new MemoryStream(fileBytes))
                        using (var zipEntryStream = entry.Open())
                        {
                            originalFileStream.CopyTo(zipEntryStream);
                        }
                    }
                }

                return outputZipFileStream.ToArray();
            }
        }
         
    }
}
